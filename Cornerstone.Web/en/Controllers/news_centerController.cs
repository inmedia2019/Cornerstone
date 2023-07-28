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
    public class news_centerController : Controller
    {
        public ContentService _contentService { get; set; }
        public BlockService _blockService { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public news_centerController(
           IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: HomesController
        public ActionResult Index()
        {
            //查询banner
            string bannerId = "15ed6269-a4d8-4ddc-9e2d-0761413b2952";
            ViewBag.IsPhone = false;
            string osPat = "mozilla|m3gate|winwap|openwave|Windows NT|Windows 3.1|95|Blackcomb|98|ME|X Window|Longhorn|ubuntu|AIX|Linux|AmigaOS|BEOS|HP-UX|OpenBSD|FreeBSD|NetBSD|OS/2|OSF1|SUN|Macintosh";
            string uAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
            Regex reg = new Regex(osPat);
            if (reg.IsMatch(uAgent))
            {

            }
            else
            {
                ViewBag.IsPhone = true;
                bannerId = "1807667e-8a2c-42c0-a5fe-a09350dc8ddb";
            }

            var bannerData = _blockService.GetBannerByParentId(bannerId).Result;
            for (int i = 0; i < bannerData.Count; i++)
            {
                bannerData[i].F_ButtonPic1 = OSSHelper.GetOSSUrl(bannerData[i].F_ButtonPic1);
                bannerData[i].F_ButtonPic2 = OSSHelper.GetOSSUrl(bannerData[i].F_ButtonPic2);
                bannerData[i].F_Content = OSSHelper.GetImg(bannerData[i].F_Content.Replace("\\\"", "\""));
                bannerData[i].F_ImageUrl = OSSHelper.GetOSSUrl(bannerData[i].F_ImageUrl);
                bannerData[i].F_PicUrl = OSSHelper.GetOSSUrl(bannerData[i].F_PicUrl);
                bannerData[i].PicUrl = OSSHelper.GetOSSUrl(bannerData[i].PicUrl);
            }
            //var bannerData = _blockService.GetBannerByChannelId("f91beac0-09fb-4e28-ade3-66d720abbd64").Result;

            var data = _contentService.GetListByChannelId("f91beac0-09fb-4e28-ade3-66d720abbd65", 1, 1).Result.FirstOrDefault();
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

            if (data.F_CoverImage != null)
                data.F_CoverImage = OSSHelper.GetOSSUrl(data.F_CoverImage);
            if (data.F_SharePic != null)
                data.F_SharePic = OSSHelper.GetOSSUrl(data.F_SharePic);

            if (data.HomePic1 != null)
                data.HomePic1 = OSSHelper.GetOSSUrl(data.HomePic1);

            if (data.HomePic2 != null)
                data.HomePic2 = OSSHelper.GetOSSUrl(data.HomePic2);
            if (data.HomePic3 != null)
                data.HomePic3 = OSSHelper.GetOSSUrl(data.HomePic3);
            if (data.HomePic4 != null)
                data.HomePic4 = OSSHelper.GetOSSUrl(data.HomePic4);
            if (data.PicUrl != null)
                data.PicUrl = OSSHelper.GetOSSUrl(data.PicUrl);

            var count = _contentService.GetListCountByChannelId("f91beac0-09fb-4e28-ade3-66d720abbd65").Result;

            var bottom = _blockService.GetBannerById("39bf5cca-561f-4690-9355-e329176b21a0").Result;

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
            ViewBag.Count = count;
            ViewBag.bottomBanner = bottom.F_ImageUrl;
            ViewBag.bottomContent = "";
            if (bottom.F_Content != null)
            {
                ViewBag.bottomContent = OSSHelper.GetImg(bottom.F_Content.Replace("\\\"", "\""));
            }
            return View();
        }


        [HttpGet]
        public string GetDataByPage(int currentPage)
        {
            var data = _contentService.GetListByChannelId("f91beac0-09fb-4e28-ade3-66d720abbd65", currentPage, 10).Result;
            string returnStr = "";
            for (int i = 0; i < data.Count; i++)
            {
                returnStr += "<a href=\"/en/Newsdis/Index/" + data[i].F_Id + "\" target=\"_blank\">" +
                                        "<div class=\"f18 \">" + data[i].F_Titile.Replace("\"", "	&quot;") + "</div>" +
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
