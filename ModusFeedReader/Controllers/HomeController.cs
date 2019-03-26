using BusinessDomain;
using DataAccess.DatabaseContext;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace ModusFeedReader.Controllers
{
    public class HomeController : Controller
    {
        Feed feed;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InitializeFeedContainer()
        {
            feed = new Feed();
            return View("~/Views/Home/FeedContainer.cshtml", feed.PopulateFeedContainer());
        }
        public ActionResult SyncroniceFeeds()
        {
            feed = new Feed();
            return View("~/Views/Home/FeedContainer.cshtml", feed.SyncroniceFeeds());
        }

        [HttpPost]
        public ActionResult FilterFeeds(FeedContainerInputModel model)
        {
            feed = new Feed();
            return View("~/Views/Home/FeedContainer.cshtml", feed.PopulateFeedContainer(model));
        }

        [HttpPost]
        public ActionResult SaveFeed(int Id)
        {
            var userId = 1; //Hardcoded for testing purposes
            feed = new Feed();
            feed.SaveFeed(Id, userId);
            return View("~/Views/Home/FeedContainer.cshtml", feed.PopulateFeedContainer(new FeedContainerInputModel { selectedFeedId = 0 }));
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