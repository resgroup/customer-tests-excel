using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelState
    {
        public ITabularPage Worksheet { get; private set; }
        public uint Row { get; private set; }
        public uint Column { get; private set; }

        public ExcelState() { }

        internal void Initialise(ITabularPage worksheet)
        {
            Worksheet = worksheet;
            Row = 1;
            Column = 1;
        }

        public bool RowToColumnIsEmpty(uint column)
        {
            for (uint columnToCheck = 1; columnToCheck <= column; columnToCheck++)
            {
                if (!string.IsNullOrWhiteSpace(Cell(Row, columnToCheck))) return false;
            }

            return true;
        }

        public bool RowToCurrentColumnIsEmpty() =>
            RowToColumnIsEmpty(Column);

        public bool AllColumnsAreEmpty() =>
            RowToColumnIsEmpty(GetLastColumn());

        public bool AnyPrecedingColumnHasAValue() =>
            !RowToColumnIsEmpty(Column - 1);

        public bool AnyFollowingColumnHasAValue(int rowOffset = 0)
        {
            for (uint columnToCheck = Column + 1; columnToCheck <
                GetLastColumn(); columnToCheck++)
            {
                if (!string.IsNullOrWhiteSpace(Cell((uint)(Row + rowOffset), columnToCheck))) return true;
            }

            return false;
        }

        public void MoveUp(uint by = 1) =>
            Row -= by;

        public void MoveDown(uint by = 1) =>
            Row += by;

        public void MoveDownToToken(string token)
        {
            while (CurrentCell() != token)
            {
                if (Row > GetLastRow()) throw new ExcelToCodeException(string.Format("Cannot find token {0} in column {1}, reached last row ({2})", token, Column, Row));
                MoveDown();
            }
        }

        public void MoveRight(uint by = 1) =>
            Column += by;

        public void MoveLeft(uint by = 1) =>
            Column -= by;

        public uint? FindTokenInCurrentRowFromCurrentColumn(string token)
        {
            uint columnToCheck = Column;
            while (Cell(Row, columnToCheck) != token)
            {
                if (columnToCheck > GetLastColumn()) return null;
                columnToCheck++;
            }

            return columnToCheck;
        }

        public TidyUp SavePosition()
        {
            uint savedRow = this.Row;
            uint savedColumn = Column;

            return new TidyUp(() => { Row = savedRow; Column = savedColumn; });
        }

        public TidyUp AutoRestoreMoveDown(uint by = 1)
        {
            MoveDown(by);
            return new TidyUp(() => MoveUp(by));
        }

        public TidyUp AutoRestoreMoveRight(uint by = 1)
        {
            MoveRight(by);
            return new TidyUp(() => MoveLeft(by));
        }

        public TidyUp AutoRestoreMoveDownRight(uint downBy = 1, uint rightBy = 1)
        {
            MoveRight(rightBy);
            MoveDown(downBy);

            return new TidyUp(() =>
            {
                MoveLeft(rightBy);
                MoveUp(downBy);
            });
        }

        public uint GetLastRow() =>
            Worksheet.MaxRow;

        public uint GetLastColumn() =>
            Worksheet.MaxColumn + 20; // maxcolumn seems to underreport the amount of columns that there are ...

        public string PeekAbove(uint by = 1) =>
            Cell(Row - by, Column);

        public string PeekBelow(uint by = 1) =>
            Cell(Row + by, Column);

        public string PeekRight(uint by = 1) =>
            Cell(Row, Column + by);

        public string PeekBelowRight(uint belowBy = 1, uint rightBy = 1) =>
            Cell(Row + belowBy, Column + rightBy);

        public string CurrentCell() =>
            Cell(Row, Column);

        public object CurrentCellRaw() =>
            CellRaw(Row, Column);

        public string Cell(uint row, uint column)
        {
            var value = Worksheet.GetCell(row, column).Value;

            return (value == null) ? "" : value.ToString();
        }

        public object CellRaw(uint row, uint column) =>
            Worksheet.GetCell(row, column).Value;

        public string CellReferenceA1Style() =>
            CellReferenceA1Style(Row, Column);

        public string CellReferenceA1Style(uint row, uint column) =>
            $"{ColumnReferenceA1Style(column)}{row}";

        public string ColumnReferenceA1Style()
            => ColumnReferenceA1Style(Column);

        public string ColumnReferenceA1Style(uint column)
        {
            const uint A = 65;
            const uint NUMBER_OF_LETTERS_IN_ALPHABET = 26;
            uint dividend = column;
            string columnName = String.Empty;
            uint modulo;

            // this works because the int representation of all capital letters starts at 65, is continuous and in alphabetical order
            while (dividend > 0)
            {
                modulo = (dividend - 1) % NUMBER_OF_LETTERS_IN_ALPHABET;
                columnName = Convert.ToChar(A + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / NUMBER_OF_LETTERS_IN_ALPHABET;
            }

            return columnName;
        }
    }
}
