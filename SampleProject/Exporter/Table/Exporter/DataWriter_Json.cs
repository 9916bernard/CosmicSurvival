using System;
using System.IO;
using System.Collections.Generic;


namespace Table.Exporter
{
    partial class DataWriter_Json
    {
        static Dictionary<string, Action<Dictionary<object, object>, Defines.Element, string>> mDicTypeAction_Data = null;

        static public void WriteData(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            Types_CheckFunc();

            Action<Dictionary<object, object>, Defines.Element, string> action = null;
            if (true == mDicTypeAction_Data.TryGetValue(elem.mDataType, out action))
            {
                action(jo, elem, cell);
            }
            else
            {
                Log.E("DataType is Invalid Type:{0}", elem.mDataType);
            }
        }

        static public void Types_CheckFunc()
        {
            if (null != mDicTypeAction_Data)
                return;

            mDicTypeAction_Data = new Dictionary<string, Action<Dictionary<object, object>, Defines.Element, string>>();

            mDicTypeAction_Data.Add("int8", Types_Func_Int8);
            mDicTypeAction_Data.Add("uint8", Types_Func_UInt8);
            mDicTypeAction_Data.Add("int16", Types_Func_Int16);
            mDicTypeAction_Data.Add("uint16", Types_Func_UInt16);
            mDicTypeAction_Data.Add("int32", Types_Func_Int32);
            mDicTypeAction_Data.Add("uint32", Types_Func_UInt32);
            mDicTypeAction_Data.Add("int64", Types_Func_Int64);
            mDicTypeAction_Data.Add("uint64", Types_Func_UInt64);

            mDicTypeAction_Data.Add("float", Types_Func_Single);
            mDicTypeAction_Data.Add("vector2", Types_Func_Vector2);
            mDicTypeAction_Data.Add("vector3", Types_Func_Vector3);
            mDicTypeAction_Data.Add("color", Types_Func_Color);
            mDicTypeAction_Data.Add("enum", Types_Func_Enum);
            mDicTypeAction_Data.Add("struct", Types_Func_Struct);
            mDicTypeAction_Data.Add("string", Types_Func_String);
        }

        static public void Types_Func_Int8(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<sbyte> dataList = new List<sbyte>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToSByte(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (sbyte)0 : Convert.ToSByte(cell));
            }
        }

        static public void Types_Func_UInt8(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<byte> dataList = new List<byte>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToByte(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (byte)0 : Convert.ToByte(cell));
            }
        }

        static public void Types_Func_Int16(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<short> dataList = new List<short>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToInt16(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (short)0 : Convert.ToInt16(cell));
            }
        }

        static public void Types_Func_UInt16(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<ushort> dataList = new List<ushort>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToUInt16(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (ushort)0 : Convert.ToUInt16(cell));
            }
        }

        static public void Types_Func_Int32(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<int> dataList = new List<int>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToInt32(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (int)0 : Convert.ToInt32(cell));
            }
        }

        static public void Types_Func_UInt32(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<uint> dataList = new List<uint>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToUInt32(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (uint)0 : Convert.ToUInt32(cell));
            }
        }

        static public void Types_Func_Int64(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<long> dataList = new List<long>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToInt64(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (long)0 : Convert.ToInt64(cell));
            }
        }

        static public void Types_Func_UInt64(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<ulong> dataList = new List<ulong>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToUInt64(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (ulong)0 : Convert.ToUInt64(cell));
            }
        }

        static public void Types_Func_Single(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<float> dataList = new List<float>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(Convert.ToSingle(v[i].Trim()));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? 0.0f : Convert.ToSingle(cell));
            }
        }

        static public void Types_Func_Vector2(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<object> dataList = new List<object>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                    {
                        Dictionary<string, float> dataDic = new Dictionary<string, float>();
                        string[] vecs = v[i].Trim().Replace("Vector2", "").Replace("(", "").Replace(")", "").Split(',');
                        if (vecs.Length > 0) { dataDic.Add("x", Convert.ToSingle(vecs[0].Trim())); } else { dataDic.Add("x", 0.0f); } // X
                        if (vecs.Length > 1) { dataDic.Add("y", Convert.ToSingle(vecs[1].Trim())); } else { dataDic.Add("y", 0.0f); } // Y
                        dataList.Add(dataDic);
                    }
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                if (cell.Length <= 0)
                {
                    cell = "Vector2(0,0)";
                }

                Dictionary<string, float> dataDic = new Dictionary<string, float>();
                string[] vecs = cell.Trim().Replace("Vector2", "").Replace("(", "").Replace(")", "").Split(',');
                if (vecs.Length > 0) { dataDic.Add("x", Convert.ToSingle(vecs[0].Trim())); } else { dataDic.Add("x", 0.0f); } // X
                if (vecs.Length > 1) { dataDic.Add("y", Convert.ToSingle(vecs[1].Trim())); } else { dataDic.Add("y", 0.0f); } // Y
                jo.Add(elem.mFieldName, dataDic);
            }
        }

        static public void Types_Func_Vector3(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<object> dataList = new List<object>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                    {
                        Dictionary<string, float> dataDic = new Dictionary<string, float>();
                        string[] vecs = v[i].Trim().Replace("Vector3", "").Replace("(", "").Replace(")", "").Split(',');
                        if (vecs.Length > 0) { dataDic.Add("x", Convert.ToSingle(vecs[0].Trim())); } else { dataDic.Add("x", 0.0f); } // X
                        if (vecs.Length > 1) { dataDic.Add("y", Convert.ToSingle(vecs[1].Trim())); } else { dataDic.Add("y", 0.0f); } // Y
                        if (vecs.Length > 2) { dataDic.Add("z", Convert.ToSingle(vecs[2].Trim())); } else { dataDic.Add("z", 0.0f); } // Z
                        dataList.Add(dataDic);
                    }
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                if (cell.Length <= 0)
                {
                    cell = "Vector3(0,0,0)";
                }

                Dictionary<string, float> dataDic = new Dictionary<string, float>();
                string[] vecs = cell.Trim().Replace("Vector3", "").Replace("(", "").Replace(")", "").Split(',');
                if (vecs.Length > 0) { dataDic.Add("x", Convert.ToSingle(vecs[0].Trim())); } else { dataDic.Add("x", 0.0f); } // X
                if (vecs.Length > 1) { dataDic.Add("y", Convert.ToSingle(vecs[1].Trim())); } else { dataDic.Add("y", 0.0f); } // Y
                if (vecs.Length > 2) { dataDic.Add("z", Convert.ToSingle(vecs[2].Trim())); } else { dataDic.Add("z", 0.0f); } // Z

                jo.Add(elem.mFieldName, dataDic);
            }
        }

        static public void Types_Func_Color(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<object> dataList = new List<object>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                    {
                        Dictionary<string, float> dataDic = new Dictionary<string, float>();
                        string trim = v[i].Trim().Trim('#').Trim();
                        if (trim.Length >= 2) { dataDic.Add("r", Extensions.ConvertHexToColor(trim.Substring(0, 2))); } else { dataDic.Add("r", 0.0f); } // R
                        if (trim.Length >= 4) { dataDic.Add("g", Extensions.ConvertHexToColor(trim.Substring(2, 2))); } else { dataDic.Add("g", 0.0f); } // G
                        if (trim.Length >= 6) { dataDic.Add("b", Extensions.ConvertHexToColor(trim.Substring(4, 2))); } else { dataDic.Add("b", 0.0f); } // B
                        if (trim.Length >= 8) { dataDic.Add("a", Extensions.ConvertHexToColor(trim.Substring(6, 2))); } else { dataDic.Add("a", 1.0f); } // A
                        dataList.Add(dataDic);
                    }
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                Dictionary<string, float> dataDic = new Dictionary<string, float>();
                string trim = cell.Trim().Trim('#').Trim();
                if (trim.Length >= 2) { dataDic.Add("r", Extensions.ConvertHexToColor(trim.Substring(0, 2))); } else { dataDic.Add("r", 0.0f); } // R
                if (trim.Length >= 4) { dataDic.Add("g", Extensions.ConvertHexToColor(trim.Substring(2, 2))); } else { dataDic.Add("g", 0.0f); } // G
                if (trim.Length >= 6) { dataDic.Add("b", Extensions.ConvertHexToColor(trim.Substring(4, 2))); } else { dataDic.Add("b", 0.0f); } // B
                if (trim.Length >= 8) { dataDic.Add("a", Extensions.ConvertHexToColor(trim.Substring(6, 2))); } else { dataDic.Add("a", 1.0f); } // A

                jo.Add(elem.mFieldName, dataDic);
            }
        }

        static public void Types_Func_Enum(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            Enums.Info enuminfo = Enums.Get(elem.mValueType);

            if (enuminfo == null)
            {
                Log.E("[Types_Func_Enum] enum not found : {0}", elem.mValueType);
                return;
            }

            if (elem.mArray == true)
            {
                List<int> dataList = new List<int>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(enuminfo.GetValue(v[i]));
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell == "" ? (int)0 : enuminfo.GetValue(cell));
            }
        }

        static public void Types_Func_Struct(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            Structs.Info structinfo = Structs.Get(elem.mValueType);

            if (structinfo == null)
            {
                Log.E("[Types_Func_Struct] struct not found : {0}", elem.mValueType);
                return;
            }

            if (elem.mArray == true)
            {
                List<object> dataList = new List<object>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                    {
                        string[] vv = v[i].Split(';');
                        if (structinfo.mValueTypeKV.Count > vv.Length)
                        {
                            Log.E("[Types_Func_Struct] invalid struct : {0}", cell);
                            return;
                        }

                        Dictionary<string, object> dataDic = new Dictionary<string, object>();

                        for (int j = 0; j < structinfo.mValueTypeKV.Count; ++j)
                        {
                            if (Extensions.WriteByType_Json(dataDic, structinfo.mValueTypeKV[j].Item1, structinfo.mValueTypeKV[j].Item2, vv[j].Trim()) == false)
                            {
                                Enums.Info enuminfo = Enums.Get(structinfo.mValueTypeKV[j].Item2);
                                if (enuminfo != null)
                                    dataDic.Add(structinfo.mValueTypeKV[j].Item1, enuminfo.GetValue(vv[j].Trim()));
                            }
                        }

                        dataList.Add(dataDic);
                    }
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                Dictionary<string, object> dataDic = new Dictionary<string, object>();

                if (cell != "")
                {
                    string[] vv = cell.Split(';');
                    if (structinfo.mValueTypeKV.Count > vv.Length)
                    {
                        Log.E("[Types_Func_Struct] invalid struct : {0}", cell);
                        return;
                    }

                    for (int j = 0; j < structinfo.mValueTypeKV.Count; ++j)
                    {
                        if (Extensions.WriteByType_Json(dataDic, structinfo.mValueTypeKV[j].Item1, structinfo.mValueTypeKV[j].Item2, vv[j].Trim()) == false)
                        {
                            Enums.Info enuminfo = Enums.Get(structinfo.mValueTypeKV[j].Item2);
                            if (enuminfo != null)
                                dataDic.Add(structinfo.mValueTypeKV[j].Item1, enuminfo.GetValue(vv[j].Trim()));
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < structinfo.mValueTypeKV.Count; ++j)
                    {
                        if (Extensions.WriteByType_Json(dataDic, structinfo.mValueTypeKV[j].Item1, structinfo.mValueTypeKV[j].Item2, "") == false)
                        {
                            Enums.Info enuminfo = Enums.Get(structinfo.mValueTypeKV[j].Item2);
                            if (enuminfo != null)
                                dataDic.Add(structinfo.mValueTypeKV[j].Item1, enuminfo.GetValue("0"));
                        }
                    }
                }

                jo.Add(elem.mFieldName, dataDic);
            }
        }

        static public void Types_Func_String(Dictionary<object, object> jo, Defines.Element elem, string cell)
        {
            if (elem.mArray == true)
            {
                List<string> dataList = new List<string>();

                if (cell != "")
                {
                    string[] v = cell.Split('|');

                    for (int i = 0; i < v.Length; ++i)
                        dataList.Add(v[i].Trim());
                }

                jo.Add(elem.mFieldName, dataList);
            }
            else
            {
                jo.Add(elem.mFieldName, cell);
            }
        }
    }
}
