using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YSPhoton
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.AllowDrop = false;
            ObservableCollection<CMB_Item> CMBObj = new ObservableCollection<CMB_Item>();
            CMBObj.Add(new CMB_Item() { Format = ImageFormat.Jpeg, Name = "将所有项转化成jpg" });
            CMBObj.Add(new CMB_Item() { Format = ImageFormat.Gif, Name = "将所有项转化成gif" });
            CMBObj.Add(new CMB_Item() { Format = ImageFormat.Png, Name = "将所有项转化成png" });
            CMBObj.Add(new CMB_Item() { Format = ImageFormat.Icon, Name = "将所有项转化成icon" });
            this.cmbformat.ItemsSource = CMBObj;
            this.cmbformat.SelectedIndex = 0;
        }
        System.Windows.Forms.Design.FolderNameEditor fDialog = new System.Windows.Forms.Design.FolderNameEditor();
        public ObservableCollection<LV_Item> ObservableObj = null;
        private void Btn_InputFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Title = "选取文件";
            openFile.Multiselect = true;
            openFile.Filter = "所有文件|*.*|所有图片|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.icon;*.ico|jpg格式|*.jpg;*.jpeg|png格式|*.png|gif格式|*.gif|ico格式|*.icon;*.ico";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] fileNames = openFile.FileNames;

                ObservableObj = new ObservableCollection<LV_Item>();
                foreach (var item in openFile.FileNames)
                {
                    ObservableObj.Add(new LV_Item() { Name = item });
                }
                this.LV_box.DataContext = ObservableObj;
            }
        }

        private void Btn_ResetFileMd5_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                foreach (var item in ObservableObj)
                {
                    this.LV_box.Dispatcher.Invoke(() =>
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(item.Name, true))
                        {
                            sw.Write((byte)0);
                        }
                        item.Status = "修改完成";
                        this.LV_box.Items.Refresh();
                        System.Threading.Thread.Sleep(1);
                    });
                }
                System.Windows.MessageBox.Show("完成");
            });
            thread.IsBackground = true;
            thread.Start();
        }


        public string ImageFormatter(string sourcePath)
        {
            string fileName = System.IO.Path.GetFileName(sourcePath);
            string directoryName = System.IO.Path.GetDirectoryName(sourcePath);

            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(sourcePath))
            {
                CMB_Item cmbitem = (CMB_Item)this.cmbformat.SelectedItem;
                string filename = directoryName + "/" + fileName;

                int ConstWH = 65000;

                int height = bitmap.Height;

                int i = 0;
                do
                {
                    int tmph = 0;
                    if (height > ConstWH)
                    {
                        tmph = ConstWH;
                        height -= ConstWH;
                    }
                    else
                    {
                        tmph = height;
                        height = 0;
                    }
                    using (FileStream fs = new FileStream(filename + (i > 0 ? "_" + i + "" : "") + "." + cmbitem.Format.ToString().ToLower(), FileMode.OpenOrCreate))
                    {
                        using (System.Drawing.Bitmap newbitmap = new System.Drawing.Bitmap(bitmap.Width, tmph))
                        {
                            //新建一个画板  
                            using (Graphics g = System.Drawing.Graphics.FromImage(newbitmap))
                            {
                                //设置高质量插值法  
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;

                                //设置高质量,低速度呈现平滑程度  
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                                //清空画布并以白色背景色填充  
                                g.Clear(Color.Transparent);
                                System.Drawing.Rectangle rce = new System.Drawing.Rectangle(0, i * ConstWH, bitmap.Width, tmph);
                                //在指定位置并且按指定大小绘制原图片的指定部分  
                                g.DrawImage(bitmap, 0, 0, rce, GraphicsUnit.Pixel);
                            }
                            using (MemoryStream ms1 = new MemoryStream())
                            {
                                newbitmap.Save(ms1, cmbitem.Format);
                                byte[] buff = ms1.ToArray();
                                fs.Write(buff, 0, buff.Length);
                            }
                        }
                    }
                    i++;
                } while (height > 0);

                return fileName + "." + cmbitem.Format.ToString().ToLower();
            }
        }

        private void Btn_BMP_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                foreach (var item in ObservableObj)
                {
                    this.LV_box.Dispatcher.Invoke(() =>
                    {
                        FileInfo fi = new FileInfo(item.Name);
                        item.Size = fi.Length;
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(item.Name);
                        string fileExtension = System.IO.Path.GetExtension(item.Name);
                        string directoryName = System.IO.Path.GetDirectoryName(item.Name);

                        try
                        {
                            item.NewExtension = ImageFormatter(item.Name);
                            fi = new FileInfo(item.NewExtension);
                            item.NewSize = fi.Length;
                            item.Status = "修改完成";
                        }
                        catch (Exception ex)
                        {
                            item.Status = ex.Message;
                        }

                        this.LV_box.Items.Refresh();
                        System.Threading.Thread.Sleep(1);
                    });
                }
                System.Windows.MessageBox.Show("完成");
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void Btn_BMP_1_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class CMB_Item
    {
        public string Name { get; set; }
        public System.Drawing.Imaging.ImageFormat Format { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class LV_Item
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public long NewSize { get; set; }
        public string NewExtension { get; set; }

        public string Status { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
