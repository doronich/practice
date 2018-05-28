using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

//написать exceptions
namespace DAL.Repository {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity {
        private readonly ApplicationContext m_context;

        private readonly DbSet<TEntity> m_dbSet;
        //private string errorMessage = string.Empty;

        public Repository(ApplicationContext context) {
            this.m_context = context;
            this.m_dbSet = context.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity item) {
            if(item == null) throw new ArgumentNullException(nameof(item));
            await this.m_dbSet.AddAsync(item);
            await this.m_context.SaveChangesAsync();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicate = null) {
            return await Task.Run(() => {
                if(predicate == null) return this.m_dbSet;

                var set = this.m_dbSet.AsQueryable();
                return predicate.Aggregate(set, (current, pr) => current.Where(pr));
            });
        }

        public async Task<TEntity> GetByIdAsync(long id) {
            return await this.m_dbSet.FindAsync(id);
        }

        // ======================
        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate = null) {
            return predicate == null ? await this.m_dbSet.AnyAsync() : await this.m_dbSet.AnyAsync(predicate);
        }

        public async Task UpdateAsync(TEntity item) {
            if(item == null) throw new ArgumentNullException(nameof(item));

            this.m_context.Entry(item).State = EntityState.Modified;
            await this.m_context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity item) {
            if(item == null) throw new ArgumentNullException(nameof(item));

            this.m_context.Remove(item);
            await this.m_context.SaveChangesAsync();
        }
    }
}
