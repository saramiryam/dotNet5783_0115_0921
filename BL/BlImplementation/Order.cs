﻿using BlApi;
using BO;
using DalApi;
using DO;
using static BO.Enums;
using Factory = DalApi.Factory;

namespace BlImplementation;

public class Order : BlApi.IOrder
{
    private static IDal? Dal = Factory.Get();
    #region method


    public IEnumerable<BO.OrderForList?> GetListOfOrders()
    {
        IEnumerable<DO.Order?> orderList = new List<DO.Order?>();
        if (Dal != null)
        {
            orderList = Dal.Order.GetAll();
        }

        IEnumerable<OrderTracking> orderTracking = new List<OrderTracking>();
        //foreach (var item in orderList)
        //{
        //    if (item != null)
        //    {
        //        ordersForList.Add(new BO.OrderForList()
        //        {
        //            OrderID = item.Value.ID,
        //            CustomerName = item.Value.CustomerName,
        //            Status = CheckStatus(item.Value.OrderDate, item.Value.ShipDate, item.Value.DeliveryDate),
        //            AmountOfItem = GetAmountItems(item.Value.ID),
        //            TotalSum = CheckTotalSum(item.Value.ID)
        //        });
        //    }

        //}


        var addOrder = orderList
                       .Where(item => item != null)
                       .Select(item => new BO.OrderForList()
                       {
                           OrderID = item.Value.ID,
                           CustomerName = item.Value.CustomerName,
                           Status = CheckStatus(item.Value.OrderDate, item.Value.ShipDate, item.Value.DeliveryDate),
                           AmountOfItem = GetAmountItems(item.Value.ID),
                           TotalSum = CheckTotalSum(item.Value.ID)
                       });
        return addOrder.ToList();
    }

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

    #endregion
    #region bonus
    public void ManagerActions(int orderId, int productId, int amount)
    {
        List<DO.OrderItem> ordersItem = new List<DO.OrderItem>();
        if (Dal != null)
        {
            ordersItem = (List<DO.OrderItem>)Dal.OrderItem.GetAll(e => e?.OrderID == orderId);
        }

        bool flag = ordersItem.Exists(e => e.ProductID == productId && e.OrderID == orderId);
        if (flag)
        {
            //find
            DO.OrderItem OI = ordersItem.Find(e => e.ProductID == productId && e.OrderID == orderId);
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
    public List<BO.OrderItem?> GetAllItemsToOrder(int id)
    {
        IEnumerable<DO.OrderItem> orderItemList = new List<DO.OrderItem>();
        List<BO.OrderItem?> BOorderItemList = new List<BO.OrderItem?>();
        if (Dal != null)
        {
            orderItemList = (IEnumerable<DO.OrderItem>)Dal.OrderItem.GetAll(e => e?.OrderID == id);
        }
        int count = 0;
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
                          .Select(item => BOorderItemList.Add(new BO.OrderItem()
                          {
                              numInOrder = count++,
                              ID = item.ID,
                              Name = getOrderItemName(item.ProductID),
                              Price = item.Price,
                              Amount = item.Amount,
                              sumItem = item.Price * item.Amount

                          }));
        return addOrderItem;
    
    }
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
