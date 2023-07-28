using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.ContentManage;
using Newtonsoft.Json;

namespace Cornerstone.Service.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-11 11:37
    /// 描 述：标签管理服务类
    /// </summary>
    public class LableService : DataFilterService<LableEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_labledata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public LableService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<LableEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Name.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_Order).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<LableEntity>> GetLookList(string keyword = "")
        {
            var list = new List<LableEntity>();
            if (!CheckDataPrivilege(className.Substring(0, className.Length - 7)))
            {
                list = await repository.CheckCacheList(cacheKey + "list");
            }
            else
            {
                var forms = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
                list = forms.ToList();
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Name.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false && t.F_Name != null).OrderByDescending(t => t.F_CreatorTime).ToList(), className.Substring(0, className.Length - 7));
        }

        public async Task<List<LableEntity>> GetLookList(SoulPage<LableEntity> pagination, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Name.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false);
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }

        public async Task<LableEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task<LableEntity> GetForm(string keyValue, string lan = "0")
        {
            //var cachedata = await repository.CheckCache(cacheKey, keyValue);
            var cachedata = await repository.FindList("select * from cms_lable where F_LanInfoId='" + keyValue.Replace("'", "") + "' and F_lan=" + lan + " and F_DeleteMark=false order by F_Order desc");
            return cachedata.FirstOrDefault();
        }

        public async Task<List<LableEntity>> GetListByLan(string lan = "0")
        {
            var cachedata = await repository.FindList("select * from cms_lable where F_lan=" + lan + " and F_DeleteMark=false order by F_Order desc");
            return cachedata;
        }
        #endregion

        public async Task<LableEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(LableEntity entity, string keyValue)
        {
            var datastr = "";
            if (string.IsNullOrEmpty(keyValue))
            {
                //此处需修改
                entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
            
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                //此处需修改
                entity.Modify(keyValue);
                await repository.Update(entity);
              
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            foreach (var item in ids)
            {
                var cachedata = await repository.CheckCache(cacheKey, item);
               
                cachedata.F_EnabledMark = false;
                cachedata.F_DeleteMark = true;
                cachedata.Modify(item);
                await repository.Update(cachedata);
                await CacheHelper.Remove(cacheKey + item);
            }
            //await repository.Delete(t => ids.Contains(t.F_Id));
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
