using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class FeedContainerVM
    {

        public int selectedFeedId { get; set; }

        public IEnumerable<FeedItemEntity> feedItems { get; set; }

        public IEnumerable<SubscriptionFeed> feeds { get; set; }
    }
}
