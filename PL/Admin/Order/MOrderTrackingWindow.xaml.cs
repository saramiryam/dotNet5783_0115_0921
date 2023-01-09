﻿using BO;
using PL.OrderTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.Admin.Order;

/// <summary>
/// Interaction logic for MOrderTrackingWindow.xaml
/// </summary>
public partial class MOrderTrackingWindow : Window
{
    BlApi.IBl? bl = BlApi.Factory.Get();

    public BO.OrderTracking? orderTrackingToUp { get; set; } = new();

    public MOrderTrackingWindow(int id)
    {
        if(bl != null)
        {
            orderTrackingToUp = bl.Order.GetOrderTracking(id);
        }
        InitializeComponent();
    }

    private void id_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

   
}
