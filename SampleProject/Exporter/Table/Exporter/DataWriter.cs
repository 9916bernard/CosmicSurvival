using System;
using System.IO;
using System.Collections.Generic;


namespace Table.Exporter
{
    partial class DataWriter
    {
        static Dictionary<string, Func<BinaryWriter, Defines.Element, string, int>> mDicTypeFunc_Data = null;

        static public int WriteData(BinaryWriter fs, Defines.Element elem, string cell)
        {
            Types_CheckFunc();

            Func<BinaryWriter, Defines.Element, string, int> func = null;
            if (true == mDicTypeFunc_Data.TryGetValue(elem.mDataType, out func))
            {
                return func(fs, elem, cell);
            }

            Log.E("DataType is Invalid Type:{0}", elem.mDataType);
            return 0;
        }

        static public void Types_CheckFunc()
        {
            if (null != mDicTypeFunc_Data)
                return;

            mDicTypeFunc_Data = new Dictionary<string, Func<BinaryWriter, Defines.Element, string, int>>();

            mDicTypeFunc_Data.Add("bool", Types_Func_UInt8);

            mDicTypeFunc_Data.Add("int8", Types_Func_Int8);
            mDicTypeFunc_Data.Add("uint8", Types_Func_UInt8);
            mDicTypeFunc_Data.Add("int16", Types_Func_Int16);
            mDicTypeFunc_Data.Add("uint16", Types_Func_UInt16);
            mDicTypeFunc_Data.Add("int32", Types_Func_Int32);
            mDicTypeFunc_Data.Add("uint32", Types_Func_UInt32);
            mDicTypeFunc_Data.Add("int64", Types_Func_Int64);
            mDicTypeFunc_Data.Add("uint64", Types_Func_UInt64);

            mDicTypeFunc_Data.Add("float", Types_Func_Single);
            mDicTypeFunc_Data.Add("vector2", Types_Func_Vector2);
            mDicTypeFunc_Data.Add("vector3", Types_Func_Vector3);
            mDicTypeFunc_Data.Add("color", Types_Func_Color);
            mDicTypeFunc_Data.Add("enum", Types_Func_Enum);
            mDicTypeFunc_Data.Add("struct", Types_Func_Struct);
            mDicTypeFunc_Data.Add("json2", Types_Func_Json2);
            mDicTypeFunc_Data.Add("string", Types_Func_String);
            mDicTypeFunc_Data.Add("name", Types_Func_String);
        }

        static public int Types_Func_Int8(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 1;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToSByte(v[i].Trim()));

                    write_size = v.Length * 1;
                }
            }
            else
            {
                fs.Write(cell == "" ? (sbyte)0 : Convert.ToSByte(cell));
            }

            return write_size;
        }

        static public int Types_Func_UInt8(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 1;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToByte(v[i].Trim()));

                    write_size = v.Length * 1;
                }
            }
            else
            {
                fs.Write(cell == "" ? (byte)0 : Convert.ToByte(cell));
            }

            return write_size;
        }

        static public int Types_Func_Int16(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 2;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToInt16(v[i].Trim()));

                    write_size = v.Length * 2;
                }
            }
            else
            {
                fs.Write(cell == "" ? (short)0 : Convert.ToInt16(cell));
            }

            return write_size;
        }

        static public int Types_Func_UInt16(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 2;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToUInt16(v[i].Trim()));

                    write_size = v.Length * 2;
                }
            }
            else
            {
                fs.Write(cell == "" ? (ushort)0 : Convert.ToUInt16(cell));
            }

            return write_size;
        }

        static public int Types_Func_Int32(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 4;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToInt32(v[i].Trim()));

                    write_size = v.Length * 4;
                }
            }
            else
            {
                fs.Write(cell == "" ? (int)0 : Convert.ToInt32(cell));
            }

            return write_size;
        }

        static public int Types_Func_UInt32(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 4;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToUInt32(v[i].Trim()));

                    write_size = v.Length * 4;
                }
            }
            else
            {
                fs.Write(cell == "" ? (uint)0 : Convert.ToUInt32(cell));
            }

            return write_size;
        }

        static public int Types_Func_Int64(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 8;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToInt64(v[i].Trim()));

                    write_size = v.Length * 8;
                }
            }
            else
            {
                fs.Write(cell == "" ? (long)0 : Convert.ToInt64(cell));
            }

            return write_size;
        }

        static public int Types_Func_UInt64(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 8;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToUInt64(v[i].Trim()));

                    write_size = v.Length * 8;
                }
            }
            else
            {
                fs.Write(cell == "" ? (ulong)0 : Convert.ToUInt64(cell));
            }

            return write_size;
        }

        static public int Types_Func_Single(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 4;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.Write(Convert.ToSingle(v[i].Trim()));

                    write_size = v.Length * 4;
                }
            }
            else
            {
                fs.Write(cell == "" ? 0.0f : Convert.ToSingle(cell));
            }

            return write_size;
        }

        static public int Types_Func_Vector2(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 8;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.WriteVector2(v[i].Trim());

                    write_size = v.Length * 8;
                }
            }
            else
            {
                fs.WriteVector2(cell);
            }

            return write_size;
        }

        static public int Types_Func_Vector3(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 12;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.WriteVector3(v[i].Trim());

                    write_size = v.Length * 12;
                }
            }
            else
            {
                fs.WriteVector3(cell);
            }

            return write_size;
        }

        static public int Types_Func_Color(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 16;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.WriteColor(v[i].Trim());

                    write_size = v.Length * 16;
                }
            }
            else
            {
                fs.WriteColor(cell);
            }

            return write_size;
        }

        static public int Types_Func_Enum(BinaryWriter fs, Defines.Element elem, string cell)
        {
            Enums.Info enuminfo = Enums.Get(elem.mValueType);

            if (enuminfo == null)
            {
                Log.E("[Types_Func_Enum] enum not found : {0}", elem.mValueType);
                return 0;
            }

            int write_size = enuminfo.mSize;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                        fs.WriteEnum(enuminfo.mSize, enuminfo.GetValue(v[i]));

                    write_size = v.Length * enuminfo.mSize;
                }
            }
            else
            {
                fs.WriteEnum(enuminfo.mSize, enuminfo.GetValue(cell));
            }

            return write_size;
        }

        static public int Types_Func_Struct(BinaryWriter fs, Defines.Element elem, string cell)
        {
            Structs.Info structinfo = Structs.Get(elem.mValueType);

            if (structinfo == null)
            {
                Log.E("[Types_Func_Struct] struct not found : {0}", elem.mValueType);
                return 0;
            }

            int write_size = structinfo.mTotalSize;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = cell.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                    {
                        string[] vv = v[i].Split(';');
                        if (structinfo.mValueTypeKV.Count > vv.Length)
                        {
                            Log.E("[Types_Func_Struct] invalid struct : {0}", cell);
                            return 0;
                        }

                        for (int j = 0; j < structinfo.mValueTypeKV.Count; ++j)
                        {
                            if (fs.WriteByType(structinfo.mValueTypeKV[j].Item2, vv[j].Trim()) == 0)
                            {
                                Enums.Info enuminfo = Enums.Get(structinfo.mValueTypeKV[j].Item2);
                                if (enuminfo != null)
                                    fs.WriteEnum(enuminfo.mSize, enuminfo.GetValue(vv[j].Trim()));
                            }
                        }
                    }

                    write_size = v.Length * structinfo.mTotalSize;
                }
            }
            else
            {
                if (cell == "")
                {
                    for (int j = 0; j < structinfo.mValueTypeKV.Count; ++j)
                    {
                        if (fs.WriteByType(structinfo.mValueTypeKV[j].Item2, "") == 0)
                        {
                            Enums.Info enuminfo = Enums.Get(structinfo.mValueTypeKV[j].Item2);
                            if (enuminfo != null)
                                fs.WriteEnum(enuminfo.mSize, enuminfo.GetValue("0"));
                        }
                    }
                }
                else
                {
                    string[] vv = cell.Split(';');
                    if (structinfo.mValueTypeKV.Count > vv.Length)
                    {
                        Log.E("[Types_Func_Struct] invalid struct : {0}", cell);
                        return 0;
                    }

                    for (int j = 0; j < structinfo.mValueTypeKV.Count; ++j)
                    {
                        if (fs.WriteByType(structinfo.mValueTypeKV[j].Item2, vv[j]) == 0)
                        {
                            Enums.Info enuminfo = Enums.Get(structinfo.mValueTypeKV[j].Item2);
                            if (enuminfo != null)
                                fs.WriteEnum(enuminfo.mSize, enuminfo.GetValue(vv[j].Trim()));
                        }
                    }
                }
            }

            return write_size;
        }

        static public int Types_Func_Json2(BinaryWriter fs, Defines.Element elem, string cell)
        {
            if (cell == "")
            {
                fs.Write((ushort)0);
                return 0;
            }

            string[] vs = cell.Split('|');

            fs.Write((ushort)vs.Length);

            Enums.Info einfo = Enums.Get(elem.mValueType);
            if (einfo == null || einfo.mIsJsonEnum == false)
            {
                Log.E("[Types_Func_Struct] json2 not found : {0} ({1})", elem.mValueType, einfo.mIsJsonEnum);
                return 0;
            }

            for (int i = 0; i < vs.Length; ++i)
            {
                string[] v = vs[i].Split('=');
                if (v.Length < 2)
                {
                    // error
                    continue;
                }

                Tuple<string, int, string> tup = einfo.GetValueTuple(v[0].Trim());
                if (tup == null)
                {
                    // error
                    fs.Write((byte)0);
                    fs.Write((int)0);
                    continue;
                }

                fs.Write((byte)tup.Item2);

                if (tup.Item3 == "float")
                {
                    fs.Write(Convert.ToSingle(v[1].Trim()));
                }
                else
                {
                    fs.Write(Convert.ToInt32(v[1].Trim()));
                }
            }

            return 4;
        }

        static public int Types_Func_String(BinaryWriter fs, Defines.Element elem, string cell)
        {
            int write_size = 0;

            string wdata = cell;

            if (elem.mArray == true)
            {
                if (cell == "")
                {
                    fs.Write((ushort)0);
                    write_size = 0;
                }
                else
                {
                    string[] v = wdata.Split('|');

                    fs.Write((ushort)v.Length);

                    for (int i = 0; i < v.Length; ++i)
                    {
                        int wlen = fs.WriteString(v[i].Trim());
                        if (write_size < wlen)
                            write_size = wlen;
                    }
                }
            }
            else
            {
                write_size = fs.WriteString(wdata);
            }

            return write_size;
        }
    }
}
