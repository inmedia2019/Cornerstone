using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.ContentManage;
using Cornerstone.Service;
using Microsoft.AspNetCore.Authorization;
using Cornerstone.Service.ContentManage;
using Cornerstone.Service.GenerateService;
using Cornerstone.Service.DictionaryDataBase;
using Cornerstone.Code.Web.SelectTree;
using System.Collections;
using Cornerstone.Web.OSSHelp;

namespace Cornerstone.Web.Areas.ContentManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-23 14:41
    /// 描 述：CMS内容管理控制器类
    /// </summary>
    [Area("ContentManage")]
    public class ContentController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public ContentService _service { get; set; }
        public ChannelService _channelservice { get; set; }
        public TemplateService _templateservice { get; set; }
        public ChannelManagerCache _channelManagerCache { get; set; }
        public LableService _lableservice { get; set; }
        public ExpertService _expertservice { get; set; }
        public override ActionResult Form()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }
            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            //var data2 = _expertservice.GetList();
            //var treeList2 = new List<SelectTreeModel>();
            //foreach (var item in data2.Result)
            //{
            //    //此处需修改
            //    SelectTreeModel treeModel = new SelectTreeModel();
            //    treeModel.value = item.F_Id;
            //    treeModel.name = item.F_Name;
            //    treeList2.Add(treeModel);
            //}

            //ViewBag.expert = treeList2.TreeList();


            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();

            ContentEntity entity = _service.GetForm(keyValue).Result;
            entity.F_Content = OSSHelper.GetImg(entity.F_Content);
            entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);
            
            entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

            entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
            entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
            entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
            entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

            entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

            entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

            ViewBag.Content = entity.ToJson();
            return View();
        }
        public override ActionResult Form2()
        {

            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            //var data2 = _expertservice.GetList();
            //var treeList2 = new List<SelectTreeModel>();
            //foreach (var item in data2.Result)
            //{
            //    //此处需修改
            //    SelectTreeModel treeModel = new SelectTreeModel();
            //    treeModel.value = item.F_Id;
            //    treeModel.name = item.F_Name;
            //    treeList2.Add(treeModel);
            //}

            //ViewBag.expert = treeList2.TreeList();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();

            ContentEntity entity = _service.GetForm(keyValue).Result;
            entity.F_Content = OSSHelper.GetImg(entity.F_Content);
            entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

            entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

            entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
            entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
            entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
            entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

            entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

            entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

            ViewBag.Content = entity.ToJson();
            return View();
        }
        public override ActionResult Form3()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();

            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                //ViewBag.Content = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result.ToJson();
                ContentEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                string[] temp = entity.F_ShareTitle.Split(',');
                string pic = "";
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].Length > 0)
                    {
                        pic += OSSHelper.GetOSSUrl(temp[i]) + ",";
                    }
                }
                if (pic.Length > 0)
                {
                    pic = pic.Substring(0, pic.Length - 1);
                }

                entity.F_ShareTitle = "," + pic;

                ViewBag.Content = entity.ToJson();
            }

            //ViewBag.Content = _service.GetForm(keyValue).Result.ToJson();
            return View();
        }
        public override ActionResult Form4()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

          

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();

            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ContentEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

                ViewBag.Content = entity.ToJson();
            }
            return View();
        }

        public override ActionResult Solution()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            //var data2 = _expertservice.GetList();
            //var treeList2 = new List<SelectTreeModel>();
            //foreach (var item in data2.Result)
            //{
            //    //此处需修改
            //    SelectTreeModel treeModel = new SelectTreeModel();
            //    treeModel.value = item.F_Id;
            //    treeModel.name = item.F_Name;
            //    treeList2.Add(treeModel);
            //}

            //ViewBag.expert = treeList2.TreeList();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();
            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ContentEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

                ViewBag.Content = entity.ToJson();
            }
            return View();
        }

        public override ActionResult DevelopHistory()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            //var data2 = _expertservice.GetList();
            //var treeList2 = new List<SelectTreeModel>();
            //foreach (var item in data2.Result)
            //{
            //    //此处需修改
            //    SelectTreeModel treeModel = new SelectTreeModel();
            //    treeModel.value = item.F_Id;
            //    treeModel.name = item.F_Name;
            //    treeList2.Add(treeModel);
            //}

            //ViewBag.expert = treeList2.TreeList();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();
            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ContentEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

                ViewBag.Content = entity.ToJson();
            }
            return View();
        }

        public override ActionResult Bottom()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            //var data2 = _expertservice.GetList();
            //var treeList2 = new List<SelectTreeModel>();
            //foreach (var item in data2.Result)
            //{
            //    //此处需修改
            //    SelectTreeModel treeModel = new SelectTreeModel();
            //    treeModel.value = item.F_Id;
            //    treeModel.name = item.F_Name;
            //    treeList2.Add(treeModel);
            //}

            //ViewBag.expert = treeList2.TreeList();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();
            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ContentEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

                ViewBag.Content = entity.ToJson();
            }
            return View();
        }

        public override ActionResult NewsCenter()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            //var data2 = _expertservice.GetList();
            //var treeList2 = new List<SelectTreeModel>();
            //foreach (var item in data2.Result)
            //{
            //    //此处需修改
            //    SelectTreeModel treeModel = new SelectTreeModel();
            //    treeModel.value = item.F_Id;
            //    treeModel.name = item.F_Name;
            //    treeList2.Add(treeModel);
            //}

            //ViewBag.expert = treeList2.TreeList();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();
            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ContentEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

                ViewBag.Content = entity.ToJson();
            }
            return View();
        }

        public override ActionResult TalentDevelopment()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            //var data2 = _expertservice.GetList();
            //var treeList2 = new List<SelectTreeModel>();
            //foreach (var item in data2.Result)
            //{
            //    //此处需修改
            //    SelectTreeModel treeModel = new SelectTreeModel();
            //    treeModel.value = item.F_Id;
            //    treeModel.name = item.F_Name;
            //    treeList2.Add(treeModel);
            //}

            //ViewBag.expert = treeList2.TreeList();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();
            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ContentEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

                ViewBag.Content = entity.ToJson();
            }
            return View();
        }

        public override ActionResult Video()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }
            var data1 = _lableservice.GetListByLan(lanInfo);
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            //var data2 = _expertservice.GetList();
            //var treeList2 = new List<SelectTreeModel>();
            //foreach (var item in data2.Result)
            //{
            //    //此处需修改
            //    SelectTreeModel treeModel = new SelectTreeModel();
            //    treeModel.value = item.F_Id;
            //    treeModel.name = item.F_Name;
            //    treeList2.Add(treeModel);
            //}

            //ViewBag.expert = treeList2.TreeList();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();
           

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ContentEntity entity = _service.GetForms(lanEntity.F_LanInfoId, lanInfo).Result;
                if (entity != null)
                {
                    entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                    entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                    entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                    entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                    entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                    entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                    entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                    entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                    entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);
                }

                ViewBag.Content = entity.ToJson();
            }
            return View();
        }
        public override ActionResult contactUS()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.ChannelId = itemId;
                ViewBag.ChannelName = _channelservice.GetForm(itemId).Result.F_ChannelName;
            }
            else
            {
                ViewBag.ChannelId = "";
                ViewBag.ChannelName = "";
            }

            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();

            var data3 = _channelservice.GetList();
            var treeList3 = new List<SelectTreeModel>();
            foreach (var item in data3.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList3.Add(treeModel);
            }
            ViewBag.channel = treeList3.TreeGridJson();
            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ContentEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_Content = OSSHelper.GetImg(entity.F_Content);
                entity.F_Summary = OSSHelper.GetImg(entity.F_Summary);

                entity.F_CoverImage = OSSHelper.GetOSSUrl(entity.F_CoverImage);

                entity.F_LiveIntroduction = OSSHelper.GetImg(entity.F_LiveIntroduction);
                entity.F_WonderfulContent = OSSHelper.GetImg(entity.F_WonderfulContent);
                entity.F_LiveBroadcast = OSSHelper.GetImg(entity.F_LiveBroadcast);
                entity.F_LiveBroadcastId = OSSHelper.GetOSSUrl(entity.F_LiveBroadcastId);

                entity.F_SharePic = OSSHelper.GetOSSUrl(entity.F_SharePic);

                entity.F_ShareTitle = OSSHelper.GetOSSUrl(entity.F_ShareTitle);

                ViewBag.Content = entity.ToJson();
            }
            return View();
        }
        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(SoulPage<ContentEntity> pagination, string itemId, string keyword)
        {
            if (string.IsNullOrEmpty(pagination.field))
            {
                pagination.field = "F_CreatorTime";
                pagination.order = "desc";
            }

            string lan = CacheHelper.Get<string>("lan").Result;
            if (lan == null)
                lan = "0";
            else
            {
                try
                {
                    lan = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }
            }


            var data = await _service.GetLookList(pagination, itemId, keyword, lan);
            foreach (ContentEntity c in data)
            {
                if (!string.IsNullOrEmpty(c.F_Author))
                {
                    var arr = c.F_Author.Split(',');
                    ArrayList strTemps = new ArrayList();
                    foreach (string au in arr)
                    {
                        var expertInfo = _expertservice.GetForm(au).Result;
                        if (expertInfo != null)
                        {
                            strTemps.Add(expertInfo.F_Name);
                        }
                    }
                    c.F_Author = string.Join(",", strTemps.ToArray());
                }
                if (!string.IsNullOrEmpty(c.F_Tags))
                {
                    var arr2 = c.F_Tags.Split(',');
                    ArrayList strTemps2 = new ArrayList();
                    foreach (string ta in arr2)
                    {
                        var lableInfo = _lableservice.GetForm(ta).Result;
                        if (lableInfo != null)
                        {
                            strTemps2.Add(lableInfo.F_Name);
                        }
                    }
                    c.F_Tags = string.Join(",", strTemps2.ToArray());
                }
                
            }
            return Content(pagination.setData(data).ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJson(string keyword)
        {
            var data = await _service.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data = await _service.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ContentEntity entity, string keyValue)
        {
            ContentEntity data = null;
            string lan = CacheHelper.Get<string>("lan").Result;
            if (lan == null)
                lan = "0";
            else
            {
                try
                {
                    lan = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }
            }

            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForms(entity.F_LanInfoId, lan);
                keyValue = data.F_Id;
                entity.F_Id = data.F_Id;
            }
            try
            {
                if (!string.IsNullOrEmpty(entity.F_MiddleFont1))
                {
                    entity.F_MiddleFont1 = entity.F_MiddleFont1.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
                else
                {
                    entity.F_MiddleFont1 = "";
                }
                if (!string.IsNullOrEmpty(entity.F_MiddleFont2))
                {
                    entity.F_MiddleFont2 = entity.F_MiddleFont2.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
                else
                {
                    entity.F_MiddleFont2 = "";
                }
                if (!string.IsNullOrEmpty(entity.F_MiddleFont3))
                {
                    entity.F_MiddleFont3 = entity.F_MiddleFont3.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
                else
                {
                    entity.F_MiddleFont3 = "";
                }


                if (!string.IsNullOrEmpty(entity.F_Content))
                {
                    entity.F_Content = entity.F_Content.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
                else
                {
                    entity.F_Content = "";
                }
                if (!string.IsNullOrEmpty(entity.F_LiveIntroduction))
                {
                    entity.F_LiveIntroduction = entity.F_LiveIntroduction.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
                else
                {
                    entity.F_LiveIntroduction = "";
                }
                if (!string.IsNullOrEmpty(entity.F_WonderfulContent))
                {
                    entity.F_WonderfulContent = entity.F_WonderfulContent.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
                else
                {
                    entity.F_WonderfulContent = "";
                }
                if (!string.IsNullOrEmpty(entity.F_Summary))
                {
                    entity.F_Summary = entity.F_Summary.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
              
                if (string.IsNullOrEmpty(entity.F_SubTitle))
                {
                    entity.F_SubTitle = entity.F_Titile;
                }
                if (!string.IsNullOrEmpty(entity.F_LiveBroadcast))
                {
                    entity.F_LiveBroadcast = entity.F_LiveBroadcast.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
                if (!string.IsNullOrEmpty(entity.F_ShareDes))
                {
                    entity.F_ShareDes = entity.F_ShareDes.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }

                ContentEntity entitys = new ContentEntity();

                entity.F_lan = Convert.ToInt32(lan);
                string guid = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(keyValue))
                {
                    entity.F_LanInfoId = guid;
                }

                if (entity.F_PublishTime == null)
                {
                    entity.F_PublishTime = DateTime.Now;
                }

                await _service.SubmitForm(entity, keyValue);
                
                if (lan == "0")
                {
                    var lanEntity = await _service.GetForm(entity.F_LanInfoId, "1");
                    var channelEntity = await _channelservice.GetForm(entity.F_ChannelId);
                    var lanChannel = await _channelservice.GetForm(channelEntity.F_LanInfoId, "1");
                    if (lanEntity == null)
                    {
                        entitys.F_lan = 1;
                        entitys.F_LanInfoId = guid;
                        entitys.F_ChannelId = lanChannel.F_Id;
                        entitys.F_Order = 0;
                        entitys.F_PublishTime = DateTime.Now;
                        await _service.SubmitForm(entitys, "");
                    }
                }
                else
                {
                    var lanEntity = await _service.GetForm(entity.F_LanInfoId, "0");
                    var channelEntity = await _channelservice.GetForm(entity.F_ChannelId);
                    var lanChannel = await _channelservice.GetForm(channelEntity.F_LanInfoId, "1");
                    if (lanEntity == null)
                    {
                        entitys.F_lan = 0;
                        entitys.F_LanInfoId = guid;
                        entitys.F_ChannelId = lanChannel.F_Id;
                        entitys.F_PublishTime = DateTime.Now;
                        entitys.F_Order = 0;
                        await _service.SubmitForm(entitys, "");
                    }
                }

                await _channelManagerCache.SetChannelLink(entity.F_ChannelId, entity.F_Id);

                return await Success("操作成功。", className, entity, data, entity.F_Id);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, entity, data, keyValue);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            ContentEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                await _service.DeleteForm(keyValue);
                return await Success("操作成功。", className, data, data, keyValue, DbLogType.Delete);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, data, data, keyValue, DbLogType.Delete);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PublishForm(string keyValue)
        {
            ContentEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                await _service.PublishForm(keyValue);
                return await Success("，发布操作成功。", className, data, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, data, data, keyValue);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecycleForm(string keyValue)
        {
            ContentEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                await _service.RecycleForm(keyValue);
                return await Success("，下架操作成功。", className, data, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, data, data, keyValue);
            }
        }
        #endregion
    }
}
