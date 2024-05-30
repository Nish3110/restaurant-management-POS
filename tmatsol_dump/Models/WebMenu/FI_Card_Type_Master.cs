using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Card_Type_Master
    {
        [Key]
        public int Card_ID { get; set; }
        public string Card_Name { get; set; }
        public int Discount_Per { get; set; }
        public int Discount_Amt { get; set; }
        public int Card_Type_No { get; set; }
        public string Status { get; set; }
        public DateTime Create_Date { get; set; }
        public int Create_USER_ID { get; set; }
        public string Create_USER_Name { get; set; }
        public int Earn_Point_Per_Amount { get; set; }
        public int AED_Per_Point { get; set; }
    }
}