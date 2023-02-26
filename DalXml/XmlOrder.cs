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
    #region methods

    /// <summary>
    /// add a new product to the list of products
    /// </summary>
    /// <param name="_p">a product</param>
    /// <returns>int of the id of the product</returns>
    /// <exception cref="Exception">product exists</exception>
    public int Add(Order _p)
    {
        List<Order?> ListOrder = XMLTools.LoadListFromXMLSerializer<Order?>(OrderPath);
        if (ListOrder.FirstOrDefault(e => e?.CustomerEmail == _p.CustomerEmail && e?.CustomerName == _p.CustomerName && e?.OrderDate == _p.OrderDate ) is not null)
        {
            throw new ItemAlreadyExistsException("Order exists, can not add") { ItemAlreadyExists = _p.ToString() };
        }
        _p.ID = XmlConfig.getOrderId();
        ListOrder.Add(_p);
        XMLTools.SaveListToXMLSerializer(ListOrder, OrderPath);
        return _p.ID;
    }


    /// <summary>
    /// check if the product demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="_num">the id of the product demanded</param>
    /// <returns>details of the product demanded</returns>
    /// <exception cref="Exception">product not exists</exception>
    public Order Get(Func<Order?, bool>? predict)
    {
        List<Order?> ListOrder = XMLTools.LoadListFromXMLSerializer<Order?>(OrderPath);

        if (ListOrder == null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = null };
        }
        if (predict == null)
        {
            throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        }
        try
        {
            DO.Order? order = ListOrder.Find(p => predict(p));
            return (Order)order;
        }
        catch
        {
            throw new RequestedItemNotFoundException("product not exists,can not do get") { RequestedItemNotFound = predict.ToString() };
        }
    }


    /// <summary>
    /// cope the products to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the products</returns>
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? predict = null)
    {
        List<Order?> ListOrder = XMLTools.LoadListFromXMLSerializer<Order?>(OrderPath);


        if (ListOrder == null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = "jjj".ToString() };
        }
        if (predict == null)
        {
            return (IEnumerable<Order?>)ListOrder;

        }
        else
        {
            try
            {
                IEnumerable<Order?> order = ListOrder.FindAll(p => predict(p));
                return (IEnumerable<Order?>)order;
            }
            catch
            {
                throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = "jjj".ToString() };
            }
        }
    }


    /// <summary>
    /// check if the product demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of product to delete</param>
    /// <exception cref="Exception">product not exists, can not delete</exception>
    public void Delete(int _num)
    {
        List<Order?> ListOrder = XMLTools.LoadListFromXMLSerializer<Order?>(OrderPath);

        if (ListOrder == null)
        {
            throw new RequestedItemNotFoundException("Order not exists,can not get") { RequestedItemNotFound = _num.ToString() };
        }
        DO.Order? pre = ListOrder.Find(p => p?.ID == _num);
        try
        {
            if (pre != null)
            {
                ListOrder.Remove(pre);
                XMLTools.SaveListToXMLSerializer(ListOrder, OrderPath);

            }
        }
        catch
        {
            throw new RequestedItemNotFoundException("Order not exists,can not do delete") { RequestedItemNotFound = _num.ToString() };
        }


    }


    /// <summary>
    /// update data of product and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of product demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void Update(Order _o)
    {
        List<Order?> ListOrder = XMLTools.LoadListFromXMLSerializer<Order?>(OrderPath);
        if (ListOrder is null)
            throw new RequestedItemNotFoundException("Order not exists,can not get") { RequestedItemNotFound = _o.ToString() };
        DO.Order? order = ListOrder.Find(p => p?.ID == _o.ID);
        try
        {
            if (order != null)
            {
                ListOrder.Remove(order);
                ListOrder.Add(_o); //no nee to Clone()
                XMLTools.SaveListToXMLSerializer(ListOrder, OrderPath);

            }
        }
        catch
        {
            throw new RequestedItemNotFoundException("Order not exists,can not do update") { RequestedItemNotFound = _o.ToString() };
        }










































   
        #endregion

    }
  

}
