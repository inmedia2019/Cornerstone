using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Cornerstone.Code;
using Cornerstone.Service;
using Cornerstone.Service.SystemSecurity;

namespace Cornerstone.Web
{
    [ServiceFilter(typeof(HandlerLoginAttribute))]
    [DataContract]
    public abstract class ControllerBase : Controller
    {
        [DataMember]
        public LogService _logService { get; set; }
        /// <summary>
        /// 演示模式过滤
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string action = context.RouteData.Values["Action"].ParseToString();
            OperatorModel user =  OperatorProvider.Provider.GetCurrent();

            if (GlobalContext.SystemConfig.Demo)
            {
                if (context.HttpContext.Request.Method.ToUpper() == "POST")
                {
                    string[] allowAction = new string[] { "LoginJson", "ExportUserJson", "CodePreviewJson" };
                    if (!allowAction.Select(p => p.ToUpper()).Contains(action.ToUpper()))
                    {

                        string Message = "演示模式，不允许操作";
                        context.Result = new JsonResult(new AjaxResult{
                            state = ResultType.error.ToString(),
                            message = Message

                        });
                        return;
                    }
                }
            }
            var resultContext = await next();
            sw.Stop();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Bottom()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Form2()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Form3()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Form4()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Solution()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Video()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult NewsCenter()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult TalentDevelopment()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult DevelopHistory()
        {
            return View();
        }
        
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult contactUS()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        public virtual ActionResult Details()
        {
            return View();
        }
        protected virtual async Task<ActionResult> Success<T>(string message,string className, T afterChange, T beforeChange, string keyValue ="", DbLogType? logType = null)
        {
            await _logService.WriteLog(message,className, afterChange, beforeChange, keyValue,logType);
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, keyValue = keyValue }.ToJson());
        }
        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success<T>(string message, T data)
        {
            return Content(new AjaxResult<T> { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult Success<T>(int total, T data)
        {
            return Content(new AjaxResult<T> { state = 0, message = "", count = total, data = data }.ToJson());
        }
        protected virtual ActionResult ResultDTree(object data)
        {
            return Content(new AjaxResultDTree { status =new StatusInfo {code=200,message= "操作成功" }, data = data }.ToJson());
        }
        protected virtual async Task<ActionResult> Error<T>(string message, string className, T afterChange, T beforeChange, string keyValue = "", DbLogType? logType = null)
        {
            await _logService.WriteLog(message, className, afterChange, beforeChange, keyValue, logType,true);
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = LogHelper.ExMsgFormat(message) }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = LogHelper.ExMsgFormat(message) }.ToJson());
        }
        /// <summary>
        /// 继承IActionResult 实现新的HtmlResult
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public virtual IActionResult Html(string htmlContent)
        {
            return new HtmlResult(htmlContent);
        }
    }
}
