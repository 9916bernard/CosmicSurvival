using System.IO;
using System.Text.Json;
using System.Collections.Generic;


namespace Table.Exporter
{
    partial class TBL_Config
    {
        static public void MakeJsonFile(Defines dfn, string outputPath, bool isServer)
        {
            Util.MakeFolder(outputPath);

            string fileExt = ".json";

            string jsonFilePath = outputPath + dfn.mTableName + fileExt;

            Dictionary<object, object> jo = new Dictionary<object, object>();

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

                DataWriter_Json.WriteData(jo, elem, elem.mValue.Trim());
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

//JsonDocument jdom = JsonDocument.Parse(jsonString);
//JsonElement jroot = jdom.RootElement;

//JsonElement jaddr = jroot.GetProperty("SPWeightByWaveCount");

//JsonElement jastruct = jroot.GetProperty("TestStruct");

//JsonElement jvalue = jastruct.GetProperty("Value");

//JsonElement jstr = jroot.GetProperty("TestStringArray");

//string sstr1 = jstr[0].GetString();
//string sstr2 = jstr[1].GetString();

//int abc = 0;