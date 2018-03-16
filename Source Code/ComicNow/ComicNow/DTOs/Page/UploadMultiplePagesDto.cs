using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs.Page
{
    public class UploadMultiplePagesDto
    {
        public int ChapterId { get; set; }

        public List<UploadLightweightPagesDto> Pages { get; set; }
    }
}