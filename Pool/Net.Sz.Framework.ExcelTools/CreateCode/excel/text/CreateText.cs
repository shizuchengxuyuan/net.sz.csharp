using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.CreateCode.excel.text
{
    
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class CreateText : CreateBase
    {
        static readonly CreateText instance = new CreateText();
        public static CreateText Instance()
        {
            return instance;
        }

        protected override void CreateClassFile(string fileName, string msg)
        {
            base.CreateClassFile(TextPath + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH") + "/" + fileName + ".txt", msg);
        }

        public void ActionExcelData(FileExcelDatas exceldatas, string strSplit, Action<string> show)
        {
            foreach (var datas in exceldatas.Datas)
            {
                StringBuilder txtBuilder = new StringBuilder();
                string sheetName = datas.Value.SheetName;
                string fileName = datas.Key;
                show("创建文本数据：" + sheetName);
                var rows = datas.Value.Rows.Values.ToArray();
                for (int i = 0; i < rows.Length; i++)
                {
                    var cells = rows[i].Cells;

                    for (int k = 0; k < cells.Count; k++)
                    {
                        txtBuilder.Append(cells[k].CellValue);
                        if (k < cells.Count - 1) { txtBuilder.Append(strSplit); }
                    }
                    txtBuilder.AppendLine();
                }
                show("创建文本数据：" + fileName + " 共 " + rows.Length + " 行");
                show("存储文件：" + fileName + "   目录：" + TextPath);
                CreateClassFile(fileName, txtBuilder.ToString());
            }
        }
    }
}
