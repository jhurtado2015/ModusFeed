﻿using DataAccess.DatabaseContext;
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
        ISavedFeedRepository savedFeedRepository;
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
            var subscribedFeeds = subscriptionRepository.All().ToList();

            foreach (FeedEntity feed in feeds)
            {
                var a = subscribedFeeds.Any(x => x.feedId == feed.Id && x.IsActive == true);

                yield return new SubscriptionFeed() {

                    Id = feed.Id,
                    description = feed.description,
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

            var result = subscriptionRepository.FindBy(x => x.feedId == subscription.feedId && x.userId == subscription.userId).FirstOrDefault();

            if (result != null)
            {
                subscriptionRepository.Erase(result);
            }
            else
            {
                subscriptionRepository.Add(subscription);
            }

            SyncroniceFeeds();

        }

        private bool hasSubscriptions()
        {
            var userId = 1;  //harcoded userId for demo purposes
            subscriptionRepository = new SubscriptionRepository(new FeedContext());
            //Find subscriptions
            if (subscriptionRepository.FindBy(x => x.userId == userId).ToList().Count > 0)
            {
               return true;
            }

            return false;

        }
        public FeedContainerVM PopulateFeedContainer(FeedContainerInputModel searchModel = null)
        {
            var userId = 1;  //harcoded userId for demo purposes

            FeedContainerVM feedContainerVM = new FeedContainerVM();

            feedRepository = new FeedRepository(new FeedContext());
            subscriptionRepository = new SubscriptionRepository(new FeedContext());
            temporalFeedRepository = new TemporalFeedRepository(new FeedContext());

            feedContainerVM.feeds = ConvertToSubscription().Where(x=>x.isSubscribed == true);

            feedContainerVM.hasSubscriptions = hasSubscriptions();

            List <FeedItemEntity> downloadFeeds = new List<FeedItemEntity>();
            //TODO: Search parameter

            if (searchModel == null)
            {
                

                var syncedFeeds = temporalFeedRepository.FindBy(x => x.userId == userId).ToList();

                var lastSyncedItem = syncedFeeds.FirstOrDefault();

                TimeSpan elapsedTime;

                if (lastSyncedItem != null)
                {
                    elapsedTime = DateTime.Now - lastSyncedItem.lastSynced;
                }

                //Syncronize every 8 hours
                if (elapsedTime.Hours >= 8 || lastSyncedItem == null)
                {
                    return SyncroniceFeeds();
                }
                else
                {
                    feedContainerVM.feedItems = 
                     syncedFeeds.Select(x => new FeedItemEntity
                    {
                        Id = x.Id,
                        description = x.description != null ? x.description : "",
                        IsActive = x.IsActive,
                        link = x.link != null ? x.link : "",
                        author = x.author != null ? x.author : "",
                        imageUrl = x.imageUrl != null ? x.imageUrl : "",
                        pubDate = x.pubDate != null ? x.pubDate : "",
                        title = x.title != null ? x.title : ""
                    }).ToList();

                    return feedContainerVM;
                }  
            }
            else
            {
                var syncedFeeds = temporalFeedRepository.FindBy(x => x.userId == userId).ToList();

                IEnumerable<FeedItemEntity> filteredFeeds;

                if (searchModel.selectedFeedId > 0)
                {
                    filteredFeeds = syncedFeeds.Where(x => x.feedId == searchModel.selectedFeedId && (x.title.ToLower().Contains(searchModel.searchKey != null ? searchModel.searchKey.ToLower() : string.Empty) || x.description.ToLower().Contains(searchModel.searchKey != null ? searchModel.searchKey.ToLower() : string.Empty))).Select(x => new FeedItemEntity
                    {
                        Id = x.Id,
                        description = x.description != null ? x.description : "",
                        IsActive = x.IsActive,
                        link = x.link != null ? x.link : "",
                        author = x.author != null ? x.author : "",
                        imageUrl = x.imageUrl != null ? x.imageUrl : "",
                        pubDate = x.pubDate != null ? x.pubDate : "",
                        title = x.title != null ? x.title : ""
                    }).ToList();
                }
                else
                {
                    filteredFeeds = syncedFeeds.Where(x => x.title.ToLower().Contains(searchModel.searchKey != null ? searchModel.searchKey.ToLower() : string.Empty) || x.description.ToLower().Contains(searchModel.searchKey != null ? searchModel.searchKey.ToLower() : string.Empty)).Select(x => new FeedItemEntity
                    {
                        Id = x.Id,
                        description = x.description != null ? x.description : "",
                        IsActive = x.IsActive,
                        link = x.link != null ? x.link : "",
                        author = x.author != null ? x.author : "",
                        imageUrl = x.imageUrl != null ? x.imageUrl : "",
                        pubDate = x.pubDate != null ? x.pubDate : "",
                        title = x.title != null ? x.title : ""
                    }).ToList();
                }


                feedContainerVM.feedItems = filteredFeeds;
            }



            return feedContainerVM;

        }

        public FeedContainerVM SyncroniceFeeds()
        {
            FeedContainerVM feedContainerVM = new FeedContainerVM();

            feedRepository = new FeedRepository(new FeedContext());
            subscriptionRepository = new SubscriptionRepository(new FeedContext());

            feedContainerVM.feeds = ConvertToSubscription().Where(x => x.isSubscribed == true); 

            feedContainerVM.hasSubscriptions = hasSubscriptions();

            List<FeedItemEntity> downloadFeeds = new List<FeedItemEntity>();

            temporalFeedRepository = new TemporalFeedRepository(new FeedContext());

            var userId = 1;  //harcoded userId for demo purposes

            var currentFeeds = temporalFeedRepository.FindBy(x => x.userId == userId).ToList();

            //Wipeout table before syncronization
            foreach (TemporalFeed tmpFeed in currentFeeds)
            {
                temporalFeedRepository.Erase(tmpFeed);
            }

            foreach (FeedEntity subscribedFeed in feedContainerVM.feeds)
            {
                downloadFeeds.AddRange(WriteFeedsToTemporalDatabase(GetFeeds(subscribedFeed.url), subscribedFeed.Id));
            }

            feedContainerVM.feedItems = downloadFeeds;

            return feedContainerVM;
        }

        public void SaveFeed(int tempFeedId,int userId)
        {
            savedFeedRepository = new SavedFeedRepository(new FeedContext());
            temporalFeedRepository = new TemporalFeedRepository(new FeedContext());
    
            //Get feed from temporal storage
            var savedFeed = temporalFeedRepository.FindBy(x => x.Id == tempFeedId).Select(feed => new SavedFeedEntity
            {
                feedId =feed.feedId,
                description = feed.description != null ? feed.description : "",
                IsActive = feed.IsActive,
                link = feed.link != null ? feed.link : "",
                author = feed.author != null ? feed.author : "",
                imageUrl = feed.imageUrl != null ? feed.imageUrl : "",
                pubDate = feed.pubDate != null ? feed.pubDate : "",
                title = feed.title != null ? feed.title : "",
                userId = userId

            }).FirstOrDefault();

            //Delete feed from temporal storage
            var temporalFeedToRemove = temporalFeedRepository.FindBy(x => x.Id == tempFeedId).FirstOrDefault();
            temporalFeedRepository.Erase(temporalFeedToRemove);

            //Save feed as a user favorite feed
            savedFeedRepository.Add(savedFeed);
        }

        private IList<FeedItemEntity> WriteFeedsToTemporalDatabase(IEnumerable<FeedItemEntity> feeds, int feedId)
        {
            var userId = 1;  //harcoded userId for demo purposes

            //Store feeds in temporal database and assign an identifier
            foreach (FeedItemEntity feed in feeds)
            {
                temporalFeedRepository.Add(new TemporalFeed {
                    feedId = feedId,
                    description = feed.description != null ? feed.description : "",
                    IsActive = feed.IsActive,
                    link = feed.link != null ? feed.link : "",
                    author = feed.author != null ? feed.author : "",
                    imageUrl = feed.imageUrl != null ? feed.imageUrl : "",
                    pubDate = feed.pubDate != null ? feed.pubDate : "",
                    title = feed.title != null ? feed.title : "",
                    userId = userId,
                    lastSynced = DateTime.Now
                });
            }

            return temporalFeedRepository.FindBy(x => x.userId == userId && x.feedId == feedId).Select(x => new FeedItemEntity {
                description = x.description != null ? x.description : "",
                IsActive = x.IsActive,
                link = x.link != null ? x.link : "",
                author = x.author != null ? x.author : "",
                imageUrl = x.imageUrl != null ? x.imageUrl : "",
                pubDate = x.pubDate != null ? x.pubDate : "",
                title = x.title != null ? x.title : "",
                feedId = x.feedId,
                Id = x.Id
            }).ToList();
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

        #region SavedFeeds
        public SavedFeedsVM DisplaySavedFeeds(int userId)
        {
            savedFeedRepository = new SavedFeedRepository(new FeedContext());

            SavedFeedsVM savedFeedsVM = new SavedFeedsVM()
            {
                savedFeedItems = savedFeedRepository.FindBy(x => x.userId == userId).ToList()
            };
          
            return savedFeedsVM;
        }

        public SavedFeedsVM DeleteSavedFeed(int userId,int Id)
        {
            savedFeedRepository = new SavedFeedRepository(new FeedContext());

            var deletedEntity = savedFeedRepository.FindBy(x => x.userId == userId && x.Id == Id).FirstOrDefault();

            savedFeedRepository.Erase(deletedEntity);

            SavedFeedsVM savedFeedsVM = new SavedFeedsVM()
            {
                savedFeedItems = savedFeedRepository.FindBy(x => x.userId == userId).ToList()
            };

            return savedFeedsVM;
        }
        
        #endregion 
    }
}
