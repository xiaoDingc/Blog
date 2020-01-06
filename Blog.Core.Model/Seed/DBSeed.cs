using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Model.Seed.Blog.Core.Repository;

namespace Blog.Core.Model.Seed
{
    public class DBSeed
    {
        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="myContext"></param>
        /// <returns></returns>
        public  static  async  Task SeedAsync(MyContext myContext)
        {
            try
            {
                myContext.CreateTableByEntity(false,typeof(Advertisement));

                //种子数据
                if (! await myContext.Db.Queryable<Advertisement>().AnyAsync())
                {
                    myContext.GetEntityDB<Advertisement>().InsertRange(new List<Advertisement>()
                    {
                        new Advertisement()
                        {
                             Createdate = DateTime.Now,
                           Remark = "mark",
                           Title = "good"
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}