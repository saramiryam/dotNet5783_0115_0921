using BlApi;
using BO;
using PL.NewOrder.ProductItem;
using PL.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for NOUserDetails.xaml
    /// </summary>
    public partial class NOUserDetails : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public static readonly DependencyProperty CartProperty = DependencyProperty.Register(nameof(Cart),
                                                                                         typeof(BO.Cart),
                                                                                 typeof(NOUserDetails));
        public BO.Cart Cart
        {
            get { return (BO.Cart)GetValue(CartProperty); }
            set { SetValue(CartProperty, value); }
        }
        public Thickness MyMargin
        {
            get { return (Thickness)GetValue(MyMarginProperty); }
            set { SetValue(MyMarginProperty, value); }
        }
        public static readonly DependencyProperty MyMarginProperty = DependencyProperty.Register(nameof(MyMargin),
                                                                                                               typeof(Thickness),
                                                                                                       typeof(NOUserDetails));
        public string ExceText
        {
            get { return (string)GetValue(ExceTextProperty); }
            set { SetValue(ExceTextProperty, value); }
        }
        public static readonly DependencyProperty ExceTextProperty = DependencyProperty.Register(nameof(ExceText),
                                                                                                               typeof(string),
                                                                                                       typeof(NOUserDetails));

        public NOUserDetails(BO.Cart MyCart)
        {
            Cart = MyCart;
            InitializeComponent();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.Cart.SubmitOrder(Cart, Cart.CustomerName, Cart.CustomerEmail, Cart.CustomerAdress);
                MessageBox.Show("ההזמנה בוצעה");
                new MainWindow().Show();
                Close();
            }
            catch (AdressIsNullException ex)
            {
                ExceText = ex.Message;
                MyMargin = new Thickness(289, 150, 0, 0);
            }
            catch (NameIsNullException ex)
            {
                ExceText = ex.Message;
                MyMargin = new Thickness(289, 110, 0, 0);
            }
            catch (UncorrectEmailException ex)
            {
                ExceText = ex.Message;
                MyMargin = new Thickness(289, 190, 0, 0);
            }
            catch (ItemInCartNotExistsAsProductException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zא-ת]+");
            e.Handled = regex.IsMatch(e.Text);
        }



    }
}
