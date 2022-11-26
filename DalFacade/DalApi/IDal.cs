
namespace DalApi;

public interface IDal
{
    IProduct Product { get; }
    IOrderItem OrderItem { get; }    
    IOrder Order { get; }
}
