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
    public class HomesController : Controller
    {
        public ContentService _contentService { get; set; }
        // GET: HomesController
        
        public ActionResult Index()
        {
           
            var data =  _contentService.GetListByChannelId("60b49913-71a0-4ba3-81bb-1350d7502678").Result;
            if (data.F_ShareTitle != null)
            {
                string[] temp = data.F_ShareTitle.Split(',');
                if (temp.Length > 1)
                    data.HomePic1 = OSSHelper.GetOSSUrl(temp[1]);
                if (temp.Length > 2)
                    data.HomePic2 = OSSHelper.GetOSSUrl(temp[2]);
                if (temp.Length > 3)
                    data.HomePic3 = OSSHelper.GetOSSUrl(temp[3]);
                if (temp.Length > 4)
                    data.HomePic4 = OSSHelper.GetOSSUrl(temp[4]);
            }

            if (data.F_ShareDes != null)
            {
                data.F_ShareDes = OSSHelper.GetImg(data.F_ShareDes.Replace("\\\"", "\""));
            }

            if (data.F_LiveBroadcastId != null)
            {
                data.F_LiveBroadcastId = OSSHelper.GetOSSUrl(data.F_LiveBroadcastId.Replace("\\\"", "\""));
            }

            if (data.F_SharePic != null)
            {
                data.F_SharePic = OSSHelper.GetOSSUrl(data.F_SharePic);
            }

            if (data.F_CoverImage != null)
            {
                data.F_CoverImage = OSSHelper.GetOSSUrl(data.F_CoverImage);
            }

            var newsData = _contentService.GetListByChannelId("f91beac0-09fb-4e28-ade3-66d720abbd64", 1, 6).Result;
            for (int i = 0; i < newsData.Count; i++)
            {
                if (newsData[i].F_Content != null)
                    newsData[i].F_Content = OSSHelper.GetImg(newsData[i].F_Content.Replace("\\\"", "\""));
                if (newsData[i].F_MiddleFont1 != null)
                    newsData[i].F_MiddleFont1 = OSSHelper.GetImg(newsData[i].F_MiddleFont1.Replace("\\\"", "\""));
                if (newsData[i].F_MiddleFont2 != null)
                    newsData[i].F_MiddleFont2 = OSSHelper.GetImg(newsData[i].F_MiddleFont2.Replace("\\\"", "\""));
                if (newsData[i].F_MiddleFont3 != null)
                    newsData[i].F_MiddleFont3 = OSSHelper.GetImg(newsData[i].F_MiddleFont3.Replace("\\\"", "\""));
                if (newsData[i].F_LiveIntroduction != null)
                    newsData[i].F_LiveIntroduction = OSSHelper.GetImg(newsData[i].F_LiveIntroduction.Replace("\\\"", "\""));
                if (newsData[i].F_Summary != null)
                    newsData[i].F_Summary = OSSHelper.GetImg(newsData[i].F_Summary.Replace("\\\"", "\""));
                if (newsData[i].F_WonderfulContent != null)
                    newsData[i].F_WonderfulContent = OSSHelper.GetImg(newsData[i].F_WonderfulContent.Replace("\\\"", "\""));
                if (newsData[i].F_LiveBroadcast != null)
                    newsData[i].F_LiveBroadcast = OSSHelper.GetImg(newsData[i].F_LiveBroadcast.Replace("\\\"", "\""));

                if (newsData[i].F_CoverImage != null)
                    newsData[i].F_CoverImage = OSSHelper.GetOSSUrl(newsData[i].F_CoverImage);
                if (newsData[i].F_SharePic != null)
                    newsData[i].F_SharePic = OSSHelper.GetOSSUrl(newsData[i].F_SharePic);

                if (newsData[i].HomePic1 != null)
                    newsData[i].HomePic1 = OSSHelper.GetOSSUrl(newsData[i].HomePic1);

                if (newsData[i].HomePic2 != null)
                    newsData[i].HomePic2 = OSSHelper.GetOSSUrl(newsData[i].HomePic2);
                if (newsData[i].HomePic3 != null)
                    newsData[i].HomePic3 = OSSHelper.GetOSSUrl(newsData[i].HomePic3);
                if (newsData[i].HomePic4 != null)
                    newsData[i].HomePic4 = OSSHelper.GetOSSUrl(newsData[i].HomePic4);
                if (newsData[i].PicUrl != null)
                    newsData[i].PicUrl = OSSHelper.GetOSSUrl(newsData[i].PicUrl);
              
            }

            var bottomData = _contentService.GetListByChannelId("8ec8e079-8591-44a4-898e-8991be2beef4", 1, 1).Result.FirstOrDefault();

            if (bottomData.F_WonderfulContent != null)
            {
                bottomData.F_WonderfulContent= OSSHelper.GetImg(bottomData.F_WonderfulContent.Replace("\\\"", "\""));
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
            ViewBag.news = newsData;

            return View();
        }

        public ActionResult Head()
        {
           
            return PartialView("~/Views/Shared/_Head.cshtml");
        }
        public ActionResult Foot()
        {
            
            return PartialView("/Views/Shared/_Foot.cshtml");

        }

    }

    public class HeadInfo
    {
        public string title { get; set; }
        public string descript { get; set; }
    }
}
