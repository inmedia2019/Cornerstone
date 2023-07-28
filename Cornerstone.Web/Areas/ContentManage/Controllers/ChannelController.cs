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
using Cornerstone.Domain.SystemManage;
using Cornerstone.Service.SystemManage;

namespace Cornerstone.Web.Areas.ContentManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-23 16:52
    /// 描 述：CMS栏目管理控制器类
    /// </summary>
    [Area("ContentManage")]
    public class ChannelController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public ChannelService _service {get;set;}
        public ContentService _contentservice { get; set; }
        public TemplateService _templateservice { get; set; }
        //public SourceService _sourceservice { get; set; }

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword)
        {
            var data = await _service.GetLookList(keyword);
            if (!string.IsNullOrEmpty(keyword))
            {
                 //此处需修改
                 data = data.TreeWhere(t => t.F_ChannelName.Contains(keyword));
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

            data = data.TreeWhere(t => Convert.ToString(t.F_lan) == lan);

            data = data.TreeWhere(t => t.F_ContentTemplate != "");
            foreach (ChannelEntity c in data)
            {
                if (!string.IsNullOrEmpty(c.F_ContentTemplate))
                {
                    c.F_FileContent = _templateservice.GetForm(c.F_ContentTemplate).Result.F_FileContent;
                }
            }

            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                treeModel.id = item.F_Id;
                treeModel.title = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeModel.code = item.F_FileContent;
                treeModel.checkArr = "0";
                //treeModel.self = item;
                treeList.Add(treeModel);
            }
            return ResultDTree(treeList.TreeList());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(string keyword)
        {
            var data = await _service.GetLookList();
            foreach (ChannelEntity c in data)
            {
                c.F_Number = await _contentservice.GetListCountByChannelId(c.F_Id);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_ChannelName.Contains(keyword));
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

            data = data.TreeWhere(t => Convert.ToString(t.F_lan) == lan);
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson()
        {
            var data = await _service.GetList();
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

            data = data.TreeWhere(p => p.F_lan == Convert.ToInt32(lan));

            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
           
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

            var data = await _service.GetLookForm(keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                data = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
            }
           
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ChannelEntity entity, string keyValue)
        {
            ChannelEntity data = null;

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
                //data = await _service.GetForm(keyValue);
               
                data = await _service.GetForm(entity.F_LanInfoId, lan);
                keyValue = data.F_Id;
                entity.F_Id = data.F_Id;
            }
            try
            {
                ChannelEntity entitys = new ChannelEntity();

                entity.F_lan = Convert.ToInt32(lan);
                string guid = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(keyValue))
                {
                    entity.F_LanInfoId = guid;
                }

                await _service.SubmitForm(entity, keyValue);

                if (lan == "0")
                {
                    var lanEntity = await _service.GetForm(entity.F_LanInfoId, "1");
                    if (lanEntity == null)
                    {
                        entitys.F_lan = 1;
                        entitys.F_LanInfoId = guid;
                        entitys.F_Order = 0;
                        entitys.F_ParentId = entity.F_ParentId;
                        await _service.SubmitForm(entitys, "");
                    }
                }
                else
                {
                    var lanEntity = await _service.GetForm(entity.F_LanInfoId, "0");
                    if (lanEntity == null)
                    {
                        entitys.F_lan = 0;
                        entitys.F_LanInfoId = guid;
                        entitys.F_Order = 0;
                        entitys.F_ParentId = entity.F_ParentId;
                        await _service.SubmitForm(entitys, "");
                    }
                }

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
            ChannelEntity data = null;
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
        /// <summary>
        /// 选择栏目内容属性
        /// </summary>
        /// <returns></returns>
        public ActionResult Choose()
        {
            return View();
        }
        #endregion
    }
}
