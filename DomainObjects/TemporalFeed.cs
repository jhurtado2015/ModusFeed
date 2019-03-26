using System;
using System.Collections.Generic;
using System.Text;

namespace DomainEntities
{
    public class TemporalFeed : BaseEntity
    {
        public int userId { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string pubDate { get; set; }
        public string author { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public int feedId { get; set; }
        public string imageUrl { get; set; }
        public DateTime lastSynced { get; set; }
    }
}
