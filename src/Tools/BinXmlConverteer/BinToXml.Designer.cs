namespace BinXmlConverter
{
    partial class BinToXml
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinToXml));
            toolTip1 = new ToolTip(components);
            button1 = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            pictureBox1 = new PictureBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            button13 = new Button();
            button14 = new Button();
            button15 = new Button();
            button16 = new Button();
            button17 = new Button();
            button18 = new Button();
            button19 = new Button();
            button20 = new Button();
            button21 = new Button();
            button22 = new Button();
            button23 = new Button();
            button24 = new Button();
            button25 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(450, 151);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-31, -14);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(640, 360);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // button2
            // 
            button2.Location = new Point(-3, 242);
            button2.Name = "button2";
            button2.Size = new Size(130, 39);
            button2.TabIndex = 2;
            button2.Text = "DigimonList.xml";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Digimon_ListToXml_Click;
            // 
            // button3
            // 
            button3.Location = new Point(-3, 184);
            button3.Name = "button3";
            button3.Size = new Size(130, 39);
            button3.TabIndex = 3;
            button3.Text = "DigimonList.bin";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Digimon_ListToBin_Click;
            // 
            // button4
            // 
            button4.Location = new Point(269, 184);
            button4.Name = "button4";
            button4.Size = new Size(130, 39);
            button4.TabIndex = 4;
            button4.Text = "DigimonEvo.bin";
            button4.UseVisualStyleBackColor = true;
            button4.Click += DigimonEvoToBin_Click;
            // 
            // button5
            // 
            button5.Location = new Point(269, 242);
            button5.Name = "button5";
            button5.Size = new Size(130, 39);
            button5.TabIndex = 5;
            button5.Text = "DigimonEvo.xml";
            button5.UseVisualStyleBackColor = true;
            button5.Click += DigimonEvoToXml_Click;
            // 
            // button6
            // 
            button6.Location = new Point(269, 128);
            button6.Name = "button6";
            button6.Size = new Size(130, 39);
            button6.TabIndex = 6;
            button6.Text = "Model.xml";
            button6.UseVisualStyleBackColor = true;
            button6.Click += ModelToXml_Click;
            // 
            // button7
            // 
            button7.Location = new Point(269, 72);
            button7.Name = "button7";
            button7.Size = new Size(130, 39);
            button7.TabIndex = 7;
            button7.Text = "Model.dat";
            button7.UseVisualStyleBackColor = true;
            button7.Click += ModelToBin_Click;
            // 
            // button8
            // 
            button8.Location = new Point(-3, 128);
            button8.Name = "button8";
            button8.Size = new Size(130, 39);
            button8.TabIndex = 8;
            button8.Text = "Ride.xml";
            button8.UseVisualStyleBackColor = true;
            button8.Click += RideToXml_Click;
            // 
            // button9
            // 
            button9.Location = new Point(-3, 72);
            button9.Name = "button9";
            button9.Size = new Size(130, 39);
            button9.TabIndex = 9;
            button9.Text = "Ride.bin";
            button9.UseVisualStyleBackColor = true;
            button9.Click += RideToBin_Click;
            // 
            // button10
            // 
            button10.Location = new Point(-3, 296);
            button10.Name = "button10";
            button10.Size = new Size(130, 39);
            button10.TabIndex = 10;
            button10.Text = "Buff.bin";
            button10.UseVisualStyleBackColor = true;
            button10.Click += BuffToBinClick;
            // 
            // button11
            // 
            button11.Location = new Point(269, 296);
            button11.Name = "button11";
            button11.Size = new Size(130, 39);
            button11.TabIndex = 11;
            button11.Text = "Buff.xml";
            button11.UseVisualStyleBackColor = true;
            button11.Click += BuffToXml_Click;
            // 
            // button12
            // 
            button12.Location = new Point(-3, 12);
            button12.Name = "button12";
            button12.Size = new Size(130, 39);
            button12.TabIndex = 12;
            button12.Text = "CharCreateTable.bin";
            button12.UseVisualStyleBackColor = true;
            button12.Click += CharCreateTableToBin_CLick;
            // 
            // button13
            // 
            button13.Location = new Point(269, 12);
            button13.Name = "button13";
            button13.Size = new Size(130, 39);
            button13.TabIndex = 13;
            button13.Text = "CharCreateTable.xml";
            button13.UseVisualStyleBackColor = true;
            button13.Click += CharCreateTableToXml_Click;
            // 
            // button14
            // 
            button14.Location = new Point(133, 12);
            button14.Name = "button14";
            button14.Size = new Size(130, 39);
            button14.TabIndex = 14;
            button14.Text = "Npc.bin";
            button14.UseVisualStyleBackColor = true;
            button14.Click += NpcToBin_Click;
            // 
            // button15
            // 
            button15.Location = new Point(133, 72);
            button15.Name = "button15";
            button15.Size = new Size(130, 39);
            button15.TabIndex = 15;
            button15.Text = "Npc.xml";
            button15.UseVisualStyleBackColor = true;
            button15.Click += NpcToXml_Click;
            // 
            // button16
            // 
            button16.Location = new Point(133, 128);
            button16.Name = "button16";
            button16.Size = new Size(130, 39);
            button16.TabIndex = 16;
            button16.Text = "DMBase.bin";
            button16.UseVisualStyleBackColor = true;
            button16.Click += DMBaseToBin_Click;
            // 
            // button17
            // 
            button17.Location = new Point(133, 184);
            button17.Name = "button17";
            button17.Size = new Size(130, 39);
            button17.TabIndex = 17;
            button17.Text = "DMBase.xml";
            button17.UseVisualStyleBackColor = true;
            button17.Click += DMBaseToXml_Click;
            // 
            // button18
            // 
            button18.Location = new Point(133, 242);
            button18.Name = "button18";
            button18.Size = new Size(130, 39);
            button18.TabIndex = 18;
            button18.Text = "Skill.xml";
            button18.UseVisualStyleBackColor = true;
            button18.Click += SkillToXml_Click;
            // 
            // button19
            // 
            button19.Location = new Point(133, 296);
            button19.Name = "button19";
            button19.Size = new Size(130, 39);
            button19.TabIndex = 19;
            button19.Text = "Skill.bin";
            button19.UseVisualStyleBackColor = true;
            button19.Click += SkillToBin_Click;
            // 
            // button20
            // 
            button20.Location = new Point(405, 12);
            button20.Name = "button20";
            button20.Size = new Size(130, 39);
            button20.TabIndex = 20;
            button20.Text = "ItemList.bin";
            button20.UseVisualStyleBackColor = true;
            button20.Click += ItemListToBin_Click;
            // 
            // button21
            // 
            button21.Location = new Point(405, 72);
            button21.Name = "button21";
            button21.Size = new Size(130, 39);
            button21.TabIndex = 21;
            button21.Text = "ItemList.xml";
            button21.UseVisualStyleBackColor = true;
            button21.Click += ItemListToXml_Click;
            // 
            // button22
            // 
            button22.Location = new Point(405, 128);
            button22.Name = "button22";
            button22.Size = new Size(130, 39);
            button22.TabIndex = 22;
            button22.Text = "Event.xml";
            button22.UseVisualStyleBackColor = true;
            button22.Click += EventToXml_Click;
            // 
            // button23
            // 
            button23.Location = new Point(405, 184);
            button23.Name = "button23";
            button23.Size = new Size(130, 39);
            button23.TabIndex = 23;
            button23.Text = "Event.bin";
            button23.UseVisualStyleBackColor = true;
            button23.Click += EventToBin_Click;
            // 
            // button24
            // 
            button24.Location = new Point(405, 242);
            button24.Name = "button24";
            button24.Size = new Size(130, 39);
            button24.TabIndex = 24;
            button24.Text = "Gotcha.bin";
            button24.UseVisualStyleBackColor = true;
            button24.Click += GotchaToBin_Click;
            // 
            // button25
            // 
            button25.Location = new Point(405, 296);
            button25.Name = "button25";
            button25.Size = new Size(130, 39);
            button25.TabIndex = 25;
            button25.Text = "Gotcha.xml";
            button25.UseVisualStyleBackColor = true;
            button25.Click += GotchaToXml_Click;
            // 
            // BinToXml
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(532, 344);
            Controls.Add(button25);
            Controls.Add(button24);
            Controls.Add(button23);
            Controls.Add(button22);
            Controls.Add(button21);
            Controls.Add(button20);
            Controls.Add(button19);
            Controls.Add(button18);
            Controls.Add(button17);
            Controls.Add(button16);
            Controls.Add(button15);
            Controls.Add(button14);
            Controls.Add(button13);
            Controls.Add(button12);
            Controls.Add(button11);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "BinToXml";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BinXmlConverter";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolTip toolTip1;
        private Button button1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private PictureBox pictureBox1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private Button button13;
        private Button button14;
        private Button button15;
        private Button button16;
        private Button button17;
        private Button button18;
        private Button button19;
        private Button button20;
        private Button button21;
        private Button button22;
        private Button button23;
        private Button button24;
        private Button button25;
    }
}