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
using System.Text.RegularExpressions;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for MProductWindow.xaml
    /// </summary>
    public partial class MProductWindow : Window
    {

        #region prorerties
        BlApi.IBl? bl = BlApi.Factory.Get();
        public static string MyContent { get; set; } = "add";
        public BO.Product ProductToUpOrAdd
        {
            get { return (BO.Product)GetValue(ProductToUpOrAddProperty); }
            set { SetValue(ProductToUpOrAddProperty, value); }
        }
        public static readonly DependencyProperty ProductToUpOrAddProperty = DependencyProperty.Register(nameof(ProductToUpOrAdd),
                                                                                                               typeof(BO.Product),
                                                                                                       typeof(MProductWindow));

        public string ExceText
        {
            get { return (string)GetValue(ExceTextProperty); }
            set { SetValue(ExceTextProperty, value); }
        }
        public static readonly DependencyProperty ExceTextProperty = DependencyProperty.Register(nameof(ExceText),
                                                                                                               typeof(string),
                                                                                                       typeof(MProductWindow));

        public Thickness MyMargin
        {
            get { return (Thickness)GetValue(MyMarginProperty); }
            set { SetValue(MyMarginProperty, value); }
        }
        public static readonly DependencyProperty MyMarginProperty = DependencyProperty.Register(nameof(MyMargin),
                                                                                                               typeof(Thickness),
                                                                                                       typeof(MProductWindow));
        public static System.Array Categories { get; set; } = Enum.GetValues(typeof(Enums.ECategory));
        private Action<ProductForList> Action;

        #endregion

        public MProductWindow(Action<ProductForList> Action)
        {
            ProductToUpOrAdd = new();
            MyContent = "add";
            InitializeComponent();
            this.Action = Action;
        }

        public MProductWindow(int idToUpdate)
        {
            try
            {
                ProductToUpOrAdd = bl.Product.GetProductDetails(idToUpdate);
            }
            catch (ProductNotExistsException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            catch (NegativeIdException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


            MyContent = "update";
            InitializeComponent();



        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new MProductListWindow().Show();
            Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = 0;
                if (MyContent == "add")
                {
                    try
                    {
                        id = bl.Product.AddProduct(ProductToUpOrAdd);
                    }
                    catch (ProductAlreadyExistsException ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    Action(bl.Product.GetProductForList(id));
                    MessageBox.Show("the product " + ProductToUpOrAdd.Name + " " + MyContent);
                    //  new MProductListWindow().Show();
                    this.Close();

                }
                else
                {
                    bl?.Product.UpdateProduct(ProductToUpOrAdd);
                    MessageBox.Show("the product " + ProductToUpOrAdd.Name + " " + MyContent);
                    this.Close();
                }
            }
            catch (ProductAlreadyExistsException p)
            {
                ExceText = p.Message;
                MyMargin = new Thickness(290, 105, 0, 0);

            }
            catch (NegativeIdException p)
            {
                ExceText = p.Message;
                MyMargin = new Thickness(290, 105, 0, 0);
            }
            catch (EmptyNameException p)
            {
                ExceText = p.Message;
                MyMargin = new Thickness(288, 157, 0, 0);
            }
            catch (GetEmptyCateporyException p)
            {
                ExceText = p.Message;
                MyMargin = new Thickness(288, 214, 0, 0);
            }
            catch (NegativePriceException p)
            {
                ExceText = p.Message;
                MyMargin = new Thickness(299, 246, 0, 0);
            }
            catch (NegativeStockException p)
            {
                ExceText = p.Message;
                MyMargin = new Thickness(299, 288, 0, 0);
            }



        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void TextValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Zא-ת]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #region text changed

        private void id_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExceText = "";
        }
        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExceText = "";
        }
        private void price_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExceText = "";
        }
        private void inStock_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExceText = "";
        }
        private void chooseCategoryToAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExceText = "";
        }
        #endregion

    }
}
