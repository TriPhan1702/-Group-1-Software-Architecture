using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs.Comment
{
    public class PostCommentDto
    {
        public int ComicId { get; set; }
        public int AccountId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }
    }
}