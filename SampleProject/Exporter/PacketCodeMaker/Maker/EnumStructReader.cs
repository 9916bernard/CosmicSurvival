using NPOI.SS.UserModel;


namespace PacketCodeMaker
{
    public partial class EnumStructReader
    {
        static readonly int COLUMN_ENUM_MARK = 0;
        static readonly int COLUMN_ENUM_NAME = 1;
        static readonly int COLUMN_ENUM_TYPE = 2;

        static readonly int COLUMN_ENUM_ELEM_NAME = 1;
        static readonly int COLUMN_ENUM_ELEM_VALUE1 = 2;
        static readonly int COLUMN_ENUM_ELEM_VALUE2 = 3;


        static public EnumStructInfos Read(IWorkbook book)
        {
            EnumStructInfos infos = new EnumStructInfos();

            for (int i = 0; i < book.NumberOfSheets; ++i)
            {
                ReadSheet(book.GetSheetAt(i), infos);
            }

            return infos;
        }

        static public void ReadSheet(ISheet sheet, EnumStructInfos infos)
        {
            EnumStructInfos.EnumInfo enum_info = null;
            EnumStructInfos.StructInfo struct_info = null;

            for (int i = 0; i <= sheet.LastRowNum; ++i)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }

                string mark = GetCell(row, COLUMN_ENUM_MARK);
                if (Util.IsComment(mark) == true)
                {
                    continue;
                }

                // ENUM or STRUCT
                if (mark == "*" || mark == ">")
                {
                    if (enum_info != null)
                    {
                        if (infos.AddEnum(enum_info) == false)
                        {
                            // error
                        }
                        enum_info = null;
                    }

                    if (struct_info != null)
                    {
                        if (infos.AddStruct(struct_info) == false)
                        {
                            // error
                        }
                        struct_info = null;
                    }

                    string ename = GetCell(row, COLUMN_ENUM_NAME);
                    string etype = GetCell(row, COLUMN_ENUM_TYPE);

                    if (mark == "*")
                    {
                        if (ename == "" || etype == "")
                        {
                            // error
                            continue;
                        }

                        enum_info = new EnumStructInfos.EnumInfo() { mName = ename, mType = etype };
                        enum_info.SetSize();
                    }
                    else
                    {
                        if (ename == "")
                        {
                            // error
                            continue;
                        }

                        struct_info = new EnumStructInfos.StructInfo() { mName = ename };
                    }
                }
                else
                {
                    if (enum_info == null && struct_info == null)
                    {
                        // error
                        continue;
                    }

                    string elem_name = GetCell(row, COLUMN_ENUM_ELEM_NAME);
                    string elem_value1 = GetCell(row, COLUMN_ENUM_ELEM_VALUE1);
                    string elem_value2 = GetCell(row, COLUMN_ENUM_ELEM_VALUE2); // struct 에서는 배열 수

                    if (elem_name == "")
                    {
                        continue;
                    }

                    if (elem_value1 == "")
                    {
                        elem_value1 = "0";
                    }

                    if (enum_info != null && enum_info.Add(elem_name, elem_value1, elem_value2) == false)
                    {
                        // error
                        continue;
                    }

                    if (struct_info != null && struct_info.Add(elem_name, elem_value1, elem_value2) == false)
                    {
                        // error
                        continue;
                    }
                }
            }

            if (enum_info != null)
            {
                if (infos.AddEnum(enum_info) == false)
                {
                    // error
                }
                enum_info = null;
            }

            if (struct_info != null)
            {
                if (infos.AddStruct(struct_info) == false)
                {
                    // error
                }
                struct_info = null;
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
