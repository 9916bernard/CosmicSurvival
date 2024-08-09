namespace Table
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
            this.Btn_ExportAll = new System.Windows.Forms.Button();
            this.Btn_ExportData = new System.Windows.Forms.Button();
            this.Btn_OpenLog = new System.Windows.Forms.Button();
            this.Btn_OpenSetting = new System.Windows.Forms.Button();
            this.Btn_SettingReload = new System.Windows.Forms.Button();
            this.ListView_Setting = new System.Windows.Forms.ListView();
            this.Text_Status = new System.Windows.Forms.Label();
            this.ListView_Table = new System.Windows.Forms.ListView();
            this.Btn_SelectedExportData = new System.Windows.Forms.Button();
            this.Btn_SelectedExportAll = new System.Windows.Forms.Button();
            this.Btn_PatchList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_ExportAll
            // 
            this.Btn_ExportAll.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_ExportAll.Location = new System.Drawing.Point(599, 528);
            this.Btn_ExportAll.Name = "Btn_ExportAll";
            this.Btn_ExportAll.Size = new System.Drawing.Size(240, 30);
            this.Btn_ExportAll.TabIndex = 0;
            this.Btn_ExportAll.Text = "코드 + 데이터 익스포트";
            this.Btn_ExportAll.UseVisualStyleBackColor = true;
            this.Btn_ExportAll.Click += new System.EventHandler(this.Btn_ExportAll_Click);
            // 
            // Btn_ExportData
            // 
            this.Btn_ExportData.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_ExportData.Location = new System.Drawing.Point(599, 604);
            this.Btn_ExportData.Name = "Btn_ExportData";
            this.Btn_ExportData.Size = new System.Drawing.Size(240, 70);
            this.Btn_ExportData.TabIndex = 1;
            this.Btn_ExportData.Text = "데이터 익스포트";
            this.Btn_ExportData.UseVisualStyleBackColor = true;
            this.Btn_ExportData.Click += new System.EventHandler(this.Btn_ExportData_Click);
            // 
            // Btn_OpenLog
            // 
            this.Btn_OpenLog.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_OpenLog.Location = new System.Drawing.Point(12, 603);
            this.Btn_OpenLog.Name = "Btn_OpenLog";
            this.Btn_OpenLog.Size = new System.Drawing.Size(240, 70);
            this.Btn_OpenLog.TabIndex = 3;
            this.Btn_OpenLog.Text = "로그파일 열기";
            this.Btn_OpenLog.UseVisualStyleBackColor = true;
            this.Btn_OpenLog.Click += new System.EventHandler(this.Btn_OpenLog_Click);
            // 
            // Btn_OpenSetting
            // 
            this.Btn_OpenSetting.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_OpenSetting.Location = new System.Drawing.Point(12, 527);
            this.Btn_OpenSetting.Name = "Btn_OpenSetting";
            this.Btn_OpenSetting.Size = new System.Drawing.Size(240, 70);
            this.Btn_OpenSetting.TabIndex = 2;
            this.Btn_OpenSetting.Text = "설정파일 열기";
            this.Btn_OpenSetting.UseVisualStyleBackColor = true;
            this.Btn_OpenSetting.Click += new System.EventHandler(this.Btn_OpenSetting_Click);
            // 
            // Btn_SettingReload
            // 
            this.Btn_SettingReload.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_SettingReload.Location = new System.Drawing.Point(258, 528);
            this.Btn_SettingReload.Name = "Btn_SettingReload";
            this.Btn_SettingReload.Size = new System.Drawing.Size(335, 70);
            this.Btn_SettingReload.TabIndex = 4;
            this.Btn_SettingReload.Text = "설정파일 다시 읽기";
            this.Btn_SettingReload.UseVisualStyleBackColor = true;
            this.Btn_SettingReload.Click += new System.EventHandler(this.Btn_SettingReload_Click);
            // 
            // ListView_Setting
            // 
            this.ListView_Setting.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ListView_Setting.FullRowSelect = true;
            this.ListView_Setting.GridLines = true;
            this.ListView_Setting.HideSelection = false;
            this.ListView_Setting.Location = new System.Drawing.Point(12, 12);
            this.ListView_Setting.MultiSelect = false;
            this.ListView_Setting.Name = "ListView_Setting";
            this.ListView_Setting.Size = new System.Drawing.Size(827, 501);
            this.ListView_Setting.TabIndex = 5;
            this.ListView_Setting.UseCompatibleStateImageBehavior = false;
            this.ListView_Setting.View = System.Windows.Forms.View.List;
            this.ListView_Setting.DoubleClick += new System.EventHandler(this.ListView_Setting_DoubleClick);
            // 
            // Text_Status
            // 
            this.Text_Status.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_Status.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Text_Status.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.Text_Status.Location = new System.Drawing.Point(258, 604);
            this.Text_Status.Name = "Text_Status";
            this.Text_Status.Size = new System.Drawing.Size(335, 69);
            this.Text_Status.TabIndex = 6;
            this.Text_Status.Text = "READY";
            this.Text_Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ListView_Table
            // 
            this.ListView_Table.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ListView_Table.FullRowSelect = true;
            this.ListView_Table.GridLines = true;
            this.ListView_Table.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListView_Table.HideSelection = false;
            this.ListView_Table.Location = new System.Drawing.Point(868, 12);
            this.ListView_Table.Name = "ListView_Table";
            this.ListView_Table.Size = new System.Drawing.Size(752, 501);
            this.ListView_Table.TabIndex = 7;
            this.ListView_Table.UseCompatibleStateImageBehavior = false;
            this.ListView_Table.View = System.Windows.Forms.View.List;
            // 
            // Btn_SelectedExportData
            // 
            this.Btn_SelectedExportData.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_SelectedExportData.Location = new System.Drawing.Point(1380, 601);
            this.Btn_SelectedExportData.Name = "Btn_SelectedExportData";
            this.Btn_SelectedExportData.Size = new System.Drawing.Size(240, 70);
            this.Btn_SelectedExportData.TabIndex = 9;
            this.Btn_SelectedExportData.Text = "(선택) 데이터 익스포트";
            this.Btn_SelectedExportData.UseVisualStyleBackColor = true;
            this.Btn_SelectedExportData.Click += new System.EventHandler(this.Btn_SelectedExportData_Click);
            // 
            // Btn_SelectedExportAll
            // 
            this.Btn_SelectedExportAll.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Btn_SelectedExportAll.Location = new System.Drawing.Point(1380, 528);
            this.Btn_SelectedExportAll.Name = "Btn_SelectedExportAll";
            this.Btn_SelectedExportAll.Size = new System.Drawing.Size(240, 30);
            this.Btn_SelectedExportAll.TabIndex = 8;
            this.Btn_SelectedExportAll.Text = "(선택) 코드 + 데이터 익스포트";
            this.Btn_SelectedExportAll.UseVisualStyleBackColor = true;
            this.Btn_SelectedExportAll.Click += new System.EventHandler(this.Btn_SelectedExportAll_Click);
            // 
            // Btn_PatchList
            // 
            this.Btn_PatchList.Location = new System.Drawing.Point(868, 528);
            this.Btn_PatchList.Name = "Btn_PatchList";
            this.Btn_PatchList.Size = new System.Drawing.Size(144, 46);
            this.Btn_PatchList.TabIndex = 10;
            this.Btn_PatchList.Text = "패치 리스트 뽑기";
            this.Btn_PatchList.UseVisualStyleBackColor = true;
            this.Btn_PatchList.Click += new System.EventHandler(this.Btn_PatchList_Click);
            // 
            // MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1632, 683);
            this.Controls.Add(this.Btn_PatchList);
            this.Controls.Add(this.Btn_SelectedExportData);
            this.Controls.Add(this.Btn_SelectedExportAll);
            this.Controls.Add(this.ListView_Table);
            this.Controls.Add(this.Text_Status);
            this.Controls.Add(this.ListView_Setting);
            this.Controls.Add(this.Btn_SettingReload);
            this.Controls.Add(this.Btn_OpenLog);
            this.Controls.Add(this.Btn_OpenSetting);
            this.Controls.Add(this.Btn_ExportData);
            this.Controls.Add(this.Btn_ExportAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainPanel";
            this.Text = "Table Exporter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_ExportAll;
        private System.Windows.Forms.Button Btn_ExportData;
        private System.Windows.Forms.Button Btn_OpenLog;
        private System.Windows.Forms.Button Btn_OpenSetting;
        private System.Windows.Forms.Button Btn_SettingReload;
        private System.Windows.Forms.ListView ListView_Setting;
        private System.Windows.Forms.Label Text_Status;
        private System.Windows.Forms.ListView ListView_Table;
        private System.Windows.Forms.Button Btn_SelectedExportData;
        private System.Windows.Forms.Button Btn_SelectedExportAll;
        private System.Windows.Forms.Button Btn_PatchList;
    }
}

