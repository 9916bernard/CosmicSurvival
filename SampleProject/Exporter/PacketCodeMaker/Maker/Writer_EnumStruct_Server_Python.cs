using System;
using System.IO;
using System.Collections.Generic;


namespace PacketCodeMaker
{
    public class Writer_EnumStruct_Server_Python
    {
        static public void Write_Enum(Util.PathInfo pathInfo, List<EnumStructInfos> infoList, string nsName)
        {
            string server_directory = pathInfo.mCodePathServerPython + "Types/";

            Util.MakeFolder(server_directory);


            string filepath_server = string.Format("{0}{1}.py", server_directory, nsName);
            StreamWriter w = Util.GetTextFile(filepath_server);


            w.WriteLine("from enum import Enum");
            w.WriteLine("");
            w.WriteLine("");


            for (int i = 0; i < infoList.Count; ++i)
            {
                for (int j = 0; j < infoList[i].mEnumInfos.Count; ++j)
                {
                    Write_Enum(w, infoList[i].mEnumInfos[j], "");
                    w.WriteLine("");
                }
            }


            Util.FlushAndReleaseTextFile(w);
            Console.WriteLine("=== Write {0}", filepath_server);
        }

        static private void Write_Enum(StreamWriter w, EnumStructInfos.EnumInfo einfo, string tab)
        {
            w.WriteLine("{0}class {1}(Enum):", tab, einfo.mName);

            for (int i = 0; i < einfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, int, string> tup = einfo.mValueTypeKV[i];
                
                if (Util.IsComment(tup.Item1) == true)
                {
                    if (i != 0)
                    {
                        w.WriteLine("");
                    }
                    w.WriteLine("{0}# {1}", tab + Util.Tab, tup.Item1.Replace("//", ""));
                }
                else
                {
                    string comment = (tup.Item3 != "" ? " # " + tup.Item3.Replace("//", "") : "");
                    w.WriteLine("{0}{1} = {2} {3}", tab + Util.Tab, tup.Item1, tup.Item2, comment);
                }
            }
        }
    }
}
