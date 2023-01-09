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

    #region ToString

    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the Order struct</returns>
//    public override string ToString() =>
//        $@"
//    Order ID={ID}: {CustomerName}, 
//    Email - {CustomerEmail}
//    Adress: {CustomerAdress}
//    Status of order:{Status}
//    Order Date: {OrderDate}
//    Ship Date: {ShipDate}
//    Delivery Date: {DeliveryDate}
//    List of Item:{ItemList!.ToString()}
//    Total sum:{TotalSum}
//";

    #endregion
    public override string ToString()
    {
        string item = "";
        if (ItemList != null)
        {
            foreach (OrderItem OneItem in ItemList)
            {
                item += OneItem;
            }
        }
        return
    $@" order ID: {ID}
        customer name: {CustomerName}, 
        customer email: {CustomerEmail}
        customerAddress: {CustomerAdress}
        order status: {Status}
        orderDate: {OrderDate}
        shipDate: {ShipDate}
        all order items:{item}
        deliveryDate: {DeliveryDate}
        total order price: {TotalSum}";
    }

}
