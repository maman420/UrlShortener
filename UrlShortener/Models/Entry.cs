using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class Entry
    {
        [Key]
        public DateTime EntryDate { get; set; } = DateTime.Now;
        public string Ip { get; set; }
    }
}
