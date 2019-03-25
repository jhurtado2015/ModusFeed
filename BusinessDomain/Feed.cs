using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ViewModels;

namespace BusinessDomain
{
    public class Feed
    {
        IFeedRepository feedRepository;
        ISubscriptionRepository subscriptionRepository;
        public Feed()
        {
        }

        /// <summary>
        /// Get all available feeds
        /// </summary>
        /// <returns></returns>
        public SubscriptionsVM GetAllFeeds()
        {
            feedRepository = new FeedRepository(new FeedContext());
            SubscriptionsVM model = new SubscriptionsVM();

            model.Feeds = ConvertToSubscription();
            return model;
        }

        private IEnumerable<SubscriptionFeed> ConvertToSubscription()
        {
            feedRepository = new FeedRepository(new FeedContext());
            subscriptionRepository = new SubscriptionRepository(new FeedContext());

            var feeds = feedRepository.All();
            var subscribedFeeds = subscriptionRepository.All();

            foreach (FeedEntity feed in feeds)
            {
                yield return new SubscriptionFeed() {

                    Id = feed.Id,
                    IsActive = feed.IsActive,
                    source = feed.source,
                    title = feed.title,
                    url = feed.url,
                    isSubscribed = subscribedFeeds.Any(x => x.feedId == feed.Id && x.IsActive == true)
                };
            };
        }

        /// <summary>
        /// Subscribe to an specific feed
        /// </summary>
        /// <param name="subscription"></param>
        public void SubscribeToFeed(SubscriptionEntity subscription)
        {
            subscriptionRepository = new SubscriptionRepository(new FeedContext());

            var result = subscriptionRepository.FindBy(x => x.feedId == subscription.feedId && x.userId == subscription.userId);

            //TODO: Move this code to an extension method
            if (result != null)
            {
                if (result.Count > 0)
                {
                    subscription.IsActive = false;
                    subscriptionRepository.Update(subscription);

                }
                else
                {
                    subscriptionRepository.Add(subscription);
                }

            }
            else
            {
                subscriptionRepository.Add(subscription);
            }

        }

        public FeedContainerVM PopulateFeedContainer()
        {
            FeedContainerVM feedContainerVM = new FeedContainerVM();

            feedRepository = new FeedRepository(new FeedContext());
            subscriptionRepository = new SubscriptionRepository(new FeedContext());

            feedContainerVM.feeds = ConvertToSubscription();

            List<FeedItemEntity> downloadFeeds = new List<FeedItemEntity>();
            foreach(FeedEntity subscribedFeed in feedContainerVM.feeds)
            {
                downloadFeeds.AddRange(GetFeeds(subscribedFeed.url));
            }

            feedContainerVM.feedItems = downloadFeeds;

            return feedContainerVM;

        }

        /*public FeedContainerVM DownloadFeedFromSource()
        {


        }*/

        public  IEnumerable<FeedItemEntity> GetFeeds(string url)
        {
            XDocument rssfeedxml;
            XNamespace namespaceName = "http://www.w3.org/2005/Atom";
            rssfeedxml = XDocument.Load("https://www.nasa.gov/rss/dyn/mission_pages/kepler/news/kepler-newsandfeatures-RSS.rss");


            StringBuilder rssContent = new StringBuilder();

            var list = (from descendant in rssfeedxml.Descendants("item")

                            //Response.Write(list);  
                        select new FeedItemEntity
                        {
                            title = descendant.Element("title") != null ? descendant.Element("title").Value : "",
                            description = descendant.Element("description") != null ? descendant.Element("description").Value : "",
                            link = descendant.Element("link") != null ? descendant.Element("link").Value : "",
                            pubDate = descendant.Element("pubDate") != null ? descendant.Element("pubDate").Value : "",
                            author = descendant.Element("author") != null ? descendant.Element("author").Value : ""

                        });
            return list.ToList();
        }
    }
}
