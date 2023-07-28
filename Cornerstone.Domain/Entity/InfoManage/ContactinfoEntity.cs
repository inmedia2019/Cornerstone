using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.InfoManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2023-02-23 16:15
    /// 描 述：Contactinfo实体类
    /// </summary>
    [TableAttribute("cms_contactinfo")]
    public class ContactinfoEntity : IEntity<ContactinfoEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
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
        public string F_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_Remark { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ImageUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ChannelId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_FrontLink { get; set; }

        public string F_Tel { get; set; }
        public string F_Email { get; set; }
        public string F_Fax { get; set; }

    
        public int F_lan { get; set; }

        public string F_LanInfoId { get; set; }
    }
}
