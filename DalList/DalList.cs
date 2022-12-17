using DalApi;
using DO;
using System.Security.Principal;

namespace Dal
{
   internal sealed class DalList : IDal
    {
        public static IDal Instance { get; } = new DalList();
        public IProduct Product => new DalProduct();
        public IOrder Order => new DalOrder();
        public IOrderItem OrderItem => new DalOrderItem();

        private void DaiList()
        {

        }

    }

}




