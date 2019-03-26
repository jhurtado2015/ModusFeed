using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class FeedContainerInputModel: FeedItemEntity
    {
        public int selectedFeedId { get; set; }

        public string searchKey { get; set; }
    }
    public class FeedContainerVM : FeedContainerInputModel
    {
        public bool hasSubscriptions { get; set; }
        public IEnumerable<FeedItemEntity> feedItems { get; set; }

        public IEnumerable<SubscriptionFeed> feeds { get; set; }
        
    }
}
