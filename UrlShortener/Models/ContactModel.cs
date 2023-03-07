
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
