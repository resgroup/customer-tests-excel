using RES.Specification.Indentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification.ExcelToCode
{
    // this clase is very much too big, split in to smaller ones
    // easy targets would be give, when, assert.
    // the various property types could probably also be split off quite easily as well
    // a better way of doing this would probably be to form a representation of the test in code (like the _assertions property) and then write this out to a string in a different class. This involves some framework overhead, but will definitely be worthwhile if this gets more complex.
    public class ExcelToCode : ExcelToCodeBase
    {
        private string _assertionClassPrefix;
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

        protected void EndSpecification()
        {
            Output("};");
            Output("}");
            Output("}");
            Output("}");
        }

        protected string DoSpecification()
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
            Output("using RES.Specification;");
            Output("using System.Linq.Expressions;");
            Output();
            foreach (var usingNamespace in usings)
                Output($"using {usingNamespace};");
            Output();
            Output($"namespace {projectRootNamespace}{_converter.ExcelFileNameToCodeNamespacePart(workBookName)}");
            Output("{");
            Output("[TestFixture]");
            Output($"public class {_converter.ExcelSpecificationNameToCodeSpecificationClassName(_worksheet.Name)} : SpecificationBase<{CSharpSUTSpecificationSpecificClassName()}>, ISpecification<{CSharpSUTSpecificationSpecificClassName()}>");
            Output("{");
            Output("public override string Description()");
            Output("{");
            Output($"return \"{ description}\";");
            Output("}");
            Output();
            Output("public override string TrunkRelativePath()");
            Output("{");
            Output($"return \"{projectRootNamespace.Replace('.', '\\')}\";");
            Output("}");
            Output();
            Output("// arrange");
            Output($"public override {CSharpSUTSpecificationSpecificClassName()} Given()");
            Output("{");
        }

        void DoGiven(IEnumerable<string> usings, string description, string projectRootNamespace, string workBookName)
        {
            MoveDownToToken(_converter.Given);

            using (AutoCloseIndent())
            {
                _sutName = CurrentCell();

                StartOutput(usings, description, projectRootNamespace, workBookName);

                CreateObject(CSharpSUTVariableName(), CSharpSUTSpecificationSpecificClassName());

                EndGiven();
            }
        }

        void CreateObject(string cSharpVariableName, string cSharpClassName)
        {
            MoveDown();

            Output($"var {CreationalCSharpVariableName(cSharpVariableName)} = new {cSharpClassName}CreationalProperties();");

            DoProperties(cSharpVariableName, _converter.Creational, "CreationalProperties");

            Output($"var {cSharpVariableName} = new {cSharpClassName}({CreationalCSharpVariableName(cSharpVariableName)});");

            DoProperties(cSharpVariableName, _converter.Properties, "");

            MoveUp();
        }

        void DoProperties(string cSharpNonCreationalVariableName, string markAtBeginningOfProperties, string cSharpVariableNamePostfix)
        {
            if (CurrentCell() == markAtBeginningOfProperties)
            {
                using (AutoCloseIndent())
                {
                    MoveDown();
                    while (!string.IsNullOrEmpty(CurrentCell()))
                    {
                        DoProperty(cSharpNonCreationalVariableName, cSharpVariableNamePostfix);
                        MoveDown();
                    }
                }
            }
        }

        protected void DoProperty(string cSharpNonCreationalVariableName, string cSharpVariableNamePostfix)
        {
            // "Calibrations(0) of", "InstrumentCalibration"
            // methodname = "Calibrations_of"
            // index = 0;
            // variable name = instrumentCalibration
            // we actually don't need the index any more now that each item in the list has its own scope, so we could remove it from the excel definition
            var excelGivenLeft = CurrentCell();
            using (AutoCloseIndent())
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
                        Output(cSharpNonCreationalVariableName + cSharpVariableNamePostfix + "." + cSharpMethodName + "(" + cSharpChildVariableName + ")" + ";");
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
                        Output(cSharpNonCreationalVariableName + cSharpVariableNamePostfix + "." + cSharpMethodName + "(" + cSharpChildVariableName + ")" + ";");
                    }
                }
                else
                {
                    string cSharpVariableName = cSharpNonCreationalVariableName + cSharpVariableNamePostfix;
                    Output(cSharpVariableName +  "." + cSharpMethodName + "(" + _converter.PropertyValueExcelToCode(excelGivenLeft, excelGivenRight) + ")" + ";");
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
            // read tokens that speficy the start of the creational and normal properties
            MoveDown();
            uint? creationalPropertiesStartColumn = FindTokenInCurrentRowFromCurrentColumn(_converter.Creational);
            uint? propertiesStartColumn = FindTokenInCurrentRowFromCurrentColumn(_converter.Properties);

            var headers = ReadHeaders();

            uint lastColumn = (uint)headers.Max(h => h.Value.EndColumn);
            uint propertiesEndColumn = lastColumn;
            uint creationalPropertiesEndColumn = propertiesStartColumn.HasValue ? propertiesStartColumn.Value - 1 : lastColumn;

            Output($"var {cSharpVariableName} = new ReportSpecificationSetupClassUsingTable<{cSharpSpecificationSpecificClassName}>();");

            int row = 0;
            uint moveDown = 1 + (headers.Max(h => h.Value.EndRow) - _row);
            MoveDown(moveDown);
            while (TableHasMoreRows())
            {
                using (SavePosition())
                {
                    string indexedCSharpVariableName = VariableCase(cSharpClassName);
                    string indexedCreationalCSharpVariableName = $"{indexedCSharpVariableName}CreationalProperties";

                    using (Scope())
                    {
                        SetAllPropertiesOnTableRowVariable(
                            indexedCSharpVariableName, 
                            indexedCreationalCSharpVariableName, 
                            cSharpSpecificationSpecificClassName, 
                            creationalPropertiesStartColumn, 
                            creationalPropertiesEndColumn, 
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
            if (PeekBelow(2) == _converter.Creational || PeekBelow(2) == _converter.Properties)
                return CreateSubClassHeader();

            return new PropertyTableHeader(CurrentCell(), _row, _column);
        }

        SubClassTableHeader CreateSubClassHeader()
        {
            string propertyName;
            string subClassName;
            uint? creationalPropertiesStartColumn;
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
                creationalPropertiesStartColumn = FindTokenInCurrentRowFromCurrentColumn(_converter.Creational);
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

            uint creationalPropertiesEndColumn = propertiesStartColumn.HasValue ? propertiesStartColumn.Value - 1 : propertiesEndColumn;

            MoveRight((uint)headers.Count - 1);

            return new SubClassTableHeader(propertyName, subClassName, _converter.ExcelClassNameToCodeName(subClassName), startRow, endRow, creationalPropertiesStartColumn, creationalPropertiesEndColumn, propertiesStartColumn, propertiesEndColumn, headers);
        }

        void SetAllPropertiesOnTableRowVariable(string cSharpVariableName, string creationalCSharpVariableName, string cSharpSpecificationSpecificClassName, uint? creationalPropertiesStartColumn, uint creationalPropertiesEndColumn, uint? propertiesStartColumn, uint propertiesEndColumn, Dictionary<uint, TableHeader> propertyNames)
        {
            Output($"var {creationalCSharpVariableName} = new {cSharpSpecificationSpecificClassName}CreationalProperties();");

            SetCreationalPropertiesOnTableRowVariable(creationalPropertiesStartColumn, propertyNames, creationalPropertiesEndColumn, creationalCSharpVariableName);

            Output($"var {cSharpVariableName} = new {cSharpSpecificationSpecificClassName}({creationalCSharpVariableName});");

            SetNonCreationalPropertiesOnTableRowVariable(propertiesStartColumn, propertyNames, propertiesEndColumn, cSharpVariableName);
        }

        // This still works with sub classes in tables
        bool TableHasMoreRows()
        {
            if (AllColumnsAreEmpty()) return false;
            if (AnyPrecedingColumnHasAValue()) return false;

            return true;
        }

        void SetCreationalPropertiesOnTableRowVariable(uint? propertiesStartColumn, Dictionary<uint, TableHeader> headers, uint propertiesEndColumn, string cSharpVariableName)
        {
            SetCreationalOrClassPropertiesOnTableRowVariable(propertiesStartColumn, headers, propertiesEndColumn, cSharpVariableName);
        }

        void SetNonCreationalPropertiesOnTableRowVariable(uint? propertiesStartColumn, Dictionary<uint, TableHeader> headers, uint propertiesEndColumn, string cSharpVariableName)
        {
            SetCreationalOrClassPropertiesOnTableRowVariable(propertiesStartColumn, headers, propertiesEndColumn, cSharpVariableName);
        }

        void SetCreationalOrClassPropertiesOnTableRowVariable(uint? propertiesStartColumn, Dictionary<uint, TableHeader> headers, uint propertiesEndColumn, string cSharpVariableName)
        {
            if (propertiesStartColumn.HasValue)
            {
                if (_column != propertiesStartColumn.Value) throw new ExcelToCodeException("Table must have 'With Creational' and / or 'With Properties' tokens, the first of which must be on the first column of the table, and both of which must be within the columns of the table.");

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
                string subClassCSharpVariableName = $"{cSharpVariableName}_{subClassHeader.subClassName.Replace(".", "")}"; // this <.Replace(".", "")> is shared with DoProperty, we should move in into the _converter

                SetAllPropertiesOnTableRowVariable(subClassCSharpVariableName, CreationalCSharpVariableName(subClassCSharpVariableName), subClassHeader.FullSubClassName, subClassHeader.CreationalPropertiesStartColumn, subClassHeader.CreationalPropertiesEndColumn, subClassHeader.PropertiesStartColumn, subClassHeader.PropertiesEndColumn, subClassHeader.Headers);

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
            return (PeekBelow() == _converter.Creational || PeekBelow() == _converter.Properties);
        }

        protected void DoWhen()
        {
            MoveDownToToken(_converter.When);

            using (AutoCloseIndent())
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
        protected void DoAssertion(int assertIndex, string cSharpClassName, string cSharpVariableName)
        {
            string excelPropertyName = CurrentCell();
            string simpleCSharpPropertyName = _converter.AssertPropertyExcelNameToCodeName(excelPropertyName);
            Indent();
            string assertionOperatorOrExcelSubClassName = CurrentCell();
            Indent();
            string excelPropertyValue = CurrentCell();
            string cSharpPropertyValue = _converter.AssertValueExcelNameToCodeName(excelPropertyName, CurrentCellRaw());
            Indent();
            string assertionSpecificKey = CurrentCell();
            Indent();
            string assertionSpecificValue = CurrentCell();
            Indent();
            string roundTripValue = CurrentCell();
            UnIndent(5);

            if (string.IsNullOrWhiteSpace(excelPropertyValue))
            {
                DoSubAssertion(assertIndex, simpleCSharpPropertyName, assertionOperatorOrExcelSubClassName, cSharpClassName);
            }
            else if (assertionOperatorOrExcelSubClassName == "=" && assertionSpecificKey.ToLowerInvariant() == "percentageprecision")
            {
                DoEqualityWithPercentagePrecisionAssertion(assertIndex, excelPropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName, assertionSpecificValue);
            }
            else if (assertionOperatorOrExcelSubClassName == "=" && assertionSpecificKey.ToLowerInvariant() == "stringformat")
            {
                DoEqualityWithStringFormatAssertion(assertIndex, excelPropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName, assertionSpecificValue);
            }
            else if (assertionOperatorOrExcelSubClassName == "=")
            {
                DoEqualityAssertion(assertIndex, excelPropertyName, cSharpPropertyValue, cSharpClassName, cSharpVariableName);
            }
            else
            {
                throw new Exception("Invalid assertion operator found in excel file: " + assertionOperatorOrExcelSubClassName);
            }

            if (!string.IsNullOrWhiteSpace(roundTripValue))
            {
                assertIndex++;
                DoExcelFormulaDoesNotMatchCodeAssertion(assertIndex, simpleCSharpPropertyName, cSharpPropertyValue, roundTripValue, cSharpClassName);
            }
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
            Output(LeadingComma(assertIndex) + "new EqualityAssertion<" + cSharpClassName + ">(" + cSharpVariableName + " => " + cSharpVariableName + "." + cSharpPropertyName + ", " + cSharpPropertyValue + ")");
        }

        void DoSubAssertion(int assertIndex, string excelPropertyName, string excelSubClassName, string cSharpClassName)
        {
            string cSharpSubClassName = _assertionClassPrefix + _converter.AssertionSubClassExcelNameToCodeName(excelSubClassName);
            string cSharpSubMethodName = _converter.AssertionSubPropertyExcelNameToCodeName(excelPropertyName);
            string cSharpVariableName = VariableCase(UnIndex(excelPropertyName));

            Output(LeadingComma(assertIndex) + "new ParentAssertion<" + cSharpClassName + ", " + cSharpSubClassName + ">");
            Output("(");
            using (_code.AutoCloseIndent())
            {
                Output(cSharpVariableName + " => " + cSharpVariableName + "." + cSharpSubMethodName + ",");
                Output("new List<IAssertion<" + cSharpSubClassName + ">>");

                Output("{");
                using (AutoCloseIndent())
                {
                    DoAssertions(cSharpSubClassName, cSharpVariableName);

                    MoveUp(); // we should leave the row at the last row of the sub assertions. The calling DoAssertions (it is recursive) calls MoveDown to move on to the next assertion row after this method.
                }
                Output("}");
            }
            Output(")");
        }

    }
}
