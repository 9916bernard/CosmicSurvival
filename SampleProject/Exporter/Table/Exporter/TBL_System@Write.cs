using System;
using System.IO;


namespace Table.Exporter
{
    partial class TBL_System
    {
        //===============================================================================================
        // ENUM 시작
        //===============================================================================================
        static public void WriteEnum()
        {
            // Client
            //if (Settings.Code_Client.Count > 0)
            //{
            //    WriteEnum_Client(Settings.Code_Client[0]);
            //}

            //for (int i = 1; i < Settings.Code_Client.Count; ++i)
            //{
            //    Util.MakeFolder(Settings.Code_Client[i] + Settings.ENUM_DIR);

            //    File.Copy(Settings.Code_Client[0] + Settings.ENUM_FILE, Settings.Code_Client[i] + Settings.ENUM_FILE, true);
            //}

            // Client For Unity
            if (Settings.Code_Client.Count > 0)
            {
                WriteEnum_Client_Unity(Settings.Code_Client[0]);
            }

            for (int i = 1; i < Settings.Code_Client.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Client[i] + Settings.ENUM_DIR);

                File.Copy(Settings.Code_Client[0] + Settings.ENUM_FILE_UNITY, Settings.Code_Client[i] + Settings.ENUM_FILE_UNITY, true);
            }


            // Server
            if (Settings.Code_Server.Count > 0)
            {
                WriteEnum_Server(Settings.Code_Server[0]);
            }

            for (int i = 1; i < Settings.Code_Server.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Server[i] + Settings.ENUM_DIR);

                File.Copy(Settings.Code_Server[0] + Settings.ENUM_FILE, Settings.Code_Server[i] + Settings.ENUM_FILE, true);
            }

            // Server For Python
            if (Settings.Code_Server_Python.Count > 0)
            {
                WriteEnum_Server_Python(Settings.Code_Server_Python[0]);
            }

            for (int i = 1; i < Settings.Code_Server_Python.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Server_Python[i] + Settings.ENUM_DIR);

                File.Copy(Settings.Code_Server_Python[0] + Settings.ENUM_FILE, Settings.Code_Server_Python[i] + Settings.ENUM_FILE, true);
            }

            // Bot
            if (Settings.Code_Bot.Count > 0)
            {
                // 서버와 같은 파일을 씀
                WriteEnum_Server(Settings.Code_Bot[0]);
            }

            for (int i = 1; i < Settings.Code_Bot.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Bot[i] + Settings.ENUM_DIR);

                File.Copy(Settings.Code_Bot[0] + Settings.ENUM_FILE, Settings.Code_Bot[i] + Settings.ENUM_FILE, true);
            }
        }

        static public void WriteEnum_Client(string dir)
        {
            Util.MakeFolder(dir + Settings.ENUM_DIR);
            StreamWriter w = Util.GetNewTextFile(dir + Settings.ENUM_FILE);

            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("");

            for (int i = 0; i < Enums.InfoList.Count; ++i)
            {
                WriteEnum_Client_Info(w, Enums.InfoList[i], "");
                w.WriteLine("");
            }

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteEnum_Client_Info(StreamWriter w, Enums.Info einfo, string tab)
        {
            if (einfo.mType == "uint8")
                w.WriteLine("{0}UENUM(BlueprintType)", tab);
            else
                w.WriteLine("{0}UENUM()", tab);
            w.WriteLine("{0}enum class {1} : {2}", tab, einfo.mName, einfo.mType);
            w.WriteLine("{0}{{", tab);

            int maxLen = 0;
            string space = "";

            for (int i = 0; i < einfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, int, string> tup = einfo.mValueTypeKV[i];

                if (tup.Item1.Length > maxLen)
                    maxLen = tup.Item1.Length;
            }

            for (int i = 0; i < einfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, int, string> tup = einfo.mValueTypeKV[i];
                space = "";
                for (int j = 0; j < maxLen - tup.Item1.Length; ++j)
                    space += " ";

                string display = string.Format("UMETA(DisplayName = \"{0}\")", tup.Item1);

                string comment = (tup.Item3 != "" ? " // " + tup.Item3 : "");
                if (Excel.IsComment(tup.Item3) == true)
                {
                    comment = (tup.Item3 != "" ? " " + tup.Item3 : "");
                }

                w.WriteLine("{0}{1}{2} = {3} {4},{5}{6}", tab + Util.Tab, tup.Item1, space, tup.Item2, display, space, comment);
            }

            //space = "";
            //for (int j = 0; j < maxLen - 3 + 5; ++j)
            //    space += " ";
            //w.WriteLine("{0}{1}{2}UMETA(Hidden)", tab + Util.Tab, "MAX", space);

            w.WriteLine("{0}}};", tab);
        }

        static public void WriteEnum_Client_Unity(string dir)
        {
            Util.MakeFolder(dir + Settings.ENUM_DIR);
            StreamWriter w = Util.GetNewTextFile(dir + Settings.ENUM_FILE_UNITY);

            w.WriteLine("");
            w.WriteLine("// [System Enum]");
            w.WriteLine("");
            w.WriteLine("");

            for (int i = 0; i < Enums.InfoList.Count; ++i)
            {
                WriteEnum_Client_Info_Unity(w, Enums.InfoList[i], "");
                w.WriteLine("");
            }

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteEnum_Client_Info_Unity(StreamWriter w, Enums.Info einfo, string tab)
        {
            w.WriteLine("{0}public enum {1} : {2}", tab, einfo.mName, Util.ConvertTypeUnity(einfo.mType));
            w.WriteLine("{0}{{", tab);

            int maxLen = 0;
            string space = "";

            for (int i = 0; i < einfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, int, string> tup = einfo.mValueTypeKV[i];

                if (tup.Item1.Length > maxLen)
                    maxLen = tup.Item1.Length;
            }

            for (int i = 0; i < einfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, int, string> tup = einfo.mValueTypeKV[i];
                space = "";
                for (int j = 0; j < maxLen - tup.Item1.Length; ++j)
                    space += " ";

                string comment = (tup.Item3 != "" ? " // " + tup.Item3 : "");
                if (Excel.IsComment(tup.Item3) == true)
                {
                    comment = (tup.Item3 != "" ? " " + tup.Item3 : "");
                }

                w.WriteLine("{0}{1}{2} = {3},{4}", tab + Util.Tab, tup.Item1, space, tup.Item2, comment);
            }

            w.WriteLine("{0}}};", tab);
        }

        static public void WriteEnum_Server(string dir)
        {
            Util.MakeFolder(dir + Settings.ENUM_DIR);
            StreamWriter w = Util.GetNewTextFile(dir + Settings.ENUM_FILE);

            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("");

            for (int i = 0; i < Enums.InfoList.Count; ++i)
            {
                WriteEnum_Server_Info(w, Enums.InfoList[i], "");
                w.WriteLine("");
            }

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteEnum_Server_Info(StreamWriter w, Enums.Info einfo, string tab)
        {
            w.WriteLine("{0}enum class {1} : {2}", tab, einfo.mName, einfo.mType);
            w.WriteLine("{0}{{", tab);

            for (int i = 0; i < einfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, int, string> tup = einfo.mValueTypeKV[i];

                string comment = (tup.Item3 != "" ? " // " + tup.Item3 : "");
                if (Excel.IsComment(tup.Item3) == true)
                {
                    comment = (tup.Item3 != "" ? " " + tup.Item3 : "");
                }

                w.WriteLine("{0}{1} = {2},{3}", tab + Util.Tab, tup.Item1, tup.Item2, comment);
            }

            w.WriteLine("{0}}};", tab);
        }

        static public void WriteEnum_Server_Python(string dir)
        {
            Util.MakeFolder(dir + Settings.ENUM_DIR);
            StreamWriter w = Util.GetNewTextFile(dir + Settings.ENUM_FILE_PYTHON);

            w.WriteLine("from enum import Enum");
            w.WriteLine("");
            w.WriteLine("");

            for (int i = 0; i < Enums.InfoList.Count; ++i)
            {
                WriteEnum_Server_Info_Python(w, Enums.InfoList[i], "");
                w.WriteLine("");
            }

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteEnum_Server_Info_Python(StreamWriter w, Enums.Info einfo, string tab)
        {
            w.WriteLine("{0}class {1}(Enum):", tab, einfo.mName);
            //w.WriteLine("{0}{{", tab);

            for (int i = 0; i < einfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, int, string> tup = einfo.mValueTypeKV[i];

                string comment = (tup.Item3 != "" ? " # " + tup.Item3.Replace("//", "") : "");
                if (Excel.IsComment(tup.Item3) == true)
                {
                    comment = (tup.Item3 != "" ? " # " + tup.Item3.Replace("//", "") : "");
                }

                w.WriteLine("{0}{1} = {2} {3}", tab + Util.Tab, tup.Item1, tup.Item2, comment);
            }

            //w.WriteLine("{0}}};", tab);
        }
        //===============================================================================================
        // ENUM 끝
        //===============================================================================================



        //===============================================================================================
        // STRUCT 시작
        //===============================================================================================
        static public void WriteStruct()
        {
            // Client
            //if (Settings.Code_Client.Count > 0)
            //{
            //    WriteStruct_Client(Settings.Code_Client[0]);
            //}

            //for (int i = 1; i < Settings.Code_Client.Count; ++i)
            //{
            //    Util.MakeFolder(Settings.Code_Client[i] + Settings.STRUCT_DIR);

            //    File.Copy(Settings.Code_Client[0] + Settings.STRUCT_FILE, Settings.Code_Client[i] + Settings.STRUCT_FILE, true);
            //}

            // Client For Unity
            if (Settings.Code_Client.Count > 0)
            {
                WriteStruct_Client_Unity(Settings.Code_Client[0]);
            }

            for (int i = 1; i < Settings.Code_Client.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Client[i] + Settings.STRUCT_DIR);

                File.Copy(Settings.Code_Client[0] + Settings.STRUCT_FILE_UNITY, Settings.Code_Client[i] + Settings.STRUCT_FILE_UNITY, true);
            }


            // Server
            if (Settings.Code_Server.Count > 0)
            {
                WriteStruct_Server(Settings.Code_Server[0]);
            }

            for (int i = 1; i < Settings.Code_Server.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Server[i] + Settings.STRUCT_DIR);

                File.Copy(Settings.Code_Server[0] + Settings.STRUCT_FILE, Settings.Code_Server[i] + Settings.STRUCT_FILE, true);
            }


            // Bot
            if (Settings.Code_Bot.Count > 0)
            {
                // 서버와 같은 파일을 씀
                WriteStruct_Server(Settings.Code_Bot[0]);
            }

            for (int i = 1; i < Settings.Code_Bot.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Bot[i] + Settings.STRUCT_DIR);

                File.Copy(Settings.Code_Bot[0] + Settings.STRUCT_FILE, Settings.Code_Bot[i] + Settings.STRUCT_FILE, true);
            }
        }

        static public void WriteStruct_Client(string dir)
        {
            Util.MakeFolder(dir + Settings.STRUCT_DIR);
            StreamWriter w = Util.GetNewTextFile(dir + Settings.STRUCT_FILE);

            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("#include \"SE.h\"");
            w.WriteLine("#include \"SS.generated.h\"");
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("#pragma pack(1)");
            w.WriteLine("");

            for (int i = 0; i < Structs.InfoList.Count; ++i)
            {
                WriteStruct_Client_Info(w, Structs.InfoList[i], "");
                w.WriteLine("");
            }

            w.WriteLine("#pragma pack()");

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteStruct_Client_Info(StreamWriter w, Structs.Info sinfo, string tab)
        {
            w.WriteLine("{0}USTRUCT(BlueprintType)", tab);
            w.WriteLine("{0}struct {1}", tab, sinfo.mName);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}GENERATED_USTRUCT_BODY()", tab + Util.Tab);

            for (int i = 0; i < sinfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, string, int> tup = sinfo.mValueTypeKV[i];

                if (tup.Item2 == "string")
                {
                    w.WriteLine("{0}UPROPERTY() FString {1};", tab + Util.Tab, tup.Item1);
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

            w.WriteLine("{0}}};", tab);
        }

        static public void WriteStruct_Client_Unity(string dir)
        {
            Util.MakeFolder(dir + Settings.STRUCT_DIR);
            StreamWriter w = Util.GetNewTextFile(dir + Settings.STRUCT_FILE_UNITY);

            w.WriteLine("");
            w.WriteLine("// [System Struct]");
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("using System;");
            w.WriteLine("using System.Runtime.InteropServices;");
            w.WriteLine("");
            w.WriteLine("");

            for (int i = 0; i < Structs.InfoList.Count; ++i)
            {
                WriteStruct_Client_Info_Unity(w, Structs.InfoList[i], "");
                w.WriteLine("");
            }

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteStruct_Client_Info_Unity(StreamWriter w, Structs.Info sinfo, string tab)
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
                    w.WriteLine("{0}public string {1};", tab + Util.Tab, tup.Item1);
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

        static public void WriteStruct_Server(string dir)
        {
            Util.MakeFolder(dir + Settings.STRUCT_DIR);
            StreamWriter w = Util.GetNewTextFile(dir + Settings.STRUCT_FILE);

            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("#pragma pack(1)");
            w.WriteLine("");

            for (int i = 0; i < Structs.InfoList.Count; ++i)
            {
                WriteStruct_Server_Info(w, Structs.InfoList[i], "");
                w.WriteLine("");
            }

            w.WriteLine("#pragma pack()");

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteStruct_Server_Info(StreamWriter w, Structs.Info sinfo, string tab)
        {
            w.WriteLine("{0}struct {1}", tab, sinfo.mName);
            w.WriteLine("{0}{{", tab);

            for (int i = 0; i < sinfo.mValueTypeKV.Count; ++i)
            {
                Tuple<string, string, int> tup = sinfo.mValueTypeKV[i];

                if (tup.Item2 == "string")
                {
                    w.WriteLine("{0}std::wstring {1};", tab + Util.Tab, tup.Item1);
                }
                else
                {
                    if (tup.Item3 <= 0)
                    {
                        w.WriteLine("{0}{1} {2};", tab + Util.Tab, tup.Item2, tup.Item1);
                    }
                    else
                    {
                        w.WriteLine("{0}{1} {2}[{3}];", tab + Util.Tab, tup.Item2, tup.Item1, tup.Item3);
                    }
                }
            }

            w.WriteLine("{0}}};", tab);
        }
        //===============================================================================================
        // STRUCT 끝
        //===============================================================================================
    }
}
