﻿
using static BO.Enums;

namespace BO;
//מעקב הזמנה ללקוח

public class OrderTracking
{
    #region orderTracking properties

    public int ID { get; set; }
    public EStatus Status { get; set; }
    public List<StatusAndDate> listOfStatus { get; set; }    

    #endregion

    #region class Status and date
    public class StatusAndDate
    { 
        public DateTime Date { get; set; }
        public BO.Enums.EStatus Statuss { get; set; }

            public override string ToString() => $@"
            Status:{Statuss}
            date:{Date}

";

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
    list of status:{listOfStatus}

";
    #endregion

}
