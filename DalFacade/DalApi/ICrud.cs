using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

public interface ICrud<T>
{
    IEnumerable<T> Get(int id);  
    T Update(T entity); 
    T Delete(int id);
    void Add(T entity); 
}
