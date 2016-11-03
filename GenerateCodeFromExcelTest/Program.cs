using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenerateCodeFromExcelTest
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string folder = GetSetting(args, "folder");
                if (string.IsNullOrWhiteSpace(folder))
                    return ShowHelp("Missing Parameter: You must select a folder with /folder");

                string specificationProject = GetSetting(args, "project");
                if (string.IsNullOrWhiteSpace(specificationProject))
                    return ShowHelp("Missing Parameter: You must select a specification project with /project");

                string rootNamespace = GetSetting(args, "namespace");
                if (string.IsNullOrWhiteSpace(rootNamespace))
                    return ShowHelp("Missing Parameter: You must specify a root namespace with /namespace");

                RES.Specification.TestProjectCreator.Create(folder, specificationProject, rootNamespace, new RES.Specification.ExcelTabularLibrary());

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error encountered creating code for tests:");
                Console.Error.WriteLine(ex.Message);
                return -1;
            }

            Console.Out.WriteLine("Tests created");
            return 0;
        }

        static string GetSetting(string[] args, string settingName)
        {
            string settingValue = args.SkipWhile(a => !a.ToLower().StartsWith("/" + settingName.ToLower())).Skip(1).Take(1).FirstOrDefault();

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
            Console.Out.WriteLine("GenerateCodeFromExcelTest /FOLDER <folder> /NAMESPACE <namespace> [/help]");
            Console.Out.WriteLine();
            Console.Out.WriteLine("<folder> : the full path to the specification folder for the project, excel files are expected to be in a sub folder called ExcelTests");
            Console.Out.WriteLine("<namespace> : the root namespace for the project");
            Console.Out.WriteLine("/help : show this message and exit");
            return -1;
        }
    }
}
