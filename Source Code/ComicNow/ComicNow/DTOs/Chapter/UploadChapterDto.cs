using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.DTOs.Page;

namespace ComicNow.DTOs.Chapter
{
    public class UploadChapterDto
    {
        public int ComicId { get; set; }
        public string Name { get; set; }
        public System.DateTime PublishingDate { get; set; }
        public int PageNumber { get; set; }
        
        public List<UploadPageDto> Pages { get; set; }
    }
}