using System.IO;
using System.Collections.Generic;
using NPOI.SS.UserModel;


namespace Table.Exporter
{
    partial class TBL_Normal
    {
        static public List<string> ExcelList = new List<string>();
        static public List<Defines> DefineList = new List<Defines>();

        static public void Clear()
        {
            ExcelList.Clear();
            DefineList.Clear();
        }

        static public void Make(bool exportCode)
        {
            for (int i = 0; i < ExcelList.Count; ++i)
            {
                string tableName = Path.GetFileNameWithoutExtension(ExcelList[i]).Trim();

                CurrentState.Clear();
                CurrentState.TableName = tableName;

                MakeOneFile(ExcelList[i], tableName, exportCode);
            }
        }

        static public void MakeWithList(List<string> excelList, bool exportCode)
        {
            for (int i = 0; i < excelList.Count; ++i)
            {
                string tableName = Path.GetFileNameWithoutExtension(excelList[i]).Trim();

                CurrentState.Clear();
                CurrentState.TableName = tableName;

                MakeOneFile(excelList[i], tableName, exportCode);
            }
        }

        static public void MakeOneFile(string filename, string tableName, bool exportCode)
        {
            Excel excel = new Excel(filename);


            Defines dfn = new Defines() { mTableName = tableName };

            DefineList.Add(dfn);


            // DEFINE
            ISheet sheetDefine = excel.FindSheet("Define_Column");

            if (sheetDefine == null)
            {
                Log.E("[Define_Column] sheet not found");
            }
            else
            {
                CurrentState.SheetName = sheetDefine.SheetName;

                ReadDefine(sheetDefine, dfn);
            }


            // DATA
            ISheet sheetData = excel.FindSheet("Data");

            if (sheetData == null)
            {
                Log.E("[Data] sheet not found");
            }
            else
            {
                CurrentState.SheetName = sheetData.SheetName;

                ReadDataAndMakeTBD(sheetData, dfn);
            }


            // Write Code
            if (exportCode == true)
            {
                WriteCodes(dfn);
            }


            excel.Close();
        }

        static public void WriteCodes(Defines dfn)
        {
            // Code Client
            //CurrentState.SheetName = "== Write Data Header (Client) ==";

            //if (Settings.Code_Client.Count > 0)
            //{
            //    WriteClient(dfn, Settings.Code_Client[0]);
            //}

            //for (int i = 1; i < Settings.Code_Client.Count; ++i)
            //{
            //    Util.MakeFolder(Settings.Code_Client[i] + Settings.DATA_DIR);

            //    File.Copy(string.Format("{0}{1}TB{2}.h", Settings.Code_Client[0], Settings.DATA_DIR, dfn.mTableName),
            //              string.Format("{0}{1}TB{2}.h", Settings.Code_Client[i], Settings.DATA_DIR, dfn.mTableName), true);
            //}


            // Code Client For Unity
            CurrentState.SheetName = "== Write Data Header (Client) ==";

            if (Settings.Code_Client.Count > 0)
            {
                WriteClient_Unity(dfn, Settings.Code_Client[0]);
            }

            for (int i = 1; i < Settings.Code_Client.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Client[i] + Settings.DATA_DIR);

                File.Copy(string.Format("{0}{1}TB{2}.cs", Settings.Code_Client[0], Settings.DATA_DIR, dfn.mTableName),
                          string.Format("{0}{1}TB{2}.cs", Settings.Code_Client[i], Settings.DATA_DIR, dfn.mTableName), true);
            }


            // Code Server
            CurrentState.SheetName = "== Write Data Header (Server) ==";

            if (Settings.Code_Server.Count > 0)
            {
                WriteServer(dfn, Settings.Code_Server[0]);
            }

            for (int i = 1; i < Settings.Code_Server.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Server[i] + Settings.DATA_DIR);

                File.Copy(string.Format("{0}{1}TB{2}.h", Settings.Code_Server[0], Settings.DATA_DIR, dfn.mTableName),
                          string.Format("{0}{1}TB{2}.h", Settings.Code_Server[i], Settings.DATA_DIR, dfn.mTableName), true);
            }


            // Code Bot
            CurrentState.SheetName = "== Write Data Header (Bot) ==";

            if (Settings.Code_Bot.Count > 0)
            {
                WriteBot(dfn, Settings.Code_Bot[0]);
            }

            for (int i = 1; i < Settings.Code_Bot.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Bot[i] + Settings.DATA_DIR);

                File.Copy(string.Format("{0}{1}TB{2}.h", Settings.Code_Bot[0], Settings.DATA_DIR, dfn.mTableName),
                          string.Format("{0}{1}TB{2}.h", Settings.Code_Bot[i], Settings.DATA_DIR, dfn.mTableName), true);
            }
        }
    }
}
