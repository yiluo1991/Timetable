using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CryptoHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Timetable.DbContext;
using Timetable.DbContext.Models;
using Timetable.Security;
using Timetable.Tools;
using Timetable.Web.CommonViewModel;
using Timetable.Web.MiniApp.Infrastructure;
using Timetable.Web.MiniApp.Infrastructure.Authorization;
using Timetable.Web.MiniApp.WeChatModel;

namespace Timetable.Web.MiniApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [HttpGet("[action]")]
        public ResponseBase LoginByUserName(string username, string password, string type)
        {
            using (var ctx = new TimeTableDbContext())
            {
                var hash = RandomHelper.CreateMixVerifyCode(16);
                if (type == "S")
                {
                    var student = ctx.Students.FirstOrDefault(s => EF.Functions.ILike(s.IdentityCode, username));
                    if (student != null && Crypto.VerifyHashedPassword(student.Password, student.IdentityCode + password))
                    {
                        student.SinglePointHash = hash;
                        ctx.SaveChanges();
                        return ReturnLoginSuccess(type, hash, student.IdentityCode);
                    }
                }
                else if (type == "T")
                {
                    var teacher = ctx.Teachers.FirstOrDefault(s => EF.Functions.ILike(s.IdentityCode, username));
                    if (teacher != null && Crypto.VerifyHashedPassword(teacher.Password, teacher.IdentityCode + password))
                    {
                        teacher.SinglePointHash = hash;
                        ctx.SaveChanges();
                        return ReturnLoginSuccess(type, hash, teacher.IdentityCode);
                    }
                }
            }
            return new ErrorResponse() { msg = "身份验证失败", success = false, errCode = "ERR_LOGIN_FAIL" };
        }
        [NonAction]
        private static ResponseBase ReturnLoginSuccess(string type, string hash, string name)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new ConfigDES().Decrypt(ConfigurationManager.AppSettings["tokenKey"])));
            //将用户信息添加到 Claim 中
            var identity = new ClaimsIdentity();
            IEnumerable<Claim> claims = new Claim[] {
                            new Claim(ClaimTypes.Name, type + ":" + name),
                            new Claim("sp", hash)
                       };
            identity.AddClaims(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,//创建声明信息
                Issuer = ConfigurationManager.AppSettings["tokenIssuer"],//Jwt token 的签发者
                Expires = DateTime.UtcNow.AddDays(2),
                Audience = ConfigurationManager.AppSettings["tokenAudience"],//Jwt token 的接收者 
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)//创建 token
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new ResponseBase
            {
                success = true,

                data = new
                {
                    Token = tokenHandler.WriteToken(token),
                    Id = type + ":" + name
                },
                msg = "登录成功"
            };
        }

        [HttpGet("[action]")]
        public ResponseBase LoginByWeChatCode(string code)
        {
            return _Login(code, WeChatConfig.AppId, WeChatConfig.AppSecret);
        }

        [Token(ValidateDatabase = true)]
        [HttpGet("[action]")]
        public ResponseBase BindWechat(string code)
        {
            WebClient client = new WebClient();
            string res = client.DownloadString("https://api.weixin.qq.com/sns/jscode2session?appid=" + WeChatConfig.AppId + "&secret=" + WeChatConfig.AppSecret + "&js_code=" + code + "&grant_type=authorization_code");
            WXSessionResponse r = JsonConvert.DeserializeObject<WXSessionResponse>(res);
            if (r.openid != null)
            {
                using (var ctx = new TimeTableDbContext())
                {
                    var type = User.Identity.Name.Substring(0, 1);
                    var identityCode = User.Identity.Name.Substring(2);
                    var students = ctx.Students.Where(s => EF.Functions.ILike(s.SchoolOpenId, r.openid)).ToList();
                    var teachers = ctx.Teachers.Where(s => EF.Functions.ILike(s.SchoolOpenId, r.openid)).ToList();
                    students.ForEach(s => s.SchoolOpenId = null);
                    teachers.ForEach(s => s.SchoolOpenId = null);
                    if (type == "T")
                    {
                        var teacher = ctx.Teachers.First(s => EF.Functions.ILike(s.IdentityCode, identityCode));
                        teacher.SchoolOpenId = r.openid;
                    }
                    else
                    {
                        var student = ctx.Students.First(s => EF.Functions.ILike(s.IdentityCode, identityCode));
                        student.SchoolOpenId = r.openid;
                    }
                    ctx.SaveChanges();
                    return new ResponseBase
                    {
                        success = true,

                        msg = "绑定成功"
                    };
                }
            }
            return new ErrorResponse() { msg = "绑定失败", success = false, errCode = "ERR_CODE_FAIL" };
        }
        [Token]
        [HttpGet("[action]")]
        public ResponseBase UnBindWechat(string code)
        {
            WebClient client = new WebClient();
            string res = client.DownloadString("https://api.weixin.qq.com/sns/jscode2session?appid=" + WeChatConfig.AppId + "&secret=" + WeChatConfig.AppSecret + "&js_code=" + code + "&grant_type=authorization_code");
            WXSessionResponse r = JsonConvert.DeserializeObject<WXSessionResponse>(res);
            if (r.openid != null)
            {
                using (var ctx = new TimeTableDbContext())
                {
                    var students = ctx.Students.Where(s => EF.Functions.ILike(s.SchoolOpenId, r.openid)).ToList();
                    var teachers = ctx.Teachers.Where(s => EF.Functions.ILike(s.SchoolOpenId, r.openid)).ToList();
                    students.ForEach(s => s.SchoolOpenId = null);
                    teachers.ForEach(s => s.SchoolOpenId = null);
                    var username = User.Identity.Name.Substring(2);
                    var type = User.FindFirst(ClaimTypes.Role).Value;
                    if (type == "T")
                    {
                        var teacher = ctx.Teachers.FirstOrDefault(s => s.IdentityCode == username);
                        if (teacher != null)
                        {
                            teacher.SinglePointHash = null;
                        }
                    }
                    else
                    {
                        var student = ctx.Students.FirstOrDefault(s => s.IdentityCode == username);
                        if (student != null)
                        {
                            student.SinglePointHash = null;
                        }
                    }
                    ctx.SaveChanges();
                    return new ResponseBase
                    {
                        success = true,
                        msg = "解绑成功"
                    };

                }
            }
            return new ErrorResponse() { msg = "绑定失败", success = false, errCode = "ERR_CODE_FAIL" };
        }


        [NonAction]
        private ResponseBase _Login(string code, string appId, string appSecret)
        {
            WebClient client = new WebClient();
            string res = client.DownloadString("https://api.weixin.qq.com/sns/jscode2session?appid=" + appId + "&secret=" + appSecret + "&js_code=" + code + "&grant_type=authorization_code");
            WXSessionResponse r = JsonConvert.DeserializeObject<WXSessionResponse>(res);
            if (r.openid != null)
            {
                using (var ctx = new TimeTableDbContext())
                {
                    var type = "";
                    var student = ctx.Students.FirstOrDefault(s => s.SchoolOpenId == r.openid);
                    var teacher = ctx.Teachers.FirstOrDefault(s => s.SchoolOpenId == r.openid);
                    string name = "";
                    var hash = RandomHelper.CreateMixVerifyCode(16);
                    if (student != null)
                    {
                        type = "S";
                        name = student.IdentityCode;
                        student.SinglePointHash = hash;

                        ctx.SaveChanges();
                    }
                    else if (teacher != null)
                    {
                        type = "T";
                        name = teacher.IdentityCode;
                        teacher.SinglePointHash = hash;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        return new ErrorResponse() { msg = "该微信没有绑定账号", success = false, errCode = "ERR_LOGIN_FAIL" };
                    }
                    return ReturnLoginSuccess(type, hash, name);
                }
            }
            return new ErrorResponse() { msg = "身份验证失败", success = false, errCode = "ERR_LOGIN_FAIL" };
        }
    }
}