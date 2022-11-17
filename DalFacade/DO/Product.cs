using System.Diagnostics;
using System.Xml.Linq;
using static DO.Enums;

namespace DO;
/// <summary>
///     
/// </summary>

public struct Product
{
    #region product properties

    public int ID { get; set; }
    /// <summary>
    /// the product name - a string
    /// </summary>
    public string Name { get; set; }
    public ECategory Category { get; set; }
    public double Price { get; set; }
    public int InStock { get; set; }

    #endregion

    #region methods
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