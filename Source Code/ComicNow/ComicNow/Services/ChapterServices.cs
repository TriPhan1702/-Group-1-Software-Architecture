using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ComicNow.DTOs.Chapter;
using ComicNow.DTOs.Page;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class ChapterServices
    {
        public ComicNowEntities Context;

        public ChapterServices()
        {
            Context = new ComicNowEntities();
        }

        public Chapter GetActiveChapterById(int id)
        {
            return Context.Chapters.SingleOrDefault(chapter => chapter.IsActive && chapter.Id == id);
        }

        public Chapter GetChapterById(int id)
        {
            return Context.Chapters.SingleOrDefault(chapter => chapter.Id == id);
        }

        public Page GetPage(Chapter chapter, int pageNumber)
        {
            return chapter.Pages.SingleOrDefault(p => p.PageNumber == pageNumber);
        }

        public List<Page> GetPages(Chapter chapter)
        {
            return (from p in chapter.Pages orderby p.PageNumber select p).ToList();
        }

        public int GetChapterId(Chapter chapter)
        {
            return chapter.Id;
        }

        public Chapter PostChapter(Comic comic, UploadChapterDto uploadChapterDto)
        {

            var chapter = new Chapter()
            {
                Comic = comic,
                Name = uploadChapterDto.Name,
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                PublishingDate = uploadChapterDto.PublishingDate,
                IsActive = true,
                PageNumber = 0
            };

            try
            {
                comic.Chapters.Add(chapter);
                comic.ChapterNumber++;
                Context.SaveChanges();
                return chapter;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Chapter ChangeChapterStatus(Chapter chapter)
        {
            try
            {
                chapter.IsActive = !chapter.IsActive;

                //Change the comic's Page number for the users to see
                if (chapter.IsActive)
                {
                    chapter.Comic.ChapterNumber++;
                }
                else
                {
                    chapter.Comic.ChapterNumber--;
                }
                Context.SaveChanges();
                return chapter;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Chapter EditChapter(Chapter chapter, EditChapterDto editChapterDto)
        {
            try
            {
                chapter.Name = editChapterDto.Name;
                chapter.PublishingDate = editChapterDto.PublishingDate;
                chapter.IsActive = editChapterDto.IsActive;
                chapter.LastUpdated = DateTime.Now;
                Context.SaveChanges();
                return chapter;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Page> AddPages(Chapter chapter, UploadMultiplePagesDto uploadMultiplePagesDto)
        {
            var count = chapter.Pages.Count;

            var resultList = new List<Page>();

            try
            {

                foreach (var page in uploadMultiplePagesDto.Pages)
                {
                    var newPage = new Page()
                    {
                        ChapterId = chapter.Id,
                        ComicId = chapter.ComicId,
                        FileName = page.FileName,
                        PageNumber = count++,
                        URL = page.URL,
                    };
                    chapter.Pages.Add(newPage);
                    resultList.Add(newPage);
                }
                chapter.PageNumber = count;
                Context.SaveChanges();
                return resultList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Page AddPage(Chapter chapter, UploadPageDto uploadPageDto)
        {
            var newPage = new Page()
            {
                ChapterId = chapter.Id,
                ComicId = chapter.ComicId,
                FileName = uploadPageDto.FileName,
                PageNumber = chapter.Pages.Count,
                URL = uploadPageDto.URL,
            };

            try
            {
                chapter.Pages.Add(newPage);
                chapter.PageNumber = chapter.Pages.Count;
                Context.SaveChanges();
                return newPage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Page> ChangePagePosition(Page page, int destinationPosition)
        {
            int operation;

            List<Page> pagesInRange;

            //Move down, select des <= 
            if (page.PageNumber > destinationPosition)
            {
                operation = 1;
                pagesInRange = (from p in page.Chapter.Pages where p.PageNumber >= destinationPosition && p.PageNumber < page.PageNumber select p).ToList();
            }
            //Move up
            else
            {
                operation = -1;
                pagesInRange = (from p in page.Chapter.Pages where p.PageNumber <= destinationPosition && p.PageNumber > page.PageNumber select p).ToList();
            }

            if (pagesInRange.Count > 0)
            {

                foreach (var p in pagesInRange)
                {
                    p.PageNumber += operation;
                }
            }
            else
            {
                var desPage = page.Chapter.Pages.SingleOrDefault(p => p.PageNumber == destinationPosition);
                if (desPage == null)
                {
                    return null;
                }

                desPage.PageNumber = page.PageNumber;
            }
            page.PageNumber = destinationPosition;

            try
            {
                Context.SaveChanges();
                return page.Chapter.Pages.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeletePage(Chapter chapter, Page page)
        {
            var pagesInRange = from p in chapter.Pages where p.PageNumber > page.PageNumber select p;

            try
            {
                foreach (var p in pagesInRange)
                {
                    p.PageNumber -= 1;
                }

                Context.Pages.Remove(page);
                Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}