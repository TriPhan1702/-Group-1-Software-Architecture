using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs.Chapter
{
    public class EditChapterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime PublishingDate { get; set; }
        public bool IsActive { get; set; }
    }
}