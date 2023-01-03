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

namespace BlImplementation
{
    internal class Cart : BlApi.ICart
    {
        private static IDal? Dal = Factory.Get();

        #region methodes

        /// <summary>
        /// get an item an try to add it to the cart
        /// </summary>
        /// <param name="cart">add to this cart an item</param>
        /// <param name="itemId">id of item to add</param>
        /// <returns>the update cart</returns>
        /// <exception cref="BO.NotEnoughInStockException">add item thet has not enough in stock</exception>
        /// <exception cref="BO.ProductNotInStockException">product not in stock</exception>
        /// <exception cref="BO.ProductNotExistsException">product not exists with this id</exception>
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
                    //BO.OrderItem BOI = cart.ItemList.Find(e => e?.ID == itemId) ?? new BO.OrderItem();
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
                        if (Dal != null)
                        {
                            DP = Dal.Product.Get(e => e?.ID == itemId);
                        }
                        else
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

        public BO.Cart UpdateAmount(BO.Cart cart, int itemId, int amount)
        {
            if (cart.ItemList == null) throw new ItemNotInCartException("item list not exsist") { ItemNotInCart = cart.ToString() };
            var exist = cart.ItemList
                       .Where(e => e?.ID == itemId)
                       .Select(e =>(BO.OrderItem?) e).FirstOrDefault();
               // cart.ItemList.Exists(e => e?.ID == itemId);
            if (exist is null)
            {
                throw new BO.ItemNotInCartException("item not in cart") { ItemNotInCart = itemId.ToString() };
            }
            // if(cart.ItemList.Find(e => e?.ID == itemId) is  null) throw new ItemNotInCartException("item list not exsist") { ItemNotInCart = cart.ToString() };
            if (itemId == 0) throw new NegativeIdException("negative id") { NegativeId = itemId.ToString() };
            Predicate<BO.OrderItem?> match = e => e?.ID == itemId;
            if (match is null) throw new ItemNotInCartException("item list is empty") { ItemNotInCart = null };
            //BO.OrderItem BOI = cart.ItemList.Find(match) ?? new BO.OrderItem();
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
            //BOI.Amount== amount
            //amount didnt change
            return cart;

        }

        public void SubmitOrder(BO.Cart cart, string name, string email, string adress)
        {
            #region check correct data

            if (name is null)
            {
                throw new BO.NameIsNullException("name is null") { NameIsNull = cart.ToString() };
            }
            if (adress is null)
            {
                throw new BO.AdressIsNullException("adress is null") { AdressIsNull = cart.ToString() };
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
                        //foreach (var item in cart.ItemList)
                        //{
                        //    if (item != null)
                        //    {
                        //        Dal.OrderItem.Add(new DO.OrderItem()
                        //        {
                        //            ID = 0,
                        //            ProductID = item.ID,
                        //            OrderID = orderID,
                        //            Price = item.Price,
                        //            Amount = item.Amount
                        //        });
                        //    }

                        //}

                        var addOrderItem = cart.ItemList
                                          .Where(cAdd => cAdd != null)
                                          .Select(cAdd => Dal.OrderItem.Add(new DO.OrderItem()
                                          {
                                              ID = 0,
                                              ProductID = cAdd!.ID,
                                              OrderID = orderID,
                                              Price = cAdd!.Price,
                                              Amount = cAdd!.Amount
                                          }));
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
