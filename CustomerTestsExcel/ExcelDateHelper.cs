using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CustomerTestsExcel
{
    // Dates are times are a complete disaster area in Excel
    // see http://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.numberingformat(v=office.14).aspx
    // and http://stackoverflow.com/questions/4730152/what-indicates-an-office-open-xml-cell-contains-a-date-time-value
    public class ExcelDateHelper
    {
        static uint[] builtInDateTimeNumberFormatIDs = new uint[] { 14, 15, 16, 17, 18, 19, 20, 21, 22, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 45, 46, 47, 50, 51, 52, 53, 54, 55, 56, 57, 58 };
        static Dictionary<uint, NumberingFormat> builtInDateTimeNumberFormats = builtInDateTimeNumberFormatIDs.ToDictionary(id => id, id => new NumberingFormat { NumberFormatId = id });
        static Regex dateTimeFormatRegex = new Regex(@"((?=([^[]*\[[^[\]]*\])*([^[]*[ymdhs]+[^\]]*))|.*\[(h|mm|ss)\].*)", RegexOptions.Compiled);

        public static Dictionary<uint, NumberingFormat> GetDateTimeCellFormats(WorkbookPart workbookPart)
        {
            Dictionary<uint, NumberingFormat> dateNumberFormats;
            if (workbookPart.WorkbookStylesPart.Stylesheet.NumberingFormats == null)
                dateNumberFormats = new Dictionary<uint, NumberingFormat>();
            else
                dateNumberFormats = workbookPart.WorkbookStylesPart.Stylesheet.NumberingFormats
                ?.Descendants<NumberingFormat>()
                .Where(nf => dateTimeFormatRegex.Match(nf.FormatCode.Value).Success)
                .ToDictionary(nf => nf.NumberFormatId.Value);

            var cellFormats = workbookPart.WorkbookStylesPart.Stylesheet.CellFormats
                .Descendants<CellFormat>();

            var dateCellFormats = new Dictionary<uint, NumberingFormat>();
            uint styleIndex = 0;
            foreach (var cellFormat in cellFormats)
            {
                if (cellFormat.ApplyNumberFormat != null && cellFormat.ApplyNumberFormat.Value)
                {
                    if (dateNumberFormats.ContainsKey(cellFormat.NumberFormatId.Value))
                    {
                        dateCellFormats.Add(styleIndex, dateNumberFormats[cellFormat.NumberFormatId.Value]);
                    }
                    else if (builtInDateTimeNumberFormats.ContainsKey(cellFormat.NumberFormatId.Value))
                    {
                        dateCellFormats.Add(styleIndex, builtInDateTimeNumberFormats[cellFormat.NumberFormatId.Value]);
                    }
                }

                styleIndex++;
            }

            return dateCellFormats;
        }

        public static bool IsDateTimeCell(WorkbookPart workbookPart, Cell cell)
        {
            if (cell.StyleIndex == null)
                return false;

            var dateTimeCellFormats = GetDateTimeCellFormats(workbookPart);

            return dateTimeCellFormats.ContainsKey(cell.StyleIndex);
        }
    }
}