using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;



namespace PacketCodeMaker
{
    public class PacketMaker
    {
        static public void Make(Util.PathInfo pathInfo)
        {
            Console.WriteLine("=== [packet make start] ===");

            string[] excelFiles = System.IO.Directory.GetFiles(pathInfo.mExcelPath, "*.xlsx");

            for (int i = 0; i < excelFiles.Length; ++i)
            {
                Console.WriteLine("");
                Console.WriteLine(string.Format("{0}", excelFiles[i]));

                string serverName = Path.GetFileNameWithoutExtension(excelFiles[i]);

                if (serverName[0] == '~')
                {
                    Console.WriteLine(">>>>>>>>>>>>>>> [skip] temp file");
                    continue;
                }

                if (serverName[0] == '_')
                {
                    Console.WriteLine(">>>>>>>>>>>>>>> [skip] sameple file");
                    continue;
                }

                FileStream stream = File.Open(excelFiles[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                IWorkbook book = new XSSFWorkbook(stream);

                string[] snames = serverName.Split('_');
                if (snames.Length == 1)
                {
                    MakeOneFile(pathInfo, serverName, book);
                }
                else if (snames.Length == 2)
                {
                    MakeOneFile_SRV(pathInfo, snames[1], book);
                }

                book.Close();
                stream.Close();
                stream.Dispose();
            }

            Console.WriteLine("");
            Console.WriteLine("=== [make end] ===");


            //Console.WriteLine("");
            //Console.WriteLine("=== [lib copy start] ===");
            //CopyLibFiles(pathInfo);
            //Console.WriteLine("");
            //Console.WriteLine("=== [lib copy end] ===");
        }

        static void MakeOneFile(Util.PathInfo pathInfo, string serverName, IWorkbook book)
        {
            // Read All Packet
            PacketInfos infos = PacketReader.Read(book);
            infos.SetServerName(serverName);
            Console.WriteLine("======================== Read [{0}]", serverName);

            // Write Header for Server & Bot
            Writer_PacketHeader.Write(pathInfo, infos, true, false);
            // Write Code for Unity Client
            Writer_PacketHeader_Unity.Write(pathInfo, infos);

            // Write Header C2S for Server & Bot
            Writer_PacketHeaderC2S.Write(pathInfo, infos, true, false);
            // Write Code C2S for Unity Client
            Writer_PacketHeaderC2S_Unity.Write(pathInfo, infos);

            // Write Header S2C for Server & Bot
            Writer_PacketHeaderS2C.Write(pathInfo, infos, true, false);
            // Write Code S2C for Unity Client
            Writer_PacketHeaderS2C_Unity.Write(pathInfo, infos);

            // Write Sender Header & Source for Client
            //Writer_C2S_Sender.Write(pathInfo, infos);
            // Write Sender Code for Unity Client
            Writer_C2S_Sender_Unity.Write(pathInfo, infos);

            // Write Receiver Header for Client
            //Writer_S2C_Receiver.Write(pathInfo, infos);
            // Write Receiver Code for Unity Client
            Writer_S2C_Receiver_Unity.Write(pathInfo, infos);
        }

        static void MakeOneFile_SRV(Util.PathInfo pathInfo, string serverName, IWorkbook book)
        {
            // Read All Packet
            PacketInfos infos = PacketReader.Read(book);
            infos.SetServerName(serverName);
            Console.WriteLine("======================== Read [{0}]", serverName);

            // Write Header for Client & Srever
            Writer_PacketHeader.Write(pathInfo, infos, false, false);

            // Write Header C2S for Client & Server
            Writer_PacketHeaderC2S.Write(pathInfo, infos, false, false);

            // Write Header S2C for Client & Server
            Writer_PacketHeaderS2C.Write(pathInfo, infos, false, false);
        }

        static void CopyLibFiles(Util.PathInfo pathInfo)
        {
            CopyLibFile("./Lib", "../../Lib", pathInfo.mCodePathClient);
            CopyLibFile("./Lib_Bot", "../../Lib_Bot", pathInfo.mCodePathBot);
            CopyLibFile("./Lib_Server", "../../Lib_Server", pathInfo.mCodePathServer);
        }

        static void CopyLibFile(string src_dir, string src_dir_ex, string dest_dir)
        {
            string[] lib_files = null;

            if (Directory.Exists(src_dir) == true)
            {
                lib_files = Directory.GetFiles(src_dir);
            }
            else if (Directory.Exists(src_dir_ex) == true)
            {
                lib_files = Directory.GetFiles(src_dir_ex);
            }
            else
            {
                // error
                return;
            }

            string dest_directory = dest_dir + "Lib/";

            Util.MakeFolder(dest_directory);

            for (int i = 0; i < lib_files.Length; ++i)
            {
                string filename = Path.GetFileName(lib_files[i]);

                File.Copy(lib_files[i], dest_directory + filename, true);
            }
        }
    }
}