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

        //POST /api/comics
        //Create a new Comic Series
        [HttpPost]
        public IHttpActionResult CreateComic(UploadComicDto uploadComicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newComic = new Comic()
            {
                Name = uploadComicDto.Name,
                OtherName = uploadComicDto.OtherName,
                Description = uploadComicDto.Description,
                ThumbnailUrl = uploadComicDto.ThumbnailUrl,
                PublisherId = uploadComicDto.PublisherId,
                ChapterNumber = 0,
                CreatedDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                IsActive = true,
                Rating = 0,
                Views = 0,
                TimeRated = 0,
                Status = false,
            };

            if (uploadComicDto.Authors.Count > 0)
            {
                try
                {
                    foreach (var author in uploadComicDto.Authors)
                    {
                        newComic.Authors.Add(Context.Authors.Single(a => a.Id == author.Id));
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }

            if (uploadComicDto.Tags.Count > 0)
            {
                try
                {
                    foreach (var tag in uploadComicDto.Tags)
                    {
                        newComic.Tags.Add(Context.Tags.Single(t => t.Id == tag.Id));
                    }
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }

            try
            {
                Context.Comics.Add(newComic);
                Context.SaveChanges();
                return Created(new Uri(Request.RequestUri + "/" + newComic.Id), Mapper.Map<Comic, ComicDto>(newComic));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

    }
}
