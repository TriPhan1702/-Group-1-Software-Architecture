using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComicNow.DTOs.Comic
{
    public class UploadComicDto
    {
        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string OtherName { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        public int PublisherId { get; set; }
        
        public string ThumbnailUrl { get; set; }

        public List<int> Authors { get; set; }
        public List<int> Tags { get; set; }
    }
}