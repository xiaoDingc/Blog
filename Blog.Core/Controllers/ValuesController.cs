using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    using Blog.Core.IServices;

    [Route("api/[controller]")]
    [ApiController]
    // [ApiExplorerSettings(IgnoreApi = true)]
    public class ValuesController : ControllerBase
    {
        private  readonly  IAdvertisementServices advertisementServices;

        public ValuesController(IAdvertisementServices advertisementServices)
        {
            this.advertisementServices = advertisementServices;
        }

        /// <summary>
        /// The get.日志测试
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<List<Advertisement>> Get(int id)
        {
            return await advertisementServices.Query(d => d.Id == id);
        } 



        /// <summary>
       /// post
       /// </summary>
       /// <param name="love">model实体类参数</param>
        [HttpPost]
        public void Post(Love love)
        {

        }

        // POST api/values
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
