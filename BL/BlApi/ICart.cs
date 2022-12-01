using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface ICart
    {
        //הוספה מהקטלוג וממסך פרטי מוצר
        public BO.Cart? AddItemToCart(BO.Cart cart,int itemId);
        public BO.Cart? UpdateAmount(BO.Cart cart,int itemId,int amount);
        public void SubmitOrder (BO.Cart cart, string name,string email,string adress); 
        
    }
}
