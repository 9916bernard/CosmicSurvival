using System;
using System.IO;


namespace Table.Exporter
{
    partial class TBL_Config
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

            w.WriteLine("class UTB{0}", dfn.mTableName);
            w.WriteLine("{");
            w.WriteLine("public:");

            WriteBot_VariableDefinition(w, dfn, Util.Tab);
            w.WriteLine("");

            w.WriteLine("{0}void Read(UTableFileStream* fs)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteBot_ReadFunc(w, dfn, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}bool Load()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            WriteBot_LoadFunc(w, dfn, Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

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

        static private void WriteBot_LoadFunc(StreamWriter w, Defines dfn, string tab)
        {
            w.WriteLine("{0}UTableFileStream fs(\"Table/{1}.txt\");", tab, dfn.mTableName);
            w.WriteLine("");

            w.WriteLine("{0}if (fs.is.is_open() == false)", tab);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}return false;", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            w.WriteLine("{0}Read(&fs);", tab);

            w.WriteLine("");

            w.WriteLine("{0}return true;", tab);
        }
    }
}
