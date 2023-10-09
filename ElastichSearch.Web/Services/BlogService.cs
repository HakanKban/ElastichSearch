using ElastichSearch.Web.Models;
using ElastichSearch.Web.Repository;
using ElastichSearch.Web.ViewModel;

namespace ElastichSearch.Web.Services;
public class BlogService
{
    private readonly BlogRepository _blogRepository;

    public BlogService(BlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<bool> SaveAsync(BlogCreateViewModel model)
    {
        Blog blog = new();
        blog.UserId = Guid.NewGuid();
        blog.Title = model.Title;
        blog.Content = model.Content;
        blog.Tags = model.Tags.Split(",");

        var isCreated = await _blogRepository.SaveAsync(blog);
        return isCreated != null;
    }   
    
    public async Task<List<Blog>> SearchAsync(string searchTect)
    {
        return  await _blogRepository.SearcAsync(searchTect);
    }

}
