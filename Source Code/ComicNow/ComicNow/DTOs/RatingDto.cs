using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicNow.DTOs
{
    public class RatingDto
    {
        public int AccountId { get; set; }
        public int ComicId { get; set; }
        public double Rating { get; set; }
    }
}