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
        public BO.ProductForList? GetProductForList(int id);

        public IEnumerable<BO.ProductForList?> GetProductForListByCategory(BO.Enums.ECategory category);

        public BO.Product GetProductDetails (int id);
        public BO.ProductItem? GetProductItemDetails(int id);


        public IEnumerable< BO.ProductItem?> GetProductItemList(Func<DO.Product?, bool>? predict = null);
        public IEnumerable<BO.ProductItem?> GetProductItemListGrouping();


        public BO.ProductItem GetProductItemForCatalog(int id, BO.Cart CostumerCart);

        public int AddProduct(BO.Product p);

        public void UpdateProduct(BO.Product item);

        public void DeleteProduct(int id);

    }
}
