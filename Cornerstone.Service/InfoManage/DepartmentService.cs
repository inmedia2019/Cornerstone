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
    /// 日 期：2023-07-18 10:47
    /// 描 述：科室管理服务类
    /// </summary>
    public class DepartmentService : DataFilterService<DepartmentEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_departmentdata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public DepartmentService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<DepartmentEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Name.Contains(keyword) || t.F_Code.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<DepartmentEntity>> GetLookList(string keyword = "")
        {
            var list =new List<DepartmentEntity>();
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
                list = list.Where(u => u.F_Name.Contains(keyword) || u.F_Code.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<List<DepartmentEntity>> GetLookList(SoulPage<DepartmentEntity> pagination,string keyword = "", string lan = "0")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Name.Contains(keyword) || u.F_Code.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark== false && u.F_lan.ToString() == lan);
            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<DepartmentEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }

        public async Task<DepartmentEntity> GetForm(string keyValue, string lan = "0")
        {
            //var cachedata = await repository.CheckCache(cacheKey, keyValue);
            var cachedata = await repository.FindList("select * from dic_department where F_LanInfoId='" + keyValue.Replace("'", "") + "' and F_lan=" + lan + " and F_DeleteMark=false order by F_Order desc");
            return cachedata.FirstOrDefault();
        }

        #endregion

        public async Task<DepartmentEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        public async Task<List<DepartmentEntity>> GetListByLan(string lan = "0")
        {
            var cachedata = await repository.FindList("select * from dic_department where F_lan=" + lan + " and F_DeleteMark=false order by F_Order desc");
            return cachedata;
        }

        #region 提交数据
        public async Task SubmitForm(DepartmentEntity entity, string keyValue)
        {
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
