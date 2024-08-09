using System;
using System.IO;


namespace Table.Exporter
{
    partial class TBL_Config
    {
        static public void WriteClient_Unity(Defines dfn, string outputPath)
        {
            string dir = outputPath + Settings.DATA_DIR;

            Util.MakeFolder(dir);


            string filepath = string.Format("{0}TB{1}.cs", dir, dfn.mTableName);
            StreamWriter w = Util.GetNewTextFile(filepath);


            w.WriteLine("using System;");
            w.WriteLine("using System.IO;");
            w.WriteLine("using UnityEngine;");
            w.WriteLine("");
            w.WriteLine("public class UTB{0}", dfn.mTableName);
            w.WriteLine("{");

            WriteClient_Unity_VariableDefinition(w, dfn, Util.Tab);
            w.WriteLine("");


            w.WriteLine("{0}void Read(BinaryReader fs, byte[] buffer)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_Unity_ReadFunc(w, dfn, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}public void Load(byte[] buffer, bool InForceResources)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_Unity_LoadFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}bool LoadFromStream(BinaryReader fs, byte[] buffer)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_Unity_LoadFromStreamFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}public bool LoadFromResources(byte[] buffer)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_Unity_LoadFromResourcesFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}public bool LoadFromFile(byte[] buffer)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_Unity_LoadFromFileFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}public int GetLastVersion(out int OutStorageType)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_Unity_GetLastVersionFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);


            w.WriteLine("}");


            Util.FlushAndReleaseTextFile(w);
        }

        static private void WriteClient_Unity_VariableDefinition(StreamWriter w, Defines dfn, string tab)
        {
            for (int i = 0; i < dfn.mElements.Count; ++i)
            {
                Defines.Element elem = dfn.mElements[i];

                if (elem.mUsage != Defines.Usage.Client && elem.mUsage != Defines.Usage.Both)
                {
                    continue;
                }

                if (elem.mArray == true)
                {
                    if (elem.mDataType == "struct" || elem.mDataType == "enum")
                    {
                        w.WriteLine("{0}public {1} {2} = new {3}[0];", tab, elem.mDataTypeDef_C_Unity, elem.mFieldName, elem.mValueType);
                    }
                    else
                    {
                        w.WriteLine("{0}public {1} {2} = new {3}[0];", tab, elem.mDataTypeDef_C_Unity, elem.mFieldName, Util.ConvertTypeUnity(elem.mDataType));
                    }
                }
                else
                {
                    w.WriteLine("{0}public {1} {2};", tab, elem.mDataTypeDef_C_Unity, elem.mFieldName);
                }
            }
        }


        static private bool define_cnt = false;

        static private void check_define_cnt(StreamWriter w, string tab) { if (define_cnt == false) { define_cnt = true; w.WriteLine("{0}ushort cnt = 0;", tab); } }

        static private void WriteClient_Unity_ReadFunc(StreamWriter w, Defines dfn, string tab)
        {
            tab += Util.Tab;

            define_cnt = false;

            for (int i = 0; i < dfn.mElements.Count; ++i)
            {
                Defines.Element elem = dfn.mElements[i];

                if (elem.mUsage != Defines.Usage.Client && elem.mUsage != Defines.Usage.Both)
                {
                    continue;
                }

                switch (elem.mDataType)
                {
                    case "string": WriteClient_Unity_ReadFunc_String(w, elem, tab); break;
                    case "struct": WriteClient_Unity_ReadFunc_Struct(w, elem, tab); break;
                    case "enum": WriteClient_Unity_ReadFunc_Enum(w, elem, tab); break;
                    case "vector2": WriteClient_Unity_ReadFunc_Vector2(w, elem, tab); break;
                    case "vector3": WriteClient_Unity_ReadFunc_Vector3(w, elem, tab); break;
                    case "color": WriteClient_Unity_ReadFunc_Color(w, elem, tab); break;
                    default: WriteClient_Unity_ReadFunc_Default(w, elem, tab); break;
                }
            }
        }

        static private void WriteClient_Unity_ReadFunc_String(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                check_define_cnt(w, tab);
                w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1} = new string[cnt];", tab, elem.mFieldName);
                w.WriteLine("{0}for (int i = 0; i < cnt; ++i) {{ ushort snt = fs.ReadUInt16(); if (snt > 0) {{ fs.Read(buffer, 0, snt * 2); {1}[i] = System.Text.Encoding.Unicode.GetString(buffer, 0, snt * 2); }}}}}}", tab + Util.Tab, elem.mFieldName);
            }
            else
            {
                check_define_cnt(w, tab);
                w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);
                w.WriteLine("{0}if (cnt > 0) {{ fs.Read(buffer, 0, cnt * 2); {1} = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }}" , tab, elem.mFieldName);
            }
        }

        static private void WriteClient_Unity_ReadFunc_Struct(StreamWriter w, Defines.Element elem, string tab)
        {
            Structs.Info st = Structs.Get(elem.mValueType);

            if (elem.mArray == true)
            {
                check_define_cnt(w, tab);
                w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1} = new {2}[cnt]; for (int i = 0; i < cnt; ++i) {{", tab, elem.mFieldName, elem.mValueType);

                bool define_snt = false;

                for (int i = 0; i < st.mValueTypeKV.Count; ++i)
                {
                    Tuple<string, string, int> tup = st.mValueTypeKV[i];
                    string val = string.Format("{0}[i].{1}", elem.mFieldName, tup.Item1);

                    if (tup.Item2 == "string")
                    {
                        if (define_snt == false)
                        {
                            define_snt = true;
                            w.WriteLine("{0}ushort snt = fs.ReadUInt16();", tab + Util.Tab);
                        }
                        else
                        {
                            w.WriteLine("{0}snt = fs.ReadUInt16();", tab + Util.Tab);
                        }
                        w.WriteLine("{0}if (snt > 0) {{ fs.Read(buffer, 0, snt * 2); {1} = System.Text.Encoding.Unicode.GetString(buffer, 0, snt * 2); }}", tab + Util.Tab, val);
                    }
                    else
                    {
                        string readFuncName = Util.GetBinaryReaderFuncNameUnity(tup.Item2);

                        if (readFuncName != "")
                        {
                            w.WriteLine("{0}{1} = fs.{2}();", tab + Util.Tab, val, readFuncName);
                        }
                        else
                        {
                            // enum 인지 아라보자
                            Enums.Info enumInfo = Enums.Get(tup.Item2);

                            if (enumInfo != null)
                            {
                                string strtype = Util.ConvertTypeUnity(enumInfo.mType);
                                string readfunc = Util.GetBinaryReaderFuncNameUnity(enumInfo.mType);
                                string val_enum = string.Format("val_{0}_{1}", elem.mFieldName, tup.Item1);
                                w.WriteLine("{0}{1} {2} = fs.{3}(); {4} = ({5}){6};", tab + Util.Tab, strtype, val_enum, readfunc, val, tup.Item2, val_enum);
                            }
                            else
                            {
                                Log.E("[Write Struct] DataType not found : {0}.{1}", elem.mFieldName, tup.Item2);
                            }
                        }
                    }
                }

                w.WriteLine("{0}}}}}", tab);
            }
            else
            {
                for (int i = 0; i < st.mValueTypeKV.Count; ++i)
                {
                    Tuple<string, string, int> tup = st.mValueTypeKV[i];
                    string val = string.Format("{0}.{1}", elem.mFieldName, tup.Item1);

                    if (tup.Item2 == "string")
                    {
                        check_define_cnt(w, tab);
                        w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);
                        w.WriteLine("{0}if (cnt > 0) {{ fs.Read(buffer, 0, cnt * 2); {1} = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }}", tab, val);
                    }
                    else
                    {
                        string readFuncName = Util.GetBinaryReaderFuncNameUnity(tup.Item2);

                        if (readFuncName != "")
                        {
                            w.WriteLine("{0}{1} = fs.{2}();", tab, val, readFuncName);
                        }
                        else
                        {
                            // enum 인지 아라보자
                            Enums.Info enumInfo = Enums.Get(tup.Item2);

                            if (enumInfo != null)
                            {
                                string strtype = Util.ConvertTypeUnity(enumInfo.mType);
                                string readfunc = Util.GetBinaryReaderFuncNameUnity(enumInfo.mType);
                                string val_enum = string.Format("val_{0}_{1}", elem.mFieldName, tup.Item1);
                                w.WriteLine("{0}{1} {2} = fs.{3}(); {4} = ({5}){6};" , tab, strtype, val_enum, readfunc, val, tup.Item2, val_enum);
                            }
                            else
                            {
                                Log.E("[Write Struct] DataType not found : {0}.{1}", elem.mFieldName, tup.Item2);
                            }
                        }
                    }
                }
            }
        }

        static private void WriteClient_Unity_ReadFunc_Enum(StreamWriter w, Defines.Element elem, string tab)
        {
            Enums.Info enumInfo = Enums.Get(elem.mValueType);

            if (enumInfo == null)
            {
                Log.E("[Write Enum] DataType not found : {0}.{1}", elem.mFieldName, elem.mValueType);
                return;
            }

            string strtype = Util.ConvertTypeUnity(enumInfo.mType);
            string readfunc = Util.GetBinaryReaderFuncNameUnity(enumInfo.mType);
            string val_enum = string.Format("val_{0}", elem.mFieldName);

            if (elem.mArray == true)
            {
                check_define_cnt(w, tab);

                w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);

                w.WriteLine("{0}if (cnt > 0) {{ {1} = new {2}[cnt]; {3} {4}; for (int i = 0; i < cnt; ++i) {{ {5} = fs.{6}(); {7}[i] = ({8}){9}; }}}}"
                    , tab, elem.mFieldName, elem.mValueType, strtype, val_enum, val_enum, readfunc, elem.mFieldName, elem.mValueType, val_enum);
            }
            else
            {
                w.WriteLine("{0}{1} {2} = fs.{3}(); {4} = ({5}){6};", tab, strtype, val_enum, readfunc, elem.mFieldName, elem.mValueType, val_enum);
            }
        }

        static private void WriteClient_Unity_ReadFunc_Vector2(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                check_define_cnt(w, tab);
                w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1} = new Vector2[cnt]; for (int i = 0; i < cnt; ++i) {{ {2}[i].x = fs.ReadSingle(); {3}[i].y = fs.ReadSingle(); }}}}"
                    , tab, elem.mFieldName, elem.mFieldName, elem.mFieldName);
            }
            else
            {
                w.WriteLine("{0}{1}.x = fs.ReadSingle(); {2}.y = fs.ReadSingle();", tab, elem.mFieldName, elem.mFieldName, elem.mFieldName);
            }
        }

        static private void WriteClient_Unity_ReadFunc_Vector3(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                check_define_cnt(w, tab);
                w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1} = new Vector3[cnt]; for (int i = 0; i < cnt; ++i) {{ {2}[i].x = fs.ReadSingle(); {3}[i].y = fs.ReadSingle(); {4}[i].z = fs.ReadSingle(); }}}}"
                    , tab, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName);
            }
            else
            {
                w.WriteLine("{0}{1}.x = fs.ReadSingle(); {2}.y = fs.ReadSingle(); {3}.z = fs.ReadSingle();", tab, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName);
            }
        }

        static private void WriteClient_Unity_ReadFunc_Color(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                check_define_cnt(w, tab);
                w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1} = new Color[cnt]; for (int i = 0; i < cnt; ++i) {{ {2}[i].r = fs.ReadSingle(); {3}[i].g = fs.ReadSingle(); {4}[i].b = fs.ReadSingle(); {5}[i].a = fs.ReadSingle(); }}}}"
                    , tab, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName);
            }
            else
            {
                w.WriteLine("{0}{1}.r = fs.ReadSingle(); {2}.g = fs.ReadSingle(); {3}.b = fs.ReadSingle(); {4}.a = fs.ReadSingle();", tab, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName);
            }
        }

        static private void WriteClient_Unity_ReadFunc_Default(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                check_define_cnt(w, tab);

                w.WriteLine("{0}cnt = fs.ReadUInt16();", tab);

                w.WriteLine("{0}if (cnt > 0) {{ {1} = new {2}[cnt]; fs.Read(buffer, 0, cnt * {3}); Buffer.BlockCopy(buffer, 0, {4}, 0, cnt * {5}); }}"
                    , tab, elem.mFieldName, Util.ConvertTypeUnity(elem.mDataType), elem.mTypeSize, elem.mFieldName, elem.mTypeSize);
            }
            else
            {
                w.WriteLine("{0}{1} = fs.{2}();", tab, elem.mFieldName, Util.GetBinaryReaderFuncNameUnity(elem.mDataType));
            }
        }

        static private void WriteClient_Unity_LoadFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}if (InForceResources == true)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}LoadFromResources(buffer);", tab + Util.Tab);
            w.WriteLine("{0}return;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            w.WriteLine("{0}int storageType = 0;", tab);
            w.WriteLine("");
            w.WriteLine("{0}GetLastVersion(out storageType);", tab);
            w.WriteLine("");
            w.WriteLine("{0}if (storageType == 1) // Resources", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}LoadFromResources(buffer);", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("{0}else if (storageType == 2) // Download", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}LoadFromFile(buffer);", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
        }

        static private void WriteClient_Unity_LoadFromStreamFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}if (fs == null)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}return false;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            w.WriteLine("{0}int version = fs.ReadInt32();", tab); // version
            w.WriteLine("{0}int totalRowCount = fs.ReadInt32();", tab); // totalRowCount
            w.WriteLine("{0}int maxBufferSize = fs.ReadInt32();", tab); // maxBufferSize
            w.WriteLine("");

            w.WriteLine("{0}Read(fs, buffer);", tab);
            w.WriteLine("");

            w.WriteLine("{0}fs.Close();", tab);
            w.WriteLine("{0}fs.Dispose();", tab);
            w.WriteLine("");

            w.WriteLine("{0}return true;", tab);
        }

        static private void WriteClient_Unity_LoadFromResourcesFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}TextAsset textAsset = Resources.Load(\"Table/{1}\") as TextAsset;", tab, dfn.mTableName);
            w.WriteLine("{0}if (textAsset == null) {{ return false; }}", tab);
            w.WriteLine("{0}bool ret = LoadFromStream(new BinaryReader(new MemoryStream(textAsset.bytes)), buffer);", tab);
            w.WriteLine("{0}Resources.UnloadAsset(textAsset);", tab);
            w.WriteLine("{0}return ret;", tab);
        }

        static private void WriteClient_Unity_LoadFromFileFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}string filePath = Application.temporaryCachePath + \"/Table/{1}.txt\";", tab, dfn.mTableName);
            w.WriteLine("{0}if (File.Exists(filePath) == false) {{ return false; }}", tab);
            w.WriteLine("{0}return LoadFromStream(new BinaryReader(new FileStream(filePath, FileMode.Open)), buffer);", tab);
        }

        static private void WriteClient_Unity_GetLastVersionFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}OutStorageType = 0;", tab);
            w.WriteLine("");
            w.WriteLine("{0}int verResources = 0;", tab);
            w.WriteLine("{0}int verDownload = 0;", tab);
            w.WriteLine("");
            w.WriteLine("{0}TextAsset textAsset = Resources.Load(\"Table/{1}\") as TextAsset;", tab, dfn.mTableName);
            w.WriteLine("{0}if (textAsset != null) {{ verResources = BitConverter.ToInt32(textAsset.bytes, 0); }}", tab);
            w.WriteLine("{0}Resources.UnloadAsset(textAsset);", tab);
            w.WriteLine("");
            w.WriteLine("{0}string filePath = Application.temporaryCachePath + \"/Table/{1}.txt\";", tab, dfn.mTableName);
            w.WriteLine("{0}if (File.Exists(filePath) == true)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}using (BinaryReader fs = new BinaryReader(new FileStream(filePath, FileMode.Open)))", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}verDownload = fs.ReadInt32();", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");
            w.WriteLine("{0}if (verResources >= verDownload) {{ OutStorageType = 1; return verResources; }}", tab);
            w.WriteLine("{0}else {{ OutStorageType = 2; return verDownload; }}", tab);
        }
    }
}
