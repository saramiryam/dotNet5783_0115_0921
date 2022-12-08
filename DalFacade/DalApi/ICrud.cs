
namespace DalApi;

public interface ICrud<T> where T:struct
{
    T Get(int id);
    IEnumerable<T?> GetAll();  
    void Update(T entity); 
    void Delete(int id);
    int Add(T entity); 



}
