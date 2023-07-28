using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.ContentManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cornerstone.Service.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-23 16:52
    /// 描 述：CMS栏目管理服务类
    /// </summary>
    public class ChannelService : DataFilterService<ChannelEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_channeldata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public ChannelService(IDbContext context) : base(context)
        {
        }

        #region 获取数据
        public async Task<List<ChannelEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_ChannelName.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

       

        public async Task<List<ChannelEntity>> GetLookList(string keyword = "")
        {
            var list = new List<ChannelEntity>();
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
                list = list.Where(u => u.F_ChannelName.Contains(keyword)).ToList();
            }
            return GetFieldsFilterData(list.Where(t => t.F_DeleteMark == false && t.F_ChannelName != null).OrderBy(t => t.F_Order).ToList(), className.Substring(0, className.Length - 7));
        }

        public async Task<List<ChannelEntity>> GetLookList(SoulPage<ChannelEntity> pagination, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_ChannelName.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false);
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }

        public async Task<ChannelEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        public async Task<ChannelEntity> GetForm(string keyValue, string lan = "0")
        {
            //var cachedata = await repository.CheckCache(cacheKey, keyValue);
            var cachedata = await repository.FindList("select * from cms_channel where F_LanInfoId='" + keyValue.Replace("'", "") + "' and F_lan=" + lan + " and F_DeleteMark=false order by F_Order desc");
            return cachedata.FirstOrDefault();
        }
        #endregion

        public async Task<ChannelEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }

        public async Task<ChannelEntity> GetInfoByChannelIndex(string channelname)
        {
            ChannelEntity query = null;
            if (!string.IsNullOrEmpty(channelname))
            {
                query = await repository.FindEntity(a => a.F_ChannelName == channelname && a.F_EnabledMark == true);

            }
            return query;
        }
        public async Task<ChannelEntity> GetInfoById(string id)
        {
            ChannelEntity query = null;
            if (!string.IsNullOrEmpty(id))
            {
                query = await repository.FindEntity(a => a.F_Id == id && a.F_EnabledMark == true);

            }
            return query;
        }
        public async Task<List<ChannelEntity>> GetListByParentId(string pid)
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(pid))
            {
                cachedata = cachedata.Where(t => t.F_ParentId == pid).ToList();

            }
            var list = cachedata.Where(t => t.F_DeleteMark == false).OrderBy(t => t.F_Order).ToList();
            return list;
        }

        public async Task<List<ChannelEntity>> GetListByParentId(string pid, string keyWord)
        {
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");
            //if (!string.IsNullOrEmpty(pid))
            //{
            //    cachedata = cachedata.Where(t => t.F_ParentId == pid).ToList();

            //}

            //if (!string.IsNullOrEmpty(keyWord))
            //{
            //    cachedata = cachedata.Where(t => t.F_Title.Contains(keyWord)).ToList();
            //}

            //var list = cachedata.Where(t => t.F_DeleteMark == false).OrderBy(t => t.F_Order).ToList();

            var list = await repository.FindList("select *, concat('" + GlobalContext.SystemConfig.url + "',F_ChannelImages)  as PicUrl from cms_channel where F_ParentId='" + pid.Replace("'","") + "' order by F_Order desc");
            list = list.Where(t => t.F_DeleteMark == false).ToList();
            if (!string.IsNullOrEmpty(keyWord))
            {
                list = list.Where(t => t.F_Title.Contains(keyWord)).ToList();
            }

            return list;

        }



        #region 提交数据
        public async Task SubmitForm(ChannelEntity entity, string keyValue)
        {
            var datastr = "";
            if (string.IsNullOrEmpty(keyValue))
            {
                //此处需新增
                entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
                //try
                //{
                //    ColumnInfo info = new ColumnInfo();
                //    var columnType = 1;
                //    if (entity.F_ChannelTemplate.ToString() == "86c1c0b3-f32c-47f4-bda2-c2721872e4b0" || entity.F_ChannelTemplate.ToString() == "be045122-f22a-42cb-950f-c7b5f20788e4" || entity.F_ChannelTemplate.ToString() == "4526a294-0277-4f2b-b9c0-6e89a6f11652")
                //    {
                //        columnType = 2;
                //    }
                //    else if (entity.F_ChannelTemplate.ToString() == "ba809475-dd25-42ea-a33d-eab5c0d3ad83")
                //    {
                //        columnType = 3;
                //    }
                //    info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_ChannelName.ToString() + entity.F_ParentId.ToString() + columnType.ToString() + "1" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //    info.Id = entity.F_Id.ToString();
                //    info.Title = entity.F_ChannelName.ToString();
                //    info.ParentId = entity.F_ParentId.ToString();
                //    info.ColumnType = columnType;
                //    info.opType = 1;
                //    datastr = JsonConvert.SerializeObject(info);
                //    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/ColumnInfo/OPColumnInfo", "POST", "", null, datastr);
                //}
                //catch (Exception ex) { }
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                //此处需修改
                entity.Modify(keyValue);
                await repository.Update(entity);
                //try
                //{
                //    ColumnInfo info = new ColumnInfo();
                //    var opType = entity.F_EnabledMark == false ? 4 : 2;
                //    var columnType = 1;
                //    if (entity.F_ChannelTemplate.ToString() == "86c1c0b3-f32c-47f4-bda2-c2721872e4b0" || entity.F_ChannelTemplate.ToString() == "be045122-f22a-42cb-950f-c7b5f20788e4" || entity.F_ChannelTemplate.ToString() == "4526a294-0277-4f2b-b9c0-6e89a6f11652")
                //    {
                //        columnType = 2;
                //    }
                //    else if (entity.F_ChannelTemplate.ToString() == "ba809475-dd25-42ea-a33d-eab5c0d3ad83")
                //    {
                //        columnType = 3;
                //    }
                //    info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_ChannelName.ToString() + entity.F_ParentId.ToString() + columnType.ToString() + opType.ToString() + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //    info.Id = entity.F_Id.ToString();
                //    info.Title = entity.F_ChannelName.ToString();
                //    info.ParentId = entity.F_ParentId.ToString();
                //    info.ColumnType = columnType;
                //    info.opType = opType;
                //    datastr = JsonConvert.SerializeObject(info);
                //    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/ColumnInfo/OPColumnInfo", "POST", "", null, datastr);
                //}
                //catch (Exception ex) { }
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
                //try
                //{
                //    var datastr = "";
                //    ColumnInfo info = new ColumnInfo();
                //    var columnType = 1;
                //    if (cachedata.F_ChannelTemplate.ToString() == "86c1c0b3-f32c-47f4-bda2-c2721872e4b0" || cachedata.F_ChannelTemplate.ToString() == "be045122-f22a-42cb-950f-c7b5f20788e4" || cachedata.F_ChannelTemplate.ToString() == "4526a294-0277-4f2b-b9c0-6e89a6f11652")
                //    {
                //        columnType = 2;
                //    }
                //    else if (cachedata.F_ChannelTemplate.ToString() == "ba809475-dd25-42ea-a33d-eab5c0d3ad83")
                //    {
                //        columnType = 3;
                //    }
                //    info.sign = Md5.md5(cachedata.F_Id.ToString() + cachedata.F_ChannelName.ToString() + cachedata.F_ParentId.ToString() + columnType.ToString() + "3" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //    info.Id = cachedata.F_Id.ToString();
                //    info.Title = cachedata.F_ChannelName.ToString();
                //    info.ParentId = cachedata.F_ParentId.ToString();
                //    info.ColumnType = columnType;
                //    info.opType = 3;
                //    datastr = JsonConvert.SerializeObject(info);
                //    string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/ColumnInfo/OPColumnInfo", "POST", "", null, datastr);
                //}
                //catch (Exception ex) { }
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
