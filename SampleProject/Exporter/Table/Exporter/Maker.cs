using System.IO;


namespace Table.Exporter
{
    partial class Maker
    {
        static public void Make(bool exportCode)
        {
            try
            {
                // 초기화
                Clear();

                // 엑셀파일 수집
                ClassifyTable();

                // Enum & Struct 수집 from System_Enum
                TBL_System.Make(exportCode);

                // Config Table
                TBL_Config.Make(exportCode);

                // String Table

                // Normal Table
                TBL_Normal.Make(exportCode);

                // Manager
                if (exportCode == true)
                {
                    WriteManager();
                }

            }
            catch (System.Exception e)
            {
                Log.E("[CRASH] {0}\n{1}\n", e.Message, e.StackTrace);
            }
        }

        static public void Clear()
        {
            Enums.Clear();
            Structs.Clear();

            TBL_System.Clear();
            TBL_Config.Clear();
            TBL_String.Clear();
            TBL_Normal.Clear();
        }

        static public void ClassifyTable()
        {
            string[] excelFiles = System.IO.Directory.GetFiles(Settings.Table_Excel, "*.xlsx");

            for (int i = 0; i < excelFiles.Length; ++i)
            {
                string tableName = Path.GetFileNameWithoutExtension(excelFiles[i]).Trim();

                if (tableName == "")
                {
                    Log.N("[skip] unknown file : {0}", tableName);
                    continue;
                }

                if (tableName[0] == '~')
                {
                    Log.N("[skip] temp file : {0}", tableName);
                    continue;
                }

                if (tableName[0] == '_')
                {
                    Log.N("[skip] sameple file : {0}", tableName);
                    continue;
                }

                string[] cmds = tableName.Split('_');

                if (cmds[0].Trim().ToLower() == "system")
                {
                    TBL_System.ExcelList.Add(excelFiles[i]);
                }
                else if (cmds[0].Trim().ToLower() == "config")
                {
                    TBL_Config.ExcelList.Add(excelFiles[i]);
                }
                else if (cmds[0].Trim().ToLower() == "string")
                {
                    //TBL_String.ExcelList.Add(excelFiles[i]);
                    TBL_Normal.ExcelList.Add(excelFiles[i]);
                }
                else
                {
                    TBL_Normal.ExcelList.Add(excelFiles[i]);
                }
            }
        }
    }
}
