using System;
using System.Threading.Tasks;
using Blog.Core.AuthHelper;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    public class LoginController : Controller
    {
        // GET
        // public IActionResult Index()
        // {
        //     return View();
        // }
        [HttpGet]
        [Route("Token")]
        public async Task<object> GetJwtStr(string name, string pass)
        {
            string jwtStr=String.Empty;
            bool suc=false;
            if (name=="admins" && pass=="admins")
            {
                TokenModelJwt tokenModel=new TokenModelJwt();
                tokenModel.Uid=1;
                tokenModel.Role="Admin";
                jwtStr=JwtHelper.IssueJwt(tokenModel);
                suc=true;
            }
            else
            {
                jwtStr="Login Fail!!";
            }
            var result=new
            {
                data=new {success=suc,token=jwtStr}
            };
            return  Json(result);
      
        }
    }
}