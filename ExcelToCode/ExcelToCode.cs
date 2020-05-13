using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCode : ExcelToCodeBase
    {
        protected string sutName;

        public ExcelToCode(ICodeNameToExcelNameConverter converter)
            : base(new ExcelToCodeState(converter))
        { }

        // This function also returns some data in the Errors, Warnings and 
        // similar properties. It would be better to wrap all this in to 
        // a new type, and return that type here. Or to take an ILogger
        // and use this to communicate the errors and warnings.
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
                log.AddError(exception.Message);
                return $"// {exception.Message}";
            }
            catch (Exception exception)
            {
                string message = $"Unable to convert worksheet due to an unexpected internal error:\n{exception.ToString()}";
                log.AddError(message);
                return $"// {message}";
            }
        }

        string TryGenerateCSharpTestCode(
            IEnumerable<string> usings,
            ITabularPage worksheet,
            string projectRootNamespace,
            string workBookName)
        {
            Initialise(worksheet);

            var description = ReadDescription();
            sutName = ReadSutName();

            DoHeaders(usings, description, projectRootNamespace, workBookName);
            DoGiven(sutName);
            DoWhen(sutName);
            DoAssert();
            DoFooters();

            return code.GeneratedCode;
        }

        private void Initialise(ITabularPage worksheet)
        {
            excel.Initialise(worksheet);
            code.Initialise();
            log.Initialise();
        }

        void OutputErrors()
        {
            if (log.Errors.Any())
            {
                code.BlankLine();
                log.Errors.ToList().ForEach(error => code.Add($"// {error}"));
            }
        }

        void OutputWarnings()
        {
            if (log.Warnings.Any())
            {
                code.BlankLine();
                log.Warnings.ToList().ForEach(warning => code.Add($"// {warning}"));
                log.Warnings.ToList().ForEach(warning => code.Add($"// {warning}"));
            }
        }

        void OutputRoundTripIssues()
        {
            if (!log.IssuesPreventingRoundTrip.Any())
                return;

            code.BlankLine();
            code.Add($"protected override bool RoundTrippable() => false;");
            code.BlankLine();
            code.Add("protected override IEnumerable<string> IssuesPreventingRoundTrip() => new List<string>");
            code.Add("{");
            code.Add(
                string.Join(
                    "," + Environment.NewLine,
                    log.IssuesPreventingRoundTrip.Select(issue => "\"" + issue + "\"")
                )
            );
            code.Add("};");
        }

        string ReadDescription()
        {
            using (excel.SavePosition())
            {
                excel.MoveDownToToken(converter.Specification);

                return excel.PeekRight();
            }
        }

        string ReadSutName()
        {
            using (excel.SavePosition())
            {
                excel.MoveDownToToken(converter.Given);

                return excel.PeekRight();
            }
        }

        void DoHeaders(IEnumerable<string> usings, string description, string projectRootNamespace, string workBookName)
        {
            code.Add("using System;");
            code.Add("using System.Collections.Generic;");
            code.Add("using System.Linq;");
            code.Add("using System.Text;");
            code.Add("using NUnit.Framework;");
            code.Add("using CustomerTestsExcel;");
            code.Add("using CustomerTestsExcel.Assertions;");
            code.Add("using CustomerTestsExcel.SpecificationSpecificClassGeneration;");
            code.Add("using System.Linq.Expressions;");
            code.Add($"using {projectRootNamespace};");
            code.Add($"using {projectRootNamespace}.GeneratedSpecificationSpecific;");
            code.BlankLine();
            foreach (var usingNamespace in usings)
                code.Add($"using {usingNamespace};");
            code.BlankLine();
            code.Add($"namespace {projectRootNamespace}.{converter.ExcelFileNameToCodeNamespacePart(workBookName)}");
            code.Add("{");
            code.Add("[TestFixture]");
            code.Add($"public class {converter.ExcelSpecificationNameToCodeSpecificationClassName(excel.worksheet.Name)} : SpecificationBase<{CSharpSUTSpecificationSpecificClassName(sutName)}>, ISpecification<{CSharpSUTSpecificationSpecificClassName(sutName)}>");
            code.Add("{");
            code.Add("public override string Description()");
            code.Add("{");
            code.Add($"return \"{description}\";");
            code.Add("}");
        }

        void DoFooters()
        {
            code.Add("};");
            code.Add("}");
            // This is so that when writing back out to excel, the prefix can be removed. So the prefix exists in the code, but not in excel, and is round trippable
            code.BlankLine();
            code.Add($"protected override string AssertionClassPrefixAddedByGenerator => \"{converter.AssertionClassPrefixAddedByGenerator}\";");
            OutputErrors();
            OutputWarnings();
            OutputRoundTripIssues();
            code.Add("}");
            code.Add("}");
        }

        void DoGiven(string sutName)
        {
            excel.MoveDownToToken(converter.Given);

            using (excel.AutoRestoreMoveRight())
            {
                code.BlankLine();
                code.Add($"public override {CSharpSUTSpecificationSpecificClassName(sutName)} Given()");
                using (code.AutoCloseCurlyBracket())
                    CreateRootObject(sutName);
            }

            CheckExactlyOneBlankLineBetweenGivenAndWhen();
        }

        void CheckExactlyOneBlankLineBetweenGivenAndWhen()
        {
            uint endOfGiven = excel.row;
            uint startOfWhen;

            using (excel.SavePosition())
            {
                excel.MoveDownToToken(converter.When);
                startOfWhen = excel.row;
            }

            if (startOfWhen - endOfGiven <= 1)
                log.AddIssuePreventingRoundTrip($"There is no blank line between the end of the Given section (Row {endOfGiven}) and the start of the When section (Row {startOfWhen}) in the Excel test, worksheet '{excel.worksheet.Name}'");
            else if (startOfWhen - endOfGiven > 2)
                log.AddIssuePreventingRoundTrip($"There should be exactly one blank line, but there are {startOfWhen - endOfGiven - 1}, between the end of the Given section (Row {endOfGiven}) and the start of the When section (Row {startOfWhen}) in the Excel test, worksheet '{excel.worksheet.Name}'");
        }

        void CreateRootObject(string excelClassName)
        {
            log.VisitGivenRootClassDeclaration(excelClassName);

            code.Add("return");
            using (code.AutoCloseIndent())
            {
                excelToCodeState.ComplexProperty.CreateObjectWithoutVisiting(excelClassName);
            }
            code.Add(";");

            log.VisitGivenRootClassFinalisation();
        }

        protected void DoWhen(string sutName)
        {
            excelToCodeState.When.DoWhen(sutName);
            //excel.MoveDownToToken(converter.When);

            //using (excel.AutoRestoreMoveRight())
            //{
            //    code.BlankLine();
            //    code.Add("public override string When(" + CSharpSUTSpecificationSpecificClassName() + " " + CSharpSUTVariableName() + ")");

            //    using (code.Scope())
            //    {
            //        code.Add($"{CSharpSUTVariableName()}.{converter.ActionExcelNameToCodeName(excel.CurrentCell())}();");
            //        code.Add($"return \"{excel.CurrentCell()}\";");
            //    }

            //    code.BlankLine();
            //}

            excel.MoveDown();
        }

        void DoAssert()
        {
            excel.MoveDownToToken(converter.Assert);

            code.Add("public override IEnumerable<IAssertion<" + CSharpSUTSpecificationSpecificClassName(sutName) + ">> Assertions()");
            code.Add("{");
            code.Add("return new List<IAssertion<" + CSharpSUTSpecificationSpecificClassName(sutName) + ">>");
            code.Add("{");

            excel.MoveRight();

            DoAssertions(CSharpSUTSpecificationSpecificClassName(sutName), VariableCase(CSharpSUTVariableName(sutName)));
        }

        void DoAssertions(string cSharpClassName, string cSharpVariableName)
        {
            int assertIndex = 0;

            excel.MoveDown();

            while (excel.row <= excel.GetLastRow() && !excel.RowToCurrentColumnIsEmpty() && !excel.AnyPrecedingColumnHasAValue())
            {
                DoAssertion(assertIndex, cSharpClassName, cSharpVariableName);

                assertIndex++;

                excel.MoveDown();
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
            string excelPropertyName = excel.CurrentCell();
            string simpleCSharpPropertyName = converter.AssertPropertyExcelNameToCodeName(excelPropertyName);
            excel.MoveRight();
            string assertionOperatorOrExcelSubClassNameOrTableOf = excel.CurrentCell();
            excel.MoveRight();
            string excelPropertyValue = excel.CurrentCell();
            string cSharpPropertyValue = converter.AssertValueExcelNameToCodeName(excelPropertyValue, excel.CurrentCellRaw());
            excel.MoveRight();
            string assertionSpecificKey = excel.CurrentCell();
            excel.MoveRight();
            string assertionSpecificValue = excel.CurrentCell();
            excel.MoveRight();
            string roundTripValue = excel.CurrentCell();
            excel.MoveLeft(5);

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
            AddErrorToCodeAndLog($"Invalid assertion operator ('{assertionOperatorOrExcelSubClassNameOrTableOf}') found at cell {excel.CellReferenceA1Style()}. Valid operators are '=' and '{converter.TableOf}'");
        }

        // check to see if it looks like a table, but does not end with converter.TableOf
        void CheckMissingTableOfForAssertion()
        {
            if (LooksLikeAnAssertionTableButIsnt())
                AddErrorToCodeAndLog($"It looks like you might be trying to set up a table assertion, starting at cell {excel.CellReferenceA1Style()}. If this is the case, please make sure that cell {excel.CellReferenceA1Style(excel.row, excel.column + 1)} is '{converter.TableOf}', and that the table itself (the bit with the class name, 'With Properties' etc) starts on column {excel.ColumnReferenceA1Style(excel.column + 2)}");
        }

        bool LooksLikeAnAssertionTableButIsnt() =>
            (
            excel.PeekRight() != converter.TableOf
            &&
                (
                excel.PeekBelowRight() == converter.WithProperties && excel.PeekBelow() == ""
                || excel.PeekBelowRight(1, 2) == converter.WithProperties && excel.PeekBelowRight() == ""
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
            string cSharpVariableName = VariableCase(excelPropertyName);

            var startCellReference = excel.CellReferenceA1Style();

            CheckMissingWithPropertiesInAssertionTable(startCellReference);
            CheckBadIndentationInAssertionTable(startCellReference);

            // first row is property name, "table of" and property type
            // then With Properties
            // then the headers 
            excel.MoveDown();
            excel.MoveDown();
            var assertionTableHeaders = ReadAssertionTableHeaders();

            CheckMissingHeadersInAssertionTable(startCellReference, assertionTableHeaders);

            // there can be 2 or 4 rows in the header
            excel.MoveDown(assertionTableHeaders.Max(a => a.Rows()));

            code.Add(LeadingComma(assertIndex) + $"new TableAssertion<{cSharpClassName}, {cSharpSubClassName}>");
            using (code.AutoCloseBracketAndIndent())
            {
                using (excel.AutoRestoreMoveRight(2))
                {
                    code.Add($"{cSharpVariableName} => {cSharpVariableName}.{cSharpSubMethodName},");
                    code.Add($"new List<List<IAssertion<{cSharpSubClassName}>>>");
                    using (code.AutoCloseCurlyBracket())
                    {
                        int tableRowIndex = 0;
                        while (excel.row <= excel.GetLastRow() && !excel.RowToCurrentColumnIsEmpty() && !excel.AnyPrecedingColumnHasAValue()) // should encapsulate this conditional
                        {
                            code.Add($"{LeadingComma(tableRowIndex)}new List<IAssertion<{cSharpSubClassName}>>");
                            tableRowIndex++;
                            using (code.AutoCloseCurlyBracket())
                            {
                                using (code.AutoCloseIndent())
                                {
                                    using (excel.SavePosition())
                                    {
                                        int tableColumnIndex = 0;
                                        foreach (var assertionTableHeader in assertionTableHeaders)
                                        {
                                            DoTableCellAssertion(tableColumnIndex, cSharpSubClassName, cSharpVariableName, assertionTableHeader);
                                            excel.MoveRight();
                                            tableColumnIndex++;
                                        }
                                    }
                                }
                            }

                            excel.MoveDown();
                        }

                        CheckNoRowsInAssertionTable(startCellReference, excel.CellReferenceA1Style(), tableRowIndex);
                    }
                }
            }

            // we should leave the row at the last row of the TABLE assertions. The calling DoAssertions (it is recursive) calls MoveDown to move on to the next assertion row after this method.
            // This seems like a bad pattern though, we should probably fix it up. Assertions should be responsible for moving the cursor on themselves.
            excel.MoveUp();
        }

        void CheckBadIndentationInAssertionTable(string tableStartCellReference)
        {
            using (excel.AutoRestoreMoveDownRight(2, 2))
            {
                if (excel.CurrentCell() == "" && excel.PeekRight() != "")
                    throw new ExcelToCodeException($"The assertion table starting at {tableStartCellReference} is not formatted correctly. The properties start on column {excel.ColumnReferenceA1Style(excel.column + 1)}, but they should start one to the left, on column {excel.ColumnReferenceA1Style()}");
            }
        }

        void CheckMissingWithPropertiesInAssertionTable(string tableStartCellReference)
        {
            using (excel.AutoRestoreMoveDownRight(1, 2))
            {
                if (excel.CurrentCell() != converter.WithProperties)
                    throw new ExcelToCodeException($"The assertion table starting at {tableStartCellReference} is not formatted correctly. Cell {excel.CellReferenceA1Style()} should be '{converter.WithProperties}', but is '{excel.CurrentCell()}'");
            }
        }

        void CheckMissingHeadersInAssertionTable(string tableStartCellReference, IEnumerable<AssertionTableHeader> headers)
        {
            if (!headers.Any())
                throw new ExcelToCodeException($"The assertion table starting at cell {tableStartCellReference} has no headers. There should be a row of Property Names starting at {excel.CellReferenceA1Style()}, with rows of Property Values below.");
        }

        void CheckNoRowsInAssertionTable(string tableStartCellReference, string rowsStartCellReference, int numberOfRows)
        {
            if (numberOfRows == 0)
                throw new ExcelToCodeException($"The assertion table starting at cell {tableStartCellReference} has no rows. There should be at least one row of Property Values starting at {rowsStartCellReference}.");
        }

        IEnumerable<AssertionTableHeader> ReadAssertionTableHeaders()
        {
            var assertionTableHeaders = new List<AssertionTableHeader>();

            using (excel.SavePosition())
            {
                excel.MoveRight(2);

                while (string.IsNullOrWhiteSpace(excel.CurrentCell()) == false)
                {
                    using (excel.SavePosition())
                    {
                        var propertyName = excel.CurrentCell();

                        excel.MoveDown();
                        var assertionOperator = excel.CurrentCell();

                        excel.MoveDown();
                        string assertionSpecificKey = "";
                        string assertionSpecificValue = "";
                        if (excel.CurrentCell().ToLowerInvariant() == "percentageprecision" || excel.CurrentCell().ToLowerInvariant() == "stringformat")
                        {
                            assertionSpecificKey = excel.CurrentCell();
                            excel.MoveDown();
                            assertionSpecificValue = excel.CurrentCell();
                        }

                        assertionTableHeaders.Add(
                            new AssertionTableHeader(
                                propertyName,
                                assertionOperator,
                                assertionSpecificKey,
                                assertionSpecificValue));
                    }

                    excel.MoveRight();
                }
            }

            return assertionTableHeaders;
        }

        void DoTableCellAssertion(int assertIndex, string cSharpClassName, string cSharpVariableName, AssertionTableHeader assertionTableHeader)
        {
            string excelPropertyValue = excel.CurrentCell();
            string cSharpPropertyValue = converter.AssertValueExcelNameToCodeName(excelPropertyValue, excel.CurrentCellRaw());

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
        }

        void DoEqualityWithStringFormatAssertion(
            int assertIndex,
            string cSharpPropertyName,
            string cSharpPropertyValue,
            string cSharpClassName,
            string cSharpVariableName,
            string assertionSpecificValue)
        {
            code.Add($"{LeadingComma(assertIndex)} new EqualityAssertionWithStringFormat<{cSharpClassName}>({cSharpVariableName} => {cSharpVariableName}.{cSharpPropertyName}, {cSharpPropertyValue}, \"{assertionSpecificValue}\")");
        }

        void DoEqualityWithPercentagePrecisionAssertion(
            int assertIndex,
            string cSharpPropertyName,
            string cSharpPropertyValue,
            string cSharpClassName,
            string cSharpVariableName,
            string assertionSpecificValue)
        {
            code.Add($"{LeadingComma(assertIndex)} new EqualityAssertionWithPercentagePrecision<{cSharpClassName}>({cSharpVariableName} => {cSharpVariableName}.{cSharpPropertyName}, {cSharpPropertyValue}, {assertionSpecificValue})");
        }

        void DoExcelFormulaDoesNotMatchCodeAssertion(
            int assertIndex,
            string cSharpPropertyName,
            string cSharpPropertyValue,
            string roundTripValue,
            string cSharpClassName)
        {
            code.Add($"{LeadingComma(assertIndex)}new ExcelFormulaDoesNotMatchCodeAssertion<{cSharpClassName}>(\"{cSharpPropertyName}\", \"{cSharpPropertyValue}\", \"{roundTripValue}\")");
        }

        void DoEqualityAssertion(
            int assertIndex,
            string cSharpPropertyName,
            string cSharpPropertyValue,
            string cSharpClassName,
            string cSharpVariableName)
        {
            code.Add($"{LeadingComma(assertIndex)} new EqualityAssertion<{cSharpClassName}>({cSharpVariableName} => {cSharpVariableName}.{cSharpPropertyName}, {cSharpPropertyValue})");
        }

        void DoSubAssertion(int assertIndex, string excelPropertyName, string excelSubClassName, string cSharpClassName)
        {
            string cSharpSubClassName = converter.AssertionSubClassExcelNameToCodeName(excelSubClassName);
            string cSharpSubMethodName = converter.AssertionSubPropertyExcelNameToCodeMethodName(excelPropertyName);
            string cSharpVariableName = VariableCase(excelPropertyName);

            code.Add(LeadingComma(assertIndex) + $"new ParentAssertion<{cSharpClassName}, {cSharpSubClassName}>");
            using (code.AutoCloseBracketAndIndent())
            {
                using (excel.AutoRestoreMoveRight())
                {
                    code.Add($"{cSharpVariableName} => {cSharpVariableName}.{cSharpSubMethodName},");
                    code.Add($"new List<IAssertion<{cSharpSubClassName}>>");

                    using (code.AutoCloseCurlyBracket())
                    {
                        DoAssertions(cSharpSubClassName, cSharpVariableName);

                        // we should leave the row at the last row of the sub assertions. The calling DoAssertions (it is recursive) calls MoveDown to move on to the next assertion row after this method.
                        // This seems like a bad pattern though, we should probably fix it up. Assertions should be responsible for moving the cursor on themselves.
                        excel.MoveUp();
                    }

                }
            }
        }

    }
}
