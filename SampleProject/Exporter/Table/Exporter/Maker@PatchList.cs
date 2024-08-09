using System;
using System.IO;
using System.Collections.Generic;


namespace Table.Exporter
{
    partial class Maker
    {
        public static void WritePatchList()
        {
            if (Settings.Patch_URL == "")
                return;

            Util.MakeFolder(Settings.Patch_TXT);

            string filepath = string.Format("{0}{1}", Settings.Patch_TXT, Settings.PATCH_FILE);
            StreamWriter w = Util.GetNewTextFile(filepath);


            w.WriteLine("// Table List");

            // Config
            for (int i = 0; i < TBL_Config.ExcelList.Count; ++i)
            {
                string tableName = Path.GetFileNameWithoutExtension(TBL_Config.ExcelList[i]).Trim();

                w.WriteLine("Table|{0}|1|{1}/Table/{2}.tbd", tableName, Settings.Patch_URL, tableName);
            }

            // Normal
            for (int i = 0; i < TBL_Normal.ExcelList.Count; ++i)
            {
                string tableName = Path.GetFileNameWithoutExtension(TBL_Normal.ExcelList[i]).Trim();

                w.WriteLine("Table|{0}|1|{1}/Table/{2}.tbd", tableName, Settings.Patch_URL, tableName);
            }

            w.WriteLine("");

            w.WriteLine("// Level List");

            // Level Folder
            if (Settings.Data_Client.Count > 0)
            {
                string[] levelfiles = System.IO.Directory.GetFiles(Settings.Data_Client[0] + "Level\\", "*.spd");

                for (int i = 0; i < levelfiles.Length; ++i)
                {
                    string levelname = Path.GetFileNameWithoutExtension(levelfiles[i]).Trim();

                    w.WriteLine("Level|{0}|1|{1}/Level/{2}.spd", levelname, Settings.Patch_URL, levelname);
                }
            }

            w.WriteLine("");

            w.WriteLine("// Sceneario List");

            // Scenario Folder
            if (Settings.Data_Client.Count > 0)
            {
                string[] scenariofiles = System.IO.Directory.GetFiles(Settings.Data_Client[0] + "Scenario\\", "*.scn");

                for (int i = 0; i < scenariofiles.Length; ++i)
                {
                    string scenarioname = Path.GetFileNameWithoutExtension(scenariofiles[i]).Trim();

                    w.WriteLine("Scenario|{0}|1|{1}/Scenario/{2}.scn", scenarioname, Settings.Patch_URL, scenarioname);
                }
            }

            Util.FlushAndReleaseTextFile(w);
        }
    }
}
