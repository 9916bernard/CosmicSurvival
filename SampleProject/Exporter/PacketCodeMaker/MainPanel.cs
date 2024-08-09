using System;
using System.Windows.Forms;
//using System.Threading;
using System.IO;


namespace PacketCodeMaker
{
    public partial class MainPanel : Form
    {
        public MainPanel()
        {
            InitializeComponent();

            string client = ClientPathValue.Text;
            string server = ServerPathValue.Text;
            string serverpython = ServerPythonPathValue.Text;
            string bot = BotPathValue.Text;
            string excel = PacketExcelPathValue.Text;
            string enumexcel = EnumExcelPathValue.Text;
            Util.LoadFolder(out client, out server, out serverpython, out bot, out excel, out enumexcel);
            ClientPathValue.Text = client;
            ServerPathValue.Text = server;
            ServerPythonPathValue.Text = serverpython;
            BotPathValue.Text = bot;
            PacketExcelPathValue.Text = excel;
            EnumExcelPathValue.Text = enumexcel;
        }

        private void Button_MakePacketCode_Click(object sender, EventArgs e)
        {
            //new Thread(() => {
            //    Maker.Make("C:/Work/space/Packet", "C:/Work/space/Packet/Client", "C:/Work/space/Packet/Server");
            //}).Start();

            Util.SaveFolder("",
                    ClientPathValue.Text,
                    ServerPathValue.Text,
                    ServerPythonPathValue.Text,
                    BotPathValue.Text,
                    PacketExcelPathValue.Text,
                    EnumExcelPathValue.Text);

            Util.PathInfo pathInfo = new Util.PathInfo(
                    ClientPathValue.Text,
                    ServerPathValue.Text,
                    ServerPythonPathValue.Text,
                    BotPathValue.Text,
                    PacketExcelPathValue.Text,
                    EnumExcelPathValue.Text);

            Status.Text = "Start";
            PacketMaker.Make(pathInfo);
            EnumStructMaker.Make(pathInfo);
            Status.Text = "Complete";
        }

        private void Button_GoClient_Click(object sender, EventArgs e)
        {
            Util.OpenFolder(ClientPathValue.Text);
        }

        private void Button_GoServer_Click(object sender, EventArgs e)
        {
            Util.OpenFolder(ServerPathValue.Text);
        }

        private void Button_GoServerPython_Click(object sender, EventArgs e)
        {
            Util.OpenFolder(ServerPythonPathValue.Text);
        }

        private void Button_GoBot_Click(object sender, EventArgs e)
        {
            Util.OpenFolder(BotPathValue.Text);
        }

        private void Button_GoEnum_Click(object sender, EventArgs e)
        {
            Util.OpenFolder(EnumExcelPathValue.Text);
        }

        private void Button_GoExcel_Click(object sender, EventArgs e)
        {
            Util.OpenFolder(PacketExcelPathValue.Text);
        }

        private void Button_SetClient_Click(object sender, EventArgs e)
        {
            string dir = ClientPathValue.Text;
            if (Directory.Exists(dir) == false)
            {
                dir = Directory.GetCurrentDirectory();
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "클라이언트 경로";
            fbd.SelectedPath = dir;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ClientPathValue.Text = fbd.SelectedPath;
                Util.SaveFolder("",
                        ClientPathValue.Text,
                        ServerPathValue.Text,
                        ServerPythonPathValue.Text,
                        BotPathValue.Text,
                        PacketExcelPathValue.Text,
                        EnumExcelPathValue.Text);
            }
        }

        private void Button_SetServer_Click(object sender, EventArgs e)
        {
            string dir = ServerPathValue.Text;
            if (Directory.Exists(dir) == false)
            {
                dir = Directory.GetCurrentDirectory();
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "서버 경로";
            fbd.SelectedPath = dir;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ServerPathValue.Text = fbd.SelectedPath;
                Util.SaveFolder("",
                        ClientPathValue.Text,
                        ServerPathValue.Text,
                        ServerPythonPathValue.Text,
                        BotPathValue.Text,
                        PacketExcelPathValue.Text,
                        EnumExcelPathValue.Text);
            }
        }

        private void Button_SetServerPython_Click(object sender, EventArgs e)
        {
            string dir = ServerPythonPathValue.Text;
            if (Directory.Exists(dir) == false)
            {
                dir = Directory.GetCurrentDirectory();
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "서버(파이썬) 경로";
            fbd.SelectedPath = dir;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ServerPythonPathValue.Text = fbd.SelectedPath;
                Util.SaveFolder("",
                        ClientPathValue.Text,
                        ServerPathValue.Text,
                        ServerPythonPathValue.Text,
                        BotPathValue.Text,
                        PacketExcelPathValue.Text,
                        EnumExcelPathValue.Text);
            }
        }

        private void Button_SetBot_Click(object sender, EventArgs e)
        {
            string dir = BotPathValue.Text;
            if (Directory.Exists(dir) == false)
            {

                dir = Directory.GetCurrentDirectory();
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "엑셀 파일 경로";
            fbd.SelectedPath = dir;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                BotPathValue.Text = fbd.SelectedPath;
                Util.SaveFolder("",
                        ClientPathValue.Text,
                        ServerPathValue.Text,
                        ServerPythonPathValue.Text,
                        BotPathValue.Text,
                        PacketExcelPathValue.Text,
                        EnumExcelPathValue.Text);
            }
        }

        private void Button_SetExcel_Click(object sender, EventArgs e)
        {
            string dir = PacketExcelPathValue.Text;
            if (Directory.Exists(dir) == false)
            {
                dir = Directory.GetCurrentDirectory();
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "PACKET 엑셀 파일 경로";
            fbd.SelectedPath = dir;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                PacketExcelPathValue.Text = fbd.SelectedPath;
                Util.SaveFolder("",
                        ClientPathValue.Text,
                        ServerPathValue.Text,
                        ServerPythonPathValue.Text,
                        BotPathValue.Text,
                        PacketExcelPathValue.Text,
                        EnumExcelPathValue.Text);
            }
        }

        private void Button_SetEnum_Click(object sender, EventArgs e)
        {
            string dir = EnumExcelPathValue.Text;
            if (Directory.Exists(dir) == false)
            {
                dir = Directory.GetCurrentDirectory();
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "ENUM 엑셀 파일 경로";
            fbd.SelectedPath = dir;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                EnumExcelPathValue.Text = fbd.SelectedPath;
                Util.SaveFolder("",
                        ClientPathValue.Text,
                        ServerPathValue.Text,
                        ServerPythonPathValue.Text,
                        BotPathValue.Text,
                        PacketExcelPathValue.Text,
                        EnumExcelPathValue.Text);
            }
        }




        public void ODBC_SELECT()
        {
            System.Data.Odbc.OdbcConnection conn = null;
            System.Data.Odbc.OdbcCommand cmd = null;
            System.Data.Odbc.OdbcDataReader reader = null;

            string connSrt = @"Driver={MariaDB ODBC 3.1 Driver};Server=10.10.11.22;UID=root;PWD=Wjsmsqnqns1!;DB=db_user;Port=12500";
            using (conn = new System.Data.Odbc.OdbcConnection(connSrt))
            {
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Open Successed");

                    cmd = new System.Data.Odbc.OdbcCommand("SELECT UserID FROM t_transfer;", conn);

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine(string.Format("{0} : {1}", reader.GetName(i), reader.GetValue(i).ToString()));
                        }
                    }

                    reader.Close();

                    conn.Close();
                }
            }
        }

        public void ODBC_INSERT()
        {
            System.Data.Odbc.OdbcConnection conn = null;
            System.Data.Odbc.OdbcCommand cmd = null;

            string connSrt = @"Driver={MariaDB ODBC 3.1 Driver};Server=10.10.11.22;UID=root;PWD=Wjsmsqnqns1!;DB=db_user;Port=12500";
            using (conn = new System.Data.Odbc.OdbcConnection(connSrt))
            {
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("Open Successed");

                    cmd = new System.Data.Odbc.OdbcCommand("INSERT INTO t_transfer (TransferCode, UserID) VALUES ('KKWENRFW324SDF', 68);", conn);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
        }
    }
}
