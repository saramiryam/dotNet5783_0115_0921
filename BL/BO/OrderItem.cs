
namespace BO;
//לקונה

public class OrderItem
{
    #region order item properties
    public int numInOrder { get; set; }//אתחול ב-0 ואז ליסט.קאונט
    public int ID { get; set; }
    public int Name { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
    public double sumItem { get; set; }

    #endregion


    #region methods
    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the  OrderItems in order</returns>
    public override string ToString() => $@"
    item number={numInOrder}
    Order Item ID={ID}
    name={Name}:
    Price: {Price}
    Amount : {Amount}
    sum item={sumItem}
";


    #endregion
}
