using System.ComponentModel.DataAnnotations;

namespace ComicNow.DTOs.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int ComicId { get; set; }
        public int AccountId { get; set; }
        public string Text { get; set; }
        public string AccountName { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime LastUpdate { get; set; }
    }
}