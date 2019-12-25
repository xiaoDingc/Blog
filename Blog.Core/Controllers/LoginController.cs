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
        public async Task<object> GetJwtStr(string name, string pass)
        {
            TokenModelJwt tokenModelJwt = new TokenModelJwt()
            {
                Role = name,
                Work = pass
            };
            var jwtStr = JwtHelper.IssueJwt(tokenModelJwt);
            var suc = true;
            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }
    }
}