
using DO;
using System;
using static DO.Enums;

namespace Dal;
using DalXml;
using DalApi;
public class DalProduct:IProduct
{
    XmlProduct productToXml = new XmlProduct();


    #region methods

    /// <summary>
    /// add a new product to the list of products
    /// </summary>
    /// <param name="_p">a product</param>
    /// <returns>int of the id of the product</returns>
    /// <exception cref="Exception">product exists</exception>
    public int Add(Product _p)
    {
       return productToXml.Add(_p);
        //if ((DataSource._Products
        //               .Where(e => e?.Name == _p.Name&& e?.Price == _p.Price && e?.Category == _p.Category && e?.InStock == _p.InStock )
        //               .Select (e=>(DO.Product?)e).FirstOrDefault() is not null))
        //        /*DataSource._Products.Exists(e => e?.ID == _p.ID))*/
        //    throw new ItemAlreadyExistsException("product exists, can not add") { ItemAlreadyExists = _p.ToString() };

        //else
        //{
        //    _p.ID=DataSource.Config.CalNumOfProduct;
        //    DataSource._Products.Add(_p);
        //    return _p.ID;
        //}
    }

    /// <summary>
    /// check if the product demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="_num">the id of the product demanded</param>
    /// <returns>details of the product demanded</returns>
    /// <exception cref="Exception">product not exists</exception>
    public Product Get(Func<Product?, bool>? predict)
    {
        if (DataSource._Products is null)
        {
            throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = null };
        }
        if (predict == null)
        {
            throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        }
        //Product? _newProduct = DataSource._Products.Find(e=> predict(e));
        try
        {
            return DataSource._Products
                      .Where(p => predict(p))
                      .Select(p => (Product)p!).First();
        }
        catch
        {
            throw new RequestedItemNotFoundException("product not exists,can not do get") { RequestedItemNotFound = predict.ToString() };
        }

    }

    /// <summary>
    /// cope the products to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the products</returns>
    public IEnumerable<Product?> GetAll(Func<Product?, bool>? predict=null)
    {
        if (DataSource._Products == null)
        { 
            throw new RequestedItemNotFoundException("product not exists,can not get") { RequestedItemNotFound = "jjj".ToString() }; 
        }
        if(predict == null)
        {
            return DataSource._Products;
        }
        else
        {
            //List<Product?> _products = new List<Product?>();
            //_products=DataSource._Products.FindAll(e=> predict(e)); 
            //return _products;
            try
            {
                return DataSource._Products.Where(p => predict(p))
                .Select(e => (DO.Product?)e!).ToList();
                return productToXml.GetAll(predict);
            }
            catch
            {
                throw new RequestedItemNotFoundException("orders not exists,can not get") { RequestedItemNotFound = "jjj".ToString() };
            }
        }
    }

    /// <summary>
    /// check if the product demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of product to delete</param>
    /// <exception cref="Exception">product not exists, can not delete</exception>
    public void Delete(int _num)
    {

        //if (DataSource._Products == null) throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = _num.ToString() };
        //Product? _productToDel = new Product();
        //_productToDel = DataSource._Products.Find(e => e.HasValue && e!.Value.ID == _num);
        try
        {
            DataSource._Products.Remove(DataSource._Products
                      .Where(e => e.HasValue && e!.Value.ID == _num)
                      .Select(e => (Product)e!).First());
            productToXml.Delete(_num);
        }
        catch {
            throw new RequestedItemNotFoundException("product not exists,can not do delete") { RequestedItemNotFound = _num.ToString() };
        }

    }

    /// <summary>
    /// update data of product and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of product demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void Update(Product _p)
    {
        if (_p.Name == null || _p.Category == null)
        {
            return;

        }

        //if (DataSource._Products == null) throw new RequestedItemNotFoundException("order not exists,can not get") { RequestedItemNotFound = _p.ToString() };
        ////Product? _productToUpdate = new Product();
        ////_productToUpdate = DataSource._Products.Find(e => e.HasValue && e!.Value.ID == _p.ID);
        try
        {
            DataSource._Products.Remove(DataSource._Products
               .Where(e => e is not null && e.Value.ID == _p.ID)
               .Select(e => (Product?)e!).First());
            DataSource._Products.Add(_p);
        }

        catch
        {
            throw new RequestedItemNotFoundException("product not exists,can not do update") { RequestedItemNotFound = _p.ToString() };
        }

    }

    #endregion

}
