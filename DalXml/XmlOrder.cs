using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;
internal class XmlOrder:IOrder
{
    string OrderPath = @"OrdersXml.xml";


    /// <summary>
    /// add a new order to the list of orders
    /// </summary>
    /// <param name="_p">an order</param>
    /// <returns>int of the id of the order</returns>
    /// <exception cref="Exception">order exists</exception>
    public int Add(DO.Order? _o)
    {
        XElement OrdersRoot = XMLTools.LoadListFromXMLElement(OrderPath);
        XElement ifExsistOrder = (from p in OrdersRoot.Elements()
                         where int.Parse(p.Element("ID").Value) == _o?.ID
                                  select p).FirstOrDefault();

        if (ifExsistOrder != null)

            throw new DO.ItemAlreadyExistsException("order exists, can not add") { ItemAlreadyExists = _o.ToString() };

        XElement OrderElement = new XElement("Order", new XElement("ID", _o?.ID.ToString()),
                                new XElement("CustomerEmail", _o?.CustomerEmail),
                                new XElement("CustomerName", _o?.CustomerName),
                                new XElement("CustomerAdress", _o?.CustomerAdress),
                                new XElement("OrderDate", DateTime.Now.ToString()),
                                new XElement("ShipDate", null),
                                new XElement("DeliveryDate", null));
                
        OrdersRoot.Add(OrderElement);
        XMLTools.SaveListToXMLElement(OrdersRoot,OrderPath);
        return 1;
    }
    /// <summary>
    /// check if the order demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="_num">the id of the order demanded</param>
    /// <returns>details of the order demanded</returns>
    /// <exception cref="Exception">order not exists</exception>
    public Order Get(Func<Order?, bool>? predict)
    {
        XElement OrdersRoot = XMLTools.LoadListFromXMLElement(OrderPath);

        if (OrdersRoot== null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
        if (predict == null)
        {
            throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        }

        try { 
        return (from order in OrdersRoot.Elements()
                                     let o1 = new DO.Order()
                                     {
                                         ID = Int32.Parse(order.Element("ID").Value),
                                         CustomerAdress = order.Element("CustomerAdress").Value,
                                         CustomerEmail = order.Element("CustomerEmail").Value,
                                         CustomerName = order.Element("CustomerName").Value,
                                         ShipDate = DateTime.Parse(order.Element("ShipDate").Value),
                                         DeliveryDate = DateTime.Parse(order.Element("DeliveryDate").Value),
                                         OrderDate = DateTime.Parse(order.Element("OrderDate").Value)

                                     }
                                     where predict(o1)
                                     select o1).FirstOrDefault();
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
        XElement OrdersRoot = XMLTools.LoadListFromXMLElement(OrderPath);
        if (OrdersRoot==null)
                    throw new RequestedItemNotFoundException("orders not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        if (predict == null)
        {
            try
            {
                return ((IEnumerable<Order?>)(from order in OrdersRoot.Elements()
                                              select new DO.Order()
                                              {
                                                  ID = Int32.Parse(order.Element("ID").Value),
                                                  CustomerAdress = order.Element("CustomerAdress").Value,
                                                  CustomerEmail = order.Element("CustomerEmail").Value,
                                                  CustomerName = order.Element("CustomerName").Value,
                                                  ShipDate = DateTime.Parse(order.Element("ShipDate").Value),
                                                  DeliveryDate = DateTime.Parse(order.Element("DeliveryDate").Value),
                                                  OrderDate = DateTime.Parse(order.Element("OrderDate").Value)

                                              }));
            }
            catch
            {
                throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
            }
        }
        try
        {
            return ((IEnumerable<Order?>)from order in OrdersRoot.Elements()
                                         let o1 = new DO.Order()
                                         {
                                             ID = Int32.Parse(order.Element("ID").Value),
                                             CustomerAdress = order.Element("CustomerAdress").Value,
                                             CustomerEmail = order.Element("CustomerEmail").Value,
                                             CustomerName = order.Element("CustomerName").Value,
                                             ShipDate = DateTime.Parse(order.Element("ShipDate").Value),
                                             DeliveryDate = DateTime.Parse(order.Element("DeliveryDate").Value),
                                             OrderDate = DateTime.Parse(order.Element("OrderDate").Value)

                                         }
                                         where predict(o1)
                                         select o1);
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




}
