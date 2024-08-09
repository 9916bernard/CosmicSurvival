using System;
using System.IO;


namespace PacketCodeMaker
{
    public class Writer_PacketHeaderC2S_Unity
    {
        static public void Write(Util.PathInfo pathInfo, PacketInfos infos)
        {
            string client_directory = pathInfo.mCodePathClient + "Packet/";

            Util.MakeFolder(client_directory);

            string filepath_client = string.Format("{0}NetPacket_{1}_C2S.cs", client_directory, infos.mServerName);

            Write_C2S(filepath_client, infos);
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_C2S(string filepath, PacketInfos infos)
        {
            StreamWriter w = Util.GetTextFile(filepath);


            w.WriteLine("using System;");
            w.WriteLine("using System.Runtime.InteropServices;");
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("");


            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "C2S")
                {
                    Write_Packet(w, infos, p);
                }
            }


            Util.FlushAndReleaseTextFile(w);
        }

        static private void Write_Packet(StreamWriter w, PacketInfos infos, PacketInfos.Packet p)
        {
            w.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]");
            w.WriteLine("[Serializable]");
            w.WriteLine("public class NP{0}C2S_{1} : NetPacketHeader", infos.mServerName, p.cmd_name);
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

            w.WriteLine("{0}public NP{1}C2S_{2}() {{ mCmd = (int)NP_{3}_C2S.{4}; }}", Util.Tab, infos.mServerName, p.cmd_name, infos.mServerNameUP, p.cmd_name.ToUpper());

            w.WriteLine("}");
            w.WriteLine("");
        }
    }
}
