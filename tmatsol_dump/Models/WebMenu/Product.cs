using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class Product
    {
        public int Sr { get; set; }
        public string ComboID { get; set; }
        public string ProductID { get; set; }
        public string Item_Description { get; set; }
        public string Item_Description_SL { get; set; }
        //public decimal Qnautity { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; } 
        public string Item_Return_Remark { get; set; }

    }
}