using EduTrack.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Data.IRepositories
{
    public interface IRepository<TEntity> where TEntity : Auditable
    {
        Task<bool> DeleteAsync(int id);
        IQueryable<TEntity> SelectAll();
        Task<TEntity> SelectByIdAsync(int id);
        Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
