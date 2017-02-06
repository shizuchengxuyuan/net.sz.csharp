using MySql.Data.MySqlClient;
using Net.Sz.Framework.ExcelTools.ini;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Net.Sz.Framework.ExcelTools.excelindb
{
    public class MySqlDB
    {
        static readonly MySqlDB instance = new MySqlDB();
        public static MySqlDB Instance()
        {
            return instance;
        }
        MySqlConnection mysqlConnection;
        MySqlCommand mySqlCommand;
        public MySqlDB()
        {
            //if (File.Exists("dbconfig.xml"))
            //{
            //    System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(DBConfigs));
            //    using (FileStream fs = new FileStream("dbconfig.xml", FileMode.Open, FileAccess.Read))
            //    {
            //        DBConfigs dbcs = (DBConfigs)xml.Deserialize(fs);
            //        SetMySqlConnection(dbcs.Configs[0]);
            //    }
            //}
        }

        DBConfig _DBC;

        public void ReMySqlConnection()
        {
            if (_DBC == null || string.IsNullOrWhiteSpace(_DBC.DBUser) || _DBC.DBUser.Equals("DBUser"))
            {
                throw new Exception("数据库配置文件错误");
            }

            String mysqlStr = "Database=" + _DBC.DBBase + ";Data Source=" + _DBC.DBPath + ";port=" + _DBC.DBPart + ";User Id=" + _DBC.DBUser + ";Password=" + _DBC.DBPwd + ";pooling=true;CharSet=utf8";
            if (mysqlConnection != null)
            {
                mysqlConnection.Close();
                mysqlConnection.Dispose();
                mysqlConnection = null;
            }
            mysqlConnection = new MySqlConnection(mysqlStr);
            mysqlConnection.Open();
            mySqlCommand = new MySqlCommand();
            mySqlCommand.Connection = mysqlConnection;
            mySqlCommand.CommandTimeout = 60 * 60;//超时1个小时
            mySqlCommand.CommandType = System.Data.CommandType.Text;//设置类型
        }

        public void Close() {
            if (mysqlConnection != null)
            {
                mysqlConnection.Close();
                mysqlConnection.Dispose();
                mysqlConnection = null;
            }
        }

        public void SetMySqlConnection(DBConfig dbc)
        {
            _DBC = dbc;
        }

        /// <summary>
        ///  执行数据库
        /// </summary>
        /// <param name="database"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            int ret = 0;
            {
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    mySqlCommand.CommandText = sql;//设置sql语句
                    ret = mySqlCommand.ExecuteNonQuery();//执行
                }
            }
            return ret;
        }
    }
}

