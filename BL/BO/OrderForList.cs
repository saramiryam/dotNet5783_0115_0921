
using static BO.Enums;

namespace BO;
//רשימת הזמנות למעקב כללי בחנות

public class OrderForList
{
    #region order properties

    public int OrderID { get; set; }
    public string? CustomerName { get; set; }
    public EStatus? Status { get; set; }
    public int AmountOfItem { get; set; }
    public double TotalSum { get; set; }


    #endregion

    #region ToString

    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the OrderForList class</returns>
    public override string ToString() => $@"
    Order ID={OrderID}: 
    name: {CustomerName}, 
    Status of order: {Status}
    amount of item: {AmountOfItem}
    Total sum:{TotalSum}
";

    #endregion
}
