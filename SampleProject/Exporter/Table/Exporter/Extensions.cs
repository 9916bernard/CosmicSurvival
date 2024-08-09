using System;
using System.IO;
using System.Text;
using System.Collections.Generic;


namespace Table.Exporter
{
    public static class Extensions
    {
        //=================================================================================
        // Stream
        //=================================================================================
        //public static void WriteInt16(this Stream fs, short data)
        //{
        //    byte[] dataArray = BitConverter.GetBytes(data);
        //    fs.Write(dataArray, 0, dataArray.Length);
        //}

        //public static void WriteInt32(this Stream fs, int data)
        //{
        //    byte[] dataArray = BitConverter.GetBytes(data);
        //    fs.Write(dataArray, 0, dataArray.Length);
        //}

        //public static void WriteInt64(this Stream fs, long data)
        //{
        //    byte[] dataArray = BitConverter.GetBytes(data);
        //    fs.Write(dataArray, 0, dataArray.Length);
        //}

        //public static void WriteSingle(this Stream fs, float data)
        //{
        //    byte[] dataArray = BitConverter.GetBytes(data);
        //    fs.Write(dataArray, 0, dataArray.Length);
        //}

        //public static int WriteString(this Stream fs, string data)
        //{
        //    byte[] dataArray = Encoding.Unicode.GetBytes(data);
        //    fs.WriteInt16((short)data.Length);
        //    if (0 < dataArray.Length)
        //        fs.Write(dataArray, 0, dataArray.Length);
        //    return dataArray.Length;
        //}

        //public static int WriteEnum(this Stream fs, int size, int data)
        //{
        //    if (size == 1) { fs.WriteByte((byte)data); }
        //    else if (size == 2) { fs.WriteInt16((short)data); }
        //    else { fs.WriteInt32(data); }
        //    return 0;
        //}

        //public static int WriteByType(this Stream fs, string type, string data)
        //{
        //    switch (type)
        //    {
        //        case "int8": case "uint8": case "bool": fs.WriteByte(Convert.ToByte(data)); return 1;
        //        case "int16": case "uint16": fs.WriteInt16(Convert.ToInt16(data)); return 2;
        //        case "int32": case "uint32": fs.WriteInt32(Convert.ToInt32(data)); return 4;
        //        case "int64": case "uint64": fs.WriteInt64(Convert.ToInt64(data)); return 8;
        //        case "float": fs.WriteSingle(Convert.ToSingle(data)); return 4;
        //        case "string": return fs.WriteString(data);
        //    }
        //    return 0;
        //}


        //=================================================================================
        // Common
        //=================================================================================
        public static float ConvertHexToColor(string hex)
        {
            if (hex == "00")
            {
                return 0.0f;
            }
            else if (hex == "FF")
            {
                return 1.0f;
            }

            return (float)Convert.ToInt32(hex, 16) * 0.00390625f; // 0.00390625f == 1.0f / 255.0f
        }

        public static bool WriteByType_Json(Dictionary<string, object> so, string name, string type, string data)
        {
            switch (type)
            {
                case "bool": so.Add(name, data == "" ? (byte)0 : Convert.ToByte(data)); return true;
                case "int8": so.Add(name, data == "" ? (sbyte)0 : Convert.ToSByte(data)); return true;
                case "uint8": so.Add(name, data == "" ? (byte)0 : Convert.ToByte(data)); return true;
                case "int16": so.Add(name, data == "" ? (short)0 : Convert.ToInt16(data)); return true;
                case "uint16": so.Add(name, data == "" ? (ushort)0 : Convert.ToUInt16(data)); return true;
                case "int32": so.Add(name, data == "" ? (int)0 : Convert.ToInt32(data)); return true;
                case "uint32": so.Add(name, data == "" ? (uint)0 : Convert.ToUInt32(data)); return true;
                case "int64": so.Add(name, data == "" ? (long)0 : Convert.ToInt64(data)); return true;
                case "uint64": so.Add(name, data == "" ? (ulong)0 : Convert.ToUInt64(data)); return true;
                case "float": so.Add(name, data == "" ? 0.0f : Convert.ToSingle(data)); return true;
                case "string": so.Add(name, data); return true;
            }

            return false;
        }


        //=================================================================================
        // BinaryWriter
        //=================================================================================
        public static int WriteString(this BinaryWriter fs, string data)
        {
            data = data.Replace("\\n", "\n");
            byte[] dataArray = Encoding.Unicode.GetBytes(data);
            fs.Write((ushort)data.Length);
            if (0 < dataArray.Length)
                fs.Write(dataArray, 0, dataArray.Length);
            return dataArray.Length;
        }

        public static int WriteEnum(this BinaryWriter fs, int size, int data)
        {
            if (size == 1) { fs.Write((byte)data); }
            else if (size == 2) { fs.Write((ushort)data); }
            else { fs.Write((uint)data); }
            return 0;
        }

        public static int WriteVector2(this BinaryWriter fs, string data)
        {
            if (data.Length <= 0)
            {
                data = "Vector2(0,0)";
            }

            string[] vecs = data.Replace("Vector2", "").Replace("(", "").Replace(")", "").Split(',');

            if (vecs.Length > 0) { fs.Write(Convert.ToSingle(vecs[0].Trim())); } else { fs.Write(0.0f); } // X
            if (vecs.Length > 1) { fs.Write(Convert.ToSingle(vecs[1].Trim())); } else { fs.Write(0.0f); } // Y

            return 0;
        }

        public static int WriteVector3(this BinaryWriter fs, string data)
        {
            if (data.Length <= 0)
            {
                data = "Vector3(0,0,0)";
            }

            string[] vecs = data.Replace("Vector3", "").Replace("(", "").Replace(")", "").Split(',');

            if (vecs.Length > 0) { fs.Write(Convert.ToSingle(vecs[0].Trim())); } else { fs.Write(0.0f); } // X
            if (vecs.Length > 1) { fs.Write(Convert.ToSingle(vecs[1].Trim())); } else { fs.Write(0.0f); } // Y
            if (vecs.Length > 2) { fs.Write(Convert.ToSingle(vecs[2].Trim())); } else { fs.Write(0.0f); } // Z

            return 0;
        }

        public static int WriteColor(this BinaryWriter fs, string data)
        {
            string trim = data.Trim().Trim('#').Trim();

            if (trim.Length >= 2) { fs.Write(ConvertHexToColor(trim.Substring(0, 2))); } else { fs.Write(0.0f); } // R
            if (trim.Length >= 4) { fs.Write(ConvertHexToColor(trim.Substring(2, 2))); } else { fs.Write(0.0f); } // G
            if (trim.Length >= 6) { fs.Write(ConvertHexToColor(trim.Substring(4, 2))); } else { fs.Write(0.0f); } // B
            if (trim.Length >= 8) { fs.Write(ConvertHexToColor(trim.Substring(6, 2))); } else { fs.Write(1.0f); } // A

            return 0;
        }

        public static int WriteByType(this BinaryWriter fs, string type, string data)
        {
            switch (type)
            {
                case "bool": fs.Write(data == "" ? (byte)0 : Convert.ToByte(data)); return 1;
                case "int8": fs.Write(data == "" ? (sbyte)0 : Convert.ToSByte(data)); return 1;
                case "uint8": fs.Write(data == "" ? (byte)0 : Convert.ToByte(data)); return 1;
                case "int16": fs.Write(data == "" ? (short)0 : Convert.ToInt16(data)); return 2;
                case "uint16": fs.Write(data == "" ? (ushort)0 : Convert.ToUInt16(data)); return 2;
                case "int32": fs.Write(data == "" ? (int)0 : Convert.ToInt32(data)); return 4;
                case "uint32": fs.Write(data == "" ? (uint)0 : Convert.ToUInt32(data)); return 4;
                case "int64": fs.Write(data == "" ? (long)0 : Convert.ToInt64(data)); return 8;
                case "uint64": fs.Write(data == "" ? (ulong)0 : Convert.ToUInt64(data)); return 8;
                case "float": fs.Write(data == "" ? 0.0f : Convert.ToSingle(data)); return 4;
                case "string": return fs.WriteString(data);
            }
            return 0;
        }
    }
}
