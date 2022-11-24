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


        //עבור מנהל 
        public BO.ProductItem GetProductItem(int id);

        //קטלוג קונה
        public BO.ProductItem GetProductItemForCatalog(int id,BO.Cart CostumerCart);

        //עבור מנהל
        public int AddProduct(BO.Product item); 
        public int UpdateProduct(BO.Product item);  
        public void DeleteProduct(int id);

    }
}
