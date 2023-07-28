using Chloe.Annotations;
using System;

namespace Cornerstone.Domain.SystemOrganize
{
    [TableAttribute("sys_roleauthorize")]

    public class RoleAuthorizeEntity : IEntity<RoleAuthorizeEntity>, ICreationAudited
    {
        [ColumnAttribute("F_Id", IsPrimaryKey = true)]
        public string F_Id { get; set; }
        public int? F_ItemType { get; set; }
        public string F_ItemId { get; set; }
        public int? F_ObjectType { get; set; }
        public string F_ObjectId { get; set; }
        public int? F_SortCode { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
    }
}
