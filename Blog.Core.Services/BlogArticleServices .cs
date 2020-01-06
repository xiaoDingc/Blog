using System;
using System.Linq;

namespace Blog.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Core.Common;
    using Blog.Core.IRepository;
    using Blog.Core.IServices;
    using Blog.Core.IServices.Base;
    using Blog.Core.Model.Models;
    using Blog.Core.Model.ViewModels;
    using Blog.Core.Services.Base;

    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IBlogArticleRepository dal;
        IMapper iMapper;

        public BlogArticleServices(IBlogArticleRepository dal,IMapper imapper)
        {
            this.dal = dal;
            this.baseDal = dal;
            iMapper=imapper;
        }


        public async Task<BlogViewModels> getBlogDetails(int id)
        {
              var bloglist = await dal.Query(a => a.bID > 0, a => a.bID);
            var blogArticle = (await dal.Query(a => a.bID == id)).FirstOrDefault();
            BlogViewModels models = null;

            if (blogArticle != null)
            {
                BlogArticle prevblog;
                BlogArticle nextblog;
                int blogIndex = bloglist.FindIndex(item => item.bID == id);
                if (blogIndex >= 0)
                {
                    try
                    {
                        // 上一篇
                        prevblog = blogIndex > 0 ? (((BlogArticle)(bloglist[blogIndex - 1]))) : null;
                        // 下一篇
                        nextblog = blogIndex + 1 < bloglist.Count() ? (BlogArticle)(bloglist[blogIndex + 1]) : null;

                        // 注意就是这里,mapper
                        models = iMapper.Map<BlogViewModels>(blogArticle);

                        if (nextblog != null)
                        {
                            models.next = nextblog.btitle;
                            models.nextID = nextblog.bID;
                        }
                        if (prevblog != null)
                        {
                            models.previous = prevblog.btitle;
                            models.previousID = prevblog.bID;
                        }
                    }
                    catch (Exception) { }
                }
                blogArticle.btraffic += 1;
                await dal.Update(blogArticle, new List<string> { "btraffic" });
            }

            return models;
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