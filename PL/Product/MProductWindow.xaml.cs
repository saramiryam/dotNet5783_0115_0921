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
        BlApi.IProduct product { get; set; }  

        public MProductWindow(BlApi.IProduct p)
        {
            InitializeComponent();
            chooseCategoryToAdd.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));
            product=p;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            int Id=int.Parse(id.Text);
            string Name=name.Text;
            var Cat = chooseCategoryToAdd.Text;
            double Price=double.Parse(price.Text);
            double InStock=double.Parse(inStock.Text);


            
            
        }
    }
}
