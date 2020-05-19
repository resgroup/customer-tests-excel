namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public static class ClassNameMatcher
    {
        public static bool NamesMatch(string cSharpClassName, string excelClassName) =>
            excelClassName.ToLowerInvariant() == "float"
            || excelClassName == cSharpClassName
            || $"I{excelClassName}" == cSharpClassName;
    }
}