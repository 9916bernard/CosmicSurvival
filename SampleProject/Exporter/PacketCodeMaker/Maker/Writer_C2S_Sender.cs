using System;
using System.IO;


namespace PacketCodeMaker
{
    public class Writer_C2S_Sender
    {
        static public void Write(Util.PathInfo pathInfo, PacketInfos infos)
        {
            string filepath_client_header = string.Format("{0}Net_{1}_C2S.h", pathInfo.mCodePathClient, infos.mServerName);
            string filepath_bot_header = string.Format("{0}Net_{1}_C2S.h", pathInfo.mCodePathBot, infos.mServerName);

            string filepath_client_source = string.Format("{0}Net_{1}_C2S.cpp", pathInfo.mCodePathClient, infos.mServerName);
            string filepath_bot_source = string.Format("{0}Net_{1}_C2S.cpp", pathInfo.mCodePathBot, infos.mServerName);


            Write_Header(filepath_client_header, filepath_bot_header, infos);
            Console.WriteLine("=== Write {0}", filepath_client_header);
            Console.WriteLine("=== Write {0}", filepath_bot_header);

            Write_Source(filepath_client_source, filepath_bot_source, infos);
            Console.WriteLine("=== Write {0}", filepath_client_source);
            Console.WriteLine("=== Write {0}", filepath_bot_source);
        }

        static private void Write_Header(string filepath_client, string filepath_bot, PacketInfos infos)
        {
            StreamWriter w = Util.GetTextFile(filepath_client);


            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("#include \"CoreMinimal.h\"");
            w.WriteLine("#include \"./Types/GE.h\"");
            w.WriteLine("#include \"./Types/RE.h\"");
            w.WriteLine("#include \"./Types/GS.h\"");
            w.WriteLine("#include \"./Lib/NetSender.h\"");
            w.WriteLine("#include \"Net_{0}_C2S.generated.h\"", infos.mServerName);
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("UCLASS()");
            w.WriteLine("class {0} UNet_{1}_C2S : public UNetSender", Util.API, infos.mServerName);
            w.WriteLine("{");
            w.WriteLine("{0}GENERATED_BODY()", Util.Tab);
            w.WriteLine("");

            w.WriteLine("public:");

            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "C2S")
                {
                    Write_Header_Packet(w, infos, p);
                }
            }

            w.WriteLine("};");

            Util.FlushAndReleaseTextFile(w);

            //File.Copy(filepath_client, filepath_bot, true);
        }

        static private void Write_Header_Packet(StreamWriter w, PacketInfos infos, PacketInfos.Packet p)
        {
            w.Write("{0}void {1}(", Util.Tab, p.cmd_name);
            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];
                if (elem.vtype == "string")
                {
                    w.Write("const FString& {0}", elem.vname.ToLower());
                }
                else
                {
                    if (elem.vcount > 0)
                    {
                        w.Write("const {0}* {1}", elem.vtype, elem.vname.ToLower());
                    }
                    else
                    {
                        w.Write("{0} {1}", elem.vtype, elem.vname.ToLower());
                    }
                }

                if (i < p.elems.Count - 1)
                {
                    w.Write(", ");
                }
            }
            w.Write(");");

            w.WriteLine("");
        }

        static private void Write_Source(string filepath_client, string filepath_bot, PacketInfos infos)
        {
            StreamWriter w = Util.GetTextFile(filepath_client);


            w.WriteLine("#include \"./Net_{0}_C2S.h\"", infos.mServerName);
            w.WriteLine("#include \"./Packet/NetPacket_{0}_C2S.h\"", infos.mServerName);
            w.WriteLine("");
            w.WriteLine("");

            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "C2S")
                {
                    Write_Source_Packet(w, infos, p);
                }
            }

            Util.FlushAndReleaseTextFile(w);

            //File.Copy(filepath_client, filepath_bot, true);
        }

        static private void Write_Source_Packet(StreamWriter w, PacketInfos infos, PacketInfos.Packet p)
        {
            w.Write("void UNet_{0}_C2S::{1}(", infos.mServerName, p.cmd_name);
            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];
                if (elem.vtype == "string")
                {
                    w.Write("const FString& {0}", elem.vname.ToLower());
                }
                else
                {
                    if (elem.vcount > 0)
                    {
                        w.Write("const {0}* {1}", elem.vtype, elem.vname.ToLower());
                    }
                    else
                    {
                        w.Write("{0} {1}", elem.vtype, elem.vname.ToLower());
                    }
                }

                if (i < p.elems.Count - 1)
                {
                    w.Write(", ");
                }
            }
            w.Write(")");

            w.WriteLine("");

            w.WriteLine("{");

            if (p.elems.Count == 0)
            {
                w.WriteLine("{0}NP{1}C2S_{2} packet;", Util.Tab, infos.mServerName, p.cmd_name);
            }
            else
            {
            
                w.Write("{0}NP{1}C2S_{2} packet(", Util.Tab, infos.mServerName, p.cmd_name);
                for (int i = 0; i < p.elems.Count; ++i)
                {
                    PacketInfos.Element elem = p.elems[i];

                    w.Write("{0}", elem.vname.ToLower());

                    if (i < p.elems.Count - 1)
                    {
                        w.Write(", ");
                    }
                }
                w.Write(");");
                w.WriteLine("");
            }

            w.WriteLine("{0}Send((uint8*)&packet, packet.GetLen());", Util.Tab);
            w.WriteLine("}");

            w.WriteLine("");
        }
    }
}
