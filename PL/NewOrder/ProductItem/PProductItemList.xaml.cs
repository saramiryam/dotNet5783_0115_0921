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

    public static readonly DependencyProperty productItemListProperty = DependencyProperty.Register(nameof(ProductsItemList),
                                                                                                       typeof(ObservableCollection<BO.ProductItem?>),
                                                                                               typeof(PProductItemList));
    public ObservableCollection<BO.ProductItem?> ProductsItemList
    {
        get { return (ObservableCollection<BO.ProductItem?>)GetValue(productItemListProperty); }
        set { SetValue(productItemListProperty, value); }
    }
    public PProductItemList()
    {
        ProductsItemList = new(bl.Product.GetProductItemList());
        InitializeComponent();
    }
}
