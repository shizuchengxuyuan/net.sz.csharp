using Net.Sz.Framework.ExcelTools.excelindb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Net.Sz.Framework.ExcelTools.CreateCode.excel.mysql
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class CreateMysql : CreateBase
    {
        static readonly CreateMysql instance = new CreateMysql();
        public static CreateMysql Instance()
        {
            return instance;
        }

        protected override void CreateClassFile(string fileName, string msg)
        {
            base.CreateClassFile(MySqlPath + "/" + fileName + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss") + ".sql", msg);
        }

        public void ActionExcel(FileExcelDatas exceldatas, string cellgs, string useStr, bool inDB, Action<string> show)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                if (inDB)
                {
                    builder.AppendLine("USE " + useStr + ";");
                }
                CreateTableHeadInster(builder, exceldatas, cellgs, useStr, inDB, show);
                CreateTableRowInster(builder, exceldatas, cellgs, useStr, inDB, show);
                if (inDB)
                {
                    try
                    {
                        MySqlDB.Instance().ReMySqlConnection();
                        int count = MySqlDB.Instance().ExecuteNonQuery(builder.ToString());
                        show("写入数据 影响行数 " + count);
                    }
                    catch (Exception ex)
                    {
                        show("数据库操作失败：" + ex.ToString());
                        return;
                    }
                    finally
                    {
                        try
                        {
                            MySqlDB.Instance().Close();
                        }
                        catch (Exception ex)
                        {
                            show("数据库关闭错误：" + ex.ToString());
                        }
                    }
                }
                else
                {
                    CreateClassFile("mysql", builder.ToString());
                }
            }
            catch (Exception ex)
            {
                show("请仔细检查配置表 错误信息：" + ex.ToString());
            }
        }

        public bool CreateTableHeadInster(StringBuilder builder, FileExcelDatas exceldatas, string cellgs, string useStr, bool inDB, Action<string> show)
        {
            HashSet<string> tablenames = new HashSet<string>();
            foreach (var datas in exceldatas.Datas)
            {
                if (datas.Value.Rows.Count > 0)
                {
                    string fileName = datas.Key;
                    string sheetName = datas.Value.SheetName;
                    if (tablenames.Add(sheetName))
                    {
                        show("创建数据表：" + sheetName);
                        builder.AppendLine("drop table if exists " + sheetName + ";")
                            .AppendLine("CREATE TABLE " + sheetName + "(")
                        .AppendLine(datas.Value.Rows.Values.ToList()[0].ToMysqlString(cellgs))
                        .AppendLine(");");
                    }
                }
            }
            return true;
        }

        public bool CreateTableRowInster(StringBuilder builder, FileExcelDatas exceldatas, string cellgs, string useStr, bool inDB, Action<string> show)
        {
            HashSet<string> tablenames = new HashSet<string>();
            foreach (var itemkey in exceldatas.SheetName_And_FileName.Keys)
            {
                var datas = exceldatas.Datas[itemkey];
                StringBuilder sqlfileBuilder = new StringBuilder();

                string sheetName = datas.SheetName;
                string fileName = itemkey;
                if (!tablenames.Add(sheetName))
                {
                    continue;
                }
                StringBuilder sqlBuilder = new StringBuilder();
                List<ExcelCell> cellNames = new List<ExcelCell>();
                var rows = datas.Rows.Values.ToArray();
                {
                    var cells = rows[0].Cells;
                    sqlBuilder.Append("INSERT " + sheetName + " (");
                    bool isdouhao = false;
                    for (int k = 0; k < cells.Count; k++)
                    {
                        if (cells[k].Cellgs.Equals("ALL") || cellgs.Equals("ALL") || cells[k].Cellgs.Equals(cellgs))
                        {
                            cellNames.Add(cells[k]);
                            if (isdouhao) { sqlBuilder.Append(", "); }
                            sqlBuilder.Append("`" + cells[k].CellName + "`");
                            isdouhao = true;
                        }
                        if (k == cells.Count - 1)
                        {
                            sqlBuilder.AppendLine(")");
                            sqlBuilder.AppendLine(" VALUES ");
                        }
                    }
                }

                for (int i = 0; i < rows.Length; i++)
                {
                    sqlBuilder.Append("(");
                    var cells = rows[i].Cells;
                    bool isdouhao = false;
                    for (int k = 0; k < cellNames.Count; k++)
                    {
                        ExcelCell cellK = cellNames[k];
                        ExcelCell cell = null;
                        foreach (var item in cells)
                        {
                            if (item.CellName.Equals(cellK.CellName))
                            {
                                cell = item;
                                break;
                            }
                        }
                        if (cell == null)
                        {
                            cell = cellK;
                        }
                        if (cell.Cellgs.Equals("ALL") || cellgs.Equals("ALL") || cell.Cellgs.Equals(cellgs))
                        {
                            if (isdouhao) { sqlBuilder.Append(","); }
                            if (cell.CellValueType.ToUpper().Equals("STRING"))
                            {
                                sqlBuilder.Append("'" + cell.CellValue + "'");
                            }
                            else
                            {
                                sqlBuilder.Append(cell.CellValue);
                            }
                            isdouhao = true;
                        }
                    }

                    if ((i > 0 && i % 1000 == 0))
                    {
                        sqlBuilder.AppendLine(");");

                        sqlfileBuilder.Append(sqlBuilder.ToString());
                        sqlBuilder = new StringBuilder();
                        cellNames = new List<ExcelCell>();
                        {
                            sqlBuilder.Append("INSERT " + sheetName + " (");
                            isdouhao = false;
                            for (int k = 0; k < cells.Count; k++)
                            {
                                if (cells[k].Cellgs.Equals("ALL") || cellgs.Equals("ALL") || cells[k].Cellgs.Equals(cellgs))
                                {
                                    cellNames.Add(cells[k]);
                                    if (isdouhao) { sqlBuilder.Append(", "); }
                                    sqlBuilder.Append("`" + cells[k].CellName + "`");
                                    isdouhao = true;
                                }
                                if (k == cells.Count - 1)
                                {
                                    sqlBuilder.AppendLine(")");
                                    sqlBuilder.AppendLine(" VALUES ");
                                }
                            }
                        }
                    }
                    else if (i == rows.Length - 1)
                    {
                        sqlBuilder.AppendLine(");");
                        sqlfileBuilder.Append(sqlBuilder.ToString());
                    }
                    else
                    {
                        sqlBuilder.AppendLine("),");
                    }
                }
                builder.Append(sqlfileBuilder.ToString());
                //CreateClassFile(fileName + "_Data", sqlfileBuilder.ToString());
            }
            return true;
        }
    }
}
