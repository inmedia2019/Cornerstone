using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.SystemManage
{
	/// <summary>
	/// QuickModule Entity Model
	/// </summary>
	[TableAttribute("sys_quickmodule")]
    public class QuickModuleEntity : IEntity<QuickModuleEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
						[ColumnAttribute("F_Id", IsPrimaryKey = true)]
			public  String  F_Id { get; set; }
					
			public  String  F_ModuleId { get; set; }
					
			public  Boolean?  F_DeleteMark { get; set; }
					
			public  Boolean?  F_EnabledMark { get; set; }
					
			public  String  F_Description { get; set; }
					
			public  DateTime?  F_CreatorTime { get; set; }
					
			public  String  F_CreatorUserId { get; set; }
					
			public  DateTime?  F_LastModifyTime { get; set; }
					
			public  String  F_LastModifyUserId { get; set; }
					
			public  DateTime?  F_DeleteTime { get; set; }
					
			public  String  F_DeleteUserId { get; set; }
		    }
}