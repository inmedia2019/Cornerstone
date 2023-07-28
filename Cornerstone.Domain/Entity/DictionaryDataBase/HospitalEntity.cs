using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.DictionaryDataBase
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-03-18 14:03
    /// 描 述：医院管理实体类
    /// </summary>
    [TableAttribute("dic_hospital")]
    public class HospitalEntity : IEntity<HospitalEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_HospitalName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Province { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Area { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_District { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? F_Number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Introduction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? F_Order { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 逻辑删除标志
        /// </summary>
        /// <returns></returns>
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        /// <returns></returns>
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        /// <returns></returns>
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        /// <returns></returns>
        public string F_DeleteUserId { get; set; }

        public string F_MarkType { get; set; }

        public string F_Code { get; set; }
    }

}
