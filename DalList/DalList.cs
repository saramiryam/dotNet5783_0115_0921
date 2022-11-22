using DalApi;
using DO;
using System.Security.Principal;

namespace Dal
{
    sealed public class DalList : IDal
    {
        public IProduct Product => new DalProduct();
        public IOrder Order => new DalOrder();
        public IOrderItem OrderItem => new DalOrderItem();

    }

}




