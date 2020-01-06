using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Common.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    using Blog.Core.IServices;
    using Blog.Core.Model;
    using Blog.Core.Model.Models;
    using Blog.Core.Services;
    using Microsoft.AspNetCore.Authorization;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy:"Admin")]
    public class BlogController : ControllerBase
    {
        private readonly IAdvertisementServices advertisementServices;
        private readonly IBlogArticleServices blogArticleServices;
        private readonly IRedisCacheManager redisCacheManager;

        public BlogController(IAdvertisementServices advertisementServices, IBlogArticleServices blogArticleServices, IRedisCacheManager redisCacheManager)
        {
            this.advertisementServices = advertisementServices;
            this.blogArticleServices = blogArticleServices;
            this.redisCacheManager = redisCacheManager;
        }

        // GET: api/Blog
        [HttpGet("{id}", Name = "Get")]
        public async Task<Object> Get(int id)
        {
            var model = await blogArticleServices.getBlogDetails(id);
            var data = new
            {
                success = true,
                data = model
            };
            return data;
        }

        /// <summary>
        /// Sum接口
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        // [AllowAnonymous]
        // [HttpGet]
        // public async Task<List<Advertisement>> Get(int id)
        // {
        //     return await advertisementServices.Query(d => d.Id == id);
        // } 

        // POST: api/Blog
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpGet]
        [Route("GetBlogs")]
        public async Task<List<BlogArticle>> getBlogs()
        {
            var connetct = Appsettings.app(new string[]
            {
                "AppSettings",
                "RedisCaching",
                "ConnectionString"
            });
            List<BlogArticle> blogArticles = new List<BlogArticle>();
            if (redisCacheManager.Get<Object>("Redis.Blog") != null)
            {
                blogArticles = redisCacheManager.Get<List<BlogArticle>>("Redis.Blog");
            }
            else
            {
                blogArticles = await this.blogArticleServices.getBlogs();
                redisCacheManager.Set("Redis.Blog", blogArticles, TimeSpan.FromHours(2));
            }

            return blogArticles;
        }

        // PUT: api/Blog/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}