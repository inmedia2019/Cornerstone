using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cornerstone.Code;
using Cornerstone.Domain.Entity.ContentManage;
using Cornerstone.Service;
using Cornerstone.Service.ContentManage;
using Cornerstone.Service.SystemManage;

namespace Cornerstone.Web.Areas.ContentManage.Controllers
{
    [Area("ContentManage")]
    [AllowAnonymous]
    public class ColumnController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public ColumnService _service { get; set; }
        public ItemsDataService _itmsdataservice { get; set; }
        public ItemsTypeService _itemstypeservice { get; set; }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson()
        {
            var data = await _service.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (ColumnEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword = "")
        {
            var data = await _service.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_Name.Contains(keyword));
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data = await _service.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ColumnEntity entity,string keyValue)
        {
            ColumnEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                await _service.SubmitForm(entity, keyValue);
                return await Success("操作成功。", className, entity, data, keyValue);
            }
            catch(Exception ex)
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
            ColumnEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                await _service.DeleteForm(keyValue);
                return await Success("操作成功。", className, data, data, keyValue, DbLogType.Delete);
            }
            catch(Exception ex)
            {
                return await Error(ex.Message, className, data, data, keyValue, DbLogType.Delete);
            }
        }
        /// <summary>
        /// 获取栏目类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetColumnType()
        {
            var typelist = await _itemstypeservice.GetList();
            var type = typelist.Where(a => a.F_EnCode == "ColumnType").FirstOrDefault();
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
    }
}
