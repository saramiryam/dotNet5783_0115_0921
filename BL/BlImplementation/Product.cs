using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace BlImplementation
{

    public class Product : BlApi.IProduct
    {

        private IDal Dal = new Dal.DalList();

        #region Methodes

        public IEnumerable<BO.ProductForList> GetListOfProduct()
        {
            IEnumerable<DO.Product?> productsList = new List<DO.Product?>();
            List<BO.ProductForList> productsForList = new List<BO.ProductForList>();
            productsList = Dal.Product.GetAll();
            foreach (var item in productsList)
            {
                if ((item != null) && (item.Value.Category != null))
                {
                    productsForList.Add(new BO.ProductForList()
                    {
                        ID = item.Value.ID,
                        Name = item.Value.Name,
                        Price = item.Value.Price,
                        Category = (BO.Enums.ECategory)item.Value.Category
                    });
                }
            }
            return productsForList;
        }
        //public IEnumerable<BO.ProductForList> GetListOfProduct(Func<Product?, bool>? predict = null)
        //{
        //    IEnumerable<DO.Product?> productsList ;
        //    List<BO.ProductForList> productsForList = new List<BO.ProductForList>();
        //    if(predict != null)
        //    {
        //        productsList = Dal.Product.GetAll(e=>predict(e));
        //    }
        //    productsList = Dal.Product.GetAll();
        //    foreach (var item in productsList)
        //    {
        //        if ((item != null) && (item.Value.Category != null))
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
        //    return productsForList;
        //}

        public IEnumerable<BO.ProductForList> GetProductForListByCategory(BO.Enums.ECategory category)
        {
            IEnumerable<DO.Product?> productsList = new List<DO.Product?>();
            List<BO.ProductForList> productsForList = new List<BO.ProductForList>();
            productsList = Dal.Product.GetAll();
            foreach (var item in productsList)
            {
                if ((item != null) && (item.Value.Category != null))
                {
                    if (item.Value.Category.ToString()== category.ToString())
                    {
                        productsForList.Add(new BO.ProductForList()
                        {
                            ID = item.Value.ID,
                            Name = item.Value.Name,
                            Price = item.Value.Price,
                            Category = (BO.Enums.ECategory)item.Value.Category
                        });
                    }
                }

            }
            return productsForList;
        }

        public BO.Product GetProductItem(int id)
        {
            if (id <= 0)
            {
                throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };
            }
            else
            {
                DO.Product p = new DO.Product();
                try
                {
                    p = Dal.Product.Get(e => e?.ID == id);
                }
                catch
                {
                    throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };

                }
                BO.Product p1 = new BO.Product();

                p1 = DOToBO(p);
                return p1;

            }
        }

        public BO.ProductItem GetProductItemForCatalog(int id, BO.Cart CostumerCart)
        {
            if (id <= 0)
            {
                throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };
            }
            else
            {
                DO.Product p = new DO.Product();
                try
                {
                    p = Dal.Product.Get(e => e?.ID == id);
                }
                catch
                {
                    throw new ProductNotInStockException("product not exsist") { ProductNotInStock = id.ToString() };

                }
                if (p.Category is not null)
                {
                    if (CostumerCart.ItemList == null) throw new ItemInCartNotExistsAsProductException("item list not exsist") { ItemInCartNotExistsAsProduct = p.ToString() };
                    BO.ProductItem PI = new BO.ProductItem()
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Category = (BO.Enums.ECategory)p.Category,
                        Price = p.Price,
                        InStock = p.InStock,
                        AmoutInYourCart = CostumerCart.ItemList.FindAll(e => e?.ID == id).Count()
                    };
                    return PI;
                }
                else
                {
                    throw new GetEmptyCateporyException("the product category is empty") { GetEmptyCatepory = null };
                }
            }
        }

        public void AddProduct(BO.Product p)
        {
            //לדאוג שהפונקציה מתחת תבדוק גם את תקינות הקטגוריה
            //להיזהר לר למחוק כדי שאם הוא לא יכיר את את הבדיקה נוכל להחזיר

            CheckCorectData(p.ID, p.Name, p.Price, p.InStock);
            try
            {
                if (p.Category != null)
                {
                    Dal.Product.Add(new DO.Product()
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Price = p.Price,
                        Category = (DO.Enums.ECategory)p.Category,
                        InStock = p.InStock

                    });
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

        public void AddProductFromWindow(int id, string name, string category, double price, int inStock, string action)
        {

            BO.Product newProduct = new() { ID = id, Name = name, Category = GetCategory(category), Price = price, InStock = inStock };
            if (action == "add")
                AddProduct(newProduct);
            else
                UpdateProduct(newProduct);
        }

        public void UpdateProduct(BO.Product item)
        {

            CheckCorectData(item.ID, item.Name, item.Price, item.InStock);
            try
            {
                if(item.Category is null||item.Name is null) throw new GetEmptyCateporyException("the product category is empty") { GetEmptyCatepory = null };

                Dal.Product.Update(newProductWithData(item.ID, item.Name, (BO.Enums.ECategory)item.Category, item.Price, item.InStock));
            }
            catch (DO.RequestedItemNotFoundException)
            {
                throw new BO.ProductNotExistsException("product not exists") { ProductNotExists = item.ToString() };

            }

        }

        public void DeleteProduct(int id)
        {
            IEnumerable<DO.OrderItem?> orderList = new List<DO.OrderItem?>();
            orderList = Dal.OrderItem.GetAll();
            bool flag = false;
            foreach (var OI in orderList)
            {
                if (OI != null && OI.Value.ProductID == id)
                {
                    flag = true;
                }
            }
            if (flag)
            {
                throw new BO.ProductInUseException("product in use") { ProductInUse = id.ToString() };
            }
            try
            {
                Dal.Product.Delete(id);
            }
            catch
            {
                throw new BO.ProductNotExistsException("product not exists") { ProductNotExists = id.ToString() };
            }

        }





        #endregion
        #region help methodes

        #region DO to BO
        private BO.Product DOToBO(DO.Product p)
        {
            if (p.Category != null)
            {
                BO.Product p1 = new BO.Product()
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
        public void CheckCorectData(int id, string? name, double price, int inStock)
        {
            if (id < 0)
            {
                throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };
            }
            if (name is null)
            {
                throw new BO.EmptyNameException("empty name") { EmptyName = null };
            }
            if (price <= 0)
            {
                throw new BO.NegativePriceException("empty name") { NegativePrice = price.ToString() };
            }
            if (inStock < 0)
            {
                throw new BO.NegativeStockException("empty name") { NegativeStock = inStock.ToString() };
            }
            return;

        }
        #endregion

        #region new product with data

        private static DO.Product newProductWithData(int id, string name, BO.Enums.ECategory category, double price, int inStock)
        {
            DO.Product p = new DO.Product();
            p.ID = id;
            p.Name = name;
            p.Category = (DO.Enums.ECategory)category;
            p.Price = price;
            p.InStock = inStock;
            return p;
        }

        #endregion

        #region GetCategory
        private static BO.Enums.ECategory GetCategory(string category)
        {
            switch (category)
            {
                case "Notebooks":
                    return BO.Enums.ECategory.Notebooks;
                case "Pens":
                    return BO.Enums.ECategory.Pens;
                case "Diaries":
                    return BO.Enums.ECategory.Diaries;
                case "ArtMaterials":
                    return BO.Enums.ECategory.ArtMaterials;
                case "Games":
                    return BO.Enums.ECategory.Games;

            }
            return BO.Enums.ECategory.Games;
        }

        #endregion

        #endregion




    }


}




