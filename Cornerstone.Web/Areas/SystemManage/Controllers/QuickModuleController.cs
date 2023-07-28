using Cornerstone.Service.SystemManage;
using Cornerstone.Code;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cornerstone.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class QuickModuleController : Controller
    {
        public QuickModuleService _moduleService { get; set; }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTransferJson()
        {
            var userId = _moduleService.currentuser.UserId;
            var data =await _moduleService.GetTransferList(userId);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitForm(string permissionIds)
        {
            await _moduleService.SubmitForm(permissionIds.Split(','));
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "操作成功" }.ToJson());
        }
    }
}
