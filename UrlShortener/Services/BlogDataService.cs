using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public class BlogDataService
    {
        private readonly DataContext _data;
        public BlogDataService(DataContext data)
        {
            _data = data;
        }
        public IEnumerable<BlogPostModel> GetAllPosts()
        {
            return _data.BlogPosts;
        }
        public BlogPostModel GetPost(int? id)
        {
            var post = _data.BlogPosts.FirstOrDefault(u => u.Id == id);
            return post;
        }
        public void AddPost(BlogPostModel post)
        {
            _data.BlogPosts.Add(post);
            _data.SaveChanges();
        }
        public void UpdatePost(int id, BlogPostModel post)
        {
            var _post = GetPost(id);
            if(_post != null)
            {
                _post = post;
                _data.SaveChanges();
            }
        }
        public void DeletePost(int id)
        {
            var itemToDelete = _data.BlogPosts.FirstOrDefault(u => u.Id == id);
            if(itemToDelete != null)
            {
                _data.BlogPosts.Remove(itemToDelete);
                _data.SaveChanges();
            }
        }
    }
}
