using BlApi;
using DalApi;
using DO;
using System.Net.Mail;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using BO;
using DalList;
using Factory = DalApi.Factory;
using System.Resources;
using Dal;
using System.Runtime.CompilerServices;

namespace BlImplementation
{
    internal class Cart : BlApi.ICart
    {
        private static IDal? Dal = Factory.Get();

        #region methodes

        /// <summary>
        /// get an item and try to add it to the cart
        /// </summary>
        /// <param name="cart">add to this cart an item</param>
        /// <param name="itemId">id of item to add</param>
        /// <returns>the update cart</returns>
        /// <exception cref="BO.NotEnoughInStockException">add item thet has not enough in stock</exception>
        /// <exception cref="BO.ProductNotInStockException">product not in stock</exception>
        /// <exception cref="BO.ProductNotExistsException">product not exists with this id</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Cart AddItemToCart(BO.Cart cart, int itemId)
        {
            if (cart == null)
            {
                throw new BO.SendEmptyCartException("try to add to an empty cart") { SendEmptyCart = itemId.ToString() };
            }
            else
            {
                if (cart.ItemList == null) throw new ItemNotInCartException("item list not exsist") { ItemNotInCart = cart.ToString() };
                var exist = cart.ItemList
                    .Where(e => e?.ID == itemId)
                    .Select(e => (BO.OrderItem?)e).FirstOrDefault();
                if (exist is not null)
                {
                    var BOI = cart.ItemList
                        .Where(e => e?.ID == itemId)
                        .Select(e => (BO.OrderItem?)e!).FirstOrDefault();
                    if (BOI != null)
                    {
                        DO.Product DP;
                        if (Dal != null)
                        {
                            DP = Dal.Product.Get(e => e?.ID == itemId);
                        }
                        else
                        {
                            throw new BO.GetDulNullException("product not exists") { GetDulNull = itemId.ToString() };
                        }

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
                        return cart;
                    }
                }
                else
                {
                    try
                    {
                        DO.Product DP;
                        try
                        {
                            DP = Dal.Product.Get(e => e?.ID == itemId);
                        }
                        catch
                        {
                            throw new BO.GetDulNullException("product not exists") { GetDulNull = itemId.ToString() };
                        }
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
        }



        /// <summary>
        /// get an item and try to change the amout if it correct
        /// </summary>
        /// <param name="cart">the cart we want to change</param>
        /// <param name="itemId">the item`s id we want to change</param>
        /// <param name="amount">the amout to change</param>
        /// <returns>the cart after update</returns>
        /// <exception cref="ItemNotInCartException">Item Not In Cart</exception>
        /// <exception cref="BO.ItemNotInCartException">Item Not In Cart</exception>
        /// <exception cref="NegativeIdException">Negative Id</exception>
        /// <exception cref="BO.NegativeAmountException">Negative Amount</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Cart UpdateAmount(BO.Cart cart, int itemId, int amount)
        {
            if (cart.ItemList == null) throw new ItemNotInCartException("item list not exsist") { ItemNotInCart = cart.ToString() };
            var f = cart.ItemList;
            var exist = cart.ItemList
                       .Where(e => e?.ID == itemId)
                       .Select(e => (BO.OrderItem?)e).FirstOrDefault();
            if (exist is null)
            {
                throw new BO.ItemNotInCartException("item not in cart") { ItemNotInCart = itemId.ToString() };
            }
            if (itemId == 0) throw new NegativeIdException("negative id") { NegativeId = itemId.ToString() };
            Predicate<BO.OrderItem?> match = e => e?.ID == itemId;
            if (match is null) throw new ItemNotInCartException("item list is empty") { ItemNotInCart = null };
            var BOI = cart.ItemList
                  .Where(e => e?.ID == itemId)
                  .Select(e => (BO.OrderItem?)e!).FirstOrDefault();
            if (BOI == null)
            {
                return cart;
            }
            if (amount < 0)
            {
                throw new BO.NegativeAmountException("negative amount") { NegativeAmount = itemId.ToString() };
            }
            else if (amount == 0)
            {
                cart.TotalSum -= BOI.sumItem;
                cart.ItemList.RemoveAll(e => e?.ID == itemId);
            }
            else if (BOI.Amount < amount)
            {
                var num = amount - BOI.Amount;
                for (int i = 0; i < num; i++)
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
            return cart;

        }



        /// <summary>
        /// submit a costemor with his cart and details
        /// </summary>
        /// <param name="cart">the costemor cart</param>
        /// <param name="name">the costemor name</param>
        /// <param name="email">the costemor email</param>
        /// <param name="adress">the costemor adress</param>
        /// <exception cref="BO.NameIsNullException">Name Is Null</exception>
        /// <exception cref="BO.AdressIsNullException">Adress Is Null</exception>
        /// <exception cref="BO.GetDulNullException">Get Dul Null</exception>
        /// <exception cref="BO.NegativeAmountException">Negative Amount</exception>
        /// <exception cref="BO.NotEnoughInStockException">Not Enough In Stock</exception>
        /// <exception cref="BO.ItemInCartNotExistsAsProductException">Item In Cart Not Exists As Product</exception>
        /// <exception cref="BO.ItemAlreadyExistsException">Item Already Exists</exception>
        /// <exception cref="BO.OrderNotExistsException">Order No tExists</exception>
        /// <exception cref="BO.FieldToGetProductException">Field To Get Product</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SubmitOrder(BO.Cart cart, string name, string email, string adress)
        {
            #region check correct data

            if (name is null)
            {
                throw new BO.NameIsNullException(" enter name ") { NameIsNull = cart.ToString() };
            }
            if (adress is null)
            {
                throw new BO.AdressIsNullException("enter address ") { AdressIsNull = cart.ToString() };
            }
            checkEmail(email);
            if (cart.ItemList != null)
            {
                foreach (var item in cart.ItemList)
                {
                    if (item != null)
                    {
                        try
                        {
                            DO.Product DP;
                            if (Dal != null)
                            {
                                DP = Dal.Product.Get(e => e?.ID == item.ID);
                            }
                            else
                            {
                                throw new BO.GetDulNullException("negative amount") { GetDulNull = item.Amount.ToString() };
                            }

                            if (item.Amount < 0)
                            {
                                throw new BO.NegativeAmountException("negative amount") { NegativeAmount = item.Amount.ToString() };
                            }
                            if (item.Amount > DP.InStock)
                            {
                                throw new BO.NotEnoughInStockException("Not enough in stock") { NotEnoughInStock = item.Amount.ToString() };
                            }

                        }
                        catch (DO.RequestedItemNotFoundException)
                        {
                            throw new BO.ItemInCartNotExistsAsProductException("item in cart not exists as product") { ItemInCartNotExistsAsProduct = item.ToString() };
                        }
                    }

                }
            }


            #endregion


            DO.Order o = new DO.Order()
            {
                ID = 0,
                CustomerName = name,
                CustomerAdress = adress,
                CustomerEmail = email,
                OrderDate = DateTime.Now,
                ShipDate = null,
                DeliveryDate = null,
            };
            try
            {
                int orderID;
                if (Dal != null)
                {
                    orderID = Dal.Order.Add(o);
                }
                else
                {
                    throw new BO.GetDulNullException("item in cart not exists as product") { GetDulNull = null };
                }

                if (cart.ItemList != null)
                {
                    try
                    {
                        var addOrderItem = cart.ItemList
                                          .Where(cAdd => cAdd != null)
                                          .Select(cAdd => Dal.OrderItem.Add(new DO.OrderItem()
                                          {
                                              ID = 0,
                                              ProductID = cAdd!.ID,
                                              OrderID = orderID,
                                              Price = cAdd!.Price,
                                              Amount = cAdd!.Amount
                                          })).ToList();
                    }
                    catch (DO.ItemAlreadyExistsException)
                    {
                        throw new BO.ItemAlreadyExistsException("item aleardy exists") { ItemAlreadyExists = o.ToString() };
                    }
                }

            }
            catch (DO.RequestedItemNotFoundException)
            {
                throw new BO.OrderNotExistsException("order not exists")
                {
                    OrderNotExists = o.ToString()
                };
            }
            try
            {
                if (cart.ItemList != null)
                {

                }

            }
            catch (DO.RequestedItemNotFoundException)
            {

                throw new BO.FieldToGetProductException("order not exists") { FieldToGetProduct = o.ToString() };
            }

        }


        #endregion
        #region help methodes
        /// <summary>
        /// check if the email entered is valid
        /// </summary>
        /// <param name="email">the email to check</param>
        /// <exception cref="BO.UncorrectEmailException">Uncorrect Email</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
