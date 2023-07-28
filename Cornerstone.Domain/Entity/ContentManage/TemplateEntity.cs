using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-23 15:46
    /// 描 述：CMS模板管理实体类
    /// </summary>
    [TableAttribute("cms_template")]
    public class TemplateEntity : IEntity<TemplateEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
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
        public string F_TemplateName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_TemplateFile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_FileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_TemplateMode { get; set; }
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
        public string F_TemplateContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? F_EditTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long? F_EditId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool? F_IsDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ParentId { get; set; }
        public string F_Order { get; set; }
        public string F_FileContent { get; set; }
    }
}
