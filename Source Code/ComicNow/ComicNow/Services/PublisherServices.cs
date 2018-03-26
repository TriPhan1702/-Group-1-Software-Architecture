using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class PublisherServices
    {
        public ComicNowEntities Context;

        public PublisherServices()
        {
            Context = new ComicNowEntities();
        }

        public List<Publisher> GetAllPublishers()
        {
            return Context.Publishers.ToList();
        }

        public Publisher GetPublisherById(int id)
        {
            return Context.Publishers.SingleOrDefault(publisher => publisher.Id == id);
        }

        public Publisher CreatePublisher(string publisherName)
        {
            try
            {
                var newPublisher = new Publisher()
                {
                    Name = publisherName
                };
                Context.Publishers.Add(newPublisher);
                Context.SaveChanges();
                return newPublisher;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetPublisherId(Publisher publisher)
        {
            return publisher.Id;
        }
    }
}