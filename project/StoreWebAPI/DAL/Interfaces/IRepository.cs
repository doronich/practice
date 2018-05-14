using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    //доделать ..Async
    public interface IRepository<TEntity> where TEntity : BaseEntity {

        Task Create(TEntity item);

        Task<TEntity> GetById(int id);

        Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null);

        Task Remove(TEntity item);

        Task Update(TEntity item);
    }
}
