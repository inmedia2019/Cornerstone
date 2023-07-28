using System;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Service;
using Cornerstone.Service.SystemSecurity;
using Cornerstone.Code;
using Cornerstone.Domain.SystemSecurity;
using System.Threading.Tasks;
using Cornerstone.Service.SystemOrganize;
using Cornerstone.Domain.SystemOrganize;
using Chloe;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Nancy.Json;

namespace Cornerstone.Web.Controllers
{
    public class LoginController : Controller
    {
        public FilterIPService _filterIPService { get; set; }
        public UserService _userService { get; set; }
        public LogService _logService { get; set; }
        public SystemSetService _setService { get; set; }
        public IDbContext _context { get; set; }
        [HttpGet]
        public virtual async Task<ActionResult> Index(string code = "", string loginType = "0")
        {
            //登录页获取logo和项目名称
            try
            {
                var systemset = await _setService.GetFormByHost("");

                if (systemset.F_DBProvider != GlobalContext.SystemConfig.DBProvider || systemset.F_DbString != GlobalContext.SystemConfig.DBConnectionString)
                {
                    SystemSetEntity temp = new SystemSetEntity();
                    temp.F_DBProvider = GlobalContext.SystemConfig.DBProvider;
                    temp.F_DbString = GlobalContext.SystemConfig.DBConnectionString;
                    await _setService.SubmitForm(temp, systemset.F_Id);
                }

                if (GlobalContext.SystemConfig.Demo)
                {
                    ViewBag.UserName = Define.SYSTEM_USERNAME;
                    ViewBag.Password = Define.SYSTEM_USERPWD;
                }

                ViewBag.ProjectName = systemset.F_ProjectName;
                ViewBag.LogoIcon = !string.IsNullOrEmpty(systemset.F_Logo) ? GlobalContext.SystemConfig.VirtualDirectory + "/icon/" + systemset.F_Logo : "";
                return View();
            }
            catch (Exception)
            {
                ViewBag.ProjectName = "CMS内容管理系统";
                ViewBag.LogoIcon = GlobalContext.SystemConfig.VirtualDirectory + "/icon/favicon.ico";
                return View();
            }

        }

      

        /// <summary>
        　　/// Base64加密
        　　/// </summary>
        　　/// <param name="Message"></param>
        　　/// <returns></returns>
        public static string Base64Code(string Message)
        {
            byte[] bytes = Encoding.Default.GetBytes(Message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>  
        /// Base64解密  
        /// </summary>  
        /// <param name="Message"></param>  
        /// <returns></returns>  
        public string Base64Decode(string Message)
        {
            string dummyData = Message.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
            if (dummyData.Length % 4 > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }
            byte[] bytes = Convert.FromBase64String(dummyData);
            return Encoding.Default.GetString(bytes);

        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DateTime dttime)
        {
            TimeSpan ts = dttime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }

        /// <summary>
        /// 验证码获取（此接口已弃用）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuthCode()
        {
            return File(new VerifyCodeHelper().GetVerifyCode(), @"image/Gif");
        }
        [HttpGet]
        public async Task<ActionResult> OutLogin()
        {
            await _logService.WriteDbLog(new LogEntity
            {
                F_ModuleName = "系统登录",
                F_Type = DbLogType.Exit.ToString(),
                F_Account = _setService.currentuser.UserCode,
                F_NickName = _setService.currentuser.UserName,
                F_Result = true,
                F_Description = "安全退出系统",
            });
            await OperatorProvider.Provider.EmptyCurrent("pc_");
            return Redirect(GlobalContext.SystemConfig.VirtualDirectory + "/Login/Index");
        }
        /// <summary>
        /// 验证登录状态请求接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> CheckLoginState()
        {
            try
            {
                if (_setService.currentuser.UserId == null)
                {
                    return Content(new AjaxResult { state = ResultType.error.ToString() }.ToJson());
                }
                //登录检测      
                if ((await OperatorProvider.Provider.IsOnLine("pc_")).stateCode<=0)
                {
                    await OperatorProvider.Provider.EmptyCurrent("pc_");
                    return Content(new AjaxResult { state = ResultType.error.ToString() }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.success.ToString() }.ToJson());
                }
            }
            catch (Exception)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString() }.ToJson());
            }

        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户</param>
        /// <param name="password">密码</param>
        /// <param name="localurl">域名</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> CheckLogin(string username, string password,string localurl)
        {

            

            //根据域名判断租户
            LogEntity logEntity = new LogEntity();
            logEntity.F_ModuleName ="系统登录";
            logEntity.F_Type = DbLogType.Login.ToString();
            if (GlobalContext.SystemConfig.Debug)
            {
                localurl = "";
            }
            try
            {
                //if (!await CheckIP())
                //{
                //    throw new Exception("IP受限");
                //}
                UserEntity userEntity =await _userService.CheckLogin(username, password, localurl);
                OperatorModel operatorModel = new OperatorModel();
                operatorModel.UserId = userEntity.F_Id;
                operatorModel.UserCode = userEntity.F_Account;
                operatorModel.UserName = userEntity.F_RealName;
                operatorModel.CompanyId = userEntity.F_OrganizeId;
                operatorModel.DepartmentId = userEntity.F_DepartmentId;
                operatorModel.RoleId = userEntity.F_RoleId;
                operatorModel.LoginIPAddress = WebHelper.Ip;
                operatorModel.LoginIPAddressName = "本地局域网";//Net.GetLocation(operatorModel.LoginIPAddress);
                operatorModel.LoginTime = DateTime.Now;
                operatorModel.DdUserId = userEntity.F_DingTalkUserId;
                operatorModel.WxOpenId = userEntity.F_WxOpenId;
                //各租户的管理员也是当前数据库的全部权限
                operatorModel.IsSystem = userEntity.F_IsAdmin.Value;
                operatorModel.IsAdmin = userEntity.F_IsAdmin.Value;
                operatorModel.IsBoss = userEntity.F_IsBoss.Value;
                operatorModel.IsLeaderInDepts = userEntity.F_IsLeaderInDepts.Value;
                operatorModel.IsSenior = userEntity.F_IsSenior.Value;
                SystemSetEntity setEntity = await _setService.GetForm(userEntity.F_OrganizeId);
                operatorModel.DbString = setEntity.F_DbString;
                operatorModel.DBProvider = setEntity.F_DBProvider;
                if (userEntity.F_Account == "admin")
                {
                    operatorModel.IsSystem = true;
                }
                else
                {
                    operatorModel.IsSystem = false;
                }
                //缓存保存用户信息
                await OperatorProvider.Provider.AddLoginUser(operatorModel, "","pc_");
                logEntity.F_Account = userEntity.F_Account;
                logEntity.F_NickName = userEntity.F_RealName;
                logEntity.F_Result = true;
                logEntity.F_Description = "登录成功";
                await _logService.WriteDbLog(logEntity);
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "登录成功。"}.ToJson());
            }
            catch (Exception ex)
            {
                logEntity.F_Account = username;
                logEntity.F_NickName = username;
                logEntity.F_Result = false;
                logEntity.F_Description = "登录失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
         /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户</param>
        /// <param name="password">密码</param>
        /// <param name="localurl">域名</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> Login(string username, string password, string localurl)
        {
            //if (_setService.currentuser.UserId == null)
            //{
            //    return Content(new AjaxResult { state = ResultType.error.ToString() }.ToJson());
            //}
            ////登录检测      
            //if ((await OperatorProvider.Provider.IsOnLine("pc_")).stateCode <= 0)
            //{
            //    await OperatorProvider.Provider.EmptyCurrent("pc_");
            //    return Content(new AjaxResult { state = ResultType.error.ToString() }.ToJson());
            //}

            //根据域名判断租户
            LogEntity logEntity = new LogEntity();
            logEntity.F_ModuleName = "系统登录";
            logEntity.F_Type = DbLogType.Login.ToString();
            if (GlobalContext.SystemConfig.Debug)
            {
                localurl = "";
            }
            try
            {
                //if (!await CheckIP())
                //{
                //    throw new Exception("IP受限");
                //}
                UserEntity userEntity = await _userService.CheckLogin(username, password, localurl);
                OperatorModel operatorModel = new OperatorModel();
                operatorModel.UserId = userEntity.F_Id;
                operatorModel.UserCode = userEntity.F_Account;
                operatorModel.UserName = userEntity.F_RealName;
                operatorModel.CompanyId = userEntity.F_OrganizeId;
                operatorModel.DepartmentId = userEntity.F_DepartmentId;
                operatorModel.RoleId = userEntity.F_RoleId;
                operatorModel.LoginIPAddress = WebHelper.Ip;
                operatorModel.LoginIPAddressName = "本地局域网";//Net.GetLocation(operatorModel.LoginIPAddress);
                operatorModel.LoginTime = DateTime.Now;
                operatorModel.DdUserId = userEntity.F_DingTalkUserId;
                operatorModel.WxOpenId = userEntity.F_WxOpenId;
                //各租户的管理员也是当前数据库的全部权限
                operatorModel.IsSystem = userEntity.F_IsAdmin.Value;
                operatorModel.IsAdmin = userEntity.F_IsAdmin.Value;
                operatorModel.IsBoss = userEntity.F_IsBoss.Value;
                operatorModel.IsLeaderInDepts = userEntity.F_IsLeaderInDepts.Value;
                operatorModel.IsSenior = userEntity.F_IsSenior.Value;
                SystemSetEntity setEntity = await _setService.GetForm(userEntity.F_OrganizeId);
                operatorModel.DbString = setEntity.F_DbString;
                operatorModel.DBProvider = setEntity.F_DBProvider;
                if (userEntity.F_Account == "admin")
                {
                    operatorModel.IsSystem = true;
                }
                else
                {
                    operatorModel.IsSystem = false;
                }
                //缓存保存用户信息
                await OperatorProvider.Provider.AddLoginUser(operatorModel, "", "pc_");
                logEntity.F_Account = userEntity.F_Account;
                logEntity.F_NickName = userEntity.F_RealName;
                logEntity.F_Result = true;
                logEntity.F_Description = "登录成功";
                await _logService.WriteDbLog(logEntity);
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "登录成功。" }.ToJson());
            }
            catch (Exception ex)
            {
                logEntity.F_Account = username;
                logEntity.F_NickName = username;
                logEntity.F_Result = false;
                logEntity.F_Description = "登录失败，" + ex.Message;
                await _logService.WriteDbLog(logEntity);
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
        /// <summary>
        /// 快速登入
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> FastLogin(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var data = SecurityHelper.fromBase64(token);
                if (!string.IsNullOrEmpty(data))
                {
                    var obj = JObject.Parse(data);
                    var userId = obj["userId"] != null ? obj["userId"].ToString() : "";
                    var userName = obj["userName"] != null ? obj["userName"].ToString() : "";
                    var userMail = obj["userMail"] != null ? obj["userMail"].ToString() : "";
                    var timeSpan = obj["timeSpan"] != null ? obj["timeSpan"].ToString() : "";

                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userMail) && !string.IsNullOrEmpty(timeSpan))
                    {
                        double time = 0.0;
                        double.TryParse(timeSpan, out time);
                        var date = SecurityHelper.UnixTimeStampToDateTime(time);
                        if (date.AddHours(2) <= DateTime.Now)
                        {
                            return Content("请求链接失效:链接已过期");
                        }
                        var Loginstring =await Login(userMail, "39fc7ec903b1f60fb48d6e6442e59cc6", "");
                        return Redirect(GlobalContext.SystemConfig.VirtualDirectory + "/Home/Index");
                    }
                    else
                    {
                        return Content(new AjaxResult<string> { state = ResultType.error.ToString(), message = "参数验证失败" }.ToJson());
                    }
                }
                else
                {
                    return Content(new AjaxResult<string> { state = ResultType.error.ToString(), message = "签名验证失效" }.ToJson());
                }
            }
            else
            {
                return Content(new AjaxResult<string> { state = ResultType.error.ToString(), message = "非法请求" }.ToJson());
            }
        }



        private async Task<bool> CheckIP()
        {  
            string ip = WebHelper.Ip;
            return await _filterIPService.CheckIP(ip);
        }
    }

    public class InSideMemberInfo
    {
        private string access_token;
        private string id_token;
        private string token_type;
        private string expires_in;



        public string Access_token { get => access_token; set => access_token = value; }
		public string Id_token { get => id_token; set => id_token = value; }
		public string Token_type { get => token_type; set => token_type = value; }
		public string Expires_in { get => expires_in; set => expires_in = value; }
	}
}
