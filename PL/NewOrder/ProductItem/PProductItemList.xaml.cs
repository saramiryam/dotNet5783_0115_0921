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
    BlApi.IBl? bl = BlApi.Factory.Get();
    public static string Action { get; set; }="";
    public static int Amount { get; set; } = 0;
    public System.Array Categories { get; set; } = Enum.GetValues(typeof(Enums.ECategory));
    public BO.ProductItem? Product { get; set; } = new();
    public Enums.ECategory? selectedCategory { get; set; } = null;
    public static readonly DependencyProperty productItemListProperty = DependencyProperty.Register(nameof(ProductsItemList),
                                                                                                       typeof(ObservableCollection<BO.ProductItem?>),
                                                                                               typeof(PProductItemList));
    public ObservableCollection<BO.ProductItem?> ProductsItemList
    {
        get { return (ObservableCollection<BO.ProductItem?>)GetValue(productItemListProperty); }
        set { SetValue(productItemListProperty, value); }
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
        ProductsItemList = new(bl.Product.GetProductItemList());
        InitializeComponent();
    }

    private void goCart_Click(object sender, RoutedEventArgs e)
    {
        new NOItemsInCartWindow().Show();
    }

    private void Plus_Click(object sender, RoutedEventArgs e)
    {
        Action = "+";
        Amount += 1;
        MessageBox.Show(Action + Amount);


    }

    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (selectedCategory is not null)
            ProductsItemList = new(bl.Product.GetProductItemList(e=> (bool)(e?.Category.Equals((DO.Enums.ECategory)selectedCategory))));
        ///////////////////////////////////////////////////////////////////
    }

    private void ProductItemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        new NPProductItemUPdateWindow(Product.ID).Show();
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
        CartItemList.Add(Product);
        MessageBox.Show(Product.Name);

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ProductsItemList = new(bl.Product.GetProductItemListGrouping());

    }
}
