using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trdb.entity.Helpers;

namespace trdb.data.Interface
{
    public interface IBaseInterface<T> where T : class
    {
        ProcessResult Add(T entity);
        ProcessResult Update(T entity);
        ProcessResult Delete(int ID);
        T Get(int ID);
        IEnumerable<T> GetAll();
        FilteredList<T> FilteredList(FilteredList<T> request);
    }
}
