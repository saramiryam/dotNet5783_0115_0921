using BlApi;
using BO;
using DalApi;
using DO;
using static BO.Enums;

namespace BlImplementation;

public class Order : BlApi.IOrder
{
    private IDal Dal = new Dal.DalList();
    #region method
    public IEnumerable<BO.OrderForList> GetListOfOrders()
    {
        IEnumerable<DO.Order> orderList = new List<DO.Order>();
        List<BO.OrderForList> ordersForList = new List<BO.OrderForList>();
        orderList = Dal.Order.GetAll();

        IEnumerable<OrderTracking> orderTracking = new List<OrderTracking>();
        foreach (var item in orderList)
        {
            ordersForList.Add(new BO.OrderForList()
            {
                OrderID = item.ID,
                CustomerName = item.CustomerName,
                Status = CheckStatus(item.OrderDate, item.ShipDate, item.DeliveryDate),
                AmountOfItem = GetAmountItems(item.ID),
                TotalSum = CheckTotalSum(item.ID)
            });

        }
        return ordersForList;
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
                o = Dal.Order.Get(id);
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
            o = Dal.Order.Get(id);
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
                    Dal.Order.Update(o);
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
            o = Dal.Order.Get(id);
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
                    Dal.Order.Update(o);
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
            o = Dal.Order.Get(orderId);
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
                var i= new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Status = BO.Enums.EStatus.Done
                };

                orderTracking1.listOfStatus.Add(i);
                break;
            case EStatus.Sent:
                orderTracking1.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Status = BO.Enums.EStatus.Done
                });
                orderTracking1.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.ShipDate,
                    Status = BO.Enums.EStatus.Sent

                });
                break;
            case EStatus.Provided:
               orderTracking1.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Status = BO.Enums.EStatus.Done
                });
                orderTracking1.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.ShipDate,
                    Status = BO.Enums.EStatus.Sent

                }); orderTracking1.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.DeliveryDate,
                    Status = BO.Enums.EStatus.Provided

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
        ordersItem = (List<DO.OrderItem>)Dal.OrderItem.getAllMyOrdesItem(orderId);

        bool flag = ordersItem.Exists(e => e.ProductID == productId && e.OrderID == orderId);
        if (flag)
        {
            //find
            DO.OrderItem OI = ordersItem.Find(e => e.ProductID == productId && e.OrderID == orderId);
            if (amount == 0)
            {
                Dal.OrderItem.Delete(OI.ID);
            }
            else
            {
                DO.Product p = Dal.Product.Get(productId);
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
        else
        {
            //didnt find
            try
            {
                DO.Product p = Dal.Product.Get(productId);
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
    public BO.Enums.EStatus CheckStatus(DateTime OrderDate, DateTime ShipDate, DateTime DeliveryDate)
    {
        DateTime today = DateTime.Now;
        if (today>=OrderDate && today>=ShipDate &&ShipDate!=DateTime.MinValue&& today>=DeliveryDate&&DeliveryDate!=DateTime.MinValue)
            return EStatus.Provided;
        else if (today>=OrderDate && today>=ShipDate&&ShipDate!=DateTime.MinValue)
            return EStatus.Sent;
        else
            return EStatus.Done;
    }
    public int GetAmountItems(int id)
    {
        IEnumerable<DO.OrderItem> orderItemList = new List<DO.OrderItem>();
        try
        {
            orderItemList = Dal.OrderItem.getAllMyOrdesItem(id);
        }
        catch
        {
            throw new BO.OrderNotExistsException("order not exists,can not get all orderItems") { OrderNotExists = id.ToString() };

        }
        int sum = 0;
        foreach (var item in orderItemList)
        {
            sum += item.Amount;
        }
        return sum;

    }
    public double CheckTotalSum(int id)
    {
        IEnumerable<DO.OrderItem> orderItemList = new List<DO.OrderItem>();
        orderItemList = Dal.OrderItem.getAllMyOrdesItem(id);
        double sum = 0;
        foreach (var item in orderItemList)
        {
            sum = sum + item.Price * item.Amount;
        }
        return sum;
    }
    public List<BO.OrderItem> GetAllItemsToOrder(int id)
    {
        IEnumerable<DO.OrderItem> orderItemList = new List<DO.OrderItem>();
        List<BO.OrderItem> BOorderItemList = new List<BO.OrderItem>();
        orderItemList = Dal.OrderItem.getAllMyOrdesItem(id);
        int count = 0;
        foreach (var item in orderItemList)
        {
            BOorderItemList.Add(new BO.OrderItem()
            {
                numInOrder = count++,
                ID = item.ID,
                Name = getOrderItemName(item.ProductID),
                Price = item.Price,
                Amount = item.Amount,
                sumItem = item.Price * item.Amount

            });
        }
        return BOorderItemList;

    }
    public string getOrderItemName(int productId)
    {
        DO.Product product = new DO.Product();
        product = Dal.Product.Get(productId);
        return product.Name;
    }


    #endregion
}
