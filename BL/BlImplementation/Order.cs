using BlApi;
using BO;
using DalApi;
using static BO.Enums;

namespace BlImplementation;

public class Order: BlApi.IOrder
{
    private IDal Dal = new Dal.DalList();
    #region method
    public List<BO.OrderForList> GetListOfOrders()
    {
    //        public string CustomerName { get; set; }
    //public EStatus Status { get; set; }
    //public int AmountOfItem { get; set; }
    //public double TotalSum { get; set; }

        //  public List<OrderItem> ItemList { get; set; }
       //   public double TotalSum { get; set; }

        IEnumerable<DO.Order> orderList = new List<DO.Order>();
        List<BO.OrderForList> ordersForList = new List<BO.OrderForList>();
        orderList = Dal.Order.GetAll();
    
        IEnumerable< OrderTracking> orderTracking = new List<OrderTracking>();  
        foreach (var item in orderList)
        {
            ordersForList.Add(new BO.OrderForList()
            {
                OrderID=item.ID,
                CustomerName=item.CustomerName,
                Status=CheckStatus(item.OrderDate, item.ShipDate, item.DeliveryDate),
                AmountOfItem=GetAmountItems(item.ID),
                TotalSum=CheckTotalSum(item.ID)
            });

        }
        return ordersForList;
    }
    public BO.Order getorderDetails(int id)
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
            catch
            {
                throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };

            }
            BO.Order newOrder = new BO.Order()
            {
                ID=o.ID,
                CustomerName=o.CustomerName,
                CustomerEmail=o.CustomerEmail,
                CustomerAdress=o.CustomerAdress,
                Status=CheckStatus(o.OrderDate, o.ShipDate, o.DeliveryDate),
                OrderDate=o.OrderDate,  
                ShipDate=o.ShipDate,
                DeliveryDate=o.DeliveryDate,
                ItemList=GetAllItemToOrder(o.ID),
                TotalSum =CheckTotalSum(o.ID)


            };

        }
    }
    #endregion


    #region ezer
    public BO.Enums.EStatus CheckStatus(DateTime OrderDate, DateTime ShipDate, DateTime DeliveryDate)
    {
        DateTime today=DateTime.Now;
        if (today.Equals(OrderDate) && today.Equals(ShipDate) && today.Equals(DeliveryDate))
            return EStatus.Done;
       else if (today.Equals(OrderDate) && today.Equals(ShipDate))
            return EStatus.Sent; 
        else
            return EStatus.Provided;
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
        double sum=0;
        foreach (var item in orderItemList)
        {
            sum=sum+item.Price*item.Amount;
        }
        return sum; 
    }
    public List<BO.OrderItem> GetAllItemToOrder(int id)
    {
        IEnumerable<DO.OrderItem> orderItemList = new List<DO.OrderItem>();
        List<BO.OrderItem> BOorderItemList = new List<BO.OrderItem>();
        orderItemList = Dal.OrderItem.getAllMyOrdesItem(id);
        int count=0;
        foreach (var item in orderItemList)
        {
            BOorderItemList.Add(new BO.OrderItem()
            {
                numInOrder = count++,
                ID = item.ID,
                //name
                Price = item.Price,
                Amount = item.Amount,
                sumItem = item.Price * item.Amount

            });
        }
        return BOorderItemList;

    }


    #endregion
}
