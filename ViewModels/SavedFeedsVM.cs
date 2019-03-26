using DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class SavedFeedsVM
    {
        public IEnumerable<SavedFeedEntity> savedFeedItems { get; set; }
    }
}
