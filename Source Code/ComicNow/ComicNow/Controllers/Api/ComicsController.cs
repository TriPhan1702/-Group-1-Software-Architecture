using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.DTOs.Comic;
using ComicNow.DTOs.Rating;
using ComicNow.Models;
using ComicNow.Services;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class ComicsController : ApiController
    {
        public ComicServices ComicServices;

        public AccountServices AccountServices;

        public ComicsController()
        {
            ComicServices = new ComicServices();

            AccountServices = new AccountServices();
        }

        //GET /api/comics
        //Get a list of active Comic thumbnails
        [HttpGet]
        public IHttpActionResult GetThumbnails()
        {
            var comics = ComicServices.GetAllActiveComic();

            if (!comics.Any()) return NotFound();

            return Ok(comics.Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //GET /api/admin/comics
        //Get list of all comics as admin
        [HttpGet]
        [Route("api/admin/comics")]
        public IHttpActionResult GetComics()
        {
            var comics = ComicServices.GetAllComic();

            if (!comics.Any()) return NotFound();

            return Ok(comics.Select(Mapper.Map<Comic, LightWeightComicDto>));
        }

        //Get /api/comics/id
        //Get an active comic based on id
        [HttpGet]
        public IHttpActionResult GetComic(int id)
        {
            var comic = ComicServices.GetActiveComicById(id);

            if (comic == null) return NotFound();

            return Ok(Mapper.Map<Comic, ComicDto>(comic));
        }

        //GET api/admin/comics/{id}
        //Get a comic as admin
        [HttpGet]
        [Route("api/admin/comics/{id}")]
        public IHttpActionResult GetComicAsAdmin(int id)
        {
            var comic = ComicServices.GetComicById(id);

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
            
            var newComic = ComicServices.CreateComic(uploadComicDto);

            if (newComic == null)
            {
                return Conflict();
            }

            return Created(new Uri(Request.RequestUri + "/" + ComicServices.GetComicId(newComic)), Mapper.Map<Comic, ComicDto>(newComic));
        }

        //PUT api/comics/edit
        [HttpPut]
        [Route("api/comics/edit")]
        public IHttpActionResult EditComic(EditComicDto editComicDto)
        {
            //Check if the comic exist
            var comic = ComicServices.GetComicById(editComicDto.Id);

            if (comic == null) return NotFound();

            comic = ComicServices.EditComic(comic, editComicDto);

            if (comic == null)
            {
                return Conflict();
            }

            return Ok(Mapper.Map<Comic, ComicDto>(comic));
        }

        //GET api/comics/search/{searchValue}
        //Search for a comic
        [HttpGet]
        [Route("api/comics/search/{searchValue}")]
        public IHttpActionResult SearchComic(string searchValue)
        {
            //Go to all of the table to search for the comics
            var comic = ComicServices.SearchComic(searchValue);

            if (!comic.Any()) return NotFound();

            return Ok(comic.Select(Mapper.Map<Comic, ComicThumbnailDto>));
        }

        //POST api/comics/rate
        //Rate a comic
        [HttpPost]
        [Route("api/comics/rate")]
        public IHttpActionResult RateComic(UploadRatingDto uploadRatingDto)
        {
            var comic = ComicServices.GetActiveComicById(uploadRatingDto.ComicId);
            if (comic == null) return NotFound();

            var account = AccountServices.GetActiveAccountById(uploadRatingDto.AccountId);
            if (account == null) return NotFound();

            var rating = ComicServices.RateComic(comic, account, uploadRatingDto.Rating);

            if (rating == null)
            {
                return Conflict();
            }

            return Ok(Mapper.Map<RatingList, RatingDto>(rating));
        }

        //POST api/comics/favorite
        [HttpPost]
        [Route("api/comics/favorite")]
        public IHttpActionResult AddToFavorite(FavoriteDto favoriteDto)
        {
            var account = AccountServices.GetActiveAccountById(favoriteDto.AccountId);
            if (account == null) return NotFound();

            var comic = ComicServices.GetActiveComicById(favoriteDto.ComicId);
            if (comic == null) return NotFound();

            if (!ComicServices.AddToFavorite(account, comic))
            {
                return Conflict();
            }

            return Ok(favoriteDto);

        }

        //GET api/comics/viewFavorite/{accountId}
        [HttpGet]
        [Route("api/comics/viewFavorite/{accountId}")]
        public IHttpActionResult ViewFavoriteList(int accountId)
        {
            var account = AccountServices.GetActiveAccountById(accountId);
            if (account == null) return NotFound();

            var favorites = AccountServices.ViewFavoriteList(account);

            if (!favorites.Any())
            {
                return NotFound();
            }

            return Ok(favorites.Select(Mapper.Map<Comic, LightWeightComicDto>));

        }

        //DELETE api/comics/favorite/delete
        [HttpDelete]
        [Route("api/comics/favorite/delete")]
        public IHttpActionResult DeleteFavorite(FavoriteDto favoriteDto)
        {
            var account = AccountServices.GetActiveAccountById(favoriteDto.AccountId);
            if (account == null) return NotFound();

            var comic = ComicServices.GetActiveComicById(favoriteDto.ComicId);
            if (comic == null) return NotFound();

            if (!AccountServices.DeleteFavorite(account, comic))
            {
                return Conflict();
            }
            return Ok();
        }

        //PUT /api/comics/changeComicStatus/comicId
        //Activate/Deactivate a Comic
        [HttpPut]
        [Route("api/comics/changeComicStatus/{comicId}")]
        public IHttpActionResult ChangeComicStatus(int comicId)
        {
            var comic = ComicServices.GetComicById(comicId);

            if (comic == null) return NotFound();

            comic = ComicServices.ChangeComicStatus(comic);

            if (comic == null)
            {
                return Conflict();
            }

            return Ok(Mapper.Map<Comic, LightWeightComicDto>(comic));
        }
    }
}