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

namespace PL
{
    /// <summary>
    /// Interaction logic for MProductForList.xaml
    /// </summary>
    public partial class MProductListWindow : Window
    {
        IBl bl = new Bl();
        public MProductListWindow()
        {
            InitializeComponent();
            ProductListView.ItemsSource = bl.Product.GetListOfProduct();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));
        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //casting toString
            string cat=CategorySelector.SelectedItem.ToString();
            ProductListView.ItemsSource = bl.Product.GetProductForListByCategory(cat);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Product.MProductWindow(bl.Product).Show();
        }
    }
}
