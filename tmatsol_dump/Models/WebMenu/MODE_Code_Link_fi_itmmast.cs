using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class MODE_Code_Link_fi_itmmast
    {
        [Key]
        public int id { get; set; }
        public int Mode_Code { get; set; }
        public int Mode_Code_Link { get; set; }
        public int ItemId { get; set; }
        public int Mode_Master_Link_Record_Id { get; set; }
        public decimal Mode_Link_Sale_Price { get; set; }
    }
}