using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Staff_Master
    {
        public string Loc_code { get; set; }
        [Key]
        public int Code { get; set; }
        public string Mobile_No { get; set; }
        public string Active_InActive { get; set; }
        public string Auth_Code { get; set; }
        public string Staff_Name { get; set; }
    }
}