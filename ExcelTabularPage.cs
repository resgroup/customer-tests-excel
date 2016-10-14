using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RES.Specification
{
    internal class ExcelTabularPage : ITabularPage
    {
        private readonly IFormatProvider _formatProvider = new CultureInfo("en-GB");
        WorksheetPart _worksheetPart;
        ChartsheetPart _chartsheetPart;
        Sheet _sheet;
        Stylesheet _stylesheet;
        Workbook _workbook;
        SpreadsheetDocument _package;
        ExcelTabularBook _parent;

        // this is for GetRow etc
        private SheetData _sheetData;
        private SheetData _rowLookupCacheAssociatedSheet = null;
        private Dictionary<uint, Row> _rowLookupCache = null;
        private IEnumerator<Row> _rowLookupCacheEnumerator;

        // this is for get cell value
        private SharedStringTable _sharedStringTable;

        public object this[uint row, uint column]
        {
            get { return GetCell(row, column).Value; }
            set { SetCell(row, column, value); }
        }

        internal ExcelTabularBook WorkBook => _parent;

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

                // standalone tocheck var corrector = new SheetAndRangeCountCorrector(package.ExtendedFilePropertiesPart.Properties);
                // standalone tocheck corrector.Correct();
            }

            return new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
        }

        internal ExcelTabularPage(SpreadsheetDocument package, Workbook workbook, WorksheetPart worksheetPart, Sheet sheet, Stylesheet stylesheet, uint index, ExcelTabularBook parent)
        {
            _package = package;
            _workbook = workbook;
            _worksheetPart = worksheetPart;
            _chartsheetPart = null;
            _sheet = sheet;
            _stylesheet = stylesheet;
            _index = index;
            _parent = parent;
            _sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            _sharedStringTable = package.WorkbookPart.SharedStringTablePart.SharedStringTable;

            PageSetup pageSetup = worksheetPart.Worksheet.GetFirstChild<PageSetup>();
            if (pageSetup != null)
                pageSetup.VerticalDpi = null;
        }

        // standalone tocheck
        //internal ExcelTabularPage(SpreadsheetDocument package, Workbook workbook, ChartsheetPart chartsheetPart, Sheet sheet, Stylesheet stylesheet, uint index, ExcelTabularBook parent)
        //{
        //    _package = package;
        //    _workbook = workbook;
        //    _worksheetPart = null;
        //    _chartsheetPart = chartsheetPart;
        //    _sheet = sheet;
        //    _stylesheet = stylesheet;
        //    _index = index;
        //    _parent = parent;
        //    _sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
        //    PageSetup pageSetup = chartsheetPart.Chartsheet.GetFirstChild<PageSetup>();

        //    if (pageSetup != null)
        //        pageSetup.VerticalDpi = null;
        //}

        private uint _index;

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
                return _sheet.Name;
            }
            set
            {
                if (value != _sheet.Name)
                {
                    SheetNameSafe(value);
                    string oldName = _sheet.Name;
                    _sheet.Name = value;
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
                return _sheet.State == null ? true : _sheet.State.Value == SheetStateValues.Visible;

            }
            set
            {
                SheetStateValues values = value ? SheetStateValues.Visible : SheetStateValues.Hidden;
                if (_sheet.State == null)
                    _sheet.State = new EnumValue<SheetStateValues>(values);
                else
                    _sheet.State.Value = values;
            }
        }

        // standalone tocheck
        //public IExcelColumn GetColumn(uint columnIndex)
        //{
        //    ThrowExceptionIfChartSheet("GetColumn(uint)");
        //    return new OpenXMLExcelColumn(
        //        _worksheetPart.Worksheet,
        //        _stylesheet,
        //        columnIndex);
        //}

        private void ThrowExceptionIfChartSheet(string functionName)
        {
            if (_worksheetPart == null && _chartsheetPart != null)
            {
                throw new ApplicationException(string.Format("Cannot call {0} on '{1}' as it is a ChartSheet", functionName, _sheet.Name));
            }
        }

        // standalone tocheck
        //public IExcelRow GetRow(uint rowIndex)
        //{
        //    ThrowExceptionIfChartSheet("GetRow(uint)");
        //    return new OpenXMLExcelRow(_worksheetPart.Worksheet, _stylesheet, rowIndex);
        //}

        // standalone tocheck
        //public IExcelRange GetRange(string range)
        //{
        //    if (_parent.NamedRanges.ContainsRange(Name, range))
        //        return _parent.NamedRanges.GetRange(Name, range).Value;
        //    else if (_parent.NamedRanges.ContainsBookLevelRange(range))
        //        return _parent.NamedRanges.GetBookLevelRange(range).Value;
        //    else
        //        throw new RangeNotFoundException(string.Format("Specified named range ('{0}') not found in this sheet ('{1}').", range, Name));
        //}

        public uint MaxColumn
        {
            get
            {
                // standalone todo - cache this value and add 20 as it doesn't always seem to work
                // maybe it is just counting cells with something in for the row?
                ThrowExceptionIfChartSheet("MaxColumn");
                uint maxColumns = 0;
                foreach (var row in _worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>())
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
                var rows = _worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>();
                if (rows.Any() == false) return 0;
                return rows.Max(r => r.RowIndex.Value);
            }
        }

        // standalone tocheck
        //private IExcelRange GetRange(DefinedName name)
        //{
        //    if (name.LocalSheetId != null && name.LocalSheetId != _index)
        //        throw new RangeNotFoundException("Cannot access a range from another sheet");

        //    string reference = name.InnerText;

        //    int index;
        //    string sheetName;

        //    if (reference.StartsWith("'"))
        //    {
        //        sheetName = reference.Split('\'')[1];
        //        index = sheetName.Length + 2;
        //    }
        //    else
        //    {
        //        sheetName = reference.Split('!')[0];
        //        index = sheetName.Length;
        //    }
        //    IExcelWorksheet sheetContainingData = _parent.GetWorkSheet(sheetName);

        //    if (reference[index++] != '!')
        //        throw new Exception("Cannot parse range reference");
        //    if (reference[index++] != '$')
        //        throw new Exception("Cannot parse range reference");
        //    uint left = 0;
        //    while (Char.IsUpper(reference[index]))
        //        left = left * 26 + (uint)((reference[index++] - 'A') + 1);
        //    if (reference[index++] != '$')
        //        throw new Exception("Cannot parse range reference");
        //    uint top = 0;
        //    while (index < reference.Length && Char.IsDigit(reference[index]))
        //        top = top * 10 + reference[index++] - '0';

        //    if (index >= reference.Length || reference[index] == 0)
        //        return sheetContainingData.GetCell(top, left);

        //    if (reference[index++] != ':')
        //        throw new Exception("Cannot parse range reference");
        //    if (reference[index++] != '$')
        //        throw new Exception("Cannot parse range reference");
        //    uint right = 0;
        //    while (Char.IsUpper(reference[index]))
        //        right = right * 26 + (uint)((reference[index++] - 'A') + 1);
        //    if (reference[index++] != '$')
        //        throw new Exception("Cannot parse range reference");
        //    uint bottom = 0;
        //    while (index < reference.Length && Char.IsDigit(reference[index]))
        //        bottom = bottom * 10 + reference[index++] - '0';

        //    return sheetContainingData.GetRange(top, left, bottom, right);

        //}

        // standalone tocheck
        //public IExcelRange GetRange(uint top, uint left, uint bottom, uint right)
        //{
        //    if (top == 0 || left == 0 || top > bottom || left > right)
        //        throw new IndexOutOfRangeException();

        //    ThrowExceptionIfChartSheet("GetRange(uint, uint, uint, uint)");
        //    if (_package.WorkbookPart.SharedStringTablePart == null)
        //        return new OpenXMLExcelRange(_worksheetPart.Worksheet, _stylesheet, top, left, bottom, right);
        //    else
        //        return new OpenXMLExcelRange(_worksheetPart.Worksheet, _stylesheet, top, left, bottom, right, _package.WorkbookPart.SharedStringTablePart.SharedStringTable);
        //}

        //standalone todo tidy this up, make it faster
        //this is surprisingly difficult and slow just to get the value of a cell.
        public void SetCell(uint row, uint column, object value)
        {
            if (row == 0 || column == 0)
                throw new IndexOutOfRangeException();

            CellValue cellValue;
            InlineString cellString;
            EnumValue<CellValues> cellDataType;

            GetCellProperties(value, out cellString, out cellValue, out cellDataType);

            var cell = GetOrAddCell(column, row);
            cell.InlineString = cellString;
            cell.CellValue = cellValue;
            cell.DataType = cellDataType;
        }

        public Row GetOrAddRow(uint rowIndex)
        {
            Row row = GetRow(rowIndex);
            if (row == null)
            {
                row = new Row() { RowIndex = rowIndex };
                AddRow(row);
            }
            return row;
        }

        public IEnumerator<Row> GetRowEnumerator()
        {
            return _sheetData.Elements<Row>().GetEnumerator();
        }

        public Row GetRow(uint rowIndex)
        {
            if (_rowLookupCacheAssociatedSheet != _sheetData)
            {
                _rowLookupCache = new Dictionary<uint, Row>();
                _rowLookupCacheEnumerator = GetRowEnumerator();
                _rowLookupCacheEnumerator.MoveNext();
                _rowLookupCacheAssociatedSheet = _sheetData;
            }

            // See if we already have it in the cache
            if (_rowLookupCache.Keys.Contains(rowIndex))
                return _rowLookupCache[rowIndex];

            // ok, is it at the enumerators current position (don't think it can be...)            
            if (_rowLookupCacheEnumerator.Current != null && _rowLookupCacheEnumerator.Current.RowIndex == rowIndex)
            {
                _rowLookupCache.Add(rowIndex, _rowLookupCacheEnumerator.Current);
                return _rowLookupCacheEnumerator.Current;
            }

            // Try the next position
            _rowLookupCacheEnumerator.MoveNext();
            if (_rowLookupCacheEnumerator.Current != null && _rowLookupCacheEnumerator.Current.RowIndex == rowIndex)
            {
                _rowLookupCache.Add(rowIndex, _rowLookupCacheEnumerator.Current);
                return _rowLookupCacheEnumerator.Current;
            }

            // OK - we're going to have to try an exhaustive search
            _rowLookupCacheEnumerator = GetRowEnumerator();
            while (_rowLookupCacheEnumerator.MoveNext())
            {
                if (_rowLookupCacheEnumerator.Current.RowIndex == rowIndex)
                {
                    _rowLookupCache.Add(rowIndex, _rowLookupCacheEnumerator.Current);
                    return _rowLookupCacheEnumerator.Current;
                }
            }

            return null;
        }

        private void AddRow(Row row)
        {
            _rowLookupCache = null;
            _rowLookupCacheAssociatedSheet = null;
            _rowLookupCacheEnumerator = null;
            if (_sheetData.Elements<Row>().Count() > 0)
            {
                if (_sheetData.Elements<Row>().Last<Row>() != null &&
                    _sheetData.Elements<Row>().Last<Row>().RowIndex > row.RowIndex.Value)
                {
                    // Add row at the correct position in the list
                    List<Row> sheetRows = new List<Row>();
                    using (IEnumerator<Row> enumerator = GetRowEnumerator())
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
                    _sheetData.RemoveAllChildren<Row>();
                    foreach (Row sheetRow in sheetRows)
                    {
                        _sheetData.Append(sheetRow);
                    }
                }
                else
                {
                    // Add row to the end of the row list
                    _sheetData.Append(row);
                }
            }
            else
            {
                // Add first row to the end of the row list
                _sheetData.Append(row);
            }
        }

        // slight variation of http://msdn.microsoft.com/en-us/library/cc861607(office.14).aspx#InsertCell
        protected Cell GetOrAddCell(uint columnIndex, uint rowIndex)
        {
            string columnName = GetColumnName(columnIndex);
            string cellReference = columnName + rowIndex;

            Row row = GetOrAddRow(rowIndex);

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

                Cell newCell = new Cell() { CellReference = cellReference, StyleIndex = 0U };
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

        private string ConvertDateTimeToString(DateTime dateTime)
        {
            return dateTime.ToOADate().ToString(_formatProvider);
        }

        private void GetCellProperties(object value, out InlineString stringValue, out CellValue cellValue, out EnumValue<CellValues> cellDataType)
        {
            value = MakeInfinityAString(value);

            cellValue = null;
            stringValue = null;
            cellDataType = null;

            if (value == null || value is System.Reflection.Missing)
                return;

            else if (value is string)
            {
                stringValue = new InlineString(new Text(value as string));
            }
            else if (value is DateTime)
            {
                cellValue = new CellValue(ConvertDateTimeToString((DateTime)value));
            }
            else if (value is Boolean)
            {
                cellValue = new CellValue(Convert.ToInt32(value).ToString(_formatProvider));
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
                cellValue = new CellValue(string.Format(_formatProvider, "{0}", value));
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

            var cell = _worksheetPart.Worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference == cellReference);

            return (cell == null) ? null : new TabularCell(GetCellValue(cell), cell.CellFormula?.Text != null);
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

        private string CellReference(uint columnIndex, uint rowIndex)
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
                                if (_sharedStringTable == null)
                                    throw new Exception("Cell references shared string table, but table not found");

                                return _sharedStringTable.ElementAt(sharedStringIndex).InnerText;
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
                    if (cell.StyleIndex != null && cell.StyleIndex.HasValue)
                    {
                        var spreadsheetDocument = _package;
                        if (spreadsheetDocument != null)
                        {
                            var cellFormats = spreadsheetDocument.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
                            if (cell.StyleIndex.Value < cellFormats.Count)
                            {
                                //see http://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.numberingformat(v=office.14).aspx
                                //and http://stackoverflow.com/questions/4730152/what-indicates-an-office-open-xml-cell-contains-a-date-time-value
                                var cellFormat = cellFormats.ElementAt((int)cell.StyleIndex.Value) as CellFormat;
                                if (cellFormat != null && cellFormat.NumberFormatId.HasValue)
                                {
                                    var numberFormat = cellFormat.NumberFormatId.Value;

                                    if (numberFormat >= 14 && numberFormat <= 17 || numberFormat == 22 || numberFormat == 30) // date with or without time
                                        return DateTime.FromOADate(double.Parse(cell.CellValue.InnerText));

                                    else if (numberFormat >= 18 && numberFormat <= 21 || numberFormat == 45 || numberFormat == 46) // time only
                                        return DateTime.FromOADate(double.Parse(cell.CellValue.InnerText)).TimeOfDay;
                                }
                            }
                        }
                    }

                    double doubleValue;
                    if (double.TryParse(cell.CellValue.InnerText, out doubleValue))
                        return doubleValue;
                    else
                        return cell.CellValue.InnerText;
                }
            }
            return null;

        }

        public void Activate()
        {

        }

        // standalone tocheck
        //public IExcelWorksheet Copy(string newSheetName)
        //{
        //    if (_worksheetPart != null)
        //    {
        //        return CopyWorkSheet(newSheetName);
        //    }
        //    else if (_chartsheetPart != null)
        //    {
        //        return CopyChartSheet(newSheetName);
        //    }
        //    else
        //    {
        //        throw new ApplicationException(string.Format("Cannot copy sheet '{0}' as it is not a Worksheet or Chartsheet", _sheet.Name));
        //    }
        //}

        // standalone tocheck
        //private IExcelWorksheet CopyWorkSheet(string newSheetName)
        //{
        //    // Can force a copy of a worksheet part by adding the sheet to another workbook then copying it back
        //    SpreadsheetDocument tempSheet = SpreadsheetDocument.Create(new System.IO.MemoryStream(), SpreadsheetDocumentType.Workbook);
        //    WorkbookPart tempWorkbookPart = tempSheet.AddWorkbookPart();
        //    WorksheetPart tempWorksheetPart = tempWorkbookPart.AddPart<WorksheetPart>(_worksheetPart);
        //    WorksheetPart clonedWorkSheetPart = _workbook.WorkbookPart.AddPart<WorksheetPart>(tempWorksheetPart);

        //    // Add a new sheet to contain the worksheet part
        //    Sheets sheets = _workbook.GetFirstChild<Sheets>();
        //    uint sheetId = 1;
        //    if (sheets.Elements<Sheet>().Count() > 0)
        //    {
        //        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
        //    }
        //    string relationshipId = _workbook.WorkbookPart.GetIdOfPart(clonedWorkSheetPart);
        //    Sheet sheet = CreateBlankSheet(relationshipId, sheetId, newSheetName, _package);
        //    sheets.Append(sheet);

        //    uint sheetIndex = 0;
        //    foreach (Sheet checkSheet in sheets.Elements<Sheet>())
        //    {
        //        if (checkSheet.SheetId == sheet.SheetId) break;
        //        sheetIndex++;
        //    }

        //    // Copy any local named ranges
        //    _parent.NamedRanges.CopyingSheet(Name, newSheetName);

        //    OpenXMLExcelWorksheet newSheet = new OpenXMLExcelWorksheet(_package, _workbook, clonedWorkSheetPart, sheet, _stylesheet, sheetIndex, _parent);
        //    newSheet.Visible = Visible;

        //    return newSheet;

        //}

        // standalone tocheck
        //private IExcelWorksheet CopyChartSheet(string newSheetName)
        //{
        //    // Can force a copy of a worksheet part by adding the sheet to another workbook then copying it back
        //    SpreadsheetDocument tempSheet = SpreadsheetDocument.Create(new System.IO.MemoryStream(), SpreadsheetDocumentType.Workbook);
        //    WorkbookPart tempWorkbookPart = tempSheet.AddWorkbookPart();
        //    ChartsheetPart tempChartsheetPart = tempWorkbookPart.AddPart<ChartsheetPart>(_chartsheetPart);
        //    ChartsheetPart clonedChartsheetPart = _workbook.WorkbookPart.AddPart<ChartsheetPart>(tempChartsheetPart);

        //    // Add a new sheet to contain the worksheet part
        //    Sheets sheets = _workbook.GetFirstChild<Sheets>();
        //    uint sheetId = 1;
        //    if (sheets.Elements<Sheet>().Count() > 0)
        //    {
        //        sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
        //    }
        //    string relationshipId = _workbook.WorkbookPart.GetIdOfPart(clonedChartsheetPart);
        //    Sheet sheet = CreateBlankSheet(relationshipId, sheetId, newSheetName, _package);
        //    sheets.Append(sheet);

        //    uint sheetIndex = 0;
        //    foreach (Sheet checkSheet in sheets.Elements<Sheet>())
        //    {
        //        if (checkSheet.SheetId == sheet.SheetId) break;
        //        sheetIndex++;
        //    }

        //    // Copy any local named ranges
        //    _parent.NamedRanges.CopyingSheet(Name, newSheetName);

        //    OpenXMLExcelWorksheet newSheet = new OpenXMLExcelWorksheet(_package, _workbook, clonedChartsheetPart, sheet, _stylesheet, sheetIndex, _parent);
        //    newSheet.Visible = Visible;

        //    return newSheet;

        //}

        // standalone tocheck
        //public void Delete()
        //{
        //    uint sheetID = (uint)_sheet.SheetId;
        //    int sheetIndex = _parent.GetSheetIndex(Name);
        //    _parent.NamedRanges.DeletingSheet(Name);
        //    if (_package.ExtendedFilePropertiesPart != null)
        //    {
        //        List<OpenXmlElement> elementsToRemove = new List<OpenXmlElement>();
        //        foreach (OpenXmlElement titleOfPart in _package.ExtendedFilePropertiesPart.Properties.TitlesOfParts.VTVector.ChildElements)
        //        {

        //            if (titleOfPart.InnerText == Name || titleOfPart.InnerText.StartsWith(string.Format("{0}!", Name)) || titleOfPart.InnerText.StartsWith(string.Format("'{0}'!", Name)))
        //            {
        //                elementsToRemove.Add(titleOfPart);
        //            }
        //        }
        //        foreach (OpenXmlElement element in elementsToRemove)
        //            element.Remove();

        //        var corrector = new SheetAndRangeCountCorrector(_package.ExtendedFilePropertiesPart.Properties);
        //        corrector.Correct();
        //    }

        //    _sheet.Remove();

        //    foreach (var namedRange in _parent.NamedRanges)
        //        namedRange.UpdateIdAsSheetRemoved(sheetIndex);

        //    if (_workbook.WorkbookPart.CalculationChainPart != null)
        //    {
        //        // Excel will automatically recreate this itself when the document is loaded
        //        _workbook.WorkbookPart.DeletePart(_workbook.WorkbookPart.CalculationChainPart);
        //    }
        //}

        // standalone tocheck
        //public void WriteValueToRange(string rangeName, object value)
        //{
        //    GetRange(rangeName).Value = value;
        //}

        // standalone tocheck
        //public void WriteValuesToRange(string rangeName, object[,] value)
        //{
        //    GetRange(rangeName).Value = value;
        //}

        // standalone tocheck
        //public void WriteInternalHyperlinkToRange(IExcelRange range, string title, string location)
        //{
        //    range.AddInternalHyperlink(title, location);
        //}

        public void Dispose()
        {
        }


    }
}