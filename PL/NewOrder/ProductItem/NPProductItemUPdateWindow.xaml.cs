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

namespace PL.NewOrder.ProductItem;

/// <summary>
/// Interaction logic for NPProductItemUPdateWindow.xaml
/// </summary>
public partial class NPProductItemUPdateWindow : Window
{
    Action<BO.ProductItem> AddNewProduct;
    BlApi.IBl? bl = BlApi.Factory.Get();
    public static string Action { get; set; } = "";
    public  int Amount { get; set; } = 0;
    public BO.ProductItem? ProductToAdd { get; set; } = new();
    public static readonly DependencyProperty CartProperty = DependencyProperty.Register(nameof(Cart),
                                                                                               typeof(BO.Cart),
                                                                                       typeof(NPProductItemUPdateWindow));
    public BO.Cart Cart
    {
        get { return (BO.Cart)GetValue(CartProperty); }
        set { SetValue(CartProperty, value); }
    }
    //public static readonly DependencyProperty CartItemListProperty = DependencyProperty.Register(nameof(CartItemList),
    //                                                                                              typeof(ObservableCollection<BO.ProductItem?>),
    //                                                                                      typeof(NPProductItemUPdateWindow));
    //public ObservableCollection<BO.ProductItem?> CartItemList
    //{
    //    get { return (ObservableCollection<BO.ProductItem?>)GetValue(CartItemListProperty); }
    //    set { SetValue(CartItemListProperty, value); }
    //}


    public NPProductItemUPdateWindow(int id) { 
        if (bl != null)
            ProductToAdd = bl.Product.GetProductItemDetails(id);
        Cart= new BO.Cart();
        Cart.ItemList = new List< BO.OrderItem?>();
        InitializeComponent();
    }
    public NPProductItemUPdateWindow(BO.Cart MyCart,int id)
    {
        if (bl != null)
            ProductToAdd = bl.Product.GetProductItemDetails(id);
        Cart = MyCart;
        InitializeComponent();
    }
    private void Plus_Click(object sender, RoutedEventArgs e)
    {
        Action = "+";
        Amount += 1;
        MessageBox.Show(Action + Amount);


    }
    private void minus_Click(object sender, RoutedEventArgs e)
    {
        Action = "-";
        if (Amount > 0)
            Amount -= 1;
        MessageBox.Show(Action + Amount);

    }

    private void addToCart_Click(object sender, RoutedEventArgs e)
    {
        Cart = bl.Cart.AddItemToCart(Cart, ProductToAdd.ID);
        //AddNewProduct(ProductToAdd);
        //ProductToAdd.AddNewProduct += new Action<BO.ProductItem>(addNewProductToCart);
        foreach (var i in Cart.ItemList) {
            MessageBox.Show(i.Name);
        }
        

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
