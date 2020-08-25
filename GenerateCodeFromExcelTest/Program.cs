using CustomerTestsExcel;
using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateCodeFromExcelTest
{
    // To work with the SampleTests project included in this solution, use the following command line parameters
    // The path to the assembly must be absolute, so you will probably need to be change it
    // /folder "..\..\..\..\SampleTests" /namespace SampleTests /usings "SampleSystemUnderTest SampleSystemUnderTest.AnovaCalculator SampleSystemUnderTest.Routing SampleSystemUnderTest.VermeulenNearWakeLength SampleSystemUnderTest.Calculator SampleSystemUnderTest.CustomProperties SampleSystemUnderTest.Misc" /assertionClassPrefix "I" /assembliesUnderTest "C:\Users\cburge\Documents\repos\customer-tests-excel\Builtdlls\debug\SampleSystemUnderTest.dll"

    // /folder C:\Users\cburge\Documents\repos\software\Energy\Gross\CustomerTestsExcel\ /project RES.Energy.Gross.CustomerTestsExcel.csproj /namespace RES.Energy.Gross.CustomerTestsExcel /usings "RES.Energy.Gross.Calculation RES.Energy.Gross.Calculation.Base RES.Energy.Gross.Calculation.DnvGlBlockage RES.TurbineLayout.Base RES.TurbineModel.Base RES.Energy.Gross.Base RES.WindAnalysis.WindClimate.Base" /assertionClassPrefix "" /assembliesUnderTest "C:\Users\cburge\Documents\repos\software\BuiltDLLs\Debug\RES.TurbineModel.Base.dll C:\Users\cburge\Documents\repos\software\BuiltDLLs\Debug\RES.TurbineLayout.Base.dll C:\Users\cburge\Documents\repos\software\BuiltDLLs\Debug\RES.Energy.Gross.Calculation.Base.dll C:\Users\cburge\Documents\repos\software\BuiltDLLs\Debug\RES.Energy.Gross.Base.dll C:\Users\cburge\Documents\repos\software\BuiltDLLs\Debug\RES.WindAnalysis.WindClimate.Base.dll"
    static class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string folder = GetSetting(args, "folder");
                if (string.IsNullOrWhiteSpace(folder))
                    return ShowHelp("Missing Parameter: You must select a folder with /folder");

                string rootNamespace = GetSetting(args, "namespace");
                if (string.IsNullOrWhiteSpace(rootNamespace))
                    return ShowHelp("Missing Parameter: You must specify a root namespace with /namespace");

                string assertionClassPrefix = GetSetting(args, "assertionClassPrefix");

                var usings = GetSetting(args, "usings").Split(' ').ToList();

                var assembliesUnderTest = GetSetting(args, "assembliesUnderTest").Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                var logger = new ConsoleLogger();

                new FileSystemGenerateCSharpFromExcel(
                    logger,
                    folder,
                    rootNamespace,
                    "ExcelTests",
                    usings,
                    assembliesUnderTest,
                    assertionClassPrefix,
                    new ExcelTabularLibrary()
                ).Create();

                return logger.HasErrors ? -1 : 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error encountered creating code for tests:");
                Console.Error.WriteLine(ex.Message);
                return -2;
            }
        }

        static string GetSetting(string[] args, string settingName)
        {
            string settingValue = args.SkipWhile(a => !a.ToLowerInvariant().StartsWith("/" + settingName.ToLowerInvariant())).Skip(1).Take(1).FirstOrDefault();

            return settingValue ?? "";
        }

        static int ShowHelp(string errorMessage)
        {
            if (errorMessage != null)
            {
                Console.Error.WriteLine(errorMessage);
                Console.Out.WriteLine();
            }
            Console.Out.WriteLine("Usage:");
            Console.Out.WriteLine("GenerateCodeFromExcelTest /FOLDER <folder> /NAMESPACE <namespace> /AssertionClassPrefix <assertionClassPrefix> [/help]");
            Console.Out.WriteLine();
            Console.Out.WriteLine("<folder> : the full path to the specification folder for the project, excel files are expected to be in a sub folder called ExcelTests");
            Console.Out.WriteLine("<namespace> : the root namespace for the project");
            Console.Out.WriteLine("<usings> : Space delimited list of namespaces to add as 'using' statements");
            Console.Out.WriteLine("<assertionClassPrefix> : Apply a prefix to the class / type names in the assertion section. For example, 'I', would output 'ICargo', in place of 'Cargo'");
            Console.Out.WriteLine("/help : show this message and exit");
            return -1;
        }
    }
}
