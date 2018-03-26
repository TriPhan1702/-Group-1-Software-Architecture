using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class RatingServices
    {
        public ComicNowEntities Context;

        public RatingServices()
        {
            Context = new ComicNowEntities();
        }

        public RatingList FindRatingList (Account account, Comic comic)
        {
            return account.RatingLists.SingleOrDefault(r =>
                r.AccountId == account.Id && r.ComicId == comic.Id);
        }
    }
}