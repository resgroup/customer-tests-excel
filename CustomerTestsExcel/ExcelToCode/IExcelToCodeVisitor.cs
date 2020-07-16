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
        void VisitGivenRootClassDeclaration(string className);
        void VisitGivenRootClassFinalisation();

        void VisitGivenSimpleProperty(IVisitedGivenSimpleProperty givenSimpleProperty);

        void VisitGivenFunction(IVisitedGivenFunction givenFunction);

        void VisitGivenListPropertyDeclaration(IVisitedGivenListProperty givenListProperty);
        void VisitGivenListPropertyFinalisation();

        void VisitGivenComplexPropertyDeclaration(IVisitedGivenComplexProperty givenComplexProperty);
        void VisitGivenComplexPropertyFinalisation();

        void VisitGivenTablePropertyDeclaration(IVisitedGivenTableProperty givenTableProperty, IEnumerable<TableHeader> tableHeaders);
        void VisitGivenTablePropertyRowDeclaration(uint row);
        void VisitGivenTablePropertyCellDeclaration(TableHeader tableHeader, uint row, uint column);
        void VisitGivenTablePropertyCellFinalisation();
        void VisitGivenTablePropertyRowFinalisation();
        void VisitGivenTablePropertyFinalisation();
    }
}
