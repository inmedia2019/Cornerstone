using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.DevelopmentManagement
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-01-20 14:54
    /// 描 述：内容属性实体类
    /// </summary>
    [TableAttribute("cms_content_attribute")]
    public class ContentAttributeEntity : IEntity<ContentAttributeEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        /// <returns></returns>
        public string F_FieldName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        /// <returns></returns>
        public string F_FieldType { get; set; }
        /// <summary>
        /// 栏目ID
        /// </summary>
        /// <returns></returns>
        public string F_ChannelId { get; set; }
        /// <summary>
        /// 备注
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
    }
}
