using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeTable : ExcelToCodeBase
    {
        public ExcelToCodeTable(ExcelToCodeState excelToCodeState)
            : base(excelToCodeState)
        {
        }

        // This is true if the current cell is the top left cell of a table property
        internal bool CanParse()
        {
            CheckMissingTableOf();

            return IsTable(excel.CurrentCell());
        }

        bool IsTable(string excelGivenLeft) =>
            excelGivenLeft.EndsWith(converter.TableOf, StringComparison.InvariantCultureIgnoreCase);

        internal void Parse()
        {
            var startCellReference = excel.CellReferenceA1Style();
            var excelGivenLeft = excel.CurrentCell();

            using (excel.AutoRestoreMoveRight())
            {
                var excelGivenRight = excel.CurrentCellRaw();
                var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

                using (code.OutputAndOpenAutoClosingBracket($".{converter.GivenTablePropertyNameExcelNameToCodeName(excelGivenLeft)}"))
                    CreateObjectsFromTable(startCellReference, excelGivenLeft, excelGivenRightString);
            }
        }

        // check to see if it looks like a table, but does not end with converter.TableOf
        void CheckMissingTableOf()
        {
            if (LooksLikeATableButIsnt())
                AddErrorToCodeAndLog($"It looks like you might be trying to set up a table, starting at cell {excel.CellReferenceA1Style()}. If this is the case, please make sure that cell {excel.CellReferenceA1Style()} ends with '{converter.TableOf}'");
        }

        bool LooksLikeATableButIsnt() =>
            (
            IsTable(excel.CurrentCell()) == false
            && excel.PeekBelowRight() == converter.WithProperties
            && (excel.PeekBelowRight(2, 1) != "" && excel.PeekBelow(2) == "")
            );

        void CreateObjectsFromTable(
            string tableStartCellReference,
            string excelGivenLeft,
            string excelGivenRightString)
        {
            string cSharpSpecificationSpecificClassName = converter.ExcelClassNameToCodeName(excelGivenRightString);

            CheckMissingWithPropertiesForTable(tableStartCellReference);

            CheckBadIndentationInTable(tableStartCellReference);

            var headers = ReadHeaders();

            CheckMissingHeadersForTable(tableStartCellReference, headers);

            CheckTableIsRoundTrippable(headers.Values);

            uint lastColumn = headers.Max(h => h.Value.EndColumn);
            uint propertiesEndColumn = lastColumn;

            code.Add($"new ReportSpecificationSetupClassUsingTable<{cSharpSpecificationSpecificClassName}>()");
            log.VisitGivenTablePropertyDeclaration(
                converter.GivenTablePropertyNameExcelNameToSutName(excelGivenLeft),
                excelGivenRightString,
                headers.Values);

            uint tableRow = 0;
            uint moveDown = 1 + (headers.Max((KeyValuePair<uint, TableHeader> h) => h.Value.EndRow) - excel.Row);
            excel.MoveDown(moveDown);
            while (TableHasMoreRows(lastColumn))
            {
                using (excel.SavePosition())
                {
                    using (code.OutputAndOpenAutoClosingBracket(".Add"))
                    {
                        log.VisitGivenTablePropertyRowDeclaration(tableRow);

                        SetAllPropertiesOnTableRowVariable(
                            cSharpSpecificationSpecificClassName,
                            excel.Column,
                            propertiesEndColumn,
                            headers,
                            tableRow);

                        log.VisitGivenTablePropertyRowFinalisation();
                    }
                    tableRow++;
                }

                excel.MoveDown();
            }

            log.VisitGivenTablePropertyFinalisation();

            CheckNoRowsInTable(tableStartCellReference, excel.CellReferenceA1Style(), tableRow);

            excel.MoveUp();
        }

        void CheckMissingHeadersForTable(string tableStartCellReference, Dictionary<uint, TableHeader> headers)
        {
            if (!headers.Any())
                throw new ExcelToCodeException($"The table starting at cell {tableStartCellReference} has no headers. There should be a row of Property Names starting at {excel.CellReferenceA1Style()}, with rows of Property Values below.");
        }

        void CheckMissingWithPropertiesForTable(string tableStartCellReference)
        {
            using (excel.AutoRestoreMoveDown())
            {
                if (excel.CurrentCell() != converter.WithProperties)
                    throw new ExcelToCodeException($"The table starting at {tableStartCellReference} is not formatted correctly. Cell {excel.CellReferenceA1Style()} should be '{converter.WithProperties}', but is '{excel.CurrentCell()}'");
            }
        }

        void CheckTableIsRoundTrippable(IEnumerable<TableHeader> tableHeaders)
        {
            if (tableHeaders.All(h => h.IsRoundTrippable))
                return;

            tableHeaders
                .Where(h => !h.IsRoundTrippable)
                .ToList()
                .ForEach(h =>
                    log.AddIssuePreventingRoundTrip($"There is a complex property ('{h.ExcelPropertyName}', cell {excel.CellReferenceA1Style()}) within a table in the Excel test, worksheet '{excel.Worksheet.Name}'")
                );
        }

        void CheckNoRowsInTable(string tableStartCellReference, string rowsStartCellReference, uint numberOfRows)
        {
            if (numberOfRows == 0)
                throw new ExcelToCodeException($"The table starting at cell {tableStartCellReference} has no rows. There should be at least one row of Property Values starting at {rowsStartCellReference}.");
        }

        Dictionary<uint, TableHeader> ReadHeaders()
        {
            excel.MoveDown();

            excel.MoveDown();

            var headers = new Dictionary<uint, TableHeader>();

            using (excel.SavePosition())
            {
                while (excel.CurrentCell() != "")
                {
                    headers.Add(excel.Column, CreatePropertyHeader());
                    excel.MoveRight();
                }
            }

            return headers;
        }

        void CheckBadIndentationInTable(string startCellReference)
        {
            using (excel.AutoRestoreMoveDown(2))
            {
                if (excel.CurrentCell() == "" && excel.PeekRight() != "")
                    throw new ExcelToCodeException($"The table starting at {startCellReference} is not formatted correctly. The properties start on column {excel.ColumnReferenceA1Style(excel.Column + 1)}, but they should start start one to the left, on column {excel.ColumnReferenceA1Style()}");
            }
        }

        TableHeader CreatePropertyHeader()
        {
            if (excel.PeekBelow(2) == converter.WithProperties)
                return CreateSubClassHeader();

            return new PropertyTableHeader(excel.CurrentCell(), excel.Row, excel.Column);
        }

        SubClassTableHeader CreateSubClassHeader()
        {
            string excelPropertyName;
            string subClassName;
            uint startRow;
            uint endRow;
            uint? propertiesStartColumn;
            uint propertiesEndColumn;
            var headers = new Dictionary<uint, TableHeader>();

            // this is a almost a straight copy of the original read proeprty headers code so we will be able to reuse it (the detection of the end of the properties is different, and the positioning is different, other than that its identical I think)
            startRow = excel.Row;
            using (excel.SavePosition())
            {
                excelPropertyName = excel.CurrentCell();
                excel.MoveDown();
                subClassName = excel.CurrentCell();
                excel.MoveDown();
                propertiesStartColumn = excel.Column;

                excel.MoveDown();
                do
                {
                    headers.Add(excel.Column, CreatePropertyHeader());
                    excel.MoveRight();
                } while (excel.PeekAbove(3) == "" && excel.CurrentCell() != "");// Need to detect end of the sub property. This is by the existence of a property name in the parent proeprty header row, which is 3 rows up, or by when there are no columns left in the table

                propertiesEndColumn = excel.Column - 1;
                endRow = excel.Row;
            }

            excel.MoveRight((uint)headers.Count - 1);

            return new SubClassTableHeader(excelPropertyName, subClassName, converter.ExcelClassNameToCodeName(subClassName), startRow, endRow, propertiesStartColumn, propertiesEndColumn, headers);
        }

        void SetAllPropertiesOnTableRowVariable(
            string cSharpSpecificationSpecificClassName,
            uint? propertiesStartColumn,
            uint propertiesEndColumn,
            Dictionary<uint, TableHeader> propertyNames,
            uint tableRow)
        {
            code.Add($"new {cSharpSpecificationSpecificClassName}()");

            SetPropertiesOnTableRowVariable(
                propertiesStartColumn,
                propertyNames,
                propertiesEndColumn,
                tableRow);
        }

        bool TableHasMoreRows(uint lastColumn)
        {
            if (excel.RowToColumnIsEmpty(lastColumn)) return false;
            if (excel.AnyPrecedingColumnHasAValue()) return false;

            return true;
        }

        void SetPropertiesOnTableRowVariable(
            uint? propertiesStartColumn,
            Dictionary<uint, TableHeader> headers,
            uint propertiesEndColumn,
            uint tableRow)
        {
            if (propertiesStartColumn.HasValue)
            {
                if (excel.Column != propertiesStartColumn.Value) throw new ExcelToCodeException("Table must have a 'With Properties' token, which must be on the first column of the table.");

                while (excel.Column <= propertiesEndColumn)
                {
                    SetPropertyOnTableRowVariable(
                        headers,
                        tableRow,
                        excel.Column - propertiesStartColumn.Value);
                    excel.MoveRight();
                }
            }
        }

        void SetPropertyOnTableRowVariable(
            Dictionary<uint, TableHeader> headers,
            uint tableRow,
            uint tableColumn)
        {
            // need to add the row and column of the table here, or just not have them
            log.VisitGivenTablePropertyCellDeclaration(headers[excel.Column], tableRow, tableColumn);

            if (headers[excel.Column] is SubClassTableHeader)
            {
                var subClassHeader = headers[excel.Column] as SubClassTableHeader;

                log.VisitGivenComplexPropertyDeclaration(
                    converter.GivenPropertyNameExcelNameToSutName(subClassHeader.ExcelPropertyName),
                    subClassHeader.SubClassName);

                using (code.OutputAndOpenAutoClosingBracket($".{converter.GivenPropertyNameExcelNameToCodeName(subClassHeader.ExcelPropertyName)}"))
                {

                    SetAllPropertiesOnTableRowVariable(
                        subClassHeader.FullSubClassName,
                        subClassHeader.PropertiesStartColumn,
                        subClassHeader.PropertiesEndColumn,
                        subClassHeader.Headers,
                        tableRow);
                }

                log.VisitGivenComplexPropertyFinalisation();

                excel.MoveLeft();
            }
            else if (headers[excel.Column] is PropertyTableHeader)
            {
                var propertyHeader = headers[excel.Column] as PropertyTableHeader;

                code.Add($".{converter.GivenPropertyNameExcelNameToCodeName(propertyHeader.ExcelPropertyName)}({converter.PropertyValueExcelToCode(propertyHeader.ExcelPropertyName, excel.CurrentCellRaw())})");

                VisitGivenSimplePropertyOrFunction(
                    propertyHeader.ExcelPropertyName,
                    excel.CurrentCellRaw()
                    );
            }
            else
            {
                throw new ExcelToCodeException("Unknown type of Table Header");
            }

            log.VisitGivenTablePropertyCellFinalisation();
        }
    }
}
