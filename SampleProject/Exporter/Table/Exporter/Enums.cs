using System;
using System.Collections.Generic;


namespace Table.Exporter
{
    class Enums
    {
        public class Info
        {
            public string mName = "";
            public string mType = "";
            public int mSize = 4;
            public bool mIsJsonEnum = false;
            public List<Tuple<string, int, string>> mValueTypeKV = new List<Tuple<string, int, string>>();

            public Info(string ename, string etype, bool isJson)
            {
                mName = ename;
                mType = etype;

                mSize = Types.GetSize(mType);

                mIsJsonEnum = isJson;
            }

            public void Add(string k, string v1, string v2)
            {
                if (mValueTypeKV.Find(x => x.Item1.CompareTo(k.Trim()) == 0) != null)
                {
                    Log.E("Enum element is duplicate : {0}, {1}, {2}", mName, k, v1);
                    return;
                }

                mValueTypeKV.Add(new Tuple<string, int, string>(k, Convert.ToInt32(v1), v2));
            }

            public int GetValue(string enumName)
            {
                string enumNameTrim = enumName.Trim();
                if (enumNameTrim == "")
                {
                    return 0;
                }

                Tuple<string, int, string> tup = mValueTypeKV.Find(x => x.Item1.CompareTo(enumNameTrim) == 0);

                if (tup == null)
                {
                    //TableMaker.Log(string.Format("[ERROR] EnumInfo.GetValue failed : {0}, {1}", mName, enumNameTrim));
                    return 0;
                }

                return tup.Item2;
            }

            public Tuple<string, int, string> GetValueTuple(string enumName)
            {
                string enumNameTrim = enumName.Trim();

                Tuple<string, int, string> tup = mValueTypeKV.Find(x => x.Item1.CompareTo(enumNameTrim) == 0);

                return tup;
            }
        }

        static public List<Info> InfoList = new List<Info>();

        static public void Clear()
        {
            InfoList.Clear();
        }

        static public void Add(Info info)
        {
            if (InfoList.Find(x => x.mName.CompareTo(info.mName) == 0) != null)
            {
                Log.E("duplicate enum name : {0}", info.mName);
            }
            else
            {
                InfoList.Add(info);
            }
        }

        static public Info Get(string enumName)
        {
            Info r = InfoList.Find(x => x.mName.CompareTo(enumName) == 0);

            if (r == null)
            {
                Log.E("not found enum name : {0}", enumName);
            }

            return r;
        }
    }
}
