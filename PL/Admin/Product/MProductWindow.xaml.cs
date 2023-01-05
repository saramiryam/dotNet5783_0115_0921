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

        #region prorerties
        BlApi.IBl? bl = BlApi.Factory.Get();
        public static string MyContent { get; set; }="add";
        public BO.Product ProductToUpOrAdd
        {
            get { return (BO.Product)GetValue(ProductToUpOrAddProperty); }
            set { SetValue(ProductToUpOrAddProperty, value); }
        }
        public static readonly DependencyProperty ProductToUpOrAddProperty = DependencyProperty.Register(nameof(ProductToUpOrAdd),
                                                                                                               typeof(BO.Product),
                                                                                                       typeof(MProductWindow));
        public static System.Array Categories { get; set; } = Enum.GetValues(typeof(Enums.ECategory));


        #endregion
        
        public MProductWindow()
        {
            ProductToUpOrAdd = new();
            MyContent = "add";
            InitializeComponent();

        }
        public MProductWindow(int idToUpdate)
        {
            ProductToUpOrAdd = new();

            if (bl != null)
            {
                ProductToUpOrAdd = bl.Product.GetProductItem(idToUpdate);
            }
            MyContent = "update";
            InitializeComponent();
            


        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (addOrUpdateButton.Content.ToString() == "add")
                {
                    bl?.Product.AddProduct(ProductToUpOrAdd);
                        MessageBox.Show("the product " + ProductToUpOrAdd.Name + " add");
                    this.Close();

                }
                else
                {
                    bl?.Product.UpdateProduct(ProductToUpOrAdd);
                    MessageBox.Show("the product " + ProductToUpOrAdd.Name + " update");
                    this.Close();
                }
            }
            catch (ProductAlreadyExistsException p)
            {
                Label ProductAlreadyExistsLable = new()
                {
                    Name = "ProductAlreadyExistsLabel",
                    Margin = new Thickness(290, 105, 0, 0),
                    Content = p.Message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = new SolidColorBrush(Colors.Red),
                };
                Grid.SetRow(ProductAlreadyExistsLable, 1);
                MainGrid.Children.Add(ProductAlreadyExistsLable);
            }
            catch (NegativeIdException p)
            {
                Label NegativeIdExceptionLable = new()
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
                Label EmptyNameExceptionLable = new()
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
            catch (GetEmptyCateporyException p)
            {
                Label EmptyCateporyException = new()
                {
                    Name = "EmptyCateporyException",
                    Margin = new Thickness(288, 214, 0, 0),
                    Content = p.Message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Foreground = new SolidColorBrush(Colors.Red),
                };
                Grid.SetRow(EmptyCateporyException, 1);
                MainGrid.Children.Add(EmptyCateporyException);
            }
            catch (NegativePriceException p)
            {
                Label NegativePriceExceptionLable = new()
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
                Label NegativeStockExceptionLable = new()
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
        #region exceptions

        private void id_TextChanged(object sender, TextChangedEventArgs e)
        {

            var child = MainGrid.Children.OfType<Control>().Where(x => x.Name == "NegativeIdExceptionLable" || x.Name == "ProductAlreadyExistsLabel").FirstOrDefault();
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

        private void chooseCategoryToAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var child = MainGrid.Children.OfType<Control>().Where(x => x.Name == "EmptyCateporyException").FirstOrDefault();
            if (child != null)
                MainGrid.Children.Remove(child);
        }
        #endregion

    }
}
