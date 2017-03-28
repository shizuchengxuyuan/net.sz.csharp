using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools
{

    /// <summary>
    /// 文件关联性
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class ItemFileInfo
    {
        public ItemFileInfo(string name, string content, string path)
        {
            this.Name = name;
            this.Content = content;
            this.Path = path;
        }

        public ItemFileInfo(string filePath)
        {
            this.Path = filePath;
            this.ExtensionName = System.IO.Path.GetExtension(filePath);
            this.Name = System.IO.Path.GetFileName(filePath);
        }

        private string _Name;
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Content;
        /// <summary>
        /// 
        /// 文件内容,xml
        /// 
        /// </summary>
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        private string _Path;

        /// <summary>
        /// 文件完整路径
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }

        string _ExtensionName;
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string ExtensionName
        {
            get { return _ExtensionName; }
            set { _ExtensionName = value; }
        }

        /// <summary>
        /// 重新toString 方法 返回文件名，
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
