using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs.Chapter
{
    public class EditChapterDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        public System.DateTime PublishingDate { get; set; }
        public bool IsActive { get; set; }
    }
}