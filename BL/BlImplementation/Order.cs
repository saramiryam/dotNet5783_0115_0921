using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class Order: BlApi.IOrder
    {
        private IDal Dal = new Dal.DalList();
    }
}
