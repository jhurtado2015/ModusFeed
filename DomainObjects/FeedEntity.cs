using System;
using System.Collections.Generic;
using System.Text;

namespace DomainEntities
{
    public class FeedEntity: BaseEntity
    {
        public string source { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string description { get; set; }
    }
}
