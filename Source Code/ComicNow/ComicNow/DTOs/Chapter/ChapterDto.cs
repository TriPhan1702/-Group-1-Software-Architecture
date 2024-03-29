﻿namespace ComicNow.DTOs.Chapter
{
    public class ChapterDto
    {
        public int Id { get; set; }
        public int ComicId { get; set; }
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public System.DateTime PublishingDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastUpdated { get; set; }
    }
}