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
        BO.OrderTracking orderTracking = new BO.OrderTracking();
        orderTracking.ID = orderId;
        orderTracking.Status = CheckStatus(o.OrderDate, o.ShipDate, o.DeliveryDate);
        switch (orderTracking.Status)
        {
            case EStatus.Done:
                orderTracking.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Status = BO.Enums.EStatus.Done
                });
                break;
            case EStatus.Sent:
                orderTracking.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Status = BO.Enums.EStatus.Done
                });
                orderTracking.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.ShipDate,
                    Status = BO.Enums.EStatus.Sent

                });
                break;
            case EStatus.Provided:
                orderTracking.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.OrderDate,
                    Status = BO.Enums.EStatus.Done
                });
                orderTracking.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.ShipDate,
                    Status = BO.Enums.EStatus.Sent

                }); orderTracking.listOfStatus.Add(new BO.OrderTracking.StatusAndDate()
                {
                    Date = o.DeliveryDate,
                    Status = BO.Enums.EStatus.Provided

                });
                break;
        }
        return orderTracking;

    }

    #endregion
    #region bonus
    //public void ManagerActions(int orderId,int productId,int amount)
    //{
    //    if (amount==0)
    //    {
    //        List<DO.OrderItem> ordersItem = new List<DO.OrderItem>();

    //        ordersItem = (List<DO.OrderItem>)Dal.OrderItem.getAllMyOrdesItem(orderId);
    //        ordersItem.
    //    }
    //}
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
        if (today.Equals(OrderDate) && today.Equals(ShipDate) && today.Equals(DeliveryDate))
            return EStatus.Provided;
        else if (today.Equals(OrderDate) && today.Equals(ShipDate))
            return EStatus.Sent;
        else
            return EStatus.Done;
    }
    public int GetAmountItems(int id)
    {
        IEnumerable<DO.OrderItem> orderItemList = new List<DO.OrderItem>();
        orderItemList = Dal.OrderItem.getAllMyOrdesItem(id);
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
