using System;
using NPOI.SS.UserModel;


namespace PacketCodeMaker
{
    public class PacketReader
    {
        static readonly int PACKET_START = 0;
        static readonly int PACKET_NAME = 1;
        static readonly int PACKET_CMD = 2;
        static readonly int PACKET_WAY = 3;

        static readonly int ELEMENT_START = 1;
        static readonly int ELEMENT_NAME = 2;
        static readonly int ELEMENT_TYPE = 3;
        static readonly int ELEMENT_COUNT = 4;


        static public PacketInfos Read(IWorkbook book)
        {
            PacketInfos infos = new PacketInfos();

            for (int i = 0; i < book.NumberOfSheets; ++i)
            {
                ReadSheet(book.GetSheetAt(i), infos);
            }

            return infos;
        }

        static public void ReadSheet(ISheet sheet, PacketInfos infos)
        {
            PacketInfos.Packet packet = null;

            for (int i = 0; i <= sheet.LastRowNum; ++i)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }

                // 패킷 시작점인가?
                if (GetCell(row, PACKET_START) == "#")
                {
                    if (packet != null && infos.Add(packet) == false)
                    {
                        // error;
                        return;
                    }

                    packet = new PacketInfos.Packet();

                    packet.cmd_name = GetCell(row, PACKET_NAME);
                    if (packet.cmd_name == "")
                    {
                        // error;
                        return;
                    }

                    string cmd = GetCell(row, PACKET_CMD);
                    packet.cmd = Convert.ToInt32(cmd);
                    if (packet.cmd <= 0)
                    {
                        // error;
                        return;
                    }

                    packet.way = GetCell(row, PACKET_WAY);
                    if (packet.way != "C2S" && packet.way != "S2C")
                    {
                        // error;
                        return;
                    }

                    continue;
                }

                // 엘리먼트 시작점인가?
                if (GetCell(row, ELEMENT_START) == "*")
                {
                    if (packet == null)
                    {
                        // error;
                        return;
                    }

                    PacketInfos.Element elem = new PacketInfos.Element();

                    elem.vname = GetCell(row, ELEMENT_NAME);
                    if (elem.vname == "")
                    {
                        // error;
                        return;
                    }

                    if (packet.elems.Find(x => x.vname.CompareTo(elem.vname) == 0) != null)
                    {
                        // error;
                        return;
                    }

                    elem.vtype = GetCell(row, ELEMENT_TYPE);
                    if (elem.vtype == "")
                    {
                        // error;
                        return;
                    }

                    string cnt = GetCell(row, ELEMENT_COUNT);
                    if (cnt != "")
                    {
                        elem.vcount = Convert.ToInt32(cnt);

                        if (elem.vcount < 0)
                        {
                            // error
                            return;
                        }
                    }

                    if (elem.vtype == "string" && elem.vcount <= 0)
                    {
                        // error;
                        return;
                    }

                    packet.elems.Add(elem);

                    continue;
                }
            }

            if (packet != null && infos.Add(packet) == false)
            {
                // error;
                return;
            }
        }

        static string GetCell(IRow row, int columnIdx)
        {
            ICell cell = row.GetCell(columnIdx);

            if (cell == null)
            {
                return "";
            }

            if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue.Trim();
            }
            else if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
