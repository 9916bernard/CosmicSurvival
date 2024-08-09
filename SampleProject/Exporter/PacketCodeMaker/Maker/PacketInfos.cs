using System;
using System.Collections.Generic;



namespace PacketCodeMaker
{
    public class PacketInfos
    {
        public class Element
        {
            public string vname = "";
            public string vtype = "";
            public int vcount = 0;

            public string ConvertType(bool isServer)
            {
                if (isServer == false)
                {
                    return vtype == "string" ? "uint16" : vtype;
                }

                switch (vtype)
                {
                    //case "uint8": return "unsigned char";
                    //case "uint16": return "unsigned short";
                    //case "uint32": return "unsigned int";
                    //case "uint64": return "unsigned __int64";
                    //case "int8": return "char";
                    //case "int16": return "short";
                    //case "int32": return "int";
                    //case "int64": return "__int64";
                    case "string": return "wchar_t";
                    default: return vtype;
                }
            }

            public string ConvertTypeUnity()
            {
                return Util.ConvertTypeUnity(vtype);
            }
        }

        public class Packet
        {
            public int cmd = 0;
            public string cmd_name = "";
            public string way = "";
            public List<Element> elems = new List<Element>();
        }

        public string mServerName = "";
        public string mServerNameUP = "";
        public SortedDictionary<int, Packet> mPackets = new SortedDictionary<int, Packet>();
        public List<string> mPacketNames = new List<string>();

        public void SetServerName(string serverName)
        {
            mServerName = serverName;
            mServerNameUP = serverName.ToUpper();
        }

        public bool Add(Packet packet)
        {
            if (mPackets.ContainsKey(packet.cmd) == true)
            {
                return false;
            }

            if (mPacketNames.Find(x => x.CompareTo(packet.cmd_name) == 0) != null)
            {
                return false;
            }

            mPackets.Add(packet.cmd, packet);
            mPacketNames.Add(packet.cmd_name);

            return true;
        }
    }
}
