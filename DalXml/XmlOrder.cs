using DalApi;
using Dal;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;
public class XmlOrder : IOrder
{
    string OrderPath = @"Orders.xml";


    /// <summary>
    /// add a new order to the list of orders
    /// </summary>
    /// <param name="_p">an order</param>
    /// <returns>int of the id of the order</returns>
    /// <exception cref="Exception">order exists</exception>
    public int Add(DO.Order _o)
    {
        XElement OrdersRoot = XMLTools.LoadListFromXMLElement(OrderPath);
        if (OrdersRoot == null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = _o.ToString() };
        }

        XElement ifExsistOrder = (from p in OrdersRoot.Elements()
                                  where int.Parse(p.Element("ID").Value) == _o.ID
                                  select p).FirstOrDefault();

        if (ifExsistOrder != null)

            throw new DO.ItemAlreadyExistsException("order exists, can not add") { ItemAlreadyExists = _o.ToString() };
        int id = XmlConfig.getOrderId();
        XElement OrderElement = new XElement("Order", new XElement("ID", id.ToString()),
                                new XElement("CustomerEmail", _o.CustomerEmail),
                                new XElement("CustomerName", _o.CustomerName),
                                new XElement("CustomerAdress", _o.CustomerAdress),
                                new XElement("OrderDate", DateTime.Now.ToString()),
                                new XElement("ShipDate", _o.ShipDate == null ? null : _o.ShipDate.ToString()),
                                new XElement("DeliveryDate", _o.DeliveryDate == null ? null : _o.DeliveryDate.ToString()));

        OrdersRoot.Add(OrderElement);
        XMLTools.SaveListToXMLElement(OrdersRoot, OrderPath);
        return id;
        #region temp

        //    List<DO.Order?> ListOrders = XMLTools.LoadListFromXMLSerializer<Order?>(OrderPath);

        //    if (ListOrders.FirstOrDefault(e => e?.ID == _o.ID ) != null)
        //        throw new ItemAlreadyExistsException("order exists, can not add") { ItemAlreadyExists = _o.ToString() };

        //    //לשנות ID
        //    _o.ID = XmlConfig.getOrderId();
        //    ListOrders.Add(_o); //no need to Clone()
        //    XMLTools.SaveListToXMLSerializer(ListOrders, OrderPath);
        //  return _o.ID;
       
        #endregion

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

        if (OrdersRoot == null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
        if (predict == null)
        {
            throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        }

        try
        {
            IEnumerable<DO.Order?>? ord = OrdersRoot.Elements().Select(x =>
            {
                DO.Order o = new();
                o.ID = Int32.Parse(x.Element("ID").Value);
                o.CustomerAdress = x.Element("CustomerAdress").Value;
                o.CustomerEmail = x.Element("CustomerEmail").Value;
                o.CustomerName = x.Element("CustomerName").Value;
                o.ShipDate = DateTime.Parse(x.Element("ShipDate").Value);
                o.DeliveryDate = DateTime.Parse(x.Element("DeliveryDate").Value);
                o.OrderDate = DateTime.Parse(x.Element("OrderDate").Value);
                return (DO.Order?)o;
            }).Where(x => predict(x));
            return (Order)ord.FirstOrDefault();
        }
        catch
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }

        #region temp
        //List<DO.Order?> ListOrders = XMLTools.LoadListFromXMLSerializer<Order?>(OrderPath);
        //if (predict == null)
        //{
        //    throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        //}
        //DO.Order? order = ListOrders.Find(p => predict(p));

        //if (order != null)
        //    return (DO.Order)order; //no need to Clone()
        //else

        //    throw new RequestedItemNotFoundException("orderItem not exists,can not do get") { RequestedItemNotFound = predict.ToString() };


        //if (predict == null)
        //{
        //    throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        //}

        //try
        //{
        //    return (from order in OrdersRoot.Elements()
        //            let o1 = new DO.Order()
        //            {
        //                ID = Int32.Parse(order.Element("ID").Value),
        //                CustomerAdress = order.Element("CustomerAdress").Value,
        //                CustomerEmail = order.Element("CustomerEmail").Value,
        //                CustomerName = order.Element("CustomerName").Value,
        //                ShipDate = DateTime.Parse(order.Element("ShipDate").Value),
        //                DeliveryDate = DateTime.Parse(order.Element("DeliveryDate").Value),
        //                OrderDate = DateTime.Parse(order.Element("OrderDate").Value)

        //            }
        //            where predict(o1)
        //            select o1).FirstOrDefault();
        //}
        #endregion

    }

    /// <summary>
    /// cope the orders to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the orders</returns>
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? predict = null)
    {


        XElement OrdersRoot = XMLTools.LoadListFromXMLElement(OrderPath);
        if (OrdersRoot == null)
            throw new RequestedItemNotFoundException("orders not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        try
        {
            IEnumerable<DO.Order?>? ord = OrdersRoot.Elements().Select(x =>
            {
                DO.Order o = new();
                o.ID = Int32.Parse(x.Element("ID").Value.ToString());
                o.CustomerAdress = x.Element("CustomerAdress").Value.ToString();
                o.CustomerEmail = x.Element("CustomerEmail").Value.ToString();
                o.CustomerName = x.Element("CustomerName").Value.ToString();
                try
                {
                    o.ShipDate = DateTime.Parse(x.Element("ShipDate").Value.ToString());
                    o.DeliveryDate = DateTime.Parse(x.Element("DeliveryDate").Value.ToString());
                    o.OrderDate = DateTime.Parse(x.Element("OrderDate").Value.ToString());
                }
                catch
                {
                    o.ShipDate = null;
                    o.DeliveryDate = null;
                    o.OrderDate = null;
                }
                return (DO.Order?)o;
            }).Where(x => predict == null || predict(x));
            return ord;
        }
        catch
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
        #region temp

        //if (predict == null)
        //{
        //    try
        //    {
        //        var i= ((from order in OrdersRoot.Element()
        //                                      select new DO.Order()
        //                                      {
        //                                          ID = Int32.Parse(order.Element("ID").Value.ToString()),
        //                                          CustomerAdress = order.Element("CustomerAdress").Value.ToString(),
        //                                          CustomerEmail = order.Element("CustomerEmail").Value.ToString(),
        //                                          CustomerName = order.Element("CustomerName").Value.ToString(),
        //                                          ShipDate = DateTime.Parse(order.Element("ShipDate").Value.ToString()),
        //                                          DeliveryDate = DateTime.Parse(order.Element("DeliveryDate").Value.ToString()),
        //                                          OrderDate = DateTime.Parse(order.Element("OrderDate").Value.ToString())

        //                                      }));
        //        return (IEnumerable<Order?>)i;
        //    }
        //    catch
        //    {
        //        throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        //    }
        //}
        //try
        //{
        //    return ((IEnumerable<Order?>)from order in OrdersRoot.Elements()
        //                                 let o1 = new DO.Order()
        //                                 {
        //                                     ID = Int32.Parse(order.Element("ID").Value),
        //                                     CustomerAdress = order.Element("CustomerAdress").Value,
        //                                     CustomerEmail = order.Element("CustomerEmail").Value,
        //                                     CustomerName = order.Element("CustomerName").Value,
        //                                     ShipDate = DateTime.Parse(order.Element("ShipDate").Value),
        //                                     DeliveryDate = DateTime.Parse(order.Element("DeliveryDate").Value),
        //                                     OrderDate = DateTime.Parse(order.Element("OrderDate").Value)

        //                                 }
        //                                 where predict(o1)
        //                                 select o1);
        //}
        //catch
        //{
        //    throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        //}
        //List<DO.Order?> ListOrders = XMLTools.LoadListFromXMLSerializer<Order?>(OrderPath);
        //if (predict == null)
        //{
        //    return (IEnumerable<Order?>)ListOrders;
        //}
        //IEnumerable<Order?> order = ListOrders.FindAll(p => predict(p));

        //if (order is null)
        //    throw new RequestedItemNotFoundException("order not exists,can not do get") { RequestedItemNotFound = predict.ToString() };

        //return order;

        #endregion
    }

    /// <summary>
    /// check if the order demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of order to delete</param>
    /// <exception cref="Exception">order not exists, can not delete</exception>
    public void Delete(int _id)
    {

        //XElement OrderRoot = XMLTools.LoadListFromXMLElement(OrderPath);

        //if (OrderRoot is null)
        //    throw new RequestedItemNotFoundException("orders not exists,can not get") { RequestedItemNotFound = _id.ToString() };

        //XElement ord = (from ordr in OrderRoot.Elements()
        //                where int.Parse(ordr.Element("ID").Value) == _id
        //                select ordr).FirstOrDefault();

        //if (ord != null)
        //{
        //    ord.Remove(); //<==>   Remove per from personsRootElem

        //    XMLTools.SaveListToXMLElement(OrderRoot, OrderPath);
        //}
        //else
        //    throw new RequestedItemNotFoundException("order not exists,can not delete") { RequestedItemNotFound = _id.ToString() };
        #region temp

        List<DO.Order?> ListOrders = XMLTools.LoadListFromXMLSerializer<DO.Order?>(OrderPath);

        DO.Order? ord = ListOrders.Find(p => p?.ID == _id);
        try
        {
            if (ord != null)
            {
                ListOrders.Remove(ord);
                XMLTools.SaveListToXMLSerializer(ListOrders, OrderPath);
            }
        }
        catch
        {
            throw new RequestedItemNotFoundException("order not exists,can not delete") { RequestedItemNotFound = _id.ToString() };
        }
        #endregion
    }

    /// <summary>
    /// update date of order and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of order demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void Update(Order _o)
    {

        //XElement OrdersRoot = XMLTools.LoadListFromXMLElement(OrderPath);
        //if (OrdersRoot is null)
        //    throw new RequestedUpdateItemNotFoundException("order not exists,can not update") { RequestedUpdateItemNotFound = _o.ToString() };
        //XElement ord = (from ordr in OrdersRoot.Elements()
        //                where int.Parse(ordr.Element("ID").Value) == _o.ID
        //                select ordr).FirstOrDefault();

        //if (ord != null)
        //{

        //    ord.Element("ID").Value = _o.ID.ToString();
        //    ord.Element("CustomerEmail").Value = _o.CustomerEmail;
        //    ord.Element("CustomerName").Value = _o.CustomerName;
        //    ord.Element("CustomerAdress").Value = _o.CustomerAdress;
        //    ord.Element("OrderDate").Value = DateTime.Now.ToString();
        //    ord.Element("ShipDate").Value = null;
        //    ord.Element("DeliveryDate").Value = null;
        //    ord.Element("ID").Value = _o.ID.ToString();

        //    XMLTools.SaveListToXMLElement(OrdersRoot, OrderPath);
        //}
        #region temp
        List<DO.Order?> ListOrders = XMLTools.LoadListFromXMLSerializer<DO.Order?>(OrderPath);
        if (ListOrders is null)
            throw new RequestedItemNotFoundException("orderItem not exists,can not do get") { RequestedItemNotFound = _o.ToString() };
        DO.Order? ord = ListOrders.Find(p => p?.ID == _o.ID);
        try
        {
            if (ord != null)
            {
                ListOrders.Remove(ord);
                ListOrders.Add(_o); //no nee to Clone()
                XMLTools.SaveListToXMLSerializer(ListOrders, OrderPath);
            }
        }
        catch
        {
            throw new RequestedItemNotFoundException("orderItem not exists,can not update") { RequestedItemNotFound = _o.ToString() };
        }
        #endregion

    }


  

}
