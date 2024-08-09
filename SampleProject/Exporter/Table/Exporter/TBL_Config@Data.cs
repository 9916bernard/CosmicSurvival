using System.IO;


namespace Table.Exporter
{
    partial class TBL_Config
    {
        static public void WriteData(Defines dfn)
        {
            // TBD Client (Include Bot Directory)
            if (Settings.Data_Client.Count > 0)
            {
                MakeTBDFile(dfn, Settings.Data_Client[0] + "Table\\", false);
            }

            for (int i = 1; i < Settings.Data_Client.Count; ++i)
            {
                Util.MakeFolder(Settings.Data_Client[i] + "Table\\");

                File.Copy(Settings.Data_Client[0] + "Table\\" + dfn.mTableName + Settings.DATA_FILE_EXT, Settings.Data_Client[i] + "Table\\" + dfn.mTableName + Settings.DATA_FILE_EXT, true);
            }

            // TBD Server
            if (Settings.Data_Server.Count > 0)
            {
                MakeTBDFile(dfn, Settings.Data_Server[0] + "Table\\", true);
            }

            for (int i = 1; i < Settings.Data_Server.Count; ++i)
            {
                Util.MakeFolder(Settings.Data_Server[i] + "Table\\");

                File.Copy(Settings.Data_Server[0] + "Table\\" + dfn.mTableName + ".tbd", Settings.Data_Server[i] + "Table\\" + dfn.mTableName + ".tbd", true);
            }

            // Json Server
            if (Settings.Data_Server_Json.Count > 0)
            {
                MakeJsonFile(dfn, Settings.Data_Server_Json[0] + "Table\\", true);
            }

            for (int i = 1; i < Settings.Data_Server_Json.Count; ++i)
            {
                Util.MakeFolder(Settings.Data_Server_Json[i] + "Table\\");

                File.Copy(Settings.Data_Server_Json[0] + "Table\\" + dfn.mTableName + ".json", Settings.Data_Server_Json[i] + "Table\\" + dfn.mTableName + ".json", true);
            }
        }

        static public void MakeTBDFile(Defines dfn, string outputPath, bool isServer)
        {
            Util.MakeFolder(outputPath);

            CurrentState.Row = -1;

            string fileExt = (isServer ? ".tbd" : Settings.DATA_FILE_EXT);

            string binaryPath = outputPath + dfn.mTableName + fileExt;

            BinaryWriter fs = new BinaryWriter(new FileStream(binaryPath, FileMode.Create));

            int version = 1;
            int totalRowCount = 1; // Config 테이블은 하나의 Row가 고정되어 있는 구조임
            int maxBufferSize = 0;

            fs.Write(version);
            fs.Write(totalRowCount);
            fs.Write(maxBufferSize);


            for (int i = 0; i < dfn.mElements.Count; ++i)
            {
                Defines.Element elem = dfn.mElements[i];

                CurrentState.Field = elem.mFieldName;

                if (isServer == true && elem.mUsage != Defines.Usage.Server && elem.mUsage != Defines.Usage.Both)
                {
                    continue;
                }
                else if (isServer == false && elem.mUsage != Defines.Usage.Client && elem.mUsage != Defines.Usage.Both)
                {
                    continue;
                }

                int write_size = DataWriter.WriteData(fs, elem, elem.mValue.Trim());

                if (maxBufferSize < write_size)
                    maxBufferSize = write_size;
            }


            fs.Seek(0, SeekOrigin.Begin);
            fs.Write(version);
            fs.Write(totalRowCount);
            fs.Write(maxBufferSize);

            fs.Flush();
            fs.Close();
            fs.Dispose();
        }
    }
}
