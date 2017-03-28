using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.CreateCode.excel.Csharp
{
    /// <summary>
    /// 生成读取XML的CSharp代码
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class CreateXMLCSharp : CreateBase
    {
        static readonly CreateXMLCSharp instance = new CreateXMLCSharp();
        public static CreateXMLCSharp Instance()
        {
            return instance;
        }

        protected override void CreateClassFile(string fileName, string msg)
        {
            base.CreateClassFile(CSharpPathXmlCode + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH") + "/" + fileName + ".cs", msg);
        }

        public void ActionExcelData(FileExcelDatas exceldatas, string strnamespace, Action<string> show)
        {
            Dictionary<string, ExcelDatas> datas = exceldatas.Datas;
            HashSet<string> filenames = new HashSet<string>();
            foreach (var data in datas)
            {
                string filename = "Q" + data.Value.SheetName.Replace("q_", "");
                if (data.Value.Rows == null || data.Value.Rows.Count == 0 || !filenames.Add(filename))
                {
                    continue;
                }
                CreateClassFile(filename, CreateCode(datas, data.Value, strnamespace, show, "      ", true));
            }
        }

        string CreateCode(Dictionary<string, ExcelDatas> datas, ExcelDatas model, string strnamespace, Action<string> show, string strspce = "      ", bool isCreatenamespace = true)
        {
            StringBuilder csharpBuilder = new StringBuilder();
            string sheetName = model.SheetName;
            if (isCreatenamespace)
            {
                csharpBuilder.AppendLine("namespace " + strnamespace)
                    .AppendLine("{")
                    .AppendLine()
                    .AppendLine()
                    .AppendLine(strspce + "using System;")
                    .AppendLine(strspce + "using System.Xml.Serialization;")
                    .AppendLine(strspce + "using System.Collections;")
                    .AppendLine(strspce + "using System.Collections.Generic;")
                    .AppendLine(strspce + "using System.Linq;")
                    .AppendLine(strspce + "using System.Text;")
                    .AppendLine(strspce + "using System.Xml;")
                    .AppendLine()
                    .AppendLine(strspce + "/// <summary> ")
                    .AppendLine(strspce + "/// Excel Data To Xml CSharp Class ")
                    .AppendLine(strspce + "/// <para>PS:</para>")
                    .AppendLine(strspce + "/// <para>@author 失足程序员</para>")
                    .AppendLine(strspce + "/// <para>@Blog http://www.cnblogs.com/ty408/</para>")
                    .AppendLine(strspce + "/// <para>@mail 492794628@qq.com</para>")
                    .AppendLine(strspce + "/// <para>@phone 13882122019</para>")
                    .AppendLine(strspce + "/// <para> @Create Time " + (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:sss")) + "</para>")
                    .AppendLine(strspce + "/// </summary>")
                    .AppendLine(strspce + "[XmlRootAttribute(\"" + sheetName + "Manager" + "\")]")
                    .AppendLine(strspce + "public class Q" + sheetName.Replace("q_", "") + "Manager")
                    .AppendLine(strspce + "{")
                    .AppendLine(strspce + "     [XmlArray(\"" + sheetName + "\")]")
                    .AppendLine(strspce + "     public List<Q" + sheetName.Replace("q_", "") + "> Q" + sheetName.Replace("q_", "") + "s { get; set; }")
                    .AppendLine(strspce + "}")// Class End
                    .AppendLine();
            }

            csharpBuilder.AppendLine(strspce + "[XmlRootAttribute(\"" + sheetName + "" + "\")]")
                    .AppendLine(strspce + "public class Q" + sheetName.Replace("q_", "") + "")
                    .AppendLine(strspce + "{")
                    .AppendLine();

            foreach (var row in model.Rows)
            {
                var cells = row.Value.Cells;
                foreach (var cell in cells)
                {
                    if ("ALL".Equals(cell.Cellgs) || "AC".Equals(cell.Cellgs))
                    {
                        csharpBuilder
                        .AppendLine(strspce + "     /// <summary> ")
                        .AppendLine(strspce + "     /// " + cell.CellNotes)
                        .AppendLine(strspce + "     /// <pre>" + (string.IsNullOrWhiteSpace(cell.CellFileName) ? "" : "关联文件：" + cell.CellFileName) + "</pre>")
                        .AppendLine(strspce + "     /// </summary> ")
                        .AppendLine(strspce + "     [XmlAttribute(\"" + cell.CellName + "\")]")
                        .AppendLine(strspce + "     public " + cell.CellValueType + " Q" + cell.CellName.Replace("q_", "") + " { get; set; }")
                        .AppendLine();
                        string dataKey = cell.CellFileName;

                        if (!string.IsNullOrWhiteSpace(dataKey) && datas.ContainsKey(dataKey))
                        {
                            csharpBuilder
                                .AppendLine(strspce + "     [XmlArray(\"" + datas[dataKey].SheetName + "\")]")
                                .AppendLine(strspce + "     public List<Q" + datas[dataKey].SheetName.Replace("q_", "") + "> Q" + datas[dataKey].SheetName.Replace("q_", "") + "s { get; set; }")
                                .AppendLine();
                        }
                    }
                }
                break;
            }
            csharpBuilder.AppendLine(strspce + "}");// Class End
            //foreach (var row in model.Rows)
            //{
            //    var cells = row.Value.Cells;
            //    foreach (var cell in cells)
            //    {
            //        string dataKey = cell.CellFileName;
            //        if (!string.IsNullOrWhiteSpace(dataKey) && datas.ContainsKey(dataKey))
            //        {
            //            csharpBuilder.AppendLine(CreateCode(datas, datas[dataKey], strnamespace, show, strspce, false));
            //        }
            //    }
            //    break;
            //}

            if (isCreatenamespace)
            {
                csharpBuilder.AppendLine("}");// namespace End
            }
            return csharpBuilder.ToString();
        }
    }
}
