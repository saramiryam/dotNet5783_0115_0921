
namespace DalApi;

public interface ICrud<T>
{
    T Get(int id);
    IEnumerable<T>[] GetAll();  
    T Update(T entity); 
    T Delete(int id);
    int Add(T entity); 
}
