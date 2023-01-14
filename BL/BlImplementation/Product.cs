using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Factory = DalApi.Factory;

namespace BlImplementation
{

    public class Product : BlApi.IProduct
    {
        //נראה שלא עושה את העבודה נכון
        //להשוות עם מישהי
        private static IDal? Dal = Factory.Get();

        #region Methodes

        public IEnumerable<BO.ProductForList> GetListOfProduct()
        {
            IEnumerable<DO.Product?> productsList = new List<DO.Product?>();
            // List<BO.ProductForList> productsForList = new List<BO.ProductForList>();
            if (Dal != null)
            {
                productsList = Dal.Product.GetAll();
            }

            var addOrderItem = productsList
                               .Where(item => (item is not null) && (item.Value.Category is not null))
                               .OrderBy(item => item.Value.Name)
                               .Select(item => new BO.ProductForList()
                               {
                                   ID = item!.Value.ID,
                                   Name = item.Value.Name,
                                   Price = item.Value.Price,
                                   Category = (BO.Enums.ECategory)item.Value.Category
                               });
            return addOrderItem;
        }
        public BO.ProductForList? GetProductForList(int id)
        {
            IEnumerable<DO.Product?> productsList = new List<DO.Product?>();
            if (Dal != null)
            {
                productsList = Dal.Product.GetAll();
            }
            return productsList
             .Where(item => (item != null) && (item.Value.ID == id))
             .Select(item => new BO.ProductForList()
             {
                 ID = item!.Value.ID,
                 Name = item.Value.Name,
                 Price = item.Value.Price,
                 Category = (BO.Enums.ECategory)item.Value.Category
             }).FirstOrDefault();

        }
        public IEnumerable<BO.ProductForList> GetProductForListByCategory(BO.Enums.ECategory category)
        {
            IEnumerable<DO.Product?> productsList = new List<DO.Product?>();
            //List<BO.ProductForList> productsForList = new List<BO.ProductForList>();
            if (Dal != null)
            {
                productsList = Dal.Product.GetAll();
            }
            //foreach (var item in productsList)
            //{
            //    if ((item != null) && (item.Value.Category != null))
            //    {
            //        if (item.Value.Category.ToString() == category.ToString())
            //        {
            //            productsForList.Add(new BO.ProductForList()
            //            {
            //                ID = item.Value.ID,
            //                Name = item.Value.Name,
            //                Price = item.Value.Price,
            //                Category = (BO.Enums.ECategory)item.Value.Category
            //            });
            //        }
            //    }

            //}
            //return productsForList;
            var listByCategory = productsList
                       .Where(item => (item != null) && (item.Value.Category != null) && (item.Value.Category.ToString() == category.ToString()))
                       .Select(item => new BO.ProductForList()
                       {
                           ID = item!.Value.ID,
                           Name = item.Value.Name,
                           Price = item.Value.Price,
                           Category = (BO.Enums.ECategory)item.Value.Category
                       });
            return listByCategory;
        }
        public BO.Product GetProductDetails(int id)
        {
            if (id <= 0)
            {
                throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };
            }
            else
            {
                DO.Product p = new();
                try
                {
                    if (Dal != null)
                    {
                        p = Dal.Product.Get(e => e?.ID == id);
                    }
                }
                catch
                {
                    throw new BO.ProductNotExistsException("negative id") { ProductNotExists = id.ToString() };

                }
                BO.Product p1 = new();

                p1 = DOToBO(p);
                return p1;

            }
        }
        public BO.ProductItem? GetProductItemDetails(BO.Cart MyCart,int id)
        {
            if (id <= 0)
            {
                throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };
            }
            IEnumerable<DO.Product?> productsList = new List<DO.Product?>();
            IEnumerable<BO.OrderItem?> orderItemList = new List<BO.OrderItem?>();
            if (Dal != null)
            {
                productsList = Dal.Product.GetAll();
            }
            orderItemList = MyCart.ItemList;

           
                var p = productsList
                   .Where(product => product is not null && product.Value.ID == id)
                   .Select(p => new BO.ProductItem()
                   {
                       ID = p.Value.ID,
                       Name = p?.Name,
                       Category = (BO.Enums.ECategory)p?.Category,
                       Price = p.Value.Price,
                       InStock = p.Value.InStock,
                       AmoutInYourCart = (from item in orderItemList
                                          where item?.ID == p.Value.ID
                                          select (item.Amount)).FirstOrDefault()


                   }).First();
            
            return p;
        }
        public IEnumerable<BO.ProductItem?> GetProductItemList( Func<DO.Product?, bool>? predict = null)
        {
            IEnumerable<DO.Product?> productsList = new List<DO.Product?>();
            IEnumerable<DO.OrderItem?> orderItemList = new List<DO.OrderItem?>();
            if (Dal != null)
            {
                productsList = Dal.Product.GetAll();
                orderItemList = Dal.OrderItem.GetAll();
            }
            if (predict == null)
                return productsList
                    .Where(product => product is not null && product.Value.Category is not null)
                    .Select(p => new BO.ProductItem()
                    {
                        ID = p.Value.ID,
                        Name = p?.Name,
                        Category = (BO.Enums.ECategory)p?.Category,
                        Price = p.Value.Price,
                        InStock = p.Value.InStock,
                        //AmoutInYourCart = CostumerCart.ItemList.FindAll(e => e?.ID == id).Count()
                        AmoutInYourCart = ((from item in orderItemList
                                           group item by item?.ID into mygroup
                                           where mygroup.Key == p.Value.ID
                                           select (mygroup.Count())).FirstOrDefault())


                    }).ToList();
            else
                return productsList
               .Where(product => product is not null && product.Value.Category is not null && (predict(product)))
               .Select(p => new BO.ProductItem()
               {
                   ID = p.Value.ID,
                   Name = p?.Name,
                   Category = (BO.Enums.ECategory)p?.Category,
                   Price = p.Value.Price,
                   InStock = p.Value.InStock,
                   AmoutInYourCart = (from item in orderItemList
                                      group item by item?.ID into mygroup
                                      where mygroup.Key == p.Value.ID
                                      select (mygroup.Count())).FirstOrDefault()


               }).ToList();
        }
       
        public BO.ProductItem GetProductItemForCatalog(int id, BO.Cart CostumerCart)
        {
            if (id <= 0)
            {
                throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };
            }
            else
            {
                DO.Product p = new();
                try
                {
                    if (Dal != null)
                    {
                        p = Dal.Product.Get(e => e?.ID == id);
                    }
                }
                catch
                {
                    throw new ProductNotInStockException("product not exsist") { ProductNotInStock = id.ToString() };

                }
                if (p.Category is not null)
                {
                    if (CostumerCart.ItemList == null) throw new ItemInCartNotExistsAsProductException("item list not exsist") { ItemInCartNotExistsAsProduct = p.ToString() };
                    BO.ProductItem PI = new()
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Category = (BO.Enums.ECategory)p.Category,
                        Price = p.Price,
                        InStock = p.InStock,
                        //AmoutInYourCart = CostumerCart.ItemList.FindAll(e => e?.ID == id).Count()
                        AmoutInYourCart = (from item in CostumerCart.ItemList
                                          group item by item.ID into mygroup
                                          where mygroup.Key == id
                                          select (mygroup.Count())).First()
                             
                    };
                    return PI;
                }
                else
                {
                    throw new GetEmptyCateporyException("the product category is empty") { GetEmptyCatepory = null };
                }
            }
        }

        public int AddProduct(BO.Product p)
        {
            //לדאוג שהפונקציה מתחת תבדוק גם את תקינות הקטגוריה
            //להיזהר לר למחוק כדי שאם הוא לא יכיר את את הבדיקה נוכל להחזיר
            int MyID;

            CheckCorectData(p.ID, p.Name,p.Category, p.Price, p.InStock);
            try
            {
                if (p.Category != null)
                {
                    if (Dal != null)
                    {
                       MyID= Dal.Product.Add(new DO.Product()
                        {
                            ID = p.ID,
                            Name = p.Name,
                            Price = p.Price,
                            Category = (DO.Enums.ECategory)p.Category,
                            InStock = p.InStock

                        });
                        return MyID;

                    }
                throw new BO.ProductNotExistsException("product not exists") { ProductNotExists = p.Name.ToString() };


                }
                else
                {
                    throw new GetEmptyCateporyException("the product category is empty") { GetEmptyCatepory = null };
                }

            }
            catch (DO.ItemAlreadyExistsException)
            {
                throw new BO.ProductAlreadyExistsException("product already exists") { ProductAlreadyExists = p.ToString() };

            }
        }

        public void UpdateProduct(BO.Product item)
        {

            CheckCorectData(item.ID, item.Name,item.Category, item.Price, item.InStock);
            try
            {
                if (item.Category is null || item.Name is null)
                    throw new GetEmptyCateporyException("the product category is empty")
                    {
                        GetEmptyCatepory = null
                    };
                if (Dal != null)
                {
                    Dal.Product.Update(NewProductWithData(item.ID, item.Name, (BO.Enums.ECategory)item.Category, item.Price, item.InStock));
                }
            }
            catch (DO.RequestedItemNotFoundException)
            {
                throw new BO.ProductNotExistsException("product not exists") { ProductNotExists = item.ToString() };

            }

        }

        public void DeleteProduct(int id)
        {
            IEnumerable<DO.OrderItem?> orderList = new List<DO.OrderItem?>();
            if (Dal != null)
            {
                orderList = Dal.OrderItem.GetAll();
            }
         
            //foreach (var OI in orderList)
            //{
            //    if (OI != null && OI.Value.ProductID == id)
            //    {
            //        flag = true;
            //    }
            //}
           var flag = orderList
                       .Where(oi => oi?.ProductID == id)
                       .Select(e => (DO.OrderItem?)e).FirstOrDefault();
            if (flag is not null)
            {
                throw new BO.ProductInUseException("product in use") { ProductInUse = id.ToString() };
            }
            try
            {
                if (Dal != null)
                {
                    Dal.Product.Delete(id);
                }
            }
            catch
            {
                throw new BO.ProductNotExistsException("product not exists") { ProductNotExists = id.ToString() };
            }

        }





        #endregion
        #region help methodes

        #region DO to BO
        private static BO.Product DOToBO(DO.Product p)
        {
            if (p.Category != null)
            {
                BO.Product p1 = new()
                {
                    ID = p.ID,
                    Name = p.Name,
                    Price = p.Price,
                    Category = (BO.Enums.ECategory)p.Category,
                    InStock = p.InStock
                };
                return p1;
            }
            else
            {
                throw new GetEmptyCateporyException("the product category is empty") { GetEmptyCatepory = null };
            }

        }
        #endregion

        #region check corect data
        public static void CheckCorectData(int id, string? name, BO.Enums.ECategory? category, double price, int inStock)
        {
            if (id < 100000)
            {
                throw new BO.NegativeIdException("id is too short") { NegativeId = id.ToString() };
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new BO.EmptyNameException("empty name") { EmptyName = null };
            }
            if (string.IsNullOrEmpty(category.ToString()))
            {
                throw new BO.GetEmptyCateporyException("empty category") { GetEmptyCatepory = null };
            }
            if (price <= 0)
            {
                throw new BO.NegativePriceException("negative price") { NegativePrice = price.ToString() };
            }
            if (inStock < 0)
            {
                throw new BO.NegativeStockException("negatuve in stock") { NegativeStock = inStock.ToString() };
            }
            return;

        }
        #endregion

        #region new product with data

        private static DO.Product NewProductWithData(int id, string name, BO.Enums.ECategory category, double price, int inStock)
        {
            DO.Product p = new()
            {
                ID = id,
                Name = name,
                Category = (DO.Enums.ECategory)category,
                Price = price,
                InStock = inStock
            };
            return p;
        }

        #endregion


        #endregion




    }


}




