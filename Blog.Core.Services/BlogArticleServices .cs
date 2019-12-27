namespace Blog.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Core.Common;
    using Blog.Core.IRepository;
    using Blog.Core.IServices;
    using Blog.Core.IServices.Base;
    using Blog.Core.Model.Models;
    using Blog.Core.Services.Base;

    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IBlogArticleRepository dal;

        public BlogArticleServices(IBlogArticleRepository dal)
        {
            this.dal = dal;
            this.baseDal = dal;
        }

   
        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]//增加特性
        public async Task<List<BlogArticle>> getBlogs()
        {
            var bloglist = await dal.Query(a => a.bID > 0, a => a.bID);

            return bloglist;
        }
    }
}