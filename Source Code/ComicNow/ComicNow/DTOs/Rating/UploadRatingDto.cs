namespace ComicNow.DTOs.Rating
{
    public class UploadRatingDto
    {
        public int AccountId { get; set; }
        public int ComicId { get; set; }
        public double Rating { get; set; }
    }
}