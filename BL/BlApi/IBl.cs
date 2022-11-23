using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBl
    {
        public ICart Cart { get; }
        public IOrder Order { get; }
        public IOrderForList OrderForList { get; }
        public IOrderItem OrderItem { get; }
        public IOrderTracking OrderTracking { get; }
        public IProduct Product{ get; }
        public IProductForList ProductForList { get; }
        public IProductItem ProductItem { get; }
        
    }
}
