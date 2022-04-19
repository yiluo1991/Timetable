using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
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
using Timetable.Web.CommonViewModel;

namespace Timetable.Web.MiniApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public IConfiguration Configuration { get; }
         

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                //不使用驼峰样式的key

                options.SerializerSettings.ContractResolver = new DefaultContractResolver();


                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

         


            //解决Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            {

                x.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
            });


            //自定义模型验证错误返回信息
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

            if ( CurrentEnvironment.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("client", new OpenApiInfo { Title = "客户端API", Version = "1.0.0" });
                    c.IncludeXmlComments(Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, @"Timetable.Web.MiniApp.xml"));
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            if ( env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
              
                    c.SwaggerEndpoint("/swagger/client/swagger.json", "客户端API_v1");
                    c.RoutePrefix = "swagger";
                });

            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
             name: "default",
             pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}
