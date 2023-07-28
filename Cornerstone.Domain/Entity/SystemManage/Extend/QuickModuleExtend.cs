using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Annotations;
using Cornerstone.Domain;

namespace Cornerstone.Domain.SystemManage
{
    /// <summary>
    /// QuickModule Entity Model
    /// </summary>
    public class QuickModuleExtend
	{
        public string id { get; set; }
        public string title { get; set; }
        public string href { get; set; }
        public string icon { get; set; }

    }
}