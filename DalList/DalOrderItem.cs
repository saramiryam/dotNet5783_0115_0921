
using DO;

namespace Dal;
using DalApi;
using System;
using System.Collections;

internal class DalOrderItem : IOrderItem
{/// <summary>
/// add a new orderitem and throw exception if it does not exist
/// </summary>
/// <param name="_newOrderItem">new one to add</param>
/// <returns>new orderIdem id</returns>
/// <exception cref="Exception"></exception>
    public int Add(OrderItem _newOrderItem)
    {
        if (DataSource._arrOrderItem.Exists(e => e?.OrderID == _newOrderItem.ID && e?.ProductID == _newOrderItem.ProductID))
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
    public OrderItem Get(Func<OrderItem?, bool>? predict)
    {

        if (DataSource._arrOrderItem == null)
        {
            throw new RequestedItemNotFoundException("orderItem not exists,can not do get") { RequestedItemNotFound = predict?.ToString() };
        }
        if (predict == null)
        {
            throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        }
        //OrderItem? _newOrderItem = new OrderItem();
        //_newOrderItem = DataSource._arrOrderItem.Find(e => predict(e));
        try
        {
            return DataSource._arrOrderItem
                        .Where(e => predict(e))
                        .Select(e => (OrderItem)e!).First();
        }
        catch
        {
            throw new RequestedItemNotFoundException("orderItem not exists,can not do get") { RequestedItemNotFound = predict.ToString() };
        }

    }
    /// <summary>
    /// return all order items and throw exception if it does not exist
    /// </summary>
    /// <returns>order item arr</returns>
    public IEnumerable<OrderItem?> GetAll(Func<OrderItem?, bool>? predict = null)
    {
        List<OrderItem?> _OrderItems = new List<OrderItem?>();
        if (DataSource._arrOrderItem == null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
        if (predict == null)
        {
            _OrderItems=DataSource._arrOrderItem;
        }
        else
        {
            //_OrderItems = DataSource._arrOrderItem.FindAll(e => predict(e));
            return DataSource._arrOrderItem
         .Where(e => predict(e))
         .Select(e => (DO.OrderItem?)e!).ToList();
        }

        if (_OrderItems.Count > 0)
            return _OrderItems;
        else
            throw new RequestedItemNotFoundException("order not exists,can not get all orderItems") { RequestedItemNotFound = predict?.ToString() };
    }
    /// <summary>
    ///  delete order item and throw exception if it does not exist
    /// 
    /// </summary>
    /// <param name="_orderItemID">order item to delete</param>
    /// <exception cref="Exception"></exception>
    public void Delete(int _orderItemID)
    {
        if (DataSource._arrOrderItem == null) throw new RequestedItemNotFoundException("orderItem not exists,can not do get") { RequestedItemNotFound = _orderItemID.ToString() };
        //OrderItem? _orderItemToDel = new OrderItem();
        //_orderItemToDel = DataSource._arrOrderItem.Find(e => e.HasValue && e!.Value.ID == _orderItemID);
        try
        {
            DataSource._arrOrderItem.Remove(DataSource._arrOrderItem
                  .Where(e => e is not null && e.Value.ID == _orderItemID)
                  .Select(e => (OrderItem)e!).First());
        }
        catch
        {
            throw new RequestedItemNotFoundException("orderItem not exists,can not delete") { RequestedItemNotFound = _orderItemID.ToString() };
        }
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

        if (DataSource._arrOrderItem == null) throw new RequestedItemNotFoundException("orderItem not exists,can not do get") { RequestedItemNotFound = _newOrderItem.ToString() };
        //OrderItem? _orderItemToUpdate = new OrderItem();
        //_orderItemToUpdate = DataSource._arrOrderItem.Find(e => e.HasValue && e!.Value.ID == _newOrderItem.ID && e.Value.OrderID == _newOrderItem.OrderID && e.Value.ProductID == _newOrderItem.ProductID);
        try
        {
            DataSource._arrOrderItem.Remove(DataSource._arrOrderItem
               .Where(e => e is not null && e.Value.ID == _newOrderItem.ID)
               .Select(e => (OrderItem?)e!).First());
            DataSource._arrOrderItem.Add(_newOrderItem);
        }
        catch { 
            throw new RequestedItemNotFoundException("orderItem not exists,can not update") { RequestedItemNotFound = _newOrderItem.ToString() };

        }
    }
   
}


