using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2020-12-23 16:52
    /// 描 述：CMS栏目管理实体类
    /// </summary>
    [TableAttribute("cms_channel")]
    public class ChannelEntity : IEntity<ChannelEntity>,ICreationAudited,IModificationAudited,IDeleteAudited
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
        public string F_ChannelName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ChannelIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ChannelImages { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ChannelHref { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ChannelTemplate { get; set; }
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
        public int? F_Order { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string F_ContentTemplate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool? F_DisDrawing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool? F_LinkType { get; set; }
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
        public string F_FileContent { get; set; }
        public string F_Icon { get; set; }
        public string F_Title { get; set; }

        public string F_FrontLink { get; set; }

        public string F_ScoreType { get; set; }

        public int F_lan { get; set; }

        public string F_LanInfoId { get; set; }

        [NotMapped]
        public string PicUrl { get; set; }

    }
}
