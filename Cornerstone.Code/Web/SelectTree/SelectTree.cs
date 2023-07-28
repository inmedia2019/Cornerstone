using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cornerstone.Code.Web.SelectTree
{
    public static class SelectTree
    {
        public static string TreeGridJson(this List<SelectTreeModel> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TreeGridJson(data, "0"));
            return sb.ToString();
        }
        private static string TreeGridJson(List<SelectTreeModel> data, string parentId)
        {
            return GetMenuTree(data, parentId).ToJson().ToString();

        }
        public static List<SelectTreeModel> GetMenuTree(List<SelectTreeModel> list, string parentId)
        {
            List<SelectTreeModel> tree = new List<SelectTreeModel>();
            var children = list.Where(m => m.parentId == parentId).ToList();
            if (children.Count > 0)
            {
               foreach(SelectTreeModel c in children) { 
                    SelectTreeModel tr = new SelectTreeModel();
                    tr.name = c.name;
                    tr.value = c.value;
                    tr.children = GetMenuTree(list, c.value).Count==0?null: GetMenuTree(list, c.value);
                    tree.Add(tr);
                }
            }
            return tree;
        }
            public static string TreeList(this List<SelectTreeModel> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (SelectTreeModel entity in data)
            {
                sb.Append(entity.ToJson());
            }
            sb.Append("]");
            return sb.ToString().Replace("}{", "},{");
        }

    }
}
