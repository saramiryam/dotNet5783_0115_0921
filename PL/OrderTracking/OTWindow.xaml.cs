using PL.Admin.Order;
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

namespace PL.OrderTracking
{
    /// <summary>
    /// Interaction logic for OTWindow.xaml
    /// </summary>
    public partial class OTWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public static readonly DependencyProperty MyIdProperty = DependencyProperty.Register(nameof(MyId),
                                                                                             typeof(int),
                                                                                     typeof(OTWindow));
        public int MyId
        {
            get { return (int)GetValue(MyIdProperty); }
            set { SetValue(MyIdProperty, value); }
        }
        public OTWindow()
        {
            InitializeComponent();
        }

        //private void IdChange_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if((MyId>199999) && (MyId < 1000000))
        //    {
        //        CanGet=true;
        //    }
        //}

        private void getButton_Click(object sender, RoutedEventArgs e)
        {
            new OOrderTracking(MyId).Show();
            Close();
            
            
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
