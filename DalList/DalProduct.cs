
using DO;
using System;

namespace Dal;


public class DalProduct
{
    public int addNewProduct(Product _p)
    {
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
    public Product[] getAllProducts()
    {
        Product[] _tempArr = new Product[DataSource.Config._productIndex];
        for (int i = 0; i < DataSource.Config._productIndex; i++)
        {
            _tempArr[i] = DataSource._arrProduct[i];
        }
        return _tempArr;
    }

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
}
