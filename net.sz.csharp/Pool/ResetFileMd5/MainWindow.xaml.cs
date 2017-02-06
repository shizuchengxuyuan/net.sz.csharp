using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ResetFileMd5
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
        }

        public ObservableCollection<LV_Item> ObservableObj = null;

        private void Btn_InputFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();
            openFile.Title = "选取文件";
            openFile.Multiselect = true;
            openFile.Filter = "所有文件(*.*)|*.*";
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
            
        }

        /// <summary>
        /// 计算文件的MD5校验
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                System.IO.FileStream file = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

    }


    public class LV_Item
    {
        public string Name { get; set; }
        public string MD5 { get; set; }
        public string NewMd5 { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
