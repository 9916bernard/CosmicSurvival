using NPOI.SS.UserModel;


namespace Table.Exporter
{
    partial class TBL_System
    {
        //===============================================================================================
        // ENUM 시작
        //===============================================================================================
        static readonly int COLUMN_ENUM_MARK = 0;
        static readonly int COLUMN_ENUM_NAME = 1;
        static readonly int COLUMN_ENUM_TYPE = 2;

        static readonly int COLUMN_ENUM_ELEM_NAME = 1;
        static readonly int COLUMN_ENUM_ELEM_VALUE1 = 2;
        static readonly int COLUMN_ENUM_ELEM_VALUE2 = 4;

        static public void ReadEnum(ISheet sheet, bool isJson)
        {
            Enums.Info enum_info = null;

            for (int i = 0; i <= sheet.LastRowNum; ++i)
            {
                CurrentState.Row = i;

                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }

                string mark = Excel.GetCell(row, COLUMN_ENUM_MARK);
                if (Excel.IsComment(mark) == true)
                {
                    continue;
                }

                // ENUM
                if (mark == "*" || mark == ">")
                {
                    if (enum_info != null)
                    {
                        Enums.Add(enum_info);
                        enum_info = null;
                    }

                    string ename = Excel.GetCell(row, COLUMN_ENUM_NAME);
                    string etype = Excel.GetCell(row, COLUMN_ENUM_TYPE);

                    if (ename == "" || etype == "")
                    {
                        Log.E("Enum name or type is empty : {0}, {1}", ename, etype);
                        continue;
                    }

                    enum_info = new Enums.Info(ename, etype, isJson);
                }
                else
                {
                    if (enum_info == null)
                    {
                        continue;
                    }

                    string elem_name = Excel.GetCell(row, COLUMN_ENUM_ELEM_NAME);
                    string elem_value1 = Excel.GetCell(row, COLUMN_ENUM_ELEM_VALUE1);
                    string elem_value2 = Excel.GetCell(row, COLUMN_ENUM_ELEM_VALUE2);

                    if (elem_name == "" || Excel.IsComment(elem_name) == true)
                    {
                        continue;
                    }

                    if (elem_value1 == "")
                    {
                        Log.E("Enum value is incorrect : {0}, {1}", enum_info.mName, enum_info.mType);
                        continue;
                    }

                    enum_info.Add(elem_name, elem_value1, elem_value2);
                }
            }

            if (enum_info != null)
            {
                Enums.Add(enum_info);
                enum_info = null;
            }
        }
        //===============================================================================================
        // ENUM 끝
        //===============================================================================================



        //===============================================================================================
        // STRUCT 시작
        //===============================================================================================
        static readonly int COLUMN_STRUCT_MARK = 0;
        static readonly int COLUMN_STRUCT_NAME = 1;

        static readonly int COLUMN_STRUCT_ELEM_NAME = 1;
        static readonly int COLUMN_STRUCT_ELEM_VALUE1 = 2;
        static readonly int COLUMN_STRUCT_ELEM_VALUE2 = 3;

        static public void ReadStruct(ISheet sheet)
        {
            Structs.Info struct_info = null;

            for (int i = 0; i <= sheet.LastRowNum; ++i)
            {
                CurrentState.Row = i;

                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }

                string mark = Excel.GetCell(row, COLUMN_STRUCT_MARK);
                if (Excel.IsComment(mark) == true)
                {
                    continue;
                }

                // STURCT
                if (mark == "*" || mark == ">")
                {
                    if (struct_info != null)
                    {
                        Structs.Add(struct_info);
                        struct_info = null;
                    }

                    string ename = Excel.GetCell(row, COLUMN_STRUCT_NAME);

                    if (ename == "")
                    {
                        Log.E("Struct name is empty : {0}");
                        continue;
                    }

                    struct_info = new Structs.Info() { mName = ename };
                }
                else
                {
                    if (struct_info == null)
                    {
                        continue;
                    }

                    string elem_name = Excel.GetCell(row, COLUMN_STRUCT_ELEM_NAME);
                    string elem_value1 = Excel.GetCell(row, COLUMN_STRUCT_ELEM_VALUE1);
                    string elem_value2 = Excel.GetCell(row, COLUMN_STRUCT_ELEM_VALUE2);

                    if (elem_name == "" || Excel.IsComment(elem_name) == true)
                    {
                        continue;
                    }

                    if (elem_value1 == "")
                    {
                        Log.E("Struct value is incorrect : {0}", struct_info.mName);
                        continue;
                    }

                    struct_info.Add(elem_name, elem_value1, elem_value2);
                }
            }

            if (struct_info != null)
            {
                Structs.Add(struct_info);
                struct_info = null;
            }
        }
        //===============================================================================================
        // STRUCT 끝
        //===============================================================================================
    }
}
