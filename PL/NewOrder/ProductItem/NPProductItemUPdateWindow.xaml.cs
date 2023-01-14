using BO;
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
 //   public  int Amount { get; set; } = 0;
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

    public static readonly DependencyProperty AmountProperty = DependencyProperty.Register(nameof(Amount),
                                                                                                  typeof(int),
                                                                                          typeof(NPProductItemUPdateWindow));
    public int  Amount
    {
        get { return (int)GetValue(AmountProperty); }
        set { SetValue(AmountProperty, value); }
    }

    public NPProductItemUPdateWindow(BO.Cart MyCart,int id)
    {
        Cart = MyCart is not null?MyCart:new();
        if (MyCart.ItemList is null) MyCart.ItemList = new List<OrderItem?>();
        if (bl != null)
            try
            {
                ProductToAdd = bl.Product.GetProductItemDetails(Cart, id);
            }
            catch (ProductNotExistsException ex)
            {
                MessageBox.Show(ex.Message);    
            }
        Amount = ProductToAdd.AmoutInYourCart == 0 ? 1 : ProductToAdd.AmoutInYourCart;
        InitializeComponent();
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
            Amount = Amount > 0 ? Amount - 1 : 0;

    }

    private void addToCart_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            for (int i = 0; i < Amount-ProductToAdd.AmoutInYourCart; i++)
            {
                try
                {
                    Cart = bl.Cart.AddItemToCart(Cart, ProductToAdd.ID);
                }
                catch (ProductNotExistsException ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                //ProductToAdd.AmoutInYourCart++;
            }
            new PProductItemList(Cart).Show();
        Close();
        }
        catch(ProductNotInStockException ex)
        {
            MessageBox.Show(ex.Message.ToString());
            new PProductItemList(Cart).Show();  
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
