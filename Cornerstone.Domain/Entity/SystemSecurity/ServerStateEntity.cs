using System;
using Chloe.Annotations;

namespace Cornerstone.Domain.SystemSecurity
{
	/// <summary>
	/// ServerState Entity Model
	/// </summary>
	[TableAttribute("sys_serverstate")]
    public class ServerStateEntity : IEntity<ServerStateEntity>
    {
						[ColumnAttribute("F_Id", IsPrimaryKey = true)]
			public  String  F_Id { get; set; }
					
			public  String  F_WebSite { get; set; }
					
			public  String  F_ARM { get; set; }
					
			public  String  F_CPU { get; set; }
					
			public  String  F_IIS { get; set; }
					
			public  DateTime  F_Date { get; set; }
		public int F_Cout { get; set; }
	}
}