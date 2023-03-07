using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class UrlPair
    {
        [Display(Name = "long url")]
        [Key]
        public string LongUrl { get; set; }
        [Display(Name = "short url")]
        public string ShortUrl { get; set; }
        public string? User { get; set; }
        public List<Entry> Entries { get; set; } = new List<Entry>();
    }
}
