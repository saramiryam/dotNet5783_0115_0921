using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class Product: BlApi.IProduct
    {
        private IDal Dal = new Dal.DalList();
    }
}
