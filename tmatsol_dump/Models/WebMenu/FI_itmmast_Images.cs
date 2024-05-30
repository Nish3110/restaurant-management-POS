using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace tmatsol_dump.Models.WebMenu
{
    public class FI_itmmast_Images
    {
        [Key]
        public int itemID { get; set; }
        public int Rec_No { get; set; }
        public string Image_Caption { get; set; }
        public byte[] Image_Data { get; set; }
        public int Nos_Of_Images { get; set; }
    }
}