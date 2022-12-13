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
            //CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));
            //CategorySelector.SelectedIndex = -1;
            CategorySelector.Items.Add(BO.Enums.ECategory.Notebooks);
            CategorySelector.Items.Add(BO.Enums.ECategory.Games);
            CategorySelector.Items.Add(BO.Enums.ECategory.Pens);
            CategorySelector.Items.Add(BO.Enums.ECategory.ArtMaterials);
            CategorySelector.Items.Add(BO.Enums.ECategory.Notebooks);
            CategorySelector.Items.Add(BO.Enums.ECategory.Diaries);
            CategorySelector.Items.Add("all products");
            CategorySelector.Text = "all";

        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //casting toString
            var cat=CategorySelector.SelectedItem;
            if (cat is BO.Enums.ECategory)
                ProductListView.ItemsSource = bl.Product.GetProductForListByCategory((BO.Enums.ECategory)cat!);
            else
                ProductListView.ItemsSource = bl.Product.GetListOfProduct();
        }
        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            BO.ProductForList p = (BO.ProductForList)ProductListView.SelectedValue;
            new Product.MProductWindow(p.ID).Show();

        }

        private void Add_Click_(object sender, RoutedEventArgs e)
        {
            new Product.MProductWindow().Show();
        }
    }
}
