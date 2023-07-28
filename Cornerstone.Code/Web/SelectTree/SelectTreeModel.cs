using System;
using System.Collections.Generic;
using System.Text;

namespace Cornerstone.Code.Web.SelectTree
{
    public class SelectTreeModel
    {
        public string value { get; set; }
        public string name { get; set; }
        public string parentId { get; set; }
        public bool? selected { get; set; }
        public bool? disabled { get; set; }
        public List<SelectTreeModel> children { get; set; }
    }
}
