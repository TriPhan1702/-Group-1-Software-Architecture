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
using WebGrease.Css.Extensions;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
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
                select c;

            if (!comics.Any())
            {
                return NotFound();
            }

            return Ok(comics.ToList().Select(Mapper.Map<Comic, ComicThumbnailDto>));
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

        //GET api/comics/search/{searchValue}
        //Search for a comic
        [Route("api/comics/search/{searchValue}")]
        public IHttpActionResult SearchComic(string searchValue)
        {
            var comic = from c in Context.Comics
                where c.Name.Contains(searchValue) 
                      || c.Authors.Any(a => a.Name.Contains(searchValue)) 
                      || c.Publisher.Name.Contains(searchValue) 
                      || c.Tags.Any(t => t.Name.Contains(searchValue) 
                      || c.Authors.Any(a => a.Name.Contains(searchValue)))
                select c;

            if (!comic.Any())
            {
                return NotFound();
            }

            return Ok(comic.ToList().Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //POST api/comics/rate
        //Rate a comic
        [HttpPost]
        [Route("api/comics/rate")]
        public IHttpActionResult RateComic(RatingDto ratingDto)
        {
            var rating = new RatingList()
            {
                AccountId = ratingDto.AccountId,
                ComicId = ratingDto.ComicId,
                Rating = ratingDto.Rating,
            };

            var comic = Context.Comics.SingleOrDefault(c => c.IsActive && c.Id == ratingDto.ComicId);
            if (comic == null)
            {
                return NotFound();
            }

            comic.Rating = ((comic.Rating * comic.TimeRated) + ratingDto.Rating) / ++comic.TimeRated;
            try
            {
                Context.RatingLists.Add(rating);
                Context.SaveChanges();
                return Ok(ratingDto);
            }
            catch (Exception)
            {
                return Conflict();
            }
            
        }

        //POST api/comics/favorite
        [HttpPost]
        [Route("api/comics/favorite")]
        public IHttpActionResult AddToFavorite(FavoriteDto favoriteDto)
        {
            var account = Context.Accounts.SingleOrDefault(a => a.IsActive && a.Id == favoriteDto.AccountId);
            if (account == null)
            {
                return NotFound();
            }

            var comic = Context.Comics.SingleOrDefault(c => c.IsActive && c.Id == favoriteDto.ComicId);
            if (comic == null)
            {
                return NotFound();
            }

            try
            {
                account.Comics.Add(comic);
                return Ok(favoriteDto);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //PUT /api/comics/changeComicStatus/comicId
        //Activate/Deactivate a Comic
        [HttpPut]
        [Route("api/comics/changeComicStatus/{comicId}")]
        public IHttpActionResult ChangeComicStatus(int comicId)
        {
            var comic = Context.Comics.SingleOrDefault(c => c.Id == comicId);

            if (comic == null)
            {
                return NotFound();
            }

            comic.IsActive = !comic.IsActive;
            Context.SaveChanges();

            return Ok(Ok(Mapper.Map<Comic, ComicDto>(comic)));
        }
    }
}
