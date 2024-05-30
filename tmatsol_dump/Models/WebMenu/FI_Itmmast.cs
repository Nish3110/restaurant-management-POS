using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_Itmmast
    {
        [Key]
        public string ITEM_CD { get; set; }
        public string ITEM_DESC { get; set; }
        public string UNIT { get; set; }
        public Nullable<Double> Rate { get; set; }
        public Double SALE_PRICE { get; set; }
        public string USER_NAME { get; set; }
        public string ACTIVE_INACTIVE { get; set; }
        public string INV_NONINV { get; set; }
        public string TYPE_OF_ITEM { get; set; }
        public Nullable<Double> fIX_DISCOUNT_PER { get; set; }
        public Nullable<Double> fIX_DISCOUNT_AMT { get; set; }
        public string BAR_CODE_NON_BAR_CODE { get; set; }
        public string ALLOW_CHANGE_RATE { get; set; }
        public Double KITCHEN_NO { get; set; }
        public string GRP_CD { get; set; }
        public Nullable<Double> VAT_PERCENTAGE { get; set; }
        public Nullable<Double> SER_PERCENTAGE { get; set; }
        public string SER_APPLICABLE { get; set; }
        public Double Dine_IN_Price { get; set; }
        public Double Drive_Through_Price { get; set; }
        public Double Counter_Price { get; set; }
        public Double Delivery_Price { get; set; }
        public Double Corporate_Price { get; set; }
        public int Make_Time { get; set; }
        public string Service_Provider_Require { get; set; }
        public int ItemID { get; set; }
        public Double Staff_Price { get; set; }
        public string Product_Mode { get; set; }
        public string ITEM_DESC_SL { get; set; }
        public string item_Category { get; set; }
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public int P4 { get; set; }
        public int P5 { get; set; }
        public int Order_By { get; set; }
        public string BarCode { get; set; }
        public string Single_Inventory_Item { get; set; }
        public Double item_cost { get; set; }
        public string Item_Button_Base_Color_ARGB { get; set; }
        public string Item_Button_Fore_Color_ARGB { get; set; }
        public string IMEI_Req { get; set; }
        public string Modifier_Exist { get; set; }
        public string Change_Name_Allow { get; set; }
        public int KOT_Display_At_Station_ID { get; set; }
        public string Ingredients { get; set; }
        public int Allow_KOT_Void_Time { get; set; }
        public int Food_Dept_Code { get; set; }
        public Decimal TAX1 { get; set; }
        public Decimal TAX2 { get; set; }
        public string TAX_Price_Mode { get; set; }
        public Decimal MC_PER { get; set; }
        public Decimal SC_PER { get; set; }
        public int Handle_Inventory { get; set; }
        public string Inventory_Item_Code { get; set; }
        public string Item_Button_Font_Name { get; set; }
        public int Item_Button_Font_Size { get; set; }
        public int Item_Button_Font_Bold { get; set; }
        public Decimal Max_Allow_Discount_Per { get; set; }
        public string Image_Path { get; set; }


        //______________Qty
        //public int Qty { get; set; }
        //______________Stock
        public int stockqty { get; set; }
        
    }
}