using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class TemporalFeedRepository : GenericFeedRepository<TemporalFeed>, IGenericFeedRepository<TemporalFeed>, ITemporalFeedRepository
    {
        public TemporalFeedRepository(FeedContext context) : base(context)
        {
        }
    }
}
