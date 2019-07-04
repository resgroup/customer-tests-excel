using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.Test
{
    public struct CellReference
    {
        public uint Row { get; set; }
        public uint Column { get; set; }
    }

    public class TestTabularPage : ITabularPage
    {
        public readonly Dictionary<CellReference, object> SetCells;

        public string Name { get; set; }

        public TestTabularPage()
        {
            SetCells = new Dictionary<CellReference, object>();
        }

        public CellReference FindCell(object cellValue)
        {
            return SetCells.First(kv => (dynamic)kv.Value == (dynamic)cellValue).Key;
        }

        public uint MaxColumn =>
            SetCells.Keys.Select(cr => cr.Column).Max();

        public uint MaxRow => 
            SetCells.Keys.Select(cr => cr.Row).Max();

        public ITabularCell GetCell(uint row, uint column)
        {
            SetCells.TryGetValue(new CellReference { Row = row, Column = column }, out object cellValue);

            return new TestTabularCell(cellValue);
        }

        public void SetCell(uint row, uint column, object value)
        {
            SetCells.Add(new CellReference { Row = row, Column = column }, value);
        }
    }
}