using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ComicNow.DTOs.Comic;
using ComicNow.DTOs.Rating;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class ComicServices
    {
        public ComicNowEntities Context;

        public ComicServices()
        {
            Context = new ComicNowEntities();
        }

        public int GetComicId(Comic comic)
        {
            return comic.Id;
        }

        public List<Comic> GetAllActiveComic()
        {
            return (from comic in Context.Comics where comic.IsActive select comic).ToList();
        }

        public List<Comic> GetAllComic()
        {
            return Context.Comics.ToList();
        }

        public Comic GetActiveComicById(int comicId)
        {
            return Context.Comics.SingleOrDefault(comic => comic.IsActive && comic.Id == comicId);
        }

        public Comic GetComicById(int comicId)
        {
            return Context.Comics.SingleOrDefault(comic => comic.Id == comicId);
        }

        public List<Comic> SearchComicByTag(Tag tag)
        {
            return tag.Comics.ToList();
        }

        public List<Comic> SearchComicByAuthor(Author author)
        {
            return author.Comics.ToList();
        }

        public List<Comic> SearchComicByPublisher(Publisher publisher)
        {
            return publisher.Comics.ToList();
        }

        public Comic CreateComic(UploadComicDto uploadComicDto)
        {
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
                    var authorServices = new AuthorServices();

                    foreach (var authorId in uploadComicDto.Authors)
                        newComic.Authors.Add(authorServices.GetAuthorById(authorId));
                }
                catch (Exception)
                {
                    return null;
                }

            //If admin didn't put any tag in the form, this comic will not be associated with any tag 
            if (uploadComicDto.Tags != null)
                try
                {
                    var tagServices = new TagServices();

                    foreach (var tagId in uploadComicDto.Tags)
                        newComic.Tags.Add(tagServices.GetTagById(tagId));
                }
                catch (Exception)
                {
                    return null;
                }

            try
            {
                Context.Comics.Add(newComic);
                Context.SaveChanges();
                return newComic;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Comic EditComic(Comic comic, EditComicDto editComicDto)
        {

            //replace all the old information
            comic.Name = editComicDto.Name;
            comic.OtherName = editComicDto.OtherName;
            comic.Description = editComicDto.Description;
            comic.PublisherId = editComicDto.PublisherId;
            comic.IsActive = editComicDto.IsActive;
            comic.Status = editComicDto.Status;
            comic.ThumbnailUrl = editComicDto.ThumbnailUrl;

            try
            {
                //clear the comic's author list
                comic.Authors.Clear();

                //clear the comic's tag list
                comic.Tags.Clear();

                var authorServices = new AuthorServices();

                //Add all authors again
                foreach (var author in editComicDto.Authors)
                {
                    //search if the author exist
                    var authorFromDb = authorServices.GetAuthorById(author);
                    if (authorFromDb == null) return null;

                    comic.Authors.Add(authorFromDb);
                }

                var tagServices = new TagServices();

                //Add all tags again
                foreach (var tag in editComicDto.Tags)
                {
                    //check if the tag exist
                    var tagFromDb = tagServices.GetTagById(tag);
                    if (tagFromDb == null) return null;

                    comic.Tags.Add(tagFromDb);
                }

                Context.SaveChanges();
                return comic;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Comic> SearchComic(string searchValue)
        {
            //Go to all of the table to search for the comics
            var comic = from c in Context.Comics
                where c.Name.Contains(searchValue)
                      || c.Authors.Any(a => a.Name.Contains(searchValue))
                      || c.Publisher.Name.Contains(searchValue)
                      || c.Tags.Any(t => t.Name.Contains(searchValue)
                                         || c.Authors.Any(a => a.Name.Contains(searchValue)))
                select c;

            return !comic.Any() ? null : comic.ToList();
        }

        public RatingList RateComic(Comic comic, Account account, double point)
        {
            var ratingServices = new RatingServices();

            var rating = ratingServices.FindRatingList(account, comic);

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
                    Rating = point
                };

                Context.RatingLists.Add(newRating);

                comic.Rating = (comic.Rating * comic.TimeRated + newRating.Rating) / ++comic.TimeRated;
                Context.SaveChanges();
                return newRating;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool AddToFavorite(Account account, Comic comic)
        {
            try
            {
                account.Comics.Add(comic);
                Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Comic ChangeComicStatus(Comic comic)
        {
            try
            {
                comic.IsActive = !comic.IsActive;
                Context.SaveChanges();
                return comic;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}