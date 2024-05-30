using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Card_Master
    {
        [Key]
        public int Card_ID { get; set; }
        public string Card_No { get; set; }
        public int Card_Type_No { get; set; }
        public int Loyalty_Point_Per_Curr { get; set; }
        public int Opening_Points { get; set; }
        public int IN_Points { get; set; }
        public int Out_Points { get; set; }
        public int Balance_Debit_Card_Amt { get; set; }
        public DateTime Reg_date { get; set; }
        public DateTime Exp_date { get; set; }
        public string Member_Code { get; set; }
        public int Status_0_1 { get; set; }
        public DateTime Entry_date { get; set; }
        public int Cust_ID { get; set; }
    }
}