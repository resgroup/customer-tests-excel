using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;

namespace AddNavigationToSpecificationHTMLOutputFiles
{
    class Program
    {
        static int Main(string[] args)
        {
            if (string.IsNullOrWhiteSpace(GetSetting(args, "rootfolder")) || string.IsNullOrWhiteSpace(GetSetting(args, "description"))) 
            {
                Console.WriteLine(@"Usage: AddNavigationToSpecificationHTMLOutputFiles /rootFolder <RootFolder> /description <Description>");
                Console.WriteLine(@" eg AddNavigationToSpecificationHTMLOutputFiles /rootFolder \\kl-web-001\CustomerTests\IDAM /description ""IDAM Customer Tests""");

                return 1;
            }

            new AddNavigationToHTMLOutput(new FileSystem(), new NavigationHTMLFormatter()).CreateIndexHtmlFiles(GetSetting(args, "rootfolder"), GetSetting(args, "description"));
            return 0;
        }

        private static string GetSetting(string[] args, string settingName)
        {
            string settingValue = args.SkipWhile(a => !a.ToLower().StartsWith("/" + settingName.ToLower())).Skip(1).Take(1).FirstOrDefault();

            return settingValue;
        }

    }
}
