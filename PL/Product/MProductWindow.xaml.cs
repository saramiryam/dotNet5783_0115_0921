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
        BO.Product productTOUp =new BO.Product();

        public MProductWindow()
        {
            InitializeComponent();
            chooseCategoryToAdd.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));
        }
        public MProductWindow(int idToUpdate)
        {
            InitializeComponent();
            addOrUpdateButton.Content = "update";
            productTOUp= bl.Product.GetProductItem(idToUpdate);
            id.Text= productTOUp.ID.ToString();
            name.Text= productTOUp.Name!.ToString();
            chooseCategoryToAdd.SelectedIndex = (int)productTOUp.Category!;
            price.Text = productTOUp.Price.ToString();
            inStock.Text = productTOUp.InStock.ToString();  
            chooseCategoryToAdd.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            int Id = int.Parse(id.Text);
            string Name = name.Text;
            var Cat =chooseCategoryToAdd.Text;
            double Price = double.Parse(price.Text);
            int InStock = int.Parse(inStock.Text);
            string action = addOrUpdateButton.Content.ToString()!;
            bl.Product.AddProductFromWindow(Id, Name, Cat, Price, InStock, action);
            if (addOrUpdateButton.Content is not null&& addOrUpdateButton.Content == (object)"add")
                MessageBox.Show("the product " + Name + " add");
            else
                MessageBox.Show("the product " + Name + " update");

        }
    }
}
