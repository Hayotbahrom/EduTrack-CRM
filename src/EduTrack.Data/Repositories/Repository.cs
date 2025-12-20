using EduTrack.Data.DBContexts;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Commons;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EduTrack.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
    {
        private readonly EduDbContext _context;
        private DbSet<TEntity> dbSet;
        public Repository(EduDbContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await dbSet.FindAsync(id);
            result.IsDeleted = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<TEntity> SelectAll()
        {
            return dbSet;
        }

        public Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = dbSet.Where(predicate).FirstOrDefaultAsync();
            return result;
        }

        public async Task<TEntity> SelectByIdAsync(int id)
        {
            var result = await dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var result = (_context.Update(entity)).Entity;
            await _context.SaveChangesAsync();
            return result;
        }
    }
}
