using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.Models;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class PublishersController : ApiController
    {
        public ComicNowEntities Context;

        public PublishersController()
        {
            Context = new ComicNowEntities();
        }

        //GET api/publishers
        //Get a list of all publisher
        [HttpGet]
        public IHttpActionResult GetPublishers()
        {
            var publisher = Context.Publishers;
            if (!publisher.Any())
            {
                return NotFound();
            }

            return Ok(publisher.ToList().Select(Mapper.Map<Publisher, PublisherDto>));
        }

        //GET api/publishers/{publisherId}
        //get a publisher based on the publisherId
        [HttpGet]
        public IHttpActionResult GetPublisher(int publisherId)
        {
            var publisher = Context.Publishers.SingleOrDefault(p => p.Id == publisherId);
            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Publisher, PublisherDto>(publisher));
        }

        //POST /api/publishers/{publisherName}
        //Add a new Publisher
        [HttpPost]
        [Route("api/publishers/{publisherName}")]
        public IHttpActionResult CreatePublisher(string publisherName)
        {
            try
            {
                var newPublisher = new Publisher()
                {
                    Name = publisherName
                };
                Context.Publishers.Add(newPublisher);
                Context.SaveChanges();

                return Created(new Uri(Request.RequestUri + "/" + newPublisher.Id), Mapper.Map<Publisher, PublisherDto>(newPublisher));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [HttpGet]
        [Route("api/publishers/{publisherId}/findComics")]
        public IHttpActionResult SearchComicByPublisher(int publisherId)
        {
            var publisher = Context.Publishers.SingleOrDefault(p => p.Id == publisherId);

            if (publisher == null || !publisher.Comics.Any())
            {
                return NotFound();
            }

            return Ok(publisher.Comics.ToList().Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }
    }
}
