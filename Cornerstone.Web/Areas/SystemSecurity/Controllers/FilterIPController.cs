using Cornerstone.Service.SystemSecurity;
using Cornerstone.Code;
using Cornerstone.Domain.SystemSecurity;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Service;
using System;
using System.Threading.Tasks;

namespace Cornerstone.Web.Areas.SystemSecurity.Controllers
{
    [Area("SystemSecurity")]
    public class FilterIPController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public FilterIPService _service { get; set; }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(string keyword)
        {
            var data =await _service.GetLookList(keyword);
            return Success(data.Count,data);
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
        public async Task<ActionResult> SubmitForm(FilterIPEntity filterIPEntity, string keyValue)
        {
            FilterIPEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    filterIPEntity.F_DeleteMark = false;
                }
                await _service.SubmitForm(filterIPEntity, keyValue);
                return await Success("操作成功。", className, filterIPEntity, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, filterIPEntity, data, keyValue);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            FilterIPEntity data = null;
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
