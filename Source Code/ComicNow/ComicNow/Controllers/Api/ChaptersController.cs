using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using ComicNow.DTOs.Chapter;
using ComicNow.DTOs.Page;
using ComicNow.Models;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class ChaptersController : ApiController
    {
        public ComicNowEntities Context;

        public ChaptersController()
        {
            Context = new ComicNowEntities();
        }

        //GET /api/chapters/id
        //Get an information about a chapter
        [HttpGet]
        public IHttpActionResult GetChapter(int id)
        {
            var chapter = Context.Chapters.SingleOrDefault(c => c.Id == id && c.IsActive);

            if (chapter == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Chapter, ChapterDto>(chapter));
        }
        
        //GET /api/chapters/chapterId/pageNumber
        //Get a page with the chapterId and the page's number (not pageId)
        [HttpGet]
        public IHttpActionResult GetPage(int chapterId, int pageNumber)
        {
            var chapter = Context.Chapters.SingleOrDefault(c => c.Id == chapterId && c.IsActive);
            if (chapter == null)
            {
                return NotFound();
            }

            var page = chapter.Pages.SingleOrDefault(p => p.PageNumber == pageNumber);
            if (page == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Page, PageDto>(page));
        }

        //GET /api/chapters/chapterId/pageList
        //Get the list of all the pages of a chapter
        [HttpGet]
        [Route("api/chapters/{chapterId}/pageList")]
        public IHttpActionResult GetPages(int chapterId)
        {
            var chapter = Context.Chapters.SingleOrDefault(c => c.Id == chapterId && c.IsActive);
            if (chapter == null)
            {
                return NotFound();
            }

            var pages = chapter.Pages;

            if (!pages.Any())
            {
                return NotFound();
            }

            return Ok(chapter.Pages.ToList().Select(Mapper.Map<Page, PageDto>));
        }


        //POST /api/chapters
        //Create a new chapter with all of its pages
        [HttpPost]
        public IHttpActionResult PostChapter(UploadChapterDto uploadChapterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var comic = Context.Comics.SingleOrDefault(c => c.Id == uploadChapterDto.ComicId);

            if (comic == null)
            {
                return NotFound();
            }

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

            if (uploadChapterDto.Pages.Any())
            {
                var pageCount = 0;
                foreach (var page in uploadChapterDto.Pages)
                {
                    var tempPage = new Page()
                    {
                        Chapter = chapter,
                        Comic = comic,
                        FileName = page.FileName,
                        URL = page.URL,
                        PageNumber = pageCount,
                    };
                    chapter.Pages.Add(tempPage);
                    pageCount++;
                }

                chapter.PageNumber = pageCount;
            }
           
            try
            {
                comic.Chapters.Add(chapter);
                Context.SaveChanges();
                return Created(new Uri(Request.RequestUri + "/" + chapter.Id),
                    Mapper.Map<Chapter, ChapterDto>(chapter));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //PUT /api/chapters/changeChapterStatus/id
        //Activate/Deactivate a Chapter
        [HttpPut]
        [Route("api/chapters/changeChapterStatus/{id}")]
        public IHttpActionResult ChangeChapterStatus(int id)
        {
            var chapter = Context.Chapters.SingleOrDefault(c => c.Id == id);

            if (chapter == null)
            {
                return NotFound();
            }

            chapter.IsActive = !chapter.IsActive;
            Context.SaveChanges();

            return Ok(Mapper.Map<Chapter, ChapterDto>(chapter)); ;
        }
    }
}
