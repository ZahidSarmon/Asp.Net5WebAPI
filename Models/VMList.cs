using System;
using System.Collections.Generic;
namespace api.Models
{
    public class VMList
    {
        public List<string> DataList { get; set; }
        public int PageSize { get; set; }
    }
}