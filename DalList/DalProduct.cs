
using DO;
using System;
using static DO.Enums;

namespace Dal;

using DalApi;
public class DalProduct:IProduct
{


    #region methods

    /// <summary>
    /// add a new product to the list of products
    /// </summary>
    /// <param name="_p">a product</param>
    /// <returns>int of the id of the product</returns>
    /// <exception cref="Exception">product exists</exception>
    public int Add(Product _p)
    {
        if (DataSource._Products.Exists(e => e?.Name == _p.Name&&e?.Category==_p.Category&&e?.Price==_p.Price&&e?.InStock==_p.InStock))
            throw new ItemAlreadyExistsException("product exists, can not add") { ItemAlreadyExists = _p.ToString() };

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
    public Product Get(int _num)
    {
        if (DataSource._Products == null) throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = _num.ToString() };
        Product? _newProduct = new Product();
        _newProduct = DataSource._Products.Find(e => e.HasValue && e!.Value.ID == _num);
        if (_newProduct.HasValue)
            return (Product)_newProduct;
        else
            throw new RequestedItemNotFoundException("product not exists,can not do get") { RequestedItemNotFound = _num.ToString() };


    }

    /// <summary>
    /// cope the products to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the products</returns>
    public IEnumerable<Product?> GetAll()
    {
        if (DataSource._Products == null) throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = "jjj".ToString() };
        return (IEnumerable<Product?>)DataSource._Products;
    }

    /// <summary>
    /// check if the product demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of product to delete</param>
    /// <exception cref="Exception">product not exists, can not delete</exception>
    public void Delete(int _num)
    {

        if (DataSource._Products == null) throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = _num.ToString() };
        Product? _productToDel = new Product();
        _productToDel = DataSource._Products.Find(e => e.HasValue && e!.Value.ID == _num);
        if (_productToDel.HasValue)
            DataSource._Products.Remove(_productToDel);
        else
            throw new RequestedItemNotFoundException("product not exists,can not do delete") { RequestedItemNotFound = _num.ToString() };


    }

    /// <summary>
    /// update data of product and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of product demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void Update(Product _p)
    {
        if (_p.ID == null || _p.Name == null || _p.Category == null || _p.Price == null || _p.InStock == null)
        {
            return;

        }

        if (DataSource._Products == null) throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = _p.ToString() };
        Product? _productToUpdate = new Product();
        _productToUpdate = DataSource._Products.Find(e => e.HasValue && e!.Value.ID == _p.ID);
        if (_productToUpdate.HasValue)
        {
            DataSource._Products.Remove(_productToUpdate);
            DataSource._Products.Add(_p);
        }
        else
            throw new RequestedItemNotFoundException("product not exists,can not do update") { RequestedItemNotFound = _p.ToString() };


    }

    #endregion

}
