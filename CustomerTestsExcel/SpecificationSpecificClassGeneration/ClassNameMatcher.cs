namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public static class ClassNameMatcher
    {
        public static bool NamesMatch(string cSharpClassName, string excelClassName) =>
            IsFramworkSuppliedClass(excelClassName)
            || excelClassName == cSharpClassName
            || $"I{excelClassName}" == cSharpClassName;

        public static bool IsFramworkSuppliedClass(string excelClassName)
        {
            // if this is a framework supplied class, then we don't want to generate a specific setup class
            // for it, as the framework already provides it.
            // Currently the framework supplied classes are:
            // - SpecificationSpecificFloat
            // - SpecificationSpecificInteger
            // - SpecificationSpecificDateTime
            // - SpecificationSpecificString
            // We could do some fancy reflection here to find these, but not sure it is merited
            return
                excelClassName == "Float"
                || excelClassName == "Integer"
                || excelClassName == "DateTime"
                || excelClassName == "String";
        }

    }
}