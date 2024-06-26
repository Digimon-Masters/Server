using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;

namespace FullPackingTool
{
    public partial class PackingTool : Form
    {
        private string spChar = @"[^a-zA-Z0-9_\s\.\`\-\'\~\!\@\#\$\%\^\&\(\)\+\{\}\[\]]+$";
        private string[] exceptFolder = new string[] {"music", "np", "shaders", "sound", "staticsound" };
        private string[] exceptExt = new string[] { ".pf", ".hf", ".exe" };

        public struct sFolderList {
            public string folderName;
            public List<string> files;
            public List<sFolderList> subDirInfo;
        }

        public PackingTool() {
            InitializeComponent();
            PackingPath.Text = Directory.GetCurrentDirectory().ToString();
        }

        private void FindFolder_Click(object sender, EventArgs e) {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = PackingPath.Text;
            dialog.IsFolderPicker = true;
            if (CommonFileDialogResult.Ok == dialog.ShowDialog() )
            {
                PackingPath.Text = dialog.FileName;
                SearchFile(PackingPath.Text);
            }
        }
        
        private void SearchFile( string path ) {
            FileView.Nodes.Clear();
            SelectTreePath.Text = @"";
            if (string.IsNullOrEmpty(path))
                return;

            List<sFolderList> finder = new List<sFolderList>();
            GetDirFiles(PackingPath.Text, finder);

            TreeNode rootNode = new TreeNode(Path.GetDirectoryName(path));
            InsertTreeViewList(rootNode, finder);
            FileView.Nodes.Add(rootNode);

            List<string> invalidFiles = new List<string>();
            SearchNode(spChar, FileView.Nodes[0], invalidFiles);

            if ( invalidFiles.Count > 0 )
            {
                FindInvalidFiles di = new FindInvalidFiles();
                di.fileList = invalidFiles;
                di.ShowDialog();
            }
        }

        private void InsertTreeViewList( TreeNode viewer, List<sFolderList> finder ) {
            foreach (sFolderList i in finder)
            {
                TreeNode rootNode = new TreeNode(i.folderName);
                if (i.subDirInfo.Count > 0)
                    InsertTreeViewList(rootNode, i.subDirInfo);

                foreach ( string j in i.files )
                    rootNode.Nodes.Add(j);

                viewer.Nodes.Add(rootNode);
            }
        }

        private bool IsExceptionStr( string org, string[] check )
        {
            foreach( string str in check )
            {
                if (str.Equals(org, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void GetDirFiles(string rootPath, List<sFolderList> findFiles ) {

            string sRootFolder = Path.GetFileName(rootPath);
            if (IsExceptionStr(sRootFolder, exceptFolder))
                return;

            sFolderList fileList = new sFolderList();
            fileList.folderName = sRootFolder;
            fileList.files = new List<string>();
            fileList.subDirInfo = new List<sFolderList>();

            // 현재 디렉토리 파일 목록
            string[] files = Directory.GetFiles(rootPath);
            foreach (string f in files)
            {
                string fileExt = Path.GetExtension(f);
                if (IsExceptionStr(fileExt, exceptExt))
                    continue;
              
                fileList.files.Add(Path.GetFileName(f));
            }                

            findFiles.Add(fileList);

            // 하위 디렉토리 목록
            string[] dir = Directory.GetDirectories(rootPath);
            if (dir.Length > 0)
            {
                foreach (string subDir in dir)
                    GetDirFiles(subDir, fileList.subDirInfo );
            }
        }    

        private void TreeViewerSelect(object sender, TreeViewEventArgs e){
            SelectTreePath.Text = e.Node.FullPath;
        }

        private void PathInputKeyDown(object sender, KeyEventArgs e){
            if( e.KeyCode == Keys.Enter)
                SearchFile(PackingPath.Text);
        }

        private void SearchNode( string Searchtext, TreeNode StartNode, List<string> itemList )
        {
            string name = Path.GetFileNameWithoutExtension(StartNode.Text);
            if (Regex.IsMatch(name, Searchtext))
            {
                itemList.Add(StartNode.FullPath);
                StartNode.ForeColor = Color.Red;
            }

            foreach (TreeNode i in StartNode.Nodes)
                SearchNode(Searchtext, i, itemList);
        }
        private void FileDeleteBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectTreePath.Text))
                return;
            try
            {
                string ext = Path.GetExtension(SelectTreePath.Text);
                if (string.IsNullOrEmpty(ext))
                {
                    DirectoryInfo di = new DirectoryInfo(SelectTreePath.Text);
                    di.Delete(true);
                }
                else
                    File.Delete(SelectTreePath.Text);

                FileView.Nodes.Remove(FileView.SelectedNode);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void PackingBtn_Click(object sender, EventArgs e)
        {
            if (FileView.Nodes.Count == 0)
                return;

            string fileName = Path.GetFileName(PackFileNameText.Text);
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("Pack 파일 이름을 넣으세요.");
                return;
            }

            List<string> files = new List<string>();
            GetFileViewList(FileView.Nodes[0], "", files);
            DoPacking(fileName, files);
        }

        private void GetFileViewList( TreeNode root, string rootPath, List<string> lists )
        {            
            if (0 == root.Nodes.Count)
                lists.Add(rootPath );
            else
            {
                foreach (TreeNode no in root.Nodes)
                {
                    if( 0 == rootPath.Length )
                        GetFileViewList(no, no.Text, lists);
                    else
                        GetFileViewList(no, rootPath + "\\" + no.Text, lists);
                }                    
            }                
        }

        private void DoPacking( string packName, List<string> packFiles )
        {
            if( 0 == packFiles.Count )
            {
                MessageBox.Show(@"Packing File Count Zero!!!");
                return;
            }

            
            var t = new Thread(() => ThreadPacking(packName, packFiles));
            t.Start();
        }

        private static void ThreadPacking(string packName, List<string> packFiles)
        {
            CliFPSystem.FPSUtil packFun = new CliFPSystem.FPSUtil();
            if (packFun.DoPacking(packName, packFiles))
            {
                MessageBox.Show(@"Success Packing!!");

            }
            else
                MessageBox.Show(string.Format(@"Packing Fales : {0}", packName));
        }

        /// <summary>
        /// UnPacking Functions
        /// </summary>        
        private void UnpackFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = UnpackFolder.Text;
            dialog.Filter = "PF/HF파일(*.pf,*.hf)|*.pf;*.hf";
            if (DialogResult.OK == dialog.ShowDialog())
                FullPath.Text = dialog.FileName;                
        }

        private void FindOutputBtn_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = OutputPath.Text;
            dialog.IsFolderPicker = true;
            if (CommonFileDialogResult.Ok == dialog.ShowDialog())
            {
                OutputPath.Text = dialog.FileName;    
            }
        }

        private void UnPackBtn_Click(object sender, EventArgs e)
        {
            string fullPath = Path.GetDirectoryName(FullPath.Text);
            string unpackfile = Path.GetFileNameWithoutExtension(FullPath.Text);
            string sFiles = fullPath + "\\" + unpackfile;
            if ( string.IsNullOrEmpty(sFiles) )
            {
                MessageBox.Show(string.Format(@"UnPackFileName false : {0}", FullPath.Text));
                return;
            }

            var t = new Thread(() => DoUnPacking(sFiles));
            t.Start();
        }

        private static void DoUnPacking(string unpackFileName)
        {
            CliFPSystem.FPSUtil packFun = new CliFPSystem.FPSUtil();
            if( packFun.DoUnPacking(unpackFileName))
                MessageBox.Show(@"Success UnPacking!!");
            else
                MessageBox.Show(string.Format(@"Un Packing Fales : {0}", unpackFileName));
        }
    }
}
