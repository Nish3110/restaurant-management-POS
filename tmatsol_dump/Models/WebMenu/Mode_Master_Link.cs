using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class Mode_Master_Link
    {
        [Key]
        public int Mode_Code_Master { get; set; }
        public int Mode_Code_Link { get; set; }
        public string Mode_Name_Link { get; set; }
        public string Mode_Name_Link_Oth { get; set; }
        public int Record_ID { get; set; }
        public string Status { get; set; }
        public int Order_by { get; set; }
    }
}