using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class SavedFeedRepository : GenericFeedRepository<SavedFeedEntity>, IGenericFeedRepository<SavedFeedEntity>, ISavedFeedRepository
    {
        public SavedFeedRepository(FeedContext context) : base(context)
        {
        }
    }
}
