using System;
using System.IO;
using System.Text;
using System.Diagnostics;


namespace PacketCodeMaker
{
    public class Util
    {
        static public string Tab = "    ";
        static public string API = "CM_API";
        public class PathInfo
        {
            public string mCodePathClient = "";
            public string mCodePathServer = "";
            public string mCodePathServerPython = "";
            public string mCodePathBot = "";
            public string mExcelPath = "";
            public string mEnumExcelPath = "";

            public PathInfo(string client, string server, string serverpython, string bot, string excel, string enumexcel)
            {
                mCodePathClient = client;
                if (mCodePathClient[mCodePathClient.Length - 1] != '/' && mCodePathClient[mCodePathClient.Length - 1] != '\\')
                    mCodePathClient += '/';

                mCodePathServer = server;
                if (mCodePathServer[mCodePathServer.Length - 1] != '/' && mCodePathServer[mCodePathServer.Length - 1] != '\\')
                    mCodePathServer += '/';

                mCodePathServerPython = serverpython;
                if (mCodePathServerPython[mCodePathServerPython.Length - 1] != '/' && mCodePathServerPython[mCodePathServerPython.Length - 1] != '\\')
                    mCodePathServerPython += '/';

                mCodePathBot = bot;
                if (mCodePathBot[mCodePathBot.Length - 1] != '/' && mCodePathBot[mCodePathBot.Length - 1] != '\\')
                    mCodePathBot += '/';

                mExcelPath = excel;
                if (mExcelPath[mExcelPath.Length - 1] != '/' && mExcelPath[mExcelPath.Length - 1] != '\\')
                    mExcelPath += '/';

                mEnumExcelPath = enumexcel;
                if (mEnumExcelPath[mEnumExcelPath.Length - 1] != '/' && mEnumExcelPath[mEnumExcelPath.Length - 1] != '\\')
                    mEnumExcelPath += '/';
            }
        }

        static public StreamWriter GetTextFileAdded(string path)
        {
            StreamWriter w = new StreamWriter(File.Open(path, FileMode.OpenOrCreate), Encoding.GetEncoding(65001));
            w.BaseStream.Seek(0, SeekOrigin.Begin);
            return w;
        }

        static public StreamWriter GetTextFile(string path)
        {
            if (File.Exists(path) == true)
            {
                File.Delete(path);
            }

            StreamWriter w = new StreamWriter(File.Open(path, FileMode.CreateNew), Encoding.GetEncoding(65001));
            w.BaseStream.Seek(0, SeekOrigin.Begin);
            return w;
        }

        static public void FlushAndReleaseTextFile(StreamWriter s)
        {
            s.Flush();
            s.Close();
            s.Dispose();
        }

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

        static public void LoadFolder(
                                out string client,
                                out string server,
                                out string serverpython,
                                out string bot,
                                out string excel,
                                out string enumexcel)
        {
            client = Directory.GetCurrentDirectory();
            server = Directory.GetCurrentDirectory();
            serverpython = Directory.GetCurrentDirectory();
            bot = Directory.GetCurrentDirectory();
            excel = Directory.GetCurrentDirectory();
            enumexcel = Directory.GetCurrentDirectory();

            if (Directory.Exists("./Config") == true)
            {
                if (File.Exists("./Config/Path.cfg") == false)
                {
                    SaveFolder("./Config/Path.cfg", client, server, serverpython, bot, excel, enumexcel);
                }
                else
                {
                    ReadConfigFile("./Config/Path.cfg", out client, out server, out serverpython, out bot, out excel, out enumexcel);
                }
            }
            else if (Directory.Exists("../../Config") == true)
            {
                if (File.Exists("../../Config/Path.cfg") == false)
                {
                    SaveFolder("../../Config/Path.cfg", client, server, serverpython, bot, excel, enumexcel);
                }
                else
                {
                    ReadConfigFile("../../Config/Path.cfg", out client, out server, out serverpython, out bot, out excel, out enumexcel);
                }
            }
            else
            {
                Directory.CreateDirectory("./Config");
                SaveFolder("../../Config/Path.cfg", client, server, serverpython, bot, excel, enumexcel);
            }
        }

        static public void SaveFolder(string config_file, string client, string server, string serverpython, string bot, string excel, string enumexcel)
        {
            if (config_file == "")
            {
                if (Directory.Exists("./Config") == true)
                {
                    config_file = "./Config/Path.cfg";
                }
                else if (Directory.Exists("../../Config") == true)
                {
                    config_file = "../../Config/Path.cfg";
                }
            }

            StreamWriter w = GetTextFileAdded(config_file);

            w.WriteLine("ClientPath={0}", client);
            w.WriteLine("ServerPath={0}", server);
            w.WriteLine("ServerPythonPath={0}", serverpython);
            w.WriteLine("BotPath={0}", bot);
            w.WriteLine("ExcelPath={0}", excel);
            w.WriteLine("EnumExcelPath={0}", enumexcel);

            FlushAndReleaseTextFile(w);
        }

        static private void ReadConfigFile(string config_file, out string client, out string server, out string serverpython, out string bot, out string excel, out string enumexcel)
        {
            client = Directory.GetCurrentDirectory();
            server = Directory.GetCurrentDirectory();
            serverpython = Directory.GetCurrentDirectory();
            bot = Directory.GetCurrentDirectory();
            excel = Directory.GetCurrentDirectory();
            enumexcel = Directory.GetCurrentDirectory();

            StreamReader sr = new StreamReader(config_file);

            while (sr.EndOfStream == false)
            {
                string[] vs = sr.ReadLine().Split('=');

                if (vs.Length >= 2)
                {
                    string cmd = vs[0].Trim().ToLower();

                    if (cmd == "clientpath")
                    {
                        client = vs[1].Trim();
                    }
                    else if (cmd == "serverpath")
                    {
                        server = vs[1].Trim();
                    }
                    else if (cmd == "serverpythonpath")
                    {
                        serverpython = vs[1].Trim();
                    }
                    else if (cmd == "botpath")
                    {
                        bot = vs[1].Trim();
                    }
                    else if (cmd == "excelpath")
                    {
                        excel = vs[1].Trim();
                    }
                    else if (cmd == "enumexcelpath")
                    {
                        enumexcel = vs[1].Trim();
                    }
                }    
            }

            sr.Close();
            sr.Dispose();
        }

        static public bool IsStruct(string type)
        {
            return type.Contains("GS::");
        }

        static public bool IsComment(string line)
        {
            return (line.Length >= 2 && line[0] == '/' && line[1] == '/');
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
                default: return vtype;
            }
        }
    }
}
