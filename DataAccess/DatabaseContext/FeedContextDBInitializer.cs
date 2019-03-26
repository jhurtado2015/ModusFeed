using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace DataAccess.DatabaseContext
{
    public class FeedContextDBInitializer : DropCreateDatabaseIfModelChanges<FeedContext>
    {
   
        protected override void Seed(FeedContext context)
        {
            //Seed Users
            IList<UserEntity> defaultUsers = new List<UserEntity>();

            defaultUsers.Add(new UserEntity() {  userName = "ModusCreate"  });
            
            context.Users.AddRange(defaultUsers);

            //Seed Feeds
            IList<FeedEntity> defaultFeed = new List<FeedEntity>();

            defaultFeed.Add(new FeedEntity() { source = "NASA", description= "A RSS feed containing the latest education related news from NASA.", title = "Education News", url = "https://www.nasa.gov/rss/dyn/educationnews.rss" });
            defaultFeed.Add(new FeedEntity() { source = "NASA", description = "Kepler News and Features RSS", title = "Kepler Mission", url = "https://www.nasa.gov/rss/dyn/mission_pages/kepler/news/kepler-newsandfeatures-RSS.rss" });
            defaultFeed.Add(new FeedEntity() { source = "NASA", description = "A RSS news feed containing the latest NASA press releases on Earth-observing missions.", title = "Earth News", url = "https://www.nasa.gov/rss/dyn/earth.rss" });
            context.Feeds.AddRange(defaultFeed);
            //Seed Feed Items

            base.Seed(context);
        }
    }
}
