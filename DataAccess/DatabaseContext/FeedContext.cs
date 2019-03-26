using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace DataAccess.DatabaseContext
{
    public class FeedContext : DbContext
    {
        public FeedContext() : base("FeedContext") {}

        public virtual DbSet<FeedEntity> Feeds { get; set; }
        public virtual DbSet<FeedItemEntity> FeedItems { get; set; }
        public virtual DbSet<SavedFeedEntity> SavedFeeds { get; set; }
        public virtual DbSet<SubscriptionEntity> Subscriptions { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<TemporalFeed> TemporalFeed { get; set; }

    }
}
