using DSOBinXml.Process;

namespace DSOBinXml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;

            new MapPortalConverter().BinToXml();

            panel1.Enabled = true;
        }

        public void button2_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;

            new MapPortalConverter().XmlToBin();

            panel1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;

            new ItemListConverter().BinToXml();

            panel1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;

            new ItemListConverter().XmlToBin();

            panel1.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;

            new QuestConverter().BinToXml();

            panel1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;

            new QuestConverter().XmlToBin();

            panel1.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;

            new ModelConverter().BinToXml();

            panel1.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            new ModelConverter().XmlToBin();
            panel1.Enabled = true;
        }
    }
}