using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs.Comic
{
    public class ComicWithCommentListDto
    {
        public int Id { get; set; }
        public virtual List<CommentDto> Comments { get; set; }
    }
}