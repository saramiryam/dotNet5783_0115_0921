using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IOrder
{
    public IEnumerable<BO.OrderForList?> GetListOfOrders();

    public BO.Order GetOrderDetails(int OrderId);
    
    public BO.OrderItem GetOrderItemDetails(int OrderId);

    public BO.Order UpdateShipDate(int orderId);

    public BO.Order UpdateDeliveryDate(int orderId);

    public BO.OrderTracking GetOrderTracking(int orderId);
}
