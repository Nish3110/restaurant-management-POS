using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class Device_Reg_Mst
    {
        [Key]
        public string UniqueID { get; set; }
        public string IsMobile { get; set; }
        public string OS { get; set; }
        public string OS_Version { get; set; }
        public string Browser { get; set; }
        public string LicenseKey { get; set; }
        public DateTime Start_date { get; set; }
        public string Bill_Printer_Name { get; set; }
        public string Ackw_Required { get; set; }

    }
}