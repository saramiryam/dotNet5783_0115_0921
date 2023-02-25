using BlApi;
using BO;
using Dal;
using DalApi;
using DO;
using System.Resources;
using System;
using System.Runtime.CompilerServices;
using static BO.Enums;
using Factory = DalApi.Factory;

namespace BlImplementation;

public class Order : BlApi.IOrder
{
    IDal? Dal = Factory.Get();
    //private static IDal? Dal = Factory.Get();
    #region method
    /// <summary>
    /// get the whole list of orders
    /// </summary>
    /// <returns> list of orders</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderForList?> GetListOfOrders()
    {
        IEnumerable<DO.Order?> orderList = new List<DO.Order?>();
        if (Dal != null)
        {
            orderList = Dal.Order.GetAll();
        }

        IEnumerable<OrderTracking> orderTracking = new List<OrderTracking>();
        var addOrder = orderList
                       .Where(item => item != null)
                       .Select(item => new BO.OrderForList()
                       {
                           OrderID = item!.Value.ID,
                           CustomerName = item.Value.CustomerName,
                           Status = CheckStatus(item.Value.OrderDate, item.Value.ShipDate, item.Value.DeliveryDate),
                           AmountOfItem = GetAmountItems(item.Value.ID),
                           TotalSum = CheckTotalSum(item.Value.ID)
                       });
        return addOrder.ToList();
    }


    /// <summary>
    /// get one order by id
    /// </summary>
    /// <param name="id"> id of order demeded</param>
    /// <returns>order demended</returns>
    /// <exception cref="BO.NegativeIdException">Negative Id</exception>
    /// <exception cref="BO.OrderNotExistsException">Order Not Exists</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order GetOrderDetails(int id)
    {
        if (id <= 0)
        {
            throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };
        }
        else
        {
            DO.Order o = new DO.Order();
            try
            {
                if (Dal != null)
                {
                    o = Dal.Order.Get(e => e?.ID == id);
                }
            }
            catch (DO.RequestedItemNotFoundException)
            {
                throw new BO.OrderNotExistsException("order not exists") { OrderNotExists = o.ToString() };

            }
            return DOorderToBOorder(o);


        }
    }


    /// <summary>
    /// get item details of demended order
    /// </summary>
    /// <param name="MyCart">from whitc catr</param>
    /// <param name="ItemId">from whitc item</param>
    /// <returns>the details</returns>
    /// <exception cref="BO.NegativeIdException">Negative Id</exception>
    /// <exception cref="BO.OrderNotExistsException">Order Not Exists</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.OrderItem GetOrderItemDetails(BO.Cart MyCart, int ItemId)
    {

        if (ItemId <= 0)
        {
            throw new BO.NegativeIdException("negative id") { NegativeId = ItemId.ToString() };
        }
        else
        {
            DO.OrderItem item = new();
            BO.OrderItem newItem = new();
            try
            {
                if (Dal != null)
                {
                    IEnumerable<BO.OrderItem?> OrderItems = MyCart.ItemList;
                    newItem = (OrderItems
                        .Where(i => i is not null && i.ID == ItemId)
                        .Select(i => new BO.OrderItem()
                        {
                            numInOrder = i.numInOrder + 1,
                            ID = i.ID,
                            Name = (from product in Dal.Product.GetAll()
                                    where product.Value.ID == i.ID
                                    select new { product.Value.Name }).First().Name,
                            Price = i.Price,
                            Amount = i.Amount,
                            sumItem = i.Amount * i.Price
                        })).First();
                    ;
                }
            }
            catch (DO.RequestedItemNotFoundException)
            {
                throw new BO.OrderNotExistsException("order not exists") { OrderNotExists = newItem.ToString() };

            }
            return newItem;


        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.OrderNotExistsException"></exception>
    /// <exception cref="BO.OrderHasAlreadySentException"></exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateShipDate(int id)
    {

        DO.Order o = new DO.Order();
        try
        {
            if (Dal != null)
            {
                o = Dal.Order.Get(e => e?.ID == id);
            }
        }
        catch (DO.RequestedItemNotFoundException)
        {
            throw new BO.OrderNotExistsException("order not exists") { OrderNotExists = o.ToString() };

        }
        try
        {
            if (CheckStatus(o.OrderDate, o.ShipDate, o.DeliveryDate) == BO.Enums.EStatus.Done)
            {
                o.ShipDate = DateTime.Now;
                try
                {
                    if (Dal != null)
                    {
                        Dal.Order.Update(o);
                    }
                }
                catch (DO.RequestedUpdateItemNotFoundException)
                {

                    throw new BO.UpdateOrderNotSucceedException("update order not succeed") { UpdateOrderNotSucceed = o.ToString() };
                }


            }

        }
        catch
        {
            throw new BO.OrderHasAlreadySentException("Order has already sent") { OrderHasAlreadySent = id.ToString() };

        }
        return DOorderToBOorder(o); ;


    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order UpdateDeliveryDate(int id)
    {

        DO.Order o = new DO.Order();
        try
        {
            if (Dal != null)
            {
                o = Dal.Order.Get(e => e?.ID == id);
            }
        }
        catch
        {
            throw new BO.OrderNotExistsException("order not exists") { OrderNotExists = o.ToString() };

        }
        try
        {
            if (CheckStatus(o.OrderDate, o.ShipDate, o.DeliveryDate) == BO.Enums.EStatus.Sent)
            {
                o.DeliveryDate = DateTime.Now;
                try
                {
                    if (Dal != null)
                    {
                        Dal.Order.Update(o);
                    }
                }
                catch (DO.RequestedUpdateItemNotFoundException)
                {

                    throw new BO.UpdateOrderNotSucceedException("update order not succeed") { UpdateOrderNotSucceed = o.ToString() };
                }


            }

        }
        catch
        {
            throw new BO.OrderHasAlreadyProvidedException("Order has already sent") { OrderHasAlreadyProvided = id.ToString() };

        }
        return DOorderToBOorder(o); ;


    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.OrderTracking GetOrderTracking(int orderId)
    {
        DO.Order o = new DO.Order();
        try
        {
            if (Dal != null)
            {
                o = Dal.Order.Get(e => e?.ID == orderId);
            }
        }
        catch (DO.RequestedItemNotFoundException)
        {
            throw new BO.OrderNotExistsException("order not exists") { OrderNotExists = o.ToString() };

        }
        BO.OrderTracking orderTracking1 = new BO.OrderTracking();
        orderTracking1.ID = orderId;
        orderTracking1.Status = CheckStatus(o.OrderDate, o.ShipDate, o.DeliveryDate);
        switch (orderTracking1.Status)
        {
            case EStatus.Done:
                var i = new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Statuss = BO.Enums.EStatus.Done
                };

                orderTracking1.listOfStatus = new List<OrderTracking.StatusAndDate?> { i };
                break;
            case EStatus.Sent:
                var i1 = new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Statuss = BO.Enums.EStatus.Done
                };

                orderTracking1.listOfStatus = new List<OrderTracking.StatusAndDate?> { i1 };
                orderTracking1.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.ShipDate,
                    Statuss = BO.Enums.EStatus.Sent

                });
                break;
            case EStatus.Provided:
                var i2 = new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Statuss = BO.Enums.EStatus.Done
                };

                orderTracking1.listOfStatus = new List<OrderTracking.StatusAndDate?> { i2 };
                orderTracking1.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.ShipDate,
                    Statuss = BO.Enums.EStatus.Sent

                }); orderTracking1.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.DeliveryDate,
                    Statuss = BO.Enums.EStatus.Provided

                });
                break;
        }
        return orderTracking1;

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? getOrderToPromote()
    {
        IEnumerable<DO.Order?> orderList = new List<DO.Order?>();
        if (Dal != null)
        {
            orderList = Dal.Order.GetAll();
        }
        var minShipDate = (from o in orderList
                           where o.Value.ShipDate is not null
                           orderby o.Value.ShipDate
                           select o.Value.ID).FirstOrDefault();
        var minOrderDate = (from o in orderList
                           where o.Value.ShipDate is null && o.Value.DeliveryDate is null
                            orderby o.Value.OrderDate
                           select o.Value.ID).FirstOrDefault();
        return (minShipDate>minOrderDate)?minOrderDate:minShipDate;

    }
    #endregion
   #region bonus
    public void ManagerActions(int orderId, int productId, int amount)
    {
        List<DO.OrderItem> ordersItem = new List<DO.OrderItem>();
        if (Dal != null)
        {
            ordersItem = (List<DO.OrderItem>)Dal.OrderItem.GetAll(e => e?.OrderID == orderId);
        }

        var flag = (ordersItem
                       .Where(e => e.ProductID == productId && e.OrderID == orderId)
                       .Select(e => (DO.OrderItem?)e).FirstOrDefault());
        //ordersItem.Exists(e => e.ProductID == productId && e.OrderID == orderId);
        if (flag is not null)
        {
            //find
            //DO.OrderItem OI = ordersItem.Find(e => e.ProductID == productId && e.OrderID == orderId);
            DO.OrderItem OI = ordersItem
                .Where(e => e.ProductID == productId && e.OrderID == orderId)
                .Select(e => (DO.OrderItem)e!).First();
            if (amount == 0)
            {
                if (Dal != null)
                {
                    Dal.OrderItem.Delete(OI.ID);
                }
            }
            else
            {
                if (Dal != null)
                {
                    DO.Product p = Dal.Product.Get(e => e?.ID == productId);
                    if (amount > p.InStock)
                    {
                        throw new BO.NotEnoughInStockException("not enough in stock") { NotEnoughInStock = amount.ToString() };
                    }
                    else
                    {
                        OI.Amount = amount;
                        Dal.OrderItem.Update(OI);
                    }
                }
            }
        }
        else
        {
            //didnt find
            try
            {
                if (Dal != null)
                {
                    DO.Product p = Dal.Product.Get(e => e?.ID == productId);
                    if (amount > p.InStock)
                    {
                        throw new BO.NotEnoughInStockException("not enough in stock") { NotEnoughInStock = amount.ToString() };
                    }
                    Dal.OrderItem.Add(new DO.OrderItem()
                    {
                        ID = 0,
                        ProductID = productId,
                        OrderID = orderId,
                        Price = p.Price,
                        Amount = amount
                    });
                }
            }
            catch
            {
                throw new BO.ProductNotExistsException("product not exists") { ProductNotExists = productId.ToString() };

            }


        }



    }

    #endregion


    #region ezer

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order DOorderToBOorder(DO.Order o)
    {
        BO.Order newOrder = new BO.Order()
        {
            ID = o.ID,
            CustomerName = o.CustomerName,
            CustomerEmail = o.CustomerEmail,
            CustomerAdress = o.CustomerAdress,
            Status = CheckStatus(o.OrderDate, o.ShipDate, o.DeliveryDate),
            OrderDate = o.OrderDate,
            ShipDate = o.ShipDate,
            DeliveryDate = o.DeliveryDate,
            ItemList = GetAllItemsToOrder(o.ID),
            TotalSum = CheckTotalSum(o.ID)


        };
        return newOrder;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Enums.EStatus CheckStatus(DateTime? OrderDate, DateTime? ShipDate, DateTime? DeliveryDate)
    {
        DateTime today = DateTime.Now;
        if (today >= OrderDate && today >= ShipDate && ShipDate != null && today >= DeliveryDate && DeliveryDate != null)
            return EStatus.Provided;
        else if (today >= OrderDate && today >= ShipDate && ShipDate != null)
            return EStatus.Sent;
        else
            return EStatus.Done;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int GetAmountItems(int id)
    {
        IEnumerable<DO.OrderItem?> orderItemList = new List<DO.OrderItem?>();
        try
        {
            if (Dal != null)
            {
                orderItemList = Dal.OrderItem.GetAll(e => e?.OrderID == id);
            }
        }
        catch
        {
            throw new BO.OrderNotExistsException("order not exists,can not get all orderItems") { OrderNotExists = id.ToString() };

        }
        //int sum = 0;
        //foreach (var item in orderItemList)
        //{
        //    if (item != null)
        //        sum += item.Value.Amount;
        //}
        //return sum;

        return orderItemList
                       .Where(item => item != null)
                       .Sum(item => item!.Value.Amount);


    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public double CheckTotalSum(int id)
    {
        IEnumerable<DO.OrderItem?> orderItemList = new List<DO.OrderItem?>();
        if (Dal != null)
        {
            orderItemList = (IEnumerable<DO.OrderItem?>)Dal.OrderItem.GetAll(e => e?.OrderID == id);
            //double sum = 0;
            //foreach (var item in orderItemList)
            //{
            //    if (item != null)
            //    {
            //        sum = sum + item.Value.Price * item.Value.Amount;
            //    }
            //}

            //return sum;
            return orderItemList
                     .Where(item => item != null)
                     .Sum(item => item!.Value.Price * item.Value.Amount);

        }
        else
        {
            throw new BO.GetDulNullException("order not exists,can not get all orderItems") { GetDulNull = id.ToString() };
        }

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public List<BO.OrderItem?> GetAllItemsToOrder(int id)
    {
        List<DO.OrderItem?> orderItemList = new List<DO.OrderItem?>();
        //List<BO.OrderItem?> BOorderItemList = new List<BO.OrderItem?>();
        if (Dal != null)
        {
            orderItemList = (List<DO.OrderItem?>)Dal.OrderItem.GetAll(e => e?.OrderID == id);
        }
        int count = 1;
        //foreach (var item in orderItemList)
        //{
        //    BOorderItemList.Add(new BO.OrderItem()
        //    {
        //        numInOrder = count++,
        //        ID = item.ID,
        //        Name = getOrderItemName(item.ProductID),
        //        Price = item.Price,
        //        Amount = item.Amount,
        //        sumItem = item.Price * item.Amount

        //    });
        //}
        //return BOorderItemList;
        var addOrderItem = orderItemList
            .Where(item => item is not null)
                          .Select(item => (BO.OrderItem?)new BO.OrderItem()
                          {
                              numInOrder = count++,
                              ID = item!.Value.ID,
                              Name = getOrderItemName(item.Value.ProductID),
                              Price = item.Value.Price,
                              Amount = item.Value.Amount,
                              sumItem = item.Value.Price * item.Value.Amount

                          }).ToList();
        return addOrderItem;

    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public string? getOrderItemName(int productId)
    {
        DO.Product product = new DO.Product();
        if (Dal != null)
        {
            product = Dal.Product.Get(e => e?.ID == productId);
        }
        return product.Name;
    }


    #endregion
}
