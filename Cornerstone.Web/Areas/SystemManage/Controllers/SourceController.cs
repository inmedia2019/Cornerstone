using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.SystemManage;
using Cornerstone.Service;
using Microsoft.AspNetCore.Authorization;
using Cornerstone.Service.SystemManage;

namespace Cornerstone.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-09 15:08
    /// 描 述：来源类型控制器类
    /// </summary>
    [Area("SystemManage")]
    public class SourceController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public SourceService _service {get;set; }
        public ItemsDataService _itmsdataservice { get; set; }
        public ItemsTypeService _itemstypeservice { get; set; }

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword = "",string type="")
        {
            var data = await _service.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_Name.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(type))
            {
                data = data.TreeWhere(t => t.F_Type== type);
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(SoulPage<SourceEntity> pagination, string keyword, string type)
        {
            if (string.IsNullOrEmpty(pagination.field))
            {
                pagination.field = "F_CreatorTime";
                pagination.order = "desc";
            }
            var data = await _service.GetLookList(pagination,keyword,type);
            return Content(pagination.setData(data).ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJson(string keyword, string type)
        {
            var data = await _service.GetList(keyword, type);
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
        /// 获取来源类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSourceType()
        {
            var typelist = await _itemstypeservice.GetList();
            var type = typelist.Where(a => a.F_EnCode == "SourceType").FirstOrDefault();
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
        public async Task<ActionResult> SubmitForm(SourceEntity entity, string keyValue)
        {
           SourceEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                entity.F_Type = "1003";
                await _service.SubmitForm(entity, keyValue);
                return await Success("操作成功。", className, entity, data, keyValue);
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
           SourceEntity data = null;
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
