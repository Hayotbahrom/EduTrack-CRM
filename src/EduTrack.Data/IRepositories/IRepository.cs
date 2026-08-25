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

        /// <summary>
        /// Entity'ni to'g'ridan-to'g'ri soft delete qiladi (Id bo'yicha qidirmasdan).
        /// Composite key'li entity'lar uchun (masalan StudentGroup) shu overload ishlatilsin —
        /// DeleteAsync(int id) ularda ishlamaydi.
        /// </summary>
        Task<bool> DeleteAsync(TEntity entity);

        IQueryable<TEntity> SelectAll();
        Task<TEntity> SelectByIdAsync(int id);
        Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
