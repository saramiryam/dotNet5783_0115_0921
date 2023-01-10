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
/// Interaction logic for PProductItemList.xaml
/// </summary>
public partial class PProductItemList : Window
{
    Action<BO.ProductItem> AddNewProduct;
    BlApi.IBl? bl = BlApi.Factory.Get();
    public static string Action { get; set; }="";
    public static int Amount { get; set; } = 0;
    public System.Array Categories { get; set; } = Enum.GetValues(typeof(Enums.ECategory));
    public BO.ProductItem Product { get; set; } = new();
    public BO.ProductItem ProductToAdd { get; set; } = new();
    public Enums.ECategory? selectedCategory { get; set; } = null;
    public static readonly DependencyProperty productItemListProperty = DependencyProperty.Register(nameof(ProductsItemList),
                                                                                                       typeof(ObservableCollection<BO.ProductItem?>),
                                                                                               typeof(PProductItemList));
    public ObservableCollection<BO.ProductItem?> ProductsItemList
    {
        get { return (ObservableCollection<BO.ProductItem?>)GetValue(productItemListProperty); }
        set { SetValue(productItemListProperty, value); }
    }
    public static readonly DependencyProperty CartProperty = DependencyProperty.Register(nameof(Cart),
                                                                                                   typeof(BO.Cart),
                                                                                           typeof(PProductItemList));
    public BO.Cart Cart
    {
        get { return (BO.Cart)GetValue(CartProperty); }
        set { SetValue(CartProperty, value); }
    }
    public static readonly DependencyProperty CartItemListProperty = DependencyProperty.Register(nameof(CartItemList),
                                                                                                  typeof(ObservableCollection<BO.ProductItem?>),
                                                                                          typeof(PProductItemList));
    public ObservableCollection<BO.ProductItem?> CartItemList
    {
        get { return (ObservableCollection<BO.ProductItem?>)GetValue(CartItemListProperty); }
        set { SetValue(CartItemListProperty, value); }
    }
    //public static readonly DependencyProperty AmountsProperty =
    //            DependencyProperty.Register(nameof(Amounts),
    //                                        typeof(int), typeof(PProductItemList));

    //public int Amounts
    //{
    //    get => (int)GetValue(AmountsProperty);
    //    set => SetValue(AmountsProperty, value);
    //}
    public PProductItemList()
    {
        Amount = 0;
        CartItemList = new();
  //      ProductToAdd.AddNewProduct += new Action<BO.ProductItem>(addNewProductToCart);
        ProductsItemList = new(bl.Product.GetProductItemList());
        InitializeComponent();
    }
    public PProductItemList(BO.Cart MyCart)
    {
        Amount = 0;
        Cart=MyCart;
      //  ProductToAdd.AddNewProduct += new Action<BO.ProductItem>(addNewProductToCart);
        ProductsItemList = new(bl.Product.GetProductItemList());
        InitializeComponent();
    }
    //public void addNewProductToCart(BO.ProductItem p)
    //{
    //    CartItemList.Add(p);
    //}

    private void goCart_Click(object sender, RoutedEventArgs e)
    {
        new NOItemsInCartWindow(Cart).Show();
       // ProductToAdd.AddNewProduct += new Action<BO.ProductItem>(addNewProductToCart);

    }


    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (selectedCategory is not null)
            ProductsItemList = new(bl.Product.GetProductItemList(e=> (bool)(e?.Category.Equals((DO.Enums.ECategory)selectedCategory))));
    }

    private void ProductItemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (Cart is null)
        {
            new NPProductItemUPdateWindow(Product.ID).Show();
            Close();
        }
        else
        {
            new NPProductItemUPdateWindow(Cart, Product.ID).Show();
            Close();
        }
    }

 
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ProductsItemList = new(bl.Product.GetProductItemListGrouping());

    }

    private void Back_Click_(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}
