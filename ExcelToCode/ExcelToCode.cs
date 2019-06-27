using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.ExcelToCode
{
    // this class is very much too big, split in to smaller ones
    // easy targets would be given, when, assert.
    // the various property types could probably also be split off quite easily as well
    // a better way of doing this would probably be to form a representation of the test in code (like the assertions property) and then write this out to a string in a different class. This involves some framework overhead, but will definitely be worthwhile if this gets more complex.
    // might be good to make it obvious which operations relate to excel and which to the code generation. eg "excel.MoveDown" and "cSharp.DeclareVariable"
    public class ExcelToCode : ExcelToCodeBase
    {
        readonly List<string> issuesPreventingRoundTrip;
        public IReadOnlyList<string> IssuesPreventingRoundTrip => issuesPreventingRoundTrip;

        readonly List<string> warnings;
        public IReadOnlyList<string> Warnings => warnings;

        readonly List<string> errors;
        public IReadOnlyList<string> Errors => errors;

        public ExcelToCode(ICodeNameToExcelNameConverter converter) : base(converter)
        {
            issuesPreventingRoundTrip = new List<string>();
            warnings = new List<string>();
            errors = new List<string>();
        }

        public string GenerateCSharpTestCode(
            IEnumerable<string> usings,
            ITabularPage worksheet,
            string projectRootNamespace,
            string workBookName)
        {
            try
            {
                return TryGenerateCSharpTestCode(
                    usings,
                    worksheet,
                    projectRootNamespace,
                    workBookName);
            }
            catch (ExcelToCodeException exception)
            {
                errors.Add(exception.Message);
                return $"// {exception.Message}";
            }
            catch (Exception exception)
            {
                string message = $"Unable to convert worksheet due to an unexpected internal error:\n{exception.ToString()}";
                errors.Add(message);
                return $"// {message}";
            }
        }

        string TryGenerateCSharpTestCode(
            IEnumerable<string> usings,
            ITabularPage worksheet,
            string projectRootNamespace,
            string workBookName)
        {
            base.worksheet = worksheet;
            code = new AutoIndentingStringBuilder("    ");
            column = 1;
            row = 1;
            issuesPreventingRoundTrip.Clear();

            var description = DoSpecification();
            DoGiven(usings, description, projectRootNamespace, workBookName);
            DoWhen();
            DoAssert();

            EndSpecification();

            return code.ToString();
        }

        void EndSpecification()
        {
            Output("};");
            Output("}");
            // This is so that when writing back out to excel, the prefix can be removed. So the prefix exists in the code, but not in excel, and is round trippable
            OutputBlankLine();
            Output($"protected override string AssertionClassPrefixAddedByGenerator => \"{converter.AssertionClassPrefixAddedByGenerator}\";");
            OutputErrors();
            OutputWarnings();
            OutputRoundTripIssues();
            Output("}");
            Output("}");
        }

        void OutputErrors()
        {
            if (Errors.Any())
            {
                OutputBlankLine();
                errors.ForEach(error => Output($"// {error}"));
            }
        }

        void OutputWarnings()
        {
            if (Warnings.Any())
            {
                OutputBlankLine();
                warnings.ForEach(warning => Output($"// {warning}"));
            }
        }

        void OutputRoundTripIssues()
        {
            if (IssuesPreventingRoundTrip.Any())
            {
                OutputBlankLine();
                Output($"protected override bool RoundTrippable() => false;");
                OutputBlankLine();
                Output("protected override IEnumerable<string> IssuesPreventingRoundTrip() => new List<string> {");
                Output(
                    string.Join(
                        "," + Environment.NewLine,
                        IssuesPreventingRoundTrip.Select(issue => "\"" + issue + "\"")
                    )
                );
                Output("};");
            }
        }

        string DoSpecification()
        {
            ExcelMoveDownToToken(converter.Specification);

            ExcelMoveRight();
            var description = CurrentCell();
            ExcelMoveLeft();

            ExcelMoveDown();

            return description;
        }

        void StartOutput(IEnumerable<string> usings, string description, string projectRootNamespace, string workBookName)
        {
            Output("using System;");
            Output("using System.Collections.Generic;");
            Output("using System.Linq;");
            Output("using System.Text;");
            Output("using NUnit.Framework;");
            Output("using CustomerTestsExcel;");
            Output("using System.Linq.Expressions;");
            Output($"using {projectRootNamespace};");
            OutputBlankLine();
            foreach (var usingNamespace in usings)
                Output($"using {usingNamespace};");
            OutputBlankLine();
            Output($"namespace {projectRootNamespace}.{converter.ExcelFileNameToCodeNamespacePart(workBookName)}");
            Output("{");
            Output("[TestFixture]");
            Output($"public class {converter.ExcelSpecificationNameToCodeSpecificationClassName(worksheet.Name)} : SpecificationBase<{CSharpSUTSpecificationSpecificClassName()}>, ISpecification<{CSharpSUTSpecificationSpecificClassName()}>");
            Output("{");
            Output("public override string Description()");
            Output("{");
            Output($"return \"{ description}\";");
            Output("}");
            OutputBlankLine();
            Output("// arrange");
            Output($"public override {CSharpSUTSpecificationSpecificClassName()} Given()");
            Output("{");
        }

        void DoGiven(IEnumerable<string> usings, string description, string projectRootNamespace, string workBookName)
        {
            ExcelMoveDownToToken(converter.Given);

            using (AutoRestoreExcelMoveRight())
            {
                _sutName = CurrentCell();

                StartOutput(usings, description, projectRootNamespace, workBookName);

                CreateObject(CSharpSUTVariableName(), CSharpSUTSpecificationSpecificClassName());

                EndGiven();
            }

            CheckExactlyOneBlankLineBetweenGivenAndWhen();
        }

        void CheckExactlyOneBlankLineBetweenGivenAndWhen()
        {
            uint endOfGiven = row;
            uint startOfWhen;

            using (SavePosition())
            {
                ExcelMoveDownToToken(converter.When);
                startOfWhen = row;
            }

            if (startOfWhen - endOfGiven <= 1)
                issuesPreventingRoundTrip.Add($"There is no blank line between the end of the Given section (Row {endOfGiven}) and the start of the When section (Row {startOfWhen}) in the Excel test, worksheet '{worksheet.Name}'");
            else if (startOfWhen - endOfGiven > 2)
                issuesPreventingRoundTrip.Add($"There should be exactly one blank line, but there are {startOfWhen - endOfGiven - 1}, between the end of the Given section (Row {endOfGiven}) and the start of the When section (Row {startOfWhen}) in the Excel test, worksheet '{worksheet.Name}'");
        }

        void CreateObject(string cSharpVariableName, string cSharpClassName)
        {
            ExcelMoveDown(); // this is a bit mysterious

            DeclareVariable(cSharpVariableName, cSharpClassName);

            SetVariableProperties(cSharpVariableName);

            ExcelMoveUp(); // this is a bit mysterious
        }

        void DeclareVariable(string cSharpVariableName, string cSharpClassName) =>
            Output($"var {cSharpVariableName} = new {cSharpClassName}();");

        void DeclareListVariable(string cSharpVariableName, string cSharpClassName) =>
            Output($"var {cSharpVariableName} = new List<{cSharpClassName}>();");

        void SetVariableProperties(string cSharpVariableName)
        {
            if (CurrentCell() == converter.WithProperties)
            {
                using (AutoRestoreExcelMoveRight())
                {
                    ExcelMoveDown();
                    while (!string.IsNullOrEmpty(CurrentCell()))
                    {
                        DoProperty(cSharpVariableName);
                        ExcelMoveDown();
                    }
                }
            }
        }

        void DoProperty(string cSharpVariableName)
        {
            // "Calibrations(0) of", "InstrumentCalibration"
            // methodname = "Calibrations_of"
            // index = 0;
            // variable name = instrumentCalibration
            // we actually don't need the index any more now that each item in the list has its own scope, so we could remove it from the excel definition

            CheckMissingTableOf();
            var startCellReference = CellReferenceA1Style();

            var excelGivenLeft = CurrentCell();
            using (AutoRestoreExcelMoveRight())
            {
                var excelGivenRight = CurrentCellRaw();
                var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

                if (IsTable(excelGivenLeft))
                {
                    var cSharpMethodName = converter.GivenTablePropertyNameExcelNameToCodeName(excelGivenLeft);

                    using (Scope())
                    {
                        string cSharpClassName = converter.ExcelClassNameToCodeName(excelGivenRightString);
                        string cSharpChildVariableName = converter.GivenTablePropertyNameExcelNameToCodeVariableName(excelGivenLeft);

                        CreateObjectsFromTable(startCellReference, cSharpChildVariableName, cSharpChildVariableName + "_Row", cSharpClassName);
                        Output(cSharpVariableName + "." + cSharpMethodName + "(" + cSharpChildVariableName + ")" + ";");
                    }
                }
                if (IsList(excelGivenLeft))
                {
                    var cSharpMethodName = converter.GivenListPropertyNameExcelNameToCodeName(excelGivenLeft);
                    var cSharpClassName = converter.ExcelClassNameToCodeName(excelGivenRightString);
                    string cSharpListVariableName = ListVariableNameFromMethodName(excelGivenLeft);
                    string cSharpListItemVariableName = ListItemVariableNameFromMethodName(excelGivenLeft);

                    OutputBlankLine();
                    using (Scope())
                    {
                        using (AutoRestoreExcelMoveDown())
                        {
                            // Declare a list variable to hold the items
                            DeclareListVariable(cSharpListVariableName, cSharpClassName);

                            while (CurrentCell() == "With Item")
                            {
                                ExcelMoveDown();

                                using (Scope())
                                {
                                    // Add an item to the list
                                    using (AutoRestoreExcelMoveRight())
                                    {
                                        DeclareVariable(cSharpListItemVariableName, cSharpClassName);

                                        while (!string.IsNullOrEmpty(CurrentCell()))
                                        {
                                            DoProperty(cSharpListVariableName);
                                            ExcelMoveDown();
                                        }

                                        OutputListAdd(cSharpListVariableName, cSharpListItemVariableName);
                                    }
                                }
                            }

                            // Add the list of the parent object
                            Output($"{cSharpVariableName}.{cSharpMethodName}({cSharpListVariableName});");
                        }
                    }
                }
                else if (HasGivenSubProperties())
                {
                    var cSharpMethodName = converter.GivenPropertyNameExcelNameToCodeName(excelGivenLeft);

                    OutputBlankLine();
                    using (Scope())
                    {
                        string cSharpClassName = converter.ExcelClassNameToCodeName(excelGivenRightString);
                        string cSharpChildVariableName = VariableCase(excelGivenRightString.Replace(".", ""));

                        CreateObject(cSharpChildVariableName, cSharpClassName);
                        Output(cSharpVariableName + "." + cSharpMethodName + "(" + cSharpChildVariableName + ")" + ";");
                    }
                }
                else
                {
                    var cSharpMethodName = converter.GivenPropertyNameExcelNameToCodeName(excelGivenLeft);

                    Output($"{cSharpVariableName}.{cSharpMethodName}({converter.PropertyValueExcelToCode(excelGivenLeft, excelGivenRight)});");
                }
            }
        }

        void OutputListAdd(string cSharpListVariableName, string cSharpListItemVariableName) =>
            Output($"{cSharpListVariableName}.Add({cSharpListItemVariableName});");

        bool IsList(string excelGivenLeft) =>
            excelGivenLeft.EndsWith(converter.ListOf, StringComparison.InvariantCultureIgnoreCase);

        string ListVariableNameFromMethodName(string excelGivenLeft) =>
            VariableCase(converter.GivenListPropertyNameExcelNameToCodeVariableName(excelGivenLeft)) + "List";

        string ListItemVariableNameFromMethodName(string excelGivenLeft) =>
            VariableCase(converter.GivenListPropertyNameExcelNameToCodeVariableName(excelGivenLeft));

        bool IsTable(string excelGivenLeft) =>
            excelGivenLeft.EndsWith(converter.TableOf, StringComparison.InvariantCultureIgnoreCase);

        // check to see if it looks like a table, but does not end with converter.TableOf
        void CheckMissingTableOf()
        {
            if (LooksLikeATableButIsnt())
            {
                var message = $"It looks like you might be trying to set up a table, starting at cell {CellReferenceA1Style()}. If this is the case, please make sure that cell {CellReferenceA1Style()} ends with '{converter.TableOf}'";

                // this will appear at the relevant point in the generated code
                Output($"// {message}");

                // this can be used elsewhere, such as in the console output of the test generation
                errors.Add(message);
            }
        }

        bool LooksLikeATableButIsnt() =>
            (
            IsTable(CurrentCell()) == false
            && PeekBelowRight() == converter.WithProperties
            && (PeekBelowRight(2, 1) != "" && PeekBelow(2) == "")
            );

        string TableVariableNameFromMethodName(string excelGivenLeft) =>
            VariableCase(converter.GivenTablePropertyNameExcelNameToCodeVariableName(excelGivenLeft));

        void CreateObjectsFromTable(
            string tableStartCellReference,
            string cSharpVariableName,
            string cSharpClassName,
            string cSharpSpecificationSpecificClassName)
        {
            CheckMissingWithPropertiesForTable(tableStartCellReference);

            CheckBadIndentationInTable(tableStartCellReference);

            var headers = ReadHeaders();

            CheckMissingHeadersForTable(tableStartCellReference, headers);

            CheckTableIsRoundTrippable(headers.Values);

            uint lastColumn = headers.Max(h => h.Value.EndColumn);
            uint propertiesEndColumn = lastColumn;

            Output($"var {cSharpVariableName} = new ReportSpecificationSetupClassUsingTable<{cSharpSpecificationSpecificClassName}>();");

            int tableRow = 0;
            uint moveDown = 1 + (headers.Max((KeyValuePair<uint, TableHeader> h) => h.Value.EndRow) - row);
            ExcelMoveDown(moveDown);
            while (TableHasMoreRows(lastColumn))
            {
                using (SavePosition())
                {
                    string indexedCSharpVariableName = VariableCase(cSharpClassName);

                    using (Scope())
                    {
                        SetAllPropertiesOnTableRowVariable(
                            indexedCSharpVariableName,
                            cSharpSpecificationSpecificClassName,
                            column,
                            propertiesEndColumn,
                            headers);

                        Output($"{cSharpVariableName}.Add({indexedCSharpVariableName});");
                    }
                    tableRow++;
                }

                ExcelMoveDown();
            }

            CheckNoRowsInTable(tableStartCellReference, CellReferenceA1Style(), tableRow);

            ExcelMoveUp();
        }

        void CheckMissingHeadersForTable(string tableStartCellReference, Dictionary<uint, TableHeader> headers)
        {
            if (!headers.Any())
                throw new ExcelToCodeException($"The table starting at cell {tableStartCellReference} has no headers. There should be a row of Property Names starting at {CellReferenceA1Style()}, with rows of Property Values below.");
        }

        void CheckMissingWithPropertiesForTable(string tableStartCellReference)
        {
            using (AutoRestoreExcelMoveDown(1))
            {
                if (CurrentCell() != converter.WithProperties)
                    throw new ExcelToCodeException($"The table starting at {tableStartCellReference} is not formatted correctly. Cell {CellReferenceA1Style()} should be '{converter.WithProperties}', but is '{CurrentCell()}'");
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
                    issuesPreventingRoundTrip.Add($"There is a complex property ('{h.PropertyName}', cell {CellReferenceA1Style()}) within a table in the Excel test, worksheet '{worksheet.Name}'")
                );
        }

        void CheckNoRowsInTable(string tableStartCellReference, string rowsStartCellReference, int numberOfRows)
        {
            if (numberOfRows == 0)
                throw new ExcelToCodeException($"The table starting at cell {tableStartCellReference} has no rows. There should be at least one row of Property Values starting at {rowsStartCellReference}.");
        }

        Dictionary<uint, TableHeader> ReadHeaders()
        {
            ExcelMoveDown();

            ExcelMoveDown();

            var headers = new Dictionary<uint, TableHeader>();

            using (SavePosition())
            {
                while (CurrentCell() != "")
                {
                    headers.Add(column, CreatePropertyHeader());
                    ExcelMoveRight();
                }
            }

            return headers;
        }

        void CheckBadIndentationInTable(string startCellReference)
        {
            using (AutoRestoreExcelMoveDown(2))
            {
                if (CurrentCell() == "" && PeekRight() != "")
                    throw new ExcelToCodeException($"The table starting at {startCellReference} is not formatted correctly. The properties start on column {ColumnReferenceA1Style(column + 1)}, but they should start start one to the left, on column {ColumnReferenceA1Style()}");
            }
        }

        TableHeader CreatePropertyHeader()
        {
            if (PeekBelow(2) == converter.WithProperties)
                return CreateSubClassHeader();

            return new PropertyTableHeader(CurrentCell(), row, column);
        }

        SubClassTableHeader CreateSubClassHeader()
        {
            string propertyName;
            string subClassName;
            uint startRow;
            uint endRow;
            uint? propertiesStartColumn;
            uint propertiesEndColumn;
            var headers = new Dictionary<uint, TableHeader>();

            // this is a almost a straight copy of the original read proeprty headers code so we will be able to reuse it (the detection of the end of the properties is different, and the positioning is different, other than that its identical I think)
            startRow = row;
            using (SavePosition())
            {
                propertyName = converter.GivenPropertyNameExcelNameToCodeName(CurrentCell());
                ExcelMoveDown();
                subClassName = CurrentCell();
                ExcelMoveDown();
                propertiesStartColumn = column;

                ExcelMoveDown();
                do
                {
                    headers.Add(column, CreatePropertyHeader());
                    ExcelMoveRight();
                } while (PeekAbove(3) == "" && CurrentCell() != "");// Need to detect end of the sub property. This is by the existence of a property name in the parent proeprty header row, which is 3 rows up, or by when there are no columns left in the table

                propertiesEndColumn = column - 1;
                endRow = row;
            }

            ExcelMoveRight((uint)headers.Count - 1);

            return new SubClassTableHeader(propertyName, subClassName, converter.ExcelClassNameToCodeName(subClassName), startRow, endRow, propertiesStartColumn, propertiesEndColumn, headers);
        }

        void SetAllPropertiesOnTableRowVariable(string cSharpVariableName, string cSharpSpecificationSpecificClassName, uint? propertiesStartColumn, uint propertiesEndColumn, Dictionary<uint, TableHeader> propertyNames)
        {
            DeclareTableRowVariable(cSharpVariableName, cSharpSpecificationSpecificClassName);

            SetPropertiesOnTableRowVariable(propertiesStartColumn, propertyNames, propertiesEndColumn, cSharpVariableName);
        }

        void DeclareTableRowVariable(string cSharpVariableName, string cSharpSpecificationSpecificClassName) =>
            Output($"var {cSharpVariableName} = new {cSharpSpecificationSpecificClassName}();");

        // This should work when there are sub classes in the table
        bool TableHasMoreRows(uint lastColumn)
        {
            if (RowToColumnIsEmpty(lastColumn)) return false;
            if (AnyPrecedingColumnHasAValue()) return false;

            return true;
        }

        void SetPropertiesOnTableRowVariable(uint? propertiesStartColumn, Dictionary<uint, TableHeader> headers, uint propertiesEndColumn, string cSharpVariableName)
        {
            if (propertiesStartColumn.HasValue)
            {
                if (column != propertiesStartColumn.Value) throw new ExcelToCodeException("Table must have a 'With Properties' token, which must be on the first column of the table.");

                while (column <= propertiesEndColumn)
                {
                    SetPropertyOnTableRowVariable(headers, cSharpVariableName);
                    ExcelMoveRight();
                }
            }
        }

        void SetPropertyOnTableRowVariable(Dictionary<uint, TableHeader> headers, string cSharpVariableName)
        {
            if (headers[column] is SubClassTableHeader)
            {
                var subClassHeader = headers[column] as SubClassTableHeader;
                string subClassCSharpVariableName = $"{cSharpVariableName}_{subClassHeader.SubClassName.Replace(".", "")}"; // this <.Replace(".", "")> is shared with DoProperty, we should move in into the _converter

                SetAllPropertiesOnTableRowVariable(subClassCSharpVariableName, subClassHeader.FullSubClassName, subClassHeader.PropertiesStartColumn, subClassHeader.PropertiesEndColumn, subClassHeader.Headers);

                ExcelMoveLeft();

                Output($"{cSharpVariableName}.{converter.GivenPropertyNameExcelNameToCodeName(subClassHeader.PropertyName)}({subClassCSharpVariableName});");
            }
            else if (headers[column] is PropertyTableHeader)
            {
                var propertyHeader = headers[column] as PropertyTableHeader;

                Output($"{cSharpVariableName}.{converter.GivenPropertyNameExcelNameToCodeName(propertyHeader.PropertyName)}({converter.PropertyValueExcelToCode(propertyHeader.PropertyName, CurrentCellRaw())});");
            }
            else
            {
                throw new ExcelToCodeException("Unknown type of Table Header");
            }
        }

        void EndGiven()
        {
            OutputBlankLine();
            Output("return " + CSharpSUTVariableName() + ";");
            Output("}");
        }

        bool HasGivenSubProperties()
        {
            return (PeekBelow() == converter.WithProperties);
        }

        protected void DoWhen()
        {
            ExcelMoveDownToToken(converter.When);

            using (AutoRestoreExcelMoveRight())
            {
                OutputBlankLine();
                Output("public override string When(" + CSharpSUTSpecificationSpecificClassName() + " " + CSharpSUTVariableName() + ")");

                using (Scope())
                {
                    Output(CSharpSUTVariableName() + "." + converter.ActionExcelNameToCodeName(CurrentCell()) + "();");
                    Output("return \"" + CurrentCell() + "\";");
                }

                OutputBlankLine();
            }

            ExcelMoveDown();
        }

        void DoAssert()
        {
            ExcelMoveDownToToken(converter.Assert);

            Output("public override IEnumerable<IAssertion<" + CSharpSUTSpecificationSpecificClassName() + ">> Assertions()");
            Output("{");
            Output("return new List<IAssertion<" + CSharpSUTSpecificationSpecificClassName() + ">>");
            Output("{");

            ExcelMoveRight();

            DoAssertions(CSharpSUTSpecificationSpecificClassName(), VariableCase(CSharpSUTVariableName()));
        }

        void DoAssertions(string cSharpClassName, string cSharpVariableName)
        {
            int assertIndex = 0;

            ExcelMoveDown();

            while (row <= GetLastRow() && !RowToCurrentColumnIsEmpty() && !AnyPrecedingColumnHasAValue())
            {
                DoAssertion(assertIndex, cSharpClassName, cSharpVariableName);

                assertIndex++;

                ExcelMoveDown();
            }
        }

        // eg 
        // IllustrativeFoundationCost|=|2479680|PercentagePrecision|0.001 
        // CFDCalculator|CFDCalulator (this starts off a class which contains sub properties to be asserted)
        //              |TotalCost|=|0 (this is a property of CFDCalculator to assert)  
        // TurbineLocationAndDimensions|table of|TurbineLocationAndDimension (this starts off a list property which contains rows to be asserted) 
        //           |easterly_m              |northerly_m (these column headers can define assertion properties, but vertically instead of horizontally
        //           |=                       |=
        //           |WithPercentagePrecision |WithPercentagePrecision
        //           |0.001                   |0.001
        //           |2                       |3 (these rows assert the properties for an item in the list)
        void DoAssertion(int assertIndex, string cSharpClassName, string cSharpVariableName)
        {
            string excelPropertyName = CurrentCell();
            string simpleCSharpPropertyName = converter.AssertPropertyExcelNameToCodeName(excelPropertyName);
            ExcelMoveRight();
            string assertionOperatorOrExcelSubClassNameOrTableOf = CurrentCell();
            ExcelMoveRight();
            string excelPropertyValue = CurrentCell();
            string cSharpPropertyValue = converter.AssertValueExcelNameToCodeName(excelPropertyValue, CurrentCellRaw());
            ExcelMoveRight();
            string assertionSpecificKey = CurrentCell();
            ExcelMoveRight();
            string assertionSpecificValue = CurrentCell();
            ExcelMoveRight();
            string roundTripValue = CurrentCell();
            ExcelMoveLeft(5);

            CheckMissingTableOfForAssertion();

            if (string.IsNullOrWhiteSpace(excelPropertyValue))
            {
                DoSubAssertion(assertIndex, simpleCSharpPropertyName, assertionOperatorOrExcelSubClassNameOrTableOf, cSharpClassName);
            }
            else if (assertionOperatorOrExcelSubClassNameOrTableOf == converter.TableOf)
            {
                DoTableAssertion(assertIndex, simpleCSharpPropertyName, excelPropertyValue, cSharpClassName);
            }
            else if (assertionOperatorOrExcelSubClassNameOrTableOf == "=" && assertionSpecificKey.ToLowerInvariant() == "percentageprecision")
            {
                DoEqualityWithPercentagePrecisionAssertion(assertIndex, simpleCSharpPropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName, assertionSpecificValue);
            }
            else if (assertionOperatorOrExcelSubClassNameOrTableOf == "=" && assertionSpecificKey.ToLowerInvariant() == "stringformat")
            {
                DoEqualityWithStringFormatAssertion(assertIndex, simpleCSharpPropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName, assertionSpecificValue);
            }
            else if (assertionOperatorOrExcelSubClassNameOrTableOf == "=")
            {
                DoEqualityAssertion(assertIndex, simpleCSharpPropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName);
            }
            else
            {
                DoInvalidAssertion(assertionOperatorOrExcelSubClassNameOrTableOf);
            }

            if (!string.IsNullOrWhiteSpace(roundTripValue))
            {
                assertIndex++;
                DoExcelFormulaDoesNotMatchCodeAssertion(assertIndex, simpleCSharpPropertyName, cSharpPropertyValue, roundTripValue, cSharpClassName);
            }
        }

        void DoInvalidAssertion(string assertionOperatorOrExcelSubClassNameOrTableOf)
        {
            AddError($"Invalid assertion operator ('{assertionOperatorOrExcelSubClassNameOrTableOf}') found at cell {CellReferenceA1Style()}. Valid operators are '=' and '{converter.TableOf}'");
        }

        void AddError(string message)
        {
            // this will appear at the relevant point in the generated code
            Output($"// {message}");

            // this can be used elsewhere, such as in the console output of the test generation
            errors.Add(message);
        }

        // check to see if it looks like a table, but does not end with converter.TableOf
        void CheckMissingTableOfForAssertion()
        {
            if (LooksLikeAnAssertionTableButIsnt())
                AddError($"It looks like you might be trying to set up a table assertion, starting at cell {CellReferenceA1Style()}. If this is the case, please make sure that cell {CellReferenceA1Style(row, column + 1)} is '{converter.TableOf}', and that the table itself (the bit with the class name, 'With Properties' etc) starts on column {ColumnReferenceA1Style(column + 2)}");
        }

        bool LooksLikeAnAssertionTableButIsnt() =>
            (
            PeekRight() != converter.TableOf
            &&
                (
                PeekBelowRight() == converter.WithProperties && PeekBelow() == ""
                || PeekBelowRight(1, 2) == converter.WithProperties && PeekBelowRight() == ""
                )
            );

        // wants to look something like this
        /*
            , new TableAssertion<BlockageLossFactorInputs, TurbineLocationAndDimension>(
            inputs => inputs.turbineLocationAndDimensions,
            new List<List<IAssertion<TurbineLocationAndDimension>>>
            {
                new List<IAssertion<TurbineLocationAndDimension>>
                {
                new EqualityAssertionWithPercentagePrecision<TurbineLocationAndDimension>(turbineLocationAndDimensions => turbineLocationAndDimensions.easterly_m, 2, 0.001),
                new EqualityAssertionWithPercentagePrecision<TurbineLocationAndDimension>(turbineLocationAndDimensions => turbineLocationAndDimensions.northerly_m, 3, 0.001),
                new EqualityAssertionWithPercentagePrecision<TurbineLocationAndDimension>(turbineLocationAndDimensions => turbineLocationAndDimensions.hubHeight_m, 4, 0.001),
                new EqualityAssertionWithPercentagePrecision<TurbineLocationAndDimension>(turbineLocationAndDimensions => turbineLocationAndDimensions.rotorDiameter_m, 5, 0.001)
                },
                new List<IAssertion<TurbineLocationAndDimension>>
                {
                new EqualityAssertionWithPercentagePrecision<TurbineLocationAndDimension>(turbineLocationAndDimensions => turbineLocationAndDimensions.easterly_m, 7, 0.001),
                new EqualityAssertionWithPercentagePrecision<TurbineLocationAndDimension>(turbineLocationAndDimensions => turbineLocationAndDimensions.northerly_m, 8, 0.001),
                new EqualityAssertionWithPercentagePrecision<TurbineLocationAndDimension>(turbineLocationAndDimensions => turbineLocationAndDimensions.hubHeight_m, 9, 0.001),
                new EqualityAssertionWithPercentagePrecision<TurbineLocationAndDimension>(turbineLocationAndDimensions => turbineLocationAndDimensions.rotorDiameter_m, 10, 0.001)
                },
            }
            )
         */
        void DoTableAssertion(int assertIndex, string excelPropertyName, string excelSubClassName, string cSharpClassName)
        {
            string cSharpSubClassName = converter.AssertionSubClassExcelNameToCodeName(excelSubClassName);
            string cSharpSubMethodName = converter.AssertionSubPropertyExcelNameToCodeMethodName(excelPropertyName);
            string cSharpVariableName = VariableCase(UnIndex(excelPropertyName));

            var startCellReference = CellReferenceA1Style();

            CheckMissingWithPropertiesInAssertionTable(startCellReference);
            CheckBadIndentationInAssertionTable(startCellReference);

            // first row is property name, "table of" and property type
            // then With Properties
            // then the headers 
            ExcelMoveDown();
            ExcelMoveDown();
            var assertionTableHeaders = ReadAssertionTableHeaders();

            CheckMissingHeadersInAssertionTable(startCellReference, assertionTableHeaders);

            // there can be 2 or 4 rows in the header
            ExcelMoveDown(assertionTableHeaders.Max(a => a.Rows()));

            Output(LeadingComma(assertIndex) + $"new TableAssertion<{cSharpClassName}, {cSharpSubClassName}>");
            using (AutoCloseBracketAndIndent())
            {
                using (AutoRestoreExcelMoveRight(2))
                {
                    Output($"{cSharpVariableName} => {cSharpVariableName}.{cSharpSubMethodName},");
                    Output($"new List<List<IAssertion<{cSharpSubClassName}>>>");
                    using (AutoCloseCurlyBracket())
                    {
                        int tableRowIndex = 0;
                        while (row <= GetLastRow() && !RowToCurrentColumnIsEmpty() && !AnyPrecedingColumnHasAValue()) // should encapsulate this conditional
                        {
                            Output($"{LeadingComma(tableRowIndex)}new List<IAssertion<{cSharpSubClassName}>>");
                            tableRowIndex++;
                            using (AutoCloseCurlyBracket())
                            {
                                using (code.AutoCloseIndent())
                                {
                                    using (SavePosition())
                                    {
                                        int tableColumnIndex = 0;
                                        foreach (var assertionTableHeader in assertionTableHeaders)
                                        {
                                            DoTableCellAssertion(tableColumnIndex, cSharpSubClassName, cSharpVariableName, assertionTableHeader);
                                            ExcelMoveRight();
                                            tableColumnIndex++;
                                        }
                                    }
                                }
                            }

                            ExcelMoveDown();
                        }

                        CheckNoRowsInAssertionTable(startCellReference, CellReferenceA1Style(), tableRowIndex);
                    }
                }
            }

            // we should leave the row at the last row of the TABLE assertions. The calling DoAssertions (it is recursive) calls MoveDown to move on to the next assertion row after this method.
            // This seems like a bad pattern though, we should probably fix it up. Assertions should be responsible for moving the cursor on themselves.
            ExcelMoveUp();
        }

        void CheckBadIndentationInAssertionTable(string tableStartCellReference)
        {
            using (AutoRestoreExcelMoveDownRight(2, 2))
            {
                if (CurrentCell() == "" && PeekRight() != "")
                    throw new ExcelToCodeException($"The assertion table starting at {tableStartCellReference} is not formatted correctly. The properties start on column {ColumnReferenceA1Style(column + 1)}, but they should start one to the left, on column {ColumnReferenceA1Style()}");
            }
        }

        void CheckMissingWithPropertiesInAssertionTable(string tableStartCellReference)
        {
            using (AutoRestoreExcelMoveDownRight(1, 2))
            {
                if (CurrentCell() != converter.WithProperties)
                    throw new ExcelToCodeException($"The assertion table starting at {tableStartCellReference} is not formatted correctly. Cell {CellReferenceA1Style()} should be '{converter.WithProperties}', but is '{CurrentCell()}'");
            }
        }

        void CheckMissingHeadersInAssertionTable(string tableStartCellReference, IEnumerable<AssertionTableHeader> headers)
        {
            if (!headers.Any())
                throw new ExcelToCodeException($"The assertion table starting at cell {tableStartCellReference} has no headers. There should be a row of Property Names starting at {CellReferenceA1Style()}, with rows of Property Values below.");
        }

        void CheckNoRowsInAssertionTable(string tableStartCellReference, string rowsStartCellReference, int numberOfRows)
        {
            if (numberOfRows == 0)
                throw new ExcelToCodeException($"The assertion table starting at cell {tableStartCellReference} has no rows. There should be at least one row of Property Values starting at {rowsStartCellReference}.");
        }

        IEnumerable<AssertionTableHeader> ReadAssertionTableHeaders()
        {
            var assertionTableHeaders = new List<AssertionTableHeader>();

            using (SavePosition())
            {
                ExcelMoveRight(2);

                while (string.IsNullOrWhiteSpace(CurrentCell()) == false)
                {
                    using (SavePosition())
                    {
                        var propertyName = CurrentCell();

                        ExcelMoveDown();
                        var assertionOperator = CurrentCell();

                        ExcelMoveDown();
                        string assertionSpecificKey = "";
                        string assertionSpecificValue = "";
                        if (CurrentCell().ToLowerInvariant() == "percentageprecision" || CurrentCell().ToLowerInvariant() == "stringformat")
                        {
                            assertionSpecificKey = CurrentCell();
                            ExcelMoveDown();
                            assertionSpecificValue = CurrentCell();
                        }

                        assertionTableHeaders.Add(
                            new AssertionTableHeader(
                                propertyName,
                                assertionOperator,
                                assertionSpecificKey,
                                assertionSpecificValue));
                    }

                    ExcelMoveRight();
                }
            }

            return assertionTableHeaders;
        }

        void DoTableCellAssertion(int assertIndex, string cSharpClassName, string cSharpVariableName, AssertionTableHeader assertionTableHeader)
        {
            string excelPropertyValue = CurrentCell();
            string cSharpPropertyValue = converter.AssertValueExcelNameToCodeName(excelPropertyValue, CurrentCellRaw());

            if (assertionTableHeader.AssertionOperator == "=" && assertionTableHeader.AssertionSpecificKey.ToLowerInvariant() == "percentageprecision")
            {
                DoEqualityWithPercentagePrecisionAssertion(assertIndex, assertionTableHeader.PropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName, assertionTableHeader.AssertionSpecificValue);
            }
            else if (assertionTableHeader.AssertionOperator == "=" && assertionTableHeader.AssertionSpecificKey.ToLowerInvariant() == "stringformat")
            {
                DoEqualityWithStringFormatAssertion(assertIndex, assertionTableHeader.PropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName, assertionTableHeader.AssertionSpecificValue);
            }
            else if (assertionTableHeader.AssertionOperator == "=")
            {
                DoEqualityAssertion(assertIndex, assertionTableHeader.PropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName);
            }
            else
            {
                throw new Exception("Invalid assertion operator found in excel file: " + assertionTableHeader.AssertionOperator);
            }

            // When the excel is created from the code, it won't overwrite any calculations, unless there is important stuff in them.
            // In this case it adds some text to reserved cell a3 to indicate this, and writes out the value it would have written
            // in the column next to the assertion (assuming there is no calculation there). If these values are in an excel 
            // sheet, the code which generates the c# outputs an error, telling you to fix it up. This logic isn't done for assertion
            // tables, which at the moment isn't a problem as the tables can't convert themselves from c# to excel anyway.

        }

        void DoEqualityWithStringFormatAssertion(int assertIndex, string cSharpPropertyName, string cSharpPropertyValue, string cSharpClassName, string cSharpVariableName, string assertionSpecificValue)
        {
            Output(string.Format("{0} new EqualityAssertionWithStringFormat<{1}>({2} => {2}.{3}, {4}, \"{5}\")", LeadingComma(assertIndex), cSharpClassName, cSharpVariableName, cSharpPropertyName, cSharpPropertyValue, assertionSpecificValue));
        }

        void DoEqualityWithPercentagePrecisionAssertion(int assertIndex, string cSharpPropertyName, string cSharpPropertyValue, string cSharpClassName, string cSharpVariableName, string assertionSpecificValue)
        {
            Output(string.Format("{0} new EqualityAssertionWithPercentagePrecision<{1}>({2} => {2}.{3}, {4}, {5})", LeadingComma(assertIndex), cSharpClassName, cSharpVariableName, cSharpPropertyName, cSharpPropertyValue, assertionSpecificValue));
        }

        void DoExcelFormulaDoesNotMatchCodeAssertion(int assertIndex, string cSharpPropertyName, string cSharpPropertyValue, string roundTripValue, string cSharpClassName)
        {
            Output(LeadingComma(assertIndex) + "new ExcelFormulaDoesNotMatchCodeAssertion<" + cSharpClassName + ">(\"" + cSharpPropertyName + "\", \"" + cSharpPropertyValue + "\", \"" + roundTripValue + "\")");
        }

        void DoEqualityAssertion(int assertIndex, string cSharpPropertyName, string cSharpPropertyValue, string cSharpClassName, string cSharpVariableName)
        {
            Output($"{LeadingComma(assertIndex)} new EqualityAssertion<{cSharpClassName}>({cSharpVariableName} => {cSharpVariableName}.{cSharpPropertyName}, {cSharpPropertyValue})");
        }

        void DoSubAssertion(int assertIndex, string excelPropertyName, string excelSubClassName, string cSharpClassName)
        {
            string cSharpSubClassName = converter.AssertionSubClassExcelNameToCodeName(excelSubClassName);
            string cSharpSubMethodName = converter.AssertionSubPropertyExcelNameToCodeMethodName(excelPropertyName);
            string cSharpVariableName = VariableCase(UnIndex(excelPropertyName));

            Output(LeadingComma(assertIndex) + $"new ParentAssertion<{cSharpClassName}, {cSharpSubClassName}>");
            using (AutoCloseBracketAndIndent())
            {
                using (AutoRestoreExcelMoveRight())
                {
                    Output($"{cSharpVariableName} => {cSharpVariableName}.{cSharpSubMethodName},");
                    Output($"new List<IAssertion<{cSharpSubClassName}>>");

                    using (AutoCloseCurlyBracket())
                    {
                        DoAssertions(cSharpSubClassName, cSharpVariableName);

                        // we should leave the row at the last row of the sub assertions. The calling DoAssertions (it is recursive) calls MoveDown to move on to the next assertion row after this method.
                        // This seems like a bad pattern though, we should probably fix it up. Assertions should be responsible for moving the cursor on themselves.
                        ExcelMoveUp();
                    }

                }
            }
        }

    }
}
