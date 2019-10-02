using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerTestsExcel.ExcelToCode
{
    public interface IExcelToCodeVisitor
    {
        void VisitGivenSimpleProperty(IGivenSimpleProperty givenSimpleProperty);
        void VisitGivenListPropertyDeclaration(IGivenListProperty givenListProperty);
        void VisitGivenListPropertyFinalisation();
        void VisitGivenComplexPropertyDeclaration(IGivenComplexProperty givenComplexProperty);
        void VisitGivenComplexPropertyFinalisation();
    }
}
