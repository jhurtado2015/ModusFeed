using BusinessDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModusFeedReader.Controllers
{
    public class SavedFeedsController : Controller
    {
        Feed feed;

        // GET: SavedFeeds
        public ActionResult DisplaySavedFeeds()
        {
            feed = new Feed();
            var userId = 1; //Hardcoded for demo purposes
            return View("~/Views/Home/SavedFeeds.cshtml",feed.DisplaySavedFeeds(userId));
        }

        [HttpPost]
        public ActionResult DeleteSavedFeeds(int Id)
        {
            feed = new Feed();
            var userId = 1; //Hardcoded for demo purposes

            return View("~/Views/Home/SavedFeeds.cshtml", feed.DeleteSavedFeed(userId, Id));
        }
    }
}