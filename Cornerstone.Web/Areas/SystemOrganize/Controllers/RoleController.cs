using Cornerstone.Service.SystemOrganize;
using Cornerstone.Code;
using Cornerstone.Domain.SystemOrganize;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Domain.SystemSecurity;
using Cornerstone.Service;
using System;
using System.Threading.Tasks;

namespace Cornerstone.Web.Areas.SystemOrganize.Controllers
{
    [Area("SystemOrganize")]
    public class RoleController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public RoleService _service { get; set; }

        [HttpGet]
        public virtual ActionResult AddForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJson(string keyword)
        {
            var data =await _service.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetSelectJson(string keyword,string ids)
        {
            var data = await _service.GetList(keyword);
            data = data.Where(a => a.F_EnabledMark == true).ToList();
            if (!string.IsNullOrEmpty(ids))
            {
                foreach (var item in ids.Split(','))
                {
                    var temp = data.Find(a => a.F_Id == item);
                    if (temp != null)
                    {
                        temp.LAY_CHECKED = true;
                    }
                }
            }
            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(Pagination pagination, string keyword)
        {
            pagination.order = "asc";
            pagination.sort = "F_EnCode";
            //导出全部页使用
            if (pagination.rows == 0 && pagination.page == 0)
            {
                pagination.rows = 99999999;
                pagination.page = 1;
            }
            var data =await _service.GetLookList(pagination,keyword);
            return Success(pagination.records,data);
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
        public async Task<ActionResult> SubmitForm(RoleEntity roleEntity, string permissionbuttonIds, string permissionfieldsIds, string keyValue)
        {
            RoleEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            if (!string.IsNullOrEmpty(keyValue)&& _service.currentuser.RoleId == keyValue)
            {
                return Error("操作失败，不能修改用户当前角色");
            }
            try
            {
                await _service.SubmitForm(roleEntity,string.IsNullOrEmpty(permissionbuttonIds) ?new string[0]: permissionbuttonIds.Split(','), string.IsNullOrEmpty(permissionfieldsIds) ? new string[0] : permissionfieldsIds.Split(','), keyValue);
                return await Success("操作成功。", className, roleEntity, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, roleEntity, data, keyValue);
            }
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            RoleEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                if (_service.currentuser.RoleId == keyValue)
                {
                    return Error("操作失败，不能删除用户当前角色");
                }
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
