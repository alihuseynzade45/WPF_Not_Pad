using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;










namespace Not_Pad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        private object folderDialog;

        public MainWindow()
        {
            InitializeComponent();

        }



        [DllImport("shell32.dll")]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

        [DllImport("shell32.dll")]
        public static extern int SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public string pszDisplayName;
            public string lpszTitle;
            public uint ulFlags;
            public IntPtr lpfn;
            public IntPtr lParam;
            public int iImage;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string filePath1 = FileTextBox.Text;
            if (string.IsNullOrWhiteSpace(FileTextBox.Text))
            {
                MessageBox.Show("xais olunur bir File secesinzi ");
                return;
            }

            string folderPath = FileTextBox.Text;

            string fileName = "NotPad " + DateTime.Now.ToString("yyyy MM dd HH;mm;ss") + ".txt";

            string filePath = System.IO.Path.Combine(folderPath, fileName);

            File.WriteAllText(filePath, new TextRange(richtextbox.Document.ContentStart, richtextbox.Document.ContentEnd).Text);
          
        }


        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {

            BROWSEINFO bi = new BROWSEINFO();
            bi.ulFlags = 0x00000040; // BIF_RETURNONLYFSDIRS
            bi.lpfn = IntPtr.Zero;
            bi.lParam = IntPtr.Zero;
            bi.lpszTitle = "xais olunur bir File secesinzi";
            IntPtr pidl = SHBrowseForFolder(ref bi);
            StringBuilder path = new StringBuilder(260);
            SHGetPathFromIDList(pidl, path);
            FileTextBox.Text = path.ToString();




        }

        private async void AutoSave_Checked(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                await Task.Delay(120000); 

                string filePath1 = FileTextBox.Text;
                if (string.IsNullOrWhiteSpace(FileTextBox.Text))
                {
                    MessageBox.Show("xais olunur auto save ucun bir fayl seçiniz");
                    return;
                }

                string folderPath = FileTextBox.Text;
                string fileName = "NotPad " + DateTime.Now.ToString("yyyy MM dd HH;mm;ss") + ".txt";
                string filePath = System.IO.Path.Combine(folderPath, fileName);

                File.WriteAllText(filePath, new TextRange(richtextbox.Document.ContentStart, richtextbox.Document.ContentEnd).Text);
                continue;
            }
        }



        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(new TextRange(richtextbox.Document.ContentStart, richtextbox.Document.ContentEnd).Text);
        }


        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                richtextbox.Selection.Text = Clipboard.GetText();
            }
        }


        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            if (richtextbox.Selection.Text.Length > 0)
            {
                richtextbox.Selection.Text = string.Empty;
            }
        }


        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            richtextbox.SelectAll();
        }




    }
}
