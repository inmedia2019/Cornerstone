using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Service.ContentManage;
using static UEditor.Core.Consts;
using System.Text.RegularExpressions;
using System.Collections;
using Cornerstone.Web.OSSHelp;

namespace Cornerstone.Web.en.Controllers
{
    [Area("en")]
    public class NewsdisController : Controller
    {
        public ContentService _contentService { get; set; }
        public BlockService _blockService { get; set; }
        // GET: HomesController
        public ActionResult Index(string id)
        {
            string currentUrl = HttpContext.Request.Path;
            var data = _contentService.GetInfoById(id).Result;
            string lan = "0";

            if (currentUrl.IndexOf("/en/") >= 0)
            {
                lan = "1";
            }

            data = _contentService.GetForm(data.F_LanInfoId, lan).Result;

            if (data != null)
            {
                data.F_LiveIntroduction = OSSHelper.GetImg(data.F_LiveIntroduction.Replace("\\\"", "\""));
            }

            var bottomData = _contentService.GetListByChannelId("8ec8e079-8591-44a4-898e-8991be2beef5", 1, 1).Result.FirstOrDefault();
            if (bottomData.F_WonderfulContent != null)
            {
                bottomData.F_WonderfulContent = OSSHelper.GetImg(bottomData.F_WonderfulContent.Replace("\\\"", "\""));
            }
            if (bottomData.F_LiveIntroduction != null)
            {
                bottomData.F_LiveIntroduction = OSSHelper.GetImg(bottomData.F_LiveIntroduction.Replace("\\\"", "\""));
            }
            if (bottomData.F_SharePic != null)
                bottomData.F_SharePic = OSSHelper.GetOSSUrl(bottomData.F_SharePic);

            if (bottomData.F_CoverImage != null)
                bottomData.F_CoverImage = OSSHelper.GetOSSUrl(bottomData.F_CoverImage);

            ViewBag.BottomInfo = bottomData;

            ViewBag.Info = data;
            if (data == null)
            {
                return Redirect("/en/");
            }
            return View();
        }

       

      

    }

}
