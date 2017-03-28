using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Net.Sz.Framework.ExcelTools.CreateCode.excel.java
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class CreateJavaJpaCode : CreateBase
    {
        static readonly CreateJavaJpaCode instance = new CreateJavaJpaCode();
        public static CreateJavaJpaCode Instance()
        {
            return instance;
        }

        protected override void CreateClassFile(string fileName, string msg)
        {
            base.CreateClassFile(JavaJpaCodePathData + "/" + fileName + ".java", msg);
        }

        public void ActionExcelData(FileExcelDatas exceldatas, string strnamespace, Action<string> show)
        {
            Dictionary<string, ExcelDatas> datas = exceldatas.Datas;

            List<string> snames = new List<string>();

            HashSet<string> filenames = new HashSet<string>();

            foreach (var data in datas)
            {
                string filename = "Q" + data.Value.SheetName.Replace("q_", "");
                if (data.Value.Rows == null || data.Value.Rows.Count == 0 || !filenames.Add(filename))
                {
                    continue;
                }
                CreateClassFile("Q" + data.Value.SheetName.Replace("q_", ""), CreateCode(datas, data.Value, strnamespace, show));
            }
        }

        string CreateCode(Dictionary<string, ExcelDatas> datas, ExcelDatas model, string strnamespace, Action<string> show)
        {
            StringBuilder csharpBuilder = new StringBuilder();
            string sheetName = model.SheetName;

            csharpBuilder.Append("package ").Append(strnamespace).AppendLine(";")
                .AppendLine()
                .AppendLine()
                .AppendLine("import java.io.Serializable;")
                .AppendLine("import javax.persistence.Column;")
                .AppendLine("import javax.persistence.Entity;")
                .AppendLine("import javax.persistence.GeneratedValue;")
                .AppendLine("import javax.persistence.GenerationType;")
                .AppendLine("import javax.persistence.Id;")
                .AppendLine("import javax.persistence.Table;")
                .AppendLine()
                .AppendLine("/**")
                .AppendLine("* ")
                .AppendLine("* <br>")
                .AppendLine("* Excel Data To Java Jpa C3P0 Class<br>")
                .AppendLine("* Create Code：失足程序员<br>")
                .AppendLine("* blog http://www.cnblogs.com/ty408/<br>")
                .AppendLine("* Create Time：" + (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:sss")) + "<br>")
                .AppendLine("* Phone：13882122019<br>")
                .AppendLine("* email：492794628@qq.com<br>")
                .AppendLine("**/")
                .AppendLine("@Entity")
                .AppendLine("@Table(name = \"" + sheetName + "" + "\")")
                .AppendLine("public class Q" + sheetName.Replace("q_", "") + " implements Serializable {")
                .AppendLine();

            foreach (var row in model.Rows)
            {
                var cells = row.Value.Cells;
                foreach (var cell in cells)
                {
                    if ("ALL".Equals(cell.Cellgs) || "AS".Equals(cell.Cellgs))
                    {
                        csharpBuilder
                            .AppendLine("     /**")
                            .AppendLine("     * " + cell.CellNotes)
                            .AppendLine("     * " + (String.IsNullOrWhiteSpace(cell.CellFileName) ? "" : "关联文件：" + cell.CellFileName) + "<br>")
                            .AppendLine("     **/");
                        if (cell.IsPKey)
                        {
                            csharpBuilder.AppendLine("     @Id");
                            //csharpBuilder.AppendLine("     @GeneratedValue(strategy = GenerationType.IDENTITY)");
                        }
                        csharpBuilder.AppendLine("     @Column(name = \"" + cell.CellName + "\")")
                            .AppendLine("     private " + cell.CellValueType + " q" + cell.CellName.Replace("q_", "") + ";")
                            .AppendLine();

                        csharpBuilder
                            .AppendLine("     /**")
                            .AppendLine("     * " + cell.CellNotes + " " + (String.IsNullOrWhiteSpace(cell.CellFileName) ? "" : ("关联文件：" + cell.CellFileName) + "<br>"))
                            .AppendLine("     **/")
                            .AppendLine("     public void setQ" + cell.CellName.Replace("q_", "") + "(" + cell.CellValueType + " q" + cell.CellName.Replace("q_", "") + "){")
                            .AppendLine("          this." + "q" + cell.CellName.Replace("q_", "") + " = q" + cell.CellName.Replace("q_", "") + ";")
                            .AppendLine("     }")
                            .AppendLine("     ")
                            .AppendLine("     ")
                            .AppendLine("     /**")
                            .AppendLine("     * " + cell.CellNotes + " " + (String.IsNullOrWhiteSpace(cell.CellFileName) ? "" : ("关联文件：" + cell.CellFileName) + "<br>"))
                            .AppendLine("     **/")
                            .AppendLine("     public " + cell.CellValueType + " getQ" + cell.CellName.Replace("q_", "") + "() {")
                            .AppendLine("          return this." + "q" + cell.CellName.Replace("q_", "") + ";")
                            .AppendLine("     }")
                            .AppendLine();
                    }
                }
                break;
            }
            csharpBuilder.AppendLine("}");// Class End
            return csharpBuilder.ToString();
        }
    }
}

