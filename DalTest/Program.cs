// See https://aka.ms/new-console-template for more information
using DO;
using Dal;
using DalApi;

namespace DalTest
{
    public class Programm
    {
         static IDal IDalVariable = new Dal.DalList();

        static private Product product = new Product();
        static private Order order = new Order();
        static private OrderItem orderItem = new OrderItem();

        public static void Main()
        {

            DataSource.startProgram();
            int choice;
            Console.WriteLine("Enter a number 1-3 or 0 to exit:  " +
                "1-to product  " +
                "2-to order  " +
                "3-to order item  ");
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


        /// <summary>
        /// all the options about product
        /// 1-add,2-get one product, 3-get all products,4-delete,5-update
        /// </summary>
        static void productMethod()
        {
            Console.WriteLine("Enter your choice " +
            "1-add,   " +
            "2-get one product,  " +
            "3-get all products,   " +
            "4-delete,  " +
            "5-update:  ");
            int choiceForProduct;
            int parse;
            double parse2;
            int.TryParse(Console.ReadLine(), out parse);
            choiceForProduct = parse;
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
                        // p.Add(product);
                        IDalVariable.Product.Add(product);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;




                case 2:
                    {
                        Console.WriteLine("Enter an Id of product:");
                        int.TryParse(Console.ReadLine(), out parse);
                        product.ID = parse;

                        try
                        {
                            Console.WriteLine(IDalVariable.Product.Get(p=>p?.ID==product.ID));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }
                case 3:
                    foreach (Product myProduct in IDalVariable.Product.GetAll())
                    {
                        Console.WriteLine(myProduct);//אולי צריך את toString??
                    }
                    break;
                case 4://delete
                    Console.WriteLine("Enter an Id of product:");
                    int id = int.Parse(Console.ReadLine());
                    try
                    {
                        IDalVariable.Product.Delete(id);
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
                            product = IDalVariable.Product.Get(p => p?.ID == Id);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        Console.WriteLine("the product to update is" + product);
                        Console.WriteLine("Enter the new details of the product:");
                        Console.WriteLine("name:");
                        name = Console.ReadLine();
                        Console.WriteLine("price:");
                        double.TryParse(Console.ReadLine(), out parse2);
                        price = parse2;
                        Console.WriteLine("amount");
                        int.TryParse(Console.ReadLine(), out parse);
                        amountInStock = parse;
                        Product proTOUpdata = new Product() { ID = Id, Name = name, Price = price, InStock = amountInStock };
                        IDalVariable.Product.Update(proTOUpdata);
                        //}
                        break;
                    }




            }
        }
        /// <summary>
        ///  all the options about order
        /// 1-add,2-get one product, 3-get all products,4-delete,5-update 
        /// </summary>
        static void orderMethod()
        {
            int parse;
            Console.WriteLine("Enter your choice:" +
                "1-add,   " +
                "2-get one order,  " +
                "3-get all orders,   " +
                "4-delete,  " +
                "5-update:  ");
            int choiceForOrder;
            int.TryParse(Console.ReadLine(), out parse);
            choiceForOrder = parse;


            switch (choiceForOrder)
            {
                case 1://add
                    DateTime date;
                    Console.WriteLine("Enter order details:");
                    Console.WriteLine("name:");
                    order.CustomerName = Console.ReadLine();
                    Console.WriteLine("Email:");
                    order.CustomerEmail = Console.ReadLine();
                    Console.WriteLine("Address:");
                    order.CustomerAdress = Console.ReadLine();
                    IDalVariable.Order.Add(order);
                    break;

                case 2:
                    Console.WriteLine("Enter an Id of Order:");
                    int Id = int.Parse(Console.ReadLine());
                    try
                    {
                        Console.WriteLine(IDalVariable.Order.Get(p => p?.ID == Id));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 3:
                    foreach (Order myOrder in IDalVariable.Order.GetAll())
                    {
                        Console.WriteLine(myOrder);
                    }
                    break;

                case 4://delete
                    Console.WriteLine("Enter an Id of order:");
                    int IdToDelete = int.Parse(Console.ReadLine());
                    try
                    {
                        IDalVariable.Order.Delete(IdToDelete);
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
                        order = IDalVariable.Order.Get(p => p?.ID == idToUpdate);
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
                    IDalVariable.Order.Update(orderToUpdate);

                    break;
            }

        }

        /// <summary>
        /// 
        ///  all the options about orderItem
        /// 1-add,2-get one product, 3-get all products,4-delete,5-update
        /// </summary>
        static void orderItemMethod()
        {
            Console.WriteLine("Enter your choice:" +
           "1-add,   " +
           "2-get one orderItem,  " +
           "3-get all orderItem   " +
           "4-delete, " +
           "5-update,   " +
           "6-see all items in order  " +
           "7-get one orderItem by orderId and productId  :");
            int parse;
            float parse3;
            int choiceOrderItem;
            int.TryParse(Console.ReadLine(), out parse);
            choiceOrderItem = parse;
            

            switch (choiceOrderItem)
            {
                case 1://add
                    Console.WriteLine("Enter order item details:");
                    Console.WriteLine("OrderID:");
                    int.TryParse(Console.ReadLine(), out parse);
                    orderItem.OrderID = parse;
                    Console.WriteLine("ProductID:");
                    int.TryParse(Console.ReadLine(), out parse);
                    orderItem.ProductID = parse;
                    Console.WriteLine("Price:");
                    float.TryParse(Console.ReadLine(), out parse3);
                    orderItem.Price = parse3;
                    int.TryParse(Console.ReadLine(), out parse);
                    Console.WriteLine("Amount:");
                    orderItem.Amount = parse;
                    IDalVariable.OrderItem.Add(orderItem);
                    break;

                case 2:
                    Console.WriteLine("Enter an Id of the order item:");
                    int orderItemID = int.Parse(Console.ReadLine());
                    try
                    {
                        Console.WriteLine(IDalVariable.OrderItem.Get(p => p?.ID == orderItemID));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 3:
                    foreach (OrderItem myOrderItem in IDalVariable.OrderItem.GetAll())
                    {
                        Console.WriteLine(myOrderItem);
                    }
                    break;

                case 4://delete
                    Console.WriteLine("Enter an Id of the order item:");
                    int orderItemID1 = int.Parse(Console.ReadLine());
                    try
                    {
                        IDalVariable.OrderItem.Delete(orderItemID1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 5://update
                    Console.WriteLine("Enter an Id of the order item:");
                    int orderItemID2 = int.Parse(Console.ReadLine());
                    try
                    {
                        orderItem = IDalVariable.OrderItem.Get(p => p?.ID == orderItemID2);
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
                    IDalVariable.OrderItem.Update(orderItem);
                    break;

                case 6:
                    Console.WriteLine("Enter Id of an order:");
                    int IdOrder = int.Parse(Console.ReadLine());
                    try
                    {
                        foreach (OrderItem myOrderItem in IDalVariable.OrderItem.GetAll(e => e?.OrderID == IdOrder))
                        {
                            if (myOrderItem.OrderID != 0)
                                Console.WriteLine(myOrderItem);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 7:
                    Console.WriteLine("Enter an Id the order of the order item:");
                    int orderID = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter an Id of the product the order item:");
                    int productID = int.Parse(Console.ReadLine());
                    try
                    {
                        Console.WriteLine(IDalVariable.OrderItem.Get(e => e.HasValue && e.Value.OrderID == orderID && e.Value.ProductID == productID));
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

