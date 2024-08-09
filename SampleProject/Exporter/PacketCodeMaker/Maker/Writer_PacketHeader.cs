using System;
using System.IO;


namespace PacketCodeMaker
{
    public class Writer_PacketHeader
    {
        static public void Write(Util.PathInfo pathInfo, PacketInfos infos, bool writeBot, bool writeClient)
        {
            string server_directory = pathInfo.mCodePathServer + "Packet/";
            string client_directory = pathInfo.mCodePathClient + "Packet/";
            string bot_directory = pathInfo.mCodePathBot + "Packet/";

            Util.MakeFolder(server_directory);

            if (writeBot == true)
            {
                Util.MakeFolder(bot_directory);
            }

            if (writeClient == true)
            {
                Util.MakeFolder(client_directory);
            }


            string filepath_server = string.Format("{0}NetPacket_{1}.h", server_directory, infos.mServerName);
            StreamWriter w = Util.GetTextFile(filepath_server);


            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("#include \"../Lib/NetPacket.h\"");
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("#pragma pack(1)");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("//=================================================================================================================");
            w.WriteLine("// {0} Server Packet Command", infos.mServerName);
            w.WriteLine("//=================================================================================================================");
            Write_Command(w, infos, "");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("//=================================================================================================================");
            w.WriteLine("// {0} Server Packet Header", infos.mServerName);
            w.WriteLine("//=================================================================================================================");
            Write_Header(w, infos, "");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("#pragma pack()");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("#include \"../Types/GE.h\"");
            w.WriteLine("#include \"../Types/RE.h\"");
            w.WriteLine("#include \"../Types/GS.h\"");
            //w.WriteLine("//#include \"NetPacket_{0}_C2S.h\"", infos.mServerName);
            //w.WriteLine("//#include \"NetPacket_{0}_S2C.h\"", infos.mServerName);


            Util.FlushAndReleaseTextFile(w);
            Console.WriteLine("=== Write {0}", filepath_server);


            if (writeBot == true)
            {
                string filepath_bot = string.Format("{0}NetPacket_{1}.h", bot_directory, infos.mServerName);
                File.Copy(filepath_server, filepath_bot, true);
                Console.WriteLine("=== Write {0}", filepath_bot);
            }

            if (writeClient == true)
            {
                string filepath_client = string.Format("{0}NetPacket_{1}.h", client_directory, infos.mServerName);
                File.Copy(filepath_server, filepath_client, true);
                Console.WriteLine("=== Write {0}", filepath_client);
            }
        }

        static private void Write_Command(StreamWriter w, PacketInfos infos, string tab)
        {
            w.WriteLine("{0}enum class NP_{1}_C2S : int32", tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", tab);
            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "C2S")
                {
                    w.WriteLine("{0}{1} = {2},", tab + Util.Tab, p.cmd_name.ToUpper(), p.cmd);
                }
            }
            w.WriteLine("{0}}};", tab);


            w.WriteLine("");


            w.WriteLine("{0}enum class NP_{1}_S2C : int32", tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", tab);
            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "S2C")
                {
                    w.WriteLine("{0}{1} = {2},", tab + Util.Tab, p.cmd_name.ToUpper(), p.cmd);
                }
            }
            w.WriteLine("{0}}};", tab);
        }

        static private void Write_Header(StreamWriter w, PacketInfos infos, string tab)
        {
            w.WriteLine("{0}struct NP{1}C2SHeader : public NetPacketHeader", tab, infos.mServerName);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}private:", tab);
            w.WriteLine("{0}NP_{1}_C2S Cmd;", tab + Util.Tab, infos.mServerNameUP);

            w.WriteLine("{0}public:", tab);
            w.WriteLine("{0}NP{1}C2SHeader(unsigned short len, NP_{2}_C2S cmd) : NetPacketHeader(len), Cmd(cmd) {{}}", tab + Util.Tab, infos.mServerName, infos.mServerNameUP);

            w.WriteLine("{0}NP_{1}_C2S GetCmd() {{ return Cmd; }}", tab + Util.Tab, infos.mServerNameUP);

            w.WriteLine("{0}}};", tab);



            w.WriteLine("");



            w.WriteLine("{0}struct NP{1}S2CHeader : public NetPacketHeader", tab, infos.mServerName);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}private:", tab);
            w.WriteLine("{0}NP_{1}_S2C Cmd;", tab + Util.Tab, infos.mServerNameUP);

            w.WriteLine("{0}public:", tab);
            w.WriteLine("{0}NP{1}S2CHeader(unsigned short len, NP_{2}_S2C cmd) : NetPacketHeader(len), Cmd(cmd) {{}}", tab + Util.Tab, infos.mServerName, infos.mServerNameUP);

            w.WriteLine("{0}NP_{1}_S2C GetCmd() {{ return Cmd; }}", tab + Util.Tab, infos.mServerNameUP);

            w.WriteLine("{0}}};", tab);
        }
    }
}
