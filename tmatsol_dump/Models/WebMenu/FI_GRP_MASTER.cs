using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_GRP_MASTER
    {
        [Key]
        public string GRP_CD { get; set; }
        public string GRP_SNAME { get; set; }
        public string GRP_NAME { get; set; }
        public string GROUP_YN { get; set; }
        public string FOR_GRP_CD { get; set; }
        public string LEVEL_TEXT { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public Nullable<Double> Customer_Profit_Percentage { get; set; }
        public Nullable<Double> Delear_Profit_Percentage { get; set; }
        public string Export_To_Zen_Cart { get; set; }
        public Nullable<DateTime> DateModified { get; set; }
        public int Order_By { get; set; }
        public string Group_Button_Base_Color_ARGB { get; set; }
        public string Group_Button_Fore_Color_ARGB { get; set; }
        public string GRP_SNAME_SL { get; set; }
        public Nullable<Double> Font_Size { get; set; }
        public string Status { get; set; }
    }
}