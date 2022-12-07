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

        public void AddProduct(BO.Product p);
        public void AddProductFromWindow(int ID,string name,string category,double price, int inStock);

        public void UpdateProduct(BO.Product item);

        public void DeleteProduct(int id);

    }
}
