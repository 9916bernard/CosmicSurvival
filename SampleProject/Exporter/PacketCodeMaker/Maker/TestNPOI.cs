using System;
using System.IO;
//공통 NPOI
using NPOI;
using NPOI.SS.UserModel;
//표준 xls 버젼 excel시트
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
//확장 xlsx 버젼 excel 시트
using NPOI.XSSF;
using NPOI.XSSF.UserModel;
using System.Runtime.InteropServices;


namespace PacketCodeMaker.Maker
{
    class TestNPOI
    {
        static public void Run(string path_excel)
        {
            path_excel += "\\Game.xlsx";

            FileStream stream = File.Open(path_excel, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            IWorkbook book = CreateWorkbook(Path.GetExtension(path_excel), stream);

            int numsheet = book.NumberOfSheets;

            ISheet sheetDefine = book.GetSheet("Account");

            for (int i = 0; i <= sheetDefine.LastRowNum; ++i)
            {
                IRow row = sheetDefine.GetRow(i);

                if (row == null)
                {
                    continue;
                }

                for (int j = 0; j < row.LastCellNum; ++j)
                {
                    ICell cell = row.GetCell(j);
                }
            }

            book.Close();

            stream.Close();
            stream.Dispose();
        }

        static private IWorkbook CreateWorkbook(string version, FileStream stream)
        {
            //표준 xls 버젼
            if (".xls".Equals(version))
            {
                return new HSSFWorkbook(stream);
            }
            //확장 xlsx 버젼
            else if (".xlsx".Equals(version))
            {
                return new XSSFWorkbook(stream);
            }
            throw new NotSupportedException();
        }
    }
}
