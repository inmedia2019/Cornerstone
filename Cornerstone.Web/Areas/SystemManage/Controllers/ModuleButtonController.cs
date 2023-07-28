using Cornerstone.Service.SystemManage;
using Cornerstone.Code;
using Cornerstone.Domain.SystemManage;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Service;
using System;
using System.Threading.Tasks;

namespace Cornerstone.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class ModuleButtonController : ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public ModuleService _moduleService { get; set; }
        public ModuleButtonService _service { get; set; }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson(string moduleId)
        {
            var data =await _service.GetList(moduleId);
            var treeList = new List<TreeSelectModel>();
            foreach (ModuleButtonEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string moduleId)
        {
            var data =await _service.GetLookList(moduleId);
            return Success(data.Count, data);
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
        [ServiceFilter(typeof(HandlerAdminAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ModuleButtonEntity moduleButtonEntity, string keyValue)
        {

            ModuleButtonEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                if (moduleButtonEntity.F_ParentId == "0")
                {
                    moduleButtonEntity.F_Layers = 1;
                }
                else
                {
                    moduleButtonEntity.F_Layers =(await _service.GetForm(moduleButtonEntity.F_ParentId)).F_Layers + 1;
                }
                await _service.SubmitForm(moduleButtonEntity, keyValue);
                return await Success("操作成功。", className, moduleButtonEntity, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, moduleButtonEntity, data, keyValue);
            }
        }
        [HttpPost]
        [ServiceFilter(typeof(HandlerAdminAttribute))]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            ModuleButtonEntity data = null;
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
        [HttpGet]
        public ActionResult CloneButton()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetCloneButtonTreeJson()
        {
            var moduledata =await _moduleService.GetList();
            var buttondata =await _service.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (ModuleEntity item in moduledata)
            {
                TreeGridModel treeModel = new TreeGridModel();
                treeModel.id = item.F_Id;
                treeModel.title = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeModel.checkArr = "0";
                treeModel.disabled = true;
                //treeModel.self = item;
                treeList.Add(treeModel);
            }
            foreach (ModuleButtonEntity item in buttondata)
            {
                TreeGridModel treeModel = new TreeGridModel();
                treeModel.id = item.F_Id;
                treeModel.title = item.F_FullName;
                treeModel.parentId = item.F_ParentId == "0" ? item.F_ModuleId : item.F_ParentId;
                treeModel.checkArr = "0";
                //treeModel.self = item;
                treeList.Add(treeModel);
            }
            return ResultDTree(treeList.TreeList());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public async Task<ActionResult> SubmitCloneButton(string moduleId, string Ids)
        {

            ModuleButtonEntity data = null;
            try
            {
                await _service.SubmitCloneButton(moduleId, Ids);
                return await Success("克隆成功。", className, data, data, Ids, DbLogType.Create);
            }
            catch (Exception ex)
            {
                return await Error("克隆失败，"+ex.Message, className, data, data, Ids, DbLogType.Create);
            }
        }
    }
}
