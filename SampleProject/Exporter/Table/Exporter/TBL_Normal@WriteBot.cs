using System;
using System.IO;
using System.Linq;


namespace Table.Exporter
{
    partial class TBL_Normal
    {
        static public void WriteBot(Defines dfn, string outputPath)
        {
            string dir = outputPath + Settings.DATA_DIR;

            Util.MakeFolder(dir);


            string filepath = string.Format("{0}TB{1}.h", dir, dfn.mTableName);
            StreamWriter w = Util.GetNewTextFile(filepath);


            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("#include <vector>");
            w.WriteLine("#include <unordered_map>");
            w.WriteLine("#include \"../Types/SE.h\"");
            w.WriteLine("#include \"../Types/SS.h\"");
            w.WriteLine("#include \"./TableFileStream.h\"");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("class UTB{0}_Record", dfn.mTableName);
            w.WriteLine("{");
            w.WriteLine("public:");

            WriteBot_VariableDefinition(w, dfn, Util.Tab);
            w.WriteLine("");

            w.WriteLine("{0}void Read(UTableFileStream* fs)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteBot_ReadFunc(w, dfn, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("};");
            w.WriteLine("");


            w.WriteLine("class UTB{0}", dfn.mTableName);
            w.WriteLine("{");
            w.WriteLine("public:");

            WriteBot_RecordVariable(w, dfn, Util.Tab);
            w.WriteLine("");


            w.WriteLine("{0}bool Load()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteBot_LoadFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}void Unload()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteBot_UnloadFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            WriteBot_FindFunc(w, dfn, Util.Tab);

            w.WriteLine("};");


            Util.FlushAndReleaseTextFile(w);
        }

        static private void WriteBot_VariableDefinition(StreamWriter w, Defines dfn, string tab)
        {
            for (int i = 0; i < dfn.mElements.Count; ++i)
            {
                Defines.Element elem = dfn.mElements[i];

                if (elem.mUsage != Defines.Usage.Client && elem.mUsage != Defines.Usage.Both)
                {
                    continue;
                }

                w.WriteLine("{0}{1} {2};", tab, elem.mDataTypeDef_S, elem.mFieldName);
            }
        }

        static private void WriteBot_ReadFunc(StreamWriter w, Defines dfn, string tab)
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
                    case "string": WriteBot_ReadFunc_String(w, elem, tab); break;
                    case "name": WriteBot_ReadFunc_String(w, elem, tab); break;
                    case "json2": WriteBot_ReadFunc_Json2(w, elem, tab); break;
                    case "struct": WriteBot_ReadFunc_Struct(w, dfn, elem, tab); break;
                    default: WriteBot_ReadFunc_Default(w, elem, tab); break;
                }
            }
        }

        static private void WriteBot_ReadFunc_String(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                w.WriteLine("{0}fs->Read(&snt, 2);", tab);

                w.WriteLine("{0}if (snt > 0)", tab);
                w.WriteLine("{0}{{", tab);

                w.WriteLine("{0}for (int32 i = 0; i < snt; ++i)", tab + Util.Tab);
                w.WriteLine("{0}{{", tab + Util.Tab);

                w.WriteLine("{0}std::wstring {1}_Str;", tab + Util.Tab + Util.Tab, elem.mFieldName);
                w.WriteLine("{0}fs->Read(&cnt, 2);", tab + Util.Tab + Util.Tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1}_Str.resize(cnt); fs->Read(&{2}_Str[0], 2 * cnt); }}", tab + Util.Tab + Util.Tab, elem.mFieldName, elem.mFieldName);
                w.WriteLine("{0}{1}.push_back({2}_Str);", tab + Util.Tab + Util.Tab, elem.mFieldName, elem.mFieldName);

                w.WriteLine("{0}}}", tab + Util.Tab);
                w.WriteLine("{0}}}", tab);
            }
            else
            {
                w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1}.resize(cnt); fs->Read(&{2}[0], 2 * cnt); }}", tab, elem.mFieldName, elem.mFieldName);
            }

            //w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
            //w.WriteLine("{0}if (cnt > 0) {{ {1}.resize(cnt); fs->Read(&{2}[0], 2 * cnt); }}", tab, elem.mFieldName, elem.mFieldName);
        }

        static private void WriteBot_ReadFunc_Json2(StreamWriter w, Defines.Element elem, string tab)
        {
            w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
            w.WriteLine("{0}for (int i = 0; i < cnt; ++i) {{ fs->Read(&k, 1); fs->Read(&v, 4); {1}.insert({{ ({2})k, v }}); }}"
                , tab, elem.mFieldName, elem.mValueType);
        }

        static private void WriteBot_ReadFunc_Struct(StreamWriter w, Defines dfn, Defines.Element elem, string tab)
        {
            Structs.Info st = Structs.Get(elem.mValueType);
            if (st.mContainString == true)
            {
                if (elem.mArray == true)
                {
                    w.WriteLine("{0}fs->Read(&snt, 2);", tab);
                    w.WriteLine("{0}if (snt > 0) {{ {1}.resize(snt); }}", tab, elem.mFieldName);
                    w.WriteLine("{0}for (int i = 0; i < snt; ++i)", tab);
                    w.WriteLine("{0}{{", tab);

                    for (int i = 0; i < st.mValueTypeKV.Count; ++i)
                    {
                        Tuple<string, string, int> tup = st.mValueTypeKV[i];
                        if (tup.Item2 == "string")
                        {
                            w.WriteLine("{0}fs->Read(&cnt, 2);", tab + Util.Tab);
                            w.WriteLine("{0}if (cnt > 0) {{ {1}[i].{2}.resize(cnt); fs->Read(&{3}[i].{4}[0], 2 * cnt); }}"
                                , tab + Util.Tab, elem.mFieldName, tup.Item1, elem.mFieldName, tup.Item1);
                        }
                        else
                        {
                            w.WriteLine("{0}fs->Read(&{1}[i].{2}, {3});"
                                , tab + Util.Tab, elem.mFieldName, tup.Item1, Defines.Element.GetSize(tup.Item2, tup.Item2));
                        }
                    }

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
                            w.WriteLine("{0}if (cnt > 0) {{ {1}.{2}.resize(cnt); fs->Read(&{3}.{4}[0], 2 * cnt); }}"
                                , tab, elem.mFieldName, tup.Item1, elem.mFieldName, tup.Item1);
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
                WriteBot_ReadFunc_Default(w, elem, tab);
            }
        }

        static private void WriteBot_ReadFunc_Default(StreamWriter w, Defines.Element elem, string tab)
        {
            if (elem.mArray == true)
            {
                w.WriteLine("{0}fs->Read(&cnt, 2);", tab);
                w.WriteLine("{0}if (cnt > 0) {{ {1}.resize(cnt); fs->Read(&{2}[0], cnt * {3}); }}", tab, elem.mFieldName, elem.mFieldName, elem.mTypeSize);
            }
            else
            {
                w.WriteLine("{0}fs->Read(&{1}, {2});", tab, elem.mFieldName, elem.mTypeSize);
            }
        }

        static private void WriteBot_RecordVariable(StreamWriter w, Defines dfn, string tab)
        {
            if (dfn.mMultiKeyElement != null)
            {
                if (dfn.mSingleKeyElement != null)
                {
                    w.WriteLine("{0}std::unordered_map<{1}, std::unordered_map<{2}, UTB{3}_Record*>> multimapTable;"
                        , tab, dfn.mMultiKeyElement.mDataTypeDef_S, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
                }
                else
                {
                    w.WriteLine("{0}std::unordered_map<{1}, std::vector<UTB{2}_Record*>> multimapTable;", tab, dfn.mMultiKeyElement.mDataTypeDef_S, dfn.mTableName);
                }
            }
            else if (dfn.mSingleKeyElement != null)
            {
                w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*> mapTable;", tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
            }
            else
            {
                w.WriteLine("{0}std::vector<UTB{1}_Record*> vecTable;", tab, dfn.mTableName);
            }
        }

        static private void WriteBot_LoadFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}UTableFileStream fs(\"Table/{1}.txt\");", tab, dfn.mTableName);
            w.WriteLine("");

            w.WriteLine("{0}if (fs.is.is_open() == false || fs.totalRowCount <= 0)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}return false;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            if (dfn.mMultiKeyElement == null && dfn.mSingleKeyElement == null)
            {
                w.WriteLine("{0}vecTable.resize(fs.totalRowCount);", tab);
                w.WriteLine("");
            }

            w.WriteLine("{0}for (int i = 0; i < fs.totalRowCount; ++i)", tab);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}UTB{1}_Record* rec = new UTB{2}_Record();", tab + Util.Tab, dfn.mTableName, dfn.mTableName);
            w.WriteLine("");
            w.WriteLine("{0}rec->Read(&fs);", tab + Util.Tab);
            w.WriteLine("");

            if (dfn.mMultiKeyElement != null)
            {
                //확인하기
                if (dfn.mSingleKeyElement != null)
                {
                    w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*>* pmapf = Find(rec->{3});"
                        , tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName, dfn.mMultiKeyElement.mFieldName);
                    w.WriteLine("{0}if (pmapf == nullptr)", tab + Util.Tab);
                    w.WriteLine("{0}{{", tab + Util.Tab);
                    w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*> mapf;", tab + Util.Tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
                    w.WriteLine("{0}mapf.insert({{ rec->{1}, rec }});", tab + Util.Tab + Util.Tab, dfn.mSingleKeyElement.mFieldName);
                    w.WriteLine("{0}multimapTable.insert({{ rec->{1}, mapf }});", tab + Util.Tab + Util.Tab, dfn.mMultiKeyElement.mFieldName);
                    w.WriteLine("{0}}}", tab + Util.Tab);
                    w.WriteLine("{0}else", tab + Util.Tab);
                    w.WriteLine("{0}{{", tab + Util.Tab);
                    w.WriteLine("{0}pmapf->insert({{ rec->{1}, rec }});", tab + Util.Tab + Util.Tab, dfn.mSingleKeyElement.mFieldName);
                    w.WriteLine("{0}}}", tab + Util.Tab);
                }
                else
                {
                    w.WriteLine("{0}std::vector<UTB{1}_Record*>* pvec = Find(rec->{2});", tab + Util.Tab, dfn.mTableName, dfn.mMultiKeyElement.mFieldName);
                    w.WriteLine("{0}if (pvec == nullptr)", tab + Util.Tab);
                    w.WriteLine("{0}{{", tab + Util.Tab);
                    w.WriteLine("{0}std::vector<UTB{1}_Record*> vec;", tab + Util.Tab + Util.Tab, dfn.mTableName);
                    w.WriteLine("{0}vec.push_back(rec);", tab + Util.Tab + Util.Tab);
                    w.WriteLine("{0}multimapTable.insert({{ rec->{1}, vec }});", tab + Util.Tab + Util.Tab, dfn.mMultiKeyElement.mFieldName);
                    w.WriteLine("{0}}}", tab + Util.Tab);
                    w.WriteLine("{0}else", tab + Util.Tab);
                    w.WriteLine("{0}{{", tab + Util.Tab);
                    w.WriteLine("{0}pvec->push_back(rec);", tab + Util.Tab + Util.Tab);
                    w.WriteLine("{0}}}", tab + Util.Tab);
                }
            }
            else if (dfn.mSingleKeyElement != null)
            {
                w.WriteLine("{0}mapTable.insert({{ rec->{1}, rec }});", tab + Util.Tab, dfn.mSingleKeyElement.mFieldName);
            }
            else
            {
                w.WriteLine("{0}vecTable[i] = rec;", tab + Util.Tab);
            }


            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            w.WriteLine("{0}return true;", tab);
        }

        static private void WriteBot_UnloadFunc(StreamWriter w, Defines dfn, string tab)
        {
            if (dfn.mMultiKeyElement != null)
            {
                if (dfn.mSingleKeyElement != null)
                {
                    WriteBot_UnloadFunc_Type3(w, dfn, tab);
                }
                else
                {
                    WriteBot_UnloadFunc_Type2(w, dfn, tab);
                }
            }
            else
            {
                if (dfn.mSingleKeyElement != null)
                {
                    WriteBot_UnloadFunc_Type1(w, dfn, tab);
                }
                else
                {
                    WriteBot_UnloadFunc_Type0(w, dfn, tab);
                }
            }
        }
        static private void WriteBot_UnloadFunc_Type3(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}std::unordered_map<{1}, std::unordered_map<{2}, UTB{3}_Record*>>::iterator itr_m = multimapTable.begin();", tab, dfn.mMultiKeyElement.mDataTypeDef_S, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}for (; itr_m != multimapTable.end(); itr_m++)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*>::iterator itr_s = itr_m->second.begin();", tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}for (; itr_s != itr_m->second.end(); itr_s++)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}SAFE_DELETE(itr_s->second);", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}itr_m->second.clear();", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("{0}multimapTable.clear();", tab);
        }
        static private void WriteBot_UnloadFunc_Type2(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}std::unordered_map<{1}, std::vector<UTB{2}_Record*>>::iterator itr_m = multimapTable.begin();", tab, dfn.mMultiKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}for (; itr_m != multimapTable.end(); itr_m++)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}std::vector<UTB{1}_Record*>::iterator itr_s = itr_m->second.begin();", tab + Util.Tab, dfn.mTableName);
            w.WriteLine("{0}for (; itr_s != itr_m->second.end(); itr_s++)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}SAFE_DELETE(*itr_s);", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}itr_m->second.clear();", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("{0}multimapTable.clear();", tab);
        }
        static private void WriteBot_UnloadFunc_Type1(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*>::iterator itr = mapTable.begin();", tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}for (; itr != mapTable.end(); itr++)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}SAFE_DELETE(itr->second);", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("{0}mapTable.clear();", tab);
        }
        static private void WriteBot_UnloadFunc_Type0(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}std::vector<UTB{1}_Record*>::iterator itr = vecTable.begin();", tab, dfn.mTableName);
            w.WriteLine("{0}for (; itr != vecTable.end(); itr++)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}SAFE_DELETE(*itr);", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("{0}vecTable.clear();", tab);
        }

        static private void WriteBot_FindFunc(StreamWriter w, Defines dfn, string tab)
        {
            if (dfn.mMultiKeyElement != null)
            {
                if (dfn.mSingleKeyElement != null)
                {
                    WriteBot_FindFunc_Type2(w, dfn, tab);
                }
                else
                {
                    WriteBot_FindFunc_Type1(w, dfn, tab);
                }
            }
            else if (dfn.mSingleKeyElement != null)
            {
                WriteBot_FindFunc_Type0(w, dfn, tab);
            }
        }
        static private void WriteBot_FindFunc_Type2(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("");
            w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*>* Find({3} key)", tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName
                , dfn.mMultiKeyElement.mDataType == "string" ? "const std::wstring&" : dfn.mMultiKeyElement.mDataTypeDef_S);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}std::unordered_map<{1}, std::unordered_map<{2}, UTB{3}_Record*>>::iterator itr = multimapTable.find(key);"
                , tab + Util.Tab, dfn.mMultiKeyElement.mDataTypeDef_S, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}if (itr == multimapTable.end())", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return &itr->second;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);

            w.WriteLine("");
            w.WriteLine("{0}UTB{1}_Record* Find({2} key1, {3} key2)", tab, dfn.mTableName
                , dfn.mMultiKeyElement.mDataType == "string" ? "const std::wstring&" : dfn.mMultiKeyElement.mDataTypeDef_S
                , dfn.mSingleKeyElement.mDataType == "string" ? "const std::wstring&" : dfn.mSingleKeyElement.mDataTypeDef_S);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*>* pmapf = Find(key1);", tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}if (pmapf == nullptr)", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*>::iterator iter = pmapf->find(key2);", tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}if (iter == pmapf->end())", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return iter->second;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
        }
        static private void WriteBot_FindFunc_Type1(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("");
            w.WriteLine("{0}std::vector<UTB{1}_Record*>* Find({2} key)", tab, dfn.mTableName
                , dfn.mMultiKeyElement.mDataType == "string" ? "const std::wstring&" : dfn.mMultiKeyElement.mDataTypeDef_S);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}std::unordered_map<{1}, std::vector<UTB{2}_Record*>>::iterator itr = multimapTable.find(key);", tab + Util.Tab, dfn.mMultiKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}if (itr == multimapTable.end())", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return &itr->second;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
        }
        static private void WriteBot_FindFunc_Type0(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("");
            w.WriteLine("{0}UTB{1}_Record* Find({2} key)", tab, dfn.mTableName
                , dfn.mSingleKeyElement.mDataType == "string" ? "const std::wstring&" : dfn.mSingleKeyElement.mDataTypeDef_S);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}std::unordered_map<{1}, UTB{2}_Record*>::iterator itr = mapTable.find(key);", tab + Util.Tab, dfn.mSingleKeyElement.mDataTypeDef_S, dfn.mTableName);
            w.WriteLine("{0}if (itr == mapTable.end())", tab + Util.Tab);
            w.WriteLine("{0}{{", tab + Util.Tab);
            w.WriteLine("{0}return nullptr;", tab + Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", tab + Util.Tab);
            w.WriteLine("{0}return itr->second;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
        }
    }
}
