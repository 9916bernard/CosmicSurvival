using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Table
{
    public partial class MainPanel : Form
    {
        public MainPanel()
        {
            InitializeComponent();

            Settings.LoadSettingFile(ListView_Setting);

            TableList.LoadTableList(ListView_Table);
        }

        private void Btn_ExportAll_Click(object sender, EventArgs e)
        {
            Log.Start("=== [code + data make start] : {0} ===", DateTime.Now.ToString());
            Text_Status.Text = "START";

            Exporter.Maker.Make(true);

            Text_Status.Text = Log.ErrorCount > 0 ? "FAILED" : "COMPLETE";
            Log.N("=== [make {0}] ===", Text_Status.Text);
            Log.Flush();

            if (Log.ErrorCount > 0)
            {
                using (new Table.Exporter.CenterWinDialog(this))
                {
                    if (MessageBox.Show("에러 발견 log.txt 파일을 열어보세요. ('예' 누르면 열림)", "ERROR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //
                        Util.OpenFile(Log.LOG_FILE);
                    }
                }
            }
        }

        private void Btn_ExportData_Click(object sender, EventArgs e)
        {
            Log.Start("=== [data make start] : {0} ===", DateTime.Now.ToString());
            Text_Status.Text = "START";

            Exporter.Maker.Make(false);

            Text_Status.Text = Log.ErrorCount > 0 ? "FAILED" : "COMPLETE";
            Log.N("=== [make {0}] ===", Text_Status.Text);
            Log.Flush();

            if (Log.ErrorCount > 0)
            {
                using (new Table.Exporter.CenterWinDialog(this))
                {
                    if (MessageBox.Show("에러 발견 log.txt 파일을 열어보세요. ('예' 누르면 열림)", "ERROR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //
                        Util.OpenFile(Log.LOG_FILE);
                    }
                }
            }
        }

        private void Btn_OpenSetting_Click(object sender, EventArgs e)
        {
            try
            {
                Util.OpenFile(Settings.CFG_FILE);
            }
            catch (System.Exception ee)
            {
                Log.E("[CRASH] {0}\n{1}\n", ee.Message, ee.StackTrace);
            }
        }

        private void Btn_OpenLog_Click(object sender, EventArgs e)
        {
            try
            {
                Util.OpenFile(Log.LOG_FILE);
            }
            catch (System.Exception ee)
            {
                Log.E("[CRASH] {0}\n{1}\n", ee.Message, ee.StackTrace);
            }
        }

        private void Btn_SettingReload_Click(object sender, EventArgs e)
        {
            Settings.LoadSettingFile(ListView_Setting);
        }

        private void ListView_Setting_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = ((System.Windows.Forms.ListView)sender).FocusedItem;

            Settings.OpenSettingFolder(item.Text);
        }

        private void Btn_SelectedExportAll_Click(object sender, EventArgs e)
        {
            Log.Start("=== [(selected) code + data make start] : {0} ===", DateTime.Now.ToString());
            Text_Status.Text = "START";

            Table.TableList.Make(ListView_Table, true);

            Text_Status.Text = Log.ErrorCount > 0 ? "FAILED" : "COMPLETE";
            Log.N("=== [make {0}] ===", Text_Status.Text);
            Log.Flush();

            if (Log.ErrorCount > 0)
            {
                using (new Table.Exporter.CenterWinDialog(this))
                {
                    if (MessageBox.Show("에러 발견 log.txt 파일을 열어보세요. ('예' 누르면 열림)", "ERROR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //
                        Util.OpenFile(Log.LOG_FILE);
                    }
                }
            }
        }

        private void Btn_SelectedExportData_Click(object sender, EventArgs e)
        {
            Log.Start("=== [(selected) code + data make start] : {0} ===", DateTime.Now.ToString());
            Text_Status.Text = "START";

            Table.TableList.Make(ListView_Table, false);

            Text_Status.Text = Log.ErrorCount > 0 ? "FAILED" : "COMPLETE";
            Log.N("=== [make {0}] ===", Text_Status.Text);
            Log.Flush();

            if (Log.ErrorCount > 0)
            {
                using (new Table.Exporter.CenterWinDialog(this))
                {
                    if (MessageBox.Show("에러 발견 log.txt 파일을 열어보세요. ('예' 누르면 열림)", "ERROR", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //
                        Util.OpenFile(Log.LOG_FILE);
                    }
                }
            }
        }

        private void Btn_PatchList_Click(object sender, EventArgs e)
        {
            // Patch List
            Exporter.Maker.WritePatchList();
        }
    }
}
