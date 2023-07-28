using Cornerstone.Service.SystemSecurity;
using Cornerstone.Code;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cornerstone.Domain.SystemSecurity;
using Cornerstone.Service;

namespace Cornerstone.Web.Areas.SystemSecurity.Controllers
{
    /// <summary>
    /// 定时任务
    /// </summary>
    [Area("SystemSecurity")]
    public class OpenJobsController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public OpenJobsService _service { get; set; }

        //获取详情
        [HttpGet]
        public async Task<ActionResult> GetFormJson(string keyValue)
        {
            var data = await _service.GetLookForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(OpenJobEntity entity, string keyValue)
        {
            OpenJobEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_EnabledMark = false;
                entity.F_DeleteMark = false;
            }
            else
            {
                entity.F_EnabledMark = null;
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
            OpenJobEntity data = null;
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
        /// 获取本地可执行的任务列表
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> QueryLocalHandlers()
        {
            var data = _service.QueryLocalHandlers();
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            pagination.order = "desc";
            pagination.sort = "F_EnabledMark";
            //导出全部页使用
            if (pagination.rows == 0 && pagination.page == 0)
            {
                pagination.rows = 99999999;
                pagination.page = 1;
            }
            var data = await _service.GetLookList(pagination, keyword);
            return Success(pagination.records, data);
        }
        /// <summary>
        /// 改变任务状态，启动/停止
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ChangeStatus(string keyValue, int status)
        {
            OpenJobEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                await _service.ChangeJobStatus(keyValue, status);
                return await Success("操作成功。", className,data,data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, data, data, keyValue);
            }
        }
    }
}
