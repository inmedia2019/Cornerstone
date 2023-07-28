using System;
using System.Collections.Generic;
using System.Text;

namespace Cornerstone.Domain.Entity.ContentManage
{
    public class SourceInfo
    {
        public string sign { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public int opType { get; set; }
        public string ParentId { get; set; }
    }
}
