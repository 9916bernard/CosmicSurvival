using System;
using System.IO;
using System.Collections.Generic;


namespace PacketCodeMaker
{
    public class Writer_EnumStruct_Client
    {
        static public void Write_Enum(Util.PathInfo pathInfo, List<EnumStructInfos> infoList, string nsName)
        {
            string client_directory = pathInfo.mCodePathClient + "Types/";

            Util.MakeFolder(client_directory);


            string filepath_client = string.Format("{0}{1}.h", client_directory, nsName);
            StreamWriter w = Util.GetTextFile(filepath_client);


            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("");
            //w.WriteLine("namespace {0}", nsName);
            //w.WriteLine("{");

            for (int i = 0; i < infoList.Count; ++i)
            {
                for (int j = 0; j < infoList[i].mEnumInfos.Count; ++j)
                {
                    //Write_Enum(w, infoList[i].mEnumInfos[j], Util.Tab);
                    Write_Enum(w, infoList[i].mEnumInfos[j], "");
                    w.WriteLine("");
                }
            }

            //w.WriteLine("}");

            Util.FlushAndReleaseTextFile(w);
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_Enum(StreamWriter w, EnumStructInfos.EnumInfo einfo, string tab)
        {
            w.WriteLine("{0}UENUM()", tab);
            w.WriteLine("{0}enum class {1} : {2}", tab, einfo.mName, einfo.mType);
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

            w.WriteLine("{0}}};", tab);
        }



        static public void Write_Struct(Util.PathInfo pathInfo, List<EnumStructInfos> infoList, string nsName)
        {
            string client_directory = pathInfo.mCodePathClient + "Types/";

            Util.MakeFolder(client_directory);


            string filepath_client = string.Format("{0}{1}.h", client_directory, nsName);
            StreamWriter w = Util.GetTextFile(filepath_client);


            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("#include \"SE.h\"");
            w.WriteLine("#include \"GE.h\"");
            w.WriteLine("#include \"{0}.generated.h\"", nsName);
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("#pragma pack(1)");
            w.WriteLine("");
            //w.WriteLine("namespace {0}", nsName);
            //w.WriteLine("{");

            for (int i = 0; i < infoList.Count; ++i)
            {
                for (int j = 0; j < infoList[i].mStructInfos.Count; ++j)
                {
                    //Write_Struct(w, infoList[i].mStructInfos[j], Util.Tab);
                    Write_Struct(w, infoList[i].mStructInfos[j], "");
                    w.WriteLine("");
                }
            }

            //w.WriteLine("}");

            w.WriteLine("");

            w.WriteLine("#pragma pack()");

            Util.FlushAndReleaseTextFile(w);
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_Struct(StreamWriter w, EnumStructInfos.StructInfo sinfo, string tab)
        {
            w.WriteLine("{0}USTRUCT()", tab);
            w.WriteLine("{0}struct {1}", tab, sinfo.mName);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}GENERATED_USTRUCT_BODY()", tab + Util.Tab);

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
                        w.WriteLine("{0}UPROPERTY() uint16 {1}[{2}];", tab + Util.Tab, tup.Item1, tup.Item3);
                    }
                }
                else
                {
                    if (tup.Item3 <= 0)
                    {
                        w.WriteLine("{0}UPROPERTY() {1} {2};", tab + Util.Tab, tup.Item2, tup.Item1);
                    }
                    else
                    {
                        w.WriteLine("{0}UPROPERTY() {1} {2}[{3}];", tab + Util.Tab, tup.Item2, tup.Item1, tup.Item3);
                    }
                }
            }

            for (int i = 0; i < sinfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, string, int> tup = sinfo.mValueTypeKV[i];

                if (tup.Item2 == "string")
                {
                    w.WriteLine("{0}FString Get{1}() {{ return FString(TCHAR_TO_UTF16({2})); }}", tab + Util.Tab, tup.Item1, tup.Item1);
                }
            }

            w.WriteLine("{0}}};", tab);
        }
    }
}
