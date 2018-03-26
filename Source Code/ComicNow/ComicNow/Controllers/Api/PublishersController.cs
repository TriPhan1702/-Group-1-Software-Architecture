using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.DTOs.Comic;
using ComicNow.Models;
using ComicNow.Services;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class PublishersController : ApiController
    {
        public PublisherServices PublisherServices;
        public ComicServices ComicServices;

        public PublishersController()
        {
            PublisherServices = new PublisherServices();
            ComicServices = new ComicServices();
        }

        //GET api/publishers
        //Get a list of all publisher
        [HttpGet]
        [Route("api/publishers")]
        public IHttpActionResult GetPublishers()
        {
            var publisher = PublisherServices.GetAllPublishers();

            if (!publisher.Any())
            {
                return NotFound();
            }

            return Ok(publisher.Select(Mapper.Map<Publisher, PublisherDto>));
        }

        //GET api/publishers/{publisherId}
        //get a publisher based on the publisherId
        [HttpGet]
        public IHttpActionResult GetPublisher(int publisherId)
        {
            var publisher = PublisherServices.GetPublisherById(publisherId);

            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Publisher, PublisherDto>(publisher));
        }

        //POST /api/publishers/create/{publisherName}
        //Add a new Publisher
        [HttpPost]
        [Route("api/publishers/create/{publisherName}")]
        public IHttpActionResult CreatePublisher(string publisherName)
        {
            var newPublisher = PublisherServices.CreatePublisher(publisherName);
            if (newPublisher == null)
            {
                return Conflict();
            }
            return Created(new Uri(Request.RequestUri + "/" + PublisherServices.GetPublisherId(newPublisher)), Mapper.Map<Publisher, PublisherDto>(newPublisher));
        }

        [HttpGet]
        [Route("api/publishers/{publisherId}/findComics")]
        public IHttpActionResult SearchComicByPublisher(int publisherId)
        {
            var publisher = PublisherServices.GetPublisherById(publisherId);

            if (publisher == null)
            {
                return NotFound();
            }

            var comics = ComicServices.SearchComicByPublisher(publisher);

            if (!comics.Any())
            {
                return NotFound();
            }

            return Ok(comics.Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }
    }
}
