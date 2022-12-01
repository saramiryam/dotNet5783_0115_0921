using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IProduct
    {
        public IEnumerable<BO.ProductForList?> GetListOfProduct();

        public IEnumerable<BO.ProductForList?> GetProductForListByCategory(string category);

        public BO.Product GetProductItem(int id);

        public BO.ProductItem GetProductItemForCatalog(int id, BO.Cart CostumerCart);


        //להוסיף פונקציה שמקבלת פרמטרים ויוצרת מהם DO<PRODUCT
        public void AddProduct(DO.Product p);

        public void UpdateProduct(BO.Product item);

        public void DeleteProduct(int id);

    }
}
