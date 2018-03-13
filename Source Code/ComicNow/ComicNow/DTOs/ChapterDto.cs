using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs
{
    public class ChapterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double OrderNumber { get; set; }
        public int PageNumber { get; set; }
        public System.DateTime PublishingDate { get; set; }
    }
}