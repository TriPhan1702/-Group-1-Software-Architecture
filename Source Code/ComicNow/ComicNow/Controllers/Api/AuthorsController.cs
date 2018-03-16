using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.Models;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class AuthorsController : ApiController
    {
        public ComicNowEntities Context;

        public AuthorsController()
        {
            Context = new ComicNowEntities();
        }

        //GET api/authors
        //Get all of the authors in the database
        [HttpGet]
        public IHttpActionResult GetAuthors()
        {
            var authors = Context.Authors;

            if (!authors.Any())
            {
                return NotFound();
            }

            return Ok(authors.ToList().Select(Mapper.Map<Author, AuthorDto>));
        }

        //GET api/authors/{authorId}/findComics
        //Search comics by authorId
        [HttpGet]
        [Route("api/authors/{authorId}/findComics")]
        public IHttpActionResult SearchComicByAuthor(int authorId)
        {
            var author = Context.Authors.SingleOrDefault(t => t.Id == authorId);

            if (author == null || !author.Comics.Any())
            {
                return NotFound();
            }

            return Ok(author.Comics.ToList().Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //POST api/authors/{authorName}
        //Add a new Author
        [HttpPost]
        [Route("api/authors/{authorName}")]
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
    }
}
