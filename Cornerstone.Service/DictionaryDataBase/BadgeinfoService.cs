using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.DictionaryDataBase;
using Cornerstone.Domain.Entity.ContentManage;
using Newtonsoft.Json;

namespace Cornerstone.Service.DictionaryDataBase
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-04-28 16:55
    /// 描 述：Badgeinfo服务类
    /// </summary>
    public class BadgeinfoService : DataFilterService<BadgeinfoEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_badgeinfodata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public BadgeinfoService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<BadgeinfoEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Name.Contains(keyword)).ToList();
            }
            
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<BadgeinfoEntity>> GetAllBadgeInfoList(string keyword = "")
        {
            var data = await repository.FindList("select * from dic_badgeinfo where F_DeleteMark=false order by F_Order desc");
            return data;
        }


        /// <summary>
        /// 根据徽章所需最小值获取徽章
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="parentId">所属类别</param>
        /// <returns></returns>
        public async Task<BadgeinfoEntity> GetInfoByFilter(int? minValue,string parentId)
        {
           // BadgeinfoEntity query = null;

            //var cachedata = await repository.CheckCacheList(cacheKey + "list");

            //query = cachedata.Where(a => a.F_MinScore <= minValue && a.F_badgetTypeId == parentId).OrderByDescending(t => t.F_MinScore).ToList().FirstOrDefault();

            var data = await repository.FindList("select * from dic_badgeinfo where F_DeleteMark=false and F_MinScore<="+ minValue + " and F_badgetTypeId='"+ parentId.Replace("'", "") + "' order by F_MinScore desc");

            return data.FirstOrDefault();
        }

        public async Task<List<BadgeinfoEntity>> GetLookList(string keyword = "")
        {
            var list =new List<BadgeinfoEntity>();
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
                list = list.Where(u => u.F_Name.Contains(keyword) ).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<List<BadgeinfoEntity>> GetLookList(SoulPage<BadgeinfoEntity> pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Name.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<List<BadgeinfoEntity>> GetLookList(SoulPage<BadgeinfoEntity> pagination, string keyword = "", string bType = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Name.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(bType) && bType != "0")
            {
                //此处需修改
                list = list.Where(u => u.F_badgetTypeId == bType);
            }
            list = list.Where(u => u.F_DeleteMark == false);
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }

        public async Task<BadgeinfoEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion


        public async Task<List<BadgeinfoEntity>> GetAllList(string keyword = "")
        {
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    //此处需修改
            //    cachedata = cachedata.Where(t => t.F_Name.Contains(keyword)).ToList();
            //}
            //return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
            string tempSql = "";
            if (!string.IsNullOrEmpty(keyword))
            {
                tempSql = " and F_Name like '%" + keyword.Replace("'", "") + "%'";
            }
            var list = await repository.FindList("select* from dic_badgeinfo where F_DeleteMark=false " + tempSql+ " order by F_Order desc");

            return list;
        }

        public async Task<BadgeinfoEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(BadgeinfoEntity entity, string keyValue)
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
