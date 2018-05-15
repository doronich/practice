using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity {
        private readonly ApplicationContext m_context;
        private readonly DbSet<TEntity> m_dbSet;
        //private string errorMessage = string.Empty;

        public Repository(ApplicationContext context) {
            this.m_context = context;
            this.m_dbSet = context.Set<TEntity>();
        }

        public async Task InsertAsync(TEntity item) {
            if(item == null) {
                throw new ArgumentNullException("item");
            }
            await this.m_dbSet.AddAsync(item);
            await this.m_context.SaveChangesAsync();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null) {
            return await Task.Run(() => predicate == null ? this.m_dbSet : this.m_dbSet.Where(predicate));
        }

        public async Task<TEntity> GetByIdAsync(long id) {
            return await this.m_dbSet.FindAsync(id);
        }

        public async Task RemoveAsync(TEntity item) {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            await Task.Run(() => this.m_dbSet.Remove(item));
        }

        public async Task UpdateAsync(TEntity item) {
            if(item == null) {
                throw new ArgumentNullException("item");
            }
            this.m_context.Entry(item).State = EntityState.Modified;
            await this.m_context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity item) {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.m_context.Remove(item);
            await this.m_context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync() {
            await this.m_context.SaveChangesAsync();
        }
    }
}
