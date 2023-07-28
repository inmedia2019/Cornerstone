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
    /// 日 期：2022-05-11 17:39
    /// 描 述：用户书单服务类
    /// </summary>
    public class FavitorinfoService : DataFilterService<FavitorinfoEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_favitorinfodata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public FavitorinfoService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<FavitorinfoEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.infoTitle.Contains(keyword) || t.morecol.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.state == 0).OrderByDescending(t => t.createDate).ToList();
        }

        public async Task<List<FavitorinfoEntity>> GetListByMid(string mid = "")
        {
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");

            //return cachedata.Where(t => t.mid == mid).OrderByDescending(t => t.createDate).ToList();
            var list = await repository.FindList("select * from web_favitorinfo where mid='" + mid.Replace("'", "") + "' order by createDate desc");
            return list;
        }

        public async Task<List<FavitorinfoEntity>> GetListByMidAndParentId(string mid = "", string parentId = "")
        {
            var temp = parentId.Split(',');
            string ids = "";
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Length > 0)
                {
                    ids += "'" + temp[i].Replace("'", "") + "',";
                }
            }

            if (ids.Length > 0)
                ids = ids.Substring(0, ids.Length - 1);

            string tempSql = "";
            if (ids.Length > 0)
            {
                tempSql += " and infoParentId in(" + ids + ")";
            }

            var list = await repository.FindList("select *, concat('" + GlobalContext.SystemConfig.url + "',infoBanner)  as PicUrl from web_favitorinfo where mid='" + mid.Replace("'", "") + "'" + tempSql);

            return list;
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");

            //return cachedata.Where(t => t.mid == mid && t.infoParentId == parentId).OrderByDescending(t => t.createDate).ToList();
        }

        /// <summary>
        /// 查询是否已经收藏
        /// </summary>
        /// <param name="mid">用户ID</param>
        /// <param name="infoId">信息ID</param>
        /// <returns></returns>
        public async Task<int> GetFavitorRecordCount(string mid = "", string infoId = "")
        {
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");
            //if (!string.IsNullOrEmpty(mid))
            //{
            //    //此处需修改
            //    cachedata = cachedata.Where(t => t.mid == mid && t.infoId == infoId).ToList();
            //}
            //return cachedata.ToList().Count;
            var list = await repository.FindList("select * from web_favitorinfo where mid='" + mid.Replace("'", "") + "' and infoId='" + infoId.Replace("'", "") + "'");
            return list.Count;

        }

        public async Task<List<FavitorinfoEntity>> GetLookList(string keyword = "")
        {
            var list =new List<FavitorinfoEntity>();
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
                list = list.Where(u => u.infoTitle.Contains(keyword) || u.morecol.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.state == 0).OrderByDescending(t => t.createDate).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<List<FavitorinfoEntity>> GetLookList(SoulPage<FavitorinfoEntity> pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.infoTitle.Contains(keyword) || u.morecol.Contains(keyword));
            }
            list = list.Where(u => u.state == 0);

            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<FavitorinfoEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<FavitorinfoEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task<string> SubmitForm(FavitorinfoEntity entity, string keyValue)
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
                //此处需修改
                entity.F_Id = keyValue;
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

        public async Task<int> DeleteForm(string mid,string infoId)
        {

           return await repository.Delete(t => t.mid == mid && t.infoId == infoId);
           
        }
        #endregion

    }
}
