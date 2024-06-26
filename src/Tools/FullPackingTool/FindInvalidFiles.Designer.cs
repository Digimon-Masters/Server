namespace FullPackingTool
{
    partial class FindInvalidFiles
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
            this.InvalidFileList = new System.Windows.Forms.ListBox();
            this.Counter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InvalidFileList
            // 
            this.InvalidFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvalidFileList.FormattingEnabled = true;
            this.InvalidFileList.ItemHeight = 12;
            this.InvalidFileList.Location = new System.Drawing.Point(12, 42);
            this.InvalidFileList.Name = "InvalidFileList";
            this.InvalidFileList.Size = new System.Drawing.Size(591, 616);
            this.InvalidFileList.TabIndex = 0;
            this.InvalidFileList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LIst_DoubleClick);
            // 
            // Counter
            // 
            this.Counter.AutoSize = true;
            this.Counter.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Counter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Counter.Location = new System.Drawing.Point(8, 9);
            this.Counter.Name = "Counter";
            this.Counter.Size = new System.Drawing.Size(65, 21);
            this.Counter.TabIndex = 1;
            this.Counter.Text = "Count";
            this.Counter.UseMnemonic = false;
            // 
            // FindInvalidFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 666);
            this.Controls.Add(this.Counter);
            this.Controls.Add(this.InvalidFileList);
            this.Name = "FindInvalidFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FindInvalidFiles";
            this.Load += new System.EventHandler(this.WindowLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox InvalidFileList;
        private System.Windows.Forms.Label Counter;
    }
}