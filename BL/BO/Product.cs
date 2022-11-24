using static BO.Enums;


namespace BO;
//משמש לניהול
public class Product
{
    #region product properties

    public int ID { get; set; }
    public string Name { get; set; }
    public ECategory Category { get; set; }
    public double Price { get; set; }
    public int InStock { get; set; }

    #endregion

    #region ToString
    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the  Product struct</returns>
    public override string ToString() => $@"
    Product ID={ID}: {Name}, 
    category - {Category}
    Price: {Price}
    Amount in stock: {InStock}
";
    #endregion

}
