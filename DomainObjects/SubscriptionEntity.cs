using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainEntities
{
    public class SubscriptionEntity: BaseEntity
    {      
        public int userId { get; set; }

        [ForeignKey("userId")]
        public UserEntity user { get; set; }

        public int feedId { get; set; } 

        [ForeignKey("feedId")]
        public FeedEntity feed { get; set; }

    }
}
