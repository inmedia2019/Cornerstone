using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.InfoManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2022-05-11 17:39
    /// 描 述：用户书单实体类
    /// </summary>
    [TableAttribute("web_favitorinfo")]
    public class FavitorinfoEntity : IEntity<FavitorinfoEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? createDate { get; set; }
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
        public string infoBanner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? infoCreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string infoId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string infoParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string infoTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string mid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string morecol { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string morecol1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? tid { get; set; }

        [NotMapped]
        public string PicUrl { get; set; }
    }
}
