using System;
using System.IO;
using System.Collections.Generic;


namespace PacketCodeMaker
{
    public class Writer_EnumStruct_Client_Unity
    {
        static public void Write_Enum(Util.PathInfo pathInfo, List<EnumStructInfos> infoList, string nsName)
        {
            string client_directory = pathInfo.mCodePathClient + "Types/";

            Util.MakeFolder(client_directory);


            string filepath_client = string.Format("{0}{1}.cs", client_directory, nsName);
            StreamWriter w = Util.GetTextFile(filepath_client);


            w.WriteLine("");

            if (nsName == "GE")
            {
                w.WriteLine("// [Global Enum]");
            }
            else if (nsName == "RE")
            {
                w.WriteLine("// [Result Enum]");
            }
            else
            {
                w.WriteLine("// [Enum]");
            }

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
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_Enum(StreamWriter w, EnumStructInfos.EnumInfo einfo, string tab)
        {
            w.WriteLine("{0}public enum {1} : {2}", tab, einfo.mName, Util.ConvertTypeUnity(einfo.mType));
            w.WriteLine("{0}{{", tab);

            for (int i = 0; i < einfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, int, string> tup = einfo.mValueTypeKV[i];

                if (Util.IsComment(tup.Item1) == true)
                {
                    if (i != 0)
                    {
                        w.WriteLine("");
                    }
                    w.WriteLine("{0}{1}", tab + Util.Tab, tup.Item1);
                }
                else
                {
                    string comment = (tup.Item3 != "" ? " // " + tup.Item3 : "");
                    w.WriteLine("{0}{1} = {2},{3}", tab + Util.Tab, tup.Item1, tup.Item2, comment);
                }
            }

            w.WriteLine("{0}}}", tab);
        }



        static public void Write_Struct(Util.PathInfo pathInfo, List<EnumStructInfos> infoList, string nsName)
        {
            string client_directory = pathInfo.mCodePathClient + "Types/";

            Util.MakeFolder(client_directory);


            string filepath_client = string.Format("{0}{1}.cs", client_directory, nsName);
            StreamWriter w = Util.GetTextFile(filepath_client);


            w.WriteLine("");
            w.WriteLine("// [Global Struct]");
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("using System;");
            w.WriteLine("using System.Runtime.InteropServices;");
            w.WriteLine("");
            w.WriteLine("");

            for (int i = 0; i < infoList.Count; ++i)
            {
                for (int j = 0; j < infoList[i].mStructInfos.Count; ++j)
                {
                    Write_Struct(w, infoList[i].mStructInfos[j], "");
                    w.WriteLine("");
                }
            }

            Util.FlushAndReleaseTextFile(w);
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_Struct(StreamWriter w, EnumStructInfos.StructInfo sinfo, string tab)
        {
            w.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]");
            w.WriteLine("[Serializable]");
            w.WriteLine("{0}public struct {1}", tab, sinfo.mName);
            w.WriteLine("{0}{{", tab);

            for (int i = 0; i < sinfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, string, int> tup = sinfo.mValueTypeKV[i];

                if (tup.Item2 == "string")
                {
                    if (tup.Item3 <= 0)
                    {
                        // error
                    }
                    else
                    {
                        w.WriteLine("{0}[MarshalAs(UnmanagedType.ByValTStr, SizeConst = {1})]", Util.Tab, tup.Item3);
                        w.WriteLine("{0}public string {1};", tab + Util.Tab, tup.Item1);
                    }
                }
                else
                {
                    if (tup.Item3 <= 0)
                    {
                        w.WriteLine("{0}public {1} {2};", tab + Util.Tab, Util.ConvertTypeUnity(tup.Item2), tup.Item1);
                    }
                    else
                    {
                        w.WriteLine("{0}[MarshalAs(UnmanagedType.ByValArray, SizeConst = {1})]", Util.Tab, tup.Item3);
                        w.WriteLine("{0}public {1}[] {2};", tab + Util.Tab, Util.ConvertTypeUnity(tup.Item2), tup.Item1);
                    }
                }
            }

            w.WriteLine("{0}}}", tab);
        }
    }
}
