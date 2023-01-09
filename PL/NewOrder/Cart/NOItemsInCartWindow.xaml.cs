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
using System.Windows.Shapes;

namespace PL.NewOrder.ProductItem;

/// <summary>
/// Interaction logic for NPCart.xaml
/// </summary>
public partial class NOItemsInCartWindow : Window
{
    public static readonly DependencyProperty CartProperty = DependencyProperty.Register(nameof(Cart),
                                                                                               typeof(BO.Cart),
                                                                                       typeof(NOItemsInCartWindow));
    public BO.Cart Cart
    {
        get { return (BO.Cart)GetValue(CartProperty); }
        set { SetValue(CartProperty, value); }
    }
    public NOItemsInCartWindow(BO.Cart MyCart)
    {
        Cart = MyCart;
        InitializeComponent();
    }
}
