﻿using DalApi;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    sealed internal class DalXml:IDal
    {
        #region singelton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }// static ctor to ensure instance init is done just before first usage
        DalXml() { } // default => private
        public static DalXml Instance { get => instance; }
        #endregion
       


        public IProduct Product { get; } = new Dal.XmlProduct();
        public IOrder Order { get; } = new Dal.XmlOrder();
        public IOrderItem OrderItem { get; } = new Dal.XmlOrderItem();
    }
}
