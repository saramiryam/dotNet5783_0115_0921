﻿
using System;

namespace DO;

public struct Order
{
    //אני לא יודעת איך ניגשים לכונפיג אבל בטוח אפשר...
    //אני באמצע לבדוק את זה אבל לבינתיים זה הבנאי שאמור להיות לדעתי
    //order o=new order("hhh","jjj","kjjjh")  במיין צריך לעשות 
    //וזה אמור לעבוד טוב
    private Order(string newCustomerName, string newCustomerEmail, string newCustomerAdress)
    { ID = 0; CustomerName = newCustomerName;CustomerEmail = newCustomerEmail;CustomerAdress = newCustomerAdress;OrderDate = DateTime.Now;ShipDate = DateTime.MinValue;DeliveryDate = DateTime.MinValue; }
    public int ID { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAdress { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime DeliveryDate { get; set; }

    public override string ToString() => $@"
    Order ID={ID}: {CustomerName}, 
    Email - {CustomerEmail}
    Adress: {CustomerAdress}
    Order Date: {OrderDate}
    Ship Date: {ShipDate}
    Delivery Date: {DeliveryDate}
";

}
