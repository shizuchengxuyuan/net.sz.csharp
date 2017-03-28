using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.CreateCode.protobuf
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class CreateJavaProtobuf
    {
        public static void CreateCode(List<ItemFileInfo> files, Action<string> show)
        {
            if (!System.IO.Directory.Exists(CreateBase.protobufjavaMessage))
            {
                System.IO.Directory.CreateDirectory(CreateBase.protobufjavaMessage);
            }

            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "protobuf/protoc.exe";
                foreach (var item in files)
                {
                    if (item.ExtensionName.Equals(".proto"))
                    {
                        show("开始处理 Protobuf 文件：" + item.Name + " 生成 Java 文件");
                        string fileNameWithoutExtension = Path.GetFileName(item.Path);
                        string dir = Path.GetDirectoryName(item.Path);
                        //process.StartInfo.Arguments = "--proto_path=protobuf\\ --java_out=" + CreateBase.protobufjava + "   " + fileNameWithoutExtension;
                        process.StartInfo.Arguments = "--proto_path=" + dir + " --java_out=" + CreateBase.protobufjavaMessage + "   " + item.Path;
                        process.Start();
                        string error = process.StandardError.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            show("java文件:" + fileNameWithoutExtension + "   错误:" + error);
                            return;
                        }
                    }
                }
                //process.WaitForExit();
                process.Close();
                show("");
                show("");
                show("创建 protobuffer Java 代码完成 目录：" + CreateBase.protobufjavaMessage);
                show("");
                //System.Diagnostics.Process.Start(CreateBase.protobufjavaMessage);
            }
        }
    }
}
