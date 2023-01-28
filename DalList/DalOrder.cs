
using Dal;
using DO;
using DalApi;
using System;

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
    public int Add(Order _o)
    {

        if ((DataSource._Orders
                     .Where(e => e?.ID == _o.ID)
                     .Select(e => (DO.Order?)e).FirstOrDefault() is not null))
            //if (DataSource._Orders.Exists(e => e?.CustomerName == _o.CustomerName && e?.CustomerAdress == _o.CustomerAdress && e?.CustomerEmail == _o.CustomerEmail && e?.OrderDate == _o.OrderDate))
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
        //Order? _orderToGet = new Order();
        //_orderToGet = DataSource._Orders.Find(e=> predict(e)); 
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

        //List<Order?> _Orders = new List<Order?>();
        //_Orders = DataSource._Orders.FindAll(e => predict(e));
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
    public void Delete(int _num)
    {
        if (DataSource._Orders == null) throw new RequestedItemNotFoundException("order not exists,can not delete") { RequestedItemNotFound = _num.ToString() };
        //Order? _orderToDel = new Order();
        //_orderToDel = DataSource._Orders.Find(e => e.HasValue && e!.Value.ID == _num);
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
    public void Update(Order _o)
    {
        if (_o.CustomerName == null && _o.CustomerEmail == null && _o.CustomerAdress == null && _o.OrderDate == null && _o.ShipDate == null && _o.DeliveryDate == null)
        {
            return;

        }
        if (DataSource._Orders == null) throw new RequestedUpdateItemNotFoundException("order not exists,can not update") { RequestedUpdateItemNotFound = _o.ToString() };
        //Order? _orderToUpdate = new Order();
        //_orderToUpdate = DataSource._Orders.Find(e => e.HasValue && e!.Value.ID == _o.ID);

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
