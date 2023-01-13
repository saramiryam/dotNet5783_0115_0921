using PL.NewOrder.Cart;
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

namespace PL.NewOrder.ProductItem;

/// <summary>
/// Interaction logic for NPCart.xaml
/// </summary>
public partial class NOItemsInCartWindow : Window
{
    BlApi.IBl? bl = BlApi.Factory.Get();
    public BO.OrderItem ProductToChange { get; set; } = new();

    public static readonly DependencyProperty MyCartProperty = DependencyProperty.Register(nameof(MyCart),
                                                                                               typeof(BO.Cart),
                                                                                       typeof(NOItemsInCartWindow));
    public BO.Cart MyCart
    {
        get { return (BO.Cart)GetValue(MyCartProperty); }
        set { SetValue(MyCartProperty, value); }
    }
    public NOItemsInCartWindow(BO.Cart MyCart)
    {
        ProductToChange = new();
        this.MyCart = MyCart;
        InitializeComponent();
    }
    private void Back_Click(object sender, RoutedEventArgs e)
    {
        new PProductItemList(MyCart).Show();
        Close();
    }
    private void Submit_Click(object sender, RoutedEventArgs e)
    {
     
        new NOUserDetails(MyCart).Show();
        Close();
    }

    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        new NOItemToUpFromCart(MyCart,ProductToChange.ID).Show();
        Close();
    }
}
