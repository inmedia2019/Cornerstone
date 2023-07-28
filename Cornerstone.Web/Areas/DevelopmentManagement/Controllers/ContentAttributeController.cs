using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.DevelopmentManagement;
using Cornerstone.Service;
using Microsoft.AspNetCore.Authorization;
using Cornerstone.Service.DevelopmentManagement;
using Cornerstone.Service.SystemManage;
using Cornerstone.Service.ContentManage;

namespace Cornerstone.Web.Areas.DevelopmentManagement.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-01-20 14:54
    /// 描 述：内容属性控制器类
    /// </summary>
    [Area("DevelopmentManagement")]
    public class ContentAttributeController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public ContentAttributeService _service {get;set;}
        public ItemsDataService _itmsdataservice { get; set; }
        public ItemsTypeService _itemstypeservice { get; set; }
        public ChannelService _channelservice { get; set; }
        public ModuleService _moduleservice { get; set; }
        public ModuleFieldsService _modulefieldsservice { get; set; }

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string channelId, string keyword)
        {
            pagination.order = "desc";
            pagination.sort = "F_CreatorTime desc";
            var data = await _service.GetLookList(pagination, channelId, keyword);
            return Success(pagination.records, data);
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
        public async Task<ActionResult> SubmitForm(ContentAttributeEntity entity, string keyValue)
        {
            ContentAttributeEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
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
            ContentAttributeEntity data = null;
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
        /// 获取内容所有字段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFieldName()
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.records = 0;
            pagination.rows = 20;
            pagination.order = "desc";
            pagination.sort = "F_CreatorTime desc";
            var modulelist = await _moduleservice.GetList();
            var module = modulelist.Where(a => a.F_EnCode == "Content").FirstOrDefault();
            var data = await _modulefieldsservice.GetLookList(pagination,module.F_Id);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_EnCode;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFieldType()
        {
            var typelist = await _itemstypeservice.GetList();
            var type = typelist.Where(a => a.F_EnCode == "FileType").FirstOrDefault();
            var data = await _itmsdataservice.GetList(type.F_Id);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_ItemName;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
        #endregion
    }
}
