using System;
using System.IO;


namespace PacketCodeMaker
{
    public class Writer_PacketHeaderS2C
    {
        static public void Write(Util.PathInfo pathInfo, PacketInfos infos, bool writeBot, bool writeClient)
        {
            string server_directory = pathInfo.mCodePathServer + "Packet/";

            Util.MakeFolder(server_directory);

            string filepath_server = string.Format("{0}NetPacket_{1}_S2C.h", server_directory, infos.mServerName);

            Write_S2C(filepath_server, infos, true);
            Console.WriteLine("=== Write {0}", filepath_server);


            if (writeBot == true)
            {
                string bot_directory = pathInfo.mCodePathBot + "Packet/";

                Util.MakeFolder(bot_directory);

                string filepath_bot = string.Format("{0}NetPacket_{1}_S2C.h", bot_directory, infos.mServerName);

                Write_S2C(filepath_bot, infos, false);
                Console.WriteLine("=== Write {0}", filepath_bot);
            }

            if (writeClient == true)
            {
                string client_directory = pathInfo.mCodePathClient + "Packet/";

                Util.MakeFolder(client_directory);

                string filepath_client = string.Format("{0}NetPacket_{1}_S2C.h", client_directory, infos.mServerName);

                Write_S2C(filepath_client, infos, false);
                Console.WriteLine("=== Write {0}", filepath_client);
            }
        }

        static private void Write_S2C(string filepath, PacketInfos infos, bool isServer)
        {
            StreamWriter w = Util.GetTextFile(filepath);


            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("#include \"./NetPacket_{0}.h\"", infos.mServerName);
            w.WriteLine("");
            w.WriteLine("");
            w.WriteLine("#pragma pack(1)");
            w.WriteLine("");
            w.WriteLine("");



            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "S2C")
                {
                    Write_Packet(w, infos, p, isServer);
                }
            }



            w.WriteLine("");
            w.WriteLine("#pragma pack()");


            Util.FlushAndReleaseTextFile(w);
        }

        static private void Write_Packet(StreamWriter w, PacketInfos infos, PacketInfos.Packet p, bool isServer)
        {
            w.WriteLine("struct NP{0}S2C_{1} : public NP{2}S2CHeader", infos.mServerName, p.cmd_name, infos.mServerName);
            w.WriteLine("{");


            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];
                if (elem.vcount <= 0)
                {
                    w.WriteLine("{0}{1} m{2};", Util.Tab, elem.ConvertType(isServer), elem.vname);
                }
                else
                {
                    if (elem.vtype == "string")
                    {
                        w.WriteLine("{0}{1} m{2}[{3}];", Util.Tab, elem.ConvertType(isServer), elem.vname, elem.vcount + 1);
                    }
                    else
                    {
                        w.WriteLine("{0}{1} m{2}[{3}];", Util.Tab, elem.ConvertType(isServer), elem.vname, elem.vcount);
                    }
                }
            }

            if (isServer == false)
            {
                for (int i = 0; i < p.elems.Count; ++i)
                {
                    PacketInfos.Element elem = p.elems[i];

                    if (elem.vtype == "string")
                    {
                        w.WriteLine("{0}FString Get{1}() {{ return FString(TCHAR_TO_UTF16(m{2})); }}", Util.Tab, elem.vname, elem.vname);
                    }
                }
            }

            //if (isServer == true && p.elems.Count > 0)
            if (p.elems.Count > 0)
            {
                w.WriteLine("{0}NP{1}S2C_{2}() : NP{3}S2CHeader(sizeof(*this), NP_{4}_S2C::{5}) {{}}", Util.Tab, infos.mServerName, p.cmd_name, infos.mServerName, infos.mServerNameUP, p.cmd_name.ToUpper());
            }

            w.Write("{0}NP{1}S2C_{2}(", Util.Tab, infos.mServerName, p.cmd_name);
            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];
                if (elem.vtype == "string")
                {
                    if (isServer == true)
                    {
                        w.Write("const wchar_t* {0}", elem.vname.ToLower());
                    }
                    else
                    {
                        w.Write("const FString& {0}", elem.vname.ToLower());
                    }
                }
                else if (Util.IsStruct(elem.vtype) == true)
                {
                    if (elem.vcount > 0)
                    {
                        w.Write("const {0}* {1}", elem.ConvertType(isServer), elem.vname.ToLower());
                    }
                    else
                    {
                        w.Write("const {0}& {1}", elem.ConvertType(isServer), elem.vname.ToLower());
                    }
                }
                else
                {
                    if (elem.vcount > 0)
                    {
                        w.Write("const {0}* {1}", elem.ConvertType(isServer), elem.vname.ToLower());
                    }
                    else
                    {
                        w.Write("{0} {1}", elem.ConvertType(isServer), elem.vname.ToLower());
                    }
                }

                if (i < p.elems.Count - 1)
                {
                    w.Write(", ");
                }
            }
            w.Write(")");

            w.WriteLine("");

            w.WriteLine("{0}{1}: NP{2}S2CHeader(sizeof(*this), NP_{3}_S2C::{4})", Util.Tab, Util.Tab, infos.mServerName, infos.mServerNameUP, p.cmd_name.ToUpper());

            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];

                if (elem.vcount <= 0)
                {
                    w.WriteLine("{0}{1}, m{2}({3})", Util.Tab, Util.Tab, elem.vname, elem.vname.ToLower());
                }
            }

            w.WriteLine("{0}{{", Util.Tab);

            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];

                if (elem.vtype == "string")
                {
                    if (isServer == true)
                    {
                        w.WriteLine("{0}{1}memcpy_s(m{2}, {3}, {4}, wcsnlen_s({5}, {6}) * 2);", Util.Tab, Util.Tab, elem.vname, (elem.vcount + 1) * 2, elem.vname.ToLower(), elem.vname.ToLower(), elem.vcount);
                        w.WriteLine("{0}{1}m{2}[wcsnlen_s({3}, {4})] = 0;", Util.Tab, Util.Tab, elem.vname, elem.vname.ToLower(), elem.vcount);
                    }
                    else
                    {
                        w.WriteLine("{0}{1}FMemory::Memcpy(m{2}, *{3}, ({4}.GetCharArray().Num() >= {5} ? {6} : {7}.GetCharArray().Num()) * 2);", Util.Tab, Util.Tab, elem.vname, elem.vname.ToLower(), elem.vname.ToLower(), elem.vcount + 1, elem.vcount + 1, elem.vname.ToLower());
                        w.WriteLine("{0}{1}m{2}[{3}] = 0;", Util.Tab, Util.Tab, elem.vname, elem.vcount);
                    }
                }
                else if (elem.vcount > 0)
                {
                    if (isServer == true)
                    {
                        w.WriteLine("{0}{1}memcpy_s(m{2}, {3} * sizeof({4}), {5}, {6} * sizeof({7}));", Util.Tab, Util.Tab, elem.vname, elem.vcount, elem.ConvertType(isServer), elem.vname.ToLower(), elem.vcount, elem.ConvertType(isServer));
                    }
                    else
                    {
                        w.WriteLine("{0}{1}FMemory::Memcpy(m{2}, {3}, {4} * sizeof({5}));", Util.Tab, Util.Tab, elem.vname, elem.vname.ToLower(), elem.vcount, elem.vtype);
                    }
                }
            }

            w.WriteLine("{0}}}", Util.Tab);


            w.WriteLine("};");
            w.WriteLine("");
        }
    }
}
