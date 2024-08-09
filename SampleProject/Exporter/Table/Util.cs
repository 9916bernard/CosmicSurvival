using System.IO;
using System.Diagnostics;
using System.Text;


namespace Table
{
    class Util
    {
        static readonly public string Tab = "    ";

        static public void MakeFolder(string folderpath)
        {
            if (Directory.Exists(folderpath) == false)
            {
                Directory.CreateDirectory(folderpath);
            }
        }

        static public void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
        }

        static public void OpenFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Process.Start(filePath);
            }
        }

        static public StreamWriter GetNewTextFile(string path)
        {
            if (File.Exists(path) == true)
                File.Delete(path);

            StreamWriter w = new StreamWriter(File.Open(path, FileMode.CreateNew), Encoding.GetEncoding(65001));

            return w;
        }

        static public StreamReader GetExistTextFile(string path)
        {
            StreamReader r = new StreamReader(path);
            
            return r;
        }

        static public void FlushAndReleaseTextFile(StreamWriter s)
        {
            s.Flush();
            s.Close();
            s.Dispose();
        }

        static public string ConvertTypeUnity(string vtype)
        {
            switch (vtype)
            {
                case "uint8": return "byte";
                case "uint16": return "ushort";
                case "uint32": return "uint";
                case "uint64": return "ulong";
                case "int8": return "sbyte";
                case "int16": return "short";
                case "int32": return "int";
                case "int64": return "long";
                case "vector2": return "Vector2";
                case "vector3": return "Vector3";
                case "color": return "Color";
                default: return vtype;
            }
        }

        static public string GetBinaryReaderFuncNameUnity(string vtype)
        {
            switch (vtype)
            {
                case "uint8": return "ReadByte";
                case "uint16": return "ReadUInt16";
                case "uint32": return "ReadUInt32";
                case "uint64": return "ReadUInt64";
                case "int8": return "ReadSByte";
                case "int16": return "ReadInt16";
                case "int32": return "ReadInt32";
                case "int64": return "ReadInt64";
                case "float": return "ReadSingle";
                default: return "";
            }
        }
    }
}
