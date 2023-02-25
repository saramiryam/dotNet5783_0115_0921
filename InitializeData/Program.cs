// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using System.Net;
using DO;
using System.Xml.Linq;

namespace Dal;
public static class Program
{

    #region Class members

    static readonly internal Random _randNum = new Random();
    static internal List<Product?> _Products = new();
    static internal List<Order?> _Orders = new();
    static internal List<OrderItem?> _arrOrderItem = new();

    #endregion
    #region constructors

    public static void Main()
    {
        XmlProduct products = new XmlProduct();
        string dir = @"../xml/";

        if (!File.Exists(dir + @"Orders.xml") && !File.Exists(dir + @"OrderItems.xml") && !File.Exists(dir + @"Product.xml"))
        {
            s_initialize();
         
            FileStream file2 = new FileStream(dir + @"Orders.xml", FileMode.Create);
            XmlSerializer p2 = new XmlSerializer(_Orders.GetType());
            p2.Serialize(file2, _Orders);
            file2.Close();
            XElement rootElemO = new XElement(@"Product.xml");
            foreach (var u in _Products)
                products.Add((DO.Product)u);
            //FileStream file = new FileStream(dir + @"Product.xml", FileMode.Create);
            //XmlSerializer p = new XmlSerializer(_Products.GetType());
            //p.Serialize(file, _Products);
            //file.Close();
            FileStream fileOI = new FileStream(dir + @"OrderItems.xml", FileMode.Create);
            XmlSerializer OI = new XmlSerializer(_arrOrderItem.GetType());
            OI.Serialize(fileOI, _arrOrderItem);
            fileOI.Close();
        }



    }


    #endregion

    #region methods

    /// <summary>
    /// enter the first orders, products and order item to thier arrey
    /// </summary>
    static private void s_initialize()
    {
        #region addNewProduct
        //להוסיף לפונקציה שתקבל סטרינג
        addNewProduct("big_notebook", DO.Enums.ECategory.Notebooks, 6.9, 50);     //10000 
        addNewProduct("small notebook", DO.Enums.ECategory.Notebooks, 4.9, 0);       ///10001
        addNewProduct("Sudoku", DO.Enums.ECategory.Games, 9.5, 41);                  //10010
        addNewProduct("campuse notebook", DO.Enums.ECategory.Notebooks, 5.9, 35);      //10002
        addNewProduct("chanan notebook", DO.Enums.ECategory.Notebooks, 4.9, 63);       //10003
        addNewProduct("pilot", DO.Enums.ECategory.Pens, 9.9, 12);                    //10004
        addNewProduct("stabilo pen", DO.Enums.ECategory.Pens, 5.6, 20);              //10005
        addNewProduct("chanan pen", DO.Enums.ECategory.Pens, 4, 10);                 //1006
        addNewProduct("blue diary", DO.Enums.ECategory.Diaries, 12.5, 38);           //10007         
        addNewProduct("red diary", DO.Enums.ECategory.Diaries, 17, 65);               //10008
        addNewProduct("paintbrush", DO.Enums.ECategory.ArtMaterials, 7, 25);          //10009
        #endregion
        #region addNewOrderItem
        //  addNewOrder()
        addNewOrderItem(100010, 200000, 6.9, 3);
        addNewOrderItem(100002, 200001, 5.9, 1);
        addNewOrderItem(100009, 200001, 7, 7);
        addNewOrderItem(100008, 200001, 9.5, 2);

        addNewOrderItem(100010, 200002, 6.9, 3);
        addNewOrderItem(100002, 200002, 5.9, 1);
        addNewOrderItem(100003, 200002, 4.9, 1);
        addNewOrderItem(100004, 200002, 9.9, 2);

        addNewOrderItem(100007, 200003, 6.9, 3);
        addNewOrderItem(100002, 200003, 5.9, 1);
        addNewOrderItem(100005, 200003, 5.6, 3);
        addNewOrderItem(100008, 200003, 17, 2);

        addNewOrderItem(100010, 200004, 6.9, 3);
        addNewOrderItem(100002, 200000, 5.9, 1);
        addNewOrderItem(100009, 200004, 7, 7);
        addNewOrderItem(100008, 200004, 9.5, 2);

        addNewOrderItem(100010, 200005, 6.9, 3);
        addNewOrderItem(100002, 200005, 5.9, 1);
        addNewOrderItem(100003, 200005, 4.9, 1);
        addNewOrderItem(100004, 200005, 9.9, 2);

        addNewOrderItem(100007, 200006, 6.9, 3);
        addNewOrderItem(100002, 200006, 5.9, 1);
        addNewOrderItem(100005, 200006, 5.6, 3);
        addNewOrderItem(100008, 200006, 17, 2);

        addNewOrderItem(100010, 200007, 6.9, 3);
        addNewOrderItem(100002, 200007, 5.9, 1);
        addNewOrderItem(100009, 200007, 7, 7);
        addNewOrderItem(100008, 200011, 9.5, 2);

        addNewOrderItem(100010, 200008, 6.9, 3);
        addNewOrderItem(100002, 200012, 5.9, 1);
        addNewOrderItem(100003, 200013, 4.9, 1);
        addNewOrderItem(100004, 200014, 9.9, 2);

        addNewOrderItem(100007, 200009, 6.9, 3);
        addNewOrderItem(100002, 200015, 5.9, 1);
        addNewOrderItem(100005, 200016, 5.6, 3);
        addNewOrderItem(100008, 200017, 17, 2);

        addNewOrderItem(100010, 200010, 6.9, 3);
        addNewOrderItem(100002, 200018, 5.9, 1);
        addNewOrderItem(100009, 200019, 7, 7);
        addNewOrderItem(100008, 200020, 9.5, 2);

        #endregion
        #region addNewOrder

        Random rnd = new Random();
        addNewOrder("Moshe Cohen", "david@gmail.com", "buksboim 12");
        addNewOrder("Sara Miriam ", "SaraMiriam@gmail.com", "הפסגה 50");
        addNewOrder("Rut", "Rut@gmail.com", "aaa 12", DateTime.Now.AddDays(-(rnd.Next(30))), DateTime.Now.AddDays(-(rnd.Next(9))), null);
        addNewOrder("David Levi", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(9))), null, null);
        addNewOrder("raviv", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(20))), DateTime.Now, null);
        addNewOrder("Moyshi", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(5))), DateTime.Now.AddDays(-(rnd.Next(6))), DateTime.Now.AddDays(-(rnd.Next(4))));
        addNewOrder("Shani", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(1))), DateTime.Now, null);
        addNewOrder("Dasi", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(9))), DateTime.Now.AddDays(-(rnd.Next(1))), null);
        addNewOrder("Noa", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(7))), null, null);
        addNewOrder("shira", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(10))), DateTime.Now.AddDays(-(rnd.Next(5))), DateTime.Now.AddDays(-(rnd.Next(3))));
        addNewOrder("Yosi", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(9))), DateTime.Now, null);
        addNewOrder("Mashuda", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(9))), null, null);
        addNewOrder("Gila", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(11))), DateTime.Now, null);
        addNewOrder("Chani", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(13))), null,null);
        addNewOrder("Yona", "Yona@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(13))), null, null);
        addNewOrder("Nadav", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(9))), DateTime.Now, null);
        addNewOrder("Pnina", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(5))), DateTime.Now.AddDays(-(rnd.Next(13))), null);
        addNewOrder("Ayala rov", "Ayala@gmail.com", "חיים ויטאל");
        addNewOrder("David Levi", "david@gmail.com", "buksboim 12", DateTime.Now.AddDays(-(rnd.Next(18))), DateTime.Now.AddDays(-(rnd.Next(3))), DateTime.Now.AddDays(-(rnd.Next(5))));

        #endregion

    }



    /// <summary>
    /// start program for enable enter new items
    /// </summary>
    static public void startProgram()
    {
        return;
    }

    /// <summary>
    /// get information of a product for update the products arrey
    /// </summary>
    /// <param name="newName">string - name of product</param>
    /// <param name="newCategory">enum ECategory - name of category</param>
    /// <param name="newPrice">double - price of product</param>
    /// <param name="newInStock">int - amount of product in stock</param>
    static private void addNewProduct(string newName, DO.Enums.ECategory newCategory, double newPrice, int newInStock)
    {
        Product newProducts = new() { ID = XmlConfig.getProductId(), Name = newName, Price = newPrice, Category = newCategory, InStock = newInStock };
        _Products.Add(newProducts);
    }

    /// <summary>
    /// get information of a order for update the orders arrey
    /// </summary>
    /// <param name="newCustomerName">string - name of inviter</param>
    /// <param name="newCustomerEmail">string - email of inviter</param>
    /// <param name="newCustomerAdress">string - adress of inviter</param>
    /// <param name="NewOrderDate">DateTime - date of order</param>
    /// <param name="newShipDate">DateTime - date of ship</param>
    /// <param name="newDeliveryDate">DateTime - date of delivery</param>
    static private void addNewOrder(string newCustomerName, string newCustomerEmail, string newCustomerAdress, DateTime NewOrderDate, DateTime? newShipDate, DateTime? newDeliveryDate)
    {
        Order newOrder = new()
        {
            ID = XmlConfig.getOrderId(),
            CustomerName = newCustomerName,
            CustomerEmail = newCustomerEmail,
            CustomerAdress = newCustomerAdress,
            OrderDate = NewOrderDate,
            ShipDate = newShipDate,
            DeliveryDate = newDeliveryDate
        };
        _Orders.Add(newOrder);
    }

    /// <summary>
    /// get information of a programmer order for update the orders arrey, with drawn dates
    /// </summary>
    /// <param name="newCustomerName">string - name of inviter</param>
    /// <param name="newCustomerEmail">string - email of inviter</param>
    /// <param name="newCustomerAdress">string - adress of inviter</param>
    static private void addNewOrder(string newCustomerName, string newCustomerEmail, string newCustomerAdress)
    {
        DateTime _today = DateTime.Now;
        int daysAgo = new Random().Next(600);
        DateTime NewOrderDate = _today.AddDays(-daysAgo);
        int daysbetweenOrderToShip = new Random().Next(10);
        DateTime newShipDate = NewOrderDate.AddDays(daysbetweenOrderToShip);
        int daysbetweenDeliveryToShip = new Random().Next(7);
        DateTime newDeliveryDate = newShipDate.AddDays(daysbetweenDeliveryToShip);
        addNewOrder(newCustomerName, newCustomerEmail, newCustomerAdress, NewOrderDate, newShipDate, newDeliveryDate);


    }

    /// <summary>
    /// get information of a order item for update the orderItems arrey
    /// </summary>
    /// <param name="newProductID">int - id of product</param>
    /// <param name="newOrderID">int - id of order</param>
    /// <param name="newPrice">double1 - price of items</param>
    /// <param name="newAmount">string - amount of items</param>
    static private void addNewOrderItem(int newProductID, int newOrderID, double newPrice, int newAmount)
    {
        OrderItem item = new OrderItem() { ID = XmlConfig.getOrderItemId(), ProductID = newProductID, OrderID = newOrderID, Price = newPrice, Amount = newAmount };
        _arrOrderItem.Add(item);
    }

}

#endregion
    