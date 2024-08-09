using System.IO;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using System.Text.Json;


namespace Table.Exporter
{
    partial class TBL_Normal
    {
        static public void MakeJsonFile(ISheet sheet, Defines dfn, string outputPath, bool isServer)
        {
            Util.MakeFolder(outputPath);

            string fileExt = ".json";

            string jsonFilePath = outputPath + dfn.mTableName + fileExt;

            object jo = null;

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

                Dictionary<object, object> lowo = new Dictionary<object, object>();

                string multikey = "";
                string singlekey = "";

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

                    DataWriter_Json.WriteData(lowo, elem, cell);

                    if (elem.mKeyType == Defines.KeyType.Multi)
                    {
                        multikey = cell.Trim();
                    }
                    else if (elem.mKeyType == Defines.KeyType.Single)
                    {
                        singlekey = cell.Trim();
                    }
                }


                if (multikey != "")
                {
                    if (singlekey != "")
                    {
                        // Dictionary + Dictionary
                        if (jo == null) { jo = new Dictionary<string, Dictionary<string, object>>(); }
                        Dictionary<string, Dictionary<string, object>> jom = (Dictionary<string, Dictionary<string, object>>)jo;
                        Dictionary<string, object> jos = null;
                        if (jom.TryGetValue(multikey, out jos) == false) { jos = new Dictionary<string, object>(); jom.Add(multikey, jos); }
                        jos.Add(singlekey, lowo);
                    }
                    else
                    {
                        // Dictionary + List
                        if (jo == null) { jo = new Dictionary<string, List<object>>(); }
                        Dictionary<string, List<object>> jom = (Dictionary<string, List<object>>)jo;
                        List<object> jos = null;
                        if (jom.TryGetValue(multikey, out jos) == false) { jos = new List<object>(); jom.Add(multikey, jos); }
                        jos.Add(lowo);
                    }
                }
                else if (singlekey != "")
                {
                    // Dictionary
                    if (jo == null) { jo = new Dictionary<string, object>(); }
                    Dictionary<string, object> jom = (Dictionary<string, object>)jo;
                    jom.Add(singlekey, lowo);
                }
                else
                {
                    // List
                    if (jo == null) { jo = new List<object>(); }
                    List<object> jom = (List<object>)jo;
                    jom.Add(lowo);
                }
            }


            string jsonString = JsonSerializer.Serialize(jo, new JsonSerializerOptions() { WriteIndented = true });


            FileStream stream = new FileStream(jsonFilePath, FileMode.Create);

            StreamWriter fs = new StreamWriter(stream);

            fs.Write(jsonString);

            fs.Flush();
            fs.Close();
            fs.Dispose();
        }
    }
}
