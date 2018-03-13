namespace ComicNow.DTOs.Account
{
    public class AccountDtoForAdmin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}