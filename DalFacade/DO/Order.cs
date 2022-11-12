
using System;

namespace DO;

public struct Order
{
    //אני לא יודעת איך ניגשים לכונפיג אבל בטוח אפשר...
    //אני באמצע לבדוק את זה אבל לבינתיים זה הבנאי שאמור להיות לדעתי
    //order o=new order("hhh","jjj","kjjjh")  במיין צריך לעשות 
    //וזה אמור לעבוד טוב

    #region order properties

    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAdress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryDate { get; set; }

    #endregion


    #region methods
    /// <summary>
    /// a constractor how creat a new order
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

    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the struct</returns>
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
