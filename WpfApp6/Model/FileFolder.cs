using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace WpfApp6.Model
{
    public class FileFolder
    {
        public FileFolder(){}
        private string[] FileStyle = new string[] { "*.png", "*.jpg", "*.jpeg", "*.jfif" };
        public void Deletefile(List<string> str, string FolderNameCopy,string FolderName,string Filename)
        {

            foreach (var item in str)
            {

                System.IO.DirectoryInfo di = new DirectoryInfo(item);
                foreach (FileInfo file in di.GetFiles())
                {
                    if (file.Name==Filename)
                    {

                         File.Delete(file.FullName);
                    }
                }
            }
        }
        public void SaveAs()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowDialog();
            CopyImagefolder(dialog.SelectedPath, "Image");
            CopyImagefolder(dialog.SelectedPath);
        }
        public void DeleteFolderFile(string FolderN)
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(FolderN);
                foreach (FileInfo file in di.GetFiles())file.Delete();
            }
            catch{}
        }
        public void CopyImagefolder(string Folder,string CopyFolder="Imagecopy")
        {
            foreach (var item in Directory.GetFiles(CopyFolder, "*.*"))
            {
                
                FileInfo f1 = new FileInfo($"{item}");
                FileInfo f = new FileInfo($"{Folder}/{f1.Name}");
                try { System.IO.File.Copy(item, f.FullName, true); }
                catch (Exception) { continue; }
            }
        }
        public void saveFolderInFolder(List<string> str,string FolderName)
        {
            bool isok2 = true;
            foreach (var item in str)
            {
                foreach (var I in FileStyle)
                {
                     foreach (var item2 in Directory.GetFiles(item, I)){

                            FileInfo f1 = new FileInfo($"{item2}");
                            FileInfo f = new FileInfo($"{FolderName}/{f1.Name}");    
                            try { System.IO.File.Copy(item2, f.FullName, true); }
                            catch (Exception){continue;}
                     }
                }
            }
            CopyImagefolder(FolderName);


        }
    }
}
