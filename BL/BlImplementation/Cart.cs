﻿using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class Cart: ICart
    {
        private IDal Dal = new Dal.DalList();
    }
}
