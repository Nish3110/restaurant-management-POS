using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class Fi_others
    {
        [Key]
        public string  fp { get; set; }
        public string ref_no { get; set; }
    }
}