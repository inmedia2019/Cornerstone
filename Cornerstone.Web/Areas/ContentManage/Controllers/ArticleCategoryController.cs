using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.ContentManage;
using Cornerstone.Service;
using Cornerstone.Service.ContentManage;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.Serialization;

namespace Cornerstone.Web.Areas.ContentManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-06-09 19:42
    /// 描 述：新闻类别控制器类
    /// </summary>
    [Area("ContentManage")]
    [AllowAnonymous]
    [DataContract]
    public class ArticleCategoryController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        //属性注入示例
        [DataMember]
        public ArticleCategoryService _service { get; set; }
        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            //此处需修改
            pagination.order = "desc";
            pagination.sort = "F_CreatorTime desc";
            var data = await _service.GetLookList(pagination, keyword);
            return Success(pagination.records, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword)
        {
            var data = await _service.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            }
            return Success(data.Count, data);
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
            var data = await _service.GetForm(keyValue);
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ArticleCategoryEntity entity, string keyValue)
        {
            ArticleCategoryEntity data = null;
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
            ArticleCategoryEntity data = null;
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
