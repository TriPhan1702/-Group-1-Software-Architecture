using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.DTOs.Comic;
using ComicNow.DTOs.Rating;
using ComicNow.Models;

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
        //Get a list of active Comic thumbnails
        [HttpGet]
        public IHttpActionResult GetThumbnails()
        {
            var comics = from c in Context.Comics
                where c.IsActive
                orderby c.LastUpdate
                select c;

            if (!comics.Any()) return NotFound();

            return Ok(comics.ToList().Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //GET /api/admin/comics
        //Get list of all comics as admin
        [HttpGet]
        [Route("api/admin/comics")]
        public IHttpActionResult GetComics()
        {
            var comics = Context.Comics;

            if (!comics.Any()) return NotFound();

            return Ok(comics.ToList().Select(Mapper.Map<Comic, LightWeightComicDto>));
        }

        //Get /api/comics/id
        //Get an active comic based on id
        [HttpGet]
        public IHttpActionResult GetComic(int id)
        {
            var comic = Context.Comics.SingleOrDefault(c => c.IsActive && c.Id == id);

            if (comic == null) return NotFound();

            return Ok(Mapper.Map<Comic, ComicDto>(comic));
        }

        //GET api/admin/comics/{id}
        //Get a comic as admin
        [HttpGet]
        [Route("api/admin/comics/{id}")]
        public IHttpActionResult GetComicAsAdmin(int id)
        {
            var comic = Context.Comics.SingleOrDefault(c => c.Id == id);

            if (comic == null) return NotFound();

            return Ok(Mapper.Map<Comic, ComicDto>(comic));
        }

        //POST /api/comics/create
        //Create a new Comic Series
        [HttpPost]
        [Route("api/comics/create")]
        public IHttpActionResult CreateComic(UploadComicDto uploadComicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            

            var newComic = new Comic
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
                Status = false
            };

            //If the admin didn't put any author in the form, this comic will not be associated with any author 
            if (uploadComicDto.Authors != null)
                try
                {
                    foreach (var authorId in uploadComicDto.Authors)
                        newComic.Authors.Add(Context.Authors.Single(a => a.Id == authorId));
                }
                catch (Exception)
                {
                    return NotFound();
                }

            //If admin didn't put any tag in the form, this comic will not be associated with any tag 
            if (uploadComicDto.Tags != null)
                try
                {
                    foreach (var tagId in uploadComicDto.Tags)
                        newComic.Tags.Add(Context.Tags.Single(t => t.Id == tagId));
                }
                catch (Exception)
                {
                    return NotFound();
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

        //PUT api/comics/edit
        [HttpPut]
        [Route("api/comics/edit")]
        public IHttpActionResult EditComic(EditComicDto editComicDto)
        {
            //Check if the comic exist
            var comic = Context.Comics.SingleOrDefault(c => c.Id == editComicDto.Id);

            if (comic == null) return NotFound();

            //replace all the old information
            comic.Name = editComicDto.Name;
            comic.OtherName = editComicDto.OtherName;
            comic.Description = editComicDto.Description;
            comic.PublisherId = editComicDto.PublisherId;
            comic.IsActive = editComicDto.IsActive;
            comic.ThumbnailUrl = editComicDto.ThumbnailUrl;

            try
            {
                //clear the comic's author list
                comic.Authors.Clear();

                //clear the comic's tag list
                comic.Tags.Clear();

                //Add all authors again
                foreach (var author in editComicDto.Authors)
                {
                    //search if the author exist
                    var authorFromDb = Context.Authors.SingleOrDefault(a => a.Id == author);
                    if (authorFromDb == null) return NotFound();

                    comic.Authors.Add(authorFromDb);
                }

                //Add all tags again
                foreach (var tag in editComicDto.Tags)
                {
                    //check if the tag exist
                    var tagFromDb = Context.Tags.SingleOrDefault(a => a.Id == tag);
                    if (tagFromDb == null) return NotFound();

                    comic.Tags.Add(tagFromDb);
                }

                Context.SaveChanges();
                return Ok(Mapper.Map<Comic, ComicDto>(comic));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //GET api/comics/search/{searchValue}
        //Search for a comic
        [HttpGet]
        [Route("api/comics/search/{searchValue}")]
        public IHttpActionResult SearchComic(string searchValue)
        {
            //Go to all of the table to search for the comics
            var comic = from c in Context.Comics
                where c.Name.Contains(searchValue)
                      || c.Authors.Any(a => a.Name.Contains(searchValue))
                      || c.Publisher.Name.Contains(searchValue)
                      || c.Tags.Any(t => t.Name.Contains(searchValue)
                      || c.Authors.Any(a => a.Name.Contains(searchValue)))
                select c;

            if (!comic.Any()) return NotFound();

            return Ok(comic.ToList().Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //POST api/comics/rate
        //Rate a comic
        [HttpPost]
        [Route("api/comics/rate")]
        public IHttpActionResult RateComic(UploadRatingDto uploadRatingDto)
        {
            var comic = Context.Comics.SingleOrDefault(c => c.IsActive && c.Id == uploadRatingDto.ComicId);
            if (comic == null) return NotFound();

            var account = Context.Accounts.SingleOrDefault(a => a.IsActive && a.Id == uploadRatingDto.AccountId);
            if (account == null) return NotFound();

            var rating = account.RatingLists.SingleOrDefault(r =>
                r.AccountId == uploadRatingDto.AccountId && r.ComicId == uploadRatingDto.ComicId);

            //For when account already rated this comic, reset rating then add again below
            if (rating != null)
            {
                //For when there was only one person that rated this, simply return the point to zero
                if (comic.TimeRated == 1)
                {
                    comic.Rating = 0;
                    comic.TimeRated = 0;
                }
                else
                {
                    comic.Rating = ((comic.Rating * comic.TimeRated - rating.Rating) / --comic.TimeRated);
                }
            }

            try
            {
                var newRating = new RatingList()
                {
                    ComicId = comic.Id,
                    AccountId = account.Id,
                    Rating = uploadRatingDto.Rating
                };

                Context.RatingLists.Add(newRating);

                comic.Rating = (comic.Rating * comic.TimeRated + newRating.Rating) / ++comic.TimeRated;
                Context.SaveChanges();
                return Ok(Mapper.Map<RatingList, RatingDto>(newRating));
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
            if (account == null) return NotFound();

            var comic = Context.Comics.SingleOrDefault(c => c.IsActive && c.Id == favoriteDto.ComicId);
            if (comic == null) return NotFound();

            try
            {
                account.Comics.Add(comic);
                Context.SaveChanges();
                return Ok(favoriteDto);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //GET api/comics/viewFavorite/{accountId}
        [HttpGet]
        [Route("api/comics/viewFavorite/{accountId}")]
        public IHttpActionResult ViewFavoriteList(int accountId)
        {
            var account = Context.Accounts.SingleOrDefault(a => a.IsActive && a.Id == accountId);
            if (account == null) return NotFound();

            var favorites = account.Comics;
            if (!favorites.Any())
            {
                return NotFound();
            }

            return Ok(favorites.ToList().Select(Mapper.Map<Comic, LightWeightComicDto>));

        }

        //DELETE api/comics/favorite/delete
        [HttpDelete]
        [Route("api/comics/favorite/delete")]
        public IHttpActionResult DeleteFavorite(FavoriteDto favoriteDto)
        {
            var account = Context.Accounts.SingleOrDefault(a => a.IsActive && a.Id == favoriteDto.AccountId);
            if (account == null) return NotFound();

            var comic = Context.Comics.SingleOrDefault(c => c.IsActive && c.Id == favoriteDto.ComicId);
            if (comic == null) return NotFound();

            try
            {
                account.Comics.Remove(comic);
                Context.SaveChanges();
                return Ok();
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

            if (comic == null) return NotFound();

            comic.IsActive = !comic.IsActive;
            Context.SaveChanges();

            return Ok(Ok(Mapper.Map<Comic, ComicDto>(comic)));
        }
    }
}