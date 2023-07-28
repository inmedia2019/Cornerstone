using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.ContentManage;

namespace Cornerstone.Service.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2023-02-21 13:39
    /// 描 述：Questiontypeinfo服务类
    /// </summary>
    public class QuestiontypeinfoService : DataFilterService<QuestiontypeinfoEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_questiontypeinfodata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public QuestiontypeinfoService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<QuestiontypeinfoEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.title.Contains(keyword) || t.descript.Contains(keyword)).ToList();
            }
            return cachedata.OrderByDescending(t => t.sn).OrderByDescending(t => t.createDate).ToList();
        }

        public async Task<List<QuestiontypeinfoEntity>> GetLookList(string keyword = "")
        {
            var list =new List<QuestiontypeinfoEntity>();
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
                list = list.Where(u => u.title.Contains(keyword) || u.descript.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.OrderByDescending(t => t.createDate).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<List<QuestiontypeinfoEntity>> GetLookList(SoulPage<QuestiontypeinfoEntity> pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.title.Contains(keyword) || u.descript.Contains(keyword));
            }
            
            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<QuestiontypeinfoEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<QuestiontypeinfoEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(QuestiontypeinfoEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                //此处需修改
                entity.F_Id = Guid.NewGuid().ToString();
                entity.createDate = DateTime.Now;
                //entity.Create();
                await repository.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                //此处需修改
                //entity.Modify(keyValue); 
                entity.F_Id = keyValue;
                entity.createDate = DateTime.Now;
                await repository.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
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
