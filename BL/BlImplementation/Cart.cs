using BlApi;
using Dal;
using DalApi;
using DO;
using System.Runtime.Intrinsics.Arm;

namespace BlImplementation
{
    internal class Cart : BlApi.ICart
    {
        private IDal Dal = new Dal.DalList();

        #region methodes

        public BO.Cart AddItemToCart(BO.Cart cart, int itemId)
        {
            bool exist = cart.ItemList.Exists(e => e.ID == itemId);
            if (exist)
            {
                BO.OrderItem BOI = cart.ItemList.Find(e => e.ID == itemId);
                DO.Product DP = Dal.Product.Get(itemId);
                if (BOI.Amount < DP.InStock)
                {
                    BOI.Amount++;
                    BOI.sumItem += BOI.Price;
                    cart.TotalSum += BOI.Price;
                    return cart;
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
                    if (DP.InStock > 0)
                    {
                        cart.ItemList.Add(new BO.OrderItem()
                        {
                            numInOrder = cart.ItemList.Count + 1,
                            ID = DP.ID,
                            Name = DP.Name,
                            Price = DP.Price,
                            Amount = 1,
                            sumItem = DP.Price

                        });
                        cart.TotalSum += DP.Price;
                        return cart;
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
            bool exist = cart.ItemList.Exists(e => e.ID == itemId);
            if (!exist)
            {
                throw new BO.ItemNotInCartException("item not in cart") { ItemNotInCart = itemId.ToString() };
            }
            BO.OrderItem BOI = cart.ItemList.Find(e => e.ID == itemId);
            if (amount < 0)
            {
                throw new BO.NegativeAmountException("negative amount") { NegativeAmount = itemId.ToString() };
            }
            else if (amount == 0)
            {
                cart.TotalSum -= BOI.sumItem;
                cart.ItemList.RemoveAll(e => e.ID == itemId);
            }
            else if (BOI.Amount < amount)
            {
                for (int i = 0; i < (amount - BOI.Amount); i++)
                {
                    AddItemToCart(cart, itemId);
                }
            }
            else if (BOI.Amount > amount)
            {
                int difference = BOI.Amount - amount;
                BOI.Amount = amount;
                BOI.sumItem -= (difference * BOI.Price);
                cart.TotalSum -= (difference * BOI.Price);
            }
            //BOI.Amount== amount
            //amount didnt change
            return cart;

        }
        public void SubmitOrder(BO.Cart cart, string name, string email, string adress)
        {
            if(name is null)
            {
                throw new BO.NameIsNullException("name is null") { NameIsNull = name.ToString() };
            }
            if (adress is null)
            {
                throw new BO.AdressIsNullException("adress is null") { AdressIsNull = adress.ToString() };
            }
            foreach (var item in cart.ItemList)
            {

            }
           
        }

        #endregion
    }
}
