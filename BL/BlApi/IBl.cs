using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBl
    {
        ICart Cart { get; }
        IOdert Order { get; }
        IOrderForList OrderForList { get; }
        IOrderItem OrderItem { get; }
        IOrderTracking OrderTracking { get; }
        IProduct Product{ get; }
        IProductForList ProductForList { get; }
        IProductItem ProductItem { get; }
        
    }
}
