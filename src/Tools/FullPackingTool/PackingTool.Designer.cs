namespace FullPackingTool
{
    partial class PackingTool
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.packing = new System.Windows.Forms.TabPage();
            this.PackingBtn = new System.Windows.Forms.Button();
            this.FileDeleteBtn = new System.Windows.Forms.Button();
            this.SelectTreePath = new System.Windows.Forms.TextBox();
            this.FileView = new System.Windows.Forms.TreeView();
            this.PackingPath = new System.Windows.Forms.TextBox();
            this.FindFolder = new System.Windows.Forms.Button();
            this.unpacking = new System.Windows.Forms.TabPage();
            this.FindOutputBtn = new System.Windows.Forms.Button();
            this.OutputPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UnPackBtn = new System.Windows.Forms.Button();
            this.TargetPath = new System.Windows.Forms.Label();
            this.UnpackFolder = new System.Windows.Forms.Button();
            this.FullPath = new System.Windows.Forms.TextBox();
            this.PackFileNameText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.packing.SuspendLayout();
            this.unpacking.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "SelectPath";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.packing);
            this.tabControl1.Controls.Add(this.unpacking);
            this.tabControl1.Location = new System.Drawing.Point(12, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(713, 540);
            this.tabControl1.TabIndex = 7;
            // 
            // packing
            // 
            this.packing.Controls.Add(this.label4);
            this.packing.Controls.Add(this.PackFileNameText);
            this.packing.Controls.Add(this.PackingBtn);
            this.packing.Controls.Add(this.FileDeleteBtn);
            this.packing.Controls.Add(this.SelectTreePath);
            this.packing.Controls.Add(this.label1);
            this.packing.Controls.Add(this.label2);
            this.packing.Controls.Add(this.FileView);
            this.packing.Controls.Add(this.PackingPath);
            this.packing.Controls.Add(this.FindFolder);
            this.packing.Location = new System.Drawing.Point(4, 22);
            this.packing.Name = "packing";
            this.packing.Padding = new System.Windows.Forms.Padding(3);
            this.packing.Size = new System.Drawing.Size(705, 514);
            this.packing.TabIndex = 0;
            this.packing.Text = "Packing";
            this.packing.UseVisualStyleBackColor = true;
            // 
            // PackingBtn
            // 
            this.PackingBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PackingBtn.Location = new System.Drawing.Point(98, 81);
            this.PackingBtn.Name = "PackingBtn";
            this.PackingBtn.Size = new System.Drawing.Size(503, 35);
            this.PackingBtn.TabIndex = 11;
            this.PackingBtn.Text = "Do Packing";
            this.PackingBtn.UseVisualStyleBackColor = true;
            this.PackingBtn.Click += new System.EventHandler(this.PackingBtn_Click);
            // 
            // FileDeleteBtn
            // 
            this.FileDeleteBtn.Location = new System.Drawing.Point(607, 50);
            this.FileDeleteBtn.Name = "FileDeleteBtn";
            this.FileDeleteBtn.Size = new System.Drawing.Size(92, 23);
            this.FileDeleteBtn.TabIndex = 10;
            this.FileDeleteBtn.Text = "Delete File";
            this.FileDeleteBtn.UseVisualStyleBackColor = true;
            this.FileDeleteBtn.Click += new System.EventHandler(this.FileDeleteBtn_Click);
            // 
            // SelectTreePath
            // 
            this.SelectTreePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectTreePath.Location = new System.Drawing.Point(98, 52);
            this.SelectTreePath.Name = "SelectTreePath";
            this.SelectTreePath.ReadOnly = true;
            this.SelectTreePath.Size = new System.Drawing.Size(503, 21);
            this.SelectTreePath.TabIndex = 9;
            // 
            // FileView
            // 
            this.FileView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileView.Location = new System.Drawing.Point(6, 126);
            this.FileView.Name = "FileView";
            this.FileView.Size = new System.Drawing.Size(693, 382);
            this.FileView.TabIndex = 8;
            this.FileView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewerSelect);
            // 
            // PackingPath
            // 
            this.PackingPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PackingPath.Location = new System.Drawing.Point(97, 21);
            this.PackingPath.Name = "PackingPath";
            this.PackingPath.Size = new System.Drawing.Size(504, 21);
            this.PackingPath.TabIndex = 7;
            this.PackingPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PathInputKeyDown);
            // 
            // FindFolder
            // 
            this.FindFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindFolder.Location = new System.Drawing.Point(607, 19);
            this.FindFolder.Name = "FindFolder";
            this.FindFolder.Size = new System.Drawing.Size(92, 23);
            this.FindFolder.TabIndex = 6;
            this.FindFolder.Text = "Folder";
            this.FindFolder.UseVisualStyleBackColor = true;
            this.FindFolder.Click += new System.EventHandler(this.FindFolder_Click);
            // 
            // unpacking
            // 
            this.unpacking.Controls.Add(this.FindOutputBtn);
            this.unpacking.Controls.Add(this.OutputPath);
            this.unpacking.Controls.Add(this.label3);
            this.unpacking.Controls.Add(this.UnPackBtn);
            this.unpacking.Controls.Add(this.TargetPath);
            this.unpacking.Controls.Add(this.UnpackFolder);
            this.unpacking.Controls.Add(this.FullPath);
            this.unpacking.Location = new System.Drawing.Point(4, 22);
            this.unpacking.Name = "unpacking";
            this.unpacking.Padding = new System.Windows.Forms.Padding(3);
            this.unpacking.Size = new System.Drawing.Size(705, 514);
            this.unpacking.TabIndex = 1;
            this.unpacking.Text = "Unpacking";
            this.unpacking.UseVisualStyleBackColor = true;
            // 
            // FindOutputBtn
            // 
            this.FindOutputBtn.Location = new System.Drawing.Point(613, 37);
            this.FindOutputBtn.Name = "FindOutputBtn";
            this.FindOutputBtn.Size = new System.Drawing.Size(83, 23);
            this.FindOutputBtn.TabIndex = 6;
            this.FindOutputBtn.Text = "Output Path";
            this.FindOutputBtn.UseVisualStyleBackColor = true;
            this.FindOutputBtn.Click += new System.EventHandler(this.FindOutputBtn_Click);
            // 
            // OutputPath
            // 
            this.OutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputPath.Location = new System.Drawing.Point(89, 39);
            this.OutputPath.Name = "OutputPath";
            this.OutputPath.Size = new System.Drawing.Size(518, 21);
            this.OutputPath.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output Path";
            // 
            // UnPackBtn
            // 
            this.UnPackBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UnPackBtn.Location = new System.Drawing.Point(613, 77);
            this.UnPackBtn.Name = "UnPackBtn";
            this.UnPackBtn.Size = new System.Drawing.Size(83, 23);
            this.UnPackBtn.TabIndex = 3;
            this.UnPackBtn.Text = "UnPacking";
            this.UnPackBtn.UseVisualStyleBackColor = true;
            this.UnPackBtn.Click += new System.EventHandler(this.UnPackBtn_Click);
            // 
            // TargetPath
            // 
            this.TargetPath.AutoSize = true;
            this.TargetPath.Location = new System.Drawing.Point(13, 12);
            this.TargetPath.Name = "TargetPath";
            this.TargetPath.Size = new System.Drawing.Size(70, 12);
            this.TargetPath.TabIndex = 2;
            this.TargetPath.Text = "Target Path";
            // 
            // UnpackFolder
            // 
            this.UnpackFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UnpackFolder.Location = new System.Drawing.Point(613, 7);
            this.UnpackFolder.Name = "UnpackFolder";
            this.UnpackFolder.Size = new System.Drawing.Size(83, 23);
            this.UnpackFolder.TabIndex = 1;
            this.UnpackFolder.Text = "Find File";
            this.UnpackFolder.UseVisualStyleBackColor = true;
            this.UnpackFolder.Click += new System.EventHandler(this.UnpackFolder_Click);
            // 
            // FullPath
            // 
            this.FullPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FullPath.Location = new System.Drawing.Point(89, 8);
            this.FullPath.Name = "FullPath";
            this.FullPath.Size = new System.Drawing.Size(518, 21);
            this.FullPath.TabIndex = 0;
            // 
            // PackFileNameText
            // 
            this.PackFileNameText.Location = new System.Drawing.Point(607, 95);
            this.PackFileNameText.Name = "PackFileNameText";
            this.PackFileNameText.Size = new System.Drawing.Size(92, 21);
            this.PackFileNameText.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(607, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Pack File Name";
            // 
            // PackingTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 557);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "PackingTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PackingTool";
            this.tabControl1.ResumeLayout(false);
            this.packing.ResumeLayout(false);
            this.packing.PerformLayout();
            this.unpacking.ResumeLayout(false);
            this.unpacking.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage packing;
        private System.Windows.Forms.TextBox SelectTreePath;
        private System.Windows.Forms.TreeView FileView;
        private System.Windows.Forms.TextBox PackingPath;
        private System.Windows.Forms.Button FindFolder;
        private System.Windows.Forms.TabPage unpacking;
        private System.Windows.Forms.Label TargetPath;
        private System.Windows.Forms.Button UnpackFolder;
        private System.Windows.Forms.TextBox FullPath;
        private System.Windows.Forms.Button FindOutputBtn;
        private System.Windows.Forms.TextBox OutputPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button UnPackBtn;
        private System.Windows.Forms.Button FileDeleteBtn;
        private System.Windows.Forms.Button PackingBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PackFileNameText;
    }
}

