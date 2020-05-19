using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassFunction : IGivenClassProperty
    {
        public string Name { get; }
        public ExcelPropertyType Type =>
            ExcelPropertyType.Function;
        // This is a very obvious violation of the liskov substituation principle, and the interface segrgation principle. Not sure what to do about it yet.
        public string ClassName =>
            "";
        public string ExampleValue =>
            "";

        public GivenClassFunction(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new System.ArgumentException("name is required", nameof(name));

            Name = name;
        }

        public bool TypesMatch(Type cSharpPropertytype) =>
            false;
            //ClassNameMatcher.NamesMatch(cSharpPropertytype.Name, ClassName);

        public override string ToString() =>
            $"Name {Name}, Type {Type}";

        public override bool Equals(object obj) =>
            obj is GivenClassFunction property
            && Name == property.Name;

        public override int GetHashCode() =>
            Name.GetHashCode();
    }
}