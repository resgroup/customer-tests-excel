using System.Collections.Generic;
using System.Xml.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class GeneratedCsharpProject
    {
        //public XDocument CsprojFile { get; set; }
        public List<CsharpProjectFileToSave> Files { get; set; }

        public GeneratedCsharpProject()
        {
            Files = new List<CsharpProjectFileToSave>();
        }
    }
}