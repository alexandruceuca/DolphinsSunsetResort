namespace DolphinsSunsetResort.Models
{
    public class AppFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }


		public News News { get; set; }

		public MenuItem MenuItem { get; set; }
	}
}
