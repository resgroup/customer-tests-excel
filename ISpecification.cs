using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CustomerTestsExcel
{
    public interface ISpecification<T>
        where T : IReportsSpecificationSetup
    {
        string Description();
        T Given();
        string When(T sut);
        IEnumerable<IAssertion<T>> Assertions();
    }
}
