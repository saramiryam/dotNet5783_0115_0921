using BO;
using PL.NewOrder.ProductItem;
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

namespace PL.NewOrder.Cart
{
    /// <summary>
    /// Interaction logic for NOItemToUpFromCart.xaml
    /// </summary>
    public partial class NOItemToUpFromCart : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public static string Action { get; set; } = "";
     //   public  BO.OrderItem ItemToChage { get; set; } = new();
        public static readonly DependencyProperty ItemToChageProperty = DependencyProperty.Register(nameof(ItemToChage),
                                                                                              typeof(BO.OrderItem),
                                                                                      typeof(NOItemToUpFromCart));
        public BO.OrderItem ItemToChage
        {
            get { return (BO.OrderItem)GetValue(ItemToChageProperty); }
            set { SetValue(ItemToChageProperty, value); }
        }
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(nameof(Amount),
                                                                                              typeof(int),
                                                                                      typeof(NOItemToUpFromCart));
        public int Amount
        {
            get { return (int)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }
        public static readonly DependencyProperty CartProperty = DependencyProperty.Register(nameof(Cart),
                                                                                           typeof(BO.Cart),
                                                                                   typeof(NOItemToUpFromCart));
        public BO.Cart Cart
        {
            get { return (BO.Cart)GetValue(CartProperty); }
            set { SetValue(CartProperty, value); }
        }

        public NOItemToUpFromCart(BO.Cart MyCart,int id)
        {
            if (bl != null)
                ItemToChage=bl.Order.GetOrderItemDetails(MyCart, id);
            Cart = MyCart;
            Amount = ItemToChage.Amount;
            InitializeComponent();
        }

        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cart = bl.Cart.UpdateAmount(Cart, ItemToChage.ID, Amount);
                MessageBox.Show("UPDATE item");
                new NOItemsInCartWindow(Cart).Show();
                Close();
            }
            catch (ProductNotInStockException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            }
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            Amount = 0;
            try
            {
                Cart = bl.Cart.UpdateAmount(Cart, ItemToChage.ID, Amount);
                MessageBox.Show("remove item");
                new NOItemsInCartWindow(Cart).Show();
                Close();
            }
            catch(ProductNotInStockException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            Action = "+";
            Amount += 1;


        }
        private void minus_Click(object sender, RoutedEventArgs e)
        {
            Action = "-";
            if (Amount > 0)
                Amount -= 1;

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new PProductItemList(Cart).Show();
            Close();
        }
        private void goCart_Click(object sender, RoutedEventArgs e)
        {
            new NOItemsInCartWindow(Cart).Show();
            Close();

        }
    }
}
