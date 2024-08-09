using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace Table.Exporter
{
    class Excel
    {
        FileStream Stream = null;

        IWorkbook Book = null;


        public Excel(string filename)
        {
            Stream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            Book = new XSSFWorkbook(Stream);
        }

        public void Close()
        {
            if (Book != null)
            {
                Book.Close();
            }

            if (Stream != null)
            {
                Stream.Close();
                Stream.Dispose();
            }
        }

        public ISheet FindSheet(string sheetName)
        {
            for (int i = 0; i < Book.NumberOfSheets; ++i)
            {
                ISheet sheet = Book.GetSheetAt(i);

                if (sheet.SheetName.ToLower() == sheetName.ToLower())
                {
                    return sheet;
                }
            }

            return null;
        }

        static public string GetCell(IRow row, int columnIdx)
        {
            ICell cell = row.GetCell(columnIdx);

            if (cell == null)
            {
                return "";
            }

            if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue.Trim();
            }
            else if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }
            else if (cell.CellType == CellType.Formula)
            {
                if (cell.CachedFormulaResultType == CellType.Numeric)
                {
                    return cell.NumericCellValue.ToString();
                }
                else if (cell.CachedFormulaResultType == CellType.String)
                {
                    return cell.StringCellValue;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        static public bool IsComment(string line)
        {
            return (line.Length >= 2 && line[0] == '/' && line[1] == '/');
        }
    }
}
