using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Service.ContentManage;
using static UEditor.Core.Consts;
using Cornerstone.Web.OSSHelp;

namespace Cornerstone.Web.Controllers
{
    public class BottomController : Controller
    {
        public ContentService _contentService { get; set; }
        // GET: HomesController
        public ActionResult Index()
        {
            // var data = _contentService.GetListByChannelId("8ec8e079-8591-44a4-898e-8991be2beef4", 1, 1).Result;

            //  ViewBag.Info = data;

           

            return View();
        }


        [HttpGet]
        public string GetDataByPage(int currentPage)
        {
            var data = _contentService.GetListByChannelId("f91beac0-09fb-4e28-ade3-66d720abbd64", currentPage, 1).Result;
            string returnStr = "";
            for (int i = 0; i < data.Count; i++)
            {
                returnStr += "<a href=\"Newsdis/Index/" + data[i].F_Id + "\">" +
                                        "<div class=\"f18 fb\">" + data[i].F_Titile.Replace("\"", "	&quot;") + "</div>" +
                                        "<div class=\"h10\"></div>" +
                                        "<div class=\"f14\" style=\"color: #b3b3b3;\">"+ OSSHelper.GetImg(data[i].F_WonderfulContent.Replace("\"", "	&quot;")) + "</div>" +
                                        "<div class=\"h10\"></div>" +
                                        "<div class=\"ter\">" + (Convert.ToDateTime(data[i].F_PublishTime).ToString("yyyy-MM-dd")) + "</div>" +
                                    "</a>";
            }
            return returnStr;
        }

    }

}
