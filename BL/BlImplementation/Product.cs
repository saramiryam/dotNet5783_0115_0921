using BlApi;
using DalApi;
using DO;
using System;

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

                p1 = eserDOToBO(p);
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
        public int AddProduct(int id, string name, BO.Enums.ECategory category, double price, int inStock)
        {
            #region check corect data
            if (id <= 0)
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
            #endregion

        }
        public int UpdateProduct(Product item)
        {

        }

        public void DeleteProduct(int id)
        {

        }





        #endregion
        #region ezer DO to BO
        public BO.Product eserDOToBO(DO.Product p)
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


    }



}
