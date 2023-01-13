using PL.NewOrder;
using PL.NewOrder.Cart;
using PL.NewOrder.ProductItem;
using PL.OrderTracking;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            new MWindow().Show();
            this.Close();
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
          new PProductItemList().Show();
            this.Close();
        }

        private void OrderTracking_Click(object sender, RoutedEventArgs e)
        {
            new OTWindow().Show();
            this.Close();
        }
    }
}
