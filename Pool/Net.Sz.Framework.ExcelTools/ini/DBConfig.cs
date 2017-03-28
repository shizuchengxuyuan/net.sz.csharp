using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.ini
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class DBConfigs
    {

        public string SavePath { get; set; }
        public string SaveJavaMessagePath { get; set; }
        public string SaveJPAPath { get; set; }
        public string SaveCsharpMessagePath { get; set; }
        public string NamespaceStr { get; set; }        
        public bool IsNullEmpty { get; set; }

        public List<DBConfig> Configs { get; set; }
        public DBConfigs()
        {
            Configs = new List<DBConfig>();
            SavePath = " ";
            SaveJavaMessagePath = " ";
            SaveJPAPath = " ";
            SaveCsharpMessagePath = " ";
            NamespaceStr = " ";
        }
    }
    public class DBConfig
    {
        public string DBPath { get; set; }
        public int DBPart { get; set; }
        public string DBUser { get; set; }
        public string DBBase { get; set; }
        public string DBPwd { get; set; }

        public DBConfig()
        {

        }

        public override string ToString()
        {
            return DBPath + "(" + DBBase + ")";//"数据库：" + DBBase + " 链接地址：" + DBPath;
        }

    }
}
