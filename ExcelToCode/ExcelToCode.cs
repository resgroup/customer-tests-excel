using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.ExcelToCode
{
    // this clase is very much too big, split in to smaller ones
    // easy targets would be given, when, assert.
    // the various property types could probably also be split off quite easily as well
    // a better way of doing this would probably be to form a representation of the test in code (like the _assertions property) and then write this out to a string in a different class. This involves some framework overhead, but will definitely be worthwhile if this gets more complex.
    // might be good to make it obvious which operations relate to excel and which to the code generation. eg "excel.MoveDown" and "cSharp.DeclareVariable"
    public class ExcelToCode : ExcelToCodeBase
    {
        string _assertionClassPrefix;
        public ExcelToCode(ICodeNameToExcelNameConverter converter) : base(converter) { }

        public string GenerateCSharpTestCode(IEnumerable<string> usings, string assertionClassPrefix, ITabularPage worksheet, string projectRootNamespace, string workBookName)
        {
            _worksheet = worksheet;
            _code = new AutoIndentingStringBuilder("    ");
            _column = 1;
            _row = 1;
            _assertionClassPrefix = assertionClassPrefix ?? "";

            var description = DoSpecification();
            DoGiven(usings, description, projectRootNamespace, workBookName);
            DoWhen();
            DoAssert();

            EndSpecification();

            return _code.ToString();
        }

        void EndSpecification()
        {
            Output("};");
            Output("}");
            // This is so that when writing back out to excel, the prefix can be removed. So the prefix exists in the code, but not in excel, and is round trippable
            Output($"protected override string AssertionClassPrefixAddedByGenerator => \"{_assertionClassPrefix}\";");
            Output("}");
            Output("}");
        }

        string DoSpecification()
        {
            MoveDownToToken(_converter.Specification);

            Indent();
            var description = CurrentCell();
            UnIndent();

            MoveDown();

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
            Output();
            foreach (var usingNamespace in usings)
                Output($"using {usingNamespace};");
            Output();
            Output($"namespace {projectRootNamespace}.{_converter.ExcelFileNameToCodeNamespacePart(workBookName)}");
            Output("{");
            Output("[TestFixture]");
            Output($"public class {_converter.ExcelSpecificationNameToCodeSpecificationClassName(_worksheet.Name)} : SpecificationBase<{CSharpSUTSpecificationSpecificClassName()}>, ISpecification<{CSharpSUTSpecificationSpecificClassName()}>");
            Output("{");
            Output("public override string Description()");
            Output("{");
            Output($"return \"{ description}\";");
            Output("}");
            Output();
            Output("// arrange");
            Output($"public override {CSharpSUTSpecificationSpecificClassName()} Given()");
            Output("{");
        }

        void DoGiven(IEnumerable<string> usings, string description, string projectRootNamespace, string workBookName)
        {
            MoveDownToToken(_converter.Given);

            using (AutoRestoreExcelMoveRight())
            {
                _sutName = CurrentCell();

                StartOutput(usings, description, projectRootNamespace, workBookName);

                CreateObject(CSharpSUTVariableName(), CSharpSUTSpecificationSpecificClassName());

                EndGiven();
            }
        }

        void CreateObject(string cSharpVariableName, string cSharpClassName)
        {
            MoveDown(); // this is a bit mysterious

            DeclareVariable(cSharpVariableName, cSharpClassName);

            SetVariableProperties(cSharpVariableName, _converter.Properties, "");

            MoveUp(); // this is a bit mysterious
        }

        void DeclareVariable(string cSharpVariableName, string cSharpClassName) =>
            Output($"var {cSharpVariableName} = new {cSharpClassName}();");

        void SetVariableProperties(string cSharpVariableName, string markAtBeginningOfProperties, string cSharpVariableNamePostfix)
        {
            if (CurrentCell() == markAtBeginningOfProperties)
            {
                using (AutoRestoreExcelMoveRight())
                {
                    MoveDown();
                    while (!string.IsNullOrEmpty(CurrentCell()))
                    {
                        DoProperty(cSharpVariableName, cSharpVariableNamePostfix);
                        MoveDown();
                    }
                }
            }
        }

        void DoProperty(string cSharpVariableName, string cSharpVariableNamePostfix)
        {
            // "Calibrations(0) of", "InstrumentCalibration"
            // methodname = "Calibrations_of"
            // index = 0;
            // variable name = instrumentCalibration
            // we actually don't need the index any more now that each item in the list has its own scope, so we could remove it from the excel definition
            var excelGivenLeft = CurrentCell();
            using (AutoRestoreExcelMoveRight())
            {
                var excelGivenRight = CurrentCellRaw();
                var excelGivenRightString = excelGivenRight != null ? excelGivenRight.ToString() : string.Empty;

                var cSharpMethodName = _converter.GivenPropertyNameExcelNameToCodeName(excelGivenLeft);

                if (IsTable(excelGivenLeft))
                {
                    using (Scope())
                    {
                        string cSharpClassName = _converter.ExcelClassNameToCodeName(excelGivenRightString);
                        string cSharpChildVariableName = TableVariableNameFromMethodName(cSharpMethodName);

                        CreateObjectsFromTable(cSharpChildVariableName, excelGivenRightString, cSharpClassName);
                        Output(cSharpVariableName + cSharpVariableNamePostfix + "." + cSharpMethodName + "(" + cSharpChildVariableName + ")" + ";");
                    }
                }
                else if (HasGivenSubProperties())
                {
                    Output();
                    using (Scope())
                    {
                        string cSharpClassName = _converter.ExcelClassNameToCodeName(excelGivenRightString);
                        string cSharpChildVariableName = VariableCase(excelGivenRightString.Replace(".", ""));

                        CreateObject(cSharpChildVariableName, cSharpClassName);
                        Output(cSharpVariableName + cSharpVariableNamePostfix + "." + cSharpMethodName + "(" + cSharpChildVariableName + ")" + ";");
                    }
                }
                else
                {
                    Output($"{cSharpVariableName}{cSharpVariableNamePostfix}.{cSharpMethodName}({_converter.PropertyValueExcelToCode(excelGivenLeft, excelGivenRight)});");
                }
            }
        }

        string TableVariableNameFromMethodName(string cSharpMethodName)
        {
            // change "Cargo table_of" (method name from Excel) to "cargoTable (c# variable name)"
            return VariableCase(cSharpMethodName.Substring(0, cSharpMethodName.Length - 3));
        }

        bool IsTable(string excelGivenLeft)
        {
            return excelGivenLeft.EndsWith("table of", StringComparison.InvariantCultureIgnoreCase);
        }

        void CreateObjectsFromTable(string cSharpVariableName, string cSharpClassName, string cSharpSpecificationSpecificClassName)
        {
            // read token that speficy the start of the properties
            //todo: this should always be the first column now that we don't have creational properties to worry about
            MoveDown();
            uint? propertiesStartColumn = FindTokenInCurrentRowFromCurrentColumn(_converter.Properties);

            var headers = ReadHeaders();

            uint lastColumn = headers.Max(h => h.Value.EndColumn);
            uint propertiesEndColumn = lastColumn;

            Output($"var {cSharpVariableName} = new ReportSpecificationSetupClassUsingTable<{cSharpSpecificationSpecificClassName}>();");

            int row = 0;
            uint moveDown = 1 + (headers.Max(h => h.Value.EndRow) - _row);
            MoveDown(moveDown);
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
                            propertiesStartColumn,
                            propertiesEndColumn,
                            headers);

                        Output($"{cSharpVariableName}.Add({indexedCSharpVariableName});");
                    }
                    row++;
                }

                MoveDown();
            }

            MoveUp();
        }

        private Dictionary<uint, TableHeader> ReadHeaders()
        {
            var headers = new Dictionary<uint, TableHeader>();
            MoveDown();
            using (SavePosition())
            {
                while (CurrentCell() != "")
                {
                    var test = CurrentCell();
                    headers.Add(_column, CreatePropertyHeader());
                    MoveRight();
                }
            }

            return headers;
        }

        TableHeader CreatePropertyHeader()
        {
            if (PeekBelow(2) == _converter.Properties)
                return CreateSubClassHeader();

            return new PropertyTableHeader(CurrentCell(), _row, _column);
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
            startRow = _row;
            using (SavePosition())
            {
                propertyName = _converter.GivenPropertyNameExcelNameToCodeName(CurrentCell());
                MoveDown();
                subClassName = CurrentCell();
                MoveDown();
                propertiesStartColumn = FindTokenInCurrentRowFromCurrentColumn(_converter.Properties);

                MoveDown();
                do
                {
                    headers.Add(_column, CreatePropertyHeader());
                    MoveRight();
                } while (PeekAbove(3) == "" && AnyFollowingColumnHasAValue(-3) == true); // Need to detect end of the sub property. This is by the existence of a property name in the parent proeprty header row, which is 3 rows up, so peek for this. The other condition is when there is no more data in the parent property header row

                propertiesEndColumn = _column - 1;
                endRow = _row;
            }

            MoveRight((uint)headers.Count - 1);

            return new SubClassTableHeader(propertyName, subClassName, _converter.ExcelClassNameToCodeName(subClassName), startRow, endRow, propertiesStartColumn, propertiesEndColumn, headers);
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
                if (_column != propertiesStartColumn.Value) throw new ExcelToCodeException("Table must have a 'With Properties' token, which must be on the first column of the table.");

                while (_column <= propertiesEndColumn)
                {
                    SetPropertyOnTableRowVariable(headers, cSharpVariableName);
                    MoveRight();
                }
            }
        }

        void SetPropertyOnTableRowVariable(Dictionary<uint, TableHeader> headers, string cSharpVariableName)
        {
            if (headers[_column] is SubClassTableHeader)
            {
                var subClassHeader = headers[_column] as SubClassTableHeader;
                string subClassCSharpVariableName = $"{cSharpVariableName}_{subClassHeader.SubClassName.Replace(".", "")}"; // this <.Replace(".", "")> is shared with DoProperty, we should move in into the _converter

                SetAllPropertiesOnTableRowVariable(subClassCSharpVariableName, subClassHeader.FullSubClassName, subClassHeader.PropertiesStartColumn, subClassHeader.PropertiesEndColumn, subClassHeader.Headers);

                MoveLeft();

                Output($"{cSharpVariableName}.{_converter.GivenPropertyNameExcelNameToCodeName(subClassHeader.PropertyName)}({subClassCSharpVariableName});");
            }
            else if (headers[_column] is PropertyTableHeader)
            {
                var propertyHeader = headers[_column] as PropertyTableHeader;

                Output($"{cSharpVariableName}.{_converter.GivenPropertyNameExcelNameToCodeName(propertyHeader.PropertyName)}({_converter.PropertyValueExcelToCode(propertyHeader.PropertyName, CurrentCellRaw())});");
            }
            else
            {
                throw new ExcelToCodeException("Unknown type of Table Header");
            }
        }

        void EndGiven()
        {
            Output();
            Output("return " + CSharpSUTVariableName() + ";");
            Output("}");
        }

        bool HasGivenSubProperties()
        {
            return (PeekBelow() == _converter.Properties);
        }

        protected void DoWhen()
        {
            MoveDownToToken(_converter.When);

            using (AutoRestoreExcelMoveRight())
            {
                Output();
                Output("// act");
                Output("public override string When(" + CSharpSUTSpecificationSpecificClassName() + " " + CSharpSUTVariableName() + ")");
                Output("{");

                if (CurrentCell() == _converter.WhenValidating)
                {
                    Output("// no action required here, the business object should have updated its validation in response to property change events");
                    Output("return \"" + _converter.WhenValidating + "\";");
                }
                else if (CurrentCell() == _converter.WhenCalculating)
                {
                    Output(CSharpSUTVariableName() + ".Calculate();");
                    Output("return \"" + _converter.WhenCalculating + "\";");
                }
                else
                {
                    Output(CSharpSUTVariableName() + "." + CurrentCell() + "();");
                    Output("return \"" + CurrentCell() + "\";");
                }

                Output("}");
                Output();
            }

            MoveDown();
        }

        void DoAssert()
        {
            MoveDownToToken(_converter.Assert);

            Output("public override IEnumerable<IAssertion<" + CSharpSUTSpecificationSpecificClassName() + ">> Assertions()");
            Output("{");
            Output("return new List<IAssertion<" + CSharpSUTSpecificationSpecificClassName() + ">>");
            Output("{");

            Indent();

            DoAssertions(CSharpSUTSpecificationSpecificClassName(), VariableCase(CSharpSUTVariableName()));
        }

        void DoAssertions(string cSharpClassName, string cSharpVariableName)
        {
            int assertIndex = 0;

            MoveDown();

            while (_row <= GetLastRow() && !RowToCurrentColumnIsEmpty() && !AnyPrecedingColumnHasAValue())
            {
                DoAssertion(assertIndex, cSharpClassName, cSharpVariableName);

                assertIndex++;

                MoveDown();
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
            string simpleCSharpPropertyName = _converter.AssertPropertyExcelNameToCodeName(excelPropertyName);
            Indent();
            string assertionOperatorOrExcelSubClassNameOrTableOf = CurrentCell();
            Indent();
            string excelPropertyValue = CurrentCell();
            string cSharpPropertyValue = _converter.AssertValueExcelNameToCodeName(excelPropertyValue, CurrentCellRaw());
            Indent();
            string assertionSpecificKey = CurrentCell();
            Indent();
            string assertionSpecificValue = CurrentCell();
            Indent();
            string roundTripValue = CurrentCell();
            UnIndent(5);

            if (string.IsNullOrWhiteSpace(excelPropertyValue))
            {
                DoSubAssertion(assertIndex, simpleCSharpPropertyName, assertionOperatorOrExcelSubClassNameOrTableOf, cSharpClassName);
            }
            else if (assertionOperatorOrExcelSubClassNameOrTableOf == "table of")
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
                throw new Exception("Invalid assertion operator found in excel file: " + assertionOperatorOrExcelSubClassNameOrTableOf);
            }

            if (!string.IsNullOrWhiteSpace(roundTripValue))
            {
                assertIndex++;
                DoExcelFormulaDoesNotMatchCodeAssertion(assertIndex, simpleCSharpPropertyName, cSharpPropertyValue, roundTripValue, cSharpClassName);
            }
        }

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
            string cSharpSubClassName = _assertionClassPrefix + _converter.AssertionSubClassExcelNameToCodeName(excelSubClassName);
            string cSharpSubMethodName = _converter.AssertionSubPropertyExcelNameToCodeMethodName(excelPropertyName);
            string cSharpVariableName = VariableCase(UnIndex(excelPropertyName));

            // first row is property name, "table of" and property type, the headers start on the row below that
            MoveDown();
            var assertionTableHeaders = ReadAssertionTableHeaders();

            Output(LeadingComma(assertIndex) + $"new TableAssertion<{cSharpClassName}, {cSharpSubClassName}>");
            using (AutoCloseBracket())
            {
                using (AutoRestoreExcelMoveRight())
                {
                    Output($"{cSharpVariableName} => {cSharpVariableName}.{cSharpSubMethodName},");
                    Output($"new List<List<IAssertion<{cSharpSubClassName}>>>");
                    using (AutoCloseCurlyBracket())
                    {
                        int tableRowIndex = 0;
                        while (_row <= GetLastRow() && !RowToCurrentColumnIsEmpty() && !AnyPrecedingColumnHasAValue()) // should encapsulate this conditional
                        {
                            Output($"{LeadingComma(tableRowIndex)}new List<IAssertion<{cSharpSubClassName}>>");
                            tableRowIndex++;
                            using (AutoCloseCurlyBracket())
                            {
                                using (_code.AutoCloseIndent())
                                {
                                    using (SavePosition())
                                    {
                                        int tableColumnIndex = 0;
                                        foreach (var assertionTableHeader in assertionTableHeaders)
                                        {
                                            DoTableCellAssertion(tableColumnIndex, cSharpSubClassName, cSharpVariableName, assertionTableHeader);
                                            Indent();
                                            tableColumnIndex++;
                                        }
                                    }
                                }
                            }

                            MoveDown();
                        }
                    }
                }
            }

            // we should leave the row at the last row of the TABLE assertions. The calling DoAssertions (it is recursive) calls MoveDown to move on to the next assertion row after this method.
            // This seems like a bad pattern though, we should probably fix it up. Assertions should be responsible for moving the cursor on themselves.
            MoveUp();
        }

        IEnumerable<AssertionTableHeader> ReadAssertionTableHeaders()
        {
            var assertionTableHeaders = new List<AssertionTableHeader>();

            using (SavePosition())
            {
                // headers are indented one to the right, this makes it visually easier to pick them out, and makes it easier to detect the end of the table in code
                MoveRight();

                while (string.IsNullOrWhiteSpace(CurrentCell()) == false)
                {
                    using (SavePosition())
                    {
                        var propertyName = CurrentCell();

                        MoveDown();
                        var assertionOperator = CurrentCell();

                        MoveDown();
                        string assertionSpecificKey = "";
                        string assertionSpecificValue = "";
                        if (CurrentCell().ToLowerInvariant() == "percentageprecision" || CurrentCell().ToLowerInvariant() == "stringformat")
                        {
                            assertionSpecificKey = CurrentCell();
                            MoveDown();
                            assertionSpecificValue = CurrentCell();
                        }

                        assertionTableHeaders.Add(
                            new AssertionTableHeader(
                                propertyName,
                                assertionOperator,
                                assertionSpecificKey,
                                assertionSpecificValue));
                    }

                    MoveRight();
                }
            }

            // there can be 2 or 4 rows in the header
            MoveDown(assertionTableHeaders.Max(a => a.Rows()));

            return assertionTableHeaders;
        }

        void DoTableCellAssertion(int assertIndex, string cSharpClassName, string cSharpVariableName, AssertionTableHeader assertionTableHeader)
        {
            string excelPropertyValue = CurrentCell();
            string cSharpPropertyValue = _converter.AssertValueExcelNameToCodeName(excelPropertyValue, CurrentCellRaw());

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
            string cSharpSubClassName = _assertionClassPrefix + _converter.AssertionSubClassExcelNameToCodeName(excelSubClassName);
            string cSharpSubMethodName = _converter.AssertionSubPropertyExcelNameToCodeMethodName(excelPropertyName);
            string cSharpVariableName = VariableCase(UnIndex(excelPropertyName));

            Output(LeadingComma(assertIndex) + $"new ParentAssertion<{cSharpClassName}, {cSharpSubClassName}>");
            using (AutoCloseBracket())
            {
                using (AutoRestoreExcelMoveRight())
                {
                    Output($"{cSharpVariableName} => {cSharpVariableName}.{cSharpSubMethodName},");
                    Output($"new List<IAssertion<{cSharpSubClassName}>>");

                    using (AutoCloseCurlyBracket())
                    {
                        using (_code.AutoCloseIndent())
                        {
                            DoAssertions(cSharpSubClassName, cSharpVariableName);

                            // we should leave the row at the last row of the sub assertions. The calling DoAssertions (it is recursive) calls MoveDown to move on to the next assertion row after this method.
                            // This seems like a bad pattern though, we should probably fix it up. Assertions should be responsible for moving the cursor on themselves.
                            MoveUp();
                        }
                    }

                }
            }
        }

    }
}
