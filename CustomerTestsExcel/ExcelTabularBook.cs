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
        const string CORE_FILE_PROPERTIES_PART_ID = "rId2";
        const string WORKBOOK_STYLES_PART_ID = "rId3";

        public string Filename { get; set; }

        Stream outputStream;
        readonly SpreadsheetDocument package;
        WorkbookPart workbookPart;
        Workbook workbook;
        string tempFilePath;

        public ExcelTabularBook()
        {
            outputStream = new MemoryStream();
            tempFilePath = "";
            package = SpreadsheetDocument.Create(outputStream, SpreadsheetDocumentType.Workbook);

            var coreFilePropertiesPart = package.AddNewPart<CoreFilePropertiesPart>(CORE_FILE_PROPERTIES_PART_ID);
            GenerateCoreFilePropertiesPart(coreFilePropertiesPart);

            workbookPart = package.AddWorkbookPart();

            var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>(WORKBOOK_STYLES_PART_ID);
            GenerateWorkbookStylesPart().Save(workbookStylesPart);

            workbook = AddWorkBook(workbookPart);
            workbook.AppendChild(new Sheets());
        }

        internal ExcelTabularBook(Stream stream, bool editable = false)
        {
            outputStream = stream ?? throw new ArgumentNullException(nameof(stream));
            package = SpreadsheetDocument.Open(outputStream, editable);
            workbookPart = package.WorkbookPart;
            workbook = package.WorkbookPart.Workbook;
            Filename = null;
            tempFilePath = "";
        }

        internal ExcelTabularBook(string existingFilePath)
        {
            if (string.IsNullOrEmpty(existingFilePath))
                throw new ArgumentNullException(nameof(existingFilePath));

            if (!File.Exists(existingFilePath))
                throw new Exception(string.Format("Specified excel template '{0}' does not exist.", existingFilePath));

            outputStream = null;
            tempFilePath = Path.GetTempFileName();
            File.Copy(existingFilePath, tempFilePath, true);
            FileInfo tempFileInfo = new FileInfo(tempFilePath);
            tempFileInfo.IsReadOnly = false;
            package = SpreadsheetDocument.Open(tempFilePath, true);
            workbookPart = package.WorkbookPart;
            workbook = package.WorkbookPart.Workbook;
            Filename = existingFilePath;
        }

        public int NumberOfPages => workbookPart.WorksheetParts.Count();

        public string[] GetPageNames() => workbook.GetFirstChild<Sheets>().Elements<Sheet>().Select(s => s.Name.Value).ToArray();

        public void SaveAs(string filename)
        {
            Filename = filename;
            Save();
        }

        public ITabularPage GetPage(string sheetName) => GetPage(GetSheetIndex(sheetName));

        public ITabularPage GetPage(int index)
        {
            var sheet = GetSheet(index);
            var part = workbookPart.GetPartById(sheet.Id);
            if (part is WorksheetPart)
            {
                return new ExcelTabularPage(package, workbook, part as WorksheetPart, sheet, workbookPart.WorkbookStylesPart.Stylesheet, (uint)index, this);
            }
            else if (part is ChartsheetPart)
            {
                return null;
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
            var sheets = workbook.GetFirstChild<Sheets>();

            var worksheet = SetupWorksheet(sheets);

            sheets.InsertAt<Sheet>(worksheet, workSheetIndex);
            workbook.Save();

            return GetPage(worksheet.Name);
        }


        internal DefinedNames DefinedNames
        {
            get
            {
                var names = workbook.GetFirstChild<DefinedNames>();

                if (names == null)
                {
                    names = new DefinedNames();
                    workbook.InsertAfter(names, workbook.GetFirstChild<Sheets>());
                }

                return names;
            }
        }

        public void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            foreach (var worksheetPart in workbookPart.WorksheetParts)
                worksheetPart.Worksheet.Save(worksheetPart);
            workbookPart.WorkbookStylesPart.Stylesheet.Save(workbookPart.WorkbookStylesPart);
            workbook.Save(workbookPart);
            package.Close();

            if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
            {
                using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
                    fileStream.CopyTo(stream);

                File.Delete(tempFilePath);
            }
            else
            {
                outputStream.Seek(0, SeekOrigin.Begin);
                outputStream.CopyTo(stream);
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Filename))
                throw new Exception("Cannot save a file before the path is set");

            using (var fileStream = new FileStream(Filename, FileMode.Create, FileAccess.ReadWrite))
                Save(fileStream);
        }
 
        public int GetSheetIndex(string sheetName)
        {
            int index = 0;
            foreach (var sheet in workbook.Elements<Sheets>().First().Elements<Sheet>())
            {
                if (sheet.Name == sheetName)
                    return index;
                index++;
            }
            throw new Exception("Could not find sheet " + sheetName);
        }

        Sheet GetSheet(int index) => workbook.Elements<Sheets>().First().Elements<Sheet>().ElementAt(index); 

        public string GetSheetName(int index) => GetSheet(index).Name;

        public bool SheetIsWorksheet(string sheetName)
        {
            var sheet = GetSheet(GetSheetIndex(sheetName));
            return workbookPart.GetPartById(sheet.Id) is WorksheetPart;
        }

        public ITabularPage AddWorkSheet()
        {
            // from http://msdn.microsoft.com/en-us/library/cc861607(office.14).aspx#InsertWorksheet

            var sheets = workbook.GetFirstChild<Sheets>();

            var worksheet = SetupWorksheet(sheets);

            // Append the new worksheet and associate it with the workbook.
            sheets.Append(worksheet);
            workbook.Save(workbookPart);

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
            var newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = ExcelTabularPage.CreateBlankWorksheet(selected);

            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            return ExcelTabularPage.CreateBlankSheet(relationshipId, sheetId, sheetName, package);
        }

        public ITabularPage AddPageAfter(int workSheetIndex)
        {
            var sheets = workbook.GetFirstChild<Sheets>();

            var worksheet = SetupWorksheet(sheets);

            if (sheets.Count() <= workSheetIndex)
                sheets.Append(worksheet);
            else
                sheets.InsertAt<Sheet>(worksheet, workSheetIndex + 1);
            workbook.Save();

            return GetPage(worksheet.Name);
        }

        public void RemoveDefaultSheets()
        {
            for (int i = 1; i <= 3; i++)
            {
                string sheetName = string.Format("Sheet{0}", i);
                DeleteWorkSheet(sheetName);
            }
        }

        public void Dispose()
        {
            if (package != null)
                package.Dispose();
            if (outputStream != null)
                outputStream.Dispose();
            if (!string.IsNullOrEmpty(tempFilePath))
                File.Delete(tempFilePath);
        }

        private Workbook AddWorkBook(WorkbookPart workbookPart)
        {
            workbookPart.Workbook = new Workbook(
                new FileVersion() { ApplicationName = "xl", LastEdited = "4", LowestEdited = "4", BuildVersion = "4506" },
                new WorkbookProperties() { CheckCompatibility = true, DefaultThemeVersion = 124226U },
                new BookViews(
                    new WorkbookView() { XWindow = 180, YWindow = 435, WindowWidth = 18855U, WindowHeight = 8895U })
            );
            return workbookPart.Workbook;
        }

        private void GenerateCoreFilePropertiesPart(OpenXmlPart part)
        {
            var writer = new System.Xml.XmlTextWriter(part.GetStream(), System.Text.Encoding.UTF8);
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
                            new Color() { Theme = 1U },
                            new FontName() { Val = "Calibri" },
                            new FontFamilyNumbering() { Val = 2 },
                            new FontScheme() { Val = FontSchemeValues.Minor }),
                        new Font(
                            new Underline(),
                            new FontSize() { Val = 11D },
                            new Color() { Indexed = 12U },
                            new FontName() { Val = "Calibri" },
                            new FontFamilyNumbering() { Val = 2 })
                    )
                    { Count = 2U },
                    new Fills(
                        new Fill(
                            new PatternFill() { PatternType = PatternValues.None }),
                        new Fill(
                            new PatternFill() { PatternType = PatternValues.Gray125 })
                    )
                    { Count = 2U },
                    new Borders(
                        new Border(
                            new LeftBorder(),
                            new RightBorder(),
                            new TopBorder(),
                            new BottomBorder(),
                            new DiagonalBorder())
                    )
                    { Count = 1U },
                    new CellStyleFormats(
                        new CellFormat() { NumberFormatId = 0U, FontId = 0U, FillId = 0U, BorderId = 0U },
                        new CellFormat() { NumberFormatId = 0U, FontId = 1U, FillId = 0U, BorderId = 0U }
                    )
                    { Count = 2U },
                    new CellFormats(
                        new CellFormat() { NumberFormatId = 0U, FontId = 0U, FillId = 0U, BorderId = 0U, FormatId = 0U },
                        new CellFormat() { NumberFormatId = 0U, FontId = 1U, FillId = 0U, BorderId = 0U, FormatId = 1U }
                    )
                    { Count = 2U },
                    new CellStyles(
                        new CellStyle() { Name = "Hyperlink", FormatId = 1U, BuiltinId = 8U },
                        new CellStyle() { Name = "Normal", FormatId = 0U, BuiltinId = 0U }
                    )
                    { Count = 2U },
                    new DifferentialFormats() { Count = 0U },
                    new TableStyles() { Count = 0U, DefaultTableStyle = "TableStyleMedium9", DefaultPivotStyle = "PivotStyleLight16" });
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

            var sheets = workbook.GetFirstChild<Sheets>();
            var theSheet = sheets.Elements<Sheet>().Single(s => s.Name == nameOfWorkSheetToDelete);

            if (theSheet == null)
            {
                return false;
            }

            var sheetIndex = GetSheetIndex(nameOfWorkSheetToDelete);

            // Remove the sheet reference from the workbook.
            sheets.RemoveChild(theSheet);

            // Delete the worksheet part.
            workbookPart.DeletePart(workbookPart.GetPartById(theSheet.Id));

            workbook.Save(workbookPart);

            return true;
        }

    }
}