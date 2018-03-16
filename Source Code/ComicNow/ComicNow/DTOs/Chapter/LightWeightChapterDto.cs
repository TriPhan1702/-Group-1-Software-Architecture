using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs.Chapter
{
    public class LightWeightChapterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime PublishingDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastUpdated { get; set; }
        public bool IsActive { get; set; }
        public int PageNumber { get; set; }
    }
}