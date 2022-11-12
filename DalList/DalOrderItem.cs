
using DO;

namespace Dal;

public class DalOrderItem
{

    #region methods

    /// <summary>
    /// add a new item to the list of orderItems
    /// </summary>
    /// <param name="_newOrderItem">an ordertem</param>
    /// <returns>int of the id of the orderItem</returns>
    /// <exception cref="Exception">orderItem exists</exception>
    public int addOrderItem(OrderItem _newOrderItem)
    {
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (_newOrderItem.ID == DataSource._arrOrderItem[i].ID)
            {
                throw new Exception("orderItem exists");
            }
        }
        _newOrderItem.ID = DataSource.Config.CalNumOfOrderItem;
        DataSource._arrOrderItem[DataSource.Config._orderItemIndex++] = _newOrderItem;
        return _newOrderItem.ProductID;
    }

    /// <summary>
    /// check if the orderItem demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="orderItemID">the id of the orderItem demanded</param>
    /// <returns>details of the orderItem demanded</returns>
    /// <exception cref="Exception">orderItem not exists</exception>
    public OrderItem getSingleOrederItem(int orderItemID)
    {
        OrderItem _newOrderItem = new OrderItem();
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].ID == orderItemID)
            {
                _newOrderItem = DataSource._arrOrderItem[i];
                return _newOrderItem;
            }
        }
        throw new Exception("orderItem not exists");
    }

    /// <summary>
    /// cope the orderItems to a new arrey, sort them by thier orderDates from the later to the current and return it
    /// </summary>
    /// <returns>arrey with all the orderItems</returns>
    public OrderItem[] getAllOrderItems()
    {
        //-----------------
        //לשלוח לפןנקציה שממינת לפי תאריכים ואת המערך הזה להחזיר
        //-----------------

        OrderItem[] _tempArr = new OrderItem[DataSource.Config._orderItemIndex];
        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            _tempArr[i] = DataSource._arrOrderItem[i];
        }
        return _tempArr;
    }

    /// <summary>
    /// check if the orderItem demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_orderItemID">id of orderItem to delete</param>
    /// <exception cref="Exception">orderItem not exists</exception>
    public void deleteOrderItem(int _orderItemID)
    {

        for (int i = 0; i < DataSource.Config._orderItemIndex; i++)
        {
            if (DataSource._arrOrderItem[i].ID == _orderItemID)
            {
                DataSource._arrOrderItem[i] = DataSource._arrOrderItem[DataSource.Config._orderItemIndex - 1];
                DataSource.Config._orderItemIndex--;
                return;
            }
        }
        throw new Exception("orderItem not exists");
    }

    /// <summary>
    /// update data of orderItem and throw exception if it does not exist
    /// </summary>
    /// <param name="_newOrderItem">orderItem demanded to change</param>
    /// <exception cref="Exception">product not exists can not update</exception>
    public void updateOrderItem(OrderItem _newOrderItem)
    {
        if (_newOrderItem.ProductID == 0 || _newOrderItem.OrderID == 0 || _newOrderItem.ID == 0 || _newOrderItem.Price == 0 || _newOrderItem.Amount == 0)
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

    //----------------
    // getAllOrderItems? מה ההבדל בין המתודה הזאת לבין המתודה 
    //----------------
    /// <summary>
    /// 
    /// </summary>
    /// <param name="orderNum"></param>
    /// <returns></returns>
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

    #endregion

}


