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
using ComicNow.Services;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class ChaptersController : ApiController
    {
        public ChapterServices ChapterServices;

        public ComicServices ComicServices;

        public PageServices PageServices;

        public ChaptersController()
        {
            ChapterServices = new ChapterServices();
            
            ComicServices = new ComicServices();

            PageServices = new PageServices();
        }

        //GET /api/chapters/id
        //Get information about a chapter
        [HttpGet]
        public IHttpActionResult GetChapter(int id)
        {
            var chapter = ChapterServices.GetActiveChapterById(id);

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
            var chapter = ChapterServices.GetActiveChapterById(chapterId);

            if (chapter == null)
            {
                return NotFound();
            }

            var page = ChapterServices.GetPage(chapter, pageNumber);

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
            var chapter = ChapterServices.GetActiveChapterById(chapterId);
            if (chapter == null)
            {
                return NotFound();
            }

            var pages = ChapterServices.GetPages(chapter);

            return Ok(pages.Select(Mapper.Map<Page, PageDto>));
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

            var comic = ComicServices.GetComicById(uploadChapterDto.ComicId);

            if (comic == null)
            {
                return NotFound();
            }

            var chapter = ChapterServices.PostChapter(comic, uploadChapterDto);

            return Created(new Uri(Request.RequestUri + "/" + ChapterServices.GetChapterId(chapter)),
                    Mapper.Map<Chapter, ChapterDto>(chapter));
        }

        //PUT /api/chapters/changeChapterStatus/id
        //Activate/Deactivate a Chapter
        [HttpPut]
        [Route("api/chapters/changeChapterStatus/{id}")]
        public IHttpActionResult ChangeChapterStatus(int id)
        {
            var chapter = ChapterServices.GetChapterById(id);

            if (chapter == null)
            {
                return NotFound();
            }

            chapter = ChapterServices.ChangeChapterStatus(chapter);

            if (chapter == null)
            {
                return Conflict();
            }

            return Ok(Mapper.Map<Chapter, ChapterDto>(chapter)); ;
        }

        //PUT api/chapters/edit
        //Edit a chapter's information
        [HttpPut]
        [Route("api/chapters/edit")]
        public IHttpActionResult EditChapter(EditChapterDto editChapterDto)
        {
            var chapter = ChapterServices.GetChapterById(editChapterDto.Id);

            if (chapter == null)
            {
                return NotFound();
            }

            chapter = ChapterServices.EditChapter(chapter, editChapterDto);

            if (chapter == null)
            {
                return Conflict();
            }

            return Ok(Mapper.Map<Chapter, ChapterDto>(chapter));
        }

        //POST api/chapters/addPages
        //Add multiple pages to a chapter
        [HttpPost]
        [Route("api/chapters/addPages")]
        public IHttpActionResult AddPages(UploadMultiplePagesDto uploadMultiplePagesDto)
        {
            var chapter = ChapterServices.GetChapterById(uploadMultiplePagesDto.ChapterId);

            if (chapter == null)
            {
                return NotFound();
            }

            var count = chapter.Pages.Count;

            var resultList = ChapterServices.AddPages(chapter, uploadMultiplePagesDto);

            if (resultList == null)
            {
                return Conflict();
            }

            return Ok(resultList.Select(Mapper.Map<Page, PageDto>));
        }

        //POST api/chapters/addPage
        //Add one page to a chapter
        [HttpPost]
        [Route("api/chapters/addPage")]
        public IHttpActionResult AddPage(UploadPageDto uploadPageDto)
        {
            var chapter = ChapterServices.GetChapterById(uploadPageDto.ChapterId);

            if (chapter == null)
            {
                return NotFound();
            }

            var newPage = ChapterServices.AddPage(chapter, uploadPageDto);

            if (newPage == null)
            {
                return Conflict();
            }

            return Created(new Uri(Request.RequestUri + "/" + PageServices.GetPagId(newPage)), Mapper.Map<Page, PageDto>(newPage));
        }

        //PUT api/chapters/changePagePosition/{pageId}/{destinationPosition}
        //Take a page at a position and insert it to another position
        [HttpPut]
        [Route("api/chapters/changePagePosition/{pageId}/{destinationPosition}")]
        public IHttpActionResult ChangePagePosition(int pageId, int destinationPosition)
        {
            var page = PageServices.GetPageById(pageId);

            if (page == null)
            {
                return NotFound();
            }
            

            if (destinationPosition <0 || destinationPosition >= page.Chapter.Pages.Count)
            {
                return BadRequest();
            }

            var result = ChapterServices.ChangePagePosition(page, destinationPosition);

            if (!result.Any())
            {
                return Conflict();
            }
            
            return Ok(result.Select(Mapper.Map<Page, PageDto>));
        }

        //DELETE api/chapters/deletePage/{pageId}
        [HttpDelete]
        [Route("api/chapters/deletePage/{pageId}")]
        public IHttpActionResult DeletePage(int pageId)
        {
            var page = PageServices.GetPageById(pageId);

            if (page == null)
            {
                return NotFound();
            }

            var chapter = PageServices.GetAPageChapter(page);

            if (chapter == null)
            {
                return NotFound();
            }

            if (!ChapterServices.DeletePage(chapter, page))
            {
                return Conflict();
            }
            return Ok();
        }
    }
}
