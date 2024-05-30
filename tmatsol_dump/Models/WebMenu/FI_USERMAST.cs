using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_USERMAST
    { 
        [Key]
        public int User_ID { get; set; }
        public string First_name { get; set; }
        public string Middle_name { get; set; }
        public string Last_name { get; set; }
        public int Job_Tile_ID { get; set; }
        public string Mobile_No { get; set; }
        public string loc_code { get; set; }
        public string Authorize_Code { get; set; }
        public int Preferred_Language_ID { get; set; }
        public string Active_InActive { get; set; }
        public string Cashier_Settlement { get; set; }
        public string Cash_Report_Required { get; set; }
        public string Rate_Display { get; set; }
        public string Return_Amount_Screen { get; set; }
        public string Item_Detail_Screen_Yes_No { get; set; }
        public string Web_Report_Require { get; set; }
    }
}