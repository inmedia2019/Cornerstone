using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.DictionaryDataBase;

namespace Cornerstone.Service.DictionaryDataBase
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-05-04 17:14
    /// 描 述：信息列表服务类
    /// </summary>
    public class MemberinfoService : DataFilterService<MemberinfoEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_memberinfodata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public MemberinfoService(IDbContext context) : base(context)
        {
        }

        #region 获取数据
        public async Task<List<MemberinfoEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.trueName.Contains(keyword) || t.phone.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.state != 3).OrderByDescending(t => t.createDate).ToList();
        }

        public async Task<List<MemberinfoEntity>> GetLookList(string keyword = "")
        {
            var list =new List<MemberinfoEntity>();
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
                list = list.Where(u => u.trueName.Contains(keyword) || u.phone.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.state != 3).OrderByDescending(t => t.createDate).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<List<MemberinfoEntity>> GetLookList(SoulPage<MemberinfoEntity> pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.trueName.Contains(keyword) || u.phone.Contains(keyword));
            }
            list = list.Where(u => u.state != 3);
            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<MemberinfoEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<MemberinfoEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        public async Task<MemberinfoEntity> GetUserInfoByOpenId(string openId)
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
           
             //此处需修改
            cachedata = cachedata.Where(t => t.openId== openId).ToList();

            return cachedata.FirstOrDefault();
        }

        public async Task<MemberinfoEntity> GetUserInfoByPhone(string phone,string pwd)
        {
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");

            ////此处需修改
            //cachedata = cachedata.Where(t => t.phone == phone).ToList();



            //return cachedata.FirstOrDefault();

            MemberinfoEntity query = null;
            if (!string.IsNullOrEmpty(phone))
            {
                var list = await repository.FindList("select web_memberinfo.* from web_memberinfo where phone='" + phone.Replace("'", "") + "' and moreCol='" + pwd.Replace("'", "") + "'");
               
                query = list.FirstOrDefault();
            }
            return query;

        }

        public async Task<MemberinfoEntity> GetUserInfoByPhone(string phone)
        {
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");

            ////此处需修改
            //cachedata = cachedata.Where(t => t.phone == phone).ToList();



            //return cachedata.FirstOrDefault();

            MemberinfoEntity query = null;
            if (!string.IsNullOrEmpty(phone))
            {
                var list = await repository.FindList("select web_memberinfo.* from web_memberinfo where phone='" + phone.Replace("'", "") + "'");

                query = list.FirstOrDefault();
            }
            return query;

        }

        public async Task<MemberinfoEntity> GetUserInfoByMid(string mid)
        {
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");

            ////此处需修改
            //cachedata = cachedata.Where(t => t.phone == phone).ToList();



            //return cachedata.FirstOrDefault();

            MemberinfoEntity query = null;
            if (!string.IsNullOrEmpty(mid))
            {
                var list = await repository.FindList("select web_memberinfo.*,(select count(1) from web_useractioninfo where moreCol='0' and tid=1 and mid=web_memberinfo.Id and createDate>='" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' and createDate<='" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59') as IsSgin from web_memberinfo where Id='" + mid.Replace("'", "") + "' ");

                query = list.FirstOrDefault();
            }
            return query;

        }

        #region 提交数据
        public async Task<string> SubmitForm(MemberinfoEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                    //此处需修改
                   // entity.F_DeleteMark = false;
                entity.Id=Utils.GuId();
                await repository.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                //此处需修改
                entity.Id = keyValue;
                await repository.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
            return entity.Id;
        }

        public async Task<int> GetRecordCount(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.openId.Contains(keyword)).ToList();
            }
            return cachedata.ToList().Count;

        }

        public async Task<int> GetRecordCountByPhone(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.phone.Contains(keyword)).ToList();
            }
            return cachedata.ToList().Count;

        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            await repository.Delete(t => ids.Contains(t.Id));
            foreach (var item in ids)
            {
            await CacheHelper.Remove(cacheKey + item);
            }
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
