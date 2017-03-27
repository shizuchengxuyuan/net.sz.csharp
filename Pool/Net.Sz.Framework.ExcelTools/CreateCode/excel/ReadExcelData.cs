using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.CreateCode.excel
{
    public class ReadExcelData
    {
        static readonly ReadExcelData instance = new ReadExcelData();
        public static ReadExcelData Instance()
        {
            return instance;
        }



        /// <summary>
        /// 过滤“-”
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        String ReplaceSideLine(String a)
        {
            if (string.IsNullOrWhiteSpace(a)) { return ""; }
            else
            {
                if (a.ToLower() == "class")
                {
                    a = "Clazz";
                }
                return a.Replace("-", "_");
            }
        }

        /// <summary>
        /// 根据excel 解析数据类型
        /// </summary>
        /// <param name="title">列头</param>
        /// <param name="notes">注释</param>
        /// <param name="data">数据</param>
        /// <param name="model">返回值</param>
        /// <returns></returns>
        bool GetExcelPropertyType(ICell title, ICell notes, ICell data, ref ExcelRow model)
        {
            if (title == null || string.IsNullOrWhiteSpace(title.ToString()))
            {
                return false;
            }

            string property = title.StringCellValue;
            string[] types = property.Split('=');
            property = property.ToUpper();
            if (property.IndexOf("=HL") >= 0)
            {
                return false;
            }

            ExcelCell at = new ExcelCell();
            at.CellName = ReplaceSideLine(types[0]).TrimStart(Convert.ToChar(" ")).TrimEnd(Convert.ToChar(" "));
            at.CellNotes = notes == null ? "" : notes.ToString().Replace("\n", "").Replace("\r", "");
            //查找关联的文件名  
            if (property.IndexOf("=FF_") >= 0)
            {
                foreach (string item in types)
                {
                    if (item.ToUpper().IndexOf("FF_") == 0)
                    {
                        //表示存在关联数据
                        at.CellFileName = item.Substring(3);
                        at.CellFileName = this.ReplaceSideLine(at.CellFileName);
                        break;
                    }
                }
            }

            if (property.IndexOf("=B") >= 0)
            {
                //bool
                at.CellValueType = "Boolean";
            }
            else if (property.IndexOf("=D") >= 0)
            {
                //double
                at.CellValueType = "double";
            }
            else if (property.IndexOf("=S") >= 0)
            {
                at.CellValueType = "String";
                if (property.IndexOf("=S_") >= 0)
                {
                    foreach (string item in types)
                    {
                        if (item.ToUpper().IndexOf("S_") == 0)
                        {
                            //表示存在字符长度指定
                            string strLen = item.Substring(2);
                            int len;
                            if (int.TryParse(strLen, out len))
                            {
                                at.CellLength = len;
                            }
                            break;
                        }
                    }
                }
                if (at.CellLength == 0)
                {
                    at.CellLength = 255;
                }
            }
            else if (property.IndexOf("=FL") >= 0)
            {
                at.CellValueType = "float";
            }
            else if (property.IndexOf("=L") >= 0)
            {
                at.CellValueType = "long";
            }
            else
            {
                //int
                at.CellValueType += "int";
            }

            switch (at.CellValueType)
            {
                case "Boolean":
                    {
                        double _value = data == null ? 0 : data.NumericCellValue;
                        at.CellValue = ((int)_value).ToString();
                    }
                    break;
                case "String":
                    {
                        string value1 = "";
                        if (data != null)
                        {
                            //string value = data.ToString();
                            //byte[] strBuffers = System.Text.UTF8Encoding.Default.GetBytes(value);
                            //value1 = System.Text.UTF8Encoding.Default.GetString(strBuffers);

                            value1 = data.ToString();
                        }
                        at.CellValue = value1;

                        //at.CellValue = data == null ? "" : System.Text.UTF8Encoding.Default.GetString(System.Text.UTF8Encoding.Default.GetBytes(data.ToString()));                        
                    }
                    break;
                case "double":
                    {
                        at.CellValue = data == null ? "0.00" : data.NumericCellValue.ToString();
                    }
                    break;
                case "float":
                    {
                        at.CellValue = data == null ? "0.00" : data.NumericCellValue.ToString();
                    }
                    break;
                case "long":
                    {
                        double _value = data == null ? 0 : data.NumericCellValue;
                        at.CellValue = ((long)_value).ToString();
                    }
                    break;
                case "int":
                    {
                        double _value = data == null ? 0 : data.NumericCellValue;
                        at.CellValue = ((int)_value).ToString();
                    }
                    break;
            }

            //去除前置后置空格，去除换行符
            at.CellValue = at.CellValue.Replace("\n", "").Replace("\r", "").Trim();

            //标识主键
            if (property.IndexOf("=P") >= 0)
            {
                at.IsPKey = true;
                model.AtKey = at.CellValue;
            }

            if (property.IndexOf("=AC") >= 0)
            {
                at.Cellgs = "AC";
            }
            else if (property.IndexOf("=AS") >= 0)
            {
                at.Cellgs = "AS";
            }
            else
            {
                at.Cellgs = "ALL";
            }
            model.Cells.Add(at);
            return true;
        }

        public bool ReadExcelDataToCache(List<string> files, out FileExcelDatas exceldatas, Action<string> show, bool isHebing = false)
        {
            exceldatas = new FileExcelDatas();
            if (files == null || files.Count == 0)
            {
                return false;
            }
            Dictionary<string, HashSet<string>> filename_and_key = new Dictionary<string, HashSet<string>>();
            show("开始处理 Excel 文件");
            foreach (var excelPath in files)
            {
                ExcelDatas exceldata = new ExcelDatas();
                try
                {
                    using (FileStream fs = File.OpenRead(excelPath))   //打开myxls.xls文件
                    {
                        HSSFWorkbook wk = new HSSFWorkbook(fs);   //把xls文件中的数据写入wk中
                        if (wk.NumberOfSheets > 0)
                        {                            
                            var table = wk.GetSheetAt(0);
                            string _sheetName = this.ReplaceSideLine(table.SheetName).ToLower();
                            show("开始分析文件：" + Path.GetFileName(excelPath) + " 文件数据");
                            if (table != null)
                            {
                                if (table.LastRowNum < 2)
                                {
                                    show("文件格式是第一行是字段名，第二行是注释，第三行是数据");
                                    continue;
                                }
                                IRow rowTitle = table.GetRow(0);  //读取头
                                IRow rowNontes = table.GetRow(1);  //读取配置说明
                                for (int i = 2; i <= table.LastRowNum; i++)
                                {
                                    IRow rowData = table.GetRow(i); //读取当前行数据
                                    if (rowData != null)
                                    {
                                        // 处理如果该行为空值那么忽略
                                        // todo 考虑怎么添加
                                        for (int k = 0; k < rowTitle.LastCellNum; k++)
                                        {
                                            ICell cell2 = rowData.GetCell(k);  //当前表格
                                            if (cell2 != null && !string.IsNullOrWhiteSpace(cell2.ToString())) { goto lab_xx; }
                                        }
                                        goto lab_Next;
                                    lab_xx:
                                        ExcelRow model = new ExcelRow();
                                        for (int k = 0; k < rowTitle.LastCellNum; k++)  //LastCellNum 是当前行的总列数
                                        {
                                            try
                                            {
                                                GetExcelPropertyType(rowTitle.GetCell(k), rowNontes.GetCell(k), rowData.GetCell(k), ref model);
                                            }
                                            catch
                                            {
                                                show("文件：" + Path.GetFileName(excelPath) + " 取值错误 第 " + (i + 1) + " 行 " + " 第 " + (k + 1) + " 列 非数字类型");
                                                return false;
                                            }
                                        }
                                        if (string.IsNullOrWhiteSpace(model.AtKey))
                                        {
                                            show("文件：" + Path.GetFileName(excelPath) + " 缺少主键");
                                            return false;
                                        }
                                        if (exceldata.Rows.ContainsKey(model.AtKey))
                                        {
                                            show("文件：" + Path.GetFileName(excelPath) + " 存在重复主键 " + model.AtKey + " 第 " + (i + 1) + " 行 ");
                                            return false;
                                        }

                                        if (model.Cells.Where(l => l.IsPKey).Count() > 1)
                                        {
                                            show("文件：" + Path.GetFileName(excelPath) + " 不支持双主键设置");
                                            return false;
                                        }
                                        exceldata.Add(model.AtKey, model);
                                    }
                                }

                            lab_Next:

                                exceldata.SheetName = _sheetName;

                                string datakey = Path.GetFileNameWithoutExtension(excelPath);
                                if (!exceldatas.Datas.ContainsKey(datakey))
                                {
                                    //存储数据
                                    exceldatas.Datas.Add(datakey, exceldata);
                                }
                                else
                                {
                                    foreach (var row in exceldata.Rows)
                                    {
                                        if (exceldatas.Datas[datakey].Rows.ContainsKey(row.Key))
                                        {
                                            show("文件：" + Path.GetFileName(excelPath) + " 存在重复主键 " + row.Key);
                                            return false;
                                        }
                                        exceldatas.Datas[datakey].Rows.Add(row.Key, row.Value);
                                    }
                                }
                                {
                                    //存储模型
                                    if (!exceldatas.SheetName_And_FileName.ContainsKey(_sheetName))
                                    {
                                        exceldatas.SheetName_And_FileName.Add(_sheetName, new HashSet<string>());
                                    }

                                    exceldatas.SheetName_And_FileName[_sheetName].Add(datakey);

                                    //存储模型
                                    if (!exceldatas.FileName_And_SheetName.ContainsKey(datakey))
                                    {
                                        exceldatas.FileName_And_SheetName.Add(datakey, new HashSet<string>());
                                    }
                                    exceldatas.FileName_And_SheetName[datakey].Add(_sheetName);
                                }
                                if (!_sheetName.Equals(datakey))
                                {
                                    {
                                        //存储数据
                                        if (!exceldatas.Datas.ContainsKey(_sheetName))
                                        {
                                            exceldatas.Datas[_sheetName] = new ExcelDatas() { SheetName = _sheetName };
                                        }
                                        foreach (var row in exceldata.Rows)
                                        {
                                            if (exceldatas.Datas[_sheetName].Rows.ContainsKey(row.Key))
                                            {
                                                show("文件：" + Path.GetFileName(excelPath) + " 存在重复主键 " + row.Key);
                                                return false;
                                            }
                                            exceldatas.Datas[_sheetName].Rows.Add(row.Key, row.Value);
                                        }
                                    }
                                }

                                //{
                                //    //存储模型
                                //    if (!filename_and_key.ContainsKey(_sheetName))
                                //    {
                                //        filename_and_key.Add(_sheetName, new HashSet<string>());
                                //    }

                                //    filename_and_key[_sheetName].Add(datakey);

                                //    foreach (var itemkey in filename_and_key[_sheetName])
                                //    {
                                //        if (itemkey == datakey)
                                //        {
                                //            continue;
                                //        }
                                //        if (!exceldatas.Datas.ContainsKey(itemkey))
                                //        {
                                //            exceldatas.Datas[itemkey] = new ExcelDatas() { SheetName = _sheetName };
                                //        }
                                //        foreach (var row in exceldatas.Datas[_sheetName].Rows)
                                //        {
                                //            if (exceldatas.Datas[itemkey].Rows.ContainsKey(row.Key))
                                //            {
                                //                show("文件：" + Path.GetFileName(excelPath) + " 存在重复主键 " + row.Key);
                                //                return false;
                                //            }
                                //            exceldatas.Datas[itemkey].Rows.Add(row.Key, row.Value);
                                //        }
                                //    }
                                //}
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    show("请确保是2003或者wps创建的excel文件并且处于关闭状态：" + Path.GetFileName(excelPath));
                    return false;
                }
            }
            return true;
        }
    }
}
