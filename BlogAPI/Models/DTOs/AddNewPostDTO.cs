namespace BlogAPI.Models.DTOs
{
    public class AddNewPostDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public int blogId { get; set; }
    }
}
