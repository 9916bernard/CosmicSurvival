using System;
using System.Collections.Generic;



namespace PacketCodeMaker
{
    public class EnumStructInfos
    {
        public class EnumInfo
        {
            public string mName = "";
            public string mType = "";
            public int mSize = 4;
            public bool mIsJsonEnum = false;
            public List<Tuple<string, int, string>> mValueTypeKV = new List<Tuple<string, int, string>>();

            public bool Add(string k, string v, string v2 = "")
            {
                if (mValueTypeKV.Find(x => x.Item1.CompareTo(k) == 0) != null)
                {
                    return false;
                }

                if (v2 != "")
                {
                    mIsJsonEnum = true;
                }

                mValueTypeKV.Add(new Tuple<string, int, string>(k, Convert.ToInt32(v), v2));

                return true;
            }

            public void SetSize()
            {
                switch (mType)
                {
                    case "int8": case "uint8": mSize = 1; break;
                    case "int16": case "uint16": mSize = 2; break;
                }
            }

            public int GetValue(string enumName)
            {
                Tuple<string, int, string> tup = mValueTypeKV.Find(x => x.Item1.CompareTo(enumName) == 0);

                if (tup == null)
                {
                    return 0;
                }

                return tup.Item2;
            }

            public Tuple<string, int, string> GetValueTuple(string enumName)
            {
                Tuple<string, int, string> tup = mValueTypeKV.Find(x => x.Item1.CompareTo(enumName) == 0);

                return tup;
            }
        }

        public class StructInfo
        {
            public string mName = "";
            public int mTotalSize = 0;
            public List<Tuple<string, string, int>> mValueTypeKV = new List<Tuple<string, string, int>>();

            public bool Add(string k, string v1, string v2)
            {
                if (mValueTypeKV.Find(x => x.Item1.CompareTo(k) == 0) != null)
                {
                    return false;
                }

                mValueTypeKV.Add(new Tuple<string, string, int>(k, v1, v2 == "" ? 0 : Convert.ToInt32(v2)));

                return true;
            }

            public void SetTotalSize()
            {
                for (int i = 0; i < mValueTypeKV.Count; ++i)
                {
                    mTotalSize += GetSize(mValueTypeKV[i].Item2);
                }
            }

            public int GetSize(string type)
            {
                switch (type)
                {
                    case "int8": case "uint8": case "bool": return 1;
                    case "int16": case "uint16": return 2;
                    case "int32": case "uint32": case "float": return 4;
                }
                return 0;
            }
        }


        public List<EnumInfo> mEnumInfos = new List<EnumInfo>();
        public List<StructInfo> mStructInfos = new List<StructInfo>();

        public bool AddEnum(EnumInfo enuminfo)
        {
            if (mEnumInfos.Find(x => x.mName.CompareTo(enuminfo.mName) == 0) != null)
            {
                return false;
            }

            mEnumInfos.Add(enuminfo);

            return true;
        }

        public EnumInfo GetEnum(string enumName)
        {
            return mEnumInfos.Find(x => x.mName.CompareTo(enumName) == 0);
        }

        public bool AddStruct(StructInfo structinfo)
        {
            if (mStructInfos.Find(x => x.mName.CompareTo(structinfo.mName) == 0) != null)
            {
                return false;
            }

            structinfo.SetTotalSize();

            mStructInfos.Add(structinfo);

            return true;
        }

        public StructInfo GetStruct(string structName)
        {
            return mStructInfos.Find(x => x.mName.CompareTo(structName) == 0);
        }
    }
}
