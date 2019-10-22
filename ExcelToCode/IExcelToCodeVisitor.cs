using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerTestsExcel.ExcelToCode
{
    // I think it would be better for ExcelToCode to expose a tree structure with the setup, 
    // and then other code can just perform operations on the tree to do stuff.
    // It's a shame I only thought of this after coding up the visitor pattern solution.
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
