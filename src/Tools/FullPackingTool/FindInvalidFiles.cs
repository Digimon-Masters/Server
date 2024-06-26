using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FullPackingTool
{
    public partial class FindInvalidFiles : Form
    {
        public List<string> fileList { get; set; }
        public FindInvalidFiles()
        {
            InitializeComponent();
        }

        private void WindowLoad(object sender, EventArgs e)
        {
            InvalidFileList.Items.Clear();            
            Counter.Text = string.Format(@"잘못된 파일 이름 및 폴더 명 갯수 : {0}", fileList.Count);
            if ( fileList.Count > 0 )
            {
                foreach (string i in fileList)
                    InvalidFileList.Items.Add(i);
            }
        }

        private void LIst_DoubleClick(object sender, MouseEventArgs e)
        {
            if(InvalidFileList.SelectedItem != null)
            {
                string selText = InvalidFileList.SelectedItem.ToString();
                string path = Path.GetDirectoryName(selText);
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }
    }
}
