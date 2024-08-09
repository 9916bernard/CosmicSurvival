using System.IO;
using System.Collections.Generic;
using NPOI.SS.UserModel;


namespace Table.Exporter
{
    partial class TBL_System
    {
        static public List<string> ExcelList = new List<string>();

        static public void Clear()
        {
            ExcelList.Clear();
        }

        static public void Make(bool exportCode)
        {
            for (int i = 0; i < ExcelList.Count; ++i)
            {
                string tableName = Path.GetFileNameWithoutExtension(ExcelList[i]).Trim();

                if (tableName.ToLower() == "system_enum")
                {
                    CurrentState.Clear();
                    CurrentState.TableName = tableName;

                    ReadEnum(ExcelList[i]);
                }
            }

            if (exportCode == true)
            {
                WriteEnum();

                WriteStruct();
            }
        }

        static public void ReadEnum(string filename)
        {
            Excel excel = new Excel(filename);


            // ENUM
            ISheet sheetEnum = excel.FindSheet("Define_enum");

            if (sheetEnum == null)
            {
                Log.E("[Define_enum] sheet not found in [System_Enum]");
            }
            else
            {
                CurrentState.SheetName = sheetEnum.SheetName;

                ReadEnum(sheetEnum, false);
            }


            // JSON2
            //ISheet sheetJson2 = excel.FindSheet("Define_json2");

            //if (sheetJson2 == null)
            //{
            //    Log.E("[Define_json2] sheet not found in [System_Enum]");
            //}
            //else
            //{
            //    CurrentState.SheetName = sheetJson2.SheetName;

            //    ReadEnum(sheetJson2, true);
            //}


            // STRUCT
            ISheet sheetStruct = excel.FindSheet("Define_struct");

            if (sheetStruct == null)
            {
                Log.E("[Define_struct] sheet not found in [System_Enum]");
            }
            else
            {
                CurrentState.SheetName = sheetStruct.SheetName;

                ReadStruct(sheetStruct);
            }


            excel.Close();
        }
    }
}
