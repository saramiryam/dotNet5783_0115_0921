using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace PL.NewOrder.ProductItem
{

        public class CnvrtIntToBool : IValueConverter
        {
            //המרה ממקור ליעד
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                //value: הערך שאיתו הגענו לפונקציה, כאן בוליאני
                //יחזור הערך לאחר המרה
                if ((bool)value == true)
                    return "true";
                return "false";
            }

            //המרה מיעד למקור
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
