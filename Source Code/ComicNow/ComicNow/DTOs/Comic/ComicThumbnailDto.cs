namespace ComicNow.DTOs.Comic
{
    public class ComicThumbnailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int Views { get; set; }
        public string ThumbnailUrl { get; set; }
        public PublisherDto Publisher { get; set; }
    }
}