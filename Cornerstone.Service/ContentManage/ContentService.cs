using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.ContentManage;
using System.Collections;
using Newtonsoft.Json;

namespace Cornerstone.Service.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-23 14:41
    /// 描 述：CMS内容管理服务类
    /// </summary>
    public class ContentService : DataFilterService<ContentEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_contentdata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public ContentService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<ContentEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_Titile.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }
        public async Task<List<ContentEntity>> GetMeetingTimePeriodList()
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            return cachedata.Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true && t.F_MeetingTimePeriod != "" && t.F_MeetingTimePeriod != null && t.F_MeetingEndTime < DateTime.Now).OrderByDescending(t => t.F_CreatorTime).ToList();
        }
        public async Task<ContentEntity> GetInfoById(string id)
        {
            ContentEntity query = null;
            if (!string.IsNullOrEmpty(id))
            {
                query = await repository.FindEntity(a => a.F_Id == id && a.F_EnabledMark == true);

            }
            return query;
        }

        public async Task<List<ContentEntity>> GetListByChannelId(string channelId, int page = 1, int psize = 10)
        {
            //var cachedata = await repository.CheckCacheList(cacheKey + "list");
            //if (!string.IsNullOrEmpty(channelId))
            //{
            //    cachedata = cachedata.Where(t => t.F_ChannelId == channelId).ToList();

            //}
            //var list = cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).OrderByDescending(t => t.F_IsTop).ToList().Skip((page - 1) * psize).Take(psize).AsQueryable().ToList();

            var list = await repository.FindList("select *,(SELECT F_ChannelName FROM cms_channel WHERE F_Id=cms_content.F_ChannelId) AS ParentTitle from cms_content where F_ChannelId='" + channelId.Replace("'", "") + "' and F_Titile is not null and F_DeleteMark=false order by F_Order desc, F_CreatorTime desc,F_IsTop desc");
            list = list.Skip((page - 1) * psize).Take(psize).AsQueryable().ToList();
            return list;

        }

        public async Task<List<ContentEntity>> GetListByChannelId(string channelId, int page = 1, int psize = 10, string tagId = "")
        {


            var list = await repository.FindList("select *,(SELECT F_ChannelName FROM cms_channel WHERE F_Id=cms_content.F_ChannelId) AS ParentTitle from cms_content where F_ChannelId='" + channelId.Replace("'", "") + "' and F_Titile is not null and F_DeleteMark=false order by F_Order desc, F_CreatorTime desc,F_IsTop desc");
            if (!string.IsNullOrEmpty(tagId))
                list = list.Where(t => t.F_Tags.Contains(tagId)).ToList();

            list = list.Skip((page - 1) * psize).Take(psize).AsQueryable().ToList();
            return list;

        }

        public async Task<ContentEntity> GetListByChannelId(string channelId)
        {
            var list = await repository.FindList("select * from cms_content where F_ChannelId='" + channelId.Replace("'", "") + "' and F_Titile is not null and F_DeleteMark=false order by F_Order desc, F_CreatorTime desc,F_IsTop desc");
            var data = list.ToList().FirstOrDefault();
            return data;
        }

        public async Task<List<ContentEntity>> GetListByChannelIds(string channelId)
        {
            var list = await repository.FindList("select * from cms_content where F_ChannelId='" + channelId.Replace("'", "") + "' and F_Titile is not null and F_DeleteMark=false order by F_Titile+0 desc, F_SubTitle+0 asc");
            var data = list.ToList();
            return data;
        }
        public async Task<int> GetListCountByChannelId(string channelId)
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(channelId))
            {
                cachedata = cachedata.Where(t => t.F_ChannelId == channelId).ToList();

            }
            var listCount = cachedata.Where(t => t.F_DeleteMark == false).ToList().Count;
            return listCount;
        }
        public async Task<int> GetListCountByLableId(string lableId)
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(lableId))
            {
                cachedata = cachedata.Where(t => !string.IsNullOrEmpty(t.F_Tags)).ToList();
                cachedata = cachedata.Where(t => t.F_Tags.Contains(lableId)).ToList();

            }
            var listCount = cachedata.Where(t => t.F_DeleteMark == false).ToList().Count;
            return listCount;
        }
        public async Task<List<ContentEntity>> GetLookList(SoulPage<ContentEntity> pagination, string itemId = "", string keyword = "", string lan = "0")
        {
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(itemId))
            {
                list = list.Where(u => itemId.Contains(u.F_ChannelId) || (u.F_SubChannelId != "" && itemId.Contains(u.F_SubChannelId)));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Titile.Contains(keyword));
            }

            list = list.Where(u => u.F_DeleteMark == false && u.F_lan.ToString() == lan);
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }

        public async Task<List<ContentEntity>> GetLookList(SoulPage<ContentEntity> pagination, string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_Titile.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark == false && u.F_Titile != null);
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }

        public async Task<ContentEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }

        public async Task<ContentEntity> GetForm(string keyValue, string lan = "0")
        {
            //var cachedata = await repository.CheckCache(cacheKey, keyValue);
            var cachedata = await repository.FindList("select * from cms_content where F_LanInfoId='" + keyValue.Replace("'", "") + "' and F_lan=" + lan + " and F_Titile is not null and F_DeleteMark=false order by F_Order desc");
            return cachedata.FirstOrDefault();
        }

        public async Task<ContentEntity> GetForms(string keyValue, string lan = "0")
        {
            //var cachedata = await repository.CheckCache(cacheKey, keyValue);
            var cachedata = await repository.FindList("select * from cms_content where F_LanInfoId='" + keyValue.Replace("'", "") + "' and F_lan=" + lan + "  order by F_Order desc");
            return cachedata.FirstOrDefault();
        }
        #endregion

        public async Task<ContentEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata, className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(ContentEntity entity, string keyValue)
        {
            var datastr = "";
            if (string.IsNullOrEmpty(keyValue))
            {
                //此处需修改
                entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
                //try
                //{
                //    if (!string.IsNullOrEmpty(entity.F_MeetingStartTime.ToString()))
                //    {
                //        var endDate = entity.F_MeetingEndTime == null ? "" : entity.F_MeetingEndTime.ToString().Replace("-", "/");
                //        var liveBroadcastId = entity.F_LiveBroadcastId == null ? "" : entity.F_LiveBroadcastId.ToString();
                //        LiveInfo info = new LiveInfo();
                //        info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_Titile.ToString() + entity.F_MeetingStartTime.ToString().Replace("-", "/") + endDate + entity.F_ChannelId.ToString() + entity.F_Tags.ToString() + liveBroadcastId + "1" + entity.F_SubChannelId.ToString() + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //        info.Id = entity.F_Id.ToString();
                //        info.Title = entity.F_Titile.ToString();
                //        info.StarDate = entity.F_MeetingStartTime.ToString().Replace("-", "/");
                //        info.EndDate = endDate;
                //        info.ColumnId = entity.F_ChannelId.ToString();
                //        info.TagId = entity.F_Tags.ToString();
                //        info.LiveId = liveBroadcastId;
                //        info.opType = 1;
                //        info.SubColumnId = entity.F_SubChannelId.ToString();
                //        datastr = JsonConvert.SerializeObject(info);
                //        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/LiveInfo/OPLiveInfo", "POST", "", null, datastr);
                //    }
                //    else
                //    {
                //        NewsInfo info = new NewsInfo();
                //        info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_Titile.ToString() + entity.F_ChannelId.ToString() + entity.F_Tags.ToString() + entity.F_CreatorTime.ToString().Replace("-", "/") + "1" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //        info.Id = entity.F_Id.ToString();
                //        info.Title = entity.F_Titile.ToString();
                //        info.ParentId = entity.F_ChannelId.ToString();
                //        info.TagId = entity.F_Tags.ToString();
                //        info.Author = "";
                //        info.PubDate = entity.F_CreatorTime.ToString().Replace("-", "/");
                //        info.opType = 1;
                //        datastr = JsonConvert.SerializeObject(info);
                //        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/NewsInfo/OPNewsInfo", "POST", "", null, datastr);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                //此处需修改
                entity.Modify(keyValue);
                await repository.Update(entity);
                //try
                //{
                //    var opType = entity.F_EnabledMark == false ? 4 : 2;
                //    if (!string.IsNullOrEmpty(entity.F_MeetingStartTime.ToString()))
                //    {
                //        var endDate = entity.F_MeetingEndTime == null ? "" : entity.F_MeetingEndTime.ToString().Replace("-", "/");
                //        var liveBroadcastId = entity.F_LiveBroadcastId == null ? "" : entity.F_LiveBroadcastId.ToString();
                //        LiveInfo info = new LiveInfo();
                //        info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_Titile.ToString() + entity.F_MeetingStartTime.ToString().Replace("-", "/") + endDate + entity.F_ChannelId.ToString() + entity.F_Tags.ToString() + liveBroadcastId  + opType.ToString() + entity.F_SubChannelId.ToString() + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //        info.Id = entity.F_Id.ToString();
                //        info.Title = entity.F_Titile.ToString();
                //        info.StarDate = entity.F_MeetingStartTime.ToString().Replace("-", "/");
                //        info.EndDate = endDate;
                //        info.ColumnId = entity.F_ChannelId.ToString();
                //        info.TagId = entity.F_Tags.ToString();
                //        info.LiveId = liveBroadcastId;
                //        info.opType = opType;
                //        info.SubColumnId = entity.F_SubChannelId.ToString();
                //        datastr = JsonConvert.SerializeObject(info);
                //        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/LiveInfo/OPLiveInfo", "POST", "", null, datastr);
                //    }
                //    else
                //    {
                //        NewsInfo info = new NewsInfo();
                //        info.sign = Md5.md5(entity.F_Id.ToString() + entity.F_Titile.ToString() + entity.F_ChannelId.ToString() + entity.F_Tags.ToString() + entity.F_CreatorTime.ToString().Replace("-", "/") + opType.ToString() + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //        info.Id = entity.F_Id.ToString();
                //        info.Title = entity.F_Titile.ToString();
                //        info.ParentId = entity.F_ChannelId.ToString();
                //        info.TagId = entity.F_Tags.ToString();
                //        info.Author = "";
                //        info.PubDate = entity.F_CreatorTime.ToString().Replace("-", "/");
                //        info.opType = opType;
                //        datastr = JsonConvert.SerializeObject(info);
                //        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/NewsInfo/OPNewsInfo", "POST", "", null, datastr);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
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
                
                cachedata.F_EnabledMark = false;
                cachedata.F_DeleteMark = true;
                cachedata.Modify(item);
                await repository.Update(cachedata);
                await CacheHelper.Remove(cacheKey + item);
            }
            //await repository.Delete(t => ids.Contains(t.F_Id));
            await CacheHelper.Remove(cacheKey + "list");
        }

        public async Task PublishForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            foreach (var item in ids)
            {
                var cachedata = await repository.CheckCache(cacheKey, item);
                //try
                //{
                //    var datastr = "";
                //    if (!string.IsNullOrEmpty(cachedata.F_MeetingStartTime.ToString()))
                //    {
                //        var endDate = cachedata.F_MeetingEndTime == null ? "" : cachedata.F_MeetingEndTime.ToString().Replace("-", "/");
                //        var liveBroadcastId = cachedata.F_LiveBroadcastId == null ? "" : cachedata.F_LiveBroadcastId.ToString();
                //        LiveInfo info = new LiveInfo();
                //        info.sign = Md5.md5(cachedata.F_Id.ToString() + cachedata.F_Titile.ToString() + cachedata.F_MeetingStartTime.ToString().Replace("-", "/") + endDate + cachedata.F_ChannelId.ToString() + cachedata.F_Tags.ToString() + liveBroadcastId  + "2" + cachedata.F_SubChannelId.ToString() + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //        info.Id = cachedata.F_Id.ToString();
                //        info.Title = cachedata.F_Titile.ToString();
                //        info.StarDate = cachedata.F_MeetingStartTime.ToString().Replace("-", "/");
                //        info.EndDate = endDate;
                //        info.ColumnId = cachedata.F_ChannelId.ToString();
                //        info.TagId = cachedata.F_Tags.ToString();
                //        info.LiveId = liveBroadcastId;
                //        info.opType = 2;
                //        info.SubColumnId = cachedata.F_SubChannelId.ToString();
                //        datastr = JsonConvert.SerializeObject(info);
                //        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/LiveInfo/OPLiveInfo", "POST", "", null, datastr);
                //    }
                //    else
                //    {
                //        NewsInfo info = new NewsInfo();
                //        info.sign = Md5.md5(cachedata.F_Id.ToString() + cachedata.F_Titile.ToString() + cachedata.F_ChannelId.ToString() + cachedata.F_Tags.ToString() + cachedata.F_CreatorTime.ToString().Replace("-", "/") + "2" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //        info.Id = cachedata.F_Id.ToString();
                //        info.Title = cachedata.F_Titile.ToString();
                //        info.ParentId = cachedata.F_ChannelId.ToString();
                //        info.TagId = cachedata.F_Tags.ToString();
                //        info.Author = "";
                //        info.PubDate = cachedata.F_CreatorTime.ToString().Replace("-", "/");
                //        info.opType = 2;
                //        datastr = JsonConvert.SerializeObject(info);
                //        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/NewsInfo/OPNewsInfo", "POST", "", null, datastr);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                cachedata.F_EnabledMark = true;
                cachedata.Modify(item);
                await repository.Update(cachedata);
                await CacheHelper.Remove(cacheKey + item);
            }
            //await repository.Delete(t => ids.Contains(t.F_Id));
            await CacheHelper.Remove(cacheKey + "list");
        }

        public async Task RecycleForm(string keyValue)
        {
            var ids = keyValue.Split(',');
            foreach (var item in ids)
            {
                var cachedata = await repository.CheckCache(cacheKey, item);
                //try
                //{
                //    var datastr = "";
                //    if (!string.IsNullOrEmpty(cachedata.F_MeetingStartTime.ToString()))
                //    {
                //        var endDate = cachedata.F_MeetingEndTime == null ? "" : cachedata.F_MeetingEndTime.ToString().Replace("-", "/");
                //        var liveBroadcastId = cachedata.F_LiveBroadcastId == null ? "" : cachedata.F_LiveBroadcastId.ToString();
                //        LiveInfo info = new LiveInfo();
                //        info.sign = Md5.md5(cachedata.F_Id.ToString() + cachedata.F_Titile.ToString() + cachedata.F_MeetingStartTime.ToString().Replace("-", "/") + endDate + cachedata.F_ChannelId.ToString() + cachedata.F_Tags.ToString() + liveBroadcastId + "4" + cachedata.F_SubChannelId.ToString() + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //        info.Id = cachedata.F_Id.ToString();
                //        info.Title = cachedata.F_Titile.ToString();
                //        info.StarDate = cachedata.F_MeetingStartTime.ToString().Replace("-", "/");
                //        info.EndDate = endDate;
                //        info.ColumnId = cachedata.F_ChannelId.ToString();
                //        info.TagId = cachedata.F_Tags.ToString();
                //        info.LiveId = liveBroadcastId;
                //        info.opType = 4;
                //        info.SubColumnId = cachedata.F_SubChannelId.ToString();
                //        datastr = JsonConvert.SerializeObject(info);
                //        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/LiveInfo/OPLiveInfo", "POST", "", null, datastr);
                //    }
                //    else
                //    {
                //        NewsInfo info = new NewsInfo();
                //        info.sign = Md5.md5(cachedata.F_Id.ToString() + cachedata.F_Titile.ToString() + cachedata.F_ChannelId.ToString() + cachedata.F_Tags.ToString() + cachedata.F_CreatorTime.ToString().Replace("-", "/") + "2" + GlobalContext.SystemConfig.secretkey, 32).ToLower();
                //        info.Id = cachedata.F_Id.ToString();
                //        info.Title = cachedata.F_Titile.ToString();
                //        info.ParentId = cachedata.F_ChannelId.ToString();
                //        info.TagId = cachedata.F_Tags.ToString();
                //        info.Author = "";
                //        info.PubDate = cachedata.F_CreatorTime.ToString().Replace("-", "/");
                //        info.opType = 4;
                //        datastr = JsonConvert.SerializeObject(info);
                //        string reult = HttpHelper.Http(GlobalContext.SystemConfig.url + "/NewsInfo/OPNewsInfo", "POST", "", null, datastr);
                //    }
                //}
                //catch (Exception ex)
                //{
                //}
                cachedata.F_EnabledMark = false;
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
