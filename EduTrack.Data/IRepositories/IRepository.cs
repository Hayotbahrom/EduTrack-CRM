using EduTrack.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Data.IRepositories
{
    internal interface IRepository<TEntity> where TEntity : Auditable
    {
        Task<bool> DeleteAsync(long id);
        IQueryable<TEntity> SelectAll();
        Task<TEntity> SelectByIdAsync(long id);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
