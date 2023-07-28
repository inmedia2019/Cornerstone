using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornerstone.Code;
using Cornerstone.Domain.Entity.ContentManage;

namespace Cornerstone.Service.ContentManage
{
    public class ColumnService:DataFilterService<ColumnEntity>,IDenpendency
    {
        public ColumnService(IDbContext context) : base(context)
        {

        }
        private string cacheKey = "Cornerstone_cms_columndata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public async Task<List<ColumnEntity>> GetList(string keyword = "")
        {
            var cachedata = await repository.CheckCacheList(cacheKey + "list");
            if (!string.IsNullOrEmpty(keyword))
            {
                cachedata = cachedata.Where(t => t.F_Name.Contains(keyword)).ToList();
            }
            return cachedata.Where(t => t.F_DeleteMark == false).OrderBy(t => t.F_Order).ToList();
        }

        public async Task<List<ColumnEntity>> GetLookList()
        {
            var list = new List<ColumnEntity>();
            if (!CheckDataPrivilege(className.Substring(0, className.Length - 7)))
            {
                list = await repository.CheckCacheList(cacheKey + "list");
            }
            else
            {
                var forms = GetDataPrivilege("u", className.Substring(0, className.Length - 7));
                list = forms.ToList();
            }
            return GetFieldsFilterData(list.Where(a => a.F_DeleteMark == false).OrderBy(t => t.F_Order).ToList(), className.Substring(0, className.Length - 7));
        }
        public async Task<ColumnEntity> GetForm(string keyValue)
        {
            var query = repository.IQueryable(a => a.F_EnabledMark == true);
            if (!string.IsNullOrEmpty(keyValue))
            {
                query = query.Where(a => a.F_Id == keyValue);
            }
            return GetFieldsFilterData(query.FirstOrDefault(), className.Substring(0, className.Length - 7));
        }
        public async Task SubmitForm(ColumnEntity entity,string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_DeleteMark = false;
                entity.Create();
                await repository.Insert(entity);
                await CacheHelper.Remove(cacheKey + "list");
            }
            else
            {
                entity.Modify(keyValue);
                await repository.Update(entity);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }
        public async Task DeleteForm(string keyValue)
        {
            if (repository.IQueryable(t => t.F_ParentId.Equals(keyValue)).Count() > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                await repository.Delete(a => a.F_Id == keyValue);
                await CacheHelper.Remove(cacheKey + keyValue);
                await CacheHelper.Remove(cacheKey + "list");
            }
        }
    }
}
