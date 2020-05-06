using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerTestsExcel
{
    public class FluentList<T> : List<T>
    {
        public FluentList<T> FluentAdd(T item)
        {
            Add(item);
            return this;
        }
    }
}
