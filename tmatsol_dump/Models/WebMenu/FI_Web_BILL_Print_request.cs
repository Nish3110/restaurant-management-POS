using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Web_BILL_Print_request
    {
        [Key]
        public int id { get; set; }
        public int bill_no { get; set; }
        public DateTime Req_DtTime { get; set; }
        public int Req_Status { get; set; }
        //public Nullable<DateTime> Printed_DTtime { get; set; }
        public DateTime Printed_DTtime { get; set; }
    }
}