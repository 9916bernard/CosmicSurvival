using System;
using System.IO;


namespace PacketCodeMaker
{
    public class Writer_C2S_Sender_Unity
    {
        static public void Write(Util.PathInfo pathInfo, PacketInfos infos)
        {
            string filepath_client = string.Format("{0}NetSender_{1}.cs", pathInfo.mCodePathClient, infos.mServerName);

            Write_Code(filepath_client, infos);
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_Code(string filepath_client, PacketInfos infos)
        {
            StreamWriter w = Util.GetTextFile(filepath_client);

            w.WriteLine("using System;");
            w.WriteLine("using System.Collections.Generic;");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("namespace TcpNet");
            w.WriteLine("{");
            w.WriteLine("");

            w.WriteLine("{0}public class NetSender_{1} : NetSender", Util.Tab, infos.mServerName);
            w.WriteLine("{0}{{", Util.Tab);


            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "C2S")
                {
                    Write_Code_PacketSendFunc(w, infos, p, Util.Tab + Util.Tab);
                }
            }


            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");
            w.WriteLine("} // end of namespace TcpNet");

            Util.FlushAndReleaseTextFile(w);
        }

        static private void Write_Code_PacketSendFunc(StreamWriter w, PacketInfos infos, PacketInfos.Packet p, string tab)
        {
            w.Write("{0}public void {1}(", tab, p.cmd_name);
            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];
                if (elem.vtype == "string")
                {
                    w.Write("string {0}", elem.vname.ToLower());
                }
                else
                {
                    if (elem.vcount > 0)
                    {
                        w.Write("{0}[] {1}", Util.ConvertTypeUnity(elem.vtype), elem.vname.ToLower());
                    }
                    else
                    {
                        w.Write("{0} {1}", Util.ConvertTypeUnity(elem.vtype), elem.vname.ToLower());
                    }
                }

                if (i < p.elems.Count - 1)
                {
                    w.Write(", ");
                }
            }

            if (p.elems.Count == 0)
            {
                w.Write("bool isData = false)", tab);
            }
            else
            {
                w.Write(", bool isData = false)", tab);
            }

            w.WriteLine("");

            w.WriteLine("{0}{{", tab);


            w.WriteLine("{0}NP{1}C2S_{2} packet = new NP{3}C2S_{4}();", tab + Util.Tab, infos.mServerName, p.cmd_name, infos.mServerName, p.cmd_name);

            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];

                w.WriteLine("{0}packet.m{1} = {2};", tab + Util.Tab, elem.vname, elem.vname.ToLower());
            }

            w.WriteLine("{0}SendOrAdd(packet, isData);", tab + Util.Tab);


            w.WriteLine("{0}}}", tab);

            w.WriteLine("");
        }
    }
}
