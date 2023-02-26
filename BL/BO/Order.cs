using System.Collections.Generic;
using static BO.Enums;

namespace BO;
//לניהול ולקוחות
public class Order
{

    #region order properties

    public int ID { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerAdress { get; set; }
    public EStatus? Status { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ShipDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public List<OrderItem?>? ItemList { get; set; }
    public double TotalSum { get; set; }


    #endregion


    //bonus
    #region ToString

    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the Order struct</returns>
    public override string ToString()
    {
        string itemsList = "";
        foreach (OrderItem item in ItemList)
        {
            itemsList += (item.ToString());
        }
        return (
       $@"
        Order ID:{ID},
        customer name: {CustomerName},
        customer email: {CustomerEmail},
        customer address: {CustomerAdress},
        order status: {Status},
        orderDate: {OrderDate}
        ship Date:{ShipDate},
        delivery Date:{DeliveryDate},
        Items List:{itemsList},
        total Sum: {TotalSum}");



    }
}

#endregion
