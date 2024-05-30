using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class Bill_Header_Footer
    {
        //public int id { get; set; }
        public int Decimal_Point { get; set; }
        public string Company_Names { get; set; }
        public string Headline1 { get; set; }
        public string Headline2 { get; set; }
        public string Headline3 { get; set; }
        public string Headline4 { get; set; }
        public string Bill_No { get; set; }
        public string BillDate { get; set; }
        public string Mode { get; set; }
        //public float GrossAmt { get; set; }
        //public float TAXAmt { get; set; }
        //public float NetAmt { get; set; }
        //public float TAX_per { get; set; }
        //public float Taxable_Amt { get; set; }
        //public float TAX_amt { get; set; }
        //public float Total { get; set; }
        public string GrossAmt { get; set; }
        public string TAXAmt { get; set; }
        public string NetAmt { get; set; }
        public string TAX_per { get; set; }
        public string Taxable_Amt { get; set; }
        public string TAX_amt { get; set; }
        public string Total { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public string Remark4 { get; set; }
        public string Remark5 { get; set; }
        public string Remark6 { get; set; }
        //---------------------------------
        public string ITEM { get; set; }
        public string QTY { get; set; }
        public string RATE { get; set; }
        public string VALUE { get; set; }
    }
}