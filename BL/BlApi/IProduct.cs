using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IProduct
    {
        //  מנהל) ועבור קטלוג ראשי
        public IEnumerable<BO.ProductForList> GetListOfProduct();


        //עבור מנהל וקונה
        public BO.ProductItem GetProductItem(int id);

        //קטלוג קונה



    }
}
