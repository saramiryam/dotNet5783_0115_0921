
using Dal;
using DO;
using DalApi;
using System;
using System.Runtime.CompilerServices;

namespace Dal;
internal class DalOrder : IOrder
{
    XmlOrder xmlOrder = new();
    #region methods

    /// <summary>
    /// add a new order to the list of orders
    /// </summary>
    /// <param name="_p">an order</param>
    /// <returns>int of the id of the order</returns>
    /// <exception cref="Exception">order exists</exception>
     [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Order _o)
    {

        if ((DataSource._Orders
                     .Where(e => e?.ID == _o.ID)
                     .Select(e => (DO.Order?)e).FirstOrDefault() is not null))
            throw new ItemAlreadyExistsException("order exists, can not add") { ItemAlreadyExists = _o.ToString() };
        else
        {
            _o.ID = DataSource.Config.CalNumOfIDOrder;
            _o.OrderDate = DateTime.Now;
            _o.ShipDate = null;
            _o.DeliveryDate = null;
            DataSource._Orders.Add(_o);
            return _o.ID;
        }
    }


    /// <summary>
    /// check if the order demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="_num">the id of the order demanded</param>
    /// <returns>details of the order demanded</returns>
    /// <exception cref="Exception">order not exists</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order Get(Func<Order?, bool>? predict)
    {
        if (DataSource._Orders == null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
        if (predict == null)
        {
            throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        }
        try
        {
            return DataSource._Orders
                     .Where(e => predict(e))
                     .Select(e => (Order)e!).First();
        }
        catch
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
    }


    /// <summary>
    /// cope the orders to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the orders</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? predict = null)
    {
        if (DataSource._Orders == null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
        if (predict == null)
        {
            return (IEnumerable<Order?>)DataSource._Orders;
        }

        try
        {
            return DataSource._Orders
                   .Where(e => predict(e))
                   .Select(e => (DO.Order?)e!).ToList();

        }
        catch
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
    }


    /// <summary>
    /// check if the order demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of order to delete</param>
    /// <exception cref="Exception">order not exists, can not delete</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int _num)
    {
        if (DataSource._Orders == null) throw new RequestedItemNotFoundException("order not exists,can not delete") { RequestedItemNotFound = _num.ToString() };
        try
        {
            DataSource._Orders.Remove(DataSource._Orders
                  .Where(e => e is not null && e.Value.ID == _num)
                  .Select(e => (Order)e!).FirstOrDefault());
        }
     
        catch
        {
            throw new RequestedItemNotFoundException("order not exists,can not delete") { RequestedItemNotFound = _num.ToString() };
        }
    }


    /// <summary>
    /// update date of order and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of order demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order _o)
    {
        if (_o.CustomerName == null && _o.CustomerEmail == null && _o.CustomerAdress == null && _o.OrderDate == null && _o.ShipDate == null && _o.DeliveryDate == null)
        {
            return;

        }
        if (DataSource._Orders == null) throw new RequestedUpdateItemNotFoundException("order not exists,can not update") { RequestedUpdateItemNotFound = _o.ToString() };

        try
        {
            DataSource._Orders.Remove(DataSource._Orders
                  .Where(e => e is not null && e.Value.ID == _o.ID)
                  .Select(e => (Order?)e!).First());
            DataSource._Orders.Add(_o);
        }

        catch
        {

            throw new RequestedUpdateItemNotFoundException("order not exists,can not update") { RequestedUpdateItemNotFound = _o.ToString() };
        }
        }

        #endregion

    }
