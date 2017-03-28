using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.CreateCode.excel
{

    /// <summary>
    /// 属性节点
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class ExcelCell
    {
        /// <summary>
        /// 标识字段的归属 ALL=客户端和服务器通用 AC=表示客户端使用 AS=表示服务使用
        /// </summary>
        public string Cellgs { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string CellNotes { get; set; }

        /// <summary>
        /// Excel节点名称
        /// </summary>
        public string CellName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string CellValue { get; set; }

        /// <summary>
        /// 属性类型 int，float
        /// </summary>
        public string CellValueType { get; set; }

        /// <summary>
        /// 长度，，如字符串类型 在数据需要设置长度 默认 255
        /// </summary>
        public int CellLength { get; set; }

        /// <summary>
        /// 是否是唯一键
        /// </summary>
        public bool IsPKey { get; set; }

        /// <summary>
        /// 关联excel文件
        /// </summary>
        public string CellFileName { get; set; }

        public string ToXmlString(string cellgs, bool is_Not_Null)
        {
            if (Cellgs.Equals("ALL") || cellgs.Equals("ALL") || Cellgs.Equals(cellgs))
            {
                if (is_Not_Null)
                {
                    if (string.IsNullOrWhiteSpace(CellValue) || "0".Equals(CellValue))
                    {
                        return null;
                    }
                }
                return " " + CellName + "=\"" + CellValue + "\"";
            }
            return "";
        }

        public string ToMysqlString(string cellgs)
        {
            StringBuilder sbuilder = new StringBuilder();
            if (Cellgs.Equals("ALL") || cellgs.Equals("ALL") || Cellgs.Equals(cellgs))
            {
                sbuilder.Append(" `" + CellName + "` ");
                switch (CellValueType.ToLower())
                {
                    case "int":
                        sbuilder.Append("int ");
                        break;
                    case "boolean":
                        sbuilder.Append(" TINYINT(1) ");
                        break;
                    case "double":
                        sbuilder.Append(" double ");
                        break;
                    case "float":
                        sbuilder.Append(" float ");
                        break;
                    case "string":

                        if (CellLength < 500)
                        {
                            sbuilder.Append(" VARCHAR(" + CellLength + ") ");
                        }
                        else if (CellLength < 10000)
                        {
                            sbuilder.Append(" TEXT ");
                        }
                        else
                        {
                            sbuilder.Append(" LONGTEXT ");
                        }
                        break;
                    case "long":
                        sbuilder.Append(" bigint ");
                        break;
                }
                if (IsPKey)
                {
                    sbuilder.Append(" NOT NULL primary key ");
                }
                sbuilder.Append(" COMMENT '" + CellNotes + "'");
            }
            return sbuilder.ToString();
        }
    }

    /// <summary>
    /// 一行数据
    /// </summary>
    public class ExcelRow
    {
        public ExcelRow()
        {
            Cells = new List<ExcelCell>();
        }

        public List<ExcelCell> Cells { get; set; }

        /// <summary>
        /// 主键，唯一键
        /// </summary>
        public string AtKey { get; set; }

        public string ToXmlString(string cellgs, bool is_Not_Null)
        {
            StringBuilder xmlBuilder = new StringBuilder();
            foreach (var item in Cells)
            {
                string tmp = item.ToXmlString(cellgs, is_Not_Null);
                if (!string.IsNullOrWhiteSpace(tmp))
                {
                    xmlBuilder.Append(tmp);
                }
            }
            return xmlBuilder.ToString();
        }


        /// <summary>
        /// 生成表头
        /// </summary>
        /// <returns></returns>
        public string ToMysqlString(string cellgs)
        {
            StringBuilder xmlBuilder = new StringBuilder();
            bool isdouhao = false;
            for (int i = 0; i < Cells.Count; i++)
            {
                string mysql = Cells[i].ToMysqlString(cellgs);
                if (!string.IsNullOrWhiteSpace(mysql))
                {
                    if (isdouhao)
                    {
                        xmlBuilder.AppendLine(",");
                    }
                    else
                    {
                        xmlBuilder.AppendLine("");
                    }
                    xmlBuilder.Append("  " + mysql);
                    isdouhao = true;
                }
            }
            return xmlBuilder.ToString();
        }
    }

    class ExcelCellComparer : IComparer<ExcelCell>
    {
        public int Compare(ExcelCell x, ExcelCell y)
        {
            return string.Compare(x.CellName, y.CellName, true);
        }
    }

    /// <summary>
    /// 单个 Excel 文件
    /// </summary>
    public class ExcelDatas
    {
        public ExcelDatas()
        {
            Rows = new Dictionary<string, ExcelRow>();
        }

        /// <summary>
        /// 数据行
        /// </summary>
        public Dictionary<string, ExcelRow> Rows { get; set; }

        public void Add(string key, ExcelRow row)
        {
            //row.Cells.Sort(new ExcelCellComparer());
            Rows.Add(key, row);
        }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string SheetName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileExcelDatas
    {
        public FileExcelDatas()
        {
            Datas = new Dictionary<string, ExcelDatas>();
        }

        /// <summary>
        /// 存储类型和文件名对应的
        /// </summary>
        public Dictionary<string, HashSet<string>> SheetName_And_FileName = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// 存储类型和文件名对应的
        /// </summary>
        public Dictionary<string, HashSet<string>> FileName_And_SheetName = new Dictionary<string, HashSet<string>>();


        /// <summary>
        ///  key：  excel 文件名
        ///  <para>value： excel 文件对应的数据</para>
        /// </summary>
        public Dictionary<string, ExcelDatas> Datas { get; set; }

    }

}
