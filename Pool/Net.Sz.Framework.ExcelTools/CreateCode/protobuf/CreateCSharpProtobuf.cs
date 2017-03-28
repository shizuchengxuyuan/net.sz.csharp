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
    public class CreateCSharpProtobuf
    {
        public static void CreateCode(List<ItemFileInfo> files, Action<string> show)
        {
            if (!System.IO.Directory.Exists(CreateBase.protobufnetMessage))
            {
                System.IO.Directory.CreateDirectory(CreateBase.protobufnetMessage);
            }

            string basepath = AppDomain.CurrentDomain.BaseDirectory + "protobuf\\";
            //string basepath = AppDomain.CurrentDomain.BaseDirectory;

            if (!System.IO.Directory.Exists(basepath)) { System.IO.Directory.CreateDirectory(basepath); }

            foreach (var item in files) { if (item.ExtensionName.Equals(".proto")) File.Copy(item.Path, basepath + item.Name, true); }

            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "protobuf/protogen.exe";
                //protogen -i:input.proto -o:output.cs  
                //protogen -i:input.proto -o:output.xml -t:xml  
                //protogen -i:input.proto -o:output.cs -p:datacontract -q  
                //protogen -i:input.proto -o:output.cs -p:observable=true  
                string str2 = "";
                foreach (var item in files)
                {
                    if (item.ExtensionName.Equals(".proto"))
                    {
                        show("开始处理 Protobuf 文件：" + item.Name + " 生成 CSharp 文件");
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item.Path);
                        process.StartInfo.Arguments = " -i:" + basepath + item.Name + " -o:" + CreateBase.protobufnetMessage + "/" + fileNameWithoutExtension + ".cs -p:datacontract -q";
                        process.Start();
                        str2 = process.StandardError.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(str2))
                        {
                            show("C#文件:" + fileNameWithoutExtension + " 错误:" + str2);
                            break;
                        }
                    }
                }
                //process.WaitForExit();
                process.Close();
            }

            foreach (var item in files) { if (item.ExtensionName.Equals(".proto")) File.Delete(basepath + item.Name); }

            //LoadProtoMessage.Instance.LoadCSharpFile(new string[] { CreateBase.protobufnetMessage });
            show("");
            show("");
            show("创建 protobuffer CSharp 代码完成 目录：" + CreateBase.protobufnetMessage);
            show("");
            //System.Diagnostics.Process.Start(CreateBase.protobufnet);
        }
    }
}
