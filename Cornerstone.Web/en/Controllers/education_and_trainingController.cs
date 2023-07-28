using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Domain.DictionaryDataBase;
using Cornerstone.Service.ContentManage;
using Cornerstone.Service.DictionaryDataBase;
using static UEditor.Core.Consts;
using Cornerstone.Web.OSSHelp;
using Cornerstone.Service.InfoManage;
using Tea;
using UEditor.Core;

namespace Cornerstone.Web.en.Controllers
{
    [Area("en")]
    public class education_and_trainingController : Controller
    {
        public ContentService _contentService { get; set; }
        public BlockService _blockService { get; set; }

        public UseractioninfoService _useractionService { get; set; }
        public MemberinfoService _memberinfoService { get; set; }

        public LableService _lableService { get; set; }

        public DepartmentService _departmentService { get; set; }

        public HistoryService _historyService { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public education_and_trainingController(
           IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // GET: HomesController
        public ActionResult Index()
        {

            //查询banner
            string bannerId = "7a51ec04-fa2d-45d7-93cd-683b17d5e0ac";

            // string osPat = "mozilla|m3gate|winwap|openwave|Windows NT|Windows 3.1|95|Blackcomb|98|ME|X Window|Longhorn|ubuntu|AIX|Linux|AmigaOS|BEOS|HP-UX|OpenBSD|FreeBSD|NetBSD|OS/2|OSF1|SUN|Macintosh";
            // string uAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
            // Regex reg = new Regex(osPat);
            // if (reg.IsMatch(uAgent))
            // {

            // }
            // else
            // {
            //     bannerId = "f17235ec-22ed-4347-a0e2-ff32743880d2";
            // }

            //var bannerData = _blockService.GetBannerByParentId(bannerId).Result;
            var bannerData = _blockService.GetBannerByChannelId(bannerId).Result.FirstOrDefault();
            if (bannerData != null)
            {
                bannerData.F_ButtonPic1 = OSSHelper.GetOSSUrl(bannerData.F_ButtonPic1);
                bannerData.F_ButtonPic2 = OSSHelper.GetOSSUrl(bannerData.F_ButtonPic2);

                bannerData.F_ImageUrl = OSSHelper.GetOSSUrl(bannerData.F_ImageUrl);
                bannerData.F_PicUrl = OSSHelper.GetOSSUrl(bannerData.F_PicUrl);
                bannerData.PicUrl = OSSHelper.GetOSSUrl(bannerData.PicUrl);
                bannerData.F_Content = OSSHelper.GetImg(bannerData.F_Content.Replace("\\\"", "\""));
            }

            //标签
            var tagData = _lableService.GetListByLan("1").Result;
            // var data = _contentService.GetListByChannelId("7e444d45-9f34-452f-8838-bd9316bf0232").Result;
            // if (data != null)
            // {

            //     if (data.F_Content != null)
            //         data.F_Content = data.F_Content.Replace("\\\"", "\"");
            //     if (data.F_MiddleFont1 != null)
            //         data.F_MiddleFont1 = data.F_MiddleFont1.Replace("\\\"", "\"");
            //     if (data.F_MiddleFont2 != null)
            //         data.F_MiddleFont2 = data.F_MiddleFont2.Replace("\\\"", "\"");
            //     if (data.F_MiddleFont3 != null)
            //         data.F_MiddleFont3 = data.F_MiddleFont3.Replace("\\\"", "\"");
            //     if (data.F_LiveIntroduction != null)
            //         data.F_LiveIntroduction = data.F_LiveIntroduction.Replace("\\\"", "\"");
            //     if (data.F_Summary != null)
            //         data.F_Summary = data.F_Summary.Replace("\\\"", "\"");
            //     if (data.F_WonderfulContent != null)
            //         data.F_WonderfulContent = data.F_WonderfulContent.Replace("\\\"", "\"");
            //     if (data.F_LiveBroadcast != null)
            //         data.F_LiveBroadcast = data.F_LiveBroadcast.Replace("\\\"", "\"");

            // }

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

            ViewBag.BottomInfo = bottomData;

            //查询科室/部门
            ViewBag.Department = _departmentService.GetListByLan("1").Result;

            //查询过往经验
            ViewBag.History = _historyService.GetListByLan("1").Result;

            // ViewBag.Info = data;
            ViewBag.banner = bannerData;
            ViewBag.tagData = tagData;
            ViewBag.Count = _contentService.GetListCountByChannelId("7a51ec04-fa2d-45d7-93cd-683b17d5e0ac").Result;
            return View();

        }

        [HttpGet]
        public string GetDataByPage(int currentPage, string tagId = "", string phone = "")
        {
            var data = _contentService.GetListByChannelId("7a51ec04-fa2d-45d7-93cd-683b17d5e0ac", currentPage, 9, tagId).Result;
            string returnStr = "[";
            string temp = "";
            for (int i = 0; i < data.Count; i++)
            {
                int count = _useractionService.GetRecordCount(phone, data[i].F_Id).Result;
                temp += "{\"Id\":\"" + data[i].F_Id + "\",\"Titile\":\"" + data[i].F_Titile.Replace("\"", "	&quot;") + "\",\"CoverImage\":\"" + OSSHelper.GetOSSUrl(data[i].F_CoverImage)
                    + "\",\"CoverImage\":\"" + OSSHelper.GetOSSUrl(data[i].F_CoverImage) + "\",\"LiveIntroduction\":\"" + OSSHelper.GetImg(data[i].F_LiveIntroduction.Replace("\\\"", "\""))
                    + "\",\"WonderfulContent\":\"" + OSSHelper.GetImg(data[i].F_WonderfulContent.Replace("\\\"", "\"")) + "\",\"VideoInfo\":\"" + data[i].F_Video + "\",\"isRead\":\"" + (count > 0 ? 1 : 0) + "\"},";
            }
            if (temp.Length > 0)
                temp = temp.Substring(0, temp.Length - 1);
            returnStr += temp + "]";
            return returnStr;
        }

        [HttpPost]
        public string AddInfo([FromBody] MemberInfo msg)
        {
            MemberinfoEntity minfo = _memberinfoService.GetUserInfoByPhone(msg.phone).Result;
            if (minfo != null)
            {
                return "2";
            }

            MemberinfoEntity info = new MemberinfoEntity();
            info.createDate = DateTime.Now;
            info.descript = msg.experience;
            info.Id = "";
            info.IsAgreeActionCollect = false;
            info.IsAgreeAgreement = false;
            info.IsSgin = 0;
            info.JobTitle = msg.Department;
            info.mediaName = msg.companyName;
            info.moreCol = msg.ConfirmPassword;
            info.moreCol1 = msg.surname;
            info.openId = msg.email;
            info.phone = msg.phone;
            info.Sex = 0;
            info.state = 0;
            info.trueName = msg.trueName;
            info.userScore = 0;

            string returnId = _memberinfoService.SubmitForm(info, "").Result;

            if (returnId.Length > 0)
                return "1";
            return "0";

        }

        [HttpPost]
        public string ModifyInfo([FromBody] MemberInfo msg)
        {
            MemberinfoEntity minfo = _memberinfoService.GetUserInfoByPhone(msg.phone).Result;
            if (minfo == null)
            {
                return "2";
            }

            minfo.moreCol = msg.ConfirmPassword;

            string returnId = _memberinfoService.SubmitForm(minfo, minfo.Id).Result;

            if (returnId.Length > 0)
                return "1";
            return "0";

        }

        [HttpPost]
        public string ValidateInfo([FromBody] MemberInfo msg)
        {
            MemberinfoEntity info = _memberinfoService.GetUserInfoByPhone(msg.phone, msg.pass).Result;

            if (info != null)
                return "1";
            return "0";

        }

        [HttpGet]
        public string CheckUserState(string phone = "")
        {
            MemberinfoEntity info = _memberinfoService.GetUserInfoByPhone(phone).Result;

            if (info != null)
            {
                return info.state.ToString();
            }
            return "0";

        }

        [HttpGet]
        public string AddUserActionInfo(string phone = "", string videoId = "")
        {
            UseractioninfoEntity info = new UseractioninfoEntity();
            info.createDate = DateTime.Now;
            info.descript = "";
            info.Id = "";
            info.infoid = videoId;
            info.InternalData = "";
            info.ipadress = "";
            info.lurl = "";
            info.mid = phone;
            info.moreCol = "";
            info.moreCol1 = "";
            info.nid = "";
            info.Phone = phone;
            info.PID = "";
            info.scoreNum = 0;
            info.SpecialData = "";
            info.state = 0;
            info.tid = 1;
            info.trueName = "";
            info.vContent = "";
            string id = _useractionService.SubmitForm(info, "").Result;

            if (id != "")
                return "1";
            return "0";

        }


        [HttpGet]
        public string SendCode(string phone = "")
        {
            string code = new RandomNumber().GenerateRandom(6);
            AlibabaCloud.SDK.Dysmsapi20170525.Client client = CreateClient(Consts.AliyunOssServer.AccessKeyIds, Consts.AliyunOssServer.AccessKeySecrets);
            AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest sendSmsRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                PhoneNumbers = phone,
                SignName = "康诺思腾",
                TemplateCode = "SMS_272960115",
                TemplateParam = "{\"code\":\"" + code + "\"}"
            };
            AlibabaCloud.TeaUtil.Models.RuntimeOptions runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
            try
            {
                // 复制代码运行请自行打印 API 的返回值
                AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsResponse result = client.SendSmsWithOptions(sendSmsRequest, runtime);
            }
            catch (TeaException error)
            {
                // 如有需要，请打印 error
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            }
            catch (Exception _error)
            {
                TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message", _error.Message }
                });
                // 如有需要，请打印 error
                AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            }

            return code;

        }

        public static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = accessKeyId,
                // 必填，您的 AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            config.Endpoint = "dysmsapi.aliyuncs.com";
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
        }

    }

    public class MemberInfo
    {
        public string phone { get; set; }
        public string ConfirmPassword { get; set; }
        public string code { get; set; }
        public string trueName { get; set; }
        public string companyName { get; set; }
        public string city { get; set; }
        public string Department { get; set; }
        public string email { get; set; }
        public string experience { get; set; }
        public string pass { get; set; }
        public string surname { get; set; }

    }

    public class RandomNumber
    {
        public static RandomNumber Instance()
        {
            return new RandomNumber();
        }

        private static char[] constant =
        {
           '0','1','2','3','4','5','6','7','8','9'
        };

        public string GenerateRandom(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }
    }

}
