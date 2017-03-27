using Net.Sz.Framework.ExcelTools.CreateCode;
using Net.Sz.Framework.ExcelTools.CreateCode.excel;
using Net.Sz.Framework.ExcelTools.CreateCode.excel.Csharp;
using Net.Sz.Framework.ExcelTools.CreateCode.excel.java;
using Net.Sz.Framework.ExcelTools.CreateCode.excel.mysql;
using Net.Sz.Framework.ExcelTools.CreateCode.excel.text;
using Net.Sz.Framework.ExcelTools.CreateCode.excel.xml;
using Net.Sz.Framework.ExcelTools.CreateCode.protobuf;
using Net.Sz.Framework.ExcelTools.excelindb;
using Net.Sz.Framework.ExcelTools.ini;
using Net.Sz.Framework.AStart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Net.Sz.Framework.ExcelTools
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.Load += FormMain_Load;
        }



        void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                this.lbFiles.Tag = new List<ItemFileInfo>();
                this.labtips.Text = "Excel 生成实体类以及数据源 \r\n\r\nXml 分析生成程序代码，\r\n\r\nPS: Excel“.xls” 建议使用 Wps！";
                this.tbmsgshow.Text = string.Empty;
                if (!File.Exists("dbconfig.xml"))
                {
                    System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(DBConfigs));
                    using (FileStream fs = new FileStream("dbconfig.xml", FileMode.CreateNew, FileAccess.Write))
                    {
                        var dbcs = new DBConfigs();
                        dbcs.NamespaceStr = "net.sz.game.po";
                        dbcs.SavePath = CreateBase.BasePath;
                        dbcs.SaveJPAPath = CreateBase.JavaJpaCodePathData;
                        dbcs.SaveJavaMessagePath = CreateBase.protobufjavaMessage;
                        dbcs.SaveCsharpMessagePath = CreateBase.protobufnetMessage;
                        dbcs.IsNullEmpty = false;
                        dbcs.Configs.Add(new DBConfig()
                        {
                            DBBase = "test",
                            DBPath = "192.168.0.188",
                            DBPart = 3306,
                            DBUser = "root",
                            DBPwd = "fuckdaohaode1314"
                        });
                        dbcs.Configs.Add(new DBConfig()
                        {
                            DBBase = "local_gamesr_data",
                            DBPath = "192.168.0.188",
                            DBPart = 3306,
                            DBUser = "root",
                            DBPwd = "fuckdaohaode1314"
                        });
                        xml.Serialize(fs, dbcs);
                    }
                }
                {
                    System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(DBConfigs));
                    DBConfigs dbcs = null;
                    using (FileStream fs = new FileStream("dbconfig.xml", FileMode.Open, FileAccess.ReadWrite))
                    {
                        dbcs = (DBConfigs)xml.Deserialize(fs);
                        if (!string.IsNullOrWhiteSpace(dbcs.SavePath)) CreateBase.SetPath(dbcs.SavePath.Trim());
                        else dbcs.SavePath = CreateBase.BasePath;
                        if (!string.IsNullOrWhiteSpace(dbcs.SaveCsharpMessagePath)) CreateBase.protobufnetMessage = (dbcs.SaveCsharpMessagePath.Trim());
                        else dbcs.SaveCsharpMessagePath = CreateBase.protobufnetMessage;
                        if (!string.IsNullOrWhiteSpace(dbcs.SaveJavaMessagePath)) CreateBase.protobufjavaMessage = (dbcs.SaveJavaMessagePath.Trim());
                        else dbcs.SaveJavaMessagePath = CreateBase.protobufjavaMessage;
                        if (!string.IsNullOrWhiteSpace(dbcs.SaveJPAPath)) CreateBase.JavaJpaCodePathData = (dbcs.SaveJPAPath.Trim());
                        else dbcs.SaveJPAPath = CreateBase.JavaJpaCodePathData;

                        this.cb_isnull.Checked = dbcs.IsNullEmpty;

                        this.combdbcons.DataSource = dbcs.Configs;
                        this.combdbcons.SelectedIndex = 0;
                        DBConfig dbc = dbcs.Configs[0];
                        this.tbdbpack.Text = dbc.DBBase.Trim();
                        this.tbcodepack.Text = dbcs.NamespaceStr.Trim();

                    }
                    using (FileStream fs1 = new FileStream("dbconfig.xml", FileMode.Create, FileAccess.Write))
                    {
                        xml.Serialize(fs1, dbcs);
                    }
                }

                MySqlDB.Instance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库连接字符串错误" + ex);
                Application.Exit();
            }
        }

        #region 基本处理
        /// <summary>
        /// 清空 list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.tbmsgshow.Text = string.Empty;
            msgid = 0;
            this.lbFiles.Items.Clear();
            this.lbFiles.Tag = new List<ItemFileInfo>();
        }

        /// <summary>
        /// 移除其中一个选定的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.lbFiles.Items.Count > 0)
            {
                int index = this.lbFiles.SelectedIndex;
                if (index < 0 || this.lbFiles.Items.Count < index)
                {
                    index = 0;
                }
                this.lbFiles.Items.RemoveAt(index);
                ((List<ItemFileInfo>)this.lbFiles.Tag).RemoveAt(index);
            }
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnaddfile_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AddFileViwe(ofdFile.FileNames);
            }
        }

        /// <summary>
        /// 递归查找文件
        /// </summary>
        /// <param name="path"></param>
        void FindFiles(string path)
        {
            AddFileViwe(Directory.GetFiles(path));
            var paths = Directory.GetDirectories(path);
            foreach (var item in paths)
            {
                FindFiles(item);
            }
        }

        void AddFileViwe(string[] fileNames)
        {
            var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
            foreach (var itemFile in fileNames)
            {
                string extname = System.IO.Path.GetExtension(itemFile);
                string name = System.IO.Path.GetFileName(itemFile);

                if (extname.ToLower().Equals(".xls") || extname.ToLower().Equals(".proto") || extname.ToLower().Equals(".xml"))
                {
                    var tifi = temps.FirstOrDefault(l => l.Name == name);
                    if (tifi == null)
                    {
                        var ifi = new ItemFileInfo(itemFile);
                        (this.lbFiles.Tag as List<ItemFileInfo>).Add(ifi);
                        this.lbFiles.Items.Add(ifi);
                    }
                    else
                    {
                        Show("无需添加的文件：" + name);
                    }
                }
            }
            if (this.lbFiles.Items.Count > 0)
            {
                this.lbFiles.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 整理文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btndic_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                CreateCode.CreateBase.DirectoryAction(new Action<string>((msg) => { ShowMsg(msg); }));
            }));
        }

        void ShowMsg(string msg = "处理完成")
        {
            Show(msg + " -> 目录：" + CreateBase.BasePath);
        }

        static ulong msgid = 0;

        void Show(string msg)
        {
            tbmsgshow.BeginInvoke(new Action(() => { tbmsgshow.Text = (++msgid) + " " + DateTime.Now.ToString("HH:mm:ss:ms") + " -> " + msg + "\r\n" + tbmsgshow.Text; }));
        }
        #endregion

        private void combdbcons_SelectedIndexChanged(object sender, EventArgs e)
        {
            DBConfig dbc = (DBConfig)(this.combdbcons.SelectedItem);
            this.tbdbpack.Text = dbc.DBBase;
            MySqlDB.Instance().SetMySqlConnection(dbc);
        }

        #region Excel 转化成数据源

        #region Xml
        private void butxmlall_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xls")).Select(l => l.Path).ToList();
                FileExcelDatas exceldatas;
                if (ReadExcelData.Instance().ReadExcelDataToCache(files, out exceldatas, new Action<string>((msg) => { Show(msg); })))
                {
                    //生成XML
                    CreateXml.Instance().ActionExcelData(exceldatas, "AC", this.cb_isnull.Checked, new Action<string>((msg) => { Show(msg); }));
                    //生成 Xml 对应 CSharp
                    CreateXMLCSharp.Instance().ActionExcelData(exceldatas, this.tbcodepack.Text, new Action<string>((msg) => { Show(msg); }));
                    Show("Excel 转 XML 文件处理完成");

                    try
                    {
                        System.Diagnostics.Process.Start(CreateBase.CSharpPath);
                    }
                    catch { }
                }
            }));
        }

        private void btnClientXml_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xls")).Select(l => l.Path).ToList();
                FileExcelDatas exceldatas;
                if (ReadExcelData.Instance().ReadExcelDataToCache(files, out exceldatas, new Action<string>((msg) => { Show(msg); }), false))
                {
                    //生成XML
                    CreateXml.Instance().ActionExcelData(exceldatas, "AC", this.cb_isnull.Checked, new Action<string>((msg) => { Show(msg); }));
                    //生成 Xml 对应 CSharp
                    CreateXMLCSharp.Instance().ActionExcelData(exceldatas, this.tbcodepack.Text, new Action<string>((msg) => { Show(msg); }));
                    Show("Excel 转 客户端专属 XML 文件处理完成");
                    try
                    {
                        System.Diagnostics.Process.Start(CreateBase.CSharpPath);
                    }
                    catch { }
                }
            }));
        }

        private void btnXmlCSharp_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
             {
                 var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                 List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xls")).Select(l => l.Path).ToList();
                 FileExcelDatas exceldatas;
                 if (ReadExcelData.Instance().ReadExcelDataToCache(files, out exceldatas, new Action<string>((msg) => { Show(msg); })))
                 {
                     //生成XML
                     CreateXml.Instance().ActionExcelData(exceldatas, "AC", this.cb_isnull.Checked, new Action<string>((msg) => { Show(msg); }));
                     //生成 Xml 对应 CSharp
                     CreateXMLCSharp.Instance().ActionExcelData(exceldatas, this.tbcodepack.Text, new Action<string>((msg) => { Show(msg); }));
                     Show("");
                     Show("Excel 转 XML 文件处理完成");
                     Show("");
                     try
                     {
                         System.Diagnostics.Process.Start(CreateBase.CSharpPath);
                     }
                     catch { }
                 }
             }));
        }

        #endregion

        #region Mysql
        private void btnCreateJpa_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xls")).Select(l => l.Path).ToList();
                FileExcelDatas exceldatas;
                if (ReadExcelData.Instance().ReadExcelDataToCache(files, out exceldatas, new Action<string>((msg) => { Show(msg); }), true))
                {
                    CreateJavaJpaCode.Instance().ActionExcelData(exceldatas, this.tbcodepack.Text, new Action<string>((msg) => { Show(msg); }));
                    Show("");
                    Show("Excel 转 服务器 Java Jpa 类 全部处理完成 文件目录：" + CreateBase.JavaJpaCodePathData);
                    try
                    {
                        System.Diagnostics.Process.Start(CreateBase.JavaJpaCodePathData);
                    }
                    catch { }
                }
            }));
        }

        private void btnCodeFirst_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xls")).Select(l => l.Path).ToList();
                FileExcelDatas exceldatas;
                if (ReadExcelData.Instance().ReadExcelDataToCache(files, out exceldatas, new Action<string>((msg) => { Show(msg); }), true))
                {
                    CreateCodeFirstCSharp.Instance().ActionExcelData(exceldatas, this.tbcodepack.Text, new Action<string>((msg) => { Show(msg); }));
                    Show("");
                    Show("Excel 转 服务器 CSharp Code First 类 全部处理完成 文件目录：" + CreateBase.JavaJpaCodePathData);
                    try
                    {
                        System.Diagnostics.Process.Start(CreateBase.JavaJpaCodePathData);
                    }
                    catch { }
                }
            }));
        }


        private void btnMysql_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xls")).Select(l => l.Path).ToList();
                FileExcelDatas exceldatas;
                if (ReadExcelData.Instance().ReadExcelDataToCache(files, out exceldatas, new Action<string>((msg) => { Show(msg); }), true))
                {
                    CreateMysql.Instance().ActionExcel(exceldatas, "AS", this.tbdbpack.Text, false, new Action<string>((msg) => { Show(msg); }));
                    Show("");
                    Show("Excel 转 Mysql 全部处理完成 文件目录：" + CreateBase.MySqlPath);
                    try
                    {
                        System.Diagnostics.Process.Start(CreateBase.MySqlPath);
                    }
                    catch { }
                }
            }));
        }

        private void btnMySqlInDB_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
             {
                 var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                 List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xls")).Select(l => l.Path).ToList();
                 FileExcelDatas exceldatas;
                 if (ReadExcelData.Instance().ReadExcelDataToCache(files, out exceldatas, new Action<string>((msg) => { Show(msg); }), true))
                 {
                     CreateMysql.Instance().ActionExcel(exceldatas, "AS", this.tbdbpack.Text, true, new Action<string>((msg) => { Show(msg); }));
                     Show("");
                     Show("Excel 转 Mysql 并且插入数据库 全部处理完成 ");
                 }
             }));
        }
        #endregion

        #region text
        private void btntxt_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
             {

                 var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                 List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xls")).Select(l => l.Path).ToList();
                 FileExcelDatas exceldatas;
                 if (ReadExcelData.Instance().ReadExcelDataToCache(files, out exceldatas, new Action<string>((msg) => { Show(msg); })))
                 {
                     CreateText.Instance().ActionExcelData(exceldatas, "|", new Action<string>((msg) => { Show(msg); }));
                     Show("Excel 转 Text 全部处理完成 文件目录：" + CreateBase.TextPath);
                     try
                     {
                         System.Diagnostics.Process.Start(CreateBase.TextPath);
                     }
                     catch { }
                 }
             }));
        }
        #endregion



        private void btnALL_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {

                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    FindFiles(fbd.SelectedPath);
                }
            }));
        }
        #endregion

        #region Protobuf 文件处理
        private void btnprotobufAll_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
             {
                 Show("开始处理Protobuf文件");
                 var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                 if (temps.Count > 0)
                 {
                     CreateCSharpProtobuf.CreateCode(temps, new Action<string>((msg) => { Show(msg); }));
                     CreateJavaProtobuf.CreateCode(temps, new Action<string>((msg) => { Show(msg); }));
                 }
             }));
        }

        private void btnprotobufJava_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                Show("开始处理Protobuf文件");
                var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                if (temps.Count > 0)
                {
                    CreateJavaProtobuf.CreateCode(temps, new Action<string>((msg) => { Show(msg); }));
                }
            }));
        }

        private void btnprotobufCSharp_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                Show("开始处理Protobuf文件");
                var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                if (temps.Count > 0)
                {
                    CreateCSharpProtobuf.CreateCode(temps, new Action<string>((msg) => { Show(msg); }));
                }
            }));
        }
        #endregion

        void action(Action action)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnClareContext_Click(object sender, EventArgs e)
        {
            tbmsgshow.Invoke(new Action(() => { tbmsgshow.Text = string.Empty; }));
        }

        private void btntime_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = new DateTime(
                        int.Parse(this.tbtimey.Text),
                        int.Parse(this.tbtimem.Text),
                        int.Parse(this.tbtimed.Text),
                        int.Parse(this.tbtimeh.Text),
                        0,
                        0);
                Show("java时间：" + dt.ToString("yyyy年MM月dd小时HH -> ") + Net.Sz.Framework.Utils.TimeUtil.CurrentTimeMillis_Java(dt).ToString());
                Show("CSharp时间：" + dt.ToString("yyyy年MM月dd小时HH -> ") + Net.Sz.Framework.Utils.TimeUtil.CurrentTimeMillis(dt).ToString());
            }
            catch (Exception)
            {
                Show("时间个数转化失败");
            }
        }

        private void btnyzm_Click(object sender, EventArgs e)
        {
            var yzms = this.tbyzm.Text.Split(',');
            int yzmcount = int.Parse(this.tbcount.Text);
            int yzmlen = int.Parse(this.tblen.Text);
            string path = CreateBase.BasePath + "111.txt";
            Random ran = new Random();
            using (System.IO.StreamWriter sw = new StreamWriter(path, false, UTF8Encoding.Default))
            {
                for (int i = 0; i < yzmcount; i++)
                {
                    for (int j = 0; j < yzmlen; j++)
                    {
                        sw.Write(yzms[ran.Next(0, yzms.Length - 1)]);
                    }
                    sw.WriteLine();
                }
            }
            try
            {
                System.Diagnostics.Process.Start(path);
            }
            catch { }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btn_block_img_Click(object sender, EventArgs e)
        {
            action(new Action(() =>
            {
                if (!Directory.Exists(CreateBase.BasePath + "/" + "blockimg" + "/"))
                {
                    Directory.CreateDirectory(CreateBase.BasePath + "/" + "blockimg" + "/");
                }
                var temps = (this.lbFiles.Tag as List<ItemFileInfo>);
                List<string> files = temps.Where((l) => l.ExtensionName.Equals(".xml")).Select(l => l.Path).ToList();
                byte[,] R = null;
                foreach (var item in files)
                {
                    string filename = System.IO.Path.GetFileName(item);
                    Show("开始处理文件：" + filename);

                    try
                    {
                        MapBlockConfig block = new MapBlockConfig();
                        System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(block.GetType());
                        System.IO.FileStream fs = new System.IO.FileStream(item, System.IO.FileMode.Open);
                        block = (MapBlockConfig)xml.Deserialize(fs);
                        fs.Dispose();

                        Bitmap bitmap = new Bitmap(block.Walk.Rxlen * 10, block.Walk.Rzlen * 15);
                        Graphics g = Graphics.FromImage(bitmap);
                        g.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
                        SolidBrush brush = new SolidBrush(Color.White);
                        Font crFont = crFont = new Font("arial", 11, FontStyle.Regular);

                        for (int z = block.Walk.Rzlen - 1; z >= 0; z--)
                        {
                            for (int x = 0; x < block.Walk.Rxlen; x++)
                            {
                                char b = block.Walk.Blockinfo[block.Walk.Rxlen * z + x];
                                g.DrawString(b + "", crFont, brush, x * 10, (block.Walk.Rzlen - 1 - z) * 15);
                            }
                        }
                        g.Dispose();

                        bitmap.Save(CreateBase.BasePath + "/" + "blockimg" + "/" + filename + ".jpg");
                        bitmap.Dispose();

                        Show("文件：" + filename + " 阻挡图片已经生成");
                    }
                    catch (Exception)
                    {
                        Show("文件：" + filename + " 生成图片失败，请查看阻挡文件是否正常！");
                    }
                }
                try
                {
                    System.Diagnostics.Process.Start(CreateBase.BasePath + "/" + "blockimg" + "/");
                }
                catch { }
            }));
        }

        static float ConstArea = 0.5f;

        static public int seat_X(float _x)
        {
            //float tmpx = _x - Move.ConstArea / 2;
            int __x = (int)(_x / ConstArea);
            //四舍五入取整数 二维数组的下标需要 -1
            return __x + (_x % ConstArea > 0 ? 1 : 0) - 1;
        }

        static public int seat_Y(float _y)
        {
            //float tmpy = _y - Move.ConstArea / 2;
            //四舍五入取整数 二维数组的下标需要 -1
            int __y = (int)(_y / ConstArea);
            return __y + (_y % ConstArea > 0 ? 1 : 0) - 1;
        }

        private void btnblock_Click(object sender, EventArgs e)
        {
            try
            {
                object obj = this.lbFiles.SelectedItem;
                if (obj is ItemFileInfo)
                {
                    ItemFileInfo item = (ItemFileInfo)obj;

                    string filename = System.IO.Path.GetFileName(item.Path);

                    MapBlockConfig block = new MapBlockConfig();
                    System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(block.GetType());
                    System.IO.FileStream fs = new System.IO.FileStream(item.Path, System.IO.FileMode.Open);
                    block = (MapBlockConfig)xml.Deserialize(fs);
                    fs.Dispose();

                    float x = 0;

                    float z = 0;

                    float.TryParse(this.tbblockx.Text, out x);

                    float.TryParse(this.tbblocky.Text, out z);

                    int ix = seat_X(x);

                    int iz = seat_Y(z);

                    Show("X：" + this.tbblockx.Text + " Z：" + this.tbblocky.Text + " Ix：" + ix + " Ix：" + iz + " 阻挡结果：" + block.Walk.Blockinfo[block.Walk.Rxlen * iz + ix]);

                    Bitmap bitmap = new Bitmap(block.Walk.Rxlen * 10, block.Walk.Rzlen * 15);
                    Graphics g = Graphics.FromImage(bitmap);
                    g.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
                    SolidBrush brush = new SolidBrush(Color.White);
                    SolidBrush brushRead = new SolidBrush(Color.Red);
                    Font crFont = crFont = new Font("arial", 11, FontStyle.Regular);

                    for (int z1 = block.Walk.Rzlen - 1; z1 >= 0; z1--)
                    {
                        for (int x1 = 0; x1 < block.Walk.Rxlen; x1++)
                        {

                            char b = block.Walk.Blockinfo[block.Walk.Rxlen * z1 + x1];
                            if (z1 == iz && x1 == ix)
                            {
                                g.DrawString(b + "", crFont, brushRead, x1 * 10, (block.Walk.Rzlen - 1 - z1) * 15);
                            }
                            else
                            {
                                g.DrawString(b + "", crFont, brush, x1 * 10, (block.Walk.Rzlen - 1 - z1) * 15);
                            }
                        }
                    }
                    g.Dispose();

                    bitmap.Save(CreateBase.BasePath + "/" + "blockimg" + "/" + filename + ".jpg");
                    bitmap.Dispose();

                    Show("文件：" + filename + " 阻挡图片已经生成");

                    System.Diagnostics.Process.Start(CreateBase.BasePath + "/" + "blockimg" + "/");
                }
            }
            catch (Exception ex)
            {
                Show("阻挡转化错误" + ex.ToString());
            }
        }

        /// <summary>
        /// <para>判断点是否在多边形的范围内</para>
        /// <para>返回值：值为1表示点在多边形范围内；</para>
        /// <para>值为0表示点在多边形边上；</para>
        /// <para>值为-1表示点不在多边形范围内。</para>
        /// </summary>
        /// <param name="point">点坐标，长度为2</param>
        /// <param name="polyline">多边形节点坐标，长度为2*n，其中n应大于或等于3，即至少为三角形</param>
        /// <returns>
        /// <para>返回值：值为1表示点在多边形范围内；</para>
        /// <para>值为0表示点在多边形边上；</para>
        /// <para>值为-1表示点不在多边形范围内。</para>
        /// </returns>
        public static int PolygonIsContainPoint(double[] point, double[] polyline)
        {
            int result = -1, count = 0, pointcount = 0, tempI;
            double maxx = 0, minx = 0, maxy = 0, miny = 0;
            if (polyline != null)
            {
                int i;
                pointcount = polyline.Length / 2;
                maxx = minx = polyline[0];
                maxy = miny = polyline[1];
                for (i = 0; i < pointcount; i++)
                {
                    tempI = i + i;
                    if (maxx < polyline[tempI])
                        maxx = polyline[tempI];
                    if (minx > polyline[tempI])
                        minx = polyline[tempI];
                    if (maxy < polyline[tempI + 1])
                        maxy = polyline[tempI + 1];
                    if (miny > polyline[tempI + 1])
                        miny = polyline[tempI + 1];
                }
            }
            if (point != null)
            {
                //首先判断是否在面的外框范围内
                if (point[0] < minx || point[0] > maxx
                || point[1] < miny || point[1] > maxy)
                {
                    return result;
                }
                else
                {
                    int i, j;
                    j = pointcount - 1;
                    double[] point1, point2;
                    double tempValue;
                    for (i = 0; i < pointcount; i++)
                    {
                        point1 = new double[2];
                        point2 = new double[2];
                        tempI = i + i;
                        point1[0] = polyline[tempI];
                        point1[1] = polyline[tempI + 1];
                        tempI = j + j;
                        point2[0] = polyline[tempI];
                        point2[1] = polyline[tempI + 1];
                        if ((point1[0] < point[0] && point2[0] >= point[0])
                        || (point2[0] < point[0] && point1[0] >= point[0]))
                        {
                            tempValue = point1[1] + (point[0] - point1[0]) / (point2[0] - point1[0]) * (point2[1] - point1[1]);
                            if (tempValue < point[1])
                            {
                                count++;
                            }
                            else if (tempValue == point[1])
                            {
                                count = -1;
                                break;
                            }
                        }
                        j = i;
                    }
                }
            }
            if (count == -1)
            {
                result = 0;//点在线段上
            }
            else
            {
                tempI = count % 2;
                if (tempI == 0)//为偶数
                {
                    result = -1;
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }
    }
}
