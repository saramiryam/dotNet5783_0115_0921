using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Enums;

namespace BO;
//מעקב הזמנה ללקוח

public class OrderTracking
{
    #region orderTracking properties

    public int ID { get; set; }
    public EStatus Status { get; set; }
    public IEnumerable<StatusAndDate> listOfStatus { get; set; }    

    #endregion

    #region class Status and date
    public class StatusAndDate
    { 
        public DateTime Date { get; set; }
        public EStatus Status { get; set; }
    }

    #endregion

    #region methods
    /// <summary>
    /// override the string function
    /// </summary>
    /// <returns>string with the properties of the order tracking class</returns>
    public override string ToString() => $@"
    Order tracking ID={ID}
    Status:{Status}

";
    #endregion

}
