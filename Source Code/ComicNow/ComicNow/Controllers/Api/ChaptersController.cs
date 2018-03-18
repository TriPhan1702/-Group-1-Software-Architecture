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
        //Get information about a chapter
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
        [Route("api/chapters/{chapterId}/{pageNumber}")]
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

            var pages = from p in chapter.Pages orderby p.PageNumber select p;

            if (!pages.Any())
            {
                return NotFound();
            }

            return Ok(chapter.Pages.ToList().Select(Mapper.Map<Page, PageDto>));
        }


        //POST /api/chapters/create
        //Create a new chapter
        [HttpPost]
        [Route("api/chapters/create")]
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

        //PUT api/chapters/edit
        //Edit a chapter's information
        [HttpPut]
        [Route("api/chapters/edit")]
        public IHttpActionResult EditChapter(EditChapterDto editChapterDto)
        {
            var chapter = Context.Chapters.SingleOrDefault(c => c.Id == editChapterDto.Id);

            if (chapter == null)
            {
                return NotFound();
            }
            
            try
            {
                chapter.Name = editChapterDto.Name;
                chapter.PublishingDate = editChapterDto.PublishingDate;
                chapter.IsActive = editChapterDto.IsActive;
                chapter.LastUpdated = DateTime.Now;
                Context.SaveChanges();
                return Ok(Mapper.Map<Chapter, ChapterDto>(chapter));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //POST api/chapters/addPages
        //Add multiple pages to a chapter
        [HttpPost]
        [Route("api/chapters/addPages")]
        public IHttpActionResult AddPages(UploadMultiplePagesDto uploadMultiplePagesDto)
        {
            var chapter = Context.Chapters.SingleOrDefault(c => c.Id == uploadMultiplePagesDto.ChapterId);

            if (chapter == null)
            {
                return NotFound();
            }

            var count = chapter.Pages.Count;

            var resultList = new List<PageDto>();

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
                    resultList.Add(Mapper.Map<Page, PageDto>(newPage));
                }
                chapter.PageNumber = count;
                Context.SaveChanges();
                return Ok(resultList);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //POST api/chapters/addPage
        //Add one page to a chapter
        [HttpPost]
        [Route("api/chapters/addPage")]
        public IHttpActionResult AddPage(UploadPageDto uploadPageDto)
        {
            var chapter = Context.Chapters.SingleOrDefault(c => c.Id == uploadPageDto.ChapterId);

            if (chapter == null)
            {
                return NotFound();
            }
            
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
                return Created(new Uri(Request.RequestUri + "/" + newPage.Id), Mapper.Map<Page, PageDto>(newPage));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //PUT api/chapters/changePagePosition/{pageId}/{destinationPosition}
        //Take a page at a position and insert it to another position
        [HttpPut]
        [Route("api/chapters/changePagePosition/{pageId}/{destinationPosition}")]
        public IHttpActionResult ChangePagePosition(int pageId, int destinationPosition)
        {
            var page = Context.Pages.SingleOrDefault(p => p.Id == pageId);

            if (page == null)
            {
                return NotFound();
            }
            

            if (destinationPosition <0 || destinationPosition >= page.Chapter.Pages.Count)
            {
                return BadRequest();
            }

            int operation;

            List<Page> pagesInRange;

            if (page.PageNumber > destinationPosition)
            {
                operation = 1;
                pagesInRange = (from p in page.Chapter.Pages where p.PageNumber >= destinationPosition && p.PageNumber < page.PageNumber select p).ToList();
            }
            else
            {
                operation = -1;
                pagesInRange = (from p in page.Chapter.Pages where p.PageNumber < destinationPosition && p.PageNumber >= page.PageNumber select p).ToList();
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
                    return NotFound();
                }

                desPage.PageNumber = page.PageNumber;
            }
            page.PageNumber = destinationPosition;

            try
            {
                Context.SaveChanges();
                return Ok(page.Chapter.Pages.Select(Mapper.Map<Page, PageDto>).ToList());
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //DELETE api/chapters/deletePage/{pageId}
        [HttpDelete]
        [Route("api/chapters/deletePage/{pageId}")]
        public IHttpActionResult DeletePage(int pageId)
        {
            var page = Context.Pages.SingleOrDefault(p => p.Id == pageId);

            if (page == null)
            {
                return NotFound();
            }

            var chapter = page.Chapter;
            if (chapter == null)
            {
                return NotFound();
            }

            var pagesInRange = from p in chapter.Pages where p.PageNumber > page.PageNumber select p;
            
                foreach (var p in pagesInRange)
                {
                    p.PageNumber -= 1;
                }

                Context.Pages.Remove(page);
                Context.SaveChanges();
                return Ok();
        }
    }
}
