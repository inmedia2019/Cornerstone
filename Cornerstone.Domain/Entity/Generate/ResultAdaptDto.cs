using System;
using System.Collections.Generic;
using System.Text;

namespace Cornerstone.Domain.Entity.Generate
{
    public class ResultAdaptDto
    {
        public ResultAdaptDto()
        {
            status = true;
            statusCode = 0;
            this.data = new Dictionary<string, object>();
        }

        public bool status { get; set; }
        public int statusCode { get; set; }
        public string msg { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}
