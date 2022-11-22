﻿
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
        if (DataSource._arrOrderItem.Exists(e => e.OrderID == _newOrderItem.ID && e.ProductID == _newOrderItem.ProductID))
            throw new Exception("orderItem not exists");
        else
        {
            _newOrderItem.ID = DataSource.Config.CalNumOfOrderItem;
            DataSource._arrOrderItem.Add(_newOrderItem);
            return _newOrderItem.ProductID;
        }
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
        _newOrderItem = DataSource._arrOrderItem.Find(e => e.ID == orderItemID);
        if (_newOrderItem.ID == 0)
            throw new Exception("orderItem not exists");
        else
            return _newOrderItem;
    }
    /// <summary>
    /// return all order items and throw exception if it does not exist
    /// </summary>
    /// <returns>order item arr</returns>
    public List<OrderItem> getAllOrderItems()
    {
        
            return DataSource._arrOrderItem;
    }
    /// <summary>
    ///  delete order item and throw exception if it does not exist
    /// 
    /// </summary>
    /// <param name="_orderItemID">order item to delete</param>
    /// <exception cref="Exception"></exception>
    public void deleteOrderItem(int _orderItemID)
    {
        OrderItem _orderItemToDel=DataSource._arrOrderItem.Find(e => e.ID == _orderItemID);
        if(_orderItemToDel.ID != 0)
            DataSource._arrOrderItem.Remove(_orderItemToDel);
        else
          throw new Exception("orderItem not exists");
    }
    /// <summary>
    ///  update date of product and throw exception if it does not exist
    /// </summary>
    /// <param name="_newOrderItem">order item to update</param>
    /// <exception cref="Exception"></exception>
    public void updateOrderItem(OrderItem _newOrderItem)
    {
        if (_newOrderItem.ProductID == 0 || _newOrderItem.OrderID == 0 || _newOrderItem.ID == 0 || _newOrderItem.Price == 0 || _newOrderItem.Amount == 0)
        {
            return;

        }
        OrderItem _orderItemToUpdate = DataSource._arrOrderItem.Find(e => e.ID == _newOrderItem.ID && e.OrderID == _newOrderItem.OrderID && e.ProductID == _newOrderItem.ProductID);
        if (_orderItemToUpdate.ID != 0)
        {
            DataSource._arrOrderItem.Remove(_orderItemToUpdate);
            DataSource._arrOrderItem.Add(_newOrderItem);
        }
        else
            throw new Exception("product not exists can not update");
    }
    /// <summary>
    /// get all items of specific order and throw exception if it does not exist
    /// </summary>
    /// <param name="orderNum">specific</param>
    /// <returns> all items</returns>
    /// <exception cref="Exception"></exception>
    public List<OrderItem> getAllMyOrdesItem(int orderNum)
    {
        if (DataSource._arrOrderItem.FindAll(e => e.OrderID == orderNum).Count > 0)
            return DataSource._arrOrderItem.FindAll(e => e.OrderID == orderNum);
        else
        throw new Exception("order not exists");
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
        _newOrderItem = DataSource._arrOrderItem.Find(e => e.OrderID == orderId&&e.ProductID==productId);
        if (_newOrderItem.ID == 0)
            throw new Exception("orderItem not exists");
        else
            return _newOrderItem;
    }
}


