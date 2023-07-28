using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.InfoManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-05-11 15:29
    /// 描 述：用户留言实体类
    /// </summary>
    [TableAttribute("web_livemessage")]
    public class LivemessageEntity : IEntity<LivemessageEntity>
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
        public string F_Mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_TrueName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_MediaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_JobTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? F_Sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool? F_IsAgreeAgreement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool? F_IsAgreeActionCollect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Message { get; set; }
        public DateTime F_createDate { get; set; }
        public string F_questionType { get; set; }
        public string F_City { get; set; }
        public string F_Email { get; set; }
    }
}
