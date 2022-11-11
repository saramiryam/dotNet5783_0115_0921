
using DO;

namespace Dal;

public class DalOrderItem
{
    public int addOrderItem(OrderItem _newOrderItem)
    {
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (_newOrderItem.ProductID == DataSource._arrOrderItem[i].ProductID && _newOrderItem.OrderID == DataSource._arrOrderItem[i].OrderID)
            {
                throw new Exception("orderItem exists");
            }
        }
        DataSource._arrOrderItem[DataSource.Config._orderItemIndex++] = _newOrderItem;
        return _newOrderItem.ProductID;
    }
    public OrderItem getSingleOrederItem(int _prodactNum, int _orderNum)
    {
        OrderItem _newOrderItem = new OrderItem();
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].ProductID == _prodactNum && DataSource._arrOrderItem[i].OrderID == _orderNum)
            {
                _newOrderItem = DataSource._arrOrderItem[i];
                return _newOrderItem;
            }
        }
        throw new Exception("orderItem not exists");
    }
    public OrderItem[] getAllOrderItems()
    {
        OrderItem[] _tempArr = new OrderItem[DataSource.Config._orderItemIndex];
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            _tempArr[i] = DataSource._arrOrderItem[i];
        }
        return _tempArr;
    }

    public void deleteOrderItem(int _prodactNum, int _orderNum)
    {

        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].ProductID == _prodactNum && DataSource._arrOrderItem[i].OrderID == _orderNum)
            {
                DataSource._arrOrderItem[i] = DataSource._arrOrderItem[DataSource.Config._orderItemIndex - 1];
                DataSource.Config._orderItemIndex--;
                return;
            }
        }
        throw new Exception("orderItem not exists");
    }
    public void updateOrderItem(OrderItem _newOrderItem)
    {

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
}


