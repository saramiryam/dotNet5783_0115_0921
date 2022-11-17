
using Dal;
using DO;

namespace Dal;
public class DalOrder
{

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
    /// update date of order and throw exception if it does not exist
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
