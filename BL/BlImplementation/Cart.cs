using BlApi;
using Dal;
using DalApi;
using DO;
using System.Runtime.Intrinsics.Arm;

namespace BlImplementation
{
    internal class Cart: BlApi.ICart
    {
        private IDal Dal = new Dal.DalList();

        #region methodes

        public BO.Cart AddItemToCart(BO.Cart cart, int itemId)
        {
           bool exist = cart.ItemList.Exists(e => e.ID == itemId);
            if (exist)
            {
                BO.OrderItem BOI=cart.ItemList.Find(e => e.ID == itemId);
                DO.Product DP= Dal.Product.Get(itemId);
                if (BOI.Amount < DP.InStock)
                {
                    BOI.Amount++;
                    BOI.sumItem+=BOI.Price;
                    cart.TotalSum += BOI.Price;
                }
                else
                {
                throw new BO.NotEnoughInStockException("Not enough in stock") { NotEnoughInStock = itemId.ToString() };
                }
            }
            else
            {
                try
                {
                    DO.Product DP = Dal.Product.Get(itemId);
                    if (DP.InStock>0)
                    {

                    }
                    else
                    {
                    throw new BO.ProductNotInStockException("product not in stock") { ProductNotInStock = itemId.ToString() };
                    }

                }
                catch (RequestedItemNotFoundException)
                {
                    throw new BO.ProductNotExistsException("product not exists") { ProductNotExists = itemId.ToString() };
                }
                


            }

        }
        public BO.Cart UpdateAmount(BO.Cart cart, int itemId, int amount)
        {

        }
        public void SubmitOrder(BO.Cart cart, string name, string email, string adress)
        {

        }

        #endregion
    }
}
