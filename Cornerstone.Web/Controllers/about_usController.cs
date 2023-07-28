using System;
using System.Collections;
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
    public class about_usController : Controller
    {
        public ContentService _contentService { get; set; }
        // GET: HomesController
        public ActionResult Index()
        {
            
            var data =  _contentService.GetListByChannelId("ec2546f7-865c-4db0-901e-3de27971c73e").Result;
            if (data.F_ShareTitle != null)
            {
                string[] temp = data.F_ShareTitle.Split(',');
                if (temp.Length > 1)
                    data.HomePic1 = OSSHelper.GetOSSUrl(temp[1]);
                if(temp.Length>2)
                    data.HomePic2 = OSSHelper.GetOSSUrl(temp[2]);
                if (temp.Length > 3)
                    data.HomePic3 = OSSHelper.GetOSSUrl(temp[3]);
                if (temp.Length > 4)
                    data.HomePic4 = OSSHelper.GetOSSUrl(temp[4]);
            }
            data.F_Content = OSSHelper.GetImg(data.F_Content.Replace("\\\"", "\""));
            data.F_MiddleFont1 = data.F_MiddleFont1.Replace("\\\"", "\"");
            data.F_MiddleFont2 = data.F_MiddleFont2.Replace("\\\"", "\"");
            data.F_MiddleFont3 = data.F_MiddleFont3.Replace("\\\"", "\"");
            data.F_LiveIntroduction = OSSHelper.GetImg(data.F_LiveIntroduction.Replace("\\\"", "\""));
            data.F_CoverImage = OSSHelper.GetOSSUrl(data.F_CoverImage);
            data.F_SharePic = OSSHelper.GetOSSUrl(data.F_SharePic);
            data.F_LiveBroadcastId = OSSHelper.GetOSSUrl(data.F_LiveBroadcastId);
            //var newsData = _contentService.GetListByChannelId("f91beac0-09fb-4e28-ade3-66d720abbd64", 1, 4).Result;
            var bottomData = _contentService.GetListByChannelId("8ec8e079-8591-44a4-898e-8991be2beef4", 1, 1).Result.FirstOrDefault();
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

            ArrayList listYear = new ArrayList();

            var developData= _contentService.GetListByChannelIds("cdcbf0d7-6771-4566-bfeb-6ac7b5b33200").Result;
            for (int i = 0; i < developData.Count; i++)
            {
                if (!listYear.Contains(developData[i].F_Titile))
                {
                    listYear.Add(developData[i].F_Titile);
                }
            }

            ViewBag.BottomInfo = bottomData;
            ViewBag.Info = data;
            ViewBag.Year = listYear;
            ViewBag.news = developData;
            return View();
        }
    }
}
