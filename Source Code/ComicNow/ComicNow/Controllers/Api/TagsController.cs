using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.Models;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class TagsController : ApiController
    {
        public ComicNowEntities Context;

        public TagsController()
        {
            Context = new ComicNowEntities();
        }

        //GET api/tags
        //Get all the tags in the database
        [HttpGet]
        public IHttpActionResult GetTags()
        {
            var tags =  Context.Tags;

            if (!tags.Any())
            {
                return NotFound();
            }

            return Ok(tags.ToList().Select(Mapper.Map<Tag, TagDto>));
        }

        //GET api/tags/comicId
        //find all the tag of a comic
        [HttpGet]
        public IHttpActionResult GetComicTag(int comicId)
        {
            var comic = Context.Comics.SingleOrDefault(c => c.Id == comicId);

            if (comic == null)
            {
                return NotFound();
            }

            var tags = (from t in comic.Tags select t).ToList();
            if (!tags.Any())
            {
                return NotFound();
            }

            return Ok(tags);
        }

        //Get api/tags/search/{searchValue}
        //Find all the tags that contain the searchValue
        [HttpGet]
        [Route("api/tags/search/{searchValue}")]
        public IHttpActionResult SearchTag(string searchValue)
        {
            var tags = from t in Context.Tags
                where t.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower())
                select t;

            if (!tags.Any())
            {
                return NotFound();
            }

            return Ok(tags.ToList().Select(Mapper.Map<Tag, TagDto>));
        }

        [HttpGet]
        [Route("api/tags/{tagId}/findComics")]
        public IHttpActionResult SearchComicByTag(int tagId)
        {
            var tag = Context.Tags.SingleOrDefault(t => t.Id == tagId);

            if (tag == null || !tag.Comics.Any())
            {
                return NotFound();
            }

            return Ok(tag.Comics.ToList().Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //POST /api/tags/{tagName}
        //Add a new Tag
        [HttpPost]
        [Route("api/tags/{tagName}")]
        public IHttpActionResult CreateTag(string tagName)
        {
            try
            {
                var newTag = new Tag()
                {
                    Name = tagName
                };
                Context.Tags.Add(newTag);
                Context.SaveChanges();

                return Created(new Uri(Request.RequestUri + "/" + newTag.Id), Mapper.Map<Tag, TagDto>(newTag));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }
    }
}
