using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class Order: IOrder
    {
        private IDal Dal = new Dal.DalList();
    }
}
