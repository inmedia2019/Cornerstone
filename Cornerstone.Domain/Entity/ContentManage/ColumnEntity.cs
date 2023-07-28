using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cornerstone.Domain.Entity.ContentManage
{
    /// <summary>
    /// 栏目类
    /// </summary>
    [TableAttribute("cms_column")]
    public class ColumnEntity : IEntity<ColumnEntity>, ICreationAudited, IModificationAudited, IDeleteAudited
    {
        /// <summary>
        /// 栏目主键
        /// </summary>
       [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string F_Name { get; set; }
       /// <summary>
       /// 栏目排序
       /// </summary>
        public string F_Order { get; set; }
        /// <summary>
        /// 子栏目ID
        /// </summary>
        public string F_ParentId { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string F_TemplateID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string F_Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? F_EnabledMark { get; set; }
        /// <summary>
        /// 逻辑删除标志
        /// </summary>
        public bool? F_DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? F_CreatorTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? F_LastModifyTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string F_LastModifyUserId { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? F_DeleteTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string F_DeleteUserId { get; set; }
        public string F_Type { get; set; }
        public string F_URL { get; set; }
        public int? F_Number { get; set; }
        public bool? F_IsBrowse { get; set; }
        public bool? F_Show { get; set; }
        public string F_PictureUrl { get; set; }
        public string F_SeoTitle { get; set; }
        public string F_SeoKeywords { get; set; }
        public string F_SeoDescription { get; set; }
        public string F_URLName { get; set; }
        public string F_LinkUrl { get; set; }

    }
}
