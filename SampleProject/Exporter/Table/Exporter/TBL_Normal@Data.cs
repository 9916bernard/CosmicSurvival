using System.IO;
using NPOI.SS.UserModel;


namespace Table.Exporter
{
    partial class TBL_Normal
    {
        static readonly int ROW_FIELDNAME = 0;
        static readonly int COLUMN_COMMENT = 0;

        static public void ReadDataAndMakeTBD(ISheet sheet, Defines dfn)
        {
            IRow rowFieldName = sheet.GetRow(ROW_FIELDNAME);

            for (int i = 0; i <= rowFieldName.LastCellNum; ++i)
            {
                string fieldName = Excel.GetCell(rowFieldName, i);

                if (fieldName == "")
                {
                    continue;
                }

                dfn.SetElementIndex(fieldName, i);
            }

            for (int i = 0; i < dfn.mElements.Count; ++i)
            {
                if (dfn.mElements[i].mIndexInData == -1)
                {
                    Log.E("Define Field not found in [Data] sheet FieldName:{0}", dfn.mElements[i].mFieldName);
                }
            }

            // TBD Client (Include Bot Directory)
            if (Settings.Data_Client.Count > 0)
            {
                MakeTBDFile(sheet, dfn, Settings.Data_Client[0] + "Table\\", false);
            }

            for (int i = 1; i < Settings.Data_Client.Count; ++i)
            {
                Util.MakeFolder(Settings.Data_Client[i] + "Table\\");

                File.Copy(Settings.Data_Client[0] + "Table\\" + dfn.mTableName + Settings.DATA_FILE_EXT, Settings.Data_Client[i] + "Table\\" + dfn.mTableName + Settings.DATA_FILE_EXT, true);
            }

            // TBD Server
            if (Settings.Data_Server.Count > 0)
            {
                MakeTBDFile(sheet, dfn, Settings.Data_Server[0] + "Table\\", true);
            }

            for (int i = 1; i < Settings.Data_Server.Count; ++i)
            {
                Util.MakeFolder(Settings.Data_Server[i] + "Table\\");

                File.Copy(Settings.Data_Server[0] + "Table\\" + dfn.mTableName + ".tbd", Settings.Data_Server[i] + "Table\\" + dfn.mTableName + ".tbd", true);
            }

            // Json Server
            if (Settings.Data_Server_Json.Count > 0)
            {
                MakeJsonFile(sheet, dfn, Settings.Data_Server_Json[0] + "Table\\", true);
            }

            for (int i = 1; i < Settings.Data_Server_Json.Count; ++i)
            {
                Util.MakeFolder(Settings.Data_Server_Json[i] + "Table\\");

                File.Copy(Settings.Data_Server_Json[0] + "Table\\" + dfn.mTableName + ".json", Settings.Data_Server_Json[i] + "Table\\" + dfn.mTableName + ".json", true);
            }
        }

        static public void MakeTBDFile(ISheet sheet, Defines dfn, string outputPath, bool isServer)
        {
            Util.MakeFolder(outputPath);

            string fileExt = (isServer ? ".tbd" : Settings.DATA_FILE_EXT);

            string binaryPath = outputPath + dfn.mTableName + fileExt;

            BinaryWriter fs = new BinaryWriter(new FileStream(binaryPath, FileMode.Create));

            int version = 1;
            int totalRowCount = 0;
            int maxBufferSize = 0;

            fs.Write(version);
            fs.Write(totalRowCount);
            fs.Write(maxBufferSize);


            int dataStartRow = ROW_FIELDNAME + 1;

            for (int i = dataStartRow; i <= sheet.LastRowNum; ++i)
            {
                CurrentState.Row = i;

                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }

                string state = Excel.GetCell(row, COLUMN_COMMENT);
                if (state == "" || Excel.IsComment(state) == true)
                {
                    continue;
                }

                totalRowCount++;

                for (int j = 0; j < dfn.mElements.Count; ++j)
                {
                    Defines.Element elem = dfn.mElements[j];
                    if (elem.mIndexInData < 0)
                    {
                        // error
                        continue;
                    }

                    CurrentState.Field = elem.mFieldName;

                    if (isServer == true && elem.mUsage != Defines.Usage.Server && elem.mUsage != Defines.Usage.Both)
                    {
                        continue;
                    }
                    else if (isServer == false && elem.mUsage != Defines.Usage.Client && elem.mUsage != Defines.Usage.Both)
                    {
                        continue;
                    }

                    string cell = Excel.GetCell(row, elem.mIndexInData);

                    int write_size = DataWriter.WriteData(fs, elem, cell);

                    if (maxBufferSize < write_size)
                        maxBufferSize = write_size;
                }

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
