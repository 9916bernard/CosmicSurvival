using System.IO;
using System.Collections.Generic;


namespace Table
{
    class Settings
    {
        static readonly public string CFG_PATH = "./setting.cfg";
        static readonly public string CFG_FILE = "setting.cfg";

        static readonly public string ENUM_DIR = "Types/";
        static readonly public string ENUM_FILE = "Types/SE.h";
        static readonly public string ENUM_FILE_UNITY = "Types/SE.cs";
        static readonly public string ENUM_FILE_PYTHON = "Types/SE.py";
        static readonly public string STRUCT_DIR = "Types/";
        static readonly public string STRUCT_FILE = "Types/SS.h";
        static readonly public string STRUCT_FILE_UNITY = "Types/SS.cs";
        static readonly public string DATA_DIR = "Data/";
        static readonly public string PATCH_FILE = "data.txt";

        static readonly public string DATA_FILE_EXT = ".txt";

        static public string API = "";
        static public string Table_Excel = "";
        static public List<string> Code_Server = new List<string>();
        static public List<string> Code_Server_Python = new List<string>();
        static public List<string> Code_Client = new List<string>();
        static public List<string> Code_Bot = new List<string>();
        static public List<string> Data_Server = new List<string>();
        static public List<string> Data_Server_Json = new List<string>();
        static public List<string> Data_Client = new List<string>();
        static public string Patch_URL = "";
        static public string Patch_TXT = "";

        static public void LoadSettingFile(System.Windows.Forms.ListView listView)
        {
            if (File.Exists(CFG_PATH) == false)
            {
                MakeDefaultSettingFile();
            }

            LoadSettingFile();

            listView.Items.Clear();

            listView.Items.Add("[Common]");
            listView.Items.Add(string.Format("API = {0}", API));

            listView.Items.Add("");

            listView.Items.Add("[Table]");
            listView.Items.Add(string.Format("Table_Excel = {0}", Table_Excel));

            listView.Items.Add("");


            listView.Items.Add("[Code]");
            for (int i = 0; i < Code_Client.Count; ++i)
                listView.Items.Add(string.Format("Code_Client = {0}", Code_Client[i]));
            for (int i = 0; i < Code_Server.Count; ++i)
                listView.Items.Add(string.Format("Code_Server = {0}", Code_Server[i]));
            for (int i = 0; i < Code_Server_Python.Count; ++i)
                listView.Items.Add(string.Format("Code_Server_Python = {0}", Code_Server_Python[i]));
            for (int i = 0; i < Code_Bot.Count; ++i)
                listView.Items.Add(string.Format("Code_Bot = {0}", Code_Bot[i]));

            listView.Items.Add("");

            listView.Items.Add("[Data File]");
            for (int i = 0; i < Data_Client.Count; ++i)
                listView.Items.Add(string.Format("Data_Client = {0}", Data_Client[i]));
            for (int i = 0; i < Data_Server.Count; ++i)
                listView.Items.Add(string.Format("Data_Server = {0}", Data_Server[i]));
            for (int i = 0; i < Data_Server_Json.Count; ++i)
                listView.Items.Add(string.Format("Data_Server_Json = {0}", Data_Server_Json[i]));

            listView.Items.Add("");

            listView.Items.Add("[PatchList File]");
            listView.Items.Add(string.Format("Patch_URL = {0}", Patch_URL));
            listView.Items.Add(string.Format("Patch_TXT = {0}", Patch_TXT));
        }

        static public void OpenSettingFolder(string text)
        {
            string[] s = text.Split('=');

            if (s.Length < 2)
                return;

            string title = s[0].Trim().ToLower();

            if (title == "table_excel" ||
                title == "code_server" ||
                title == "code_server_python" ||
                title == "code_client" ||
                title == "code_bot" ||
                title == "data_server" ||
                title == "data_server_json" ||
                title == "data_client" ||
                title == "patch_txt")
            {
                Util.OpenFolder(s[1].Trim());
            }
        }

        static public void MakeDefaultSettingFile()
        {
            StreamWriter w = Util.GetNewTextFile(CFG_PATH);

            w.WriteLine("[Common]");
            w.WriteLine("API = CM_API");
            w.WriteLine("");

            w.WriteLine("[Table]");
            w.WriteLine("Table_Excel = C:\\");
            w.WriteLine("");

            w.WriteLine("[Code]");
            w.WriteLine("Code_Server = C:\\");
            w.WriteLine("Code_Server_Python = C:\\");
            w.WriteLine("Code_Client = C:\\");
            w.WriteLine("Code_Bot = C:\\");
            w.WriteLine("");

            w.WriteLine("[Data File]");
            w.WriteLine("Data_Server = C:\\");
            w.WriteLine("Data_Server = C:\\");
            w.WriteLine("Data_Server_Json = C:\\");
            w.WriteLine("Data_Client = C:\\");
            w.WriteLine("Data_Client = C:\\");
            w.WriteLine("");

            w.WriteLine("[PatchList File]");
            w.WriteLine("Patch_URL = ");
            w.WriteLine("Patch_TXT = C:\\");

            Util.FlushAndReleaseTextFile(w);
        }

        static public void LoadSettingFile()
        {
            API = "CM_API";
            Table_Excel = "C:\\";
            Code_Server.Clear();
            Code_Server_Python.Clear();
            Code_Client.Clear();
            Code_Bot.Clear();
            Data_Server.Clear();
            Data_Server_Json.Clear();
            Data_Client.Clear();

            StreamReader sr = Util.GetExistTextFile(CFG_PATH);

            while (sr.EndOfStream == false)
            {
                string[] vs = sr.ReadLine().Split('=');

                if (vs.Length >= 2)
                {
                    string cmd = vs[0].Trim().ToLower();

                    if (cmd == "api")
                    {
                        API = vs[1].Trim();
                    }
                    else if (cmd == "table_excel")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Table_Excel = dir;
                    }
                    else if (cmd == "code_server")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Code_Server.Add(dir);
                    }
                    else if (cmd == "code_server_python")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Code_Server_Python.Add(dir);
                    }
                    else if (cmd == "code_client")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Code_Client.Add(dir);
                    }
                    else if (cmd == "code_bot")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Code_Bot.Add(dir);
                    }
                    else if (cmd == "data_server")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Data_Server.Add(dir);
                    }
                    else if (cmd == "data_server_json")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Data_Server_Json.Add(dir);
                    }
                    else if (cmd == "data_client")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Data_Client.Add(dir);
                    }
                    else if (cmd == "patch_url")
                    {
                        Patch_URL = vs[1].Trim();
                    }
                    else if (cmd == "patch_txt")
                    {
                        string dir = vs[1].Trim();
                        if (dir[dir.Length - 1] != '\\')
                            dir += '\\';
                        Patch_TXT = dir;
                    }
                }
            }

            sr.Close();
            sr.Dispose();
        }
    }
}
