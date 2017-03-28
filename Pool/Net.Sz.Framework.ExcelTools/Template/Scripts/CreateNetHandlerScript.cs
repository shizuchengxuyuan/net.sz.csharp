using Net.Sz.Framework.ExcelTools.CreateCode.protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sz.ExcelTools.Scripts
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class CreateNetHandlerScript : ICreateHandlerScript
    {

        public CreateNetHandlerScript() { }

        public void Create(Type memberType, MemberInfo member)
        {
            string fileName = "";
            string classname = member.Name.Replace("Message", "");
            string handllername = memberType.Namespace.Substring(memberType.Namespace.LastIndexOf('.') + 1);
            if (member.Name.StartsWith("Req"))
            {
                fileName = Net.Sz.Framework.ExcelTools.CreateCode.CreateBase.protobufnetHandler + "/Req/" + handllername + "/" + classname + "Handler.cs";
            }
            else if (member.Name.StartsWith("Res"))
            {
                fileName = Net.Sz.Framework.ExcelTools.CreateCode.CreateBase.protobufnetHandler + "/Res/" + handllername + "/" + classname + "Handler.cs";
            }
            else { return; }

            StringBuilder builder = new StringBuilder();
            builder.Append("using Net.Sz.Game.MMOGame.GameMessages;").AppendLine();
            builder.Append("using System;").AppendLine();
            builder.Append("using System.Collections.Generic;").AppendLine();
            builder.Append("").AppendLine();
            builder.Append("").AppendLine();
            builder.Append("namespace ").Append(memberType.Namespace).AppendLine();
            builder.Append("{").AppendLine();
            builder.Append("").AppendLine();
            builder.Append("").AppendLine();
            builder.Append("    /// <summary>                                ").AppendLine();
            builder.Append("    ///                                          ").AppendLine();
            builder.Append("    /// <para>PS:</para>                            ").AppendLine();
            builder.Append("    /// <para>@Author 失足程序员                       </para>").AppendLine();
            builder.Append("    /// <para>@Blog http://www.cnblogs.com/ty408/      </para>").AppendLine();
            builder.Append("    /// <para>@Mail 492794628@qq.com                   </para>").AppendLine();
            builder.Append("    /// <para>@Phone 13882122019                       </para>").AppendLine();
            builder.Append("    /// <para>@Create time : " + (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:sss")) + "</para>").AppendLine();
            builder.Append("    /// </summary>                                ").AppendLine();
            builder.Append("    public class ").Append(classname).Append("Handler : TcpHandler, Net.Sz.Framework.Script.IInitBaseScript").AppendLine();
            builder.Append("    {").AppendLine();
            builder.Append("        /// <summary>").AppendLine();
            builder.Append("        /// 执行脚本的初始化").AppendLine();
            builder.Append("        /// </summary>").AppendLine();
            builder.Append("        public void InitScript()").AppendLine();
            builder.Append("        {").AppendLine();
            builder.Append("            ").Append("MessageHelper.RegisterMessage(").AppendLine();
            builder.Append("            ").Append("(int)").Append(memberType.FullName).Append(".").Append(classname).Append(", //消息id").AppendLine();
            builder.Append("            ").Append("xx, //处理线程").AppendLine();
            builder.Append("            ").Append("typeof(").Append(classname).Append("Handler), //处理控制器").AppendLine();
            builder.Append("            ").Append("new ").Append(memberType.FullName).Append(".").Append(member.Name).Append("());//消息").AppendLine();
            builder.Append("        }").AppendLine();
            builder.Append("").AppendLine();
            builder.Append("").AppendLine();
            builder.Append("        public ").Append(classname).Append("Handler()").AppendLine();
            builder.Append("        {").AppendLine();
            builder.Append("        }").AppendLine();
            builder.Append("").AppendLine();
            builder.Append("").AppendLine();
            builder.Append("        public override void Run()").AppendLine();
            builder.Append("        {").AppendLine();
            builder.Append("            //处理消息").AppendLine();
            builder.Append("            var message = (").Append(memberType.FullName).Append(".").Append(member.Name).Append(")this.Message;").AppendLine();
            builder.Append("            ").AppendLine();
            builder.Append("        }").AppendLine();
            builder.Append("    }").AppendLine();
            builder.Append("}").AppendLine();

            string filePath = System.IO.Path.GetDirectoryName(fileName);
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, false))
            {
                sw.WriteLine(builder.ToString());
            }
        }
    }
}
