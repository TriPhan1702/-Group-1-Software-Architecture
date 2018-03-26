using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.DTOs.Comic;
using ComicNow.Models;
using ComicNow.Services;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class TagsController : ApiController
    {
        public TagServices TagServices;

        public ComicServices ComicServices;

        public TagsController()
        {
            TagServices = new TagServices();
            ComicServices = new ComicServices();
        }

        //GET api/tags
        //Get all the tags in the database
        [HttpGet]
        public IHttpActionResult GetTags()
        {
            var tags =  TagServices.GetAllTags();

            if (!tags.Any())
            {
                return NotFound();
            }

            return Ok(tags.Select(Mapper.Map<Tag, TagDto>));
        }

        //GET api/tags/comicId
        //find all the tag of a comic
        [HttpGet]
        public IHttpActionResult GetComicTag(int comicId)
        {
            var comic = ComicServices.GetActiveComicById(comicId);

            if (comic == null)
            {
                return NotFound();
            }

            var tags = TagServices.FindTagsOfAComic(comic);

            if (!tags.Any())
            {
                return NotFound();
            }

            return Ok(tags.Select(Mapper.Map<Tag, TagDto>));
        }

        //Get api/tags/search/{searchValue}
        //Find all the tags that contain the searchValue
        [HttpGet]
        [Route("api/tags/search/{searchValue}")]
        public IHttpActionResult SearchTag(string searchValue)
        {
            var tags = TagServices.FindTagByString(searchValue);

            if (!tags.Any())
            {
                return NotFound();
            }

            return Ok(tags.Select(Mapper.Map<Tag, TagDto>));
        }

        [HttpGet]
        [Route("api/tags/{tagId}/findComics")]
        public IHttpActionResult SearchComicByTag(int tagId)
        {
            var tag = TagServices.GetTagById(tagId);

            if (tag == null)
            {
                return NotFound();
            }

            var comics = ComicServices.SearchComicByTag(tag);
            if (!comics.Any())
            {
                return NotFound();
            }

            return Ok(comics.Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //POST /api/tags/create/{tagName}
        //Add a new Tag
        [HttpPost]
        [Route("api/tags/create/{tagName}")]
        public IHttpActionResult CreateTag(string tagName)
        {
            var newTag = TagServices.CreateTag(tagName);

            if (newTag == null)
            {
                return Conflict();
            }

            return Created(new Uri(Request.RequestUri + "/" + TagServices.GetTagId(newTag)), Mapper.Map<Tag,TagDto>(newTag));
        }
    }
}
