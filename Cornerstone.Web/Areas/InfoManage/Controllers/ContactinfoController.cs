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
    /// 日 期：2023-02-23 16:15
    /// 描 述：Contactinfo控制器类
    /// </summary>
    [Area("InfoManage")]
    public class ContactinfoController :  ControllerBase
    {
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[5];
        public ContactinfoService _service {get;set;}
        public override ActionResult Form()
        {
            //控制器视图传值示例
            if (_service.currentuser.UserId == null)
            {
                return View();
            }
            var keyValue = HttpContext.Request.Query["keyValue"].ToString();
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

            if (keyValue != "")
            {
                var lanEntity = _service.GetForm(keyValue).Result;

                ViewBag.Content = _service.GetForm(lanEntity.F_LanInfoId, lanInfo).Result.ToJson();
            }
            //ViewBag.Content = _service.GetForm(keyValue).Result.ToJson();
            return View();
        }
        #region 获取数据
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetTreeGridJson(string keyword = "")
        {
            var data = await _service.GetLookList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_Name.Contains(keyword));
            }


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

            data = data.TreeWhere(t => Convert.ToString(t.F_lan) == lanInfo);

            return Success(data.Count, data);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public async Task<ActionResult> GetGridJson(SoulPage<ContactinfoEntity> pagination, string keyword)
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
        public async Task<ActionResult> GetTreeSelectJson()
        {
            var data = await _service.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                //此处需修改
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_Name;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
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
        public async Task<ActionResult> SubmitForm(ContactinfoEntity entity, string keyValue)
        {
           ContactinfoEntity data = null;
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
                data = await _service.GetForm(entity.F_LanInfoId, lan);
                keyValue = data.F_Id;
                entity.F_Id = data.F_Id;
            }
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //data = await _service.GetForm(keyValue);
            //}
            try
            {
                ContactinfoEntity entitys = new ContactinfoEntity();

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
                        entitys.F_lan = 1;
                        entitys.F_LanInfoId = guid;
                        entitys.F_ChannelId = entity.F_ChannelId;
                        entitys.F_ParentId = entity.F_ParentId;
                        entitys.F_Order = "0";
                        await _service.SubmitForm(entitys, "");
                    }
                }
                else
                {
                    var lanEntity = await _service.GetForm(entity.F_LanInfoId, "0");
                    if (lanEntity == null)
                    {
                        entitys.F_lan = 0;
                        entitys.F_LanInfoId = guid;
                        entitys.F_ChannelId = entity.F_ChannelId;
                        entitys.F_ParentId = entity.F_ParentId;
                        entitys.F_Order = "0";
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
           ContactinfoEntity data = null;
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
