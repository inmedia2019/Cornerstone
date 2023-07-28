using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.DictionaryDataBase
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-04-28 16:55
    /// 描 述：Badgeinfo实体类
    /// </summary>
    [TableAttribute("dic_badgeinfo")]
    public class BadgeinfoEntity : IEntity<BadgeinfoEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_DeleteUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool? F_EnabledMark { get; set; }
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
        public string F_Introduction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? F_MaxScore { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? F_MinScore { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? F_Order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_picUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_badgetTypeId { get; set; }
    }
}
