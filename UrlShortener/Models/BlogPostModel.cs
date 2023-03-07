using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class BlogPostModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string ImgSrc { get; set; }
    }
}
