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
using Cornerstone.Service.SystemManage;
using Cornerstone.Domain.SystemManage;
using Cornerstone.Service.SystemOrganize;
using Cornerstone.Web.OSSHelp;

namespace Cornerstone.Web.Areas.ContentManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-03-18 14:10
    /// 描 述：区块管理控制器类
    /// </summary>
    [Area("ContentManage")]
    public class BlockController : ControllerBase
    {
       

        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public BlockService _service { get; set; }
        public ItemsDataService _itmsdataservice { get; set; }
        public ItemsTypeService _itemstypeservice { get; set; }
        public SourceService _sourceservice { get; set; }

        public SystemSetService _setService { get; set; }
        public override ActionResult Form()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
           
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

                BlockEntity entity = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
                entity.F_ImageUrl = OSSHelper.GetOSSUrl(entity.F_ImageUrl);
                entity.F_PicUrl = OSSHelper.GetOSSUrl(entity.F_PicUrl);
                entity.F_ButtonPic1 = OSSHelper.GetOSSUrl(entity.F_ButtonPic1);
                entity.F_ButtonPic2 = OSSHelper.GetOSSUrl(entity.F_ButtonPic2);

                ViewBag.Content = entity.ToJson();
            }
           
            return View();
        }

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword = "")
        {
            var data = await _service.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_Name.Contains(keyword));
            }

            string lan =  CacheHelper.Get<string>("lan").Result;
            if (lan == null)
                lan = "0";
            else {
                try
                {
                    lan = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }
            }
            data = data.TreeWhere(t => Convert.ToString(t.F_lan) == lan);
            for (int i = 0; i < data.Count; i++)
            {
                data[i].F_ImageUrl = OSSHelper.GetOSSUrl(data[i].F_ImageUrl);
                data[i].F_PicUrl = OSSHelper.GetOSSUrl(data[i].F_PicUrl);
                data[i].F_ButtonPic1 = OSSHelper.GetOSSUrl(data[i].F_ButtonPic1);
                data[i].F_ButtonPic2 = OSSHelper.GetOSSUrl(data[i].F_ButtonPic2);
            }

            return Success(data.Count, data);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SetLan(string lan = "")
        {
            string userId = _setService.currentuser.UserId;
            await CacheHelper.Set("lan", userId + "_" + lan);
            return Success("");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(SoulPage<BlockEntity> pagination, string keyword)
        {
            if (string.IsNullOrEmpty(pagination.field))
            {
                pagination.field = "F_CreatorTime";
                pagination.order = "desc";
            }
            var data = await _service.GetLookList(pagination, keyword);
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
            data = data.TreeWhere(t => Convert.ToString(t.F_lan) == lan);

            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        /// <summary>
        /// 获取区块类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetBlockType()
        {
            var typelist = await _itemstypeservice.GetList();
            var type = typelist.Where(a => a.F_EnCode == "BlockType").FirstOrDefault();
            var data = await _itmsdataservice.GetList(type.F_Id);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_ItemCode;
                treeModel.text = item.F_ItemName;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
        /// <summary>
        /// 获取显示标识
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetShowType()
        {
            var typelist = await _itemstypeservice.GetList();
            var type = typelist.Where(a => a.F_EnCode == "ShowType").FirstOrDefault();
            var data = await _itmsdataservice.GetList(type.F_Id);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_ItemCode;
                treeModel.text = item.F_ItemName;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(BlockEntity entity, string keyValue)
        {
            BlockEntity data = null;

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
                data =  await _service.GetForm(entity.F_LanInfoId, lan);
                keyValue = data.F_Id;
                entity.F_Id = data.F_Id;
            }

            try
            {
                if (!string.IsNullOrEmpty(entity.F_Content))
                {
                    entity.F_Content = entity.F_Content.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\"", "\\\"").Replace("'", "&#39;");
                }
                else
                {
                    entity.F_Content = "";
                }

             

                BlockEntity entitys = new BlockEntity();

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
                        entitys.F_ChannelId = entity.F_ChannelId;
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
                        entitys.F_ChannelId = entity.F_ChannelId;
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
            BlockEntity data = null;
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
        #endregion
    }
}
