using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Patch
{
    public partial class PatchFile
    {
        static public string mDirectoryPath = "";
        static public string mClientTablePath = "";

        static private string mVersionValue_Version1 = "";
        static private string mVersionValue_Version2 = "";

        static private string mPath_Version1 = "";
        static private string mPath_Version2 = "";

        static private int mNewPatchVersion = 0;

        static private int mLastVersion = 0;

        static private List<FileItem> mList_Version1Item = new List<FileItem>();
        static private List<FileItem> mList_Version2Item = new List<FileItem>();
        static private List<FileItem> mList_DiffVersionItem = new List<FileItem>();


        static private void ClearItemList()
        {
            mList_Version1Item.Clear();
            mList_Version2Item.Clear();
            mList_DiffVersionItem.Clear();
        }

        static private void SetPatchFileDirecotyPath()
        {
            string path = "C:\\";

            if (File.Exists("./setting.cfg") == true)
            {
                StreamReader sr = new StreamReader("./setting.cfg");
                mDirectoryPath = sr.ReadLine();
                
                mClientTablePath = sr.ReadLine();

                sr.Close();
                sr.Dispose();
            }

            if (mDirectoryPath == null)
            {
                mDirectoryPath = path;
            }

            if (mClientTablePath == null)
            {
                mClientTablePath = path;
            }
            
            SetDirectoryPathText();
        }

        static private void SetVersion()
        {
            string[] versionFileNames = Directory.GetDirectories(mDirectoryPath);

            if (versionFileNames.Length < 2)
            {
                mVersionValue_Version1 = "";
                mVersionValue_Version2 = "";
                mPath_Version1 = "";
                mPath_Version2 = "";
                PatchVersion = 0;
                return;
            }

            List<string> versionList = new List<string>();

            for (int i = 0; i < versionFileNames.Length; i++)
            {
                string strVer = Path.GetFileName(versionFileNames[i]);

                if (strVer.Length != 8)
                {
                    continue;
                }

                //if (strVer.Substring(0, 2) != "22")
                //{
                    //continue;
                //}

                //int validNum = 0;

                //for (int j = 0; j < verCmd.Length; ++j)
                //{
                //    int verNum = 0;

                //    if (int.TryParse(verCmd[j], out verNum) == true && verNum <= 999)
                //    {
                //        validNum++;
                //    }
                //}

                versionList.Add(strVer);
            }

            if (versionList.Count > 0)
            {
                mLastVersion = int.Parse(versionList[versionList.Count - 1]);
            }

            if (versionList.Count < 2)
            {
                mVersionValue_Version1 = "";
                mVersionValue_Version2 = "";
                mPath_Version1 = "";
                mPath_Version2 = "";
                PatchVersion = 0;
                return;
            }

            int valVersion = 0;

            if (int.TryParse(versionList[versionList.Count - 1], out valVersion) == true && SetAndCheckUpdatePatchVersion(valVersion) == true)
            {
                mVersionValue_Version1 = versionList[versionList.Count - 2];
                mVersionValue_Version2 = versionList[versionList.Count - 1];

                mPath_Version1 = mDirectoryPath + "/" + mVersionValue_Version1;
                mPath_Version2 = mDirectoryPath + "/" + mVersionValue_Version2;
            }
        }

        static private bool SetAndCheckUpdatePatchVersion(int InVersion)
        {
            int todayVersion = int.Parse(string.Format("{0:D2}{1:D2}{2:D2}{3:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, "01").Remove(0, 2));

            if (InVersion >= todayVersion)
            {
                PatchVersion = InVersion;
                return true;
            }

            return false;
        }


        static private void InitItemList()
        {
            FindItemList(mPath_Version1, mList_Version1Item);
            FindItemList(mPath_Version2, mList_Version2Item);
        }

        static private void SetNewPatchVersion()
        {
            if (PatchVersion != 0)
            {
                mNewPatchVersion = PatchVersion + 1;
            }
            else
            {
                int todayVersion = int.Parse(string.Format("{0:D2}{1:D2}{2:D2}{3:D2}", DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, "01").Remove(0, 2));

                if (mLastVersion == todayVersion)
                {
                    todayVersion++;
                }

                mNewPatchVersion = todayVersion;
            }
        }
        static private void FindItemList(string InPath, List<FileItem> InList)
        {
            if (InPath == "")
            {
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(InPath);

            FileInfo[] files = directoryInfo.GetFiles("*.txt", SearchOption.TopDirectoryOnly);

            //files 정렬
            SortToFileArray(ref files);

            foreach (FileInfo file in files)
            {
                InList.Add(new FileItem(file, file.Length, file.Name));
            }
        }

        // 파일 정렬
        static private void SortToFileArray(ref FileInfo[] InFileArray)
        {
            if (IsFileAscending == true)
            {
                InFileArray = InFileArray.OrderBy(x => x.Name).ToArray();
            }
            else
            {
                InFileArray = InFileArray.OrderByDescending(x => x.Name).ToArray();
            }
        }

        public static void CompareToFileInfo()
        {
            mList_DiffVersionItem.Clear();

            if (mList_Version1Item.Count <= 0 || mList_Version2Item.Count <= 0)
            {
                return;
            }

            int loopCount_version1 = 0;
            int loopCount_version2 = 0;

            for (loopCount_version2 = 0; loopCount_version2 < mList_Version2Item.Count;)
            {
                if (loopCount_version1 >= mList_Version1Item.Count)
                {
                    loopCount_version1 = mList_Version1Item.Count - 1;
                }

                // 파일의 이름이 다른가?
                if (mList_Version1Item[loopCount_version1].mName.ToLower().CompareTo(mList_Version2Item[loopCount_version2].mName.ToLower()) != 0)
                {
                    // 이전 파일이 다음 버전에 없는가?
                    if (mList_Version2Item.Find(x => x.mName.ToLower().CompareTo(mList_Version1Item[loopCount_version1].mName.ToLower()) == 0) == null)
                    {
                        loopCount_version1++;
                    }

                    // 다음 파일이 이전 파일에 없는가?
                    if (mList_Version1Item.Find(x => x.mName.ToLower().CompareTo(mList_Version2Item[loopCount_version2].mName.ToLower()) == 0) == null)
                    {
                        mList_DiffVersionItem.Add(mList_Version2Item[loopCount_version2]);
                        loopCount_version2++;
                    }

                    continue;
                }

                // 파일의 이름이 같으니 파일 크기 검사
                if (mList_Version1Item[loopCount_version1].mByteSize.CompareTo(mList_Version2Item[loopCount_version2].mByteSize) != 0)
                {
                    //FileVersionNewUpdate(ref mList_Version2Item[loopCount_version2].mByte);
                    mList_DiffVersionItem.Add(mList_Version2Item[loopCount_version2]);
                    loopCount_version1++;
                    loopCount_version2++;

                    continue;
                }

                // 파일의 이름, 크기가 같으니 전체 바이트 값 검사
                if (CompareByteArray(mList_Version1Item[loopCount_version1].mByte, mList_Version2Item[loopCount_version2].mByte) == false)
                {
                    //FileVersionNewUpdate(ref mList_Version2Item[loopCount_version2].mByte);
                    mList_DiffVersionItem.Add(mList_Version2Item[loopCount_version2]);
                    loopCount_version1++;
                    loopCount_version2++;

                    continue;
                }

                //FileVersionUpdate(mList_Version1Item[loopCount_version1].mByte, ref mList_Version2Item[loopCount_version2].mByte);
                loopCount_version1++;
                loopCount_version2++;
            }
        }

        private static bool CompareByteArray(byte[] InByteArray1, byte[] InByteArray2)
        {
            if (InByteArray1 == null || InByteArray2 == null || InByteArray1.Length != InByteArray2.Length)
            {
                return false;
            }

            for (int i = 12; i < InByteArray1.Length; i++)
            {
                if (InByteArray1[i] != InByteArray2[i])
                {
                    return false;
                }
            }

            return true;
        }

        //private static void UpdateVersionCheck()
        //{
        //    foreach (FileItem item in mList_Version1Item)
        //    {
        //        if (item.mVersion >= PatchVersion)
        //        {
        //            PatchVersion = item.mVersion + 1;
        //        }
        //    }
        //}

        private static void FilesVersionUpdate()
        {
            int loopCount_version1 = 0;
            int loopCount_version2 = 0;

            for (loopCount_version2 = 0; loopCount_version2 < mList_Version2Item.Count;)
            {
                if (loopCount_version1 >= mList_Version1Item.Count)
                {
                    loopCount_version1 = mList_Version1Item.Count - 1;
                }

                // 파일의 이름이 다른가?
                if (mList_Version1Item[loopCount_version1].mName.CompareTo(mList_Version2Item[loopCount_version2].mName) != 0)
                {
                    // 이전 파일이 다음 버전에 없는가?
                    if (mList_Version2Item.Find(x => x.mName.ToLower().CompareTo(mList_Version1Item[loopCount_version1].mName.ToLower()) == 0) == null)
                    {
                        loopCount_version1++;
                    }

                    // 다음 파일이 이전 파일에 없는가?
                    if (mList_Version1Item.Find(x => x.mName.ToLower().CompareTo(mList_Version2Item[loopCount_version2].mName.ToLower()) == 0) == null)
                    {
                        FileVersionNewUpdate(mList_Version2Item[loopCount_version2]);
                        loopCount_version2++;
                    }

                    continue;
                }

                // 파일의 이름이 같으니 파일 크기 검사
                if (mList_Version1Item[loopCount_version1].mByteSize.CompareTo(mList_Version2Item[loopCount_version2].mByteSize) != 0)
                {
                    FileVersionNewUpdate(mList_Version2Item[loopCount_version2]);
                    loopCount_version1++;
                    loopCount_version2++;

                    continue;
                }

                // 파일의 이름, 크기가 같으니 전체 바이트 값 검사
                if (CompareByteArray(mList_Version1Item[loopCount_version1].mByte, mList_Version2Item[loopCount_version2].mByte) == false)
                {
                    FileVersionNewUpdate(mList_Version2Item[loopCount_version2]);
                    loopCount_version1++;
                    loopCount_version2++;

                    continue;
                }

                FileVersionUpdate(mList_Version1Item[loopCount_version1].mByte, mList_Version2Item[loopCount_version2]);
                loopCount_version1++;
                loopCount_version2++;
            }
        }

        private static void FileVersionUpdate(byte[] InByteArray1, FileItem InItem)
        {
            int File1Version = BitConverter.ToInt32(InByteArray1, 0);
            int File2Version = BitConverter.ToInt32(InItem.mByte, 0);

            if (File1Version != File2Version)
            {
                InItem.VersionUpdate(File1Version);
            }
        }

        private static void FileVersionNewUpdate(FileItem InItem)
        {
            InItem.VersionUpdate(PatchVersion);
        }

        private static void WirtePatchFileInfo()
        {
            string dirPath = string.Format("{0}/info", mPath_Version2);

            if (Directory.Exists(dirPath) == false)
            {
                Directory.CreateDirectory(dirPath);
            }

            StreamWriter sw = new StreamWriter(string.Format("{0}/data.txt", dirPath));

            foreach (FileItem file in mList_Version2Item)
            {
                string noext = Path.GetFileNameWithoutExtension(file.mName);

                string saveData = string.Format("{0}|{1}", noext, file.mVersion);

                sw.WriteLine(saveData);
            }

            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        public static void CreateNewVersion()
        {
            string dirPath = string.Format("{0}/{1}", mDirectoryPath, mNewPatchVersion);

            if (Directory.Exists(dirPath) == false)
            {
                Directory.CreateDirectory(dirPath);
            }

            NewFileSetting(dirPath);
        }

        private static void NewFileSetting(string InNewFilePath)
        {
            if (InNewFilePath == "")
            {
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(mClientTablePath);

            FileInfo[] files = directoryInfo.GetFiles("*.txt", SearchOption.TopDirectoryOnly);

            //files 정렬
            SortToFileArray(ref files);

            foreach (FileInfo file in files)
            {
                // 파일 생성
                File.Copy(file.FullName, string.Format("{0}/{1}", InNewFilePath, file.Name));
            }
        }
    }
}
