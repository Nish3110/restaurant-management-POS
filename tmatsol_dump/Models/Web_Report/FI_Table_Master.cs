using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace tmatsol_dump.Models.Web_Report
{
    public class FI_Table_Master
    {
        public string loc_code { get; set; }
        public int Table_Grp_cd { get; set; }
        public string Table_no { get; set; }
        public int Nos_Of_Persons { get; set; }
        [Key]
        public int Fi_Table_Template_Master_sr_no { get; set; }
        public string Image_File_Name { get; set; }
        public int IOT { get; set; }
      
    }
}