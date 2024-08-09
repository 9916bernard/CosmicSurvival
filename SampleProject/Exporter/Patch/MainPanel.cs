using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Patch
{
    public partial class MainPanel : Form
    {
        public MainPanel()
        {
            InitializeComponent();

            Connect_Panel();

            PatchFile.InitPatchFile();
            PatchFile.CompareToFileInfo();
            PatchFile.SetListViewDifElement();
        }

        private void Connect_Panel()
        {
            PatchFile.mListView_Version1 = ListView_Version1;
            PatchFile.mListView_Version2 = ListView_Version2;
            PatchFile.mListView_Dif = ListView_VersionDif;

            PatchFile.mText_Version1 = Text_Version1;
            PatchFile.mText_Version2 = Text_Version2;
            PatchFile.mText_Version1Path = Text_Version1Path;
            PatchFile.mText_Version2Path = Text_Version2Path;
            PatchFile.mText_Version1FileCount = Text_Version1FileCount;
            PatchFile.mText_Version2FileCount = Text_Version2FileCount;

            PatchFile.mButton_VersionUpdateButton = VersionUpdate;
            PatchFile.mButton_DirectorySettingButton = Button_OpenDirecotyFile;
            PatchFile.mButton_CopyDirectorySettingButton = Button_SetCopyTableDirectory;
            PatchFile.mButton_CreateFile = ButtonCreateFile;
        }

        private void VersionUpdate_Click(object sender, EventArgs e)
        {
            PatchFile.PatchVersionUpdate();
        }

        private void FileOrderby_Click(object sender, EventArgs e)
        {
            PatchFile.IsFileAscending = !PatchFile.IsFileAscending;
            Button_FileOrderBy.Text = PatchFile.IsFileAscending ? "파일 정렬 : 오름차순" : "파일 정렬 : 내림차순";

            PatchFile.InitPatchFile();
            PatchFile.CompareToFileInfo();
            PatchFile.SetListViewDifElement();
        }

        private void CreateFile_Click(object sender, EventArgs e)
        {
            PatchFile.CreateNewVersion();

            PatchFile.InitPatchFile();
            PatchFile.CompareToFileInfo();
            PatchFile.SetListViewDifElement();
        }

        private void Check_Version1ItemCheckBox(object sender, EventArgs e)
        {
            Check_VersionCheckBox(PatchFile.mListView_Version1);
        }

        private void Check_Version2ItemCheckBox(object sender, EventArgs e)
        {
            Check_VersionCheckBox(PatchFile.mListView_Version2);
        }

        private void Check_VersionDiffCheckBox(object sender, EventArgs e)
        {
            Check_VersionCheckBox(PatchFile.mListView_Dif);
        }

        private void Check_VersionCheckBox(ListView InListView)
        {
            foreach (ListViewItem checkItem in InListView.Items)
            {
                if (checkItem.Checked == true)
                {
                    checkItem.BackColor = Color.FromArgb(255, 196, 255, 155);
                }
                else
                {
                    checkItem.BackColor = Color.Empty;
                }
            }
        }

        private void ResultText_TextChanged(object sender, EventArgs e)
        {

        }

        private void Text_Version1_Click(object sender, EventArgs e)
        {

        }

        private void MainPanel_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Button_OpenDirecotyFile_Click(object sender, EventArgs e)
        {
            OpenFileDirectoryDialog();
        }

        private void Button_OpenDirecotyCopyFile_Click(object sender, EventArgs e)
        {
            OpenCopyFileDirectoryDialog();
        }

        private void OpenFileDirectoryDialog()
        {
            if (Dialog_SetRootDirectoryPath.ShowDialog() == DialogResult.OK)
            {
                string filePath = Dialog_SetRootDirectoryPath.SelectedPath;

                StreamWriter sw = new StreamWriter("./setting.cfg");
                sw.WriteLine(filePath);

                sw.WriteLine(PatchFile.mClientTablePath);

                sw.Flush();
                sw.Close();
                sw.Dispose();

                Button_OpenDirecotyFile.Text = string.Format("파일 경로 설정 : {0}", filePath);

                PatchFile.InitPatchFile();
                PatchFile.CompareToFileInfo();
                PatchFile.SetListViewDifElement();
            }
        }

        private void OpenCopyFileDirectoryDialog()
        {
            if (Dialog_SetCopyDirectoryPath.ShowDialog() == DialogResult.OK)
            {
                string filePath = Dialog_SetCopyDirectoryPath.SelectedPath;

                StreamWriter sw = new StreamWriter("./setting.cfg");
                sw.WriteLine(PatchFile.mDirectoryPath);

                sw.WriteLine(filePath);

                sw.Flush();
                sw.Close();
                sw.Dispose();

                Button_SetCopyTableDirectory.Text = string.Format("복사 파일 경로 설정 : {0}", filePath);
            }
        }

        private void ListView_Version1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void Dialog_SetRootDirectoryPath_HelpRequest(object sender, EventArgs e)
        {

        }

        private void DirectoryFileOpen_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("./setting.cfg");
            string path = sr.ReadLine();

            Process.Start(path);
        }

        private void CopyDirectoryFileOpen_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("./setting.cfg");
            sr.ReadLine();
            string path = sr.ReadLine();

            Process.Start(path);
        }
    }
}
