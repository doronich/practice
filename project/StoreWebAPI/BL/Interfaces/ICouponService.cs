using System.Threading.Tasks;
using ClothingStore.Data.Entities.Order;
using ClothingStore.Service.Models.CouponCode;

namespace ClothingStore.Service.Interfaces {
    public interface ICouponService {
        Task CreateCouponAsync(CreateCouponCodeDTO model, int amount);

        Task<CouponCode> GetCouponByCodeAsync(string code);

        Task UpdateCouponCodeAsync(CouponCode coupon);

        Task<bool> CheckCodeAsync(string code);
    }
}
