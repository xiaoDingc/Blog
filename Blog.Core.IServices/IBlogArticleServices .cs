using Blog.Core.Model.ViewModels;

namespace Blog.Core.IServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Blog.Core.IServices.Base;
    using Blog.Core.Model.Models;

    public interface IBlogArticleServices:IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>>  getBlogs();
        Task<BlogViewModels>  getBlogDetails(int id);
    }
}