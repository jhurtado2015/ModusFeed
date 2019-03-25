using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class SubscriptionRepository : GenericFeedRepository<SubscriptionEntity>, IGenericFeedRepository<SubscriptionEntity>, ISubscriptionRepository
    {
        public SubscriptionRepository(FeedContext context) : base(context)
        {
        }
    }
}
