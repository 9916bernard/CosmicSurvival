using System;
using System.IO;


namespace Table.Exporter
{
    partial class TBL_Normal
    {
        static public void WriteClient_Unity(Defines dfn, string outputPath)
        {
            string dir = outputPath + Settings.DATA_DIR;

            Util.MakeFolder(dir);


            string filepath = string.Format("{0}TB{1}.cs", dir, dfn.mTableName);
            StreamWriter w = Util.GetNewTextFile(filepath);


            w.WriteLine("using System;");
            w.WriteLine("using System.IO;");
            w.WriteLine("using System.Collections.Generic;");
            w.WriteLine("using UnityEngine;");
            w.WriteLine("");

            w.WriteLine("public class UTB{0}_Record", dfn.mTableName);
            w.WriteLine("{");

            WriteClient_Unity_VariableDefinition(w, dfn, Util.Tab);
            w.WriteLine("");

            WriteClient_Unity_CustomFunction(w, dfn, Util.Tab);
            w.WriteLine("");

            w.WriteLine("{0}public void Read(BinaryReader fs, byte[] buffer)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_Unity_ReadFunc(w, dfn, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("};");
            w.WriteLine("");


            w.WriteLine("public class UTB{0}", dfn.mTableName);
            w.WriteLine("{");

            string dsName = WriteClient_Unity_RecordVariable(w, dfn, Util.Tab);
            w.WriteLine("");


            w.WriteLine("{0}public void Load(byte[] buffer, bool InForceResources)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_Unity_LoadFunc(w, dfn, dsName, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}public bool LoadFromStream(BinaryReader fs, byte[] buffer)", Util.Tab);
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


            WriteClient_Unity_FindFunc(w, dfn, Util.Tab);


            //WriteClient_Unity_FindStringFunc(w, dfn, Util.Tab);

            w.WriteLine("};");


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

        static private void WriteClient_Unity_CustomFunction(StreamWriter w, Defines dfn, string tab)
        {
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
                w.WriteLine("{0}if (cnt > 0) {{ fs.Read(buffer, 0, cnt * 2); {1} = System.Text.Encoding.Unicode.GetString(buffer, 0, cnt * 2); }}", tab, elem.mFieldName);
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
                                w.WriteLine("{0}{1} {2} = fs.{3}(); {4} = ({5}){6};", tab, strtype, val_enum, readfunc, val, tup.Item2, val_enum);
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

        static private void WriteClient_Unity_MultiKeyStorage(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("UCLASS(Blueprintable, BlueprintType)");
            w.WriteLine("class {0} UTB{1}_Storage : public UObject", Settings.API, dfn.mTableName);
            w.WriteLine("{");
            w.WriteLine("{0}GENERATED_BODY()", Util.Tab);
            w.WriteLine("public:");

            if (dfn.mSingleKeyElement == null)
            {
                w.WriteLine("{0}UPROPERTY() TArray<UTB{1}_Record*> mArray;", Util.Tab, dfn.mTableName);
            }
            else
            {
                w.WriteLine("{0}UPROPERTY() TMap<{1}, UTB{2}_Record*> mMap;", Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_C, dfn.mTableName);
            }

            w.WriteLine("");


            w.WriteLine("{0}virtual ~UTB{1}_Storage()", Util.Tab, dfn.mTableName);
            w.WriteLine("{0}{{", Util.Tab);

            if (dfn.mSingleKeyElement == null)
            {
                w.WriteLine("{0}mArray.Empty();", Util.Tab + Util.Tab);
            }
            else
            {
                w.WriteLine("{0}mMap.Empty();", Util.Tab + Util.Tab);
            }

            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");


            w.WriteLine("{0}void Add(UTB{1}_Record* rec)", Util.Tab, dfn.mTableName);
            w.WriteLine("{0}{{", Util.Tab);

            if (dfn.mSingleKeyElement == null)
            {
                w.WriteLine("{0}mArray.Add(rec);", Util.Tab + Util.Tab);
            }
            else
            {
                w.WriteLine("{0}mMap.Add(rec->{1}, rec);", Util.Tab + Util.Tab, dfn.mSingleKeyElement.mFieldName);
            }

            w.WriteLine("{0}}}", Util.Tab);

            if (dfn.mSingleKeyElement != null)
            {
                w.WriteLine("");
                w.WriteLine("{0}UTB{1}_Record* Find({2} key)", Util.Tab, dfn.mTableName, dfn.mSingleKeyElement.mDataTypeDef_C);
                w.WriteLine("{0}{{", Util.Tab);

                w.WriteLine("{0}UTB{1}_Record** prec = mMap.Find(key);", Util.Tab + Util.Tab, dfn.mTableName);
                w.WriteLine("{0}if (prec == nullptr)", Util.Tab + Util.Tab);
                w.WriteLine("{0}{{", Util.Tab + Util.Tab);
                w.WriteLine("{0}return nullptr;", Util.Tab + Util.Tab + Util.Tab);
                w.WriteLine("{0}}}", Util.Tab + Util.Tab);
                w.WriteLine("{0}return (*prec);", Util.Tab + Util.Tab);

                w.WriteLine("{0}}}", Util.Tab);
            }

            w.WriteLine("};");
        }

        static private string WriteClient_Unity_RecordVariable(StreamWriter w, Defines dfn, string tab)
        {
            string dsName = "";

            if (dfn.mMultiKeyElement != null)
            {
                if (dfn.mSingleKeyElement != null)
                {
                    w.WriteLine("{0}public Dictionary<{1}, Dictionary<{2}, UTB{3}_Record>> mapTable = new Dictionary<{4}, Dictionary<{5}, UTB{6}_Record>>();"
                        , tab, dfn.mMultiKeyElement.mDataTypeDef_C_Unity, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName, dfn.mMultiKeyElement.mDataTypeDef_C_Unity, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName);
                    dsName = "mapTable";
                }
                else
                {
                    w.WriteLine("{0}public Dictionary<{1}, List<UTB{2}_Record>> mapTable = new Dictionary<{3}, List<UTB{4}_Record>>();", tab, dfn.mMultiKeyElement.mDataTypeDef_C_Unity, dfn.mTableName, dfn.mMultiKeyElement.mDataTypeDef_C_Unity, dfn.mTableName);
                    dsName = "mapTable";
                }
            }
            else if (dfn.mSingleKeyElement != null)
            {
                w.WriteLine("{0}public Dictionary<{1}, UTB{2}_Record> mapTable = new Dictionary<{3}, UTB{4}_Record>();", tab, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName);
                dsName = "mapTable";
            }
            else
            {
                w.WriteLine("{0}public List<UTB{1}_Record> arrayTable = new List<UTB{2}_Record>();", tab, dfn.mTableName, dfn.mTableName);
                dsName = "arrayTable";
            }

            return dsName;
        }

        static private void WriteClient_Unity_LoadFunc(StreamWriter w, Defines dfn, string dsname, string tab)
        {
            w.WriteLine("{0}{1}.Clear();", tab, dsname);
            w.WriteLine("");

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
            w.WriteLine("{0}return false;", tab + Util.Tab);
            w.WriteLine("");

            w.WriteLine("{0}int version = fs.ReadInt32();", tab); // version
            w.WriteLine("{0}int totalRowCount = fs.ReadInt32();", tab); // totalRowCount
            w.WriteLine("{0}int maxBufferSize = fs.ReadInt32();", tab); // maxBufferSize
            w.WriteLine("");

            w.WriteLine("{0}for (int i = 0; i < totalRowCount; ++i)", tab);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Record rec = new UTB{2}_Record();", tab + Util.Tab, dfn.mTableName, dfn.mTableName);
            w.WriteLine("");
            w.WriteLine("{0}rec.Read(fs, buffer);", tab + Util.Tab);
            w.WriteLine("");


            if (dfn.mMultiKeyElement != null)
            {
                if (dfn.mSingleKeyElement != null)
                {
                    w.WriteLine("{0}Dictionary<{1}, UTB{2}_Record> dic = null;", tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName);
                    w.WriteLine("{0}if (mapTable.TryGetValue(rec.{1}, out dic) == false)", tab + Util.Tab, dfn.mMultiKeyElement.mFieldName);
                    w.WriteLine("{0}{{", tab + Util.Tab);
                    w.WriteLine("{0}dic = new Dictionary<{1}, UTB{2}_Record>();", tab + Util.Tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName);
                    w.WriteLine("{0}mapTable.Add(rec.{1}, dic);", tab + Util.Tab + Util.Tab, dfn.mMultiKeyElement.mFieldName);
                    w.WriteLine("{0}}}", tab + Util.Tab);
                    w.WriteLine("");
                    w.WriteLine("{0}if (dic.TryAdd(rec.{1}, rec) == false)", tab + Util.Tab, dfn.mSingleKeyElement.mFieldName);
                    w.WriteLine("{0}{{", tab + Util.Tab);
                    w.WriteLine("{0}return false;", tab + Util.Tab + Util.Tab);
                    w.WriteLine("{0}}}", tab + Util.Tab);
                }
                else
                {
                    w.WriteLine("{0}List<UTB{1}_Record> list = null;", tab + Util.Tab, dfn.mTableName);
                    w.WriteLine("{0}if (mapTable.TryGetValue(rec.{1}, out list) == false)", tab + Util.Tab, dfn.mMultiKeyElement.mFieldName);
                    w.WriteLine("{0}{{", tab + Util.Tab);
                    w.WriteLine("{0}list = new List<UTB{1}_Record>();", tab + Util.Tab + Util.Tab, dfn.mTableName);
                    w.WriteLine("{0}mapTable.Add(rec.{1}, list);", tab + Util.Tab + Util.Tab, dfn.mMultiKeyElement.mFieldName);
                    w.WriteLine("{0}}}", tab + Util.Tab);
                    w.WriteLine("");
                    w.WriteLine("{0}list.Add(rec);", tab + Util.Tab);
                }
            }
            else
            {
                if (dfn.mSingleKeyElement != null)
                {
                    w.WriteLine("{0}mapTable.Add(rec.{1}, rec);", tab + Util.Tab, dfn.mSingleKeyElement.mFieldName);
                }
                else
                {
                    w.WriteLine("{0}arrayTable.Add(rec);", tab + Util.Tab);
                }
            }


            w.WriteLine("{0}}}", tab);
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

        static private void WriteClient_Unity_FindFunc(StreamWriter w, Defines dfn, string tab)
        {
            if (dfn.mMultiKeyElement != null)
            {
                if (dfn.mSingleKeyElement != null)
                {
                    WriteClient_Unity_FindFunc_Type2(w, dfn, tab);
                }
                else
                {
                    WriteClient_Unity_FindFunc_Type1(w, dfn, tab);
                }
            }
            else if (dfn.mSingleKeyElement != null)
            {
                WriteClient_Unity_FindFunc_Type0(w, dfn, tab);
            }
        }

        static private void WriteClient_Unity_FindFunc_Type2(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("");
            w.WriteLine("{0}public Dictionary<{1}, UTB{2}_Record> Find({3} key1)", tab, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName, dfn.mMultiKeyElement.mDataTypeDef_C_Unity);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}Dictionary<{1}, UTB{2}_Record> dic = null;", tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName);
            w.WriteLine("{0}if (mapTable.TryGetValue(key1, out dic) == false)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return null;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return dic;", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);


            w.WriteLine("");


            w.WriteLine("{0}public UTB{1}_Record Find({2} key1, {3} key2)", tab, dfn.mTableName, dfn.mMultiKeyElement.mDataTypeDef_C_Unity, dfn.mSingleKeyElement.mDataTypeDef_C_Unity);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}Dictionary<{1}, UTB{2}_Record> dic = null;", tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_C_Unity, dfn.mTableName);
            w.WriteLine("{0}if (mapTable.TryGetValue(key1, out dic) == false)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return null;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("");
            w.WriteLine("{0}UTB{1}_Record rec = null;", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}if (dic.TryGetValue(key2, out rec) == false)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return null;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return rec;", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);
        }

        static private void WriteClient_Unity_FindFunc_Type1(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("");
            w.WriteLine("{0}public List<UTB{1}_Record> Find({2} key1)", tab, dfn.mTableName, dfn.mMultiKeyElement.mDataTypeDef_C_Unity);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}List<UTB{1}_Record> list = null;", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}if (mapTable.TryGetValue(key1, out list) == false)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return null;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return list;", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);
        }

        static private void WriteClient_Unity_FindFunc_Type0(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("");
            w.WriteLine("{0}public UTB{1}_Record Find({2} key1)", tab, dfn.mTableName, dfn.mSingleKeyElement.mDataTypeDef_C_Unity);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Record rec = null;", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}if (mapTable.TryGetValue(key1, out rec) == false)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return null;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return rec;", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);
        }

        static private void WriteClient_Unity_FindStringFunc(StreamWriter w, Defines dfn, string tab)
        {
            Defines.Element elem = dfn.mElements.Find(x => x.mDataType == "name");

            if (elem == null)
                return;

            w.WriteLine("");
            w.WriteLine("{0}FString Get(const WIDECHAR* key)", tab);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Record** prec = mapTable.Find(FName(key));", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}if (prec == nullptr)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return FString();", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return (*prec)->Str_KR;", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);
        }
    }
}
