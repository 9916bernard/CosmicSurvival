using System;
using System.IO;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace PacketCodeMaker
{
    public class EnumStructMaker
    {
        static public void Make(Util.PathInfo pathInfo)
        {
            Console.WriteLine("=== [enumstruct make start] ===");

            string[] excelFiles = System.IO.Directory.GetFiles(pathInfo.mEnumExcelPath, "*.xlsx");

            for (int i = 0; i < excelFiles.Length; ++i)
            {
                Console.WriteLine("");
                Console.WriteLine(string.Format("{0}", excelFiles[i]));

                string enumfileName = Path.GetFileNameWithoutExtension(excelFiles[i]);

                if (enumfileName[0] == '~')
                {
                    Console.WriteLine(">>>>>>>>>>>>>>> [skip] temp file");
                    continue;
                }

                if (enumfileName[0] == '_')
                {
                    Console.WriteLine(">>>>>>>>>>>>>>> [skip] sameple file");
                    continue;
                }

                FileStream stream = File.Open(excelFiles[i], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                IWorkbook book = new XSSFWorkbook(stream);

                if (enumfileName == "RE")
                {
                    MakeOneFile(pathInfo, enumfileName, book, "RE", "");
                }
                else if (enumfileName == "GE")
                {
                    MakeOneFile(pathInfo, enumfileName, book, "GE", "GS");
                }

                book.Close();
                stream.Close();
                stream.Dispose();
            }

            Console.WriteLine("");
            Console.WriteLine("=== [make end] ===");
        }

        static void MakeOneFile(Util.PathInfo pathInfo, string enumfileName, IWorkbook book, string nsEnum, string nsStruct)
        {
            // Read All Packet
            EnumStructInfos infos = EnumStructReader.Read(book);
            Console.WriteLine("======================== Read [{0}]", enumfileName);

            List<EnumStructInfos> esList = new List<EnumStructInfos>();
            esList.Add(infos);

            // Write Enum
            if (nsEnum != "")
            {
                //Writer_EnumStruct_Client.Write_Enum(pathInfo, esList, nsEnum);
                Writer_EnumStruct_Client_Unity.Write_Enum(pathInfo, esList, nsEnum);
                Writer_EnumStruct_Server.Write_Enum(pathInfo, esList, nsEnum);
                Writer_EnumStruct_Server_Python.Write_Enum(pathInfo, esList, nsEnum);
            }

            // Write Struct
            if (nsStruct != "")
            {
                //Writer_EnumStruct_Client.Write_Struct(pathInfo, esList, nsStruct);
                Writer_EnumStruct_Client_Unity.Write_Struct(pathInfo, esList, nsStruct);
                Writer_EnumStruct_Server.Write_Struct(pathInfo, esList, nsStruct);
            }
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