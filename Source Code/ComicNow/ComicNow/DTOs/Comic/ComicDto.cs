using System.Collections.Generic;
using ComicNow.DTOs.Chapter;
using ComicNow.Models;

namespace ComicNow.DTOs.Comic
{
    public class ComicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OtherName { get; set; }
        public string Description { get; set; }
        public int ChapterNumber { get; set; }
        public bool Status { get; set; }
        public double Rating { get; set; }
        public int TimeRated { get; set; }
        public int Views { get; set; }
        public string ThumbnailUrl { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastUpdate { get; set; }

        public List<ChapterDto> Chapters { get; set; }
        public PublisherDto Publisher { get; set; }
        public List<AuthorDto> Authors { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}