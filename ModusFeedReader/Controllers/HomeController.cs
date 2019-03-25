using BusinessDomain;
using DataAccess.DatabaseContext;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModusFeedReader.Controllers
{
    public class HomeController : Controller
    {
        Feed feed;

        public ActionResult Index()
        {
           

            return View();
        }

        public ActionResult FeedContainer()
        {
            feed = new Feed();
            return View(feed.PopulateFeedContainer());
        }

        public ActionResult DownloadFeedNews(string url)
        {
            feed = new Feed();
            feed.GetFeeds("aa");
            return View();
        }

        [HttpGet]
        public ActionResult Subscriptions()
        {
            feed = new Feed();
            return View(feed.GetAllFeeds());
        }

        [HttpPost]
        public ActionResult Subscriptions(int Id)
        {
            var userId = 1;

            feed = new Feed();

            feed.SubscribeToFeed(new SubscriptionEntity { userId = 1, feedId = Id });

            return View(feed.GetAllFeeds());
        }
    }
}