using DataAccess.DatabaseContext;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using ViewModels;

namespace BusinessDomain
{
    public class Feed
    {
        IFeedRepository feedRepository;
        ISubscriptionRepository subscriptionRepository;
        ITemporalFeedRepository temporalFeedRepository;
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

        public FeedContainerVM PopulateFeedContainer(FeedContainerInputModel searchModel = null)
        {
            FeedContainerVM feedContainerVM = new FeedContainerVM();

            feedRepository = new FeedRepository(new FeedContext());
            subscriptionRepository = new SubscriptionRepository(new FeedContext());

            feedContainerVM.feeds = ConvertToSubscription();

            List<FeedItemEntity> downloadFeeds = new List<FeedItemEntity>();
            

            //TODO: Search parameter

          /* if(searchModel == null )
            {
                foreach (FeedEntity subscribedFeed in feedContainerVM.feeds)
                {
                    downloadFeeds.AddRange(GetFeeds(subscribedFeed.url));
                }   
            }
            else
            {
                foreach (FeedEntity subscribedFeed in feedContainerVM.feeds)
                {
                    if (searchModel.selectedFeedId > 0)
                    {
                        if (subscribedFeed.Id == searchModel.selectedFeedId)
                            downloadFeeds.AddRange(GetFeeds(subscribedFeed.url));
                    }
                    else
                    {
                        downloadFeeds.AddRange(GetFeeds(subscribedFeed.url));
                    }
                }


                if (!String.IsNullOrEmpty(searchModel.searchKey))
                    downloadFeeds = downloadFeeds.Where(x => x.title.Contains(searchModel.searchKey) || x.description.Contains(searchModel.searchKey)).ToList();
            }*/

            feedContainerVM.feedItems = downloadFeeds;

            return feedContainerVM;

        }

        private IEnumerable<FeedItemEntity> WriteFeedsToTemporalDatabase(IEnumerable<FeedItemEntity> feeds)
        {
            var userId = 1;  //harcoded userId for demo purposes
            //Store feeds in temporal database and assign an identifier
            foreach (FeedItemEntity feed in feeds)
            {
                temporalFeedRepository.Add(new TemporalFeed {
                         description = feed.description,
                         IsActive = feed.IsActive,
                         link = feed.link,
                         author = feed.author,
                         category = feed.category,
                         imageUrl = feed.imageUrl,
                         pubDate = feed.pubDate,
                         title = feed.title,
                         userId = userId
                });
            }

            return temporalFeedRepository.FindBy(x => x.userId == userId).ToList();
        }
   
        private  IEnumerable<FeedItemEntity> GetFeeds(string url)
        {
            XDocument rssfeedxml;
            XNamespace namespaceName = "http://www.w3.org/2005/Atom";
            
            rssfeedxml = XDocument.Load(url);

            StringBuilder rssContent = new StringBuilder();

            byte[] byteArray = Encoding.ASCII.GetBytes(rssfeedxml.ToString());
            MemoryStream stream = new MemoryStream(byteArray);

            XmlDocument xmlDocument = new XmlDocument();

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { CheckCharacters = false };

            using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
            {
                // Load our XmlDocument
                xmlReader.MoveToContent();
                xmlDocument.Load(xmlReader);
            }

            XDocument securedRssFeedXml = XDocument.Parse(xmlDocument.OuterXml);

            var list = (from descendant in securedRssFeedXml.Descendants("item")

                        select new FeedItemEntity
                        {
                            
                            title = descendant.Element("title") != null ? descendant.Element("title").Value : "",
                            description = descendant.Element("description") != null ? descendant.Element("description").Value : "",
                            link = descendant.Element("link") != null ? descendant.Element("link").Value : "",
                            pubDate = descendant.Element("pubDate") != null ? descendant.Element("pubDate").Value : "",
                            author = descendant.Element("author") != null ? descendant.Element("author").Value : "",
                            imageUrl = descendant.Element("enclosure") != null ? descendant.Element("enclosure").Attribute("url").Value : "",
                            

                        });
            return list.ToList();


        }


    }
}
