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
/// Interaction logic for NPProductItemUPdateWindow.xaml
/// </summary>
public partial class NPProductItemUPdateWindow : Window
{
    BlApi.IBl? bl = BlApi.Factory.Get();
    public BO.ProductItem? ProductToSee { get; set; } = new();

    public NPProductItemUPdateWindow(int id) { 
        if (bl != null)
            ProductToSee = bl.Product.GetProductItemDetails(id);
        InitializeComponent();
    }
}
