using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.ContentManage;
using Cornerstone.Service;
using Microsoft.AspNetCore.Authorization;
using Cornerstone.Service.ContentManage;

namespace Cornerstone.Web.Areas.ContentManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-25 15:07
    /// 描 述：CMS模板类型控制器类
    /// </summary>
    [Area("ContentManage")]
    public class TemplateTypeController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public TemplateTypeService _service {get;set;}

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword)
        {
            var data = await _service.GetLookList(keyword);
            if (!string.IsNullOrEmpty(keyword))
            {
                 //此处需修改
                 data = data.TreeWhere(t => t.F_TypeName.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                treeModel.id = item.F_Id;
                treeModel.title = item.F_TypeName;
                treeModel.parentId = item.F_ParentId;
                treeModel.checkArr = "0";
                //treeModel.self = item;
                treeList.Add(treeModel);
            }
            return ResultDTree(treeList.TreeList());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeSelectJson()
        {
            var data = await _service.GetList();
            data = data.Where(a => a.F_Layers == 1).ToList();
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_TypeName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeJson()
        {
            var data = await _service.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (var item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_TypeName;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(string keyword)
        {
            var data = await _service.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_TypeName.Contains(keyword));
            }
            return Success(data.Count, data);
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
        public async Task<ActionResult> SubmitForm(TemplateTypeEntity entity, string keyValue)
        {
            TemplateTypeEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                if (entity.F_ParentId == "0")
                {
                    entity.F_Layers = 1;
                }
                else
                {
                    entity.F_Layers = (await _service.GetForm(entity.F_ParentId)).F_Layers + 1;
                }
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            TemplateTypeEntity data = null;
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
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetListJson(string keyword)
        {
            var data = await _service.GetList(keyword);
            return Content(data.ToJson());
        }
        #endregion
    }
}
