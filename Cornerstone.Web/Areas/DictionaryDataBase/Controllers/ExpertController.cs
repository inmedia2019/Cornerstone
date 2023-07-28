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
using Cornerstone.Code.Web.SelectTree;
using Cornerstone.Service.ContentManage;

namespace Cornerstone.Web.Areas.DictionaryDataBase.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-03-18 14:06
    /// 描 述：专家管理控制器类
    /// </summary>
    [Area("DictionaryDataBase")]
    public class ExpertController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public ExpertService _service {get;set;}
        public LableService _lableservice { get; set; }
        public ChannelService _channelservice { get; set; }
        public override ActionResult Form()
        {
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            ViewBag.Content = _service.GetForm(keyValue).Result.ToJson();
            var data1 = _lableservice.GetList();
            var treeList = new List<SelectTreeModel>();
            foreach (var item in data1.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            ViewBag.lable = treeList.TreeGridJson();
            var data2 = _channelservice.GetList();
            var treeList2 = new List<SelectTreeModel>();
            foreach (var item in data2.Result)
            {
                //此处需修改
                SelectTreeModel treeModel = new SelectTreeModel();
                treeModel.value = item.F_Id;
                treeModel.name = item.F_ChannelName;
                treeModel.parentId = item.F_ParentId;
                treeList2.Add(treeModel);
            }
            ViewBag.channel = treeList2.TreeGridJson();
            return View();
        }
        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(SoulPage<ExpertEntity> pagination, string keyword)
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
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(ExpertEntity entity, string keyValue)
        {
            ExpertEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                if (string.IsNullOrEmpty(entity.F_Introduction))
                {
                    entity.F_Introduction = Cornerstone.Code.TextHelper.GetSubString(Cornerstone.Code.WebHelper.NoHtml(entity.F_Introduction), 255);
                }
                if (!string.IsNullOrEmpty(entity.F_Introduction))
                {
                    entity.F_Introduction = entity.F_Introduction.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\"", "\\\""); ;
                }
                if (!string.IsNullOrEmpty(entity.F_Name))
                {
                    entity.F_Initials = Cornerstone.Code.TextHelper.GetSpellCode(entity.F_Name);
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
        [ServiceFilter(typeof(HandlerAuthorizeAttribute))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteForm(string keyValue)
        {
            ExpertEntity data = null;
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
