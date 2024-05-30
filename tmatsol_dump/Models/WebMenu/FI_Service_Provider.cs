using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Service_Provider
    {
        [Key]
        public int Service_Provider_ID { get; set; }
        public string First_name { get; set; }
        public string Middle_name { get; set; }
        public string Last_name { get; set; }
        public string Mobile_No { get; set; }
        public string loc_code { get; set; }
        public string Active_InActive { get; set; }
        public double Comm_Per { get; set; }
        public string Base_Color_ARGB { get; set; }
        public string Fore_Color_ARGB { get; set; }
    }
}