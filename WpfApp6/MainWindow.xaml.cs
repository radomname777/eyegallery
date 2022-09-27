
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfApp6.Model;

namespace WpfApp6
{
    public partial class MainWindow : Window
    {
        private FileFolder FiFO = new();
        private string FileName { get; set; } = "Image";
        private string[] FileStyle = new string[] { "*.png", "*.jpg","*.jpeg","*.jfif" };
        private bool Boolen { get; set; } = false;
        private int Tags = 0;
        private Button btn()
        {
            Button img = new(); img.Margin = new Thickness(100,10,10,10);
            img.Height = 150; img.Width = 170; img.MouseLeave += İmg_MouseLeave;
            img.Click += Button_Click; img.MouseEnter += İmg_MouseEnter;
            img.BorderThickness = new Thickness(2);
            img.Tag = Tags;
            Tags++;
            return img;
        }
        private void İmg_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Margin = new Thickness(100, 10, 10, 10);
                btn.Height = btn.Height / 1.5;
                btn.Width = btn.Width / 1.5;
            }
        }

        private void İmg_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.Margin = new Thickness(60, 10, 10, 10);
                btn.Height = btn.Height * 1.5;
                btn.Width = btn.Width * 1.5;
            }
        }
        private void MyNewFolderCheck(bool ok)
        {
            if (ok){SelectFolder(true);return; }
            SelectFolder();
        }
        private void MyNewFolder(string FolderName,bool isok=false)
        {
            bool isok2 = true;
            if (isok) PanelW.Children.Clear();
            for (int i = 0; i < FileStyle.Length; i++)
            {
                if (Directory.GetFiles(FolderName, FileStyle[i]).Length!=0)
                {
                    isok2 = false;
                    foreach (var item in Directory.GetFiles(FolderName, FileStyle[i]))
                    {
                        FileInfo fl = new FileInfo(item);
                        var brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(new Uri(@$"{fl.FullName}", UriKind.Relative));
                        Button img = btn();
                        img.Background = brush; 
                        PanelW.Children.Add(img);
                    }
                }
            }
            if (FolderName == FileName){ FiFO.CopyImagefolder("Imagecopy", FileName); }
        }
        
        private void Check(string FileName)
        {
            bool exists = System.IO.Directory.Exists(FileName);
            if (!exists)
                System.IO.Directory.CreateDirectory(FileName);
            else MyNewFolder(FileName);
        }
        public MainWindow()
        {
            InitializeComponent();
            FiFO.DeleteFolderFile("Imagecopy");
            Check("Imagecopy");
            Check(FileName);
        }
        private void ImageCopy(string FileName,bool iso = false)
        {
            bool exists = System.IO.Directory.Exists(FileName);
            if (!exists)
                System.IO.Directory.CreateDirectory(FileName);
            else MyNewFolder(FileName,iso);
        }
        private void Addimage(string FileName,bool iso = false) {
           
            Button img = btn();
            ImageCopy(FileName,iso);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|jpeg files (*.jpeg)| *jpg|jfif files (*.jfif)|*jfif";
             if (openFileDialog.ShowDialog()==true)
             {
                 var brush = new ImageBrush();
                 FileInfo f = new FileInfo($"{FileName}/{openFileDialog.SafeFileName}");
                 try { System.IO.File.Copy(openFileDialog.FileName, f.FullName, true); }
                 catch (Exception)
                 {
                     MessageBox.Show("This photo already exists");
                     return;
                 }
                 brush.ImageSource = new BitmapImage(new Uri(@$"{f.FullName}", UriKind.Relative));
                 img.Background = brush;
                 PanelW.Children.Add(img);
             }
        }
        private List<string> AddFolderN = new List<string> { };
        private void SelectFolder(bool isok = false)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            if (dialog.SelectedPath.Length != 0)
            {
                MyNewFolder(dialog.SelectedPath,isok);
                AddFolderN.Add(dialog.SelectedPath);
                FiFO.saveFolderInFolder(new List<string> {dialog.SelectedPath}, "Imagecopy");
            }
        }
        private void DeleteImage()
        {
            foreach (var item in PanelW.Children)
            {
                if (item is Button btn && btn.BorderBrush.ToString()!="#FF673AB7")
                {
                    ImageBrush ims = new ImageBrush();
                    ims = (ImageBrush)btn.Background;
                    FileInfo F = new FileInfo(ims.ImageSource.ToString());
                    MyNewFolder("Imagecopy",true);
                    FiFO.Deletefile(AddFolderN, "Imagecopy", "Image",F.Name);
                }
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
           
            
            if (sender is MenuItem btn)
            {
                switch (btn.Tag)
                {
                    case "Add":
                        Addimage("Imagecopy",true);
                        break;
                    case "AddF":
                        SelectFolder();
                        Boolen = true;
                        break;
                    case "E_X":
                        if (Boolen){MyNewFolder(FileName, true);Boolen = false; return; }
                        Close();
                        break;
                    case "OpenF":
                        Boolen = true;
                        SelectFolder(true);
                        break;
                    case "Save":
                        FiFO.saveFolderInFolder(AddFolderN, FileName);
                        break;
                    case "SaveA":
                        FiFO.SaveAs();
                        break;
                    case "Del":
                        DeleteImage();
                        break;
                    default:
                        Close();
                        break;
                }
            }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                var window = new view.Window1();
                ImageBrush Folder = (ImageBrush)btn.Background;
                FileInfo a = new FileInfo(Folder.ToString());
                window.Funtion(Folder);
                window.ShowDialog();
            }
        }
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>DragMove();
        private void BG_COLORS(bool var)
        {
            if (var) BG_Color.Background = new SolidColorBrush(Color.FromArgb(200, 20, 20, 20));
            else BG_Color.Background = new SolidColorBrush(Color.FromArgb(250,20,20,20));
         
        }
        private void Btn_Size_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Name == "Min_size")
            {
                WindowState = WindowState.Normal;
                BG_COLORS(true);
                return;
            }
            WindowState = WindowState.Maximized;
            BG_COLORS(false);
        }
    }
}
