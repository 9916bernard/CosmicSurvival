using System;
using System.IO;


namespace Table.Exporter
{
    partial class TBL_Normal
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
            w.WriteLine("class {0} UTB{1}_Record : public UObject", Settings.API, dfn.mTableName);
            w.WriteLine("{");
            w.WriteLine("{0}GENERATED_BODY()", Util.Tab);
            w.WriteLine("public:");

            WriteClient_VariableDefinition(w, dfn, Util.Tab);
            w.WriteLine("");

            WriteClient_CustomFunction(w, dfn, Util.Tab);
            w.WriteLine("");

            w.WriteLine("{0}void Read(UTableFileStream* fs)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteClient_ReadFunc(w, dfn, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("};");
            w.WriteLine("");


            if (dfn.mMultiKeyElement != null)
            {
                WriteClient_MultiKeyStorage(w, dfn, "");
                w.WriteLine("");
            }


            w.WriteLine("UCLASS(Blueprintable, BlueprintType)");
            w.WriteLine("class {0} UTB{1} : public UObject", Settings.API, dfn.mTableName);
            w.WriteLine("{");
            w.WriteLine("{0}GENERATED_BODY()", Util.Tab);
            w.WriteLine("public:");

            WriteClient_RecordVariable(w, dfn, Util.Tab);
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


            WriteClient_FindFunc(w, dfn, Util.Tab);


            WriteClient_FindStringFunc(w, dfn, Util.Tab);

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

        static private void WriteClient_CustomFunction(StreamWriter w, Defines dfn, string tab)
        {
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
                    case "name": WriteClient_ReadFunc_Name(w, elem, tab); break;
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

        static private void WriteClient_ReadFunc_Name(StreamWriter w, Defines.Element elem, string tab)
        {
            w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
            w.WriteLine("{0}if (cnt > 0) {{ FString s{1}; s{2}.GetCharArray().AddUninitialized(cnt + 1); fs->Read(s{3}.GetCharArray().GetData(), cnt * 2); s{4}.GetCharArray()[cnt] = 0; {5} = FName(*s{6}); }}"
                , tab, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName, elem.mFieldName);
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
                        else if (tup.Item2 == "name")
                        {
                            w.WriteLine("{0}fs->Read(&cnt, 2);", tab + Util.Tab);
                            string val = string.Format("s{0}.{1}", elem.mFieldName, tup.Item1);
                            w.WriteLine("{0}if (cnt > 0) {{ FString s{1}; s{2}.GetCharArray().AddUninitialized(cnt + 1); fs->Read(s{3}.GetCharArray().GetData(), cnt * 2); s{4}.GetCharArray()[cnt] = 0; }}"
                                , tab + Util.Tab, val, val, val, val);
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
                        else if (tup.Item2 == "name")
                        {
                            w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
                            string val = string.Format("{0}.{1}", elem.mFieldName, tup.Item1);
                            w.WriteLine("{0}if (cnt > 0) {{ FString s{1}; s{2}.GetCharArray().AddUninitialized(cnt + 1); fs->Read(s{3}.GetCharArray().GetData(), cnt * 2); s{4}.GetCharArray()[cnt] = 0; }}", tab, val, val, val, val);
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

        static private void WriteClient_MultiKeyStorage(StreamWriter w, Defines dfn, string tab)
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

        static private void WriteClient_RecordVariable(StreamWriter w, Defines dfn, string tab)
        {
            if (dfn.mMultiKeyElement != null)
            {
                w.WriteLine("{0}UPROPERTY() TMap<{1}, UTB{2}_Storage*> mapTable;", tab, dfn.mMultiKeyElement.mDataTypeDef_C, dfn.mTableName);
            }
            else if (dfn.mSingleKeyElement != null)
            {
                w.WriteLine("{0}UPROPERTY() TMap<{1}, UTB{2}_Record*> mapTable;", tab, dfn.mSingleKeyElement.mDataTypeDef_C, dfn.mTableName);
            }
            else
            {
                w.WriteLine("{0}UPROPERTY() TArray<UTB{1}_Record*> arrayTable;", tab, dfn.mTableName);
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

            w.WriteLine("{0}for (int i = 0; i < fs->totalRowCount; ++i)", tab);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Record* rec = NewObject<UTB{2}_Record>(this);", tab + Util.Tab, dfn.mTableName, dfn.mTableName);
            w.WriteLine("");
            w.WriteLine("{0}rec->Read(fs);", tab + Util.Tab);
            w.WriteLine("");


            if (dfn.mMultiKeyElement != null)
            {
                w.WriteLine("{0}UTB{1}_Storage** pstorage = mapTable.Find(rec->{2});", tab + Util.Tab, dfn.mTableName, dfn.mMultiKeyElement.mFieldName);
                w.WriteLine("{0}if (pstorage == nullptr)", tab + Util.Tab);
                w.WriteLine("{0}{{", tab + Util.Tab);
                w.WriteLine("{0}UTB{1}_Storage* storage = NewObject<UTB{2}_Storage>(this);", tab + Util.Tab + Util.Tab, dfn.mTableName, dfn.mTableName);
                w.WriteLine("{0}storage->Add(rec);", tab + Util.Tab + Util.Tab);
                w.WriteLine("{0}mapTable.Add(rec->{1}, storage);", tab + Util.Tab + Util.Tab, dfn.mMultiKeyElement.mFieldName);
                w.WriteLine("{0}}}", tab + Util.Tab);
                w.WriteLine("{0}else", tab + Util.Tab);
                w.WriteLine("{0}{{", tab + Util.Tab);
                w.WriteLine("{0}(*pstorage)->Add(rec);", tab + Util.Tab + Util.Tab);
                w.WriteLine("{0}}}", tab + Util.Tab);
            }
            else
            {
                if (dfn.mSingleKeyElement != null)
                {
                    w.WriteLine("{0}mapTable.Add(rec->{1}, rec);", tab + Util.Tab, dfn.mSingleKeyElement.mFieldName);
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

        //static private void WriteClient_LoadFromStreamFunc(StreamWriter w, Defines dfn, string tab)
        //{
        //    w.WriteLine("{0}UTableFileStream* fs = NewObject<UTableFileStream>(this);", tab);
        //    w.WriteLine("");

        //    w.WriteLine("{0}fs->Open(TEXT(\"/Table/{1}.tbd\"));", tab, dfn.mTableName);
        //    w.WriteLine("");

        //    w.WriteLine("{0}if (fs->is_open == false || fs->totalRowCount <= 0)", tab);
        //    w.WriteLine("{0}{{", tab);
        //    w.WriteLine("{0}return false;", tab + Util.Tab);
        //    w.WriteLine("{0}}}", tab);
        //    w.WriteLine("");

        //    w.WriteLine("{0}for (int i = 0; i < fs->totalRowCount; ++i)", tab);
        //    w.WriteLine("{0}{{", tab);

        //    w.WriteLine("{0}UTB{1}_Record* rec = NewObject<UTB{2}_Record>(this);", tab + Util.Tab, dfn.mTableName, dfn.mTableName);
        //    w.WriteLine("");
        //    w.WriteLine("{0}rec->Read(fs);", tab + Util.Tab);
        //    w.WriteLine("");


        //    if (dfn.mMultiKeyElement != null)
        //    {
        //        w.WriteLine("{0}UTB{1}_Storage** pstorage = mapTable.Find(rec->{2});", tab + Util.Tab, dfn.mTableName, dfn.mMultiKeyElement.mFieldName);
        //        w.WriteLine("{0}if (pstorage == nullptr)", tab + Util.Tab);
        //        w.WriteLine("{0}{{", tab + Util.Tab);
        //        w.WriteLine("{0}UTB{1}_Storage* storage = NewObject<UTB{2}_Storage>(this);", tab + Util.Tab + Util.Tab, dfn.mTableName, dfn.mTableName);
        //        w.WriteLine("{0}storage->Add(rec);", tab + Util.Tab + Util.Tab);
        //        w.WriteLine("{0}mapTable.Add(rec->{1}, storage);", tab + Util.Tab + Util.Tab, dfn.mMultiKeyElement.mFieldName);
        //        w.WriteLine("{0}}}", tab + Util.Tab);
        //        w.WriteLine("{0}else", tab + Util.Tab);
        //        w.WriteLine("{0}{{", tab + Util.Tab);
        //        w.WriteLine("{0}(*pstorage)->Add(rec);", tab + Util.Tab + Util.Tab);
        //        w.WriteLine("{0}}}", tab + Util.Tab);
        //    }
        //    else
        //    {
        //        if (dfn.mSingleKeyElement != null)
        //        {
        //            w.WriteLine("{0}mapTable.Add(rec->{1}, rec);", tab + Util.Tab, dfn.mSingleKeyElement.mFieldName);
        //        }
        //        else
        //        {
        //            w.WriteLine("{0}arrayTable.Add(rec);", tab + Util.Tab);
        //        }
        //    }


        //    w.WriteLine("{0}}}", tab);
        //    w.WriteLine("");

        //    w.WriteLine("{0}return true;", tab);
        //}

        static private void WriteClient_FindFunc(StreamWriter w, Defines dfn, string tab)
        {
            if (dfn.mMultiKeyElement != null)
            {
                if (dfn.mSingleKeyElement != null)
                {
                    WriteClient_FindFunc_Type2(w, dfn, tab);
                }
                else
                {
                    WriteClient_FindFunc_Type1(w, dfn, tab);
                }
            }
            else if (dfn.mSingleKeyElement != null)
            {
                WriteClient_FindFunc_Type0(w, dfn, tab);
            }
        }

        static private void WriteClient_FindStringFunc(StreamWriter w, Defines dfn, string tab)
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

        static private void WriteClient_FindFunc_Type2(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}UTB{1}_Storage* Find({2} key1)", tab, dfn.mTableName, dfn.mMultiKeyElement.mDataTypeDef_C);
            w.WriteLine("");
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Storage** pstorage = mapTable.Find(key1);", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}if (pstorage == nullptr)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return (*pstorage);", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);
            w.WriteLine("");


            w.WriteLine("{0}UTB{1}_Record* Find({2} key1, {3} key2)", tab, dfn.mTableName, dfn.mMultiKeyElement.mDataTypeDef_C, dfn.mSingleKeyElement.mDataTypeDef_C);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Storage** pstorage = mapTable.Find(key1);", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}if (pstorage == nullptr)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return (*pstorage)->Find(key2);", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);
        }

        static private void WriteClient_FindFunc_Type1(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("");
            w.WriteLine("{0}UTB{1}_Storage* Find({2} key1)", tab, dfn.mTableName, dfn.mMultiKeyElement.mDataTypeDef_C);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Storage** pstorage = mapTable.Find(key1);", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}if (pstorage == nullptr)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return (*pstorage);", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);
        }

        static private void WriteClient_FindFunc_Type0(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("");
            w.WriteLine("{0}UTB{1}_Record* Find({2} key)", tab, dfn.mTableName, dfn.mSingleKeyElement.mDataTypeDef_C);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Record** prec = mapTable.Find(key);", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}if (prec == nullptr)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return (*prec);", tab + Util.Tab);

            w.WriteLine("{0}}}", tab);
        }
    }
}
