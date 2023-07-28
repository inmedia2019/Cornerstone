using System;
using System.Collections.Generic;
using System.Text;

namespace Cornerstone.Domain.Entity.ContentManage
{
    public class HospitalInfo
    {
        public string sign { get; set; }
        public string Id { get; set; }
        public string HospitalName { get; set; }
        public string Province { get; set; }
        public string HospitalCode { get; set; }
        public int opType { get; set; }
    }
}
