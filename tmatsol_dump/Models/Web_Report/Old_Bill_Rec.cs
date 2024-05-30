using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tmatsol_dump.Models.Web_Report
{
    public class Old_Bill_Rec
    {
        public int ref_no { get; set; }
        public int OrderType { get; set; }
        public string ModeTypeStr { get; set; }
        public int Print_Seq_Number_Bill { get; set; }
        public string Bill_Note { get; set; }
    }
}