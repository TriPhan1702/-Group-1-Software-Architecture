using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.DTOs.Comment;

namespace ComicNow.DTOs.Comic
{
    public class ComicWithCommentListDto
    {
        public int Id { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}