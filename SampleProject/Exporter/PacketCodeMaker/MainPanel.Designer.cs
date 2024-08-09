namespace PacketCodeMaker
{
    partial class MainPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPanel));
            this.Button_MakePacketCode = new System.Windows.Forms.Button();
            this.ClientPathValue = new System.Windows.Forms.TextBox();
            this.ClientPathText = new System.Windows.Forms.Label();
            this.ServerPathText = new System.Windows.Forms.Label();
            this.ServerPathValue = new System.Windows.Forms.TextBox();
            this.PacketExcelPathText = new System.Windows.Forms.Label();
            this.PacketExcelPathValue = new System.Windows.Forms.TextBox();
            this.Status = new System.Windows.Forms.Label();
            this.Button_SetClient = new System.Windows.Forms.Button();
            this.Button_GoClient = new System.Windows.Forms.Button();
            this.Button_GoServer = new System.Windows.Forms.Button();
            this.Button_SetServer = new System.Windows.Forms.Button();
            this.Button_GoExcel = new System.Windows.Forms.Button();
            this.Button_SetExcel = new System.Windows.Forms.Button();
            this.Button_GoBot = new System.Windows.Forms.Button();
            this.Button_SetBot = new System.Windows.Forms.Button();
            this.BotPathText = new System.Windows.Forms.Label();
            this.BotPathValue = new System.Windows.Forms.TextBox();
            this.Button_GoEnum = new System.Windows.Forms.Button();
            this.Button_SetEnum = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.EnumExcelPathValue = new System.Windows.Forms.TextBox();
            this.Button_GoServerPython = new System.Windows.Forms.Button();
            this.Button_SetServerPython = new System.Windows.Forms.Button();
            this.ServerPythonPathText = new System.Windows.Forms.Label();
            this.ServerPythonPathValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Button_MakePacketCode
            // 
            this.Button_MakePacketCode.Location = new System.Drawing.Point(12, 396);
            this.Button_MakePacketCode.Name = "Button_MakePacketCode";
            this.Button_MakePacketCode.Size = new System.Drawing.Size(709, 64);
            this.Button_MakePacketCode.TabIndex = 0;
            this.Button_MakePacketCode.Text = "Make Packet Code";
            this.Button_MakePacketCode.UseVisualStyleBackColor = true;
            this.Button_MakePacketCode.Click += new System.EventHandler(this.Button_MakePacketCode_Click);
            // 
            // ClientPathValue
            // 
            this.ClientPathValue.Location = new System.Drawing.Point(12, 32);
            this.ClientPathValue.Name = "ClientPathValue";
            this.ClientPathValue.Size = new System.Drawing.Size(590, 21);
            this.ClientPathValue.TabIndex = 1;
            this.ClientPathValue.Text = "E:\\SVN\\ClosersMobile\\Program\\Server\\SampleClient\\NetSample\\Source\\NetSample\\Net";
            // 
            // ClientPathText
            // 
            this.ClientPathText.AutoSize = true;
            this.ClientPathText.Location = new System.Drawing.Point(12, 14);
            this.ClientPathText.Name = "ClientPathText";
            this.ClientPathText.Size = new System.Drawing.Size(74, 12);
            this.ClientPathText.TabIndex = 2;
            this.ClientPathText.Text = "ClientPath : ";
            // 
            // ServerPathText
            // 
            this.ServerPathText.AutoSize = true;
            this.ServerPathText.Location = new System.Drawing.Point(12, 71);
            this.ServerPathText.Name = "ServerPathText";
            this.ServerPathText.Size = new System.Drawing.Size(78, 12);
            this.ServerPathText.TabIndex = 4;
            this.ServerPathText.Text = "ServerPath : ";
            // 
            // ServerPathValue
            // 
            this.ServerPathValue.Location = new System.Drawing.Point(12, 89);
            this.ServerPathValue.Name = "ServerPathValue";
            this.ServerPathValue.Size = new System.Drawing.Size(590, 21);
            this.ServerPathValue.TabIndex = 3;
            this.ServerPathValue.Text = "E:\\SVN\\ClosersMobile\\Program\\Server\\Trunk\\PacketCommon";
            // 
            // PacketExcelPathText
            // 
            this.PacketExcelPathText.AutoSize = true;
            this.PacketExcelPathText.Location = new System.Drawing.Point(12, 331);
            this.PacketExcelPathText.Name = "PacketExcelPathText";
            this.PacketExcelPathText.Size = new System.Drawing.Size(112, 12);
            this.PacketExcelPathText.TabIndex = 6;
            this.PacketExcelPathText.Text = "PacketExcelPath : ";
            // 
            // PacketExcelPathValue
            // 
            this.PacketExcelPathValue.Location = new System.Drawing.Point(12, 349);
            this.PacketExcelPathValue.Name = "PacketExcelPathValue";
            this.PacketExcelPathValue.Size = new System.Drawing.Size(590, 21);
            this.PacketExcelPathValue.TabIndex = 5;
            this.PacketExcelPathValue.Text = "E:\\SVN\\ClosersMobile\\Program\\Deploy\\Packet";
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(13, 479);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(41, 12);
            this.Status.TabIndex = 7;
            this.Status.Text = "Ready";
            // 
            // Button_SetClient
            // 
            this.Button_SetClient.Location = new System.Drawing.Point(607, 16);
            this.Button_SetClient.Name = "Button_SetClient";
            this.Button_SetClient.Size = new System.Drawing.Size(55, 39);
            this.Button_SetClient.TabIndex = 8;
            this.Button_SetClient.Text = "...";
            this.Button_SetClient.UseVisualStyleBackColor = true;
            this.Button_SetClient.Click += new System.EventHandler(this.Button_SetClient_Click);
            // 
            // Button_GoClient
            // 
            this.Button_GoClient.Location = new System.Drawing.Point(666, 16);
            this.Button_GoClient.Name = "Button_GoClient";
            this.Button_GoClient.Size = new System.Drawing.Size(55, 39);
            this.Button_GoClient.TabIndex = 9;
            this.Button_GoClient.Text = "▶";
            this.Button_GoClient.UseVisualStyleBackColor = true;
            this.Button_GoClient.Click += new System.EventHandler(this.Button_GoClient_Click);
            // 
            // Button_GoServer
            // 
            this.Button_GoServer.Location = new System.Drawing.Point(666, 73);
            this.Button_GoServer.Name = "Button_GoServer";
            this.Button_GoServer.Size = new System.Drawing.Size(55, 39);
            this.Button_GoServer.TabIndex = 11;
            this.Button_GoServer.Text = "▶";
            this.Button_GoServer.UseVisualStyleBackColor = true;
            this.Button_GoServer.Click += new System.EventHandler(this.Button_GoServer_Click);
            // 
            // Button_SetServer
            // 
            this.Button_SetServer.Location = new System.Drawing.Point(607, 73);
            this.Button_SetServer.Name = "Button_SetServer";
            this.Button_SetServer.Size = new System.Drawing.Size(55, 39);
            this.Button_SetServer.TabIndex = 10;
            this.Button_SetServer.Text = "...";
            this.Button_SetServer.UseVisualStyleBackColor = true;
            this.Button_SetServer.Click += new System.EventHandler(this.Button_SetServer_Click);
            // 
            // Button_GoExcel
            // 
            this.Button_GoExcel.Location = new System.Drawing.Point(666, 333);
            this.Button_GoExcel.Name = "Button_GoExcel";
            this.Button_GoExcel.Size = new System.Drawing.Size(55, 39);
            this.Button_GoExcel.TabIndex = 13;
            this.Button_GoExcel.Text = "▶";
            this.Button_GoExcel.UseVisualStyleBackColor = true;
            this.Button_GoExcel.Click += new System.EventHandler(this.Button_GoExcel_Click);
            // 
            // Button_SetExcel
            // 
            this.Button_SetExcel.Location = new System.Drawing.Point(607, 333);
            this.Button_SetExcel.Name = "Button_SetExcel";
            this.Button_SetExcel.Size = new System.Drawing.Size(55, 39);
            this.Button_SetExcel.TabIndex = 12;
            this.Button_SetExcel.Text = "...";
            this.Button_SetExcel.UseVisualStyleBackColor = true;
            this.Button_SetExcel.Click += new System.EventHandler(this.Button_SetExcel_Click);
            // 
            // Button_GoBot
            // 
            this.Button_GoBot.Location = new System.Drawing.Point(666, 187);
            this.Button_GoBot.Name = "Button_GoBot";
            this.Button_GoBot.Size = new System.Drawing.Size(55, 39);
            this.Button_GoBot.TabIndex = 17;
            this.Button_GoBot.Text = "▶";
            this.Button_GoBot.UseVisualStyleBackColor = true;
            this.Button_GoBot.Click += new System.EventHandler(this.Button_GoBot_Click);
            // 
            // Button_SetBot
            // 
            this.Button_SetBot.Location = new System.Drawing.Point(607, 187);
            this.Button_SetBot.Name = "Button_SetBot";
            this.Button_SetBot.Size = new System.Drawing.Size(55, 39);
            this.Button_SetBot.TabIndex = 16;
            this.Button_SetBot.Text = "...";
            this.Button_SetBot.UseVisualStyleBackColor = true;
            this.Button_SetBot.Click += new System.EventHandler(this.Button_SetBot_Click);
            // 
            // BotPathText
            // 
            this.BotPathText.AutoSize = true;
            this.BotPathText.Location = new System.Drawing.Point(12, 185);
            this.BotPathText.Name = "BotPathText";
            this.BotPathText.Size = new System.Drawing.Size(60, 12);
            this.BotPathText.TabIndex = 15;
            this.BotPathText.Text = "BotPath : ";
            // 
            // BotPathValue
            // 
            this.BotPathValue.Location = new System.Drawing.Point(12, 203);
            this.BotPathValue.Name = "BotPathValue";
            this.BotPathValue.Size = new System.Drawing.Size(590, 21);
            this.BotPathValue.TabIndex = 14;
            this.BotPathValue.Text = "E:\\SVN\\ClosersMobile\\Program\\Server\\Trunk\\BotClient\\Net";
            // 
            // Button_GoEnum
            // 
            this.Button_GoEnum.Location = new System.Drawing.Point(667, 278);
            this.Button_GoEnum.Name = "Button_GoEnum";
            this.Button_GoEnum.Size = new System.Drawing.Size(55, 39);
            this.Button_GoEnum.TabIndex = 21;
            this.Button_GoEnum.Text = "▶";
            this.Button_GoEnum.UseVisualStyleBackColor = true;
            this.Button_GoEnum.Click += new System.EventHandler(this.Button_GoEnum_Click);
            // 
            // Button_SetEnum
            // 
            this.Button_SetEnum.Location = new System.Drawing.Point(608, 278);
            this.Button_SetEnum.Name = "Button_SetEnum";
            this.Button_SetEnum.Size = new System.Drawing.Size(55, 39);
            this.Button_SetEnum.TabIndex = 20;
            this.Button_SetEnum.Text = "...";
            this.Button_SetEnum.UseVisualStyleBackColor = true;
            this.Button_SetEnum.Click += new System.EventHandler(this.Button_SetEnum_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 276);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "EnumExcelPath : ";
            // 
            // EnumExcelPathValue
            // 
            this.EnumExcelPathValue.Location = new System.Drawing.Point(13, 294);
            this.EnumExcelPathValue.Name = "EnumExcelPathValue";
            this.EnumExcelPathValue.Size = new System.Drawing.Size(590, 21);
            this.EnumExcelPathValue.TabIndex = 18;
            this.EnumExcelPathValue.Text = "E:\\SVN\\ClosersMobile\\Program\\Deploy\\Packet";
            // 
            // Button_GoServerPython
            // 
            this.Button_GoServerPython.Location = new System.Drawing.Point(666, 130);
            this.Button_GoServerPython.Name = "Button_GoServerPython";
            this.Button_GoServerPython.Size = new System.Drawing.Size(55, 39);
            this.Button_GoServerPython.TabIndex = 25;
            this.Button_GoServerPython.Text = "▶";
            this.Button_GoServerPython.UseVisualStyleBackColor = true;
            this.Button_GoServerPython.Click += new System.EventHandler(this.Button_GoServerPython_Click);
            // 
            // Button_SetServerPython
            // 
            this.Button_SetServerPython.Location = new System.Drawing.Point(607, 130);
            this.Button_SetServerPython.Name = "Button_SetServerPython";
            this.Button_SetServerPython.Size = new System.Drawing.Size(55, 39);
            this.Button_SetServerPython.TabIndex = 24;
            this.Button_SetServerPython.Text = "...";
            this.Button_SetServerPython.UseVisualStyleBackColor = true;
            this.Button_SetServerPython.Click += new System.EventHandler(this.Button_SetServerPython_Click);
            // 
            // ServerPythonPathText
            // 
            this.ServerPythonPathText.AutoSize = true;
            this.ServerPythonPathText.Location = new System.Drawing.Point(12, 128);
            this.ServerPythonPathText.Name = "ServerPythonPathText";
            this.ServerPythonPathText.Size = new System.Drawing.Size(117, 12);
            this.ServerPythonPathText.TabIndex = 23;
            this.ServerPythonPathText.Text = "ServerPythonPath : ";
            // 
            // ServerPythonPathValue
            // 
            this.ServerPythonPathValue.Location = new System.Drawing.Point(12, 146);
            this.ServerPythonPathValue.Name = "ServerPythonPathValue";
            this.ServerPythonPathValue.Size = new System.Drawing.Size(590, 21);
            this.ServerPythonPathValue.TabIndex = 22;
            this.ServerPythonPathValue.Text = "E:\\SVN\\ClosersMobile\\Program\\Server\\Python";
            // 
            // MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 508);
            this.Controls.Add(this.Button_GoServerPython);
            this.Controls.Add(this.Button_SetServerPython);
            this.Controls.Add(this.ServerPythonPathText);
            this.Controls.Add(this.ServerPythonPathValue);
            this.Controls.Add(this.Button_GoEnum);
            this.Controls.Add(this.Button_SetEnum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EnumExcelPathValue);
            this.Controls.Add(this.Button_GoBot);
            this.Controls.Add(this.Button_SetBot);
            this.Controls.Add(this.BotPathText);
            this.Controls.Add(this.BotPathValue);
            this.Controls.Add(this.Button_GoExcel);
            this.Controls.Add(this.Button_SetExcel);
            this.Controls.Add(this.Button_GoServer);
            this.Controls.Add(this.Button_SetServer);
            this.Controls.Add(this.Button_GoClient);
            this.Controls.Add(this.Button_SetClient);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.PacketExcelPathText);
            this.Controls.Add(this.PacketExcelPathValue);
            this.Controls.Add(this.ServerPathText);
            this.Controls.Add(this.ServerPathValue);
            this.Controls.Add(this.ClientPathText);
            this.Controls.Add(this.ClientPathValue);
            this.Controls.Add(this.Button_MakePacketCode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainPanel";
            this.Text = "PacketCodeMaker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_MakePacketCode;
        private System.Windows.Forms.TextBox ClientPathValue;
        private System.Windows.Forms.Label ClientPathText;
        private System.Windows.Forms.Label ServerPathText;
        private System.Windows.Forms.TextBox ServerPathValue;
        private System.Windows.Forms.Label PacketExcelPathText;
        private System.Windows.Forms.TextBox PacketExcelPathValue;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Button Button_SetClient;
        private System.Windows.Forms.Button Button_GoClient;
        private System.Windows.Forms.Button Button_GoServer;
        private System.Windows.Forms.Button Button_SetServer;
        private System.Windows.Forms.Button Button_GoExcel;
        private System.Windows.Forms.Button Button_SetExcel;
        private System.Windows.Forms.Button Button_GoBot;
        private System.Windows.Forms.Button Button_SetBot;
        private System.Windows.Forms.Label BotPathText;
        private System.Windows.Forms.TextBox BotPathValue;
        private System.Windows.Forms.Button Button_GoEnum;
        private System.Windows.Forms.Button Button_SetEnum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EnumExcelPathValue;
        private System.Windows.Forms.Button Button_GoServerPython;
        private System.Windows.Forms.Button Button_SetServerPython;
        private System.Windows.Forms.Label ServerPythonPathText;
        private System.Windows.Forms.TextBox ServerPythonPathValue;
    }
}