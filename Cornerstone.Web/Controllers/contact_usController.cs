using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Domain.InfoManage;
using Cornerstone.Service.ContentManage;
using Cornerstone.Service.InfoManage;
using static UEditor.Core.Consts;
using Cornerstone.Web.OSSHelp;

namespace Cornerstone.Web.Controllers
{
    public class contact_usController : Controller
    {
        public ContentService _contentService { get; set; }
        public BlockService _blockService { get; set; }
        public LivemessageService _livemessageService { get; set; }

        public ContactinfoService _contactinfoService { get; set; }
        //public JobinfoService _jobinfoService { get; set; }
       // public QuestiontypeinfoService _questiontypeinfoService { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public contact_usController(
           IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: HomesController
        public ActionResult Index()
        {
            //查询banner
            string bannerId = "522f0314-0055-40a0-bee5-07c9103fbf7a";

            string osPat = "mozilla|m3gate|winwap|openwave|Windows NT|Windows 3.1|95|Blackcomb|98|ME|X Window|Longhorn|ubuntu|AIX|Linux|AmigaOS|BEOS|HP-UX|OpenBSD|FreeBSD|NetBSD|OS/2|OSF1|SUN|Macintosh";
            string uAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
            Regex reg = new Regex(osPat);
            if (reg.IsMatch(uAgent))
            {

            }
            else
            {
                bannerId = "5218d2aa-477f-44a1-96c9-8b4965a2c719";
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

            // var bannerData = _blockService.GetBannerByChannelId("8a38c314-4bdd-480a-bdc7-93c585ad8163").Result.FirstOrDefault();
            var backGround = _blockService.GetBannerById("7939f3c9-c825-4204-b8bd-44cbe8a86292").Result;
            backGround.F_ButtonPic1 = OSSHelper.GetOSSUrl(backGround.F_ButtonPic1);
            backGround.F_ButtonPic2 = OSSHelper.GetOSSUrl(backGround.F_ButtonPic2);
            backGround.F_Content = OSSHelper.GetImg(backGround.F_Content.Replace("\\\"", "\""));
            backGround.F_ImageUrl = OSSHelper.GetOSSUrl(backGround.F_ImageUrl);
            backGround.F_PicUrl = OSSHelper.GetOSSUrl(backGround.F_PicUrl);
            backGround.PicUrl = OSSHelper.GetOSSUrl(backGround.PicUrl);

            var data = _contactinfoService.GetBannerByChannelId("8a38c314-4bdd-480a-bdc7-93c585ad8163").Result;
            int c = data.Where(p => p.F_ParentId == "0").Count();
            string returnHtml = "";
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].F_ParentId == "0")
                {
                    returnHtml += " <div class=\"\" " + (i < c - 1 ? "style=\"border-bottom:1px solid #979797; margin-bottom: 30px; " + (reg.IsMatch(uAgent) == false ? "padding-bottom: 20px;" : "") + "\"" : "") + "><div class=\"f22 fb c005059\">" + data[i].F_Name + "</div><div class=\"h30\"></div>";
                }

                for (int j = 0; j < data.Count; j++)
                {
                    if (data[j].F_ParentId == data[i].F_Id)
                    {
                        returnHtml += "<div class=\"flex f16 alignitems_center justify_between\">" +
                                    "<div class=\"contname\">" + data[j].F_Name + "</div>" +
                                    "<div class=\"flex alignitems_center\" style=\"width:230px;" + (data[j].F_Tel == "" ? "opacity:0;" : "") + "" + GetStyleInfo(reg.IsMatch(uAgent), data[j].F_Tel) + "\">" +
                                        "<div class=\"f14\" style=\"letter-spacing:1px;\">" +
                                            "<span class=\"fb\">电话：</span>" + data[j].F_Tel +
                                        "</div>" +
                                   " </div>" +
                                    "<div class=\"flex alignitems_center\" style=\"width:230px;" + (data[j].F_Email == "" ? "opacity:0;" : "") + "" + GetStyleInfo(reg.IsMatch(uAgent), data[j].F_Email) + "\">" +
                                        "<div class=\"f14\" style=\"letter-spacing:1px;\">" +
                                           " <span class=\"fb\">邮箱：</span>" + data[j].F_Email +
                                        "</div>" +
                                   " </div>" +
                                    "<div class=\"flex alignitems_center\" style=\"width:230px; " + (data[j].F_Fax == "" ? "opacity:0;" : "") + "" + GetStyleInfo(reg.IsMatch(uAgent), data[j].F_Fax) + "\">" +
                                        "<div class=\"f14\" style=\"letter-spacing:1px;\">" +
                                           " <span class=\"fb\">传真：</span>" + data[j].F_Fax +
                                      " </div>" +
                                   " </div>" +
                               " </div>" +
                             "   <div class=\"h30\"></div>";
                    }
                }

                if (data[i].F_ParentId == "0")
                {
                    returnHtml += "</div>";
                }
            }

            //查询职业
            //var jobData = _jobinfoService.GetList("").Result;
            //string jobStr = "";
            //for (int i = 0; i < jobData.Count; i++)
            //{
            //    jobStr += "<el-option label=\""+ jobData[i].F_Name + "\" value=\""+ jobData[i].F_Name + "\"></el-option>";
            //}

            ////查询问题类型
            //var questionType = _questiontypeinfoService.GetList("").Result;
            //string qStr = "";
            //for (int i = 0; i < questionType.Count; i++)
            //{
            //    qStr += "<el-option label=\"" + questionType[i].title + "\" value=\"" + questionType[i].title + "\"></el-option>";
            //}

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

            var seoData = _contentService.GetListByChannelId("8a38c314-4bdd-480a-bdc7-93c585ad8163", 1, 1).Result.FirstOrDefault();
            
            ViewBag.SEO = seoData;
            ViewBag.BottomInfo = bottomData;

            ViewBag.Info = returnHtml;
            ViewBag.banner = bannerData;
            ViewBag.backGroud = backGround;
          //  ViewBag.job = jobStr;
           // ViewBag.question = qStr;

            return View();
        }

        private string GetStyleInfo(bool v, string f_Fax)
        {
            if (v == false && f_Fax == "")
                return "display:none;";
            else
                return "";
        }

        [HttpPost]
        public string AddInfo([FromBody] MessageInfo msg)
        {
            LivemessageEntity info = new LivemessageEntity();
            info.F_City = msg.city;
            info.F_createDate = DateTime.Now;
            info.F_Email = msg.email;
            info.F_Id = Guid.NewGuid().ToString();
            info.F_IsAgreeActionCollect = true;
            info.F_IsAgreeAgreement = true;
            info.F_JobTitle = msg.job;
            info.F_MediaName = msg.companyName;
            info.F_Message = msg.message;
            info.F_Mid = "";
            info.F_Phone = msg.phone;
            info.F_questionType = msg.questionType;
            info.F_Sex = 0;
            info.F_TrueName = msg.trueName;
            string returnId = _livemessageService.SubmitForm(info, "").Result;
            if (returnId.Length > 0)
                return "1";
            return "0";
        }
    }

    public class MessageInfo
    {
        public string job { get; set; }
        public string questionType { get; set; }
        public string message { get; set; }
        public string trueName { get; set; }
        public string city { get; set; }
        public string companyName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

}
