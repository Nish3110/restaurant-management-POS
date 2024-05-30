using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Sale_Invoice_Pend_From_Cart_Start
    {
        [Key]
        public int Id { get; set; }
        public string Session_ID { get; set; }      
        //public int Sr_No { get; set; }
        public string item_id { get; set; }
        public string item_name { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public int Uniq_ItemID { get; set; }
        public string Item_Return_Remark { get; set; }
        public string Name_Display { get; set; }
        public int ComboID { get; set; }
        public string deviceid { get; set; }
        public string Api_Ref_No { get; set; }
    }
}