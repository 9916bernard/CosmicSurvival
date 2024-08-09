using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace Patch
{
    public partial class PatchFile
    {
        static public ListView mListView_Version1 = null;
        static public ListView mListView_Version2 = null;
        static public ListView mListView_Dif = null;

        static public Label mText_Version1 = null;
        static public Label mText_Version2 = null;
        static public Label mText_Version1Path = null;
        static public Label mText_Version2Path = null;
        static public Label mText_Version1FileCount = null;
        static public Label mText_Version2FileCount = null;

        static public Button mButton_VersionUpdateButton = null;
        static public Button mButton_DirectorySettingButton = null;
        static public Button mButton_CopyDirectorySettingButton = null;
        static public Button mButton_CreateFile = null;

        static private void ClearListView()
        {
            mListView_Version1.Items.Clear();
            mListView_Version2.Items.Clear();
            mListView_Dif.Items.Clear();
        }

        static private void SetListViewColum()
        {
            mListView_Version1.Columns.Add("파일명", 400, HorizontalAlignment.Left);
            mListView_Version2.Columns.Add("파일명", 400, HorizontalAlignment.Left);
            mListView_Dif.Columns.Add("파일명", 400, HorizontalAlignment.Left);
        }

        static private void SetListViewElement()
        {
            FindElement(mListView_Version1, mList_Version1Item);
            FindElement(mListView_Version2, mList_Version2Item);
        }

        static public void SetListViewDifElement()
        {
            mListView_Dif.Items.Clear();
            FindElement(mListView_Dif, mList_DiffVersionItem);
        }

        static private void FindElement(ListView InListView, List<FileItem> InItemList)
        {
            foreach (FileItem item in InItemList)
            {
                string txt = string.Format("{0:D8} - {1}", item.mVersion, item.mName);

                InListView.Items.Add(txt);
            }
        }

        static private void SetDirectoryPathText()
        {
            mButton_DirectorySettingButton.Text = string.Format("파일 경로 설정 : {0}", mDirectoryPath);
            mButton_CopyDirectorySettingButton.Text = string.Format("복사 파일 경로 설정 : {0}", mClientTablePath);
        }

        static private void SetOutLineText()
        {
            mButton_VersionUpdateButton.Text = string.Format("업데이트 버전 : {0}", PatchVersion.ToString());
            SetTextVersion1();
            SetTextVersion2();
        }

        static private void SetTextVersion1()
        {
            mText_Version1.Text = mVersionValue_Version1;
            mText_Version1Path.Text = mPath_Version1;
            mText_Version1FileCount.Text = mList_Version1Item.Count.ToString();
        }

        static private void SetTextVersion2()
        {
            mText_Version2.Text = mVersionValue_Version2;
            mText_Version2Path.Text = mPath_Version2;
            mText_Version2FileCount.Text = mList_Version2Item.Count.ToString();
        }

        static private void SetText_CreateFile()
        {
            if (PatchVersion == 0)
            {
                mButton_CreateFile.Text = string.Format("오늘 날짜의 비교할 버전이 없습니다. 새 버전을 만드실레요? 버전 : {0}", mNewPatchVersion);
            }
            else
            {
                mButton_CreateFile.Text = string.Format("오늘 날짜의 비교할 버전이 있습니다. 그래도 새 버전을 만드실레요? 버전 : {0}", mNewPatchVersion);
            }
        }
    }
}
