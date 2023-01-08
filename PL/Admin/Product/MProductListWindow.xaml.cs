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

namespace PL;

/// <summary>
/// Interaction logic for MProductForList.xaml
/// </summary>
public partial class MProductListWindow : Window
{
    #region prorerties
    BlApi.IBl? bl = BlApi.Factory.Get();
    public System.Array Categories { get; set; } = Enum.GetValues(typeof(Enums.ECategory));
    public Enums.ECategory? selectedCategory { get; set; } = null;
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
        productsForListList = new(bl.Product.GetListOfProduct());
        InitializeComponent();
        //  ProductListView.ItemsSource = (bl?.Product.GetListOfProduct());
        // CategorySelector.Items.Add("all products");

    }

    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (selectedCategory is not null)
            productsForListList = new(bl.Product.GetProductForListByCategory((Enums.ECategory)selectedCategory!));
    }
    private void addProduct(ProductForList product)=>productsForListList.ToList().Add(product);
    private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if(productToUp is not null)
       // BO.ProductForList p = (BO.ProductForList)ProductListView.SelectedValue;
        new PL.Product.MProductWindow(productToUp.ID).ShowDialog();
        productsForListList=new(bl.Product.GetListOfProduct());
    }

    private void Add_Click_(object sender, RoutedEventArgs e)
    {
        new Product.MProductWindow(addProduct).ShowDialog();
//        productsForListList = Convert(bl.Product.GetListOfProduct());

    }
}
