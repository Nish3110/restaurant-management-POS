using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class fi_rights_master
    {
        [Key]
        public int Right_ID { get; set; }
        public string Right_name { get; set; }
        public string Menu_Name { get; set; }
        public string Mode { get; set; }
    }
}