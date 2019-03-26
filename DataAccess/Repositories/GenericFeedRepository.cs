using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class  GenericFeedRepository<TEntity> :  IGenericFeedRepository<TEntity> where TEntity: BaseEntity    {
        internal FeedContext _dbContext;
        internal DbSet<TEntity> _dbSet;

        public GenericFeedRepository(FeedContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> All()
        {
            return _dbSet.AsNoTracking();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.SaveChanges();
        }

        public void Erase(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public IList<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IList<TEntity> results = _dbSet.AsNoTracking().Where(predicate).ToList();
            return results;
        }
    }
}
