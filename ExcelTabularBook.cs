using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.IO;
using System.Linq;
using static System.Reflection.Assembly;

namespace CustomerTestsExcel
{
    public class ExcelTabularBook : ITabularBook
    {
        public int NumberOfPages => _workbookPart.WorksheetParts.Count();

        public string[] GetPageNames() => _workbook.GetFirstChild<Sheets>().Elements<Sheet>().Select(s => s.Name.Value).ToArray();

        public void SaveAs(string path)
        {
            _path = path;
            Save();
        }

        public ITabularPage GetPage(string sheetName) => GetPage(GetSheetIndex(sheetName));

        public ITabularPage GetPage(int index)
        {
            var sheet = GetSheet(index);
            var part = _workbookPart.GetPartById(sheet.Id);
            if (part is WorksheetPart)
            {
                return new ExcelTabularPage(_package, _workbook, part as WorksheetPart, sheet, _workbookPart.WorkbookStylesPart.Stylesheet, (uint)index, this);
            }
            else if (part is ChartsheetPart)
            {
                //standalone tocheck
                return null;
                //return new ExcelTabularPage(_package, _workbook, part as ChartsheetPart, sheet, _workbookPart.WorkbookStylesPart.Stylesheet, (uint)index, this);
            }
            else if (part == null)
            {
                throw new ApplicationException(string.Format("WorksheetPart not found for sheet '{0}'", sheet.Name));
            }
            else
            {
                throw new ApplicationException(string.Format("Sheet '{0}' is not a worksheet or chartsheet.", sheet.Name));
            }
        }

        public ITabularPage AddPageBefore(int workSheetIndex)
        {
            Sheets sheets = _workbook.GetFirstChild<Sheets>();

            Sheet worksheet = SetupWorksheet(sheets);

            sheets.InsertAt<Sheet>(worksheet, workSheetIndex);
            _workbook.Save();

            return GetPage(worksheet.Name);
        }


        const string CORE_FILE_PROPERTIES_PART_ID = "rId2";
        const string WORKBOOK_STYLES_PART_ID = "rId3";

        string _path;
        Stream _outputStream;
        readonly SpreadsheetDocument _package;
        WorkbookPart _workbookPart;
        Workbook _workbook;
        string _tempFilePath;

        internal DefinedNames DefinedNames
        {
            get
            {
                DefinedNames names = _workbook.GetFirstChild<DefinedNames>();

                if (names == null)
                {
                    names = new DefinedNames();
                    _workbook.InsertAfter(names, _workbook.GetFirstChild<Sheets>());
                }

                return names;
            }
        }

        #region Constructors

        public ExcelTabularBook()
        {
            _outputStream = new MemoryStream();
            _tempFilePath = "";
            _package = SpreadsheetDocument.Create(_outputStream, SpreadsheetDocumentType.Workbook);

            var coreFilePropertiesPart = _package.AddNewPart<CoreFilePropertiesPart>(CORE_FILE_PROPERTIES_PART_ID);
            GenerateCoreFilePropertiesPart(coreFilePropertiesPart);

            _workbookPart = _package.AddWorkbookPart();

            WorkbookStylesPart workbookStylesPart = _workbookPart.AddNewPart<WorkbookStylesPart>(WORKBOOK_STYLES_PART_ID);
            GenerateWorkbookStylesPart().Save(workbookStylesPart);

            _workbook = AddWorkBook(_workbookPart);
            _workbook.AppendChild(new Sheets());
        }

        internal ExcelTabularBook(Stream stream, bool editable = false)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            _outputStream = stream;
            _package = SpreadsheetDocument.Open(_outputStream, editable);
            _workbookPart = _package.WorkbookPart;
            _workbook = _package.WorkbookPart.Workbook;
            _path = null;
            _tempFilePath = "";
        }

        internal ExcelTabularBook(string existingFilePath)
        {
            if (string.IsNullOrEmpty(existingFilePath))
                throw new ArgumentNullException(nameof(existingFilePath));

            if (!System.IO.File.Exists(existingFilePath))
                throw new Exception(string.Format("Specified excel template '{0}' does not exist.", existingFilePath));

            // Temporary workaround
            _outputStream = null;
            _tempFilePath = Path.GetTempFileName();
            File.Copy(existingFilePath, _tempFilePath, true);
            FileInfo tempFileInfo = new FileInfo(_tempFilePath);
            tempFileInfo.IsReadOnly = false;
            _package = SpreadsheetDocument.Open(_tempFilePath, true);
            _workbookPart = _package.WorkbookPart;
            _workbook = _package.WorkbookPart.Workbook;
            _path = existingFilePath;
        }
        #endregion

        #region Private Methods

        private Workbook AddWorkBook(WorkbookPart workbookPart)
        {
            workbookPart.Workbook = new Workbook(
                new FileVersion() { ApplicationName = "xl", LastEdited = "4", LowestEdited = "4", BuildVersion = "4506" },
                new WorkbookProperties() { CheckCompatibility = true, DefaultThemeVersion = (UInt32Value)124226U },
                new BookViews(
                    new WorkbookView() { XWindow = 180, YWindow = 435, WindowWidth = (UInt32Value)18855U, WindowHeight = (UInt32Value)8895U })
            );
            return workbookPart.Workbook;
        }

        private void GenerateCoreFilePropertiesPart(OpenXmlPart part)
        {
            System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(part.GetStream(), System.Text.Encoding.UTF8);
            writer.WriteRaw(
                string.Format(
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n<cp:coreProperties xmlns:cp=\"http://schemas.openxmlformats.org/package/2006/metadata/core-properties\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:dcterms=\"http://purl.org/dc/terms/\" xmlns:dcmitype=\"http://purl.org/dc/dcmitype/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><dc:creator>{0}</dc:creator><cp:lastModifiedBy>{0}</cp:lastModifiedBy><dcterms:created xsi:type=\"dcterms:W3CDTF\">{1:s}Z</dcterms:created><dcterms:modified xsi:type=\"dcterms:W3CDTF\">{1:s}Z</dcterms:modified></cp:coreProperties>",
                    GetCreatingApplicationName(),
                    DateTime.Now));
            writer.Flush();
            writer.Close();
        }

        private Stylesheet GenerateWorkbookStylesPart()
        {
            var element =
                new Stylesheet(
                    new Fonts(
                        new Font(
                            new FontSize() { Val = 11D },
                            new Color() { Theme = (UInt32Value)1U },
                            new FontName() { Val = "Calibri" },
                            new FontFamilyNumbering() { Val = 2 },
                            new FontScheme() { Val = FontSchemeValues.Minor }),
                        new Font(
                            new Underline(),
                            new FontSize() { Val = 11D },
                            new Color() { Indexed = (UInt32Value)12U },
                            new FontName() { Val = "Calibri" },
                            new FontFamilyNumbering() { Val = 2 })
                    )
                    { Count = (UInt32Value)2U },
                    new Fills(
                        new Fill(
                            new PatternFill() { PatternType = PatternValues.None }),
                        new Fill(
                            new PatternFill() { PatternType = PatternValues.Gray125 })
                    )
                    { Count = (UInt32Value)2U },
                    new Borders(
                        new Border(
                            new LeftBorder(),
                            new RightBorder(),
                            new TopBorder(),
                            new BottomBorder(),
                            new DiagonalBorder())
                    )
                    { Count = (UInt32Value)1U },
                    new CellStyleFormats(
                        new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U },
                        new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U }
                    )
                    { Count = (UInt32Value)2U },
                    new CellFormats(
                        new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U },
                        new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)1U }
                    )
                    { Count = (UInt32Value)2U },
                    new CellStyles(
                        new CellStyle() { Name = "Hyperlink", FormatId = (UInt32Value)1U, BuiltinId = (UInt32Value)8U },
                        new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U }
                    )
                    { Count = (UInt32Value)2U },
                    new DifferentialFormats() { Count = (UInt32Value)0U },
                    new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium9", DefaultPivotStyle = "PivotStyleLight16" });
            return element;
        }

        string GetCreatingApplicationName()
        {
            var splitString = GetCreatingApplicationFullNameSplitByCommas();
            return (splitString.Length > 0) ? splitString[0] : "Unknown";
        }

        string[] GetCreatingApplicationFullNameSplitByCommas() =>
            GetEntryAssembly()?.FullName?.Split(',') ?? new string[] { };

        public bool DeleteWorkSheet(string nameOfWorkSheetToDelete)
        {
            // Delete the specified sheet.
            // Return True if the sheet was found and deleted, False if it was not.
            // Note that this procedure might leave "orphaned" references, such as strings
            // in the shared strings table. You must take care when adding new strings, for example. 

            Sheets sheets = _workbook.GetFirstChild<Sheets>();
            Sheet theSheet = sheets.Elements<Sheet>().Single(s => s.Name == nameOfWorkSheetToDelete);

            if (theSheet == null)
            {
                return false;
            }

            var sheetIndex = GetSheetIndex(nameOfWorkSheetToDelete);

            // Remove the sheet reference from the workbook.
            sheets.RemoveChild(theSheet);

            // Delete the worksheet part.
            _workbookPart.DeletePart(_workbookPart.GetPartById(theSheet.Id));

            _workbook.Save(_workbookPart);

            return true;
        }

        #endregion

        private void Recalculate()
        {
            // standalone tocheck
            //foreach (var worksheetPart in _workbookPart.WorksheetParts)
            //{
            //    foreach (var row in new OpenXMLExcelRows(worksheetPart.Worksheet.GetFirstChild<SheetData>()))
            //    {
            //        foreach (var cell in row.Elements<Cell>())
            //        {
            //            if (cell.CellFormula != null)
            //                cell.CellFormula.CalculateCell = new BooleanValue(true);
            //        }
            //    }
            //}
        }

        public void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Recalculate();

            foreach (var worksheetPart in _workbookPart.WorksheetParts)
                worksheetPart.Worksheet.Save(worksheetPart);
            _workbookPart.WorkbookStylesPart.Stylesheet.Save(_workbookPart.WorkbookStylesPart);
            _workbook.Save(_workbookPart);
            _package.Close();

            if (!string.IsNullOrEmpty(_tempFilePath) && File.Exists(_tempFilePath))
            {
                using (var fileStream = new FileStream(_tempFilePath, FileMode.Open, FileAccess.Read))
                    fileStream.CopyTo(stream);

                File.Delete(_tempFilePath);
            }
            else
            {
                _outputStream.Seek(0, SeekOrigin.Begin);
                _outputStream.CopyTo(stream);
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_path))
                throw new Exception("Cannot save a file before the path is set");

            using (var fileStream = new FileStream(_path, FileMode.Create, FileAccess.ReadWrite))
                Save(fileStream);
        }
 
        public int GetSheetIndex(string sheetName)
        {
            int index = 0;
            foreach (var sheet in _workbook.Elements<Sheets>().First().Elements<Sheet>())
            {
                if (sheet.Name == sheetName)
                    return index;
                index++;
            }
            throw new Exception("Could not find sheet " + sheetName);
        }

        Sheet GetSheet(int index) => _workbook.Elements<Sheets>().First().Elements<Sheet>().ElementAt(index); 

        public string GetSheetName(int index) => GetSheet(index).Name;

        public bool SheetIsWorksheet(string sheetName)
        {
            Sheet sheet = GetSheet(GetSheetIndex(sheetName));
            return _workbookPart.GetPartById(sheet.Id) is WorksheetPart;
        }

        public ITabularPage AddWorkSheet()
        {
            // from http://msdn.microsoft.com/en-us/library/cc861607(office.14).aspx#InsertWorksheet

            Sheets sheets = _workbook.GetFirstChild<Sheets>();

            Sheet worksheet = SetupWorksheet(sheets);

            // Append the new worksheet and associate it with the workbook.
            sheets.Append(worksheet);
            _workbook.Save(_workbookPart);

            return GetPage(worksheet.Name);
        }

        private Sheet SetupWorksheet(Sheets sheets)
        {

            // Get a unique ID for the new sheet.
            uint sheetId = 1;
            bool selected = true;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                selected = false;
            }

            string sheetName = "Sheet" + sheetId;
            // Add a new worksheet part to the workbook.
            WorksheetPart newWorksheetPart = _workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = ExcelTabularPage.CreateBlankWorksheet(selected);

            string relationshipId = _workbookPart.GetIdOfPart(newWorksheetPart);

            return ExcelTabularPage.CreateBlankSheet(relationshipId, sheetId, sheetName, _package);
        }

        public ITabularPage AddPageAfter(int workSheetIndex)
        {
            Sheets sheets = _workbook.GetFirstChild<Sheets>();

            Sheet worksheet = SetupWorksheet(sheets);

            if (sheets.Count() <= workSheetIndex)
                sheets.Append(worksheet);
            else
                sheets.InsertAt<Sheet>(worksheet, workSheetIndex + 1);
            _workbook.Save();

            return GetPage(worksheet.Name);
        }

        public void RemoveDefaultSheets()
        {
            for (int i = 1; i <= 3; i++)
            {
                string sheetName = System.String.Format("Sheet{0}", i);
                DeleteWorkSheet(sheetName);
            }
        }

        public void Dispose()
        {
            if (_package != null)
                _package.Dispose();
            if (_outputStream != null)
                _outputStream.Dispose();
            if (!string.IsNullOrEmpty(_tempFilePath))
                File.Delete(_tempFilePath);
        }

    }
}