using System;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ClothingStore.Service.Services
{
    public class BaseService<T> where T :BaseEntity{

        public BaseService(IHttpContextAccessor accessor, IRepository<T> repository) {
            this.Repository = repository;
            this.HttpContext = accessor?.HttpContext;
        }

        protected readonly HttpContext HttpContext;
        protected readonly IRepository<T> Repository;

        public virtual async Task<T> GetByIdAsync(long id) {
            var entity = await this.Repository.GetByIdAsync(id);
            if (entity == null) throw new Exception(nameof(entity) + " not found.");
            return entity;
        }

        public virtual async Task RemoveAsync(long id) {
            var entity = await this.Repository.GetByIdAsync(id);
            if(entity==null) throw new Exception(nameof(entity)+" not found.");
            await this.Repository.DeleteAsync(entity);
        }
    }
}
