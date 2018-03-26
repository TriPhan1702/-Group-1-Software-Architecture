using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.DTOs.Comic;
using ComicNow.Models;
using ComicNow.Services;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class AuthorsController : ApiController
    {
        public AuthorServices AuthorServices;
        public ComicServices ComicServices;

        public AuthorsController()
        {
            AuthorServices = new AuthorServices();
            ComicServices = new ComicServices();
        }

        //GET api/authors
        //Get all of the authors in the database
        [HttpGet]
        [Route("api/authors")]
        public IHttpActionResult GetAuthors()
        {
            var authors = AuthorServices.GetAllAuthors();

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
            var author = AuthorServices.GetAuthorById(authorId);

            if (author == null)
            {
                return NotFound();
            }

            var comics = ComicServices.SearchComicByAuthor(author);

            if (!comics.Any())
            {
                return NotFound();
            }

            return Ok(comics.Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //POST api/authors/create/{authorName}
        //Add a new Author
        [HttpPost]
        [Route("api/authors/create/{authorName}")]
        public IHttpActionResult CreateAuthor(string authorName)
        {
                var newAuthor = AuthorServices.CreateNewAuthor(authorName);

                if (newAuthor == null)
                {
                    return Conflict();
                }

                return Created(new Uri(Request.RequestUri + "/" + newAuthor.Id), Mapper.Map<Author, AuthorDto>(newAuthor));
        }
    }
}
