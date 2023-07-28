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
using Cornerstone.Service.GenerateService;
using System.IO;
using Cornerstone.Domain.Entity.Generate;

namespace Cornerstone.Web.Areas.ContentManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-23 15:46
    /// 描 述：CMS模板管理控制器类
    /// </summary>
    [Area("ContentManage")]
    public class TemplateController : ControllerBase
    {
        private string HomeDirectory = "Template/Home";//首页模板
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public TemplateService _service { get; set; }
        public TemplateManagerCache _templateManagerCache { get; set; }

        public GenerateHome _generateHome { get; set; }
        [HttpGet]
        public override ActionResult Form()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            var itemId = HttpContext.Request.Query["itemId"].ToString();
            if (!string.IsNullOrEmpty(itemId))
            {
                ViewBag.TemplateTypeId = itemId;
               
            }
            else
            {
                ViewBag.TemplateTypeId = "";
            }
            ViewBag.UserName = _service.currentuser.UserName;
            ViewBag.Content = "";
            if (_service.GetForm(keyValue).Result != null)
                ViewBag.Content = _service.GetForm(keyValue).Result.F_TemplateContent;
            return View();
        }
        [HttpGet]
        public override ActionResult Details()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            ViewBag.Content = _service.GetForm(keyValue).Result.ToJson();
            return View();
        }
        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(string itemId, string keyword)
        {
            var data = await _service.GetLookList(itemId, keyword);
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
                treeModel.text = item.F_TemplateName;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(TemplateEntity entity, string keyValue)
        {
            TemplateEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                if (!string.IsNullOrEmpty(entity.F_TemplateContent))
                {
                    entity.F_TemplateContent = entity.F_TemplateContent.Replace("'", "\"");
                }
                await _service.SubmitForm(entity, keyValue);
                if (entity != null)
                {
                    _templateManagerCache.AddTemplate(entity);
                    RenderDocumentCache.Clear();
                }
                GenerateTemplate.Create(entity.F_TemplateMode, entity.F_TemplateFile, entity.F_TemplateContent);
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
            TemplateEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                string filePath = Path.Combine(GlobalParamsDto.WebRoot, HomeDirectory, data.F_TemplateFile);
                FileUtils.DeleteFileIfExists(filePath);
                RenderDocumentCache.Clear();
                await _service.DeleteForm(keyValue);
                return await Success("操作成功。", className, data, data, keyValue, DbLogType.Delete);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, data, data, keyValue, DbLogType.Delete);
            }
        }
        /// <summary>
        /// 生成首页
        /// </summary>
        /// <param name="keyValue">模板ID</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GenerateHome(string keyValue)
        {
            TemplateEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                await _generateHome.GenerateHomeHtml(keyValue);
                return await Success("首页生成成功。", className, data, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error("首页生成失败," + ex.Message, className, data, data, keyValue);
            }
        }
        /// <summary>
        /// 模板副本复制
        /// </summary>
        /// <param name="keyValue">模板ID</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CopyTemplate(string keyValue)
        {
            TemplateEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                var template = await _service.GetLookForm(keyValue);
                if (template.F_Id != "")
                {
                    string templateFile = template.F_TemplateFile;
                    if (template.F_TemplateMode == "1001")
                    {
                        templateFile = $"Template/{templateFile}";
                    }
                    else if (template.F_TemplateMode == "1002")
                    {
                        templateFile = $"Template/Channel/{templateFile}";
                    }
                    else if (template.F_TemplateMode == "1003")
                    {
                        templateFile = $"Template/Content/{templateFile}";
                    }
                    else if (template.F_TemplateMode == "1004")
                    {
                        templateFile = $"Template/Single/{templateFile}";
                    }
                    else if (template.F_TemplateMode == "1005")
                    {
                        templateFile = $"Template/include/{templateFile}";
                    }
                    string filePath = Path.Combine(GlobalParamsDto.WebRoot, templateFile);
                    FileInfo file = new FileInfo(filePath);

                    string copyFile = filePath.Replace(file.Name, "副本" + file.Name);
                    //Path.Combine(file.DirectoryName, "副本"+ file.Name);
                    bool status = FileUtils.CopyFile(filePath, copyFile);
                    if (status)
                    {
                        template.F_Id = "";
                        template.F_TemplateName = "副本" + template.F_TemplateName;
                        template.F_TemplateFile = template.F_TemplateFile.Replace(file.Name, "副本" + file.Name);
                        await _service.SubmitForm(template, template.F_Id);
                        _templateManagerCache.AddTemplate(template);
                        RenderDocumentCache.Clear();

                    }
                }
                return await Success("模板复制成功。", className, data, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error("模板复制失败," + ex.Message, className, data, data, keyValue);
            }
        }
        /// <summary>
        /// 本地模板同步
        /// </summary>
        /// <param name="keyValue">模板ID</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SyncTemplate(string keyValue)
        {
            TemplateEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                var template = await _service.GetLookForm(keyValue);
                if (template.F_Id != "")
                {
                    string templateFile = template.F_TemplateFile;
                    if (template.F_TemplateMode == "1001")
                    {
                        templateFile = $"Template/{templateFile}";
                    }
                    else if (template.F_TemplateMode == "1002")
                    {
                        templateFile = $"Template/Channel/{templateFile}";
                    }
                    else if (template.F_TemplateMode == "1003")
                    {
                        templateFile = $"Template/Content/{templateFile}";
                    }
                    else if (template.F_TemplateMode == "1004")
                    {
                        templateFile = $"Template/Single/{templateFile}";
                    }
                    else if (template.F_TemplateMode == "1005")
                    {
                        templateFile = $"Template/include/{templateFile}";
                    }

                    string filePath = Path.Combine(GlobalParamsDto.WebRoot, templateFile);
                    string newContent = FileUtils.ReadText(filePath);
                    if (newContent.IsEmpty())
                    {
                        return Error("模板同步失败");
                    }
                    template.F_TemplateContent = newContent;
                    await _service.SubmitForm(template, keyValue);
                    _templateManagerCache.AddTemplate(template);
                    RenderDocumentCache.Clear();

                }
                return await Success("模板同步成功。", className, data, data, keyValue);
            }
            catch (Exception ex)
            {
                return await Error("模板同步失败," + ex.Message, className, data, data, keyValue);
            }
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTemplate(string keyword = "")
        {
            var data = await _service.GetList(keyword);
            //if (type == 1)
            //    data = data.Where(a => a.F_EnabledMark == true && (a.F_TemplateMode=="1001"|| a.F_TemplateMode == "1002")).ToList();
            //else if(type == 2)
            //    data = data.Where(a => a.F_EnabledMark == true && a.F_TemplateMode == "1003").ToList();
            return Content(data.ToJson());
        }
        #endregion
    }
}
