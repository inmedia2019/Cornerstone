using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.SystemManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2021-07-09 15:08
    /// 描 述：来源类型实体类
    /// </summary>
    [TableAttribute("cms_source")]
    public class SourceEntity : IEntity<SourceEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public string F_Name { get; set; }
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
        /// 父级Id
        /// </summary>
        /// <returns></returns>
        public string F_ParentId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        /// <returns></returns>
        public string F_Type { get; set; }
        /// <summary>
        /// 前缀链接
        /// </summary>
        /// <returns></returns>
        public string F_Link { get; set; }
        public string F_FrontLink { get; set; }
    }
}
