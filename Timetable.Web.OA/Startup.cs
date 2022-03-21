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
            //��ʹ���շ���ʽ��key
            services.AddControllers().AddNewtonsoftJson(options =>
            { 
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); 
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
              {
                    /***
                     * �Զ����ض������������ض��򣬶��Ƿ���403��һ��JSON��Ϣ
                     * */
                  option.Events.OnRedirectToLogin = context =>
                  {
                      context.Response.StatusCode = 403;
                      context.Response.ContentType = "application/json; charset=utf-8";
                      return context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new ErrorResponse
                      {
                          success = false,
                          errCode = "ERR_AUTH_INVALID",
                          msg = "δ��¼����Ȩ���ʸýӿ�"
                      }));
                  };
                  option.Cookie.Name = "ttboa";//���ô洢�û���¼��Ϣ���û�Token��Ϣ����Cookie����
                    option.Cookie.HttpOnly = true;//���ô洢�û���¼��Ϣ���û�Token��Ϣ����Cookie���޷�ͨ���ͻ���������ű�(��JavaScript��)���ʵ�
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
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "����Ա�����̨API_v1", Version = "v1" });
                    c.IncludeXmlComments(Path.Join(System.AppDomain.CurrentDomain.BaseDirectory, @"Timetable.Web.OA.xml"));
                });
            }


            //���Multipart body length limit 134217728 exceeded
            services.Configure<FormOptions>(x =>
            { 
                x.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
            });


            /*�Զ���ģ����֤�ķ��ؽ��*/
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                  .Where(e => e.Value.Errors.Count > 0)
                  .Select(e => e.Value.Errors.First().ErrorMessage)
                  .ToList();
                    var str = string.Join("\n", errors);
                    //���÷�������
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "����Ա�����̨API_v1");
                    c.RoutePrefix = "swagger";
                });
            }
            app.UseEndpoints(endpoints =>
            {
              
                /**
                   * API����·��֧��
                   */
                endpoints.MapControllers();
            });

         
        }
    }
}
