using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    using Blog.Core.IServices;
    using Blog.Core.Model;
    using Blog.Core.Services;

    using Microsoft.AspNetCore.Authorization;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(policy:"Admin")]
    public class BlogController : ControllerBase
    {
        private  readonly  IAdvertisementServices advertisementServices;

        public BlogController(IAdvertisementServices advertisementServices)
        {
            this.advertisementServices = advertisementServices;
        }
        // GET: api/Blog
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }
        
        /// <summary>
        /// Sum接口
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        // [AllowAnonymous]
        [HttpGet]
        public async Task<List<Advertisement>> Get(int id)
        {
            return await advertisementServices.Query(d => d.Id == id);
        } 

        // POST: api/Blog
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT: api/Blog/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
