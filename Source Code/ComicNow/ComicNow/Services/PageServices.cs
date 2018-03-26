using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class PageServices
    {
        public ComicNowEntities Context;

        public PageServices()
        {
            Context = new ComicNowEntities();
        }

        public int GetPagId(Page page)
        {
            return page.Id;
        }

        public Page GetPageById(int id)
        {
            return Context.Pages.SingleOrDefault(page => page.Id == id);
        }

        public Chapter GetAPageChapter(Page page)
        {
            return page.Chapter;
        }
    }
}