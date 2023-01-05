﻿using BO;
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
/// Interaction logic for MOrderListWindow.xaml
/// </summary>
public partial class MOrderListWindow : Window
{
    BlApi.IBl? bl = BlApi.Factory.Get();
    public BO.OrderForList? OrderToUp { get; set; } = new();


    public static readonly DependencyProperty OrdersForListListProperty = DependencyProperty.Register(nameof(OrdersForListList),
                                                                                                           typeof(ObservableCollection<OrderForList?>),
                                                                                                   typeof(MOrderListWindow));
    public ObservableCollection<OrderForList?> OrdersForListList
    {
        get { return (ObservableCollection<OrderForList?>)GetValue(OrdersForListListProperty); }
        set { SetValue(OrdersForListListProperty, value); }
    }
    public MOrderListWindow()
    {
        OrdersForListList = new (bl.Order.GetListOfOrders());
        InitializeComponent();
    }


    private void ordersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
    private void ordersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (OrderToUp is not null)
            new MOrderWindow(OrderToUp.OrderID).ShowDialog();
        OrdersForListList = new(bl.Order.GetListOfOrders());
    }
}
