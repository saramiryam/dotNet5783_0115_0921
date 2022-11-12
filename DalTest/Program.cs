// See https://aka.ms/new-console-template for more information
using DO;
using System.Linq.Expressions;
using DalList;
using Dal;

using System.Net.Http.Headers;
using System.Data.Common;

namespace DalTest
{
    public class Programm
    {
        static private Product product = new Product();
        static private Order order = new Order();
        static private OrderItem orderItem = new OrderItem();

        public static void Main()
        {
            DataSource.startProgram();
            int choice;
            Console.WriteLine("Enter a number 1-3 or 0 to exit:")
            int.TryParse(Console.ReadLine(), out choice);

            while (choice != 0)
            {

                switch (choice)
                {
                    case 1://product
                        productMethod();
                        break;

                    case 2://order
                        orderMethod();
                        break;

                    case 3://order item
                        orderItemMethod();
                        break;
                }
            }
        }



        static void productMethod()
        {

            Console.WriteLine("Enter your choice " +
                "1-add,   " +
                "2-get one product,  " +
                "3-get all products,   " +
                "4-delete," +
                "5-update:");
            int choiceForProduct;
            DalProduct p = new Dal.DalProduct();
            int parse;
            double parse2;
            int.TryParse(Console.ReadLine(), out parse);
            choiceForProduct = parse;

            while (choiceForProduct != 0)
            {

                switch (choiceForProduct)
                {

                    case 1://add
                        Console.WriteLine("Enter product details:");
                        Console.WriteLine("Name:");
                        product.Name = Console.ReadLine();
                        Console.WriteLine("Price:");
                        int.TryParse(Console.ReadLine(), out parse);
                        product.Price = parse;
                        Console.WriteLine("type 0 for category Notebooks, 1 - Pens, 2 - Diaries, 3 - ArtMaterials, 4-Games");
                        int category = int.Parse(Console.ReadLine());
                        switch (category)
                        {
                            case 0:
                                product.Category = Enums.ECategory.Notebooks;
                                break;
                            case 1:
                                product.Category = Enums.ECategory.Pens;
                                break;
                            case 2:
                                product.Category = Enums.ECategory.Diaries;
                                break;
                            case 3:
                                product.Category = Enums.ECategory.ArtMaterials;
                                break;
                            case 4:
                                product.Category = Enums.ECategory.Games;
                                break;

                        }
                        Console.WriteLine("Amount in stock:");
                        int.TryParse(Console.ReadLine(), out parse);
                        product.InStock = parse;
                        try
                        {
                            p.addProduct(product);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;




                    case 2:
                        {
                            Console.WriteLine("Enter an Id of product:");
                            //צריך פו את זה 
                            int.TryParse(Console.ReadLine(), out parse);
                            product.ID = parse;

                            try
                            {
                                Console.WriteLine(p.getSingleProduct(product.ID));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            break;
                        }
                    case 3:
                        foreach (Product myProduct in p.getAllProducts())
                        {
                            Console.WriteLine(myProduct);//אולי צריך את toString??
                        }
                        break;
                    case 4://delete
                        Console.WriteLine("Enter an Id of product:");
                        int id = int.Parse(Console.ReadLine());
                        try
                        {
                            p.deleteProduct(id);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    case 5://update
                        {

                            Console.WriteLine("Enter an Id of product:");
                            int Id = int.Parse(Console.ReadLine());
                            string name;
                            double price;
                            int amountInStock;

                            try
                            {
                                product = p.getSingleProduct(Id);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                            Console.WriteLine("the product to update is" + product);
                            Console.WriteLine("Enter the new details of the product:");
                            //if (Console.ReadLine() != "")
                            //{
                            Console.WriteLine("name:");
                            name = Console.ReadLine();
                            Console.WriteLine("price:");
                            double.TryParse(Console.ReadLine(), out parse2);
                            price = parse2;
                            Console.WriteLine("amount");
                            int.TryParse(Console.ReadLine(), out parse);
                            amountInStock = parse;
                            Product proTOUpdata = new Product() { ID = Id, Name = name, Price = price, InStock = amountInStock };
                            p.updateProduct(proTOUpdata);
                            //}
                            break;
                        }


                }

            }
        }
        static void orderMethod()
        {
            int parse;
            Console.WriteLine("Enter your choice:" +
                "1-add,   " +
                "2-get one product,  " +
                "3-get all products,   " +
                "4-delete," +
                "5-update:");
            int choiceForOrder;
            int.TryParse(Console.ReadLine(), out parse);
            choiceForOrder = parse;

            DalOrder o = new Dal.DalOrder();
            switch (choiceForOrder)
            {
                case 1://add
                    DateTime date;
                    Console.WriteLine("Enter order details:");
                   // int.TryParse(Console.ReadLine(), out parse);
                    order.CustomerName = Console.ReadLine();
                    order.CustomerEmail = Console.ReadLine();
                    order.CustomerAdress = Console.ReadLine();
                    //DateTime.TryParse(Console.ReadLine(), out date);
                    //order._orderDate = date;
                    //DateTime.TryParse(Console.ReadLine(), out date);
                    //order._shippingDate = date;
                    //DateTime.TryParse(Console.ReadLine(), out date);
                    //order._deliveryDate = date;
                    o.addOrder(order);
                    break;

                case 2:
                    Console.WriteLine("Enter an Id of Order:");
                    int Id = int.Parse(Console.ReadLine());
                    try
                    {
                        Console.WriteLine(o.getSingleOrder(Id));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 3:
                    foreach (Order myOrder in o.getAllOrder())
                    {
                        Console.WriteLine(myOrder);
                    }
                    break;

                case 4://delete
                    Console.WriteLine("Enter an Id of order:");
                    int IdToDelete = int.Parse(Console.ReadLine());
                    try
                    {
                        o.deleteOrder(IdToDelete);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 5://update
                    Console.WriteLine("Enter an Id of order:");
                    int idToUpdate = int.Parse(Console.ReadLine());
                    try
                    {
                        order = o.getSingleOrder(idToUpdate);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    Order orderToUpdate = new Order();
                    orderToUpdate.ID = idToUpdate;
                    Console.WriteLine("the order to update is" + order);
                    Console.WriteLine("Enter the new details of the order:");
                    Console.WriteLine("customer name:");
                   orderToUpdate.CustomerName = Console.ReadLine();
                    Console.WriteLine("email:");
                    orderToUpdate.CustomerEmail = Console.ReadLine();
                    Console.WriteLine("address:");
                    orderToUpdate.CustomerAdress = Console.ReadLine();
                    DateTime.TryParse(Console.ReadLine(), out date);
                    orderToUpdate.OrderDate = date;
                    DateTime.TryParse(Console.ReadLine(), out date);
                    orderToUpdate.ShipDate = date;
                    DateTime.TryParse(Console.ReadLine(), out date);
                    orderToUpdate.DeliveryDate = date;
                    o.updateOrder(orderToUpdate);

                    break;
            }

        }


        static void orderItemMethod()
        {
            Console.WriteLine("Enter your choice:" +
           "1-add,   " +
           "2-get one product,  " +
           "3-get all products,   " +
           "4-delete," +
           "5-update,   " +
           "6-see al items in order:");
            int parse;
            float parse3;
            int choiceOrderItem;
            int.TryParse(Console.ReadLine(), out parse);
            choiceOrderItem = parse;
            DalOrderItem OI = new DalOrderItem();
            switch (choiceOrderItem)
            {
                case 1://add
                    Console.WriteLine("Enter order item details:");
                    int.TryParse(Console.ReadLine(), out parse);
                    orderItem.OrderID = parse;
                    int.TryParse(Console.ReadLine(), out parse);
                    orderItem.ProductID = parse;
                    float.TryParse(Console.ReadLine(), out parse3);
                    orderItem.Price = parse3;
                    int.TryParse(Console.ReadLine(), out parse);
                    orderItem.Amount = parse;
                    OI.addOrderItem(orderItem);
                    break;

                case 2:
                    Console.WriteLine("Enter an Id of the order of the order item:");
                    int OrderId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter an Id of the product of the order item:");
                    int ProductId = int.Parse(Console.ReadLine());
                    try
                    {
                        Console.WriteLine(OI.getSingleOrederItem(ProductId,OrderId));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 3:
                    foreach (OrderItem myOrderItem in OI.getAllOrderItems())
                    {
                        Console.WriteLine(myOrderItem);//אולי צריך את toString??
                    }
                    break;

                case 4://delete
                    Console.WriteLine("Enter an Id of the product of the order item:");
                    int ProductIdToDelete = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter an Id of the order of the order item:");
                    int OrderIdToDelete = int.Parse(Console.ReadLine());
                    try
                    {
                        OI.deleteOrderItem(ProductIdToDelete,OrderIdToDelete);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 5://update
                    Console.WriteLine("Enter an Id of the order of the order item:");
                    int OrderIdToUpdate = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter an Id of the product of the order item:");
                    int ProductIdTouodate = int.Parse(Console.ReadLine());
                    try
                    {
                        orderItem = OI.getSingleOrederItem(ProductIdTouodate,OrderIdToUpdate);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    Console.WriteLine("the order item to update is" + orderItem);
                    Console.WriteLine("Enter the new details of the order item:");
                    Console.WriteLine("orderId:");
                    int orderId = int.Parse(Console.ReadLine());
                    orderItem.OrderID = orderId;
                    Console.WriteLine("productId:");
                    int productId = int.Parse(Console.ReadLine());
                    orderItem.ProductID = productId;
                    Console.WriteLine("price:");
                    int pricePerUnit = int.Parse(Console.ReadLine());
                    orderItem.Price = pricePerUnit;
                    Console.WriteLine("amount");
                    int quantity = int.Parse(Console.ReadLine());
                    orderItem.Amount = quantity;
                    OI.updateOrderItem(orderItem);
                    break;

                case 6:
                    Console.WriteLine("Enter Id of an order:");
                    int IdOrder = int.Parse(Console.ReadLine());
                    try
                    {
                        foreach (OrderItem myOrderItem in OI.getAllMyOrdesItem(IdOrder))
                        {
                            if(myOrderItem.OrderID != 0)  
                            Console.WriteLine(myOrderItem);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

            }
        }




    }
}

