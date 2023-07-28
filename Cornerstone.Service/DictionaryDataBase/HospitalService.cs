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
    /// 日 期：2021-03-18 14:03
    /// 描 述：医院管理服务类
    /// </summary>
    public class HospitalService : DataFilterService<HospitalEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_hospitaldata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public HospitalService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<HospitalEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_HospitalName.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }


        public async Task<List<HospitalEntity>> GetLookList(string keyword = "")
        {
            var list =new List<HospitalEntity>();
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
                list = list.Where(u => u.F_HospitalName.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<List<HospitalEntity>> GetLookList(SoulPage<HospitalEntity> pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_HospitalName.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<HospitalEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<HospitalEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(HospitalEntity entity,string province, string keyValue)
        {
            var datastr = "";
            if (string.IsNullOrEmpty(keyValue))
            {
                    //此处需修改
                 entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
                try
                {
                    HospitalInfo info = new HospitalInfo();
                    info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_HospitalName.ToString() + province + entity.F_Code.ToString() + "1" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                    info.Id = entity.F_Id.ToString();
                    info.HospitalName = entity.F_HospitalName.ToString();
                    info.Province = province;
                    info.HospitalCode = entity.F_Code.ToString();
                    info.opType = 1;
                    datastr = JsonConvert.SerializeObject(info);
                    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/HospitalInfo/OPHospitalInfo", "POST", "", null, datastr);
                }
                catch (Exception ex) { }
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                    //此处需修改
                entity.Modify(keyValue); 
                await repository.Update(entity);
                try
                {
                    HospitalInfo info = new HospitalInfo();
                    info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_HospitalName.ToString() + province + entity.F_Code.ToString() + "2" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                    info.Id = entity.F_Id.ToString();
                    info.HospitalName = entity.F_HospitalName.ToString();
                    info.Province = province;
                    info.HospitalCode = entity.F_Code.ToString();
                    info.opType = 2;
                    datastr = JsonConvert.SerializeObject(info);
                    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/HospitalInfo/OPHospitalInfo", "POST", "", null, datastr);
                }
                catch (Exception ex) { }

                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }

        public async Task DeleteForm(string province, string keyValue)
        {
            var ids = keyValue.Split(',');
            //await repository.Delete(t => ids.Contains(t.F_Id));
            foreach (var item in ids)
            {
                var cachedata = await repository.CheckCache(cacheKey, item);
                try
                {
                    var datastr = "";
                    HospitalInfo info = new HospitalInfo();
                    info.sign = Md5.md5(cachedata.F_Id.ToString() + cachedata.F_HospitalName.ToString() + province + cachedata.F_Code.ToString() + "3" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                    info.Id = cachedata.F_Id.ToString();
                    info.HospitalName = cachedata.F_HospitalName.ToString();
                    info.Province = province;
                    info.HospitalCode = cachedata.F_Code.ToString();
                    info.opType = 3;
                    datastr = JsonConvert.SerializeObject(info);
                    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/HospitalInfo/OPHospitalInfo", "POST", "", null, datastr);
                }
                catch (Exception ex) { }
                cachedata.F_EnabledMark = false;
                cachedata.F_DeleteMark = true;
                cachedata.Modify(item);
                await repository.Update(cachedata);
                await CacheHelper.Remove(cacheKey + item);
            }
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
