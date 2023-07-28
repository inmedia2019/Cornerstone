using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.InfoManage;

namespace Cornerstone.Service.InfoManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-05-11 15:29
    /// 描 述：用户留言服务类
    /// </summary>
    public class LivemessageService : DataFilterService<LivemessageEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_livemessagedata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public LivemessageService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<LivemessageEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Phone.Contains(keyword) || t.F_TrueName.Contains(keyword)).ToList();
            }
            return cachedata.OrderByDescending(t => t.F_createDate).ToList();
        }

        public async Task<List<LivemessageEntity>> GetLookList(string keyword = "")
        {
            var list =new List<LivemessageEntity>();
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
                list = list.Where(u => u.F_Phone.Contains(keyword) || u.F_TrueName.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.OrderByDescending(t => t.F_createDate).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<List<LivemessageEntity>> GetLookList(SoulPage<LivemessageEntity> pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Phone.Contains(keyword) || u.F_TrueName.Contains(keyword));
            }
          
            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<LivemessageEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<LivemessageEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task<string> SubmitForm(LivemessageEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                //此处需修改
                entity.F_Id = Utils.GuId();
                
                await repository.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                entity.F_Id = keyValue;
                //此处需修改
                entity.Modify(keyValue); 
                await repository.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            return entity.F_Id;
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Delete(t => ids.Contains(t.F_Id));
            foreach (var item in ids)
            {
            await CacheHelper.Remove(cacheKey + item);
            }
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
