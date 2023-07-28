using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.DictionaryDataBase
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-05-04 17:14
    /// 描 述：信息列表实体类
    /// </summary>
    [TableAttribute("web_memberinfo")]
    public class MemberinfoEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? createDate { get; set; }
        /// <summary>
        /// 用户描述
        /// </summary>
        /// <returns></returns>
        public string descript { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ColumnAttribute("Id", IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 0:未同意 1：已同意
        /// </summary>
        /// <returns></returns>
        public bool? IsAgreeActionCollect { get; set; }
        /// <summary>
        /// 0:未同意 1：已同意
        /// </summary>
        /// <returns></returns>
        public bool? IsAgreeAgreement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string JobTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string mediaName { get; set; }
        /// <summary>
        /// 扩展字段1
        /// </summary>
        /// <returns></returns>
        public string moreCol { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        /// <returns></returns>
        public string moreCol1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string openId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string phone { get; set; }
        /// <summary>
        /// 1:男 2：女
        /// </summary>
        /// <returns></returns>
        public int? Sex { get; set; }
        /// <summary>
        /// 0:待审核 1：已审核 3:已禁用
        /// </summary>
        /// <returns></returns>
        public int? state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string trueName { get; set; }
        /// <summary>
        /// 用户积分
        /// </summary>
        /// <returns></returns>
        public int? userScore { get; set; }

        [NotMapped]
        /// <summary>
        /// 用户是否签到
        /// </summary>
        /// <returns></returns>
        public int? IsSgin { get; set; }
    }



}
