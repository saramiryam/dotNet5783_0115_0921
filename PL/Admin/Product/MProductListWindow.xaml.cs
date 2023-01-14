//להוסיף אופציה של בחירת כל המוצרים
//לשנות את פונקציית קבלת המוצרים לפרדיקט
//לתקןאת הBIDING של הID

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
using BlImplementation;
using BlApi;
using BO;
using System.Collections.ObjectModel;
using DO;
using PL.NewOrder.ProductItem;

namespace PL;

/// <summary>
/// Interaction logic for MProductForList.xaml
/// </summary>
public partial class MProductListWindow : Window
{
    #region prorerties
    BlApi.IBl? bl = BlApi.Factory.Get();
    public System.Array Categories { get; set; } = Enum.GetValues(typeof(BO.Enums.ECategory));
    public BO.Enums.ECategory? selectedCategory { get; set; } = null;
    public BO.ProductForList? productToUp { get; set; }=new();
    #region הפרופרטי של בחירת קטגוריה
    //public readonly DependencyProperty selectedCategoryProperty = DependencyProperty.Register(nameof(selectedCategory),
    //                                                                                                typeof(Enums.ECategory?),
    //                                                                                                typeof(MProductListWindow));
    //public Enums.ECategory? selectedCategory
    //{
    //    get { return (Enums.ECategory?)GetValue(selectedCategoryProperty); }
    //    set { SetValue(selectedCategoryProperty, value); }
    //}
    #endregion
    #region productsForListListProperty
    public static readonly DependencyProperty productsForListListProperty = DependencyProperty.Register(nameof(productsForListList),
                                                                                                           typeof(ObservableCollection<ProductForList?>),
                                                                                                   typeof(MProductListWindow));
    public ObservableCollection<ProductForList?> productsForListList
    {
        get { return (ObservableCollection<ProductForList?>)GetValue(productsForListListProperty); }
        set { SetValue(productsForListListProperty, value); }
    }
    #endregion
    #region product to up property
    //public readonly DependencyProperty productToUpProperty = DependencyProperty.Register(nameof(productToUp),
    //                                                                                                       typeof(BO.Product),
    //                                                                                               typeof(MProductListWindow));
    //public BO.Product productToUp
    //{
    //    get { return (BO.Product)GetValue(productToUpProperty); }
    //    set { SetValue(productToUpProperty, value); }
    //}
    #endregion
    #endregion




    public MProductListWindow()
    {
        try
        {
            productsForListList = new ObservableCollection<ProductForList?>(bl.Product.GetListOfProduct().Cast<ProductForList?>());
        }
        catch (RequestedItemNotFoundException ex)
        {
            MessageBox.Show(ex.Message.ToString());
        }
        InitializeComponent();
        //  ProductListView.ItemsSource = (bl?.Product.GetListOfProduct());
        // CategorySelector.Items.Add("all products");

    }

    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (selectedCategory is not null)
            try
            {
                productsForListList = new(bl.Product.GetProductForListByCategory((BO.Enums.ECategory)selectedCategory!));
            }
            catch (RequestedItemNotFoundException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
    }
    public void addProduct(ProductForList product) => productsForListList.Insert(productsForListList.Count, product);
    public void updateProduct(ProductForList product)
    {
        int index = productsForListList.IndexOf(product) + 1;
        productsForListList[index] = product;
    }
    private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        new PL.Product.MProductWindow(productToUp.ID).ShowDialog();
        try
        {
            productsForListList = new(bl.Product.GetListOfProduct());
        }
        catch (RequestedItemNotFoundException ex)
        {
            MessageBox.Show(ex.Message.ToString());
        }
    }

    private void Add_Click_(object sender, RoutedEventArgs e)
    {
        new Product.MProductWindow(addProduct).ShowDialog();
//        productsForListList = Convert(bl.Product.GetListOfProduct());

    }
    private void Back_Click(object sender, RoutedEventArgs e)
    {
        new MWindow().Show();
        Close();
    }
}
