using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.ContentManage
{
    /// <summary>
    /// 创 建：超级管理员
    /// 日 期：2023-02-21 13:39
    /// 描 述：Questiontypeinfo实体类
    /// </summary>
    [TableAttribute("web_questiontypeinfo")]
    public class QuestiontypeinfoEntity : IEntity<QuestiontypeinfoEntity>
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
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string descript { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? createDate { get; set; }
        public int sn { get; set; }
    }
}
