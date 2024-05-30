using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class DeviceLog_Rec
    {
        public string UniqueID { get; set; }
        public Nullable<DateTime> InSessionLog { get; set; }
        public Nullable<DateTime> OutSessionLog { get; set; }
        [Key]
        public int id { get; set; }
        public string SessionLogId { get; set; }
    }
}