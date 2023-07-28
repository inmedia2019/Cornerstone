using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.InfoManage;
using Cornerstone.Service;
using Microsoft.AspNetCore.Authorization;
using Cornerstone.Service.InfoManage;

namespace Cornerstone.Web.Areas.InfoManage.Controllers
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2023-07-18 10:47
    /// 描 述：科室管理控制器类
    /// </summary>
    [Area("InfoManage")]
    public class DepartmentController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public DepartmentService _service {get;set;}

        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(SoulPage<DepartmentEntity> pagination, string keyword)
        {
            if (string.IsNullOrEmpty(pagination.field))
            {
                pagination.field = "F_CreatorTime";
                pagination.order = "desc";
            }

            string lan = CacheHelper.Get<string>("lan").Result;
            if (lan == null)
                lan = "0";
            else
            {
                try
                {
                    lan = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }
            }

            var data = await _service.GetLookList(pagination, keyword, lan);
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
            var lan = CacheHelper.Get<string>("lan").Result;
            string lanInfo = "0";
            if (lan != null)
            {
                try
                {
                    lanInfo = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }

            }

            var data = await _service.GetLookForm(keyValue);
            if (!string.IsNullOrEmpty(keyValue))
            {
                var lanEntity = _service.GetForm(keyValue).Result;
                if (lanEntity != null)
                    data = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result;
            }
            return Content(data.ToJson());
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(DepartmentEntity entity, string keyValue)
        {
            DepartmentEntity data = null;
            string lan = CacheHelper.Get<string>("lan").Result;
            if (lan == null)
                lan = "0";
            else
            {
                try
                {
                    lan = lan.ToString().Split('_')[1];
                }
                catch (Exception)
                {


                }
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                //  data = await _service.GetForm(keyValue);
                data = await _service.GetForm(entity.F_LanInfoId, lan);
                keyValue = data.F_Id;
                entity.F_Id = data.F_Id;
            }
            try
            {
                DepartmentEntity entitys = new DepartmentEntity();

                entity.F_lan = Convert.ToInt32(lan);
                string guid = Guid.NewGuid().ToString();
                if (string.IsNullOrEmpty(keyValue))
                {
                    entity.F_LanInfoId = guid;
                }

                await _service.SubmitForm(entity, keyValue);

                if (lan == "0")
                {
                    var lanEntity = await _service.GetForm(entity.F_LanInfoId, "1");
                    if (lanEntity == null)
                    {
                        entitys.F_Name = "";
                        entitys.F_lan = 1;
                        entitys.F_LanInfoId = guid;
                        entitys.F_Order = 0;
                        await _service.SubmitForm(entitys, "");
                    }
                }
                else
                {
                    var lanEntity = await _service.GetForm(entity.F_LanInfoId, "0");
                    if (lanEntity == null)
                    {
                        entitys.F_Name = "";
                        entitys.F_lan = 0;
                        entitys.F_LanInfoId = guid;
                        entitys.F_Order = 0;
                     
                        await _service.SubmitForm(entitys, "");
                    }
                }
                return await Success("操作成功。", className, entity, data, entity.F_Id);
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
           DepartmentEntity data = null;
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
