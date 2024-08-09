using System;
using System.IO;


namespace Table.Exporter
{
    partial class TBL_Config
    {
        static public void WriteClient(Defines dfn, string outputPath)
        {
            string dir = outputPath + Settings.DATA_DIR;

            Util.MakeFolder(dir);


            string filepath = string.Format("{0}TB{1}.h", dir, dfn.mTableName);
            StreamWriter w = Util.GetNewTextFile(filepath);


            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("#include \"../Types/SE.h\"");
            w.WriteLine("#include \"../Types/SS.h\"");
            w.WriteLine("#include \"./TableFileStream.h\"");
            w.WriteLine("#include \"TB{0}.generated.h\"", dfn.mTableName);
            w.WriteLine("");
            w.WriteLine("");


            w.WriteLine("UCLASS(Blueprintable, BlueprintType)");
            w.WriteLine("class {0} UTB{1} : public UObject", Settings.API, dfn.mTableName);
            w.WriteLine("{");
            w.WriteLine("{0}GENERATED_BODY()", Util.Tab);
            w.WriteLine("public:");

            WriteClient_VariableDefinition(w, dfn, Util.Tab);
            w.WriteLine("");


            w.WriteLine("{0}void Read(UTableFileStream* fs)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_ReadFunc(w, dfn, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}bool Load()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_LoadFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}UTableFileStream* LoadFile()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_LoadFileFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}bool LoadFromStream(UTableFileStream* fs)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_LoadFromStreamFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);


            w.WriteLine("};");


            Util.FlushAndReleaseTextFile(w);
        }

        static private void WriteClient_VariableDefinition(StreamWriter w, Defines dfn, string tab)
        {
            for (int i = 0; i < dfn.mElements.Count; ++i)
            {
                Defines.Element elem = dfn.mElements[i];

                if (elem.mUsage != Defines.Usage.Client && elem.mUsage != Defines.Usage.Both)
                {
                    continue;
                }

                w.WriteLine("{0}UPROPERTY() {1} {2};", tab, elem.mDataTypeDef_C, elem.mFieldName);
            }
        }

        static private void WriteClient_ReadFunc(StreamWriter w, Defines dfn, string tab)
        {
            tab += Util.Tab;

            w.WriteLine("{0}uint16 cnt = 0; uint16 snt = 0; uint8 k = 0; uint32 v = 0;", tab);
            w.WriteLine("");

            for (int i = 0; i < dfn.mElements.Count; ++i)
            {
                Defines.Element elem = dfn.mElements[i];

                if (elem.mUsage != Defines.Usage.Client && elem.mUsage != Defines.Usage.Both)
                {
                    continue;
                }

                switch (elem.mDataType)
                {
                    case "string": WriteClient_ReadFunc_String(w, elem, tab); break;
                    case "json2": WriteClient_ReadFunc_Json2(w, elem, tab); break;
                    case "struct": WriteClient_ReadFunc_Struct(w, dfn, elem, tab); break;
                    default: WriteClient_ReadFunc_Default(w, elem, tab); break;
                }
            }
        }

        static private void WriteClient_ReadFunc_String(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                w.WriteLine("{0}fs->Read(&snt, 2);", tab);

                w.WriteLine("{0}if (snt > 0)", tab);
                w.WriteLine("{0}{{", tab);

                w.WriteLine("{0}for (int32 i = 0; i < snt; ++i)", tab + Util.Tab);
                w.WriteLine("{0}{{", tab + Util.Tab);

                w.WriteLine("{0}FString {1}_Str;", tab + Util.Tab + Util.Tab, elem.mFieldName);
                w.WriteLine("{0}fs->Read(&cnt, 2);", tab + Util.Tab + Util.Tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1}_Str.GetCharArray().AddUninitialized(cnt + 1); fs->Read({2}_Str.GetCharArray().GetData(), cnt * 2); {3}_Str.GetCharArray()[cnt] = 0; }}"
                    , tab + Util.Tab + Util.Tab, elem.mFieldName, elem.mFieldName, elem.mFieldName);
                w.WriteLine("{0}{1}.Add({2}_Str);", tab + Util.Tab + Util.Tab, elem.mFieldName, elem.mFieldName);

                w.WriteLine("{0}}}", tab + Util.Tab);
                w.WriteLine("{0}}}", tab);
            }
            else
            {
                w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1}.GetCharArray().AddUninitialized(cnt + 1); fs->Read({2}.GetCharArray().GetData(), cnt * 2); {3}.GetCharArray()[cnt] = 0; }}"
                    , tab, elem.mFieldName, elem.mFieldName, elem.mFieldName);
            }
        }

        static private void WriteClient_ReadFunc_Json2(StreamWriter w, Defines.Element elem, string tab)
        {
            w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
            w.WriteLine("{0}for (int i = 0; i < cnt; ++i) {{ fs->Read(&k, 1); fs->Read(&v, 4); {1}.Add(({2})k, v); }}"
                , tab, elem.mFieldName, elem.mValueType);
        }

        static private void WriteClient_ReadFunc_Struct(StreamWriter w, Defines dfn, Defines.Element elem, string tab)
        {
            Structs.Info st = Structs.Get(elem.mValueType);
            if (st.mContainString == true)
            {
                if (elem.mArray == true)
                {
                    w.WriteLine("{0}fs->Read(&snt, 2);", tab);
                    w.WriteLine("{0}for (int i = 0; i < snt; ++i)", tab);
                    w.WriteLine("{0}{{", tab);

                    w.WriteLine("{0}{1} s{2};", tab + Util.Tab, elem.mValueType, elem.mFieldName);

                    for (int i = 0; i < st.mValueTypeKV.Count; ++i)
                    {
                        Tuple<string, string, int> tup = st.mValueTypeKV[i];
                        if (tup.Item2 == "string")
                        {
                            w.WriteLine("{0}fs->Read(&cnt, 2);", tab + Util.Tab);
                            string val = string.Format("s{0}.{1}", elem.mFieldName, tup.Item1);
                            w.WriteLine("{0}if (cnt > 0) {{ {1}.GetCharArray().AddUninitialized(cnt + 1); fs->Read({2}.GetCharArray().GetData(), cnt * 2); {3}.GetCharArray()[cnt] = 0; }}"
                                , tab + Util.Tab, val, val, val);
                        }
                        else
                        {
                            w.WriteLine("{0}fs->Read(&s{1}.{2}, {3});"
                                , tab + Util.Tab, elem.mFieldName, tup.Item1, Defines.Element.GetSize(tup.Item2, tup.Item2));
                        }
                    }

                    w.WriteLine("{0}{1}.Add(s{2});", tab + Util.Tab, elem.mFieldName, elem.mFieldName);

                    w.WriteLine("{0}}}", tab);
                }
                else
                {
                    for (int i = 0; i < st.mValueTypeKV.Count; ++i)
                    {
                        Tuple<string, string, int> tup = st.mValueTypeKV[i];
                        if (tup.Item2 == "string")
                        {
                            w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
                            string val = string.Format("{0}.{1}", elem.mFieldName, tup.Item1);
                            w.WriteLine("{0}if (cnt > 0) {{ {1}.GetCharArray().AddUninitialized(cnt + 1); fs->Read({2}.GetCharArray().GetData(), cnt * 2); {3}.GetCharArray()[cnt] = 0; }}", tab, val, val, val);
                        }
                        else
                        {
                            w.WriteLine("{0}fs->Read(&{1}.{2}, {3});", tab, elem.mFieldName, tup.Item1, Defines.Element.GetSize(tup.Item2, tup.Item2));
                        }
                    }
                }
            }
            else
            {
                WriteClient_ReadFunc_Default(w, elem, tab);
            }
        }

        static private void WriteClient_ReadFunc_Default(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1}.Empty(cnt); {2}.AddUninitialized(cnt); fs->Read({3}.GetData(), cnt * {4}); }}"
                    , tab, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mTypeSize);
            }
            else
            {
                w.WriteLine("{0}fs->Read(&{1}, {2});", tab, elem.mFieldName, elem.mTypeSize);
            }
        }

        static private void WriteClient_LoadFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}return LoadFromStream(LoadFile());", tab);
        }

        static private void WriteClient_LoadFileFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}UTableFileStream* fs = NewObject<UTableFileStream>(this);", tab);
            w.WriteLine("");

            w.WriteLine("{0}fs->Open(TEXT(\"/Table/{1}.tbd\"));", tab, dfn.mTableName);
            w.WriteLine("");

            w.WriteLine("{0}if (fs->is_open == false || fs->totalRowCount <= 0)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            w.WriteLine("{0}return fs;", tab);
        }

        static private void WriteClient_LoadFromStreamFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}if (fs == nullptr)", tab);
            w.WriteLine("{0}return false;", tab + Util.Tab);
            w.WriteLine("");

            w.WriteLine("{0}Read(fs);", tab);
            w.WriteLine("");

            w.WriteLine("{0}return true;", tab);
        }
    }
}
