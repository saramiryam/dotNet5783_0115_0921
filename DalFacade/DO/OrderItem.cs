

namespace DO;

public struct OrderItem
{
    #region order item properties
    public int ID { get; set; }
    public int ProductID { get; set; }
    public int OrderID { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }

    #endregion


    #region methods
    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the  OrderItem struct</returns>
    public override string ToString() => $@"
    Order Item ID={ID}
    Product ID={ProductID}:
    OrderID={OrderID}, 
    Price: {Price}
    Amount : {Amount}
";
    

    #endregion
}


