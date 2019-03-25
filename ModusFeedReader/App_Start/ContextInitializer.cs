using DataAccess.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ModusFeedReader.App_Start
{
    public static class ContextInitializer
    {
        public static void Initialize()
        {
       
            using(var c = new FeedContext())
            {
                Database.SetInitializer<FeedContext>(new FeedContextDBInitializer());
            }
            

            

        }
    }
}