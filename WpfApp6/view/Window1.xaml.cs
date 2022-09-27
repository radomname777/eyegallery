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
using System.Windows.Shapes;

namespace WpfApp6.view
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int a = 0;
        int iz = 0;
        List<string> str1 = new() {};
        public int numcal()
        {
            int num = 0;
            foreach (var item2 in Directory.GetFiles("Imagecopy", "*.*"))
            {
                FileInfo f1 = new FileInfo($"{item2}");
                str1.Add(f1.FullName);
                num++;
            }
            iz = num;
            //MessageBox.Show(iz.ToString());
            return num;
        }
        public void Func(int num )
        {
            if (iz-1<=a) {a = 0;}
            else if (num <= -1) { a = iz - 1; }
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(@$"{str1[a]}", UriKind.Relative));
            DP.Background = brush;
        }
        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

           // DragMove();
        }
        public void Funtion(ImageBrush U)
        {

             FileInfo f = new FileInfo(U.ImageSource.ToString());
             var brush = new ImageBrush();
             int num = 0;
             foreach (var item in str1)
             {
                 FileInfo f1 = new FileInfo(item);
                 if (f1.Name==f.Name){a = num; break; }
                 num++;
             }
             brush.ImageSource = new BitmapImage(new Uri(@$"{str1[a]}", UriKind.Relative));
                 DP.Background = brush;  
        }
            
        
        public Window1()
        {
            InitializeComponent();
            numcal();
        }
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private void Times()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Func(++a);
        }
        private void BTN_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Tag)
                {
                    case "1":
                        Func(++a);
                        break;
                    case "2":
                        Func(--a);
                        break;
                    case "3":
                        Times();
                        break;
                    case "4":
                        dispatcherTimer.Stop();
                        break;
                    default:
                        break;
                }

            }
         
            
        }
    }
}
