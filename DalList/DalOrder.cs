
using Dal;
using DO;

namespace Dal;
public class DalOrder
{
public int addNewOrder(Order _p)
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
        DataSource._arrOrder[DataSource.Config._orderIndex++] = _p;
        return _p.ID;
    }
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
    public Order[] getAllOrder()
    {
        Order[] _tempArr = new Order[DataSource.Config._orderIndex];
        for (int i = 0; i < DataSource.Config._orderIndex; i++)
        {
            _tempArr[i] = DataSource._arrOrder[i];
        }
        return _tempArr;
    }

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
    public void updateOrder(Order _p)
    {

        bool flag = true;
        for (int i = 0; i < DataSource.Config._orderIndex; i++)
        {
            if (DataSource._arrOrder[i].ID == _p.ID)
            {
                DataSource._arrOrder[i] = _p;
                flag= true;
            }
        }
        if (!flag)
        {
            throw new Exception("product not exists, can not update");
        }
    }
}
