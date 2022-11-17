
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
        //DataSource.Config._productIndex = 0;
        for (int i = 0; i < DataSource.Config._productIndex; i++)
        {
            if (_p.Name == DataSource._arrProduct[i].Name && _p.InStock == DataSource._arrProduct[i].InStock && _p.Price == DataSource._arrProduct[i].Price)
            {
                throw new Exception("product exists");
            }
        }
        _p.ID = DataSource.Config.CalNumOfProduct;
        DataSource._arrProduct[DataSource.Config._productIndex++] = _p;
        return _p.ID;
    }

    /// <summary>
    /// check if the product demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="_num">the id of the product demanded</param>
    /// <returns>details of the product demanded</returns>
    /// <exception cref="Exception">product not exists</exception>
    public Product getSingleProduct(int _num)
    {
        Product _p = new Product();
        for (int i = 0; i < DataSource.Config._productIndex; i++)
        {
            if (DataSource._arrProduct[i].ID == _num)
            {
                _p = DataSource._arrProduct[i];
                return _p;
            }
        }
        throw new Exception("product not exists");
    }

    /// <summary>
    /// cope the products to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the products</returns>
    public Product[] getAllProducts()
    {
        Product[] _tempArr = new Product[DataSource.Config._productIndex];
        for (int i = 0; i < DataSource.Config._productIndex; i++)
        {
            _tempArr[i] = DataSource._arrProduct[i];
        }
        return _tempArr;
    }

    /// <summary>
    /// check if the product demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of product to delete</param>
    /// <exception cref="Exception">product not exists, can not delete</exception>
    public void deleteProduct(int _num)
    {
        bool flag = false;
        for (int i = 0; i < DataSource.Config._productIndex; i++)
        {
            if (DataSource._arrProduct[i].ID == _num)
            {
                DataSource._arrProduct[i] = DataSource._arrProduct[DataSource.Config._productIndex - 1];
                DataSource.Config._productIndex--;
                flag = true;
            }
        }
        if (!flag)
        {
            throw new Exception("product not exists, can not delete");

        }
    }

    /// <summary>
    /// update data of product and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of product demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void updateProduct(Product _p)
    {
        bool flag=false;
        for (int i = 0; i < DataSource.Config._productIndex; i++)
        {
            if (DataSource._arrProduct[i].ID == _p.ID)
            {
                DataSource._arrProduct[i] = _p;
                flag = true;
            }
        }
        if (!flag)
        {
            throw new Exception("product not exists, can not update");
        }
    }

    #endregion

}
