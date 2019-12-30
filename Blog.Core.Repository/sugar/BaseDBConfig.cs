namespace Blog.Core.Repository
{
    using System.IO;

    public class BaseDbConfig
    {
        // public static string ConnectionString = File.ReadAllText(@"D:\my-file\dbCountPsw1.txt").Trim();

       // 正常格式是

        public static string ConnectionString = "server=.;uid=sa;pwd=123;database=WMBlogDB";

        //原谅我用配置文件的形式，因为我直接调用的是我的服务器账号和密码，安全起见
    }
}