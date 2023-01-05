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

        public IEnumerable<BO.ProductForList?> GetProductForListByCategory(BO.Enums.ECategory category);

        public BO.Product GetProductDetails (int id);

        public IEnumerable< BO.ProductItem?> GetProductItemList();

        public BO.ProductItem GetProductItemForCatalog(int id, BO.Cart CostumerCart);

        public void AddProduct(BO.Product p);
        public void AddProductFromWindow(int ID,string name,string category,double price, int inStock, string action);

        public void UpdateProduct(BO.Product item);

        public void DeleteProduct(int id);

    }
}
