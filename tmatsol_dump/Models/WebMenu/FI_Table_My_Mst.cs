using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Table_My_Mst
    {
        public string Table_no { get; set; }
        public int Nos_Of_Persons { get; set; }
        public int Fi_Table_Template_Master_sr_no { get; set; }
        public string Image_File_Name { get; set; }
        public int Pos_X { get; set; }
        public int Pos_Y { get; set; }
        public string Table_Occupuied { get; set; }
        public string Bill_Printed { get; set; }
        public int IOT { get; set; }
        public string color { get; set; }     
    }
}