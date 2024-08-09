using System;


namespace Table.Exporter
{
    class CurrentState
    {
        static public string TableName = "";
        static public string SheetName = "";
        static public int Row = -1;
        static public string Field = "";

        static public void Clear()
        {
            TableName = "";
            SheetName = "";
            Row = -1;
            Field = "";
        }

        static public string GetStr()
        {
            return string.Format("[Table:{0}, Sheet:{1}, Row:{2}, Field:{3}]", TableName, SheetName, Row, Field);
        }
    }
}
