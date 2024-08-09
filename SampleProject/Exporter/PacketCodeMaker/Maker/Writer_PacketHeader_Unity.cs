using System;
using System.IO;


namespace PacketCodeMaker
{
    public class Writer_PacketHeader_Unity
    {
        static public void Write(Util.PathInfo pathInfo, PacketInfos infos)
        {
            string client_directory = pathInfo.mCodePathClient + "Packet/";

            Util.MakeFolder(client_directory);


            string filepath_client = string.Format("{0}NetPacket_{1}.cs", client_directory, infos.mServerName);
            StreamWriter w = Util.GetTextFile(filepath_client);


            //w.WriteLine("");
            //w.WriteLine("namespace TcpNet");
            //w.WriteLine("{");
            w.WriteLine("");
            w.WriteLine("// [{0} Server]", infos.mServerName);
            w.WriteLine("");
            w.WriteLine("");

            Write_Command(w, infos, "");

            //w.WriteLine("");
            //w.WriteLine("}");


            Util.FlushAndReleaseTextFile(w);
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_Command(StreamWriter w, PacketInfos infos, string tab)
        {
            w.WriteLine("{0}//=================================================================================================================", tab);
            w.WriteLine("{0}// {1} Server Packet Command (Client -> Server)", tab, infos.mServerName);
            w.WriteLine("{0}//=================================================================================================================", tab);

            w.WriteLine("{0}public enum NP_{1}_C2S : int", tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", tab);
            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "C2S")
                {
                    w.WriteLine("{0}{1} = {2},", tab + Util.Tab, p.cmd_name.ToUpper(), p.cmd);
                }
            }
            w.WriteLine("{0}}}", tab);


            w.WriteLine("");

            w.WriteLine("{0}//=================================================================================================================", tab);
            w.WriteLine("{0}// {1} Server Packet Command (Server -> Client)", tab, infos.mServerName);
            w.WriteLine("{0}//=================================================================================================================", tab);


            w.WriteLine("{0}public enum NP_{1}_S2C : int", tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", tab);
            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "S2C")
                {
                    w.WriteLine("{0}{1} = {2},", tab + Util.Tab, p.cmd_name.ToUpper(), p.cmd);
                }
            }
            w.WriteLine("{0}}}", tab);
        }
    }
}
