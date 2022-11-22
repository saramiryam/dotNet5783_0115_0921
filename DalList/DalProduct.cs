
using DO;
using System;
using static DO.Enums;

namespace Dal;


public class DalProduct
{
    //private DalProduct(string newName, string newCategory, double newPrice, int newInStock)

    //{
    //    //לתקן פה
    //    Product p = new Product() {ID=DataSource.Config.CalNumOfProduct,Name=newName,
    //        //Category=Tryparse(newCategory,ECategory),
    //        Price=newPrice,InStock=newInStock};    

    //}

    #region methods

    /// <summary>
    /// add a new product to the list of products
    /// </summary>
    /// <param name="_p">a product</param>
    /// <returns>int of the id of the product</returns>
    /// <exception cref="Exception">product exists</exception>
    public int addProduct(Product _p)
    {
        if (DataSource._Products.Exists(e => e.Name == _p.Name&&e.Category==_p.Category&&e.Price==_p.Price&&e.InStock==_p.InStock))
            throw new Exception("product exists");
        else
        {
            _p.ID = DataSource.Config.CalNumOfProduct;
            DataSource._Products.Add(_p);
            return _p.ID;
        }
    }

    /// <summary>
    /// check if the product demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="_num">the id of the product demanded</param>
    /// <returns>details of the product demanded</returns>
    /// <exception cref="Exception">product not exists</exception>
    public Product getSingleProduct(int _num)
    {
        Product _newProduct = new Product();
        _newProduct = DataSource._Products.Find(e => e.ID == _num);
        if (_newProduct.ID!=0)
            return _newProduct;
        else
            throw new Exception("product not exists");

    }

    /// <summary>
    /// cope the products to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the products</returns>
    public List<Product> getAllProducts()
    {
      return DataSource._Products;
    }

    /// <summary>
    /// check if the product demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of product to delete</param>
    /// <exception cref="Exception">product not exists, can not delete</exception>
    public void deleteProduct(int _num)
    {
        Product _productToDel = DataSource._Products.Find(e => e.ID == _num);
        if (_productToDel.ID != 0)
            DataSource._Products.Remove(_productToDel);
        else
            throw new Exception("product not exists, can not delete");

    }

    /// <summary>
    /// update data of product and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of product demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void updateProduct(Product _p)
    {
        if (_p.ID == null || _p.Name == null || _p.Category == null || _p.Price == null || _p.InStock == null)
        {
            return;

        }
        Product _productToUpdate = DataSource._Products.Find(e => e.ID == _p.ID );
        if (_productToUpdate.ID != 0)
        {
            DataSource._Products.Remove(_productToUpdate);
            DataSource._Products.Add(_p);
        }
        else
            throw new Exception("product not exists can not update");

    }

    #endregion

}
