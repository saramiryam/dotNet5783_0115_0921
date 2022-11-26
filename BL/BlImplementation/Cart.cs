using BlApi;
using Dal;
using DalApi;
using DO;
using System.Net.Mail;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;

namespace BlImplementation
{
    internal class Cart : BlApi.ICart
    {
        private IDal Dal = new Dal.DalList();

        #region methodes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cart">add to this cart an item</param>
        /// <param name="itemId">id of item to add</param>
        /// <returns>the update cart</returns>
        /// <exception cref="BO.NotEnoughInStockException">add item thet has not enough in stock</exception>
        /// <exception cref="BO.ProductNotInStockException">product not in stock</exception>
        /// <exception cref="BO.ProductNotExistsException">product not exists with this id</exception>
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
                catch (DO.RequestedItemNotFoundException)
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
            if (name is null)
            {
                throw new BO.NameIsNullException("name is null") { NameIsNull = name.ToString() };
            }
            if (adress is null)
            {
                throw new BO.AdressIsNullException("adress is null") { AdressIsNull = adress.ToString() };
            }
            checkEmail(email);
            foreach (var item in cart.ItemList)
            {
                try
                {
                    DO.Product DP = Dal.Product.Get(item.ID);
                    if (item.Amount < 0)
                    {
                        throw new BO.NegativeAmountException("negative amount")
                        {
                            NegativeAmount = item.Amount.ToString()
                        };
                    }
                    if (item.Amount > DP.InStock)
                    {
                        throw new BO.NotEnoughInStockException("Not enough in stock") { NotEnoughInStock = item.Amount.ToString() };
                    }

                }
                catch(DO.RequestedItemNotFoundException)
                {
                    throw new BO.ItemInCartNotExistsAsProductException("item in cart not exists as product") { ItemInCartNotExistsAsProduct = item.ToString() };
                }
            }

            DO.Order o = new DO.Order()
            {
                ID = 0,
                CustomerName = name,
                CustomerAdress = adress,
                CustomerEmail = email,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.MinValue,
                DeliveryDate = DateTime.MinValue,
            };
            try
            {

                int orderID = Dal.Order.Add(o);
                try
                {
                    foreach (var item in cart.ItemList)
                    {
                        Dal.OrderItem.Add(new DO.OrderItem()
                        {
                            ID = 0,
                            ProductID = item.ID,
                            OrderID = orderID,
                            Price = item.Price,
                            Amount = item.Amount
                        });

                    }
                }
                catch (DO.ItemAlreadyExistsException)
                {
                    throw new BO.ItemAlreadyExistsException("item aleardy exists") { ItemAlreadyExists = o.ToString() };
                }
            }
            catch (DO.RequestedItemNotFoundException)
            {
                throw new BO.OrderNotExistsException("order not exists") { OrderNotExists = o.ToString() };
            }

        }

        #endregion
        #region help methodes
        private void checkEmail(string email)
        {
            try
            {
                MailAddress emailAddress = new MailAddress(email);
            }
            catch
            {
                throw new BO.UncorrectEmailException("uncorrect email") { UncorrectEmail = email.ToString() };
            }
        }
        #endregion
    }
}
