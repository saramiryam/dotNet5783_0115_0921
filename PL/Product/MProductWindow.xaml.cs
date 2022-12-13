using BlApi;
using BlImplementation;
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

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for MProductWindow.xaml
    /// </summary>
    public partial class MProductWindow : Window
    {

        IBl bl = new Bl();
        BO.Product productTOUp = new BO.Product();

        public MProductWindow()
        {
            InitializeComponent();
            chooseCategoryToAdd.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));
            id.Visibility=Visibility.Collapsed;
            idLabel.Visibility = Visibility.Collapsed;
            


        }
        public MProductWindow(int idToUpdate)
        {
            InitializeComponent();
            addOrUpdateButton.Content = "update";
            productTOUp = bl.Product.GetProductItem(idToUpdate);
            id.Text = productTOUp.ID.ToString();
            name.Text = productTOUp.Name!.ToString();
            chooseCategoryToAdd.SelectedIndex = (int)productTOUp.Category!;
            price.Text = productTOUp.Price.ToString();
            inStock.Text = productTOUp.InStock.ToString();
            chooseCategoryToAdd.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
    
            string _Name = name.Text;
            var _Cat = chooseCategoryToAdd.SelectedValue;
            double _Price = double.Parse(price.Text);
            int _InStock = int.Parse(inStock.Text);
            string _action = addOrUpdateButton.Content.ToString()!;
            bl.Product.AddProduct(new BO.Product() { Name=_Name,Category=(BO.Enums.ECategory)_Cat,Price=_Price,InStock=_InStock});
            if (addOrUpdateButton.Content.ToString() == "add")
            {
                MessageBox.Show("the product " + _Name + " add");
                new MProductListWindow().Show();
                this.Close();
                Close();
            }
            else
            {
                MessageBox.Show("the product " + _Name + " update");
                new MProductListWindow().Show();
                this.Close();
                Close();
            }

        }
    }
}
