using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Timetable.DbContext;
using Timetable.Security;
using Timetable.Web.CommonViewModel;

namespace Timetable.Web.MiniApp.Infrastructure.Authorization
{
    public class TokenAttribute : ActionFilterAttribute
    {
        private static SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new ConfigDES().Decrypt(ConfigurationManager.AppSettings["tokenKey"])));
        public TokenAttribute() { }


        public bool ValidateDatabase { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            string token = context.HttpContext.Request.Headers["token"];
            if (!string.IsNullOrEmpty(token))
            {
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = ConfigurationManager.AppSettings["tokenIssuer"],

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = ConfigurationManager.AppSettings["tokenAudience"],

                    // Validate the token expiry
                    ValidateLifetime = true,

                    // If you want to allow a certain amount of clock drift, set that here:
                    ClockSkew = TimeSpan.FromSeconds(60),

                };
                ClaimsPrincipal principal = null;
                SecurityToken validToken = null;

                var handler = new JwtSecurityTokenHandler();
                try
                {
                    principal = handler.ValidateToken(token, validationParameters, out validToken);
                    if (principal.Identity.IsAuthenticated)
                    {
                        var id = principal.Identity.Name.Split(":")[1];
                        var v = principal.FindFirst("sp").Value;
                        var type = principal.Identity.Name.Split(":")[0];
                        if (ValidateDatabase)
                        {
                            bool pass = true;
                            using (var ctx = new TimeTableDbContext())
                            {
                                if (type == "T")
                                {
                                   var teacher= ctx.Teachers.FirstOrDefault(s => s.IdentityCode == id);
                                    if (teacher != null)
                                    {
                                        if (teacher.Enable == false) pass = false;
                                        if (teacher.SinglePointHash != v) pass = false;
                                    }
                                }
                                else
                                {
                                    var student = ctx.Students.FirstOrDefault(s => s.IdentityCode == id);
                                    if (student != null)
                                    {
                                        if(student.StudentState== DbContext.Models.StudentState.DropOut)
                                        {
                                            pass = false;
                                        }
                                        if (student.SinglePointHash != v)
                                        {
                                            pass = false;
                                        }
                                    }
                                }
                                if (pass)
                                {
                                    //注入实名信息
                                    var identity = new ClaimsIdentity();
                                    IEnumerable<Claim> claims = new Claim[] { new Claim(ClaimTypes.Role, type) };
                                    identity.AddClaims(claims);
                                    principal.AddIdentity(identity);
                                    context.HttpContext.User = principal;
                                    return;
                                }
                             
                            }
                        }
                        else
                        {
                            var identity = new ClaimsIdentity();
                            IEnumerable<Claim> claims = new Claim[] { new Claim(ClaimTypes.Role, type) };
                            identity.AddClaims(claims);
                            principal.AddIdentity(identity);
                            context.HttpContext.User = principal;
                            return;
                        }
                      
                    }
                }
                catch (Exception ex)
                {

                }
                context.Result = new ContentResult()
                {
                    Content = JsonConvert.SerializeObject(new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_AUTH_INVALID",
                        msg = "TOKEN无效或用户已被禁用不允许访问该接口"
                    }),
                    ContentType = "application/json; charset=utf-8",
                    StatusCode = 403
                };
            }
            else
            {
                context.Result = new ContentResult()
                {
                    Content = JsonConvert.SerializeObject(new ErrorResponse
                    {
                        success = false,
                        errCode = "ERR_AUTH_INVALID",
                        msg = "您未登录或无权访问"
                    }),
                    ContentType = "application/json; charset=utf-8",
                    StatusCode = 403
                };
            }
        }
    }
}
