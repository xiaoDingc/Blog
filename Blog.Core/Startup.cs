﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Common.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Blog.Core.Model.Seed;
using Blog.Core.Model.Seed.Blog.Core.Repository;

namespace Blog.Core
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Autofac.Extras.DynamicProxy;

    using Blog.Core.AOP;
    using Blog.Core.IServices;
    using Blog.Core.MemoryCacheHelper;
    using Blog.Core.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<ICaching, MemoryCaching>(); //记得把缓存注入！！！
            services.AddScoped<IRedisCacheManager, RedisCacheManager>(); //这里说下，如果是自己的项目，个人更建议使用单例模式 


            services.AddAutoMapper(typeof(Startup)); //这是AutoMapper的2.0新特性

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "Blog.Core API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Blog.Core",
                        Email = "Blog.Core@xxx.com",
                        Url = "https://www.jianshu.com/u/94102b59cc2a"
                    }
                });

                var baseDomin = AppDomain.CurrentDomain.BaseDirectory;
                var combinePath = Path.Join(baseDomin, "Blog.Core.xml");
                c.IncludeXmlComments(combinePath);

                var xmlModelPath = Path.Combine(baseDomin, "Blog.Core.Model.xml"); //这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);

                #region token

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {
                        "Blog.Core", new List<string>()
                        {
                        }
                    }
                };
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("Blog.Core", new ApiKeyScheme()
                {
                    Description = "JwtAuth",
                    Name = "Authorization",
                    In = "header", //jwt 默认存放authorization信息的位置(请求头中)
                    Type = "apiKey"
                });

                #endregion
            });

            #endregion

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                opt.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                opt.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin", "Client"));
                opt.AddPolicy("AdminAndClient", policy => policy.RequireRole("Admin").RequireRole("Client"));
            });


            #region 参数

            //读取配置文件
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            #endregion

            #region 跨域
            services.AddCors(x=>
            {
                x.AddPolicy("LimitRequest",policy=>
                {
                    //端口后不加斜杠 
                    // policy.WithOrigins(new string[]{"http://127.0.0.1:1818",
                    //         "https://localhost:8080",
                    //         "https://localhost:8021", "https://localhost:8081", "http://localhost:1818"})
                    // policy.AllowAnyOrigin()
                    //     .AllowAnyHeader()
                    //     .AllowAnyMethod();
                    policy
                    .AllowAnyOrigin()//允许任何源
                    .AllowAnyMethod()//允许任何方式
                    .AllowAnyHeader()//允许任何头
                    .AllowCredentials();//允许cookie
                });
            });
            #endregion

            #region token

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey, //参数配置在下边
                    ValidateIssuer = true,
                    ValidIssuer = audienceConfig["Issuer"], //发行人
                    ValidateAudience = true,
                    ValidAudience = audienceConfig["Audience"], //订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };
                o.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if (context == null)
                        {
                            throw new ArgumentNullException(nameof(context));
                        }

                        var res = context.Request.Query["token"];
                        return Task.CompletedTask;
                    }
                };
            });

            #endregion

            services.AddScoped<DBSeed>();
            services.AddScoped<MyContext>();

            #region autofac
            // 实例化autofac容器
            var builder = new ContainerBuilder();


            // builder.RegisterType(typeof(BlogCacheAOP));
            builder.RegisterType(typeof(BlogLogAOP));
            builder.RegisterType(typeof(BlogRedisCacheAOP));
            var assemblyServices = Assembly.Load("Blog.Core.Services");

            builder.RegisterAssemblyTypes(assemblyServices).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                                              // .InterceptedBy(typeof(BlogLogAOP));
                .InterceptedBy(typeof(BlogLogAOP), typeof(BlogRedisCacheAOP));

            var assemblyRepository = Assembly.Load("Blog.Core.Repository");
            // builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();
            builder.RegisterAssemblyTypes(assemblyRepository).AsImplementedInterfaces();

            // 将services填充到Autofac容器生成器中
            builder.Populate(services);

            // 使用已进行的组件登记创建新容器
            var applicationContainer = builder.Build(); 
            #endregion
            return new AutofacServiceProvider(applicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                    c.RoutePrefix = "";
                    // 将swagger首页，设置成我们自定义的页面，记得这个字符串的写法：解决方案名.index.html
                    // c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Blog.Core.Index.html");//这里是配合MiniProfiler进行性能监控的，《文章：完美基于AOP的接口性能分析》，如果你不需要，可以暂时先注释掉，不影响大局。
                });

                #endregion
            }
            app.UseCors("LimitRequest");

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}