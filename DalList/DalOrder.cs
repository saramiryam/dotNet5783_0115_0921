
using Dal;
using DO;

namespace Dal;
public class DalOrder
{

    #region methods

    /// <summary>
    /// add a new order to the list of orders
    /// </summary>
    /// <param name="_p">an order</param>
    /// <returns>int of the id of the order</returns>
    /// <exception cref="Exception">order exists</exception>
    public int addOrder(Order _p)
    {
        for (int i = 0; i < DataSource.Config._orderIndex; i++)
        {
            if (_p.CustomerName == DataSource._arrOrder[i].CustomerName && _p.CustomerEmail == DataSource._arrOrder[i].CustomerEmail && _p.CustomerAdress == DataSource._arrOrder[i].CustomerAdress && _p.OrderDate == DataSource._arrOrder[i].OrderDate && _p.ShipDate == DataSource._arrOrder[i].ShipDate && _p.DeliveryDate == DataSource._arrOrder[i].DeliveryDate)
            {
                throw new Exception("order exists");
            }
        }
        _p.ID = DataSource.Config.CalNumOfIDOrder;
        _p.OrderDate = DateTime.Now;
        _p.ShipDate = DateTime.MinValue;
        _p.DeliveryDate = DateTime.MinValue;
        DataSource._arrOrder[DataSource.Config._orderIndex++] = _p;
        return _p.ID;
    }

    /// <summary>
    /// check if the order demanded exist and return it or an exception if not
    /// </summary>
    /// <param name="_num">the id of the order demanded</param>
    /// <returns>details of the order demanded</returns>
    /// <exception cref="Exception">order not exists</exception>
    public Order getSingleOrder(int _num)
    {
        Order _p = new Order();
        for (int i = 0; i < DataSource.Config._orderIndex; i++)
        {
            if (DataSource._arrOrder[i].ID == _num)
            {
                _p = DataSource._arrOrder[i];
                return _p;
            }
        }
        throw new Exception("order not exists");
    }

    /// <summary>
    /// cope the orders to a new arrey and return it
    /// </summary>
    /// <returns>arrey with all the orders</returns>
    public Order[] getAllOrder()
    {
        Order[] _tempArr = new Order[DataSource.Config._orderIndex];
        for (int i = 0; i < DataSource.Config._orderIndex; i++)
        {
            _tempArr[i] = DataSource._arrOrder[i];
        }
        return _tempArr;
    }

    /// <summary>
    /// check if the order demanded exist and delete it or throw an exception if not
    /// </summary>
    /// <param name="_num">id of order to delete</param>
    /// <exception cref="Exception">order not exists, can not delete</exception>
    public void deleteOrder(int _num)
    {
        bool flag = false;
        for (int i = 0; i < DataSource.Config._orderIndex; i++)
        {
            if (DataSource._arrOrder[i].ID == _num)
            {
                DataSource._arrOrder[i] = DataSource._arrOrder[DataSource.Config._orderIndex - 1];
                DataSource.Config._orderIndex--;
                flag = true;
            }
        }
        if (!flag)
        {
            throw new Exception("order not exists, can not delete");
        }
    }

    /// <summary>
    /// update data of order and throw exception if it does not exist
    /// </summary>
    /// <param name="_p"> id of order demanded to change</param>
    /// <exception cref="Exception">product not exists, can not update</exception>
    public void updateOrder(Order _p)
    {

        bool flag = true;
        for (int i = 0; i < DataSource.Config._orderIndex; i++)
        {
            if (DataSource._arrOrder[i].ID == _p.ID)
            {
                DataSource._arrOrder[i] = _p;
                flag = true;
            }
        }
        if (!flag)
        {
            throw new Exception("order not exists, can not update");
        }
    }

    #endregion

}
