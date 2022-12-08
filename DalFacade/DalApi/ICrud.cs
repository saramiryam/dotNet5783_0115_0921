
namespace DalApi;

public interface ICrud<T> where T:struct
{
    T Get(Func<T?, bool>? predict);
    IEnumerable<T?> GetAll(Func<T?, bool>? predict = null);  
    void Update(T entity); 
    void Delete(int id);
    int Add(T entity); 



}
