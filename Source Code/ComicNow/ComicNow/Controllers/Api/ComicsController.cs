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

namespace ComicNow.Controllers.Api
{
    public class ComicsController : ApiController
    {
        public ComicNowEntities Context;

        public ComicsController()
        {
            Context = new ComicNowEntities();
        }

        //GET /api/comics
        //Get a list of Comic thumbnails
        [HttpGet]
        public IHttpActionResult GetThumbnails()
        {
            var comics = from c in Context.Comics
                where c.IsActive
                orderby c.LastUpdate
                select new ComicThumbnailDto()
                {
                    Publisher = Mapper.Map<Publisher, PublisherDto>(c.Publisher),
                    Id = c.Id,
                    Name = c.Name,
                    Rating = c.Rating,
                    ThumbnailUrl = c.ThumbnailUrl,
                    Views = c.Views,
                };

            if (!comics.Any())
            {
                return NotFound();
            }

            return Ok(comics.ToList());
        }

        //Get /api/comics/id
        //Get a Comic based on id
        [HttpGet]
        public IHttpActionResult GetComic(int id)
        {
            var comic = Context.Comics.SingleOrDefault(c => c.IsActive);

            if (comic == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Comic, ComicDto>(comic));
        }

    }
}
