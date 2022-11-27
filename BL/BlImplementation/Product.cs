using BlApi;
using DalApi;
using DO;
using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace BlImplementation
{

    public class Product : BlApi.IProduct
    {

        private IDal Dal = new Dal.DalList();


        #region Methodes

        public IEnumerable<BO.ProductForList> GetListOfProduct()
        {
            IEnumerable<DO.Product> productsList = new List<DO.Product>();
            List<BO.ProductForList> productsForList = new List<BO.ProductForList>();
            productsList = Dal.Product.GetAll();
            foreach (var item in productsList)
            {
                productsForList.Add(new BO.ProductForList()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Price = item.Price,
                    Category = (BO.Enums.ECategory)item.Category
                });

            }
            return productsForList;
        }


        //עבור מנהל 
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
                    p = Dal.Product.Get(id);
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

        //קטלוג קונה
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
                    p = Dal.Product.Get(id);
                }
                catch
                {
                    throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };

                }
                BO.ProductItem PI = new BO.ProductItem()
                {
                    ID = p.ID,
                    Name = p.Name,
                    Category = (BO.Enums.ECategory)p.Category,
                    Price = p.Price,
                    InStock = p.InStock,

                    AmoutInYourCart = CostumerCart.ItemList.FindAll(e => e.ID == id).Count()
                };
                return PI;
            }
        }

        //עבור מנהל
        public void AddProduct(DO.Product p)
        {

            CheckCorectData(p.ID, p.Name, (BO.Enums.ECategory)p.Category, p.Price, p.InStock);
            try
            {
                Dal.Product.Add(p);
            }
            catch (DO.ItemAlreadyExistsException)
            {
                throw new BO.ProductAlreadyExistsException("product already exists") { ProductAlreadyExists = p.ToString() };

            }
        }

        public void UpdateProduct(BO.Product item)
        {

            CheckCorectData(item.ID, item.Name, item.Category, item.Price, item.InStock);
            try
            {
                Dal.Product.Update(newProductWithData(item.ID, item.Name, item.Category, item.Price, item.InStock));
            }
            catch (DO.RequestedItemNotFoundException)
            {
                throw new BO.ProductNotExistsException("product not exists") { ProductNotExists = item.ToString() };

            }

        }

        public void DeleteProduct(int id)
        {
            IEnumerable<DO.OrderItem> orderList = new List<DO.OrderItem>();
            orderList = Dal.OrderItem.GetAll();
            bool flag = false;
            foreach (var OI in orderList)
            {
                if (OI.ProductID == id)
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
        #endregion

        #region check corect data
        public void CheckCorectData(int id, string name, BO.Enums.ECategory category, double price, int inStock)
        {
            if (id < 0)
            {
                throw new BO.NegativeIdException("negative id") { NegativeId = id.ToString() };
            }
            if (name is null)
            {
                throw new BO.EmptyNameException("empty name") { EmptyName = name.ToString() };
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
            return new DO.Product();
        }
        #endregion
        #endregion




    }


}




