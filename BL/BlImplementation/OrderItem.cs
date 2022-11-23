using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class OrderItem: IOrderItem
    {
        private IDal Dal = new Dal.DalList();
    }
}
