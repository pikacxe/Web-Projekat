using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Repository
{
    public interface IDao<T>
    {
        T FindByID(int id);
        T Add(T item);
        T Delete(int id);
        T Update(T updated);
        bool Exists(int id);

    }
}
