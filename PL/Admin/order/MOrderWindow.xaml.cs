using BO;
using DO;
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
        public bool Enable
        {
            get { return (bool)GetValue(EnableProperty); }
            set { SetValue(EnableProperty, value); }
        }
        public static readonly DependencyProperty EnableProperty = DependencyProperty.Register(nameof(Enable),
                                                                                                  typeof(bool),
                                                                                                  typeof(MOrderWindow));
        public string MyContent
        {
            get { return (string)GetValue(MyContentProperty); }
            set { SetValue(MyContentProperty, value); }
        }
        public static readonly DependencyProperty MyContentProperty = DependencyProperty.Register(nameof(MyContent),
                                                                                                  typeof(string),
                                                                                                  typeof(MOrderWindow));
        // public static string MyContent { get; set; } = "";

        //public static bool Enable { get; set; } = true;
        public static bool fromOT { get; set; } = true;
        public int id { get; set; }

        #region order item
        public BO.OrderItem? orderItemToUp { get; set; } = new();


        #endregion


        #endregion

        public MOrderWindow(int orderID, bool OTOrNot)
        {
            fromOT = OTOrNot;
            id = orderID;
            try
            {
                OrderToUp = bl.Order.GetOrderDetails(orderID);
            }
            catch (RequestedItemNotFoundException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            if (OrderToUp.Status == BO.Enums.EStatus.Done)
            {

                Enable = true;
                MyContent = "send";
            }
            else if (OrderToUp.Status == BO.Enums.EStatus.Sent)
            {
                Enable = true;
                MyContent = "Provide";

            }
            else
            {
                MyContent = "alredy Provided";
                Enable = false;
            }
            if (fromOT == true)
            {
                Enable = false;
            }
            InitializeComponent();
        }



        private void ChengeButton_Click(object sender, RoutedEventArgs e)
        {

            if (MyContent == "Provide")
            {
                try
                {
                    OrderToUp = bl.Order.UpdateDeliveryDate(OrderToUp.ID);
                    MyContent = "alredy Provided";
                    Enable = false;

                }
                catch (RequestedItemNotFoundException ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
            else if (MyContent == "send")
            {
                try
                {
                    OrderToUp = bl.Order.UpdateShipDate(OrderToUp.ID);
                    MyContent = "Provide";

                }
                catch (RequestedItemNotFoundException ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (fromOT == true)
            {
                new OOrderTracking(id).Show();

            }
            this.Close();
        }
    }
}
