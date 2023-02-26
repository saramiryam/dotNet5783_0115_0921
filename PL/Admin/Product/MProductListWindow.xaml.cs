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
using PL.Product;

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
    public BO.ProductForList? productToUp { get; set; } = new();
    #region productsForListListProperty
    public static readonly DependencyProperty? productsForListListProperty = DependencyProperty.Register(nameof(productsForListList),
                                                                                                           typeof(ObservableCollection<ProductForList?>),
                                                                                                   typeof(MProductListWindow));
    public ObservableCollection<ProductForList?>? productsForListList
    {
        get { return (ObservableCollection<ProductForList?>)GetValue(productsForListListProperty); }
        set { SetValue(productsForListListProperty, value); }
    }
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

    }
    private void Back_Click(object sender, RoutedEventArgs e)
    {
        new MWindow().Show();
        Close();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        productsForListList = new(bl.Product.GetListOfProduct());
    }
}
