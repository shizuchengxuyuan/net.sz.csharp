namespace Net.Sz.Framework.ExcelTools
{
   partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbdbpack = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labtips = new System.Windows.Forms.Label();
            this.combdbcons = new System.Windows.Forms.ComboBox();
            this.btndelfile = new System.Windows.Forms.Button();
            this.btnALL = new System.Windows.Forms.Button();
            this.btnXmlJava = new System.Windows.Forms.Button();
            this.btnXmlCSharp = new System.Windows.Forms.Button();
            this.btnMySqlInDB = new System.Windows.Forms.Button();
            this.btnMysql = new System.Windows.Forms.Button();
            this.btnaddfile = new System.Windows.Forms.Button();
            this.btndic = new System.Windows.Forms.Button();
            this.btnprotobufAll = new System.Windows.Forms.Button();
            this.btnprotobufJava = new System.Windows.Forms.Button();
            this.btnprotobufCSharp = new System.Windows.Forms.Button();
            this.tbcodepack = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.butxmlall = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_isnull = new System.Windows.Forms.CheckBox();
            this.btn_block_img = new System.Windows.Forms.Button();
            this.btnClientXml = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCreateJpa = new System.Windows.Forms.Button();
            this.btnCodeFirst = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbmsgshow = new System.Windows.Forms.TextBox();
            this.btnClareContext = new System.Windows.Forms.Button();
            this.tbyzm = new System.Windows.Forms.TextBox();
            this.btnyzm = new System.Windows.Forms.Button();
            this.btntime = new System.Windows.Forms.Button();
            this.tbtimey = new System.Windows.Forms.TextBox();
            this.tbtimem = new System.Windows.Forms.TextBox();
            this.tbtimed = new System.Windows.Forms.TextBox();
            this.tbtimeh = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tblen = new System.Windows.Forms.TextBox();
            this.tbcount = new System.Windows.Forms.TextBox();
            this.openFilexml = new System.Windows.Forms.OpenFileDialog();
            this.tbblockx = new System.Windows.Forms.TextBox();
            this.tbblocky = new System.Windows.Forms.TextBox();
            this.btnblock = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdFile
            // 
            this.ofdFile.Filter = "工具可读文件|*.xls;*.proto;*.xml";
            this.ofdFile.Multiselect = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbFiles);
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(249, 389);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "处理文件";
            // 
            // lbFiles
            // 
            this.lbFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.ItemHeight = 12;
            this.lbFiles.Location = new System.Drawing.Point(3, 17);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(243, 369);
            this.lbFiles.TabIndex = 5;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(103, 451);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(64, 40);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbdbpack
            // 
            this.tbdbpack.Location = new System.Drawing.Point(64, 21);
            this.tbdbpack.Name = "tbdbpack";
            this.tbdbpack.Size = new System.Drawing.Size(355, 21);
            this.tbdbpack.TabIndex = 11;
            this.tbdbpack.Text = "local_gamesr_data";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "数据库:";
            // 
            // labtips
            // 
            this.labtips.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labtips.Location = new System.Drawing.Point(264, 9);
            this.labtips.Name = "labtips";
            this.labtips.Size = new System.Drawing.Size(419, 67);
            this.labtips.TabIndex = 16;
            // 
            // combdbcons
            // 
            this.combdbcons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combdbcons.FormattingEnabled = true;
            this.combdbcons.Location = new System.Drawing.Point(11, 48);
            this.combdbcons.Name = "combdbcons";
            this.combdbcons.Size = new System.Drawing.Size(408, 20);
            this.combdbcons.TabIndex = 19;
            this.combdbcons.SelectedIndexChanged += new System.EventHandler(this.combdbcons_SelectedIndexChanged);
            // 
            // btndelfile
            // 
            this.btndelfile.Location = new System.Drawing.Point(103, 401);
            this.btndelfile.Name = "btndelfile";
            this.btndelfile.Size = new System.Drawing.Size(64, 44);
            this.btndelfile.TabIndex = 20;
            this.btndelfile.Text = "删除";
            this.btndelfile.UseVisualStyleBackColor = true;
            this.btndelfile.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnALL
            // 
            this.btnALL.Location = new System.Drawing.Point(172, 401);
            this.btnALL.Name = "btnALL";
            this.btnALL.Size = new System.Drawing.Size(68, 44);
            this.btnALL.TabIndex = 0;
            this.btnALL.Text = "文件夹";
            this.btnALL.UseVisualStyleBackColor = true;
            this.btnALL.Click += new System.EventHandler(this.btnALL_Click);
            // 
            // btnXmlJava
            // 
            this.btnXmlJava.Location = new System.Drawing.Point(29, 18);
            this.btnXmlJava.Name = "btnXmlJava";
            this.btnXmlJava.Size = new System.Drawing.Size(77, 24);
            this.btnXmlJava.TabIndex = 2;
            this.btnXmlJava.Text = "XmlJava";
            this.btnXmlJava.UseVisualStyleBackColor = true;
            // 
            // btnXmlCSharp
            // 
            this.btnXmlCSharp.Location = new System.Drawing.Point(29, 48);
            this.btnXmlCSharp.Name = "btnXmlCSharp";
            this.btnXmlCSharp.Size = new System.Drawing.Size(77, 24);
            this.btnXmlCSharp.TabIndex = 1;
            this.btnXmlCSharp.Text = "XmlCSharp";
            this.btnXmlCSharp.UseVisualStyleBackColor = true;
            this.btnXmlCSharp.Click += new System.EventHandler(this.btnXmlCSharp_Click);
            // 
            // btnMySqlInDB
            // 
            this.btnMySqlInDB.BackColor = System.Drawing.Color.Red;
            this.btnMySqlInDB.Location = new System.Drawing.Point(12, 75);
            this.btnMySqlInDB.Name = "btnMySqlInDB";
            this.btnMySqlInDB.Size = new System.Drawing.Size(84, 21);
            this.btnMySqlInDB.TabIndex = 0;
            this.btnMySqlInDB.Text = "导入Mysql";
            this.btnMySqlInDB.UseVisualStyleBackColor = false;
            this.btnMySqlInDB.Click += new System.EventHandler(this.btnMySqlInDB_Click);
            // 
            // btnMysql
            // 
            this.btnMysql.Location = new System.Drawing.Point(101, 76);
            this.btnMysql.Name = "btnMysql";
            this.btnMysql.Size = new System.Drawing.Size(73, 21);
            this.btnMysql.TabIndex = 0;
            this.btnMysql.Text = "Mysql文件";
            this.btnMysql.UseVisualStyleBackColor = true;
            this.btnMysql.Click += new System.EventHandler(this.btnMysql_Click);
            // 
            // btnaddfile
            // 
            this.btnaddfile.Location = new System.Drawing.Point(10, 401);
            this.btnaddfile.Name = "btnaddfile";
            this.btnaddfile.Size = new System.Drawing.Size(86, 44);
            this.btnaddfile.TabIndex = 23;
            this.btnaddfile.Text = "文件";
            this.btnaddfile.UseVisualStyleBackColor = true;
            this.btnaddfile.Click += new System.EventHandler(this.btnaddfile_Click);
            // 
            // btndic
            // 
            this.btndic.Location = new System.Drawing.Point(10, 451);
            this.btndic.Name = "btndic";
            this.btndic.Size = new System.Drawing.Size(86, 40);
            this.btndic.TabIndex = 23;
            this.btndic.Text = "整理文件夹";
            this.btndic.UseVisualStyleBackColor = true;
            this.btndic.Click += new System.EventHandler(this.btndic_Click);
            // 
            // btnprotobufAll
            // 
            this.btnprotobufAll.Location = new System.Drawing.Point(262, 20);
            this.btnprotobufAll.Name = "btnprotobufAll";
            this.btnprotobufAll.Size = new System.Drawing.Size(75, 23);
            this.btnprotobufAll.TabIndex = 2;
            this.btnprotobufAll.Text = "All";
            this.btnprotobufAll.UseVisualStyleBackColor = true;
            this.btnprotobufAll.Click += new System.EventHandler(this.btnprotobufAll_Click);
            // 
            // btnprotobufJava
            // 
            this.btnprotobufJava.Location = new System.Drawing.Point(148, 20);
            this.btnprotobufJava.Name = "btnprotobufJava";
            this.btnprotobufJava.Size = new System.Drawing.Size(75, 23);
            this.btnprotobufJava.TabIndex = 1;
            this.btnprotobufJava.Text = "Java";
            this.btnprotobufJava.UseVisualStyleBackColor = true;
            this.btnprotobufJava.Click += new System.EventHandler(this.btnprotobufJava_Click);
            // 
            // btnprotobufCSharp
            // 
            this.btnprotobufCSharp.Location = new System.Drawing.Point(36, 20);
            this.btnprotobufCSharp.Name = "btnprotobufCSharp";
            this.btnprotobufCSharp.Size = new System.Drawing.Size(75, 23);
            this.btnprotobufCSharp.TabIndex = 0;
            this.btnprotobufCSharp.Text = "CSharp";
            this.btnprotobufCSharp.UseVisualStyleBackColor = true;
            this.btnprotobufCSharp.Click += new System.EventHandler(this.btnprotobufCSharp_Click);
            // 
            // tbcodepack
            // 
            this.tbcodepack.Location = new System.Drawing.Point(327, 263);
            this.tbcodepack.Name = "tbcodepack";
            this.tbcodepack.Size = new System.Drawing.Size(356, 21);
            this.tbcodepack.TabIndex = 11;
            this.tbcodepack.Text = "Net.Sz.Game.MMOGame.GameModel.PoData";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(260, 267);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "名子空间:";
            // 
            // butxmlall
            // 
            this.butxmlall.Location = new System.Drawing.Point(317, 49);
            this.butxmlall.Name = "butxmlall";
            this.butxmlall.Size = new System.Drawing.Size(90, 25);
            this.butxmlall.TabIndex = 3;
            this.butxmlall.Text = "全部生成";
            this.butxmlall.UseVisualStyleBackColor = true;
            this.butxmlall.Click += new System.EventHandler(this.butxmlall_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnblock);
            this.groupBox2.Controls.Add(this.tbblocky);
            this.groupBox2.Controls.Add(this.tbblockx);
            this.groupBox2.Controls.Add(this.cb_isnull);
            this.groupBox2.Controls.Add(this.btn_block_img);
            this.groupBox2.Controls.Add(this.btnClientXml);
            this.groupBox2.Controls.Add(this.butxmlall);
            this.groupBox2.Controls.Add(this.btnXmlJava);
            this.groupBox2.Controls.Add(this.btnXmlCSharp);
            this.groupBox2.Location = new System.Drawing.Point(264, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(419, 111);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "生成Xml";
            // 
            // cb_isnull
            // 
            this.cb_isnull.AutoSize = true;
            this.cb_isnull.Location = new System.Drawing.Point(154, 20);
            this.cb_isnull.Name = "cb_isnull";
            this.cb_isnull.Size = new System.Drawing.Size(138, 16);
            this.cb_isnull.TabIndex = 30;
            this.cb_isnull.Text = "过滤空白字符和0字符";
            this.cb_isnull.UseVisualStyleBackColor = true;
            // 
            // btn_block_img
            // 
            this.btn_block_img.Location = new System.Drawing.Point(157, 46);
            this.btn_block_img.Name = "btn_block_img";
            this.btn_block_img.Size = new System.Drawing.Size(120, 23);
            this.btn_block_img.TabIndex = 29;
            this.btn_block_img.Text = "阻挡图片生成器";
            this.btn_block_img.UseVisualStyleBackColor = true;
            this.btn_block_img.Click += new System.EventHandler(this.btn_block_img_Click);
            // 
            // btnClientXml
            // 
            this.btnClientXml.BackColor = System.Drawing.Color.Red;
            this.btnClientXml.Location = new System.Drawing.Point(317, 18);
            this.btnClientXml.Name = "btnClientXml";
            this.btnClientXml.Size = new System.Drawing.Size(90, 23);
            this.btnClientXml.TabIndex = 28;
            this.btnClientXml.Text = "C#客户端XML";
            this.btnClientXml.UseVisualStyleBackColor = false;
            this.btnClientXml.Click += new System.EventHandler(this.btnClientXml_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnCreateJpa);
            this.groupBox3.Controls.Add(this.combdbcons);
            this.groupBox3.Controls.Add(this.tbdbpack);
            this.groupBox3.Controls.Add(this.btnCodeFirst);
            this.groupBox3.Controls.Add(this.btnMysql);
            this.groupBox3.Controls.Add(this.btnMySqlInDB);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(264, 292);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(425, 100);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "生成Mysql";
            // 
            // btnCreateJpa
            // 
            this.btnCreateJpa.Location = new System.Drawing.Point(260, 75);
            this.btnCreateJpa.Name = "btnCreateJpa";
            this.btnCreateJpa.Size = new System.Drawing.Size(75, 21);
            this.btnCreateJpa.TabIndex = 20;
            this.btnCreateJpa.Text = "JPA";
            this.btnCreateJpa.UseVisualStyleBackColor = true;
            this.btnCreateJpa.Click += new System.EventHandler(this.btnCreateJpa_Click);
            // 
            // btnCodeFirst
            // 
            this.btnCodeFirst.Location = new System.Drawing.Point(182, 75);
            this.btnCodeFirst.Name = "btnCodeFirst";
            this.btnCodeFirst.Size = new System.Drawing.Size(73, 21);
            this.btnCodeFirst.TabIndex = 0;
            this.btnCodeFirst.Text = "codefirst";
            this.btnCodeFirst.UseVisualStyleBackColor = true;
            this.btnCodeFirst.Click += new System.EventHandler(this.btnCodeFirst_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnprotobufAll);
            this.groupBox4.Controls.Add(this.btnprotobufCSharp);
            this.groupBox4.Controls.Add(this.btnprotobufJava);
            this.groupBox4.Location = new System.Drawing.Point(263, 81);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(420, 53);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "protobuf";
            // 
            // tbmsgshow
            // 
            this.tbmsgshow.AcceptsReturn = true;
            this.tbmsgshow.AcceptsTab = true;
            this.tbmsgshow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbmsgshow.Location = new System.Drawing.Point(0, 497);
            this.tbmsgshow.Multiline = true;
            this.tbmsgshow.Name = "tbmsgshow";
            this.tbmsgshow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbmsgshow.Size = new System.Drawing.Size(701, 169);
            this.tbmsgshow.TabIndex = 32;
            // 
            // btnClareContext
            // 
            this.btnClareContext.Location = new System.Drawing.Point(171, 451);
            this.btnClareContext.Name = "btnClareContext";
            this.btnClareContext.Size = new System.Drawing.Size(69, 41);
            this.btnClareContext.TabIndex = 0;
            this.btnClareContext.Text = "清除";
            this.btnClareContext.UseVisualStyleBackColor = true;
            this.btnClareContext.Click += new System.EventHandler(this.btnClareContext_Click);
            // 
            // tbyzm
            // 
            this.tbyzm.Location = new System.Drawing.Point(264, 406);
            this.tbyzm.Multiline = true;
            this.tbyzm.Name = "tbyzm";
            this.tbyzm.Size = new System.Drawing.Size(424, 22);
            this.tbyzm.TabIndex = 33;
            this.tbyzm.Text = "A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z,2,3,4,5,6,7,8,9";
            // 
            // btnyzm
            // 
            this.btnyzm.Location = new System.Drawing.Point(631, 433);
            this.btnyzm.Name = "btnyzm";
            this.btnyzm.Size = new System.Drawing.Size(58, 23);
            this.btnyzm.TabIndex = 34;
            this.btnyzm.Text = "激活码";
            this.btnyzm.UseVisualStyleBackColor = true;
            this.btnyzm.Click += new System.EventHandler(this.btnyzm_Click);
            // 
            // btntime
            // 
            this.btntime.Location = new System.Drawing.Point(641, 460);
            this.btntime.Name = "btntime";
            this.btntime.Size = new System.Drawing.Size(48, 23);
            this.btntime.TabIndex = 35;
            this.btntime.Text = "时间";
            this.btntime.UseVisualStyleBackColor = true;
            this.btntime.Click += new System.EventHandler(this.btntime_Click);
            // 
            // tbtimey
            // 
            this.tbtimey.Location = new System.Drawing.Point(336, 463);
            this.tbtimey.Name = "tbtimey";
            this.tbtimey.Size = new System.Drawing.Size(50, 21);
            this.tbtimey.TabIndex = 36;
            this.tbtimey.Text = "2016";
            // 
            // tbtimem
            // 
            this.tbtimem.Location = new System.Drawing.Point(414, 463);
            this.tbtimem.Name = "tbtimem";
            this.tbtimem.Size = new System.Drawing.Size(31, 21);
            this.tbtimem.TabIndex = 37;
            this.tbtimem.Text = "01";
            // 
            // tbtimed
            // 
            this.tbtimed.Location = new System.Drawing.Point(491, 463);
            this.tbtimed.Name = "tbtimed";
            this.tbtimed.Size = new System.Drawing.Size(33, 21);
            this.tbtimed.TabIndex = 38;
            this.tbtimed.Text = "01";
            // 
            // tbtimeh
            // 
            this.tbtimeh.Location = new System.Drawing.Point(567, 462);
            this.tbtimeh.Name = "tbtimeh";
            this.tbtimeh.Size = new System.Drawing.Size(35, 21);
            this.tbtimeh.TabIndex = 39;
            this.tbtimeh.Text = "00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(316, 467);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 40;
            this.label4.Text = "年";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(392, 467);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 41;
            this.label5.Text = "月";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(458, 467);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 42;
            this.label6.Text = "日";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(534, 467);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 43;
            this.label7.Text = "小时";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(268, 438);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 44;
            this.label8.Text = "长度";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(466, 440);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 45;
            this.label9.Text = "个数";
            // 
            // tblen
            // 
            this.tblen.Location = new System.Drawing.Point(299, 435);
            this.tblen.Name = "tblen";
            this.tblen.Size = new System.Drawing.Size(100, 21);
            this.tblen.TabIndex = 46;
            this.tblen.Text = "12";
            // 
            // tbcount
            // 
            this.tbcount.Location = new System.Drawing.Point(501, 436);
            this.tbcount.Name = "tbcount";
            this.tbcount.Size = new System.Drawing.Size(100, 21);
            this.tbcount.TabIndex = 47;
            this.tbcount.Text = "200";
            // 
            // openFilexml
            // 
            this.openFilexml.Filter = "工具可读文件|*.xml;";
            this.openFilexml.Multiselect = true;
            // 
            // tbblockx
            // 
            this.tbblockx.Location = new System.Drawing.Point(43, 84);
            this.tbblockx.Name = "tbblockx";
            this.tbblockx.Size = new System.Drawing.Size(100, 21);
            this.tbblockx.TabIndex = 31;
            // 
            // tbblocky
            // 
            this.tbblocky.Location = new System.Drawing.Point(180, 84);
            this.tbblocky.Name = "tbblocky";
            this.tbblocky.Size = new System.Drawing.Size(100, 21);
            this.tbblocky.TabIndex = 32;
            // 
            // btnblock
            // 
            this.btnblock.Location = new System.Drawing.Point(317, 84);
            this.btnblock.Name = "btnblock";
            this.btnblock.Size = new System.Drawing.Size(75, 23);
            this.btnblock.TabIndex = 33;
            this.btnblock.Text = "验证阻挡";
            this.btnblock.UseVisualStyleBackColor = true;
            this.btnblock.Click += new System.EventHandler(this.btnblock_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 34;
            this.label2.Text = "X：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(154, 89);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 12);
            this.label10.TabIndex = 34;
            this.label10.Text = "Y：";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(701, 666);
            this.Controls.Add(this.tbcount);
            this.Controls.Add(this.tblen);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbtimeh);
            this.Controls.Add(this.tbtimed);
            this.Controls.Add(this.tbtimem);
            this.Controls.Add(this.tbtimey);
            this.Controls.Add(this.btntime);
            this.Controls.Add(this.btnyzm);
            this.Controls.Add(this.tbyzm);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.tbmsgshow);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnClareContext);
            this.Controls.Add(this.tbcodepack);
            this.Controls.Add(this.btnALL);
            this.Controls.Add(this.btndic);
            this.Controls.Add(this.btnaddfile);
            this.Controls.Add(this.btndelfile);
            this.Controls.Add(this.labtips);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameTools Excel Protobuf";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbdbpack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labtips;
        private System.Windows.Forms.ComboBox combdbcons;
        private System.Windows.Forms.Button btndelfile;
        private System.Windows.Forms.Button btnaddfile;
        private System.Windows.Forms.Button btndic;
        private System.Windows.Forms.Button btnMySqlInDB;
        private System.Windows.Forms.Button btnMysql;
        private System.Windows.Forms.Button btnALL;
        private System.Windows.Forms.Button btnXmlJava;
        private System.Windows.Forms.Button btnXmlCSharp;
        private System.Windows.Forms.Button btnprotobufAll;
        private System.Windows.Forms.Button btnprotobufJava;
        private System.Windows.Forms.Button btnprotobufCSharp;
        private System.Windows.Forms.TextBox tbcodepack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button butxmlall;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbmsgshow;
        private System.Windows.Forms.Button btnClientXml;
        private System.Windows.Forms.Button btnClareContext;
        private System.Windows.Forms.TextBox tbyzm;
        private System.Windows.Forms.Button btnyzm;
        private System.Windows.Forms.Button btntime;
        private System.Windows.Forms.TextBox tbtimey;
        private System.Windows.Forms.TextBox tbtimem;
        private System.Windows.Forms.TextBox tbtimed;
        private System.Windows.Forms.TextBox tbtimeh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tblen;
        private System.Windows.Forms.TextBox tbcount;
        private System.Windows.Forms.Button btnCreateJpa;
        private System.Windows.Forms.Button btnCodeFirst;
        private System.Windows.Forms.Button btn_block_img;
        private System.Windows.Forms.OpenFileDialog openFilexml;
        private System.Windows.Forms.CheckBox cb_isnull;
        private System.Windows.Forms.Button btnblock;
        private System.Windows.Forms.TextBox tbblocky;
        private System.Windows.Forms.TextBox tbblockx;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
    }
}

