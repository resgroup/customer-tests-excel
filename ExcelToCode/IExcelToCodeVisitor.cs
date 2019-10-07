using System;
using System.Collections.Generic;
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

        void VisitGivenTablePropertyDeclaration(IEnumerable<TableHeader> tableHeaders);
        void VisitGivenTablePropertyRowDeclaration(uint row);
        void VisitGivenTablePropertyCellDeclaration(TableHeader tableHeader, uint row, uint column);
        void VisitGivenTablePropertyCellFinalisation();
        void VisitGivenTablePropertyRowFinalisation();
        void VisitGivenTablePropertyFinalisation();
    }
}
