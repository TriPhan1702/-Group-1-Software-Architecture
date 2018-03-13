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
    public class AdminController : ApiController
    {
        public ComicNowEntities Context;

        public AdminController()
        {
            Context = new ComicNowEntities();
        }

        //PUT /api/admin/changeAccountStatus/id
        //Activate/Deactivate an Account
        [HttpPut]
        [Route("api/admin/changeAccountStatus/{id}")]
        public IHttpActionResult ChangeAccountStatus(int id)
        {
            var account = Context.Accounts.SingleOrDefault(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            account.IsActive = !account.IsActive;
            Context.SaveChanges();

            return Ok();
        }

        //PUT /api/admin/changeComicStatus/id
        //Activate/Deactivate a Comic
        [HttpPut]
        [Route("api/admin/changeComicStatus/{id}")]
        public IHttpActionResult ChangeComicStatus(int id)
        {
            var comic = Context.Comics.SingleOrDefault(c => c.Id == id);

            if (comic == null)
            {
                return NotFound();
            }

            comic.IsActive = !comic.IsActive;
            Context.SaveChanges();

            return Ok();
        }

        //PUT /api/admin/changeChapterStatus/id
        //Activate/Deactivate a Chapter
        [HttpPut]
        [Route("api/admin/changeChapterStatus/{id}")]
        public IHttpActionResult ChangeChapterStatus(int id)
        {
            var chapter = Context.Chapters.SingleOrDefault(c => c.Id == id);

            if (chapter == null)
            {
                return NotFound();
            }

            chapter.IsActive = !chapter.IsActive;
            Context.SaveChanges();

            return Ok();
        }

        //POST /api/admin/publishers
        //Add a new Publisher
        [HttpPost]
        [Route("api/admin/publishers/{publisherName}")]
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
        
        //POST /api/admin/authors/{authorName}
        //Add a new Author
        [HttpPost]
        [Route("api/admin/authors/{authorName}")]
        public IHttpActionResult CreateAuthor(string authorName)
        {
            try
            {
                var newAuthor = new Author()
                {
                    Name = authorName
                };
                Context.Authors.Add(newAuthor);
                Context.SaveChanges();

                return Created(new Uri(Request.RequestUri + "/" + newAuthor.Id), Mapper.Map<Author, AuthorDto>(newAuthor));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //POST /api/admin/tags/{tagName}
        //Add a new Tag
        [HttpPost]
        [Route("api/admin/authors/{tagName}")]
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
