using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_User_Rights_Master
    {
        [Key]
        public string id { get; set; }
        public string Loc_Code { get; set; }
        public int User_ID { get; set; }    
        public int Right_ID { get; set; }
    }
}