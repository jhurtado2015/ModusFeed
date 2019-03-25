using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class FeedRepository : GenericFeedRepository<FeedEntity>, IGenericFeedRepository<FeedEntity>, IFeedRepository
    {
        public FeedRepository(FeedContext context) : base(context)
        {
           
        }
    }
}
