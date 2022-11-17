
using DO;

namespace Dal;

public class DalOrderItem
{/// <summary>
/// add a new orderitem and throw exception if it does not exist
/// </summary>
/// <param name="_newOrderItem">new one to add</param>
/// <returns>new orderIdem id</returns>
/// <exception cref="Exception"></exception>
    public int addOrderItem(OrderItem _newOrderItem)
    {
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (_newOrderItem.ID == DataSource._arrOrderItem[i].ID)
            {
                throw new Exception("orderItem exists");
            }
        }
        _newOrderItem.ID= DataSource.Config.CalNumOfOrderItem;   
        DataSource._arrOrderItem[DataSource.Config._orderItemIndex++] = _newOrderItem;
        return _newOrderItem.ProductID;
    }
    /// <summary>
    /// return specific item by id and throw exception if it does not exist 
    /// </summary>
    /// <param name="orderItemID"></param>
    /// <returns>order item</returns>
    /// <exception cref="Exception"></exception>
    public OrderItem getSingleOrederItem(int orderItemID)
    {
        OrderItem _newOrderItem = new OrderItem();
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].ID==orderItemID)
            {
                _newOrderItem = DataSource._arrOrderItem[i];
                return _newOrderItem;
            }
        }
        throw new Exception("orderItem not exists");
    }
    /// <summary>
    /// return all order items and throw exception if it does not exist
    /// </summary>
    /// <returns>order item arr</returns>
    public OrderItem[] getAllOrderItems()
    {
        OrderItem[] _tempArr = new OrderItem[DataSource.Config._orderItemIndex];
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            _tempArr[i] = DataSource._arrOrderItem[i];
        }
        return _tempArr;
    }
    /// <summary>
    ///  delete order item and throw exception if it does not exist
    /// 
    /// </summary>
    /// <param name="_orderItemID">order item to delete</param>
    /// <exception cref="Exception"></exception>
    public void deleteOrderItem(int _orderItemID)
    {

        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].ID==_orderItemID)
            {
                DataSource._arrOrderItem[i] = DataSource._arrOrderItem[DataSource.Config._orderItemIndex - 1];
                DataSource.Config._orderItemIndex--;
                return;
            }
        }
        throw new Exception("orderItem not exists");
    }
    /// <summary>
    ///  update date of product and throw exception if it does not exist
    /// </summary>
    /// <param name="_newOrderItem">order item to update</param>
    /// <exception cref="Exception"></exception>
    public void updateOrderItem(OrderItem _newOrderItem)
    {
            if ( _newOrderItem.ProductID==0 || _newOrderItem.OrderID==0||_newOrderItem.ID==0||_newOrderItem.Price==0||_newOrderItem.Amount==0)
        {
            return;

        }
            for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].ProductID == _newOrderItem.ProductID && DataSource._arrOrderItem[i].OrderID == _newOrderItem.OrderID)
            {
                DataSource._arrOrderItem[i] = _newOrderItem;
                return;
            }
        }
        throw new Exception("product not exists can not update");
    }
    /// <summary>
    /// get all items of specific order and throw exception if it does not exist
    /// </summary>
    /// <param name="orderNum">specific</param>
    /// <returns> all items</returns>
    /// <exception cref="Exception"></exception>
    public OrderItem[] getAllMyOrdesItem(int orderNum)
    {
        int j = 0;
        OrderItem[] oi = new OrderItem[DataSource.Config._orderItemIndex];
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].OrderID == orderNum)
            {
                oi[j] = DataSource._arrOrderItem[i];
                j++;

            }
        }
        return oi;
        throw new Exception("product not exists can not update");
    }
    /// <summary>
    /// get the order item by productId and orderId
    /// </summary>
    /// <param name="orderId">num order of this order item</param>
    /// <param name="productId">num product of this order item</param>
    /// <returns>order item</returns>
    /// <exception cref="Exception"></exception>
    public OrderItem getSingleOrederItemByProductAndOrder(int orderId, int productId)
    {
        OrderItem _newOrderItem = new OrderItem();
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].OrderID == orderId && DataSource._arrOrderItem[i].ProductID == productId)
            {
                _newOrderItem = DataSource._arrOrderItem[i];
                return _newOrderItem;
            }
        }
        throw new Exception("orderItem not exists");

    }
}


