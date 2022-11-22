
using Dal;
using DO;
using DalApi;
using System;

namespace Dal;
internal class DalOrder:IOrder
{

    #region methods

    /// <summary>
    /// add a new order to the list of orders
    /// </summary>
    /// <param name="_p">an order</param>
    /// <returns>int of the id of the order</returns>
    /// <exception cref="Exception">order exists</exception>
    public int Add(Order _o)
    {
        if (DataSource._Orders.Exists(e => e.CustomerName == _o.CustomerName && e.CustomerAdress == _o.CustomerAdress&& e.CustomerEmail == _o.CustomerEmail && e.OrderDate == _o.OrderDate))
            throw new RequestedItemNotFoundException("order exists, can not add") { RequestedItemNotFound = _o.ToString() };
        else
        {
            _o.ID = DataSource.Config.CalNumOfIDOrder;
            _o.OrderDate = DateTime.Now;
            _o.ShipDate = DateTime.MinValue;
            _o.DeliveryDate = DateTime.MinValue;
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
    public Order Get(int _num)
    {
        Order _orderToGet = new Order();
        _orderToGet = DataSource._Orders.Find(e => e.ID == _num);
        if (_orderToGet.ID != 0)
            return _orderToGet;
        else
            throw new ItemAlreadyExistsException("order not exists,can not get") { ItemAlreadyExists = _num.ToString() };

    }

    /// <summary>
    /// cope the orders to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the orders</returns>
    public IEnumerable<Order> GetAll()
    {
        return DataSource._Orders;
    }

    /// <summary>
    /// check if the order demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of order to delete</param>
    /// <exception cref="Exception">order not exists, can not delete</exception>
    public void Delete(int _num)
    {
        Order _orderToDel = DataSource._Orders.Find(e => e.ID == _num);
        if (_orderToDel.ID != 0)
            DataSource._Orders.Remove(_orderToDel);
        else
            throw new Exception("order not exists, can not delete");
    }

    /// <summary>
    /// update date of order and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of order demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void Update(Order _o)
    {
        if (_o.ID == null || _o.CustomerName == null || _o.CustomerEmail == null || _o.CustomerAdress == null || _o.OrderDate == null||_o.ShipDate==null||_o.DeliveryDate==null)
        {
            return;

        }
        Order _orderToUpdate = DataSource._Orders.Find(e => e.ID == _o.ID);
        if (_orderToUpdate.ID != 0)
        {
            DataSource._Orders.Remove(_orderToUpdate);
            DataSource._Orders.Add(_o);
        }
        else
            throw new ItemAlreadyExistsException("order not exists,can not update") { ItemAlreadyExists = _o.ToString() };
        
    }

    #endregion

}
