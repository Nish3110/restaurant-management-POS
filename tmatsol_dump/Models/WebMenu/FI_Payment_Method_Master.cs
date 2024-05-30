using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Payment_Method_Master
    {
        [Key]
        public int Code { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double Conversion { get; set; }
        public string Name_Of_Country { get; set; }
        public int Order_By { get; set; }
        public string Allowed_Mode_To { get; set; }
        public string Active_InActive { get; set; }
    }
}