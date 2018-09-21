using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClothingStore.Data.Entities.Order;
using ClothingStore.Repository.Interfaces;
using ClothingStore.Service.Helpers;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.CouponCode;
using ClothingStore.Service.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClothingStore.Service.Services {
    public class CouponCodeService : BaseService<CouponCode>, ICouponService {
        private readonly CodeGenerator<BaseGeneratorSettings> m_generator;

        public CouponCodeService(IRepository<CouponCode> repository, IOptionsSnapshot<BaseGeneratorSettings> options, IHttpContextAccessor accessor) :
            base(accessor, repository) {
            this.m_generator = new CodeGenerator<BaseGeneratorSettings>(options.Get("CodeOptions"));
        }

        public async Task<CouponCode> GetCouponByCodeAsync(string code) {
            if(string.IsNullOrWhiteSpace(code)) return null;

            var coupon = await (await this.Repository.GetAllAsync(new List<Expression<Func<CouponCode, bool>>> { c => c.Code == code })).FirstAsync();

            if(coupon == null) throw new Exception("Incorrect code");
            if(!coupon.Active) throw new Exception("Code is expired.");
            if(coupon.ExpiryDate >= DateTime.UtcNow) return coupon;
            coupon.Active = false;
            await this.Repository.UpdateAsync(coupon);
            throw new Exception("Code is expired.");
        }

        public async Task<bool> CheckCodeAsync(string code) {
            if(string.IsNullOrWhiteSpace(code)) return false;

            var exist = await this.Repository.ExistAsync(o => o.Code == code && o.Active && o.ExpiryDate > DateTime.UtcNow);

            return exist;
        }

        public async Task CreateCouponAsync(CreateCouponCodeDTO model, int amount) {
            var couponList = new List<CouponCode>();
            var code = string.Empty;
            for(var i = 0; i < amount; i++) {
                code = this.m_generator.Generate();

                while(await this.Repository.ExistAsync(x => x.Code == code))
                    code = this.m_generator.Generate();

                var coupon = new CouponCode {
                    Active = true,
                    CreatedBy = this.HttpContext.User.Claims.FirstOrDefault()?.Value,
                    Discount = model.Discount,
                    ExpiryDate = model.ExpiryDate,
                    Code = code
                };

                couponList.Add(coupon);
            }

            await this.Repository.Context.CouponCodes.AddRangeAsync(couponList);
            var res = await this.Repository.Context.SaveChangesAsync();
            if(res <= 0) throw new Exception("Creating coupon error.");
        }

        public async Task UpdateCouponCodeAsync(CouponCode coupon) {
            coupon.UpdatedBy = this.HttpContext.User.Claims.FirstOrDefault()?.Value;
            var c = await this.Repository.GetByIdAsync(coupon.Id);
            if(c == null) throw new Exception("Coupon not found.");

            await this.Repository.UpdateAsync(coupon);
        }

        public async Task RemoveCouponAsync(long id) {
            await this.RemoveAsync(id);
        }

        private async Task<string> GenerateCodeAsync() {
            return await Task.Run(() => this.m_generator.Generate());
        }
    }
}
