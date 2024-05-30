using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace tmatsol_dump.Models.Web_Report
{
    public class _DBContext : DbContext
    {
        public _DBContext(): base("Report_DB_Entry")
        {
        }

        public DbSet<Cust_Rec> Cust_Recs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}