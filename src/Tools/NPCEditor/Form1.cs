using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;

namespace NPCEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.Maximum = 7;
            progressBar1.Step = 1;
            progressBar1.PerformStep();

            //Unpack .pf
            var currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Directory.SetCurrentDirectory(currentDir + "\\Util");

            Process.Start("Unpack.exe", "Pack03");

            progressBar1.PerformStep();
            

            Thread.Sleep(1500);

            Directory.SetCurrentDirectory(currentDir);
            File.Delete(currentDir + "\\Util\\data\\bin\\english\\temp.file");
            
            var dir = new DirectoryInfo(currentDir + "\\Util\\data_mod\\bin\\english");
            foreach (FileInfo fi in dir.GetFiles())
                fi.Delete();

            progressBar1.PerformStep();

            Thread.Sleep(1500);

            //Copy all bins to data_mod
            //File.Copy(currentDir + "\\data\\bin\\english", currentDir + "\\data_mod\\bin\\english", true);
            FileSystem.CopyDirectory(currentDir + "\\Util\\data\\bin\\english", currentDir + "\\Util\\data_mod\\bin\\english");

            progressBar1.PerformStep();
            Thread.Sleep(1500);

            //Copy bins to .exe dir
            File.Copy(currentDir + "\\Util\\data\\bin\\english\\Npc.bin", currentDir + "\\Util\\Npc.bin", true);
            File.Copy(currentDir + "\\Util\\data\\bin\\english\\MapNpc.bin", currentDir + "\\Util\\MapNpc.bin", true);

            progressBar1.PerformStep();

            Thread.Sleep(1500);

            //Set current dir
            Directory.SetCurrentDirectory(currentDir + "\\Util");

            //Unpack NPCs bin
            Process.Start("BinerpApp.exe", $"Npc.json Npc.bin Npc.xml");

            progressBar1.PerformStep();

            Thread.Sleep(1500);

            //Unpack Map NPCs bin
            Process.Start("BinerpApp.exe", $"MapNpc.json MapNpc.bin MapNpc.xml");

            progressBar1.PerformStep();

            Thread.Sleep(1500);

            //Copy XMLs to root dir
            File.Copy(currentDir + "\\Util\\Npc.xml", currentDir + "\\Npc.xml", true);
            File.Copy(currentDir + "\\Util\\Npc.json", currentDir + "\\Npc.json", true);
            File.Copy(currentDir + "\\Util\\MapNpc.xml", currentDir + "\\MapNpc.xml", true);
            File.Copy(currentDir + "\\Util\\MapNpc.json", currentDir + "\\MapNpc.json", true);

            File.Copy(currentDir + "\\Util\\Npc.xml", currentDir + "\\XMLs\\Npc.xml", true);
            File.Copy(currentDir + "\\Util\\MapNpc.xml", currentDir + "\\XMLs\\MapNpc.xml", true);

            progressBar1.PerformStep();

            MessageBox.Show($"Files has been extracted to \n\"{currentDir + "\\XMLs"}\".", "Unpacked files!");
            Process.Start("explorer.exe", $"{currentDir + "\\XMLs"}");

            progressBar1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.Maximum = 7;
            progressBar1.Step = 1;
            progressBar1.PerformStep();

            progressBar1.Refresh();

            var folder = textBox1.Text;

            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("Select the client folder!");
                return;
            }

            var currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            File.Copy(currentDir + "\\XMLs\\Npc.xml", currentDir + "\\Util\\Npc.xml", true);
            File.Copy(currentDir + "\\XMLs\\MapNpc.xml", currentDir + "\\Util\\MapNpc.xml", true);

            Directory.SetCurrentDirectory(currentDir + "\\Util");

            //Pack NPCs xml
            Process.Start("BinerpApp.exe", $"Npc.json Npc.xml Npc.bin");

            progressBar1.PerformStep();
            Thread.Sleep(1500);

            //PackMap NPCs xml
            Process.Start("BinerpApp.exe", $"MapNpc.json MapNpc.xml MapNpc.bin");

            progressBar1.PerformStep();
            Thread.Sleep(1500);

            currentDir += "\\Util";

            //Update bin files
            File.Copy(currentDir + "\\Npc.bin", currentDir + "\\data_mod\\bin\\english\\Npc.bin", true);
            File.Copy(currentDir + "\\MapNpc.bin", currentDir + "\\data_mod\\bin\\english\\MapNpc.bin", true);

            progressBar1.PerformStep();
            Thread.Sleep(1500);

            //Pack .pf
            Process.Start("MISPack.exe", "-s dso.json -o ./");

            progressBar1.PerformStep();
            progressBar1.PerformStep();
            Thread.Sleep(6500);

            //Jogar no client
            File.Copy(currentDir + "\\Pack03.hf", folder + "\\Data\\Pack03.hf", true);
            File.Copy(currentDir + "\\Pack03.pf", folder + "\\Data\\Pack03.pf", true);

            progressBar1.PerformStep();

            MessageBox.Show($"You can run your client now", "Done");

            progressBar1.Visible = false;
            progressBar1.Value = 0;
        }
    }
}