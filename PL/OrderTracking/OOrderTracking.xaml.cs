using PL.Admin.Order;
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

namespace PL.OrderTracking
{
    /// <summary>
    /// Interaction logic for OOrderTracking.xaml
    /// </summary>
    public partial class OOrderTracking : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public int orderId { get; set; }
        public BO.OrderTracking? orderTrackingToUp { get; set; } = new();

        public OOrderTracking(int id)
        {
            orderId = id;
            if (bl != null)
            {
                orderTrackingToUp = bl.Order.GetOrderTracking(orderId);
            }
             InitializeComponent();
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            new MOrderWindow(orderId, true).Show();
            Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            new OTWindow().Show();
            Close();
        }
    }
}
