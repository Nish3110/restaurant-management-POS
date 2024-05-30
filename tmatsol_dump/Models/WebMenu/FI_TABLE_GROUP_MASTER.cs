using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_TABLE_GROUP_MASTER
    {
        public string LOC_CODE { get; set; }
        [Key]
        public int TABLE_GRP_CD { get; set; }
        public string TABLE_GRP_NAME { get; set; }
        public string Status { get; set; }
    }
}