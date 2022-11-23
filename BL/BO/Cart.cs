using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Enums;

namespace BO;
//לקונה
public class Cart
{
    #region cart properties

    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAdress { get; set; }
    public List<OrderItem> ItemList { get; set; }
    public double TotalSum { get; set; }

    #endregion

    #region ToString

    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the Cart class</returns>
    public override string ToString() => $@"
    Name: {CustomerName}, 
    Email - {CustomerEmail}
    Adress: {CustomerAdress}
    List of Item:{ItemList}
    Total sum:{TotalSum}
";

    #endregion
}
