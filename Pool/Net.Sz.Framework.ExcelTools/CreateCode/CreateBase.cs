using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.CreateCode
{
    /// <summary>
    /// 创建代码父类
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class CreateBase
    {
        public static string BasePath = "C:/ExcelSource";
        public static string JavaPath = BasePath + "/Java";
        public static string JavaPathXml = JavaPath + "/xml";
        public static string JavaPathData = JavaPath + "/Data";
        public static string JavaJpaCodePathData = JavaPath + "/Jpa";
        public static string CSharpPath = BasePath + "/C#";
        public static string CSharpPathXml = CSharpPath + "/xml";
        public static string CSharpPathXmlCode = CSharpPath + "/XmlCode";
        public static string CSharpPathCodeFirstCode = CSharpPath + "/CodeFirstCode";
        public static string TextPath = BasePath + "/Text";
        public static string MySqlPath = BasePath + "/MySql";
        public static string protobuf = BasePath + "/protobuf";
        public static string protobufnet = protobuf + "/net";
        public static string protobufnetMessage = protobufnet + "/Message";
        public static string protobufnetHandler = protobufnet + "/Handler";
        public static string protobufjava = protobuf + "/java";
        public static string protobufjavaMessage = protobufjava + "/Message";
        public static string protobufjavaHandle = protobufjava + "/handle";

        public static void SetPath(string basepath)
        {
            BasePath = basepath;
            JavaPath = BasePath + "/Java";
            JavaPathXml = JavaPath + "/xml";
            JavaPathData = JavaPath + "/Data";
            JavaJpaCodePathData = JavaPath + "/Jpa";
            CSharpPath = BasePath + "/C#";
            CSharpPathXml = CSharpPath + "/xml";
            CSharpPathXmlCode = CSharpPath + "/XmlCode";
            CSharpPathCodeFirstCode = CSharpPath + "/CodeFirstCode";
            TextPath = BasePath + "/Text";
            MySqlPath = BasePath + "/MySql";
            protobuf = BasePath + "/protobuf";
            protobufnet = protobuf + "/net";
            protobufnetMessage = protobufnet + "/Message";
            protobufnetHandler = protobufnet + "/Handler";
            protobufjava = protobuf + "/java";
            protobufjavaMessage = protobufjava + "/Message";
            protobufjavaHandle = protobufjava + "/handle";
        }

        /// <summary>
        /// 目录控制,删除目录，重建
        /// </summary>
        public static void DirectoryAction(Action<string> show)
        {
            try
            {
                if (System.IO.Directory.Exists(BasePath))
                {
                    System.IO.Directory.Delete(BasePath, true);
                }
                System.Threading.Thread.Sleep(1000);
                System.IO.Directory.CreateDirectory(JavaPathData);
                System.IO.Directory.CreateDirectory(JavaPathXml);
                System.IO.Directory.CreateDirectory(JavaJpaCodePathData);
                System.IO.Directory.CreateDirectory(CSharpPathXml);
                System.IO.Directory.CreateDirectory(CSharpPathXmlCode);
                System.IO.Directory.CreateDirectory(CSharpPathCodeFirstCode);
                System.IO.Directory.CreateDirectory(MySqlPath);
                System.IO.Directory.CreateDirectory(TextPath);
                System.IO.Directory.CreateDirectory(protobufnet);
                System.IO.Directory.CreateDirectory(protobufnetMessage);
                System.IO.Directory.CreateDirectory(protobufnetHandler);
                System.IO.Directory.CreateDirectory(protobufjava);
                System.IO.Directory.CreateDirectory(protobufjavaHandle);
                System.IO.Directory.CreateDirectory(protobufjavaMessage);
                show("目录整理完成 " + BasePath);
            }
            catch
            {
                show("目录整理失败，请先关闭打开的文件或目录");
                return;
            }
        }

        /// <summary>
        /// 创建 文件
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        protected virtual void CreateClassFile(string fileName, string msg)
        {
            string filePath = System.IO.Path.GetDirectoryName(fileName);
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, false))
            {
                sw.WriteLine(msg);
            }
        }

        /// <summary>
        /// 首字母大写，过滤关键字，
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public String FirstCharUpper(String a)
        {
            return a.Substring(0, 1).ToUpper() + a.Substring(1);
        }
    }
}
