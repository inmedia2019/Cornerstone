using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.DictionaryDataBase;
using Cornerstone.Service;
using Microsoft.AspNetCore.Authorization;
using Cornerstone.Service.DictionaryDataBase;

namespace Cornerstone.Web.Areas.DictionaryDataBase.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-04-28 16:47
    /// 描 述：Badgetypeinfo控制器类
    /// </summary>
    [Area("DictionaryDataBase")]
    public class BadgetypeinfoController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public BadgetypeinfoService _service {get;set;}

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(SoulPage<BadgetypeinfoEntity> pagination, string keyword)
        {
            if (string.IsNullOrEmpty(pagination.field))
            {
                pagination.field = "F_CreatorTime";
                pagination.order = "desc";
            }
            var data = await _service.GetLookList(pagination,keyword);
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
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_Name;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
        #endregion

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTemplate(string keyword = "", int typeId = 2)
        {
            var data = await _service.GetLists(keyword, typeId);

            return Content(data.ToJson());
        }


        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(BadgetypeinfoEntity entity, string keyValue)
        {
           BadgetypeinfoEntity data = null;
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
           BadgetypeinfoEntity data = null;
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
