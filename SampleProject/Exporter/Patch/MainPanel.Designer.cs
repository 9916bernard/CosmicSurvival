
namespace Patch
{
    partial class MainPanel
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPanel));
            this.VersionUpdate = new System.Windows.Forms.Button();
            this.ListView_Version1 = new System.Windows.Forms.ListView();
            this.ListView_Version2 = new System.Windows.Forms.ListView();
            this.Title_Version1 = new System.Windows.Forms.Label();
            this.Title_Version2 = new System.Windows.Forms.Label();
            this.Title_Version1Path = new System.Windows.Forms.Label();
            this.Title_Version2Path = new System.Windows.Forms.Label();
            this.Text_Version1Path = new System.Windows.Forms.Label();
            this.Text_Version2Path = new System.Windows.Forms.Label();
            this.Title_Version1FileCount = new System.Windows.Forms.Label();
            this.Text_Version1FileCount = new System.Windows.Forms.Label();
            this.Title_Version2FileCount = new System.Windows.Forms.Label();
            this.Text_Version2FileCount = new System.Windows.Forms.Label();
            this.ListView_VersionDif = new System.Windows.Forms.ListView();
            this.Text_Version1 = new System.Windows.Forms.Label();
            this.Text_Version2 = new System.Windows.Forms.Label();
            this.Button_FileOrderBy = new System.Windows.Forms.Button();
            this.Button_OpenDirecotyFile = new System.Windows.Forms.Button();
            this.Dialog_SetRootDirectoryPath = new System.Windows.Forms.FolderBrowserDialog();
            this.ButtonCreateFile = new System.Windows.Forms.Button();
            this.Button_SetCopyTableDirectory = new System.Windows.Forms.Button();
            this.Dialog_SetCopyDirectoryPath = new System.Windows.Forms.FolderBrowserDialog();
            this.DirectoryFileOpen = new System.Windows.Forms.Button();
            this.CopyDirectoryFileOpen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // VersionUpdate
            // 
            this.VersionUpdate.Location = new System.Drawing.Point(16, 499);
            this.VersionUpdate.Name = "VersionUpdate";
            this.VersionUpdate.Size = new System.Drawing.Size(419, 74);
            this.VersionUpdate.TabIndex = 1;
            this.VersionUpdate.Text = "우측 버전으로 업데이트 하기 : versionValue";
            this.VersionUpdate.UseVisualStyleBackColor = true;
            this.VersionUpdate.Click += new System.EventHandler(this.VersionUpdate_Click);
            // 
            // ListView_Version1
            // 
            this.ListView_Version1.CheckBoxes = true;
            this.ListView_Version1.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ListView_Version1.FullRowSelect = true;
            this.ListView_Version1.GridLines = true;
            this.ListView_Version1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ListView_Version1.HideSelection = false;
            this.ListView_Version1.Location = new System.Drawing.Point(15, 50);
            this.ListView_Version1.Name = "ListView_Version1";
            this.ListView_Version1.Size = new System.Drawing.Size(419, 403);
            this.ListView_Version1.TabIndex = 6;
            this.ListView_Version1.UseCompatibleStateImageBehavior = false;
            this.ListView_Version1.View = System.Windows.Forms.View.Details;
            this.ListView_Version1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.Check_Version1ItemCheckBox);
            this.ListView_Version1.SelectedIndexChanged += new System.EventHandler(this.ListView_Version1_SelectedIndexChanged);
            // 
            // ListView_Version2
            // 
            this.ListView_Version2.CheckBoxes = true;
            this.ListView_Version2.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ListView_Version2.FullRowSelect = true;
            this.ListView_Version2.GridLines = true;
            this.ListView_Version2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ListView_Version2.HideSelection = false;
            this.ListView_Version2.Location = new System.Drawing.Point(449, 50);
            this.ListView_Version2.Name = "ListView_Version2";
            this.ListView_Version2.Size = new System.Drawing.Size(419, 403);
            this.ListView_Version2.TabIndex = 7;
            this.ListView_Version2.UseCompatibleStateImageBehavior = false;
            this.ListView_Version2.View = System.Windows.Forms.View.Details;
            this.ListView_Version2.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.Check_Version2ItemCheckBox);
            // 
            // Title_Version1
            // 
            this.Title_Version1.AutoSize = true;
            this.Title_Version1.Font = new System.Drawing.Font("굴림", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Title_Version1.Location = new System.Drawing.Point(13, 9);
            this.Title_Version1.Name = "Title_Version1";
            this.Title_Version1.Size = new System.Drawing.Size(88, 17);
            this.Title_Version1.TabIndex = 8;
            this.Title_Version1.Text = "Version : ";
            this.Title_Version1.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Title_Version2
            // 
            this.Title_Version2.AutoSize = true;
            this.Title_Version2.Font = new System.Drawing.Font("굴림", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Title_Version2.Location = new System.Drawing.Point(446, 9);
            this.Title_Version2.Name = "Title_Version2";
            this.Title_Version2.Size = new System.Drawing.Size(88, 17);
            this.Title_Version2.TabIndex = 8;
            this.Title_Version2.Text = "Version : ";
            this.Title_Version2.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Title_Version1Path
            // 
            this.Title_Version1Path.AutoSize = true;
            this.Title_Version1Path.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Title_Version1Path.Location = new System.Drawing.Point(13, 29);
            this.Title_Version1Path.Name = "Title_Version1Path";
            this.Title_Version1Path.Size = new System.Drawing.Size(41, 13);
            this.Title_Version1Path.TabIndex = 8;
            this.Title_Version1Path.Text = "경로 :";
            this.Title_Version1Path.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Title_Version2Path
            // 
            this.Title_Version2Path.AutoSize = true;
            this.Title_Version2Path.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Title_Version2Path.Location = new System.Drawing.Point(447, 29);
            this.Title_Version2Path.Name = "Title_Version2Path";
            this.Title_Version2Path.Size = new System.Drawing.Size(41, 13);
            this.Title_Version2Path.TabIndex = 8;
            this.Title_Version2Path.Text = "경로 :";
            this.Title_Version2Path.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Text_Version1Path
            // 
            this.Text_Version1Path.AutoSize = true;
            this.Text_Version1Path.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Text_Version1Path.Location = new System.Drawing.Point(54, 29);
            this.Text_Version1Path.Name = "Text_Version1Path";
            this.Text_Version1Path.Size = new System.Drawing.Size(63, 13);
            this.Text_Version1Path.TabIndex = 8;
            this.Text_Version1Path.Text = "Directory";
            this.Text_Version1Path.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Text_Version2Path
            // 
            this.Text_Version2Path.AutoSize = true;
            this.Text_Version2Path.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Text_Version2Path.Location = new System.Drawing.Point(488, 29);
            this.Text_Version2Path.Name = "Text_Version2Path";
            this.Text_Version2Path.Size = new System.Drawing.Size(63, 13);
            this.Text_Version2Path.TabIndex = 8;
            this.Text_Version2Path.Text = "Directory";
            this.Text_Version2Path.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Title_Version1FileCount
            // 
            this.Title_Version1FileCount.AutoSize = true;
            this.Title_Version1FileCount.Font = new System.Drawing.Font("굴림", 10F);
            this.Title_Version1FileCount.Location = new System.Drawing.Point(321, 460);
            this.Title_Version1FileCount.Name = "Title_Version1FileCount";
            this.Title_Version1FileCount.Size = new System.Drawing.Size(83, 14);
            this.Title_Version1FileCount.TabIndex = 8;
            this.Title_Version1FileCount.Text = "File Count :";
            this.Title_Version1FileCount.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Text_Version1FileCount
            // 
            this.Text_Version1FileCount.AutoSize = true;
            this.Text_Version1FileCount.Font = new System.Drawing.Font("굴림", 10F);
            this.Text_Version1FileCount.Location = new System.Drawing.Point(410, 460);
            this.Text_Version1FileCount.Name = "Text_Version1FileCount";
            this.Text_Version1FileCount.Size = new System.Drawing.Size(15, 14);
            this.Text_Version1FileCount.TabIndex = 8;
            this.Text_Version1FileCount.Text = "0";
            this.Text_Version1FileCount.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Title_Version2FileCount
            // 
            this.Title_Version2FileCount.AutoSize = true;
            this.Title_Version2FileCount.Font = new System.Drawing.Font("굴림", 10F);
            this.Title_Version2FileCount.Location = new System.Drawing.Point(754, 460);
            this.Title_Version2FileCount.Name = "Title_Version2FileCount";
            this.Title_Version2FileCount.Size = new System.Drawing.Size(83, 14);
            this.Title_Version2FileCount.TabIndex = 8;
            this.Title_Version2FileCount.Text = "File Count :";
            this.Title_Version2FileCount.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Text_Version2FileCount
            // 
            this.Text_Version2FileCount.AutoSize = true;
            this.Text_Version2FileCount.Font = new System.Drawing.Font("굴림", 10F);
            this.Text_Version2FileCount.Location = new System.Drawing.Point(843, 460);
            this.Text_Version2FileCount.Name = "Text_Version2FileCount";
            this.Text_Version2FileCount.Size = new System.Drawing.Size(15, 14);
            this.Text_Version2FileCount.TabIndex = 8;
            this.Text_Version2FileCount.Text = "0";
            this.Text_Version2FileCount.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // ListView_VersionDif
            // 
            this.ListView_VersionDif.CheckBoxes = true;
            this.ListView_VersionDif.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ListView_VersionDif.FullRowSelect = true;
            this.ListView_VersionDif.GridLines = true;
            this.ListView_VersionDif.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ListView_VersionDif.HideSelection = false;
            this.ListView_VersionDif.Location = new System.Drawing.Point(886, 50);
            this.ListView_VersionDif.Name = "ListView_VersionDif";
            this.ListView_VersionDif.Size = new System.Drawing.Size(419, 403);
            this.ListView_VersionDif.TabIndex = 7;
            this.ListView_VersionDif.UseCompatibleStateImageBehavior = false;
            this.ListView_VersionDif.View = System.Windows.Forms.View.Details;
            this.ListView_VersionDif.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.Check_VersionDiffCheckBox);
            // 
            // Text_Version1
            // 
            this.Text_Version1.AutoSize = true;
            this.Text_Version1.Font = new System.Drawing.Font("굴림", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Text_Version1.Location = new System.Drawing.Point(94, 9);
            this.Text_Version1.Name = "Text_Version1";
            this.Text_Version1.Size = new System.Drawing.Size(47, 17);
            this.Text_Version1.TabIndex = 8;
            this.Text_Version1.Text = "0.0.0";
            this.Text_Version1.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Text_Version2
            // 
            this.Text_Version2.AutoSize = true;
            this.Text_Version2.Font = new System.Drawing.Font("굴림", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Text_Version2.Location = new System.Drawing.Point(530, 9);
            this.Text_Version2.Name = "Text_Version2";
            this.Text_Version2.Size = new System.Drawing.Size(47, 17);
            this.Text_Version2.TabIndex = 8;
            this.Text_Version2.Text = "0.0.0";
            this.Text_Version2.Click += new System.EventHandler(this.Text_Version1_Click);
            // 
            // Button_FileOrderBy
            // 
            this.Button_FileOrderBy.Location = new System.Drawing.Point(450, 499);
            this.Button_FileOrderBy.Name = "Button_FileOrderBy";
            this.Button_FileOrderBy.Size = new System.Drawing.Size(156, 74);
            this.Button_FileOrderBy.TabIndex = 9;
            this.Button_FileOrderBy.Text = "파일 정렬 : 오름차순";
            this.Button_FileOrderBy.UseVisualStyleBackColor = true;
            this.Button_FileOrderBy.Click += new System.EventHandler(this.FileOrderby_Click);
            // 
            // Button_OpenDirecotyFile
            // 
            this.Button_OpenDirecotyFile.Location = new System.Drawing.Point(886, 499);
            this.Button_OpenDirecotyFile.Name = "Button_OpenDirecotyFile";
            this.Button_OpenDirecotyFile.Size = new System.Drawing.Size(338, 32);
            this.Button_OpenDirecotyFile.TabIndex = 10;
            this.Button_OpenDirecotyFile.Text = "Set Root File Directory";
            this.Button_OpenDirecotyFile.UseVisualStyleBackColor = true;
            this.Button_OpenDirecotyFile.Click += new System.EventHandler(this.Button_OpenDirecotyFile_Click);
            // 
            // Dialog_SetRootDirectoryPath
            // 
            this.Dialog_SetRootDirectoryPath.Description = "Select Root Direcoty Path";
            this.Dialog_SetRootDirectoryPath.SelectedPath = "C:\\SVN";
            this.Dialog_SetRootDirectoryPath.ShowNewFolderButton = false;
            this.Dialog_SetRootDirectoryPath.HelpRequest += new System.EventHandler(this.Dialog_SetRootDirectoryPath_HelpRequest);
            // 
            // ButtonCreateFile
            // 
            this.ButtonCreateFile.Location = new System.Drawing.Point(627, 499);
            this.ButtonCreateFile.Name = "ButtonCreateFile";
            this.ButtonCreateFile.Size = new System.Drawing.Size(241, 74);
            this.ButtonCreateFile.TabIndex = 9;
            this.ButtonCreateFile.Text = "오늘 날짜의 비교할 버전이 없습니다. 새 버전을 만드실레요?";
            this.ButtonCreateFile.UseVisualStyleBackColor = true;
            this.ButtonCreateFile.Click += new System.EventHandler(this.CreateFile_Click);
            // 
            // Button_SetCopyTableDirectory
            // 
            this.Button_SetCopyTableDirectory.Location = new System.Drawing.Point(886, 537);
            this.Button_SetCopyTableDirectory.Name = "Button_SetCopyTableDirectory";
            this.Button_SetCopyTableDirectory.Size = new System.Drawing.Size(338, 32);
            this.Button_SetCopyTableDirectory.TabIndex = 10;
            this.Button_SetCopyTableDirectory.Text = "Set Copy Directory";
            this.Button_SetCopyTableDirectory.UseVisualStyleBackColor = true;
            this.Button_SetCopyTableDirectory.Click += new System.EventHandler(this.Button_OpenDirecotyCopyFile_Click);
            // 
            // Dialog_SetCopyDirectoryPath
            // 
            this.Dialog_SetCopyDirectoryPath.Description = "Dialog_SetCopyDirectoryPath";
            this.Dialog_SetCopyDirectoryPath.SelectedPath = "C:\\SVN";
            this.Dialog_SetCopyDirectoryPath.ShowNewFolderButton = false;
            this.Dialog_SetCopyDirectoryPath.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // DirectoryFileOpen
            // 
            this.DirectoryFileOpen.Location = new System.Drawing.Point(1230, 499);
            this.DirectoryFileOpen.Name = "DirectoryFileOpen";
            this.DirectoryFileOpen.Size = new System.Drawing.Size(73, 29);
            this.DirectoryFileOpen.TabIndex = 11;
            this.DirectoryFileOpen.Text = "▶";
            this.DirectoryFileOpen.UseVisualStyleBackColor = true;
            this.DirectoryFileOpen.Click += new System.EventHandler(this.DirectoryFileOpen_Click);
            // 
            // CopyDirectoryFileOpen
            // 
            this.CopyDirectoryFileOpen.Location = new System.Drawing.Point(1230, 537);
            this.CopyDirectoryFileOpen.Name = "CopyDirectoryFileOpen";
            this.CopyDirectoryFileOpen.Size = new System.Drawing.Size(73, 32);
            this.CopyDirectoryFileOpen.TabIndex = 12;
            this.CopyDirectoryFileOpen.Text = "▶";
            this.CopyDirectoryFileOpen.UseVisualStyleBackColor = true;
            this.CopyDirectoryFileOpen.Click += new System.EventHandler(this.CopyDirectoryFileOpen_Click);
            // 
            // MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1322, 592);
            this.Controls.Add(this.CopyDirectoryFileOpen);
            this.Controls.Add(this.DirectoryFileOpen);
            this.Controls.Add(this.Button_SetCopyTableDirectory);
            this.Controls.Add(this.Button_OpenDirecotyFile);
            this.Controls.Add(this.ButtonCreateFile);
            this.Controls.Add(this.Button_FileOrderBy);
            this.Controls.Add(this.Title_Version2);
            this.Controls.Add(this.Text_Version2Path);
            this.Controls.Add(this.Title_Version2Path);
            this.Controls.Add(this.Text_Version2FileCount);
            this.Controls.Add(this.Title_Version2FileCount);
            this.Controls.Add(this.Text_Version1FileCount);
            this.Controls.Add(this.Title_Version1FileCount);
            this.Controls.Add(this.Text_Version1Path);
            this.Controls.Add(this.Title_Version1Path);
            this.Controls.Add(this.Text_Version2);
            this.Controls.Add(this.Text_Version1);
            this.Controls.Add(this.Title_Version1);
            this.Controls.Add(this.ListView_VersionDif);
            this.Controls.Add(this.ListView_Version2);
            this.Controls.Add(this.ListView_Version1);
            this.Controls.Add(this.VersionUpdate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainPanel";
            this.Text = "Patch";
            this.Load += new System.EventHandler(this.MainPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        // -----------------------------------------
        // Button
        // -----------------------------------------
        private System.Windows.Forms.Button VersionUpdate;


        // -----------------------------------------
        // ListView
        // -----------------------------------------
        private System.Windows.Forms.ListView ListView_Version1;
        private System.Windows.Forms.ListView ListView_Version2;
        private System.Windows.Forms.ListView ListView_VersionDif;

        // -----------------------------------------
        // Label : Title Text
        // -----------------------------------------
        private System.Windows.Forms.Label Title_Version1;
        private System.Windows.Forms.Label Title_Version2;
        private System.Windows.Forms.Label Title_Version1Path;
        private System.Windows.Forms.Label Title_Version2Path;
        private System.Windows.Forms.Label Title_Version1FileCount;
        private System.Windows.Forms.Label Title_Version2FileCount;


        // -----------------------------------------
        // Label : Value Text
        // -----------------------------------------
        private System.Windows.Forms.Label Text_Version1;
        private System.Windows.Forms.Label Text_Version2;
        private System.Windows.Forms.Label Text_Version1Path;
        private System.Windows.Forms.Label Text_Version2Path;
        private System.Windows.Forms.Label Text_Version1FileCount;
        private System.Windows.Forms.Label Text_Version2FileCount;
        private System.Windows.Forms.Button Button_FileOrderBy;
        private System.Windows.Forms.Button Button_OpenDirecotyFile;
        private System.Windows.Forms.FolderBrowserDialog Dialog_SetRootDirectoryPath;
        private System.Windows.Forms.Button ButtonCreateFile;
        private System.Windows.Forms.Button Button_SetCopyTableDirectory;
        private System.Windows.Forms.Button DirectoryFileOpen;
        private System.Windows.Forms.Button CopyDirectoryFileOpen;
        private System.Windows.Forms.FolderBrowserDialog Dialog_SetCopyDirectoryPath;
    }
}

