using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int ComicId { get; set; }
        public int AccountId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastUpdate { get; set; }
    }
}