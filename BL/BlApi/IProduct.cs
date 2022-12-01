using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IProduct
    {
        //  (מנהל) ועבור קטלוג ראשי
        public IEnumerable<BO.ProductForList> GetListOfProduct();
        public IEnumerable<BO.ProductForList> GetProductForListByCategory(string category);



        //עבור מנהל 
        public BO.Product GetProductItem(int id);


        //קטלוג קונה
        public BO.ProductItem GetProductItemForCatalog(int id, BO.Cart CostumerCart);

        //עבור מנהל
        //להוסיף פונקציה שמקבלת פרמטרים ויוצרת מהם DO<PRODUCT
        public void AddProduct(DO.Product p);
        public void UpdateProduct(BO.Product item);
        public void DeleteProduct(int id);

    }
}
