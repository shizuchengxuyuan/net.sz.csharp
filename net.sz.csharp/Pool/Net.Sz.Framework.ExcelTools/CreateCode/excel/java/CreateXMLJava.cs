using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.CreateCode.excel.java
{
    public class CreateXMLJava : CreateBase
    {
        static readonly CreateXMLJava instance = new CreateXMLJava();
        public static CreateXMLJava Instance()
        {
            return instance;
        }

        protected override void CreateClassFile(string fileName, string msg)
        {
            if (!System.IO.Directory.Exists(JavaPath))
            {
                System.IO.Directory.CreateDirectory(JavaPath);
            }
            base.CreateClassFile(JavaPathData + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH") + "/" + fileName + ".java", msg);
        }

        public void ActionExcelData(FileExcelDatas exceldatas, string strnamespace, Action<string> show)
        {
            StringBuilder javaBuilder = new StringBuilder();
            javaBuilder.AppendLine("package " + strnamespace + ";")
                         .AppendLine()
                         .AppendLine()
                         .AppendLine("import java.util.List;")
                         .AppendLine("import java.util.ArrayList;")
                         .AppendLine("import javax.persistence.Column;")
                         .AppendLine("import javax.persistence.Entity;")
                         .AppendLine("import javax.persistence.Id;")
                         .AppendLine("import javax.xml.bind.annotation.XmlRootElement;")
                         .AppendLine("import javax.persistence.Table;")
                         .AppendLine("import javax.persistence.Basic;")
                         .AppendLine()
                         .AppendLine()
                         .AppendLine("/* ")
                         .AppendLine("* Data Source To java Class ")
                         .AppendLine("* ")
                         .AppendLine("* @Create Code Troy.Chen")
                         .AppendLine("* @Phone 13882122019")
                         .AppendLine("* @email 492794628@qq.com")
                         .AppendLine("*  " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:sss"))
                         .AppendLine("* ")
                         .AppendLine("*/")
                         .AppendLine("@XmlRootElement")
                         //.AppendLine("public class " + sheetName)
                         .AppendLine("{");

        }
        string CreateCode(Dictionary<string, ExcelDatas> datas, ExcelDatas model, string strnamespace, Action<string> show, string strspce = "      ", bool isCreatenamespace = true) 
        {
            
            StringBuilder javaBuilder = new StringBuilder();
            string sheetName = model.SheetName;
            if (isCreatenamespace)
            {
                javaBuilder.AppendLine("package " + strnamespace + ";")
                    .AppendLine()
                    .AppendLine()
                    .AppendLine("import java.util.List;")
                    .AppendLine("import java.util.ArrayList;")
                    .AppendLine("import javax.persistence.Column;")
                    .AppendLine("import javax.persistence.Entity;")
                    .AppendLine("import javax.persistence.Id;")
                    .AppendLine("import javax.xml.bind.annotation.XmlRootElement;")
                    .AppendLine("import javax.persistence.Table;")
                    .AppendLine("import javax.persistence.Basic;")
                    .AppendLine()
                    .AppendLine()
                    .AppendLine("/* ")
                    .AppendLine("* Data Source To java Class ")
                    .AppendLine("* ")
                    .AppendLine("* @Create Code Troy.Chen")
                    .AppendLine("* @Phone 13882122019")
                    .AppendLine("* @email 492794628@qq.com")
                    .AppendLine("*  " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:sss"))
                    .AppendLine("* ")
                    .AppendLine("*/")
                    .AppendLine("[XmlRootAttribute(\"" + sheetName + "" + "\")]")
                    .AppendLine("public class " + sheetName + "")
                    .AppendLine("{")
                    .AppendLine();

            }

            foreach (var row in model.Rows)
            {
                var cells = row.Value.Cells;
                foreach (var cell in cells)
                {



                    javaBuilder
                        .AppendLine(strspce + "     /// <summary> ")
                        .AppendLine(strspce + "     /// " + cell.CellNotes)
                        .AppendLine(strspce + "     /// <pre>" + (string.IsNullOrWhiteSpace(cell.CellFileName) ? "" : "关联文件：" + cell.CellFileName) + "</pre>")
                        .AppendLine(strspce + "     /// </summary> ")
                        .AppendLine(strspce + "     [XmlAttribute(\"" + cell.CellName + "\")]")
                        .AppendLine(strspce + "     public " + cell.CellValueType + " " + cell.CellName + " { get; set; }")
                        .AppendLine();
                    string dataKey = cell.CellFileName;

                    if (!string.IsNullOrWhiteSpace(dataKey) && datas.ContainsKey(dataKey))
                    {
                        javaBuilder
                            .AppendLine(strspce + "     [XmlArray(\"" + datas[dataKey].SheetName + "\")]")
                            .AppendLine(strspce + "     public List<" + datas[dataKey].SheetName + "> " + datas[dataKey].SheetName + "s { get; set; }")
                            .AppendLine();
                    }
                }
                break;
            }
            javaBuilder.AppendLine(strspce + "}");// Class End
            foreach (var row in model.Rows)
            {
                var cells = row.Value.Cells;
                foreach (var cell in cells)
                {
                    string dataKey = cell.CellFileName;
                    if (!string.IsNullOrWhiteSpace(dataKey) && datas.ContainsKey(dataKey))
                    {
                        javaBuilder.AppendLine(CreateCode(datas, datas[dataKey], strnamespace, show, strspce, false));
                    }
                }
                break;
            }

            if (isCreatenamespace)
            {
                javaBuilder.AppendLine("}");// namespace End
            }
            return javaBuilder.ToString();
        }
    }
}
