using PL.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Admin.Order
{
    /// <summary>
    /// Interaction logic for MOrderWindow.xaml
    /// </summary>
    public partial class MOrderWindow : Window
    {
        #region prorerties
        BlApi.IBl? bl = BlApi.Factory.Get();

        public BO.Order OrderToUp
        {
            get { return (BO.Order)GetValue(OrderToUpProperty); }
            set { SetValue(OrderToUpProperty, value); }
        }
        public static readonly DependencyProperty OrderToUpProperty = DependencyProperty.Register(nameof(OrderToUp),
                                                                                                               typeof(BO.Order),
                                                                                                       typeof(MOrderWindow));


        #endregion

        public MOrderWindow(int orderID)
        {
            if (bl != null)
            {
                OrderToUp = bl.Order.GetOrderDetails(orderID);
            }
            InitializeComponent();
        }
    }
}
