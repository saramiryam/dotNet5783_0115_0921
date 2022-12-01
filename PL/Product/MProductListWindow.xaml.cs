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
        IBl Bl = new Bl();
        public MProductListWindow(IBl b)
        {
            InitializeComponent();
        }
    }
}
