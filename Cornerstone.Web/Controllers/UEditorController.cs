using Microsoft.AspNetCore.Mvc;
using UEditor.Core;
using Cornerstone.Domain.SystemSecurity;
using Cornerstone.Service;
using Cornerstone.Service.SystemSecurity;

namespace Cornerstone.Web.Controllers
{
    [Route("api/[controller]")]
    public class UEditorController : Controller
    {
        private UEditorService ue;
        public LogService _logService { get; set; }
        public UEditorController(UEditorService ue)
        {
            this.ue = ue;
        }

        [ServiceFilter(typeof(HandlerLoginAttribute))]
        public void Do()
        {
            try
            {
                ue.DoAction(HttpContext);

                LogEntity logEntity = new LogEntity();
                logEntity.F_ModuleName = "系统登录";
                logEntity.F_Type = DbLogType.Login.ToString();
                logEntity.F_Account = "";
                logEntity.F_NickName = "";
                logEntity.F_Result = false;
                logEntity.F_Description = "登录失败";
                _logService.WriteDbLog(logEntity);
            }
            catch (System.Exception ex)
            {
               
                LogEntity logEntity = new LogEntity();
                logEntity.F_ModuleName = "系统登录";
                logEntity.F_Type = DbLogType.Login.ToString();
                logEntity.F_Account = "";
                logEntity.F_NickName = "";
                logEntity.F_Result = false;
                logEntity.F_Description = "登录失败，" + ex.Message;
                 _logService.WriteDbLog(logEntity);
            }
          
        }
    }
}
