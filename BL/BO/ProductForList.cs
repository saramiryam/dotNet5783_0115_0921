using static BO.Enums;
namespace BO;
//מוצר כללי בקטלוג ראשי
public class ProductForList
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public ECategory? Category { get; set; }
    public double Price { get; set; }


    #region ToString
    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the  ProductForList class</returns>
    public override string ToString() => $@"
    Product ID={ID}: {Name}, 
    category - {Category}
    Price: {Price}
";
    #endregion

}
