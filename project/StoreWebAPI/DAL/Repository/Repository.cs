using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Repository.Context;
using ClothingStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Repository.Repository {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity {
        public ApplicationContext Context { get; set; }

        private readonly DbSet<TEntity> m_dbSet;

        public Repository(ApplicationContext context) {
            this.Context = context;
            this.m_dbSet = context.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity item) {
            if(item == null) throw new ArgumentNullException(nameof(item));
            await this.m_dbSet.AddAsync(item);
            await this.Context.SaveChangesAsync();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicate = null, string[] includes = null) {
            return await Task.Run(() => {
                var set = this.m_dbSet.AsQueryable();
                if(includes != null) {
                    set = includes.Aggregate(set, (current, inc) => current.Include(inc));
                }
                
                return predicate == null ? set : predicate.Aggregate(set, (current, pr) => current.Where(pr));
            });
        }

        public async Task<TEntity> GetByIdAsync(long id) {
            return await this.m_dbSet.FindAsync(id);
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate = null) {
            return predicate == null ? await this.m_dbSet.AnyAsync() : await this.m_dbSet.AnyAsync(predicate);
        }

        public async Task UpdateAsync(TEntity item) {
            if(item == null) throw new ArgumentNullException(nameof(item));

            this.Context.Entry(item).State = EntityState.Modified;
            await this.Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity item) {
            if(item == null) throw new ArgumentNullException(nameof(item));

            this.Context.Remove(item);
            await this.Context.SaveChangesAsync();
        }
    }
}
