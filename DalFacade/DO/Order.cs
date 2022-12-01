
using System;

namespace DO;

public struct Order
{
    #region order properties

    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAdress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryDate { get; set; }

    #endregion


    #region constructor

    /// <summary>
    /// a constructor how creat a new order
    /// </summary>
    /// <param name="newCustomerName">string of a name</param>
    /// <param name="newCustomerEmail"> string of an email</param>
    /// <param name="newCustomerAdress">string of an adress</param>
    private Order(string newCustomerName, string newCustomerEmail, string newCustomerAdress)
    {
        ID = 0;
        CustomerName = newCustomerName;
        CustomerEmail = newCustomerEmail;
        CustomerAdress = newCustomerAdress;
        OrderDate = DateTime.Now;
        ShipDate = DateTime.MinValue;
        DeliveryDate = DateTime.MinValue;
    }

    #endregion


    #region methods

    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the Order struct</returns>
    public override string ToString() => $@"
    Order ID={ID}: {CustomerName}, 
    Email - {CustomerEmail}
    Adress: {CustomerAdress}
    Order Date: {OrderDate}
    Ship Date: {ShipDate}
    Delivery Date: {DeliveryDate}
";


    #endregion

}
