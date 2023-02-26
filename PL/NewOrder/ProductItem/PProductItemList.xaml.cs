using BO;
using DO;
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
    public static string Action { get; set; } = "";
    public static int Amount { get; set; } = 0;
    public static bool ChkIfEnable { get; set; } = false;
    public System.Array Categories { get; set; } = Enum.GetValues(typeof(BO.Enums.ECategory));
    public BO.ProductItem Product { get; set; } = new();
    public BO.ProductItem ProductToAdd { get; set; } = new();
    public BO.Enums.ECategory? selectedCategory { get; set; } = null;
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

    public PProductItemList()
    {
        Amount = 0;
        Cart = new();
        try
        {
            ProductsItemList = new(bl.Product.GetProductItemList());
        }
        catch (RequestedItemNotFoundException ex)
        {
            MessageBox.Show(ex.Message.ToString());
        }
        InitializeComponent();
    }

    public PProductItemList(BO.Cart MyCart)
    {
        Amount = 0;
        Cart = MyCart;
        try
        {
            ProductsItemList = new(bl.Product.GetProductItemList());
        }
        catch (RequestedItemNotFoundException ex)
        {
            MessageBox.Show(ex.Message.ToString());
        }
        InitializeComponent();

    }
    public static string InStockCnvrt()
    {
        if (Amount > 0)
            return "true";
        return "false";
    }


    private void goCart_Click(object sender, RoutedEventArgs e)
    {
        new NOItemsInCartWindow(Cart).Show();
        Close();
        // ProductToAdd.AddNewProduct += new Action<BO.ProductItem>(addNewProductToCart);

    }


    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (selectedCategory is not null)
            try
            {
                ProductsItemList = new(bl.Product.GetProductItemList(e => (bool)(e?.Category.Equals((DO.Enums.ECategory)selectedCategory))));
            }
            catch (RequestedItemNotFoundException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
    }

    private void ProductItemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {

        new NPProductItemUPdateWindow(Cart, Product.ID, updateList).Show();
       


    }


    private void chkEnable_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ProductsItemList = new(bl.Product.GetProductItemList());
        }
        catch (RequestedItemNotFoundException ex)
        {
            MessageBox.Show(ex.Message.ToString());
        }
        if (chkEnable.IsChecked == true)
        {

            var GropupingProducts = (from p in ProductsItemList
                                     group p by p.Category into catGroup
                                     from pr in catGroup
                                     select pr).ToList();
            ProductsItemList = new(GropupingProducts);
        }
    }

    private void Back_Click_(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    //זה הפונקציה שמעדכנת את הרשימה
    // תשלחי אותה כפרמטר לחלון שבו את מעדכנת (הייתי עושה את זה בכיף אבל אין לי מושג מה הולך פה עם החלונות)
    //בחלון ההוא תגדירי משתנה מסוג ACTION 
    //בבנאי תשוי את המשתנה לפונקציה שקיבלת בפרטמר
    //תזמני אותה בסוף המתודה של העדכון
    //זה יקרא לפונקציה שפה ויעדכן את הרשימה פה....
    //רוב הצלחה שפע וברכה!
    private void updateList(BO.ProductItem p)
    {
        var item = ProductsItemList.FirstOrDefault(item => item.ID == p.ID);
        //item.AmoutInYourCart = p.AmoutInYourCart;
        ProductsItemList[ProductsItemList.IndexOf(item)] = p;
    }
}
