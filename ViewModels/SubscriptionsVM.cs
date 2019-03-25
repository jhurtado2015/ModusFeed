using DomainEntities;
using System;
using System.Collections.Generic;

namespace ViewModels
{
    public class SubscriptionFeed : FeedEntity
    {
        public bool isSubscribed { get; set; }

    }

    public class SubscriptionsVM  
    {
        public IEnumerable<SubscriptionFeed> Feeds { get; set; }

        
    }
}
