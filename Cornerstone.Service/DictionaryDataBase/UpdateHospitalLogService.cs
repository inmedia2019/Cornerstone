using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cornerstone.Code;
using Chloe;
using Cornerstone.Domain.DictionaryDataBase;
using Cornerstone.Domain.Entity.DictionaryDataBase;

namespace Cornerstone.Service.DictionaryDataBase
{

    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-03-18 14:03
    /// 描 述：医院管理服务类
    /// </summary>
    public class UpdateHospitalLogService : DataFilterService<UpdateHospitalLogEntity>, IDenpendency
    {
        private string cacheKey = "Cornerstone_updatehospitallogdata_";
        private string className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.Split('.')[3];
        public UpdateHospitalLogService(IDbContext context) : base(context)
        {
        }

        #region 提交数据
        public async Task SubmitForm(UpdateHospitalLogEntity entity)
        {
            entity.Create();
            await repository.Insert(entity);
            await CacheHelper.Remove(cacheKey + "list");
        }
        #endregion

    }
}
