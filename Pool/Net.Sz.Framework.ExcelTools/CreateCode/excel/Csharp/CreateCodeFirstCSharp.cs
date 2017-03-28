using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Net.Sz.Framework.ExcelTools.CreateCode.excel.Csharp
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class CreateCodeFirstCSharp : CreateBase
    {
        static readonly CreateCodeFirstCSharp instance = new CreateCodeFirstCSharp();
        public static CreateCodeFirstCSharp Instance()
        {
            return instance;
        }

        protected override void CreateClassFile(string fileName, string msg)
        {
            base.CreateClassFile(CSharpPathCodeFirstCode + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH") + "/" + fileName + ".cs", msg);
        }

        public void ActionExcelData(FileExcelDatas exceldatas, string strnamespace, Action<string> show)
        {
            Dictionary<string, ExcelDatas> datas = exceldatas.Datas;

            List<string> snames = new List<string>();

            StringBuilder builderDB = new StringBuilder();

            builderDB.AppendLine("using System;");
            builderDB.AppendLine("using System.Collections.Generic;");
            builderDB.AppendLine("using System.Linq;");
            builderDB.AppendLine("using System.Text;");
            builderDB.AppendLine("");
            builderDB.AppendLine("");
            builderDB.AppendLine("namespace " + strnamespace);
            builderDB.AppendLine("{");
            builderDB.AppendLine("    /// <summary>");
            builderDB.AppendLine("    ///");
            builderDB.AppendLine("    /// <para>PS:</para>");
            builderDB.AppendLine("    /// <para>@author 失足程序员</para>");
            builderDB.AppendLine("    /// <para>@Blog http://www.cnblogs.com/ty408/</para>");
            builderDB.AppendLine("    /// <para>@mail 492794628@qq.com</para>");
            builderDB.AppendLine("    /// <para>@phone 13882122019</para>");
            builderDB.AppendLine("    /// <para> @Create Time " + (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:sss")) + "</para>");
            builderDB.AppendLine("    /// </summary>");
            builderDB.AppendLine("    public class Gamesr_Data : Net.Sz.Framework.DB.Mysql.MysqlDbContext");
            builderDB.AppendLine("    {");
            builderDB.AppendLine("        static GameSr_Data()");
            builderDB.AppendLine("        {");
            builderDB.AppendLine("             Net.Sz.Framework.DB.ReportingDbMigrationsConfiguration<GameSr_Data>.Initializer();");
            builderDB.AppendLine("        }");
            builderDB.AppendLine("");
            builderDB.AppendLine("        public Gamesr_Data(string nameOrConnectionString)");
            builderDB.AppendLine("            : base(nameOrConnectionString)");
            builderDB.AppendLine("        {");
            builderDB.AppendLine("        }");
            builderDB.AppendLine("");

            HashSet<string> filenames = new HashSet<string>();

            foreach (var data in datas)
            {
                string filename = "Q" + data.Value.SheetName.Replace("q_", "");
                if (data.Value.Rows == null || data.Value.Rows.Count == 0 || !filenames.Add(filename))
                {
                    continue;
                }
                CreateClassFile("Q" + data.Value.SheetName.Replace("q_", ""), CreateCode(datas, data.Value, strnamespace, show, builderDB));
            }

            builderDB.AppendLine("");
            builderDB.AppendLine("    }");
            builderDB.AppendLine("}");
            CreateClassFile("Gamesr_Data", builderDB.ToString());
        }

        string CreateCode(Dictionary<string, ExcelDatas> datas, ExcelDatas model, string strnamespace, Action<string> show, StringBuilder builderDB, string strspce = "      ")
        {
            StringBuilder csharpBuilder = new StringBuilder();
            string sheetName = model.SheetName;

            builderDB.AppendLine("        public System.Data.Entity.DbSet<Q" + sheetName.Replace("q_", "") + "> Q" + sheetName.Replace("q_", "") + "s { get; set; } ");

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
                .AppendLine(strspce + "/// <summary>")
                .AppendLine(strspce + "/// ")
                .AppendLine(strspce + "/// <para>Excel Data To CSharp CodeFirst Class</para>")
                .AppendLine(strspce + "/// <para>@Create Code：失足程序员</para>")
                .AppendLine(strspce + "/// <para>@Create Time：" + (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:sss")) + "</para>")
                .AppendLine(strspce + "/// <para>@Phone：13882122019</para>")
                .AppendLine(strspce + "/// <para>@email：492794628@qq.com</para>")
                .AppendLine(strspce + "/// </summary>")
                .AppendLine(strspce + "[System.ComponentModel.DataAnnotations.Schema.Table(\"" + sheetName + "" + "\")]")
                .AppendLine(strspce + "public class Q" + sheetName.Replace("q_", "") + "")
                .AppendLine(strspce + "{")
                .AppendLine();

            foreach (var row in model.Rows)
            {
                var cells = row.Value.Cells;
                foreach (var cell in cells)
                {
                    csharpBuilder
                        .AppendLine(strspce + "     /// <summary> ")
                        .AppendLine(strspce + "     /// " + cell.CellNotes)
                        .AppendLine(strspce + "     /// <para>" + (String.IsNullOrWhiteSpace(cell.CellFileName) ? "" : "关联文件：" + cell.CellFileName) + "</para>")
                        .AppendLine(strspce + "     /// </summary> ");
                    if (cell.IsPKey)
                    {
                        csharpBuilder.AppendLine(strspce + "     [System.ComponentModel.DataAnnotations.Key, System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]");
                    }
                    csharpBuilder.AppendLine(strspce + "     [System.ComponentModel.DataAnnotations.Schema.Column(\"" + cell.CellName + "\")]")
                        .AppendLine(strspce + "     public " + cell.CellValueType + " Q" + cell.CellName.Replace("q_", "") + " { get; set; }")
                        .AppendLine();
                }
                break;
            }
            csharpBuilder.AppendLine(strspce + "}");// Class End
            csharpBuilder.AppendLine("}");// namespace End
            return csharpBuilder.ToString();
        }
    }
}
