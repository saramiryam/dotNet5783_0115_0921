using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal;

public class XmlProduct : IProduct
{
    readonly string ProductPath = @"Product.xml";

    #region methods
    /// <summary>
    /// add a new order to the list of orders
    /// </summary>
    /// <param name="_p">an order</param>
    /// <returns>int of the id of the order</returns>
    /// <exception cref="Exception">order exists</exception>
    public int Add(DO.Product _p)
    {
        XElement ProductRoot = XMLTools.LoadListFromXMLElement(ProductPath);
        if (ProductRoot == null)
        {
            throw new RequestedItemNotFoundException("product not exists,can not get") { RequestedItemNotFound = _p.ToString() };
        }

        XElement ifExsistPro = (from p in ProductRoot.Elements()
                                  where int.Parse(p.Element("ID").Value) == _p.ID
                                  select p).FirstOrDefault();

        if (ifExsistPro != null)
              throw new DO.ItemAlreadyExistsException("Product exists, can not add") { ItemAlreadyExists = _p.ToString() };
        XElement ProductElement = new XElement("Product", new XElement("ID", _p.ID.ToString()),
                                new XElement("Name", _p.Name),
                                new XElement("Price", _p.Price),
                                new XElement("InStock", _p.InStock),
                                new XElement("Category", _p.Category));

        ProductRoot.Add(ProductElement);
        XMLTools.SaveListToXMLElement(ProductRoot, ProductPath);
        return _p.ID;
        #endregion

    }


    /// <summary>
    /// check if the order demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="_num">the id of the order demanded</param>
    /// <returns>details of the order demanded</returns>
    /// <exception cref="Exception">order not exists</exception>
    public Product Get(Func<Product?, bool>? predict)
    {



        XElement ProductRoot = XMLTools.LoadListFromXMLElement(ProductPath);

        if (ProductRoot == null)
        {
            throw new RequestedItemNotFoundException("Product not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
        if (predict == null)
        {
            throw new GetPredictNullException("the predict is empty") { GetPredictNull = null };
        }

        try
        {
            IEnumerable<DO.Product?>? Pro = ProductRoot.Elements().Select(x =>
            {
                DO.Product p = new();
                p.ID = Int32.Parse(x.Element("ID").Value);
                p.Name = x.Element("Name").Value;
                p.Price = double.Parse(x.Element("Price").Value);
                p.InStock = int.Parse(x.Element("InStock").Value);
                p.Category= GetCategory(x.Element("Category").Value);
              
                return (DO.Product?)p;
            }).Where(x => predict(x));
            return (Product)Pro.FirstOrDefault();
        }
        catch
        {
            throw new RequestedItemNotFoundException("Product not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }

    }


    private DO.Enums.ECategory GetCategory(string mycat)
    {
        switch (mycat)
        {
            case "Notebooks":
                return DO.Enums.ECategory.Notebooks;
            case "Games":
                return DO.Enums.ECategory.Games;
            case "Pens":
                return DO.Enums.ECategory.Pens;
            case "Diaries":
                return DO.Enums.ECategory.Diaries;
            case "ArtMaterials":
                return DO.Enums.ECategory.ArtMaterials;
        }
        throw new CategoryNotExsistException(mycat) { CategoryNotExsist=mycat};
    }


    /// <summary>
    /// cope the orders to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the orders</returns>
    /// 

    public IEnumerable<Product?> GetAll(Func<Product?, bool>? predict = null)
    {


        XElement ProductRoot = XMLTools.LoadListFromXMLElement(ProductPath);
        if (ProductRoot == null)
            throw new RequestedItemNotFoundException("Product not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        try
        {
            IEnumerable<DO.Product?>? Pro = ProductRoot.Elements().Select(x =>
            {
                DO.Product p = new();
                p.ID = Int32.Parse(x.Element("ID").Value);
                p.Name = x.Element("Name").Value;
                p.Price = double.Parse(x.Element("Price").Value);
                p.InStock = int.Parse(x.Element("InStock").Value);
                p.Category = GetCategory(x.Element("Category").Value);
                return (DO.Product?)p;
            }).Where(x => predict == null || predict(x));
            return Pro.ToList();
        }
        catch
        {
            throw new RequestedItemNotFoundException("Product not exists,can not get") { RequestedItemNotFound = predict?.ToString() };
        }
        
    }


    /// <summary>
    /// check if the order demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of order to delete</param>
    /// <exception cref="Exception">order not exists, can not delete</exception>
    public void Delete(int _id)
    {

        XElement ProductRoot = XMLTools.LoadListFromXMLElement(ProductPath);

        if (ProductRoot is null)
            throw new RequestedItemNotFoundException("Product not exists,can not get") { RequestedItemNotFound = _id.ToString() };

        XElement Pro = (from pro in ProductRoot.Elements()
                        where int.Parse(pro.Element("ID").Value) == _id
                        select pro).FirstOrDefault();

        if (Pro != null)
        {
            Pro.Remove(); //<==>   Remove per from personsRootElem

            XMLTools.SaveListToXMLElement(ProductRoot, ProductPath);
        }
        else
            throw new RequestedItemNotFoundException("Product not exists,can not delete") { RequestedItemNotFound = _id.ToString() };
      
    }


    /// <summary>
    /// update date of order and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of order demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void Update(Product _p)
    {

        XElement ProductRoot = XMLTools.LoadListFromXMLElement(ProductPath);
        if (ProductRoot is null)
            throw new RequestedUpdateItemNotFoundException("Product not exists,can not update") { RequestedUpdateItemNotFound = _p.ToString() };
        XElement pro = (from p in ProductRoot.Elements()
                        where int.Parse(p.Element("ID").Value) == _p.ID
                        select p).FirstOrDefault();

        if (pro != null)
        {

            pro.Element("ID").Value = _p.ID.ToString();
            pro.Element("Price").Value = _p.Price.ToString();
            pro.Element("Name").Value = _p.Name;
            pro.Element("InStock").Value = _p.InStock.ToString();
            pro.Element("Category").Value = _p.Category.ToString();


            XMLTools.SaveListToXMLElement(ProductRoot, ProductPath);
        }

    }
}
