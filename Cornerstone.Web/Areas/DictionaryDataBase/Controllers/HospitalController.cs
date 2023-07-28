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
using Cornerstone.Service.SystemManage;
using Cornerstone.Domain.Entity.DictionaryDataBase;

namespace Cornerstone.Web.Areas.DictionaryDataBase.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-03-18 14:03
    /// 描 述：医院管理控制器类
    /// </summary>
    [Area("DictionaryDataBase")]
    public class HospitalController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public HospitalService _service {get;set; }
        public UpdateHospitalLogService _updateHospitalLogservice { get; set; }
        public ItemsDataService _itmsdataservice { get; set; }
        public ItemsTypeService _itemstypeservice { get; set; }
        public ExpertService _expertservice { get; set; }
        public override ActionResult Form()
        {
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
            ViewBag.Content = _service.GetForm(keyValue).Result.ToJson();
            return View();
        }
        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(SoulPage<HospitalEntity> pagination, string keyword)
        {
            if (string.IsNullOrEmpty(pagination.field))
            {
                pagination.field = "F_Number";
                pagination.order = "desc";
            }
            var data = await _service.GetLookList(pagination,keyword);
            foreach (HospitalEntity h in data)
            {
                h.F_Number = await _expertservice.GetListCountByHospitalId(h.F_Id);
            }
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
                treeModel.text = item.F_HospitalName;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(HospitalEntity entity, string keyValue)
        {
            HospitalEntity data = null;
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
            }
            try
            {
                var typelist = await _itemstypeservice.GetList();
                var type = typelist.Where(a => a.F_EnCode == "AreaType").FirstOrDefault();
                var typedetaillist = await _itmsdataservice.GetList();
                var typedetail = typedetaillist.Where(a => a.F_ItemCode == entity.F_Province && a.F_ItemId== type.F_Id).FirstOrDefault();
                var province = typedetail != null ? typedetail.F_ItemName : "";
                if (string.IsNullOrEmpty(entity.F_Introduction))
                {
                    entity.F_Introduction = Cornerstone.Code.TextHelper.GetSubString(Cornerstone.Code.WebHelper.NoHtml(entity.F_Introduction), 255);
                }
                if (!string.IsNullOrEmpty(entity.F_Introduction))
                {
                    entity.F_Introduction = entity.F_Introduction.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\"", "\\\""); ;
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    if (data.F_HospitalName != entity.F_HospitalName)
                    {
                        UpdateHospitalLogEntity uentity = new UpdateHospitalLogEntity
                        {
                            F_Id = Utils.GuId(),
                            F_BeforeChange = data.F_HospitalName,
                            F_AfterChange = entity.F_HospitalName,
                            F_CreatorUserId = entity.F_CreatorUserId,
                            F_CreatorTime = DateTime.Now
                        };
                        await _updateHospitalLogservice.SubmitForm(uentity);
                    }
                }
                await _service.SubmitForm(entity, province, keyValue);
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
           
            HospitalEntity data = null;
            var province = "";
            if (!string.IsNullOrEmpty(keyValue))
            {
                data = await _service.GetForm(keyValue);
                var typelist = await _itemstypeservice.GetList();
                var type = typelist.Where(a => a.F_EnCode == "AreaType").FirstOrDefault();
                var typedetaillist = await _itmsdataservice.GetList();
                var typedetail = typedetaillist.Where(a => a.F_ItemCode == data.F_Province && a.F_ItemId == type.F_Id).FirstOrDefault();
                province = typedetail != null ? typedetail.F_ItemName : "";
            }
            try
            {
                await _service.DeleteForm(province,keyValue);
                return await Success("操作成功。", className, data, data, keyValue, DbLogType.Delete);
            }
            catch (Exception ex)
            {
                return await Error(ex.Message, className, data, data, keyValue, DbLogType.Delete);
            }
        }


        #endregion
        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetProvince()
        {
            var typelist = await _itemstypeservice.GetList();
            var type = typelist.Where(a => a.F_EnCode == "AreaType").FirstOrDefault();
            var data = await _itmsdataservice.GetList(type.F_Id);
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_ItemCode;
                treeModel.text = item.F_ItemName;
                treeList.Add(treeModel);
            }
            return Content(treeList.ToJson());
        }
    }
}
