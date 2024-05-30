using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Sale_Invoice_Pend_From_Cart_End_DONE
    {
        [Key]
        public string Session_ID { get; set; }
        public int User_ID { get; set; }
        public string Table_No { get; set; }
        public DateTime Session_date { get; set; }
        public int Old_ref_no { get; set;} 
        public int Order_Type { get; set; }
        public int Mode_Code_Link { get; set; }
        public int Customer_ID { get; set; }
        public string User_Name { get; set; }
        public int PAX { get; set; }
        public string bill_note { get; set; }
        public int Return_Ref_No { get; set; }
        public string deviceid { get; set; }
        public string Api_Ref_No { get; set; }
    }
}