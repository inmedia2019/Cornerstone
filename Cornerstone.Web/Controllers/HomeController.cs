using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Service.SystemOrganize;

namespace Cornerstone.Web.Controllers
{
    public class HomeController : Controller
    {
        public SystemSetService _setService { get; set; }
        [HttpGet]
        [ServiceFilter(typeof(HandlerLoginAttribute))]
        public ActionResult Index()
        {
            //主页信息获取
            if (_setService.currentuser.UserId == null)
            {
                return View();
            }
            var systemset = _setService.GetForm(_setService.currentuser.CompanyId).Result;
            ViewBag.ProjectName = systemset.F_ProjectName;
            ViewBag.LogoIcon = !string.IsNullOrEmpty(systemset.F_Logo)? GlobalContext.SystemConfig.VirtualDirectory + "/icon/" + systemset.F_Logo:"";
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerLoginAttribute))]
        public ActionResult Default()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerLoginAttribute))]
        public ActionResult UserSetting()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        [ServiceFilter(typeof(HandlerLoginAttribute))]
        public ActionResult Message()
        {
            return View();
        }
    }
}
