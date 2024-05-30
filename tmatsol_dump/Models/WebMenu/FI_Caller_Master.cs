using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Caller_Master
    {
        [Key]
        public int CallerIDNumber { get; set; }
        public string CallerID { get; set; }
        public string Type { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Addl1 { get; set; }
        public string Addl2 { get; set; }
        public string Addl3 { get; set; }
        public string Addl4 { get; set; }
        public string Addl5 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
        public string EmailID { get; set; }
        public int Caller_Type { get; set; }
        public Nullable<DateTime> Birth_day { get; set; }
        public int Status { get; set; }
        public string TRN { get; set; }
        public int Zone_CD { get; set; }
        public int Nos_Of_Images { get; set; }
        public Decimal Fix_Per_Discount { get; set; }
    }
}