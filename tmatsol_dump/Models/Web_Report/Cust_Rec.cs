using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace tmatsol_dump.Models.Web_Report
{
    public class Cust_Rec
    {
        [Key]
        [Column(Order = 1)]
        public string Cust_ID { get; set; }// [nvarchar] (50) NOT NULL,
        public DateTime From_dt { get; set; }//] [datetime]	NOT NULL,
        public DateTime To_dt { get; set; }//] [datetime]		NOT NULL,
        [Key]
        [Column(Order = 2)]
        public string DBServer { get; set; }//] [nvarchar] (50) NOT NULL,
        [Key]
        [Column(Order = 3)]
        public string DBName{ get; set; }//] [nvarchar] (50) NOT NULL,
        public string DBuser{ get; set; }//] [nvarchar] (50) NOT NULL,
        public string DBtype { get; set; }//] [nvarchar] (50) NOT NULL, 
        public string Comp_Name { get; set; }//] [nchar] (100) NOT NULL,  
        public string Login_ID { get; set; }//] [varchar] (50) NULL,
        public string Login_PWD { get; set; }//] [varchar] (100) NULL,
        public string Pwd_Policy { get; set; }//] [varchar] (50) NULL,
        public string PIP { get; set; }//] [varchar] (50) NULL,
        public string PIP_Updated { get; set; }//] [varchar] (50) NULL,

    }
}