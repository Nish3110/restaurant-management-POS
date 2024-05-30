using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Rights_Master_Oth_lan
    {
        [Key, Column("Language_ID", Order = 0)]
        public int Language_ID { get; set; }
        [Key, Column("Right_ID", Order = 1)]
        public int Right_ID { get; set; }
        public string Right_name { get; set; }
        public string Right_name_ENGLISH { get; set; }
    }
}