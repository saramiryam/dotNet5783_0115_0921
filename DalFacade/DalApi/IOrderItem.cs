using DO;

namespace DalApi
{
    public interface IOrderItem:ICrud<OrderItem>
    {

        public OrderItem[] getAllMyOrdesItem(int orderNum);

        public OrderItem getSingleOrederItemByProductAndOrder(int orderId, int productId);
    }
}
