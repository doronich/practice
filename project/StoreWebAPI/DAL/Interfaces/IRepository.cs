using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces {
    public interface IRepository<TEntity> where TEntity : BaseEntity {
        Task InsertAsync(TEntity item);

        Task<TEntity> GetByIdAsync(long id);

        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null);

        Task RemoveAsync(TEntity item);

        Task UpdateAsync(TEntity item);

        Task DeleteAsync(TEntity item);

        Task SaveChangesAsync();
    }
}
