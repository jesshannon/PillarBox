using System;
using System.Linq;
using System.Linq.Expressions;
using PillarBox.Data.Common;

namespace PillarBox.Business.Services.Common
{
    public interface IEntityService<T> where T : EntityBase, new()
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        T GetById(Guid id, params Expression<Func<T, object>>[] includes);
        void Delete(Guid id);
    }
}