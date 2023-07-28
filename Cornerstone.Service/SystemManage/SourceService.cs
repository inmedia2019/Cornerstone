using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.SystemManage;
using Newtonsoft.Json;
using Cornerstone.Domain.Entity.ContentManage;

namespace Cornerstone.Service.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-09 15:08
    /// 描 述：来源类型服务类
    /// </summary>
    public class SourceService : DataFilterService<SourceEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_sourcedata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public SourceService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<SourceEntity>> GetList(string keyword = "",string type = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Name.Contains(keyword)).ToList();
            }
            if (!string.IsNullOrEmpty(type))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Type == type).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<SourceEntity>> GetLookList(string keyword = "",string type = "")
        {
            var list =new List<SourceEntity>();
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
            if (!string.IsNullOrEmpty(type))
            {
                //此处需修改
                list = list.Where(u => u.F_Type == type).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList(),className.Substring(0, className.Length - 7));
        }

        public async Task<List<SourceEntity>> GetLookList(SoulPage<SourceEntity> pagination,string keyword = "", string type = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Name.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(type))
            {
                //此处需修改
                list = list.Where(u => u.F_Type==type);
            }
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<SourceEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<SourceEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(SourceEntity entity, string keyValue)
        {
            var datastr = "";
            if (string.IsNullOrEmpty(keyValue))
            {
                    //此处需修改
                    entity.F_DeleteMark = false;
                entity.Create(entity.F_Id);
                await repository.Insert(entity);
                try
                {
                    SourceInfo info = new SourceInfo();
                    info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_Name.ToString() + "1" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                    info.Id = entity.F_Id.ToString();
                    info.Title = entity.F_Name.ToString();
                    info.opType = 1;
                    info.ParentId = entity.F_ParentId;
                    datastr = JsonConvert.SerializeObject(info);
                    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/SourceTypeInfo/OPSourceTypeInfo", "POST", "", null, datastr);
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
                    SourceInfo info = new SourceInfo();
                    info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_Name.ToString()  + "2" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                    info.Id = entity.F_Id.ToString();
                    info.Title = entity.F_Name.ToString();
                    info.opType = 2;
                    info.ParentId = entity.F_ParentId;
                    datastr = JsonConvert.SerializeObject(info);
                    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/SourceTypeInfo/OPSourceTypeInfo", "POST", "", null, datastr);
                }
                catch (Exception ex) { }
          
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }

        public async Task DeleteForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            //await repository.Delete(t => ids.Contains(t.F_Id));
            foreach (var item in ids)
            {
                var cachedata = await repository.CheckCache(cacheKey, item);
                try
                {
                    var datastr = "";
                    SourceInfo info = new SourceInfo();
                    info.sign = Md5.md5(cachedata.F_Id.ToString() + cachedata.F_Name.ToString() + "3" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                    info.Id = cachedata.F_Id.ToString();
                    info.Title = cachedata.F_Name.ToString();
                    info.opType = 3;
                    info.ParentId = cachedata.F_ParentId;
                    datastr = JsonConvert.SerializeObject(info);
                    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/SourceTypeInfo/OPSourceTypeInfo", "POST", "", null, datastr);
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
