using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IGenericFeedRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> All();
        IList<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Erase(TEntity entity);

    }
}
