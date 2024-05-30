using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20
{
    public class FI_Language_Master
    {
        [Key]
        public int Language_ID { get; set; }
        public string Language_Name { get; set; }
        public string Active_Language_ID { get; set; }
        public int Display_Desc { get; set; }
    }
}