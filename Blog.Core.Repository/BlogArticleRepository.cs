namespace Blog.Core.Repository
{
    using Blog.Core.IRepository;
    using Blog.Core.IRepository.Base;
    using Blog.Core.Model.Models;
    using Blog.Core.Repository.Base;

    public class BlogArticleRepository:BaseRepository<BlogArticle>,IBlogArticleRepository
    {
        
    }
}