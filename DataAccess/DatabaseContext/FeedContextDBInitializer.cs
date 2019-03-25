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

            defaultFeed.Add(new FeedEntity() { source = "Fandango", title = "New Movies", url = "https://www.fandango.com/rss/newmovies.rss" });
            defaultFeed.Add(new FeedEntity() { source = "GameSpot", title = "Game News", url = "https://www.gamespot.com/feeds/game-news/" });

            context.Feeds.AddRange(defaultFeed);
            //Seed Feed Items

            base.Seed(context);
        }
    }
}
