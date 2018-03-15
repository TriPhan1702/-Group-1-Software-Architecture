using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ComicNow.DTOs.Page;

namespace ComicNow.DTOs.Chapter
{
    public class UploadChapterDto
    {
        public int ComicId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        public System.DateTime PublishingDate { get; set; }
        
        public List<PageDto> Pages { get; set; }
    }
}