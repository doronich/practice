using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Repository.Context;

namespace ClothingStore.Repository.Interfaces {
    public interface IRepository<TEntity> where TEntity : BaseEntity {
        Task CreateAsync(TEntity item);

        Task<TEntity> GetByIdAsync(long id);

        Task<IQueryable<TEntity>> GetAllAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicate = null, string[] includes = null);

        Task UpdateAsync(TEntity item);

        Task DeleteAsync(TEntity item);

        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);

        ApplicationContext m_context { get; set; }
    }
}
