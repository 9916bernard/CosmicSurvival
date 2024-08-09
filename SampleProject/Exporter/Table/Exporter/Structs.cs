using System;
using System.Collections.Generic;


namespace Table.Exporter
{
    class Structs
    {
        public class Info
        {
            public string mName = "";
            public int mTotalSize = 0;
            public List<Tuple<string, string, int>> mValueTypeKV = new List<Tuple<string, string, int>>();
            public bool mContainString = false;

            public void Add(string k, string v1, string v2)
            {
                if (mValueTypeKV.Find(x => x.Item1.CompareTo(k) == 0) != null)
                {
                    Log.E("Struct element is duplicate : {0}, {1}, {2}", mName, k, v1);
                    return;
                }

                if (v1 == "string")
                {
                    mContainString = true;
                }

                mValueTypeKV.Add(new Tuple<string, string, int>(k, v1, v2 == "" ? 0 : Convert.ToInt32(v2)));
            }

            public void SetTotalSize()
            {
                mTotalSize = 0;

                for (int i = 0; i < mValueTypeKV.Count; ++i)
                {
                    mTotalSize += Types.GetSize(mValueTypeKV[i].Item2);
                }
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
                Log.E("duplicate struct name : {0}", info.mName);
            }
            else
            {
                info.SetTotalSize();

                InfoList.Add(info);
            }
        }

        static public Info Get(string structName)
        {
            Info r = InfoList.Find(x => x.mName.CompareTo(structName) == 0);

            if (r == null)
            {
                Log.E("not found struct name : {0}", structName);
            }

            return r;
        }
    }
}
