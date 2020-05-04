using System.IO;

namespace CustomerTestsExcel.ExcelToCode
{
    public struct ExcelFileIo
    {
        public Stream Stream { get; set; }
        public ITabularBook ExcelWorkbook { get; set; }
    }
}
