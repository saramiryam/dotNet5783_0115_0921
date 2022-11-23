using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class OrderTracking: IOrderTracking
    {
        private IDal Dal = new Dal.DalList();
    }
}
