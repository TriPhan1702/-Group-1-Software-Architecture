using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ComicNow.DTOs.Chapter;

namespace ComicNow.Controllers.Api
{
    public class ChaptersController : ApiController
    {
        //POST /api/chapters
        [HttpPost]
        public IHttpActionResult PostChapter(UploadChapterDto uploadChapterDto)
        {

        }
    }
}
