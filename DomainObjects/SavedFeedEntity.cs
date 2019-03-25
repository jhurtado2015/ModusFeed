using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainEntities
{
    public class SavedFeedEntity: BaseEntity
    {
        
        public int feedItemId { get; set; }

        [ForeignKey("feedItemId")]
        public FeedItemEntity feedItem { get; set; }

        public int userId { get;set; }

        [ForeignKey("userId")]
        public UserEntity user { get; set; }



    }
}
