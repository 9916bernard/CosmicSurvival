using System;
using NPOI.SS.UserModel;


namespace Table.Exporter
{
    partial class TBL_Normal
    {
        static readonly int COLUMN_MARK = 0;
        static readonly int COLUMN_DEFINESTART = 1;

        static int COLUMN_FieldName = 0;
        static int COLUMN_DataType = 0;
        static int COLUMN_ValueType = 0;
        static int COLUMN_ValueMax = 0;
        static int COLUMN_KeyType = 0;
        static int COLUMN_Usage = 0;
        static int COLUMN_Array = 0;

        static public void ReadDefine(ISheet sheet, Defines dfn)
        {
            int defineStartRow = 0;

            for (int i = 0; i <= sheet.LastRowNum; ++i)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }

                string cell = Excel.GetCell(row, COLUMN_MARK);

                if (cell == "*")
                {
                    defineStartRow = i + 1;

                    for (int j = COLUMN_DEFINESTART; j <= row.LastCellNum; ++j)
                    {
                        string keyword = Excel.GetCell(row, j).ToLower();
                        switch (keyword)
                        {
                            case "fieldname": COLUMN_FieldName = j; break;
                            case "datatype": COLUMN_DataType = j; break;
                            case "valuetype": COLUMN_ValueType = j; break;
                            case "valuemax": COLUMN_ValueMax = j; break;
                            case "keytype": COLUMN_KeyType = j; break;
                            case "usage": COLUMN_Usage = j; break;
                            case "array": COLUMN_Array = j; break;
                        }
                    }

                    break;
                }
            }

            for (int i = defineStartRow; i <= sheet.LastRowNum; ++i)
            {
                CurrentState.Row = i;


                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }

                string mark = Excel.GetCell(row, COLUMN_MARK);
                if (Excel.IsComment(mark) == true)
                {
                    continue;
                }

                // Usage
                string strusage = Excel.GetCell(row, COLUMN_Usage);
                Defines.Usage usage = Defines.Usage.None;
                if (strusage != "")
                {
                    byte byteusage = Convert.ToByte(strusage);
                    usage = (Defines.Usage)byteusage;
                }

                if (usage == Defines.Usage.None)
                {
                    continue;
                }

                Defines.Element elem = new Defines.Element();

                // FieldName
                elem.mFieldName = Excel.GetCell(row, COLUMN_FieldName);
                if (elem.mFieldName == "" || Excel.IsComment(elem.mFieldName) == true)
                {
                    continue;
                }

                // DataType
                elem.mDataType = Excel.GetCell(row, COLUMN_DataType).ToLower();
                if (elem.mDataType == "")
                {
                    Log.E("DataType is empty FieldName : {0}", elem.mFieldName);
                    continue;
                }

                // ValueType
                elem.mValueType = Excel.GetCell(row, COLUMN_ValueType);

                // ValueMax
                string strvaluemax = Excel.GetCell(row, COLUMN_ValueMax);
                if (strvaluemax != "")
                {
                    elem.mValueMax = Convert.ToInt64(strvaluemax);
                }

                // KeyType
                string strkeytype = Excel.GetCell(row, COLUMN_KeyType);
                if (strkeytype != "")
                {
                    byte bytekeytype = Convert.ToByte(strkeytype);
                    elem.mKeyType = (Defines.KeyType)bytekeytype;
                }

                // Usage
                elem.mUsage = usage;

                // Array
                elem.mArray = (Excel.GetCell(row, COLUMN_Array) == "1");


                dfn.AddElement(elem);
            }
        }
    }
}
