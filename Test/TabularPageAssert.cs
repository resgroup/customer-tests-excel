using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.Test
{
    public static class TabularPageAssert
    {
        // This will find the firect occurence of expectedCells[0][0] in page, and assume
        // that this is the place to check. It could be extended to check for all occurences
        // of expectedCells[0][0], and pass if any of them contain the expectedCells.
        public static void Contains(object[][] expectedCells, TestTabularPage page)
        {
            var startCellReference = GetStartCellReference(expectedCells, page);

            var failures = CheckExpectedCells(expectedCells, page, startCellReference);

            if (failures.Any())
            {
                Assert.Fail(
                    $"Found start of expectedCells at Row {startCellReference.Row} Column {startCellReference.Column}."
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, failures)
                );
            }
        }

        public static object[][] Table(params object[][] rows) =>
            rows;

        public static object[] Row(params object[] cells) =>
            cells;

        private static List<string> CheckExpectedCells(object[][] expectedCells, TestTabularPage page, CellReference startCellReference)
        {
            var failures = new List<string>();

            for (uint row = 0; row < expectedCells.Length - 1; row++)
            {
                for (uint column = 0; column < expectedCells[row].Length - 1; column++)
                {
                    if ((dynamic)expectedCells[row][column] != (dynamic)page.GetCell(startCellReference.Row + row, startCellReference.Column + column).Value)
                    {
                        failures.Add($"Expecting {expectedCells[row][column]} at Row {row} Column {column}, but found {page.GetCell(startCellReference.Row + row, startCellReference.Column + column).Value}");
                    }
                }
            }

            return failures;
        }

        private static CellReference GetStartCellReference(object[][] expectedCells, TestTabularPage page)
        {
            if (expectedCells.Length == 0)
                Assert.Fail("No expectedCells specified, which is considered to be a mistake.");

            if (expectedCells[0].Length == 0)
                Assert.Fail("The first row in expectedCells must have at least one column");

            return page.FindCell(expectedCells[0][0]);
        }
    }
}