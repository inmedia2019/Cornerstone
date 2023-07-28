using System;
using Chloe.Annotations;
using System.Text;

namespace Cornerstone.Domain.Entity.DictionaryDataBase
{
    [TableAttribute("sys_updatehospitallog")]
    public class UpdateHospitalLogEntity:IEntity<UpdateHospitalLogEntity>,ICreationAudited
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        public string F_BeforeChange { get; set; }
        public string F_AfterChange { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
    }
}