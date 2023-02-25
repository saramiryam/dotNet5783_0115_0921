﻿using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation
{
    sealed public class Bl : IBl
    {
        public ICart Cart => new Cart();
        public BlApi.IOrder Order => new Order();
        public BlApi.IProduct Product => new Product();
    }
}
