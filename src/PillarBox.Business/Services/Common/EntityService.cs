using Microsoft.EntityFrameworkCore;
using PillarBox.Business.Exceptions;
using PillarBox.Data;
using PillarBox.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PillarBox.Business.Services.Common
{
    public abstract class EntityService<T> : IEntityService<T> where T : EntityBase, new()
    {
        protected PillarBoxContext _dbContext;

        public EntityService(PillarBoxContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<T> Table
        {
            get
            {
                return _dbContext.Set<T>();
            }
        }


        /// <summary>
        /// Load single entity by Id and optionally include properties in load
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual T GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            return GetSingleItemWhere(t => t.Id == id, includes);
        }

        /// <summary>
        /// Get all undeleted entities
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return IncludeProperties(includes);
        }

        /// <summary>
        /// Get a single item matching the given predicate
        /// </summary>
        /// <param name="where"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        protected virtual T GetSingleItemWhere(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> tbl = IncludeProperties(includes);

            var entity = tbl.Where(where).FirstOrDefault();
            if (entity == null)
            {
                throw new EntityNotFoundException();
            }
            else
            {
                return entity;
            }
        }

        /// <summary>
        /// Add any given inclusions to the query
        /// </summary>
        /// <param name="includes"></param>
        /// <returns></returns>
        protected IQueryable<T> IncludeProperties(Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> tbl = Table;
            if (includes != null)
            {
                foreach (var i in includes)
                {
                    tbl = tbl.Include(i);
                }
            }
            return tbl;
        }

        public virtual void Delete(Guid id)
        {
            var item = GetById(id);
            Table.Remove(item);
            _dbContext.SaveChanges();
        }
    }
}
