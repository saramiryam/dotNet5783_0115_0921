using BO;
using PL.OrderTracking;
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
        public static bool CanChange { get; set; } = true;
        public  int id { get; set; }

        #region order item
        public BO.OrderItem? orderItemToUp { get; set; } = new();

      
        #endregion


        #endregion

        public MOrderWindow(int orderID,bool change)
        {
            id=orderID;
            CanChange=change;
            if (bl != null)
            {
                OrderToUp = bl.Order.GetOrderDetails(orderID);
            }
            if (OrderToUp.Status == BO.Enums.EStatus.Done)
            { 
                if(CanChange == true)
                {
                    anable = false;
                }
                else
                {
                    anable = true;
                }
                MyContent = "send";
             }
            else if (OrderToUp.Status == BO.Enums.EStatus.Sent)
            {
                if (CanChange == true)
                {
                    anable = false;
                }
                else
                {
                    anable = true;
                }
                MyContent = "Provide";

            }
            else
            {
                MyContent = "alredy Provided";
               anable =false;   
            }
            InitializeComponent();
        }

        

        private void ChengeButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (MyContent == "Provide")
            {
                OrderToUp = bl.Order.UpdateDeliveryDate(OrderToUp.ID);
                MessageBox.Show(OrderToUp.Status.ToString());
            }
            else if (MyContent == "send")
            {
                MyContent = "Provide";
                OrderToUp = bl.Order.UpdateShipDate(OrderToUp.ID);
                MessageBox.Show(OrderToUp.Status.ToString());
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanChange == true)
            {
                new OOrderTracking(id).Show();
            }
            else
            {
                new MOrderListWindow().Show();
            }
            Close();
        }
    }
}
