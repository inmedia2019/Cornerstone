using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.DictionaryDataBase;
using Cornerstone.Domain.ContentManage;
using Newtonsoft.Json;

namespace Cornerstone.Service.DictionaryDataBase
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-03-18 14:06
    /// 描 述：专家管理服务类
    /// </summary>
    public class ExpertService : DataFilterService<ExpertEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_expertdata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public ExpertService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<ExpertEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Name.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<ExpertEntity>> GetLookList(string keyword = "")
        {
            var list = new List<ExpertEntity>();
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
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList(), className.Substring(0, className.Length - 7));
        }

        public async Task<List<ExpertEntity>> GetLookList(SoulPage<ExpertEntity> pagination, string keyword = "")
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
        public async Task<int> GetListCountByHospitalId(string hospitalId)
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(hospitalId))
            {
                cachedata = cachedata.Where(t => t.F_HospitalId == hospitalId).ToList();

            }
            var listCount = cachedata.Where(t => t.F_DeleteMark == false).ToList().Count;
            return listCount;
        }

        public async Task<ExpertEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<ExpertEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(ExpertEntity entity, string keyValue)
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
                    if (entity.F_IsEP == true)
                    {
                        NewsInfo info = new NewsInfo();
                        info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_Name.ToString() + "6216ad83-ce39-49bc-a3f0-50871aad302f" + entity.F_MarkType.ToString() + entity.F_CreatorTime.ToString().Replace("-", "/") + "1" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                        info.Id = entity.F_Id.ToString();
                        info.Title = entity.F_Name.ToString();
                        info.ParentId = "6216ad83-ce39-49bc-a3f0-50871aad302f";
                        info.TagId = entity.F_MarkType.ToString();
                        info.Author = "";
                        info.PubDate = entity.F_CreatorTime.ToString().Replace("-", "/");
                        info.opType = 1;
                        datastr = JsonConvert.SerializeObject(info);
                        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/NewsInfo/OPNewsInfo", "POST", "", null, datastr);
                    }
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
                    var opType = entity.F_EnabledMark == false ? 4 : 2;
                    if (entity.F_IsEP == true)
                    {
                        NewsInfo info = new NewsInfo();
                        info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_Name.ToString() + "6216ad83-ce39-49bc-a3f0-50871aad302f" + entity.F_MarkType.ToString() + entity.F_CreatorTime.ToString().Replace("-", "/") + opType.ToString() + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                        info.Id = entity.F_Id.ToString();
                        info.Title = entity.F_Name.ToString();
                        info.ParentId = "6216ad83-ce39-49bc-a3f0-50871aad302f";
                        info.TagId = entity.F_MarkType.ToString();
                        info.Author = "";
                        info.PubDate = entity.F_CreatorTime.ToString().Replace("-", "/");
                        info.opType = opType;
                        datastr = JsonConvert.SerializeObject(info);
                        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/NewsInfo/OPNewsInfo", "POST", "", null, datastr);
                    }
                }
                catch (Exception ex) { }
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
                try
                {
                    var datastr = "";
                    if (cachedata.F_IsEP == true)
                    {
                        NewsInfo info = new NewsInfo();
                        info.sign = Md5.md5(cachedata.F_Id.ToString() + cachedata.F_Name.ToString() + "6216ad83-ce39-49bc-a3f0-50871aad302f" + cachedata.F_MarkType.ToString() + cachedata.F_CreatorTime.ToString().Replace("-", "/") + "3" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                        info.Id = cachedata.F_Id.ToString();
                        info.Title = cachedata.F_Name.ToString();
                        info.ParentId = "6216ad83-ce39-49bc-a3f0-50871aad302f";
                        info.TagId = cachedata.F_MarkType.ToString();
                        info.Author = "";
                        info.PubDate = cachedata.F_CreatorTime.ToString().Replace("-", "/");
                        info.opType = 3;
                        datastr = JsonConvert.SerializeObject(info);
                        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/NewsInfo/OPNewsInfo", "POST", "", null, datastr);
                    }
                }
                catch (Exception ex) { }
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
