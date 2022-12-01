
using DO;

namespace Dal;
using DalApi;
using System;
using System.Collections;

internal class DalOrderItem:IOrderItem
{/// <summary>
/// add a new orderitem and throw exception if it does not exist
/// </summary>
/// <param name="_newOrderItem">new one to add</param>
/// <returns>new orderIdem id</returns>
/// <exception cref="Exception"></exception>
    public int Add(OrderItem _newOrderItem)
    {
        if (DataSource._arrOrderItem.Exists(e => e.OrderID == _newOrderItem.ID && e.ProductID == _newOrderItem.ProductID))
            throw new ItemAlreadyExistsException("order exists, can not add") { ItemAlreadyExists = _newOrderItem.ToString() };

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
    public OrderItem Get(int orderItemID)
    {
        OrderItem _newOrderItem = new OrderItem();
        _newOrderItem = DataSource._arrOrderItem.Find(e => e.ID == orderItemID);
        if (_newOrderItem.ID == 0)
            throw new RequestedItemNotFoundException("orderItem not exists,can not do get") { RequestedItemNotFound = orderItemID.ToString() };

        else
            return _newOrderItem;
    }
    /// <summary>
    /// return all order items and throw exception if it does not exist
    /// </summary>
    /// <returns>order item arr</returns>
    public IEnumerable<OrderItem> GetAll()
    {
        
            return DataSource._arrOrderItem;
    }
    /// <summary>
    ///  delete order item and throw exception if it does not exist
    /// 
    /// </summary>
    /// <param name="_orderItemID">order item to delete</param>
    /// <exception cref="Exception"></exception>
    public void Delete(int _orderItemID)
    {
        OrderItem _orderItemToDel=DataSource._arrOrderItem.Find(e => e.ID == _orderItemID);
        if(_orderItemToDel.ID != 0)
            DataSource._arrOrderItem.Remove(_orderItemToDel);
        else
            throw new RequestedItemNotFoundException("orderItem not exists,can not delete") { RequestedItemNotFound = _orderItemID.ToString() };

    }
    /// <summary>
    ///  update date of product and throw exception if it does not exist
    /// </summary>
    /// <param name="_newOrderItem">order item to update</param>
    /// <exception cref="Exception"></exception>
    public void Update(OrderItem _newOrderItem)
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
            throw new RequestedItemNotFoundException("orderItem not exists,can not update") { RequestedItemNotFound = _newOrderItem.ToString() };

    }
    /// <summary>
    /// get all items of specific order and throw exception if it does not exist
    /// </summary>
    /// <param name="orderNum">specific</param>
    /// <returns> all items</returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<OrderItem> getAllMyOrdesItem(int orderNum)
    {
        if (DataSource._arrOrderItem.FindAll(e => e.OrderID == orderNum).Count > 0)
            return DataSource._arrOrderItem.FindAll(e => e.OrderID == orderNum);
        else
            throw new RequestedItemNotFoundException("order not exists,can not get all orderItems") { RequestedItemNotFound = orderNum.ToString() };

    }
    /// <summary>
    /// get the order item by productId and orderId
    /// </summary>
    /// <param name="orderId">num order of this order item</param>
    /// <param name="productId">num product of this order item</param>
    /// <returns>order item</returns>
    /// <exception cref="Exception"></exception>
    public OrderItem? getSingleOrederItemByProductAndOrder(int orderId, int productId)
    {

        OrderItem _newOrderItem = new OrderItem();
        _newOrderItem = DataSource._arrOrderItem.Find(e => e.OrderID == orderId&&e.ProductID==productId);
        if (_newOrderItem.ID == 0)
            throw new RequestedItemNotFoundException("orderItem not exists,can not get") { RequestedItemNotFound = orderId.ToString() };

        else
            return _newOrderItem;
    }
}


