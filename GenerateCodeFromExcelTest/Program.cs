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
            string folder = null;
            string nameSpace = null;
            for (int i = 0; i < args.Length; i++)
            {
                if(args[i][0] != '/')
                    return ShowHelp("Argument " + i + " \"" + args[i] + "\" should be an option starting with /, but it isn't");

                if(char.ToUpper(args[i][1]) != 'H' && char.ToUpper(args[i][1]) != '?')
                    if(i == args.Length - 1 || args[i+1][0] == '/')
                        return ShowHelp("Argument " + i + " \"" + args[i] + "\" should be followed by a value, but it isn't");

                switch (char.ToUpper(args[i][1]))
                {
                    case 'F': // FOLDER
                        folder = args[++i];
                        break;
                    case 'N': // NAMESPACE
                        nameSpace = args[++i];
                        break;
                    case 'H': // HELP
                    case '?':
                        ShowHelp(null);
                        return 0;
                }
            }

            if (string.IsNullOrWhiteSpace(folder))
                return ShowHelp("Missing Parameter: You must select a folder using the /FOLDER option.");
            if (string.IsNullOrWhiteSpace(nameSpace))
                return ShowHelp("Missing Parameter: You must select a namespace folder using the /NAMESPACE option.");

            try
            {
                RES.Specification.TestProjectCreator.Create(folder, nameSpace, new RES.Excel.OpenXML.OpenXMLExcelApplication());
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
