using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Service.ContentManage;
using static UEditor.Core.Consts;
using Cornerstone.Web.OSSHelp;

namespace Cornerstone.Web.en.Controllers
{
    [Area("en")]
    public class careersController : Controller
    {
        public ContentService _contentService { get; set; }
        public BlockService _blockService { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public careersController(
           IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: HomesController
        public ActionResult Index()
        {
            //查询banner
            string bannerId = "fc92e29b-3625-40a5-a658-7ab642c1afa4";

            string osPat = "mozilla|m3gate|winwap|openwave|Windows NT|Windows 3.1|95|Blackcomb|98|ME|X Window|Longhorn|ubuntu|AIX|Linux|AmigaOS|BEOS|HP-UX|OpenBSD|FreeBSD|NetBSD|OS/2|OSF1|SUN|Macintosh";
            string uAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
            Regex reg = new Regex(osPat);
            if (reg.IsMatch(uAgent))
            {

            }
            else
            {
                bannerId = "f17235ec-22ed-4347-a0e2-ff32743880d3";
            }

            var bannerData = _blockService.GetBannerByParentId(bannerId).Result;
            for (int i = 0; i < bannerData.Count; i++)
            {
                bannerData[i].F_ButtonPic1 = OSSHelper.GetOSSUrl(bannerData[i].F_ButtonPic1);
                bannerData[i].F_ButtonPic2= OSSHelper.GetOSSUrl(bannerData[i].F_ButtonPic2);
                bannerData[i].F_Content = OSSHelper.GetImg(bannerData[i].F_Content.Replace("\\\"", "\""));
                bannerData[i].F_ImageUrl = OSSHelper.GetOSSUrl(bannerData[i].F_ImageUrl);
                bannerData[i].F_PicUrl= OSSHelper.GetOSSUrl(bannerData[i].F_PicUrl);
                bannerData[i].PicUrl = OSSHelper.GetOSSUrl(bannerData[i].PicUrl);
            }
           // var bannerData = _blockService.GetBannerByChannelId("7e444d45-9f34-452f-8838-bd9316bf0232").Result.FirstOrDefault();

            var data = _contentService.GetListByChannelId("7e444d45-9f34-452f-8838-bd9316bf0233").Result;
            if (data != null)
            {
               
                if (data.F_Content != null)
                    data.F_Content = OSSHelper.GetImg(data.F_Content.Replace("\\\"", "\""));
                if (data.F_MiddleFont1 != null)
                    data.F_MiddleFont1 = OSSHelper.GetImg(data.F_MiddleFont1.Replace("\\\"", "\""));
                if (data.F_MiddleFont2 != null)
                    data.F_MiddleFont2 = OSSHelper.GetImg(data.F_MiddleFont2.Replace("\\\"", "\""));
                if (data.F_MiddleFont3 != null)
                    data.F_MiddleFont3 = OSSHelper.GetImg(data.F_MiddleFont3.Replace("\\\"", "\""));
                if (data.F_LiveIntroduction != null)
                    data.F_LiveIntroduction = OSSHelper.GetImg(data.F_LiveIntroduction.Replace("\\\"", "\""));
                if (data.F_Summary != null)
                    data.F_Summary = OSSHelper.GetImg(data.F_Summary.Replace("\\\"", "\""));
                if (data.F_WonderfulContent != null)
                    data.F_WonderfulContent = OSSHelper.GetImg(data.F_WonderfulContent.Replace("\\\"", "\""));
                if (data.F_LiveBroadcast != null)
                    data.F_LiveBroadcast = OSSHelper.GetImg(data.F_LiveBroadcast.Replace("\\\"", "\""));

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
            ViewBag.banner = bannerData;

            return View();

        }
    }
}
