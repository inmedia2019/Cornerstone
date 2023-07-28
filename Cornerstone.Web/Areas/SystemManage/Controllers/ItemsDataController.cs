using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.SystemManage;
using Cornerstone.Service;
using Cornerstone.Service.SystemManage;
using System.Threading.Tasks;

namespace Cornerstone.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class ItemsDataController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public ItemsDataService _service { get; set; }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(string itemId, string keyword)
        {
            var data =await _service.GetLookList(itemId, keyword);
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSelectJson(string enCode)
        {
            var data =await _service.GetItemList(enCode);
            List<object> list = new List<object>();
            foreach (ItemsDetailEntity item in data)
            {
                list.Add(new { id = item.F_ItemCode, text = item.F_ItemName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data =await _service.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ItemsDetailEntity itemsDetailEntity, string keyValue)
        {
            ItemsDetailEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                await _service.SubmitForm(itemsDetailEntity, keyValue);
                return await Success("操作成功。", className, itemsDetailEntity, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, itemsDetailEntity, data, keyValue);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            ItemsDetailEntity data = null;
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
    }
}
