namespace DolphinsSunsetResort.Models
{
    public class News
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime PublishedDate { get; set; }

        public int? ImageId { get; set; }
        public AppFile? Image { get; set; }

    }
}
