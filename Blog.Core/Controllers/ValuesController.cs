﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [ApiExplorerSettings(IgnoreApi = true)]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// 获取id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
