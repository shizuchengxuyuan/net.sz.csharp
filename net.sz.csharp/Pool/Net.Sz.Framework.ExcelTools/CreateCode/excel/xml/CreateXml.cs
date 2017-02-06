using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.CreateCode.excel.xml
{
    public class CreateXml : CreateBase
    {
        static readonly CreateXml instance = new CreateXml();
        public static CreateXml Instance()
        {
            return instance;
        }


        void CreateJavaXml(string fileName, string msg)
        {
            base.CreateClassFile(JavaPathXml + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH") + "/" + fileName + ".xml", msg);
        }

        void CreateCSharpXml(string fileName, string msg)
        {
            if (!System.IO.Directory.Exists(CSharpPathXml))
            {
                System.IO.Directory.CreateDirectory(CSharpPathXml);
            }
            base.CreateClassFile(CSharpPathXml + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH") + "/" + fileName + ".xml", msg);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceldatas"></param>
        /// <param name="cellgs"></param>
        /// <param name="is_Not_Null">是否过滤空字符</param>
        /// <param name="show"></param>
        public void ActionExcelData(FileExcelDatas exceldatas, string cellgs, bool is_Not_Null, Action<string> show)
        {
            Dictionary<string, ExcelDatas> datas = exceldatas.Datas;
            foreach (var file in datas)
            {
                StringBuilder notesBuilder = new StringBuilder();
                string notes = "";
                List<string> snames = new List<string>();
                if (!GetNotes(datas, file.Key, ref snames, ref notesBuilder, show)) return;
                while (snames.Count > 0)
                {
                    notesBuilder.AppendLine(notes);
                    List<string> names = new List<string>();
                    foreach (var item in snames)
                    {
                        if (!GetNotes(datas, item, ref names, ref notesBuilder, show)) return;
                    }
                    snames.Clear();
                    snames.AddRange(names);
                }
                {
                    StringBuilder xmlBuilder = new StringBuilder();
                    string fileName = file.Key;
                    string SheetName = file.Value.SheetName;
                    xmlBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    xmlBuilder.Append(notesBuilder.ToString());
                    xmlBuilder.AppendLine("<" + SheetName + "Manager>");
                    foreach (var row in file.Value.Rows)
                    {
                        xmlBuilder.AppendLine(ExcelDataToXml("  ", SheetName, cellgs, row.Value, exceldatas, datas, is_Not_Null, false));
                    }
                    xmlBuilder.AppendLine("</" + SheetName + "Manager>");
                    show("创建 XML文件数据 共 " + file.Value.Rows.Count + " 行");
                    show("生成 CSharp XML文件：" + fileName + ".xml 文件目录：" + CSharpPathXml);
                    CreateCSharpXml(fileName, xmlBuilder.ToString());
                }
                {
                    StringBuilder xmlBuilder = new StringBuilder();
                    string fileName = file.Key;
                    string SheetName = file.Value.SheetName;
                    xmlBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    xmlBuilder.Append(notesBuilder.ToString());
                    xmlBuilder.AppendLine("<" + SheetName + "Manager>");
                    xmlBuilder.AppendLine("  <" + SheetName + "s>");
                    foreach (var row in file.Value.Rows)
                    {
                        xmlBuilder.AppendLine(ExcelDataToXml("    ", SheetName, cellgs, row.Value, exceldatas, datas, is_Not_Null, true));
                    }
                    xmlBuilder.AppendLine("  </" + SheetName + "s>");
                    xmlBuilder.AppendLine("</" + SheetName + "Manager>");
                    show("创建 XML文件数据 共 " + file.Value.Rows.Count + " 行");
                    show("生成 Java XML文件：" + fileName + ".xml 文件目录：" + JavaPathXml);
                    CreateJavaXml(fileName, xmlBuilder.ToString());
                }
            }
        }

        bool GetNotes(Dictionary<string, ExcelDatas> datas, string fileName, ref List<string> snames, ref StringBuilder notesBuilder, Action<string> show)
        {
            if (!datas.ContainsKey(fileName))
            {
                show("");
                show("无法找到字段关联文件：" + fileName);
                show("");
                return false;
            }
            var file = datas[fileName];

            notesBuilder.AppendLine()
                .AppendLine("<!-- * @Create Code Troy.Chen -->")
                .AppendLine("<!-- * @Phone 13882122019 -->")
                .AppendLine("<!-- * @email 492794628@qq.com -->")
                .AppendLine("<!-- * " + (DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:sss") + " -->"));
            var rows = file.Rows.Values.ToArray();
            notesBuilder.AppendLine();
            notesBuilder.AppendLine("<!-- " + file.SheetName + " -->");
            notesBuilder.AppendLine();
            if (rows.Length > 0)
            {
                var cells = rows[0].Cells;
                for (int k = 0; k < cells.Count; k++)
                {
                    string tempstring = "";
                    if (!string.IsNullOrWhiteSpace(cells[k].CellFileName))
                    {
                        snames.Add(cells[k].CellFileName);
                        tempstring = "    关联文件：" + cells[k].CellFileName;
                    }
                    notesBuilder.AppendLine("<!-- " + cells[k].CellName + "    " + cells[k].CellNotes + tempstring + " -->");
                }
            }
            return true;
        }

        string ExcelDataToXml(string spces, string keyname, string cellgs, ExcelRow model, FileExcelDatas exceldatas, Dictionary<string, ExcelDatas> datas, bool is_Not_Null, bool isJava)
        {
            StringBuilder xmlBuilder = new StringBuilder();
            bool isChild = model.Cells.Where(l => !string.IsNullOrWhiteSpace(l.CellFileName) && !string.IsNullOrWhiteSpace(l.CellValue) && (!l.CellValue.Equals("0"))).Count() > 0;
            if (isChild)
            {
                xmlBuilder.AppendLine(spces + "<" + keyname + model.ToXmlString(cellgs, is_Not_Null) + ">");
                foreach (var cell in model.Cells)
                {
                    string dataKey = cell.CellFileName;
                    if (cell.Cellgs.Equals("ALL") || cell.Cellgs.Equals(cellgs))
                    {
                        if (!string.IsNullOrWhiteSpace(dataKey) && datas.ContainsKey(dataKey))
                        {
                            string[] keys = cell.CellValue.Split(',');
                            string sname = datas[cell.CellFileName].SheetName;
                            if (isJava)
                            {
                                xmlBuilder.AppendLine(spces + "  <" + sname + "s>");
                            }
                            foreach (var keyID in keys)
                            {
                                if (!keyID.Equals("0"))
                                {
                                    ExcelRow _cellModel = null;
                                    if (!datas[cell.CellFileName].Rows.ContainsKey(keyID))
                                    {
                                        if (exceldatas.FileName_And_SheetName.ContainsKey(cell.CellFileName))
                                        {
                                            foreach (var item in exceldatas.FileName_And_SheetName[cell.CellFileName])
                                            {
                                                if (exceldatas.Datas[item].Rows.ContainsKey(keyID))
                                                {
                                                    _cellModel = exceldatas.Datas[item].Rows[keyID];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _cellModel = datas[cell.CellFileName].Rows[keyID];
                                    }
                                    if (_cellModel != null)
                                    {
                                        if (isJava)
                                        {
                                            xmlBuilder.AppendLine(ExcelDataToXml(spces + "    ", sname, cellgs, _cellModel, exceldatas, datas, is_Not_Null, isJava));
                                        }
                                        else
                                        {
                                            xmlBuilder.AppendLine(ExcelDataToXml(spces + "  ", sname, cellgs, _cellModel, exceldatas, datas, is_Not_Null, isJava));
                                        }
                                    }
                                }
                            }
                            if (isJava)
                            {
                                xmlBuilder.AppendLine(spces + "  </" + sname + "s>");
                            }
                        }
                    }
                }
                xmlBuilder.Append(spces + "</" + keyname + ">");
            }
            else
            {
                string tmp = model.ToXmlString(cellgs, is_Not_Null);
                //if (!string.IsNullOrWhiteSpace(tmp))
                {
                    xmlBuilder.Append(spces + "<" + keyname + tmp + " />");
                }
            }
            return xmlBuilder.ToString();
        }
    }
}
