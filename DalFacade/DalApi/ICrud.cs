
namespace DalApi;

public interface ICrud<T>
{
    IEnumerable<T> Get(int id);
    IEnumerable<T> GetAll();  
    T Update(T entity); 
    T Delete(int id);
    void Add(T entity); 
}
