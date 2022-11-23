using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class ProductItem : IProductItem
    {
        private IDal Dal = new Dal.DalList();
    }
}
