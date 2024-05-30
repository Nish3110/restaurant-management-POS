using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class MODE_MASTER
    {
        [Key]
        public int MODE_CODE { get; set; }
        public string MODE_NAME { get; set; }
        public string Status { get; set; }
        public int Order_By { get; set; }
        public string Remark { get; set; }
    }
}