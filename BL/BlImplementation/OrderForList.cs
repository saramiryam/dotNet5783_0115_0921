using BlApi;
using DalApi;

namespace BlImplementation
{
    internal class OrderForList: IOrderForList
    {
        private IDal Dal = new Dal.DalList();
    }
}
