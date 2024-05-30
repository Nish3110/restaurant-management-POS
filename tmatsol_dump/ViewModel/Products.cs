using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace tmatsol_dump.ViewModel
{
    public class Products
    {
        public Products()
        {
            ProductImages = new List<string>();
        }
        public string ITEM_CD { get; set; }
        public string ITEM_DESC { get; set; }
        public string ITEM_DESC_SL { get; set; }
        public Nullable<Double> Rate { get; set; }
        public Double SALE_PRICE { get; set; }
        public string Inventory_Item_Code { get; set; }
        public int stockqty { get; set; }

        public List<string> ProductImages { get; set; }
    }
}
