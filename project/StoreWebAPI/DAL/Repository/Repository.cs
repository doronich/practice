using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity {

        private readonly DbContext m_context;
        private readonly DbSet<TEntity> m_dbSet;

        public Repository(DbContext context) {
            this.m_context = context;
            this.m_dbSet = context.Set<TEntity>();
        }

        public async Task Create(TEntity item) {
            await this.m_dbSet.AddAsync(item);
            await this.m_context.SaveChangesAsync();
        }

        public async Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity,bool>> predicate = null) {
            return await Task.Run(() => predicate == null ? this.m_dbSet : this.m_dbSet.Where(predicate));
        }

        public async Task<TEntity> GetById(int id) {
            return await this.m_dbSet.FindAsync(id);
        }

        public async Task Remove(TEntity item) {
            this.m_dbSet.Remove(item);
            await this.m_context.SaveChangesAsync();
        }

        public async Task Update(TEntity item) {
            this.m_context.Entry(item).State = EntityState.Modified;
            await this.m_context.SaveChangesAsync();
        }
    }
}
