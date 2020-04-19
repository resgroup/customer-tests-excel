using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CustomerTestsExcel
{
    internal class ExcelTabularPage : ITabularPage
    {
        private readonly IFormatProvider formatProvider = new CultureInfo("en-GB");
        WorksheetPart worksheetPart;
        ChartsheetPart chartsheetPart;
        Sheet sheet;
        readonly SpreadsheetDocument spreadsheetDocument;
        ExcelTabularBook parent;

        // this is for GetRow etc
        private SheetData sheetData;
        private SheetData rowLookupCacheAssociatedSheet = null;
        private Dictionary<uint, Row> rowLookupCache = null;
        private IEnumerator<Row> rowLookupCacheEnumerator;

        // this is for get cell value
        private SharedStringTable sharedStringTable;

        public object this[uint row, uint column]
        {
            get { return GetCell(row, column).Value; }
            set { SetCell(row, column, value); }
        }

        internal ExcelTabularBook WorkBook => parent;

        internal static Worksheet CreateBlankWorksheet(bool selected)
        {
            if (selected)
            {
                return new Worksheet(
                    new SheetDimension() { Reference = "A1" },
                    new SheetViews(
                        new SheetView() { TabSelected = (BooleanValue)true, WorkbookViewId = (UInt32Value)0U }),
                    new SheetFormatProperties() { DefaultRowHeight = 15D },
                    new SheetData(),
                    new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D });

            }
            else
            {
                return new Worksheet(
                    new SheetDimension() { Reference = "A1" },
                    new SheetViews(
                        new SheetView() { WorkbookViewId = (UInt32Value)0U }),
                    new SheetFormatProperties() { DefaultRowHeight = 15D },
                    new SheetData(),
                    new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D });

            }

        }

        internal static Sheet CreateBlankSheet(string relationshipId, uint sheetId, string sheetName, SpreadsheetDocument package)
        {
            if (package.ExtendedFilePropertiesPart != null)
            {
                package.ExtendedFilePropertiesPart.Properties.TitlesOfParts.VTVector.Append(new VTLPSTR(sheetName));
            }

            return new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
        }

        internal ExcelTabularPage(
            SpreadsheetDocument spreadsheetDocument, 
            Workbook workbook, 
            WorksheetPart worksheetPart, 
            Sheet sheet, 
            Stylesheet stylesheet, 
            uint index, 
            ExcelTabularBook parent)
        {
            this.spreadsheetDocument = spreadsheetDocument;
            this.worksheetPart = worksheetPart;
            chartsheetPart = null;
            this.sheet = sheet;
            this.parent = parent;
            sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            sharedStringTable = spreadsheetDocument.WorkbookPart.SharedStringTablePart?.SharedStringTable;

            PageSetup pageSetup = worksheetPart.Worksheet.GetFirstChild<PageSetup>();
            if (pageSetup != null)
                pageSetup.VerticalDpi = null;
        }

        public void SetName(string name)
        {
            string safeVersion = name.Substring(0, System.Math.Min(name.Length, 31));
            foreach (var bad in BAD_CHARS)
                safeVersion = safeVersion.Replace(bad, "-");
            Name = safeVersion;
        }

        public string Name
        {
            get
            {
                return sheet.Name;
            }
            set
            {
                if (value != sheet.Name)
                {
                    SheetNameSafe(value);
                    sheet.Name = value;
                }
            }
        }

        //I got this list from http://stackoverflow.com/questions/451452/valid-characters-for-excel-sheet-names
        private string[] BAD_CHARS
        {
            get
            {
                return new string[] { "[", "]", "*", "/", "\\", "?", ":" };
            }
        }

        private void SheetNameSafe(string name)
        {
            if (name.Length > 31)
                throw new ArgumentOutOfRangeException("Name is too long");

            foreach (var bad in BAD_CHARS)
                if (name.Contains(bad))
                    throw new ArgumentException(string.Format("Name contains a \"{0}\"", bad));
        }

        public bool Visible
        {
            get
            {
                return sheet.State == null ? true : sheet.State.Value == SheetStateValues.Visible;

            }
            set
            {
                var values = value ? SheetStateValues.Visible : SheetStateValues.Hidden;
                if (sheet.State == null)
                    sheet.State = new EnumValue<SheetStateValues>(values);
                else
                    sheet.State.Value = values;
            }
        }

        private void ThrowExceptionIfChartSheet(string functionName)
        {
            if (worksheetPart == null && chartsheetPart != null)
            {
                throw new ApplicationException(string.Format("Cannot call {0} on '{1}' as it is a ChartSheet", functionName, sheet.Name));
            }
        }
        public uint MaxColumn
        {
            get
            {
                // todo - cache this value and add 20 as it doesn't always seem to work
                // maybe it is just counting cells with something in for the row?
                ThrowExceptionIfChartSheet("MaxColumn");
                uint maxColumns = 0;
                foreach (var row in worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>())
                {
                    var cells = row.Elements<Cell>();
                    var count = cells.Count();
                    if (count > maxColumns)
                    {
                        maxColumns = (uint)count;
                    }
                }
                return maxColumns;
            }
        }

        public uint MaxRow
        {
            get
            {
                ThrowExceptionIfChartSheet("MaxRow");
                var rows = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>();
                if (rows.Any() == false) return 0;
                return rows.Max(r => r.RowIndex.Value);
            }
        }
        public void SetCell(uint row, uint column, object value)
        {
            if (row == 0 || column == 0)
                throw new IndexOutOfRangeException();

            CellValue cellValue;
            InlineString cellString;
            EnumValue<CellValues> cellDataType;
            uint? cellStyleIndex;

            // Getting the things and then setting them seems strange, 
            // I think it would make more sense to pass in the cell, 
            // and let this function set the properties
            GetCellProperties(
                value, 
                out cellString, 
                out cellValue, 
                out cellDataType,
                out cellStyleIndex
                );

            var cell = GetOrAddCell(column, row);
            cell.InlineString = cellString;
            cell.CellValue = cellValue;
            cell.DataType = cellDataType;
            if (cellStyleIndex.HasValue)
                cell.StyleIndex = cellStyleIndex;
        }

        public Row GetOrAddRow(uint rowIndex)
        {
            var row = GetRow(rowIndex);
            if (row == null)
            {
                row = new Row() { RowIndex = rowIndex };
                AddRow(row);
            }
            return row;
        }

        public IEnumerator<Row> GetRowEnumerator()
        {
            return sheetData.Elements<Row>().GetEnumerator();
        }

        public Row GetRow(uint rowIndex)
        {
            if (rowLookupCacheAssociatedSheet != sheetData)
            {
                rowLookupCache = new Dictionary<uint, Row>();
                rowLookupCacheEnumerator = GetRowEnumerator();
                rowLookupCacheEnumerator.MoveNext();
                rowLookupCacheAssociatedSheet = sheetData;
            }

            // See if we already have it in the cache
            if (rowLookupCache.Keys.Contains(rowIndex))
                return rowLookupCache[rowIndex];

            // ok, is it at the enumerators current position (don't think it can be...)            
            if (rowLookupCacheEnumerator.Current != null && rowLookupCacheEnumerator.Current.RowIndex == rowIndex)
            {
                rowLookupCache.Add(rowIndex, rowLookupCacheEnumerator.Current);
                return rowLookupCacheEnumerator.Current;
            }

            // Try the next position
            rowLookupCacheEnumerator.MoveNext();
            if (rowLookupCacheEnumerator.Current != null && rowLookupCacheEnumerator.Current.RowIndex == rowIndex)
            {
                rowLookupCache.Add(rowIndex, rowLookupCacheEnumerator.Current);
                return rowLookupCacheEnumerator.Current;
            }

            // OK - we're going to have to try an exhaustive search
            rowLookupCacheEnumerator = GetRowEnumerator();
            while (rowLookupCacheEnumerator.MoveNext())
            {
                if (rowLookupCacheEnumerator.Current.RowIndex == rowIndex)
                {
                    rowLookupCache.Add(rowIndex, rowLookupCacheEnumerator.Current);
                    return rowLookupCacheEnumerator.Current;
                }
            }

            return null;
        }

        private void AddRow(Row row)
        {
            rowLookupCache = null;
            rowLookupCacheAssociatedSheet = null;
            rowLookupCacheEnumerator = null;
            if (sheetData.Elements<Row>().Count() > 0)
            {
                if (sheetData.Elements<Row>().Last() != null &&
                    sheetData.Elements<Row>().Last().RowIndex > row.RowIndex.Value)
                {
                    // Add row at the correct position in the list
                    var sheetRows = new List<Row>();
                    using (var enumerator = GetRowEnumerator())
                    {
                        bool newSheetAdded = false;
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.RowIndex.Value > row.RowIndex.Value && !newSheetAdded)
                            {
                                sheetRows.Add(row);
                                newSheetAdded = true;
                            }
                            sheetRows.Add(enumerator.Current);
                        }
                    }
                    sheetData.RemoveAllChildren<Row>();
                    foreach (var sheetRow in sheetRows)
                    {
                        sheetData.Append(sheetRow);
                    }
                }
                else
                {
                    // Add row to the end of the row list
                    sheetData.Append(row);
                }
            }
            else
            {
                // Add first row to the end of the row list
                sheetData.Append(row);
            }
        }

        // slight variation of http://msdn.microsoft.com/en-us/library/cc861607(office.14).aspx#InsertCell
        protected Cell GetOrAddCell(uint columnIndex, uint rowIndex)
        {
            string columnName = GetColumnName(columnIndex);
            string cellReference = columnName + rowIndex;

            var row = GetOrAddRow(rowIndex);

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    // Cell references could start with multiple alphabetic characters (e.g 'AA1'). Columns 'A' to 'Z' must be before 'AA' to 'AZ'.
                    if (cellReference.Length < cell.CellReference.Value.Length)
                    {
                        refCell = cell;
                        break;
                    }
                    else if (cellReference.Length == cell.CellReference.Value.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                var newCell = new Cell() { CellReference = cellReference, StyleIndex = 0U };
                if (row.StyleIndex != null)
                {
                    newCell.StyleIndex.Value = row.StyleIndex.Value;
                }

                row.InsertBefore(newCell, refCell);

                return newCell;
            }
        }

        private object MakeInfinityAString(object cellValue)
        {
            if (cellValue is double || cellValue is float)
            {
                float val;

                if (cellValue is double)
                    val = (float)(double)cellValue;
                else
                    val = (float)cellValue;

                if (float.IsNaN(val))
                    return "NaN";
                else if (float.IsPositiveInfinity(val))
                    return "Infinity";
                else if (float.IsNegativeInfinity(val))
                    return "Negative Infinity";
                else
                    return cellValue;
            }
            else
            {
                return cellValue;
            }
        }

        private void GetCellProperties(
            object value, 
            out InlineString stringValue, 
            out CellValue cellValue, 
            out EnumValue<CellValues> cellDataType,
            out uint? cellStyleIndex)
        {
            value = MakeInfinityAString(value);

            cellValue = null;
            stringValue = null;
            cellDataType = null;
            cellStyleIndex = null;

            if (value == null || value is System.Reflection.Missing)
                return;

            else if (value is string)
            {
                stringValue = new InlineString(new Text(value as string));
            }
            else if (value is DateTime)
            {
                cellValue = new CellValue(((DateTime)value).ToOADate().ToString());
                // this causes a problem loading in excel stupidly: cellDataType = CellValues.Date;

                var spreadsheetDocument = this.spreadsheetDocument;
                if (spreadsheetDocument != null)
                {
                    var cellFormats = spreadsheetDocument.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.Cast<CellFormat>();
                    var dateFormat =
                        cellFormats
                            .Where(cellFormat => cellFormat.NumberFormatId.Value >= 14 && cellFormat.NumberFormatId.Value <= 17 || cellFormat.NumberFormatId.Value == 22 || cellFormat.NumberFormatId.Value == 30)
                            .FirstOrDefault();

                    cellStyleIndex = (uint) cellFormats.TakeWhile(cellFormat => cellFormat != dateFormat).Count();
                }
            }
            else if (value is Boolean)
            {
                cellValue = new CellValue(Convert.ToInt32(value).ToString(formatProvider));
            }
            else if (value is Byte ||
                     value is Char ||
                     value is Decimal ||
                     value is Double ||
                     value is Int16 ||
                     value is Int32 ||
                     value is Int64 ||
                     value is Single ||
                     value is UInt16 ||
                     value is UInt32 ||
                     value is UInt64)
            {
                cellValue = new CellValue(string.Format(formatProvider, "{0}", value));
            }
            else
            {
                throw new Exception("Cannot write value of type " + value.GetType().Name + " must be converted to a basic type (string, number, datetime).");
            }
            if (!(value is DateTime))
            {
                cellDataType = new EnumValue<CellValues>(GetDataType(value));
            }
        }

        private CellValues GetDataType(object value)
        {
            if (value is bool)
                return CellValues.Boolean;
            else if (value is string)
                return CellValues.InlineString;
            return CellValues.Number;
        }

        //standalone todo tidy this up, make it faster
        //this is surprisingly difficult and slow just to get the value of a cell.
        public ITabularCell GetCell(uint row, uint column)
        {
            if (row == 0 || column == 0)
                throw new IndexOutOfRangeException();

            string cellReference = CellReference(row, column);

            var cell = worksheetPart.Worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference == cellReference);

            return (cell == null) 
                ? new TabularCell(null, false) 
                : new TabularCell(GetCellValue(cell), cell.CellFormula?.Text != null);
        }

        private string GetColumnName(uint columnNumber)
        {
            int dividend = (int) columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private string CellReference(uint rowIndex, uint columnIndex)
        {
            return GetColumnName(columnIndex) + rowIndex.ToString();
        }

        private object GetCellValue(Cell cell)
        {
            if (cell != null)
            {
                if (cell.DataType != null && cell.DataType.HasValue)
                {
                    switch (cell.DataType.Value)
                    {
                        case CellValues.String:
                        case CellValues.SharedString:
                            int sharedStringIndex;

                            if (int.TryParse(cell.CellValue.InnerText, out sharedStringIndex))
                            {
                                if (sharedStringTable == null)
                                    throw new Exception("Cell references shared string table, but table not found");

                                return sharedStringTable.ElementAt(sharedStringIndex).InnerText;
                            }
                            else
                                return cell.CellValue.InnerText;

                        case CellValues.Boolean:
                            if (cell.CellValue.InnerText == "1")
                                return true;
                            else if (cell.CellValue.InnerText == "0")
                                return false;
                            else
                                throw new Exception("Failed to parse boolean cell with value " + cell.CellValue.InnerText + " at " + cell.CellReference.Value);

                        case CellValues.Number:
                            return double.Parse(cell.CellValue.InnerText);

                        case CellValues.InlineString:
                            return cell.InlineString.InnerText;

                        case CellValues.Error:
                        default:
                            break;
                    }
                }
                else if (cell.CellValue != null)
                {
                    if (ExcelDateHelper.IsDateTimeCell(spreadsheetDocument.WorkbookPart, cell))
                        return DateTime.FromOADate(double.Parse(cell.CellValue.InnerText));

                    if (double.TryParse(cell.CellValue.InnerText, out var doubleValue))
                        return doubleValue;
                    else
                        return cell.CellValue.InnerText;
                }
            }
            return null;
        }
        
    }
}