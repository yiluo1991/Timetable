using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using Timetable.Security;
using Timetable.Web.CommonViewModel;

namespace Timetable.Web.OA
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            //不使用驼峰样式的key
            services.AddControllers().AddNewtonsoftJson(options =>
            { 
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); 
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
              {
                    /***
                     * 自定义重定向处理，不返回重定向，而是返回403的一个JSON信息
                     * */
                  option.Events.OnRedirectToLogin = context =>
                  {
                      context.Response.StatusCode = 403;
                      context.Response.ContentType = "application/json; charset=utf-8";
                      return context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new ErrorResponse
                      {
                          success = false,
                          errCode = "ERR_AUTH_INVALID",
                          msg = "未登录或无权访问该接口"
                      }));
                  };
                  option.Cookie.Name = "ttboa";//设置存储用户登录信息（用户Token信息）的Cookie名称
                    option.Cookie.HttpOnly = true;//设置存储用户登录信息（用户Token信息）的Cookie，无法通过客户端浏览器脚本(如JavaScript等)访问到
                    option.SlidingExpiration = true;
                  option.Cookie.SameSite = SameSiteMode.Strict;
                  option.ExpireTimeSpan = TimeSpan.FromHours(48); 
           
                });


            var redis = ConnectionMultiplexer.Connect(WebConfigDES.ConfigDES.Decrypt(ConfigurationManager.AppSettings["redis"]));

            services.AddDataProtection()
            .SetApplicationName("oa").PersistKeysToStackExchangeRedis(redis, "oa-share-key")
           .SetDefaultKeyLifetime(TimeSpan.FromDays(7));


            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "管理员管理后台API_v1", Version = "v1" });
                    c.IncludeXmlComments(Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, @"Timetable.Web.OA.xml"));
                });
            }


            //解决Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            { 
                x.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
            });


            /*自定义模型验证的返回结果*/
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                  .Where(e => e.Value.Errors.Count > 0)
                  .Select(e => e.Value.Errors.First().ErrorMessage)
                  .ToList();
                    var str = string.Join("\n", errors);
                    //设置返回内容
                    var result = new
                    {
                        success = false,
                        errCode = "ERR_MODEL_INVAILD",
                        msg = str,
                        data = actionContext.ModelState.Select(s => new { s.Key, Errors = s.Value.Errors.Select(t => t.ErrorMessage).ToList() })
                    };
                    return new BadRequestObjectResult(result);

                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (true && env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "管理员管理后台API_v1");
                    c.RoutePrefix = "swagger";
                });
            }
            app.UseEndpoints(endpoints =>
            {
              
                /**
                   * API属性路由支持
                   */
                endpoints.MapControllers();
            });

         
        }
    }
}
