using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class Cart
    {
        //private List<CartLine> lineCollection = new List<CartLine>();
        public List<CartLine> lineCollection = new List<CartLine>();
        //public static int Srno;
        public void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                    .Where(p => p.Product.Sr == product.Sr)
                    .FirstOrDefault();
            //CartLine line = lineCollection
            //        .Where(p => p.Product.ProductID == product.ProductID)
            //        .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine { Product = product , Qnautity = quantity });
            }
            else
            {
                line.Qnautity += quantity;
                if (line.Qnautity == 0)
                {
                    line.Qnautity += 1;
                }
            }
        }
        public void AddItemNoteOLD(Product product, int quantity, string index)
        {
            List<CartLine> line = lineCollection;

            if (product.Sr > line.Count)
            {
                lineCollection.Add(new CartLine { Product = product, Qnautity = quantity });
                return;
            }
            else
            {
                lineCollection = new List<CartLine>();
                foreach (var item in line)
                {
                    if (product.Sr.ToString() == item.Product.Sr.ToString())
                    {
                        lineCollection.Add(new CartLine { Product = product, Qnautity = quantity });
                        item.Product.Sr = item.Product.Sr + 1;
                        lineCollection.Add(new CartLine { Product = item.Product, Qnautity = item.Qnautity });
                    }
                    else
                    {
                        if (index == item.Product.Sr.ToString())
                        {
                            lineCollection.Add(new CartLine { Product = item.Product, Qnautity = item.Qnautity });
                        }
                        else
                        {
                            item.Product.Sr = item.Product.Sr + 1;
                            lineCollection.Add(new CartLine { Product = item.Product, Qnautity = item.Qnautity });
                        }
                    }
                }
            }

        }
        public void AddItemNote(Product product, int quantity,string index)
        {            
            List <CartLine> line= lineCollection;
           
            if (product.Sr>line.Count)
            {
                lineCollection.Add(new CartLine { Product = product, Qnautity = quantity });
                return;
            }
            else
            {
                lineCollection = new List<CartLine>();
                foreach (var item in line)
                {
                    if (product.Sr.ToString() == item.Product.Sr.ToString())
                    {
                        lineCollection.Add(new CartLine { Product = product, Qnautity = quantity });
                        item.Product.Sr = item.Product.Sr + 1;
                        lineCollection.Add(new CartLine { Product = item.Product, Qnautity = item.Qnautity });
                    }
                    else
                    {
                        if (index == item.Product.Sr.ToString())
                        {
                            lineCollection.Add(new CartLine { Product = item.Product, Qnautity = item.Qnautity });
                        }
                        else
                        {
                            if (item.Product.Sr>1)
                            {
                                item.Product.Sr = item.Product.Sr + 1;
                            }
                            lineCollection.Add(new CartLine { Product = item.Product, Qnautity = item.Qnautity });
                        }
                    }
                }
            }
            
        }
        public void ReturnItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                    .Where(p => p.Product.Sr == product.Sr)
                    .FirstOrDefault();
            //CartLine line = lineCollection
            //        .Where(p => p.Product.ProductID == product.ProductID)
            //        .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine { Product = product, Qnautity = quantity });
            }
            else
            {
                line.Qnautity += quantity;
                //if (line.Qnautity == 0)
                //{
                //    line.Qnautity += 1;
                //}
            }
        }
        //public void AddOpenItem(Product product, int quantity)
        //{

        //    CartLine line = lineCollection
        //            .Where(p => p.Product.Item_Description  == product.Item_Description)
        //            .FirstOrDefault();

        //    if (line == null)
        //    {
        //        lineCollection.Add(new CartLine { Product = product, Qnautity = quantity });
        //    }
        //    else
        //    {
        //        line.Qnautity += quantity;
        //        if (line.Qnautity == 0)
        //        {
        //            line.Qnautity += 1;
        //        }
        //    }
        //}
        //public void RemoveLine(string ID)
        public void RemoveLine(int ID)
        {
           // var del = lineCollection.Where(l => l.Product.ProductID == ID).First();
            var del = lineCollection.Where(l => l.Product.Sr == ID).First();

            // lineCollection.RemoveAll(l => l.Product.ProductID == ID);
            lineCollection.Remove(del);
            refill();
        }
        public void refill()
        {
            List<CartLine> line = lineCollection;

            lineCollection = new List<CartLine>();
            int index = 0;
            foreach (var item in line)
            {
                index = index + 1;
                item.Product.Sr = index;
                lineCollection.Add(new CartLine { Product = item.Product, Qnautity = item.Qnautity });
            }
        }
        public void RemoveLineForCombo(string ID)
        {
            // var del = lineCollection.Where(l => l.Product.ProductID == ID).First();
            lineCollection.RemoveAll(l => l.Product.ComboID == ID);
            refill();
        }      
        //public void RemoveLine(Product product)
        //{
        //    lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        //}

        //public decimal ComputeTotalValue()
        //{
        //    return lineCollection.Sum(e => e.Product.Price * e.Qnautity);

        //}

        public void Clear()
        {
            lineCollection.Clear();
        }
   
        public IEnumerable<CartLine> lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Qnautity { get; set; }
    }

}
