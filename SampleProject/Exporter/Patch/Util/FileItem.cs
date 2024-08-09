using System.IO;
using System;


namespace Patch
{
    public class FileItem
    {
        public FileInfo mFileInfo = null;
        public long mByteSize = 0;
        public string mName = null;
        public byte[] mByte = null;
        public int mVersion = 0;

        // Init FileItem Data
        public FileItem(FileInfo InFileInfo, long InByteSize, string InName)
        {
            mFileInfo = InFileInfo;
            mByteSize = InByteSize;
            mName = InName;

            if (InFileInfo != null)
            {
                mByte = File.ReadAllBytes(InFileInfo.FullName);
                VersionRead();
            }
        }

        public void VersionRead()
        {
            BinaryReader br = new BinaryReader(new FileStream(mFileInfo.FullName, FileMode.Open));
            mVersion = br.ReadInt32();
            br.Close();
            br.Dispose();
        }

        public void VersionUpdate(int InValue)
        {
            //Buffer.BlockCopy(mByte, 0, mByte, 0, 4);

            BinaryWriter bw = new BinaryWriter(new FileStream(mFileInfo.FullName, FileMode.Open));
            bw.Write(InValue);
            bw.Flush();
            bw.Close();
            bw.Dispose();

            VersionRead();
        }
    }
}
