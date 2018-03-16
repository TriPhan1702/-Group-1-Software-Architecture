namespace ComicNow.DTOs.Page
{
    public class PageDto
    {
        public int Id { get; set; }
        public int ComicId { get; set; }
        public int ChapterId { get; set; }
        public string FileName { get; set; }
        public string URL { get; set; }
        public int PageNumber { get; set; }
    }
}