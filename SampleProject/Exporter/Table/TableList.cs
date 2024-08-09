using System.IO;
using System.Collections.Generic;


namespace Table
{
    class TableList
    {
        static public void LoadTableList(System.Windows.Forms.ListView listView)
        {
            // 초기화
            Exporter.Maker.Clear();

            // 엑셀파일 수집
            Exporter.Maker.ClassifyTable();

            //listView.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
            //listView.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.HeaderSize);

            listView.Items.Clear();

            for (int i = 0; i < Exporter.TBL_Config.ExcelList.Count; ++i)
            {
                listView.Items.Add(Path.GetFileName(Exporter.TBL_Config.ExcelList[i]));
            }

            for (int i = 0; i < Exporter.TBL_Normal.ExcelList.Count; ++i)
            {
                listView.Items.Add(Path.GetFileName(Exporter.TBL_Normal.ExcelList[i]));
            }
        }

        static public void Make(System.Windows.Forms.ListView listView, bool exportCode)
        {
            List<string> configList = new List<string>();
            List<string> normalList = new List<string>();

            for (int i = 0; i < listView.SelectedItems.Count; ++i)
            {
                string [] cmds = listView.SelectedItems[i].Text.Split('_');

                string[] filepath = { Settings.Table_Excel, listView.SelectedItems[i].Text };

                if (cmds.Length > 1 && cmds[0].ToLower() == "config")
                {
                    configList.Add(Path.Combine(filepath));
                }
                else
                {
                    normalList.Add(Path.Combine(filepath));
                }
            }


            // Clear
            Exporter.Maker.Clear();

            // 엑셀파일 수집
            Exporter.Maker.ClassifyTable();


            // Enum & Struct 수집 from System_Enum
            Exporter.TBL_System.Make(exportCode);

            // Config Table
            Exporter.TBL_Config.MakeWithList(configList, exportCode);

            // String Table

            // Normal Table
            Exporter.TBL_Normal.MakeWithList(normalList, exportCode);
        }

        //static public void OpenSettingFolder(string text)
        //{
        //    string[] s = text.Split('=');

        //    if (s.Length < 2)
        //        return;

        //    string title = s[0].Trim().ToLower();

        //    if (title == "table_excel" || title == "code_server" || title == "code_client" || title == "code_bot" || title == "data_server" || title == "data_client" || title == "patch_txt")
        //    {
        //        Util.OpenFolder(s[1].Trim());
        //    }
        //}
    }
}
