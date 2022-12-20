using BlApi;
using BlImplementation;
using BO;
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

        BlApi.IBl? bl = BlApi.Factory.Get();

        BO.Product productTOUp = new BO.Product();

        public MProductWindow()
        {
            InitializeComponent();
            chooseCategoryToAdd.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));
            


        }
        public MProductWindow(int idToUpdate)
        {
            InitializeComponent();
            addOrUpdateButton.Content = "update";
            if (bl != null)
            {
                productTOUp = bl.Product.GetProductItem(idToUpdate);
            }
            id.Text = productTOUp.ID.ToString();
            id.IsReadOnly = true;
            name.Text = productTOUp.Name!.ToString();
            chooseCategoryToAdd.SelectedIndex = (int)productTOUp.Category!;
            price.Text = productTOUp.Price.ToString();
            inStock.Text = productTOUp.InStock.ToString();
            chooseCategoryToAdd.ItemsSource = Enum.GetValues(typeof(BO.Enums.ECategory));

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            int _id=int.Parse(id.Text.ToString());
            string _Name = name.Text;
            var _Cat = chooseCategoryToAdd.SelectedValue;
            double _Price = double.Parse(price.Text);
            int _InStock = int.Parse(inStock.Text);
            string _action = addOrUpdateButton.Content.ToString()!;
            try
            {

                if (addOrUpdateButton.Content.ToString() == "add")
                {
                    bl?.Product.AddProduct(new BO.Product() { ID = _id, Name = _Name, Category = (BO.Enums.ECategory)_Cat, Price = _Price, InStock = _InStock });
                    MessageBox.Show("the product " + _Name + " add");
                    this.Close();
                    
                }
                else
                {
                    bl?.Product.UpdateProduct(new BO.Product() { ID = _id, Name = _Name, Category = (BO.Enums.ECategory)_Cat, Price = _Price, InStock = _InStock });
                    MessageBox.Show("the product " + _Name + " update");
                    this.Close();
                }
            }
            catch(ProductAlreadyExistsException p) 
            {
                Label ProductAlreadyExistsLable = new Label()
                {
                    Name = "ProductAlreadyExistsLabel",
                    Margin = new Thickness(290, 105, 0, 0),
                    Content = p.Message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = new SolidColorBrush(Colors.Red),
                };
                Grid.SetRow(ProductAlreadyExistsLable,1 );
                MainGrid.Children.Add(ProductAlreadyExistsLable);
            }
            catch(NegativeIdException p)
            {
                Label NegativeIdExceptionLable = new Label()
                {
                    Name = "NegativeIdExceptionLable",
                    Margin = new Thickness(290, 105, 0, 0),
                    Content = p.Message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = new SolidColorBrush(Colors.Red),
                };
                Grid.SetRow(NegativeIdExceptionLable, 1);
                MainGrid.Children.Add(NegativeIdExceptionLable);
            }
            catch (EmptyNameException p)
            {
                Label EmptyNameExceptionLable = new Label()
                {
                    Name = "EmptyNameExceptionLable",
                    Margin = new Thickness(288, 157, 0, 0),
                    Content = p.Message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = new SolidColorBrush(Colors.Red),
                };
                Grid.SetRow(EmptyNameExceptionLable, 1);
                MainGrid.Children.Add(EmptyNameExceptionLable);
            }
            catch (NegativePriceException p)
            {
                Label NegativePriceExceptionLable = new Label()
                {
                    Name = "NegativePriceExceptionLabel",
                    Margin = new Thickness(299, 246, 0, 0),
                    Content = p.Message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = new SolidColorBrush(Colors.Red),
                };
                Grid.SetRow(NegativePriceExceptionLable, 1);
                MainGrid.Children.Add(NegativePriceExceptionLable);
            }
            catch (NegativeStockException p)
            {
                Label NegativeStockExceptionLable = new Label()
                {
                    Name = "NegativeStockExceptionLable",
                    Margin = new Thickness(299, 288, 0, 0),
                    Content = p.Message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = new SolidColorBrush(Colors.Red),
                };
                Grid.SetRow(NegativeStockExceptionLable, 1);
                MainGrid.Children.Add(NegativeStockExceptionLable);
            }



        }

        private void id_TextChanged(object sender, TextChangedEventArgs e)
        {

            var child = MainGrid.Children.OfType<Control>().Where(x => x.Name == "NegativeIdExceptionLable"||x.Name== "ProductAlreadyExistsLabel").FirstOrDefault();
            if (child != null)
                MainGrid.Children.Remove(child);
        }
        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            var child = MainGrid.Children.OfType<Control>().Where(x => x.Name == "EmptyNameExceptionLable").FirstOrDefault();
            if (child != null)
                MainGrid.Children.Remove(child);
        }

        private void price_TextChanged(object sender, TextChangedEventArgs e)
        {
            var child = MainGrid.Children.OfType<Control>().Where(x => x.Name == "NegativePriceExceptionLabel").FirstOrDefault();
            if (child != null)
                MainGrid.Children.Remove(child);
        }

       
        private void inStock_TextChanged(object sender, TextChangedEventArgs e)
        {
            var child = MainGrid.Children.OfType<Control>().Where(x => x.Name == "NegativeStockExceptionLable").FirstOrDefault();
            if (child != null)
                MainGrid.Children.Remove(child);

        }
    }
}
