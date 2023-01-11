using BO;
using PL.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static BO.Enums;

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
        public static string MyContent { get; set; } = "";

        public static bool anable { get; set; }=true;
        
        #region order item
        public BO.OrderItem? orderItemToUp { get; set; } = new();

      
        #endregion


        #endregion

        public MOrderWindow(int orderID)
        {
            if (bl != null)
            {
                OrderToUp = bl.Order.GetOrderDetails(orderID);
            }
            if (OrderToUp.Status == BO.Enums.EStatus.Done)
            {
                MyContent = "send";
                anable = true;

            }
            else if (OrderToUp.Status == BO.Enums.EStatus.Sent)
            {
                MyContent = "Provide";
                anable = true;

            }
            else
            {
                MyContent = "alredy Provided";
                anable =false;   
            }
            InitializeComponent();
        }

        private void id_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void status_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void order_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void adress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ship_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void delivery_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void item_list_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void totalSum_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OTButton_Click(object sender, RoutedEventArgs e)
        {
            new Order.MOrderTrackingWindow(OrderToUp.ID).ShowDialog();
        }

        private void ChengeButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (MyContent == "Provide")
            {
                
                MessageBox.Show(bl.Order.UpdateDeliveryDate(OrderToUp.ID).Status.ToString());
            }
            else if (MyContent == "send")
            {
                MessageBox.Show(bl.Order.UpdateShipDate(OrderToUp.ID).Status.ToString());
            }
        }
    }
}
