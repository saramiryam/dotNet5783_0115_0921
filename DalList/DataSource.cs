using DO;
using static DO.Enums;

namespace Dal;
internal static class DataSource
{
    static readonly internal Random _randNum = new Random();
    static internal Product[] _arrProduct = new Product[50];
    static internal Order[] _arrOrder = new Order[100];
    static internal OrderItem[] _arrOrderItem = new OrderItem[200];
    static DataSource()
    {
        s_initialize();
    }

    static private void addNewProduct(string newName, ECategory newCategory, double newPrice, int newInStock)
    {
        _arrProduct[Config._productIndex].ID = Config.CalNumOfProduct;
        _arrProduct[Config._productIndex].Name = newName;
        _arrProduct[Config._productIndex].Category = newCategory;
        _arrProduct[Config._productIndex].InStock = newInStock;
        Config._productIndex++;
    }
    static private void addNewOrder(string newCustomerName, string newCustomerEmail, string newCustomerAdress, DateTime newShipDate, DateTime newDeliveryDate)
    {
        _arrOrder[Config._orderIndex].ID = Config.CalNumOfIDOrder;
        _arrOrder[Config._orderIndex].CustomerName = newCustomerName;
        _arrOrder[Config._orderIndex].CustomerEmail = newCustomerEmail;
        _arrOrder[Config._orderIndex].CustomerAdress = newCustomerAdress;
        _arrOrder[Config._orderIndex].OrderDate = DateTime.Now;
        _arrOrder[Config._orderIndex].ShipDate = newShipDate;
        _arrOrder[Config._orderIndex].DeliveryDate = newDeliveryDate;
        Config._orderIndex++;
    }
    static private void addNewOrderItem(int newProductID, int newOrderID, double newPrice, int newAmount)
    {
        _arrOrderItem[Config._orderItemIndex].ProductID = newProductID;
        _arrOrderItem[Config._orderItemIndex].OrderID = newOrderID;
        _arrOrderItem[Config._orderItemIndex].Price = newPrice;
        _arrOrderItem[Config._orderItemIndex].Amount = newAmount;
        Config._productIndex++;
    }
    static internal class Config
    {
        static internal int _productIndex = 0;
        static internal int _orderIndex = 0;
        static internal int _orderItemIndex = 0;
        static private int _calNumOfProduct = 100000;
        static public int CalNumOfProduct { get { return _calNumOfProduct++; } }
        static private int _calNumOfIDOrder = 200000;
        static public int CalNumOfIDOrder { get { return _calNumOfIDOrder++; } }



    }
    static private void s_initialize()
    {
        addNewProduct("big_notebook", ECategory.Notebooks, 6.9, 50);     //10000 
        addNewProduct("small notebook", ECategory.Notebooks, 4.9, 0);       ///10001
        addNewProduct("campuse notebook", ECategory.Notebooks, 5.9, 35);      //10002
        addNewProduct("chanan notebook", ECategory.Notebooks, 4.9, 63);       //10003
        addNewProduct("pilot", ECategory.Pens, 9.9, 12);                    //10004
        addNewProduct("stabilo pen", ECategory.Pens, 5.6, 20);              //10005
        addNewProduct("chanan pen", ECategory.Pens, 4, 10);                 //1006
        addNewProduct("blue diary", ECategory.Diaries, 12.5, 38);           //10007         
        addNewProduct("red diary", ECategory.Diaries, 17,65);               //10008
        addNewProduct("paintbrush", ECategory.ArtMaterials, 7, 25);          //10009
        addNewProduct("Sudoku", ECategory.Games, 9.5, 41);                  //10010

        //  addNewOrder()
        addNewOrderItem(100000, 200007, 6.9, 3);
        addNewOrderItem(100002, 200007, 5.9, 1);
        addNewOrderItem(100009, 200007, 7, 7);
        addNewOrderItem(100010, 200007, 9.5, 2);

        addNewOrderItem(100000, 200008, 6.9, 3);
        addNewOrderItem(100002, 200008, 5.9, 1);
        addNewOrderItem(100003, 200008, 4.9, 1);
        addNewOrderItem(100004, 200008, 9.9, 2);

        addNewOrderItem(100007, 200009, 6.9, 3);
        addNewOrderItem(100002, 200009, 5.9, 1);
        addNewOrderItem(100003, 200009, 4.9, 1);
        addNewOrderItem(100004, 200009, 9.9, 2);


    }
}

