namespace DSOBinXml
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            panel1 = new Panel();
            panel3 = new Panel();
            button8 = new Button();
            button6 = new Button();
            panel2 = new Panel();
            button7 = new Button();
            button5 = new Button();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(22, 31);
            button1.Name = "button1";
            button1.Size = new Size(99, 29);
            button1.TabIndex = 0;
            button1.Text = "MapPortal";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(29, 10);
            label1.Name = "label1";
            label1.Size = new Size(83, 15);
            label1.TabIndex = 1;
            label1.Text = "BIN >>> XML";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(40, 10);
            label2.Name = "label2";
            label2.Size = new Size(83, 15);
            label2.TabIndex = 2;
            label2.Text = "XML >>> BIN";
            // 
            // button2
            // 
            button2.Location = new Point(32, 31);
            button2.Name = "button2";
            button2.Size = new Size(99, 29);
            button2.TabIndex = 3;
            button2.Text = "MapPortal";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(22, 66);
            button3.Name = "button3";
            button3.Size = new Size(99, 29);
            button3.TabIndex = 4;
            button3.Text = "ItemList";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(32, 66);
            button4.Name = "button4";
            button4.Size = new Size(99, 29);
            button4.TabIndex = 5;
            button4.Text = "ItemList";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(334, 211);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(button8);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(button6);
            panel3.Controls.Add(button4);
            panel3.Location = new Point(172, 6);
            panel3.Name = "panel3";
            panel3.Size = new Size(155, 197);
            panel3.TabIndex = 9;
            // 
            // button8
            // 
            button8.Location = new Point(32, 136);
            button8.Name = "button8";
            button8.Size = new Size(99, 29);
            button8.TabIndex = 8;
            button8.Text = "Model";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button6
            // 
            button6.Location = new Point(32, 101);
            button6.Name = "button6";
            button6.Size = new Size(99, 29);
            button6.TabIndex = 7;
            button6.Text = "Quest";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(button7);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(button5);
            panel2.Controls.Add(button3);
            panel2.Location = new Point(6, 6);
            panel2.Name = "panel2";
            panel2.Size = new Size(155, 197);
            panel2.TabIndex = 8;
            // 
            // button7
            // 
            button7.Location = new Point(22, 136);
            button7.Name = "button7";
            button7.Size = new Size(99, 29);
            button7.TabIndex = 7;
            button7.Text = "Model";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button5
            // 
            button5.Location = new Point(22, 101);
            button5.Name = "button5";
            button5.Size = new Size(99, 29);
            button5.TabIndex = 6;
            button5.Text = "Quest";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(359, 238);
            Controls.Add(panel1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DSO Bin Conversor";
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private Button button2;
        private Button button3;
        private Button button4;
        private Panel panel1;
        private Button button5;
        private Button button6;
        private Panel panel3;
        private Panel panel2;
        private Button button8;
        private Button button7;
    }
}