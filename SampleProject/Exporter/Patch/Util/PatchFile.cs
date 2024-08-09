using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace Patch
{
    public partial class PatchFile
    {
        static public bool IsVersionAscending = true;
        static public bool IsFileAscending = true;
        static public int PatchVersion = 0;

        static public void InitPatchFile()
        {
            InitPatchFileData();
            InitPatchFilePanel();
            //UpdateVersionCheck();
        }

        static private void InitPatchFileData()
        {
            ClearItemList();
            SetPatchFileDirecotyPath();
            SetVersion();
            InitItemList();
            SetNewPatchVersion();
        }

        static private void InitPatchFilePanel()
        {
            ClearListView();
            SetListViewColum();
            SetListViewElement();
            SetOutLineText();
            SetText_CreateFile();
        }

        static public void PatchVersionUpdate()
        {
            FilesVersionUpdate();
            InitPatchFilePanel();
            SetListViewDifElement();
            WirtePatchFileInfo();
        }
    }
}
