namespace BlogAPI.Models.DTOs
{
    public class UpdatePostDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Content
        {
            get; set;
        }
    }
}
