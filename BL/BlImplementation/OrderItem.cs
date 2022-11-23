using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class OrderItem: BlApi.IOrderItem
    {
        private IDal Dal = new Dal.DalList();
    }
}
