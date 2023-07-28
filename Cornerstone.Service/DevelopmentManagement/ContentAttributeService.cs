using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.DevelopmentManagement;
using Cornerstone.Domain.SystemManage;
using Cornerstone.Domain.ContentManage;

namespace Cornerstone.Service.DevelopmentManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-01-20 14:54
    /// 描 述：内容属性服务类
    /// </summary>
    public class ContentAttributeService : DataFilterService<ContentAttributeEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_contentattributedata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public ContentAttributeService(IDbContext context) : base(context)
        {
        }
        #region 获取数据
        public async Task<List<ContentAttributeEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                cachedata = cachedata.Where(t => t.F_FieldName.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderByDescending(t => t.F_CreatorTime).ToList();
        }

        public async Task<List<ContentAttributeEntity>> GetLookList(Pagination pagination, string channelId, string keyword = "")
        {
            //获取新闻列表
            var query = repository.IQueryable(a => a.F_EnabledMark == true)
            .InnerJoin<ItemsDetailEntity>((a, b) => a.F_FieldType == b.F_Id && b.F_EnabledMark == true)
            .InnerJoin<ModuleFieldsEntity>((a,b,c) => a.F_FieldName == c.F_Id && c.F_EnabledMark == true)
            .Select((a, b,c) => new ContentAttributeEntity
            {
                F_Id = a.F_Id,
                F_FieldName = c.F_EnCode,
                F_FieldType = b.F_ItemName,
                F_Remark =a.F_Remark,
                F_ChannelId = a.F_ChannelId,
                F_EnabledMark = a.F_EnabledMark,
                F_CreatorTime = a.F_CreatorTime,
                F_CreatorUserId = a.F_CreatorUserId,
                F_DeleteMark = a.F_DeleteMark,
                F_DeleteTime = a.F_DeleteTime,
                F_DeleteUserId = a.F_DeleteUserId,
                F_LastModifyTime = a.F_LastModifyTime,
                F_LastModifyUserId = a.F_LastModifyUserId
            });
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(u => u.F_FieldName.Contains(keyword));
            }
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7), query);
            list = list.Where(u => u.F_DeleteMark == false && u.F_ChannelId == channelId);
            return GetFieldsFilterData(await repository.OrderList(list, pagination), className.Substring(0, className.Length - 7));
        }

        public async Task<List<ContentAttributeEntity>> GetLookList(SoulPage<ContentAttributeEntity> pagination,string keyword = "")
        {
            //获取数据权限
            var list = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
            if (!string.IsNullOrEmpty(keyword))
            {
                //此处需修改
                list = list.Where(u => u.F_FieldName.Contains(keyword));
            }
            list = list.Where(u => u.F_DeleteMark==false);
            return GetFieldsFilterData(await repository.OrderList(list, pagination),className.Substring(0, className.Length - 7));
        }

        public async Task<ContentAttributeEntity> GetForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return cachedata;
        }
        #endregion

        public async Task<ContentAttributeEntity> GetLookForm(string keyValue)
        {
            var cachedata = await repository.CheckCache(cacheKey, keyValue);
            return GetFieldsFilterData(cachedata,className.Substring(0, className.Length - 7));
        }

        #region 提交数据
        public async Task SubmitForm(ContentAttributeEntity entity, string keyValue)
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
            //await repository.Delete(t => ids.Contains(t.F_Id));
            foreach (var item in ids)
            {
                var cachedata = await repository.CheckCache(cacheKey, item);
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
