using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs.Page
{
    public class ChangePositionPageDto
    {
        public int ChapterId { get; set; }
        public int SourcePosition { get; set; }
        public int DestinationPosition { get; set; }
    }
}