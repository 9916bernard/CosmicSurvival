using System;
using System.IO;


namespace PacketCodeMaker
{
    public class Writer_PacketHeaderS2C_Unity
    {
        static public void Write(Util.PathInfo pathInfo, PacketInfos infos)
        {
            string client_directory = pathInfo.mCodePathClient + "Packet/";

            Util.MakeFolder(client_directory);

            string filepath_client = string.Format("{0}NetPacket_{1}_S2C.cs", client_directory, infos.mServerName);

            Write_S2C(filepath_client, infos, true);
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_S2C(string filepath, PacketInfos infos, bool isServer)
        {
            StreamWriter w = Util.GetTextFile(filepath);


            w.WriteLine("using System;");
            w.WriteLine("using System.Runtime.InteropServices;");
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("");


            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "S2C")
                {
                    Write_Packet(w, infos, p, isServer);
                }
            }


            Util.FlushAndReleaseTextFile(w);
        }

        static private void Write_Packet(StreamWriter w, PacketInfos infos, PacketInfos.Packet p, bool isServer)
        {
            w.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]");
            w.WriteLine("[Serializable]");
            w.WriteLine("public class NP{0}S2C_{1} : NetPacketHeader", infos.mServerName, p.cmd_name);
            w.WriteLine("{");

            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];
                if (elem.vcount <= 0)
                {
                    w.WriteLine("{0}public {1} m{2};", Util.Tab, elem.ConvertTypeUnity(), elem.vname);
                }
                else
                {
                    if (elem.vtype == "string")
                    {
                        w.WriteLine("{0}[MarshalAs(UnmanagedType.ByValTStr, SizeConst = {1})]", Util.Tab, elem.vcount + 1);
                        w.WriteLine("{0}public {1} m{2};", Util.Tab, elem.ConvertTypeUnity(), elem.vname);
                    }
                    else
                    {
                        w.WriteLine("{0}[MarshalAs(UnmanagedType.ByValArray, SizeConst = {1})]", Util.Tab, elem.vcount);
                        w.WriteLine("{0}public {1}[] m{2};", Util.Tab, elem.ConvertTypeUnity(), elem.vname);
                    }
                }
            }

            w.WriteLine("}");
            w.WriteLine("");
        }
    }
}
