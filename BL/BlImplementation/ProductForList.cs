using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class ProductForList: IProductForList
    {
        private IDal Dal = new Dal.DalList();
    }
}
