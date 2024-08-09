using System;


namespace Table.Exporter
{
    class Types
    {
        static public int GetSize(string type)
        {
            switch (type)
            {
                case "int8": case "uint8": case "bool": return 1;
                case "int16": case "uint16": return 2;
                case "int32": case "uint32": case "float": return 4;
                case "int64": case "uint64": return 8;
            }
            return 0;
        }
    }
}
