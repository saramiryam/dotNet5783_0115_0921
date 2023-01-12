﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlImplementation;
using BlApi;
using PL.Admin.Order;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MWindow : Window
    {
        public MWindow()
        {
            InitializeComponent();


        }
        BlApi.IBl? bl = BlApi.Factory.Get();


        private void productList_Click(object sender, RoutedEventArgs e)
        {
            new MProductListWindow().Show();
            this.Close();
        }

        private void orderList_Click(object sender, RoutedEventArgs e)
        {
            new MOrderListWindow().Show();
            this.Close();
        }

        private void backToMain_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
