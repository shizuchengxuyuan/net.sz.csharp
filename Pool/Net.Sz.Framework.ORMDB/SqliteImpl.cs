using Net.Sz.Framework.Szlog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Reflection;
using System.Text;

namespace Net.Sz.Framework.ORMDB
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public class SqliteImpl : Net.Sz.Framework.ORMDB.ISqlImpl 
    {
        private static SzLogger log = SzLogger.getLogger();

        /// <summary>
        /// 存储所有类型解析
        /// </summary>
        protected static ConcurrentDictionary<String, DBCache> ColumnAttributeMap = new ConcurrentDictionary<String, DBCache>();
        protected static ConcurrentDictionary<String, String[]> insertSqlMap = new ConcurrentDictionary<String, String[]>();
        protected static ConcurrentDictionary<String, String> updateSqlMap = new ConcurrentDictionary<String, String>();
        protected static ConcurrentDictionary<String, List<ColumnAttribute>> updateColumnMap = new ConcurrentDictionary<String, List<ColumnAttribute>>();
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected String dbUrl;
        /// <summary>
        /// 数据库名字
        /// </summary>
        protected String dbName;
        /// <summary>
        /// 数据库用户
        /// </summary>
        protected String dbUser;
        /// <summary>
        /// 数据库密码
        /// </summary>
        protected String dbPwd;
        /// <summary>
        /// 是否显示sql语句
        /// </summary>
        protected Boolean ShowSql;

        public SqliteImpl()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUrl"></param>
        /// <param name="dbName"></param>
        /// <param name="dbUser"></param>
        /// <param name="dbPwd"></param>
        /// <param name="showSql"></param>
        public SqliteImpl(String dbUrl, String dbName, String dbUser, String dbPwd, Boolean showSql)
        {
            this.dbUrl = dbUrl;
            this.dbName = dbName;
            this.dbUser = dbUser;
            this.dbPwd = dbPwd;
            this.ShowSql = showSql;

        }

        /// <summary>
        /// 获取数据库的连接
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection GetConnection()
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=" + this.dbName + ";Pooling=true;FailIfMissing=false", true);
            con.Open();
            return con;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {

        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="oClass"></param>
        /// <returns></returns>
        public String GetTableName(Type oClass)
        {
            //判断指定类型的注释是否存在于此元素上
            if (oClass.IsDefined(typeof(EntityAttribute), false))
            {
                object[] entityDBs = oClass.GetCustomAttributes(typeof(EntityAttribute), false);//拿到对应的表格注解类型
                if (entityDBs.Length > 0)
                {
                    EntityAttribute entity = (EntityAttribute)entityDBs[0];
                    if (!string.IsNullOrWhiteSpace(entity.Name))
                    {
                        return entity.Name;//返回注解中的值，也就是表名
                    }
                }
            }
            return oClass.Name;//不存在就不需要获取其表名
        }

        /// <summary>
        /// 反射获取字段信息 过滤 字段
        /// </summary>
        /// <param name="oClass"></param>
        /// <returns></returns>
        public DBCache GetProperty(Type oClass)
        {
            string tableName = this.GetTableName(oClass);

            if (ColumnAttributeMap.ContainsKey(tableName))
            {
                return ColumnAttributeMap[tableName];
            }

            DBCache dbCache = new DBCache();
            dbCache.Instance = oClass;
            dbCache.TableName = tableName;
            var members = oClass.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in members)
            {
                if (oClass.FullName.Contains("System")) { continue; }
                if (!propertyInfo.CanRead
                    || !propertyInfo.CanWrite)
                {
                    if (log.IsErrorEnabled()) log.Error("类：" + oClass.FullName + " 字段：" + propertyInfo.Name + " is transient or static or final;");
                    continue;
                }
                object[] columnDBs = propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), false);
                ColumnAttribute columnAttribute = null;
                if (columnDBs.Length > 0) { columnAttribute = (ColumnAttribute)columnDBs[0]; }
                else { columnAttribute = new ColumnAttribute(); }
                if (string.IsNullOrWhiteSpace(columnAttribute.DBType))
                {
                    switch (propertyInfo.PropertyType.Name.ToLower())
                    {
                        case "int32":
                        case "uint32":
                            columnAttribute.DBType = "int(4)";
                            break;
                        case "string":
                            if (columnAttribute.Length < 255)
                            {
                                columnAttribute.Length = 255;
                            }
                            if (columnAttribute.Length < 1000) columnAttribute.DBType = "varchar(" + columnAttribute.Length + ")";
                            else if (columnAttribute.Length < 10000) columnAttribute.DBType = "text";
                            else columnAttribute.DBType = "longtext";
                            break;
                        case "double":
                            columnAttribute.DBType = "double";
                            break;
                        case "float":
                            columnAttribute.DBType = "float";
                            break;
                        case "byte":
                            columnAttribute.DBType = "tinyint(2)";
                            break;
                        case "short":
                        case "int16":
                        case "uint16":
                            columnAttribute.DBType = "tinyint(2)";
                            break;
                        case "bool":
                        case "boolean":
                            columnAttribute.DBType = "tinyint(1)";
                            break;
                        case "long":
                        case "int64":
                        case "uint64":
                            columnAttribute.DBType = "bigint";
                            break;
                        case "decimal":
                        case "bigdecimal":
                            columnAttribute.DBType = "decimal";
                            break;
                        default:
                            columnAttribute.DBType = "longblob";
                            break;
                    }
                }
                columnAttribute.Name = propertyInfo.Name;

                if (string.IsNullOrWhiteSpace(columnAttribute.ColumnName))
                {
                    columnAttribute.ColumnName = propertyInfo.Name;
                }
                if (columnAttribute.IsP)
                {
                    dbCache.ColumnPs.Add(columnAttribute);
                    if (columnAttribute.IsAuto)
                    {
                        columnAttribute.DBType = " INTEGER PRIMARY KEY AUTOINCREMENT";
                    }
                    else
                    {
                        columnAttribute.DBType += " PRIMARY KEY";
                    }
                }
                else if (columnAttribute.IsAuto)
                {
                    columnAttribute.DBType = " INTEGER PRIMARY KEY AUTOINCREMENT";
                }

                if (columnAttribute.IsNotNull)
                {
                    columnAttribute.DBType += " NOT NULL";
                }
                else
                {
                    columnAttribute.DBType += " NULL";
                }
                columnAttribute.PropertyType = propertyInfo;
                columnAttribute.ValueType = propertyInfo.PropertyType;
                dbCache.Columns.Add(columnAttribute);
            }
            if (dbCache.Columns == null || dbCache.Columns.Count == 0 || dbCache.ColumnPs == null || dbCache.ColumnPs.Count == 0)
            {
                throw new Exception("实体类没有任何字段，" + oClass.FullName);
            }
            return dbCache;
        }

        /// <summary>
        /// 设置字段值，插入数据库，支持sql注入攻击
        /// </summary>
        /// <param name="stmt"></param>
        /// <param name="ColumnAttribute"></param>
        /// <param name="nums"></param>
        /// <param name="value"></param>
        protected void SetStmtParam(SQLiteParameterCollection stmt, ColumnAttribute ColumnAttribute, int nums, Object value)
        {
            switch (ColumnAttribute.ValueType.Name.ToLower())
            {
                case "int32":
                    if (value == null)
                    {
                        if (ColumnAttribute.IsNotNull)
                        {
                            value = 0;
                        }
                    }

                    break;
                case "uint32":
                    if (value == null)
                    {
                        if (ColumnAttribute.IsNotNull)
                        {
                            value = 0;
                        }
                    }
                    break;
                case "int16":
                case "short":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "uint16":
                case "ushort":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "long":
                case "int64":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "ulong":
                case "uint64":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "bigdecimal":
                case "decimal":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = new Decimal(0);
                        }
                    }
                    break;
                case "byte":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "double":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "float":
                case "single":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "boolean":
                case "bool":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = false;
                        }
                    }
                    break;
                case "date":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "datetime":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = 0.0;
                        }
                    }
                    break;
                case "string":
                    if (value == null)
                    {
                        if (!ColumnAttribute.IsNotNull)
                        {
                            value = "";
                        }
                    }
                    break;
                default:
                    if (value != null)
                    {
                        //stmt.setBytes(nums, ZipUtil.zipObject(value));
                        throw new Exception();
                    }
                    break;
            }
            stmt.AddWithValue(string.Empty, value);
        }

        /// <summary>
        /// 设置字段值，插入数据库，支持sql注入攻击
        /// </summary>
        /// <param name="stmt"></param>
        /// <param name="objs"></param>
        protected void SetStmtParams(SQLiteParameterCollection stmt, params Object[] objs)
        {
            if (objs != null && objs.Length > 0)
            {
                for (int j = 0; j < objs.Length; j++)
                {
                    SetStmtParam(stmt, j + 1, objs[j]);
                }
            }
        }

        /// <summary>
        /// 设置字段值，插入数据库，支持sql注入攻击
        /// </summary>
        /// <param name="stmt"></param>
        /// <param name="nums"></param>
        /// <param name="value"></param>
        protected void SetStmtParam(SQLiteParameterCollection stmt, int nums, Object value)
        {
            stmt.AddWithValue(string.Empty, value);
            //switch (value.GetType().Name.ToLower())
            //{
            //    case "int32":
            //        stmt.Add("?", DbType.Int32).Value = value;
            //        break;
            //    case "uint32":
            //        stmt.Add("?", DbType.UInt32).Value = value;
            //        break;
            //    case "int16":
            //    case "short":
            //        stmt.Add("?", DbType.Int16).Value = value;
            //        break;
            //    case "uint16":
            //    case "ushort":
            //        stmt.Add("?", DbType.UInt16).Value = value;
            //        break;
            //    case "long":
            //    case "int64":
            //        stmt.Add("?", DbType.Int64).Value = value;
            //        break;
            //    case "ulong":
            //    case "uint64":
            //        stmt.Add("?", DbType.UInt64).Value = value;
            //        break;
            //    case "bigdecimal":
            //    case "decimal":
            //        stmt.Add("?", DbType.Decimal).Value = value;
            //        break;
            //    case "byte":
            //        stmt.Add("?", DbType.Byte).Value = value;
            //        break;
            //    case "double":
            //        stmt.Add("?", DbType.Double).Value = value;
            //        break;
            //    case "float":
            //    case "single":
            //        stmt.Add("?", DbType.Single).Value = value;
            //        break;
            //    case "boolean":
            //    case "bool":
            //        stmt.Add("?", DbType.Boolean).Value = value;
            //        break;
            //    case "date":
            //        stmt.Add("?", DbType.Date).Value = value;
            //        break;
            //    case "datetime":
            //        stmt.Add("?", DbType.DateTime).Value = value;
            //        break;
            //    case "string":
            //        stmt.Add("?", DbType.String).Value = value;
            //        break;
            //    default:
            //        if (value != null)
            //        {
            //            //stmt.setBytes(nums, ZipUtil.zipObject(value));
            //            throw new Exception();
            //        }
            //        break;
            //}
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="clazzs">所有需要创建表的实体对象</param>
        public void createTable(List<Type> clazzs)
        {
            //遍历所有要创建表的对象
            foreach (var clazz in clazzs)
            {
                createTable(clazz);
            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="clazz"></param>
        public void createTable(Type clazz)
        {
            if (!clazz.IsClass)
            {
                if (log.IsErrorEnabled()) log.Error(clazz.Name + " 类无法识别实体模型", new Exception());
                return;
            }
            DBCache dbCache = this.GetProperty(clazz);
            //拿到表的所有要创建的字段名
            using (SQLiteConnection con = GetConnection())
            {
                createTable(con, dbCache);
            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="con"></param>
        /// <param name="clazz"></param>
        public void createTable(SQLiteConnection con, Type clazz)
        {
            DBCache dbCache = this.GetProperty(clazz);
            createTable(con, dbCache);
        }



        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="con"></param>
        /// <param name="cache"></param>
        protected void createTable(SQLiteConnection con, DBCache cache)
        {
            if (ExistsTable(con, cache.TableName, null))
            {
                foreach (var item in cache.Columns)
                {
                    if (!ExistsColumn(con, cache.TableName, item.ColumnName))
                    {
                        string sqls = "ALTER TABLE `" + cache.TableName + "` ADD `" + item.ColumnName + "` " + item.DBType + ";";
                        int execute1 = this.ExecuteQuery(con, sqls);
                        if (ShowSql)
                        {
                            if (log.IsErrorEnabled()) log.Error("执行语句：" + sqls + " 执行结果：" + execute1);
                        }
                    }
                    else
                    {
                        if (ShowSql)
                        {
                            if (log.IsErrorEnabled()) log.Error("表：" + cache.TableName + " 字段：" + item.Name + " 映射数据库字段：" + item.ColumnName + " 存在，将不会修改，");
                        }
                        /*   String sqls = "ALTER TABLE " + tableName + " CHANGE `" + key + "` " + value.getValue() + ";";
                                    if (showSql) {
                                        log.error("执行语句：" + sqls);
                                    }
                                    try (Statement cs1 = con.createStatement()) {
                                        boolean execute1 = cs1.execute(sqls);
                                        if (showSql) {
                                            log.error("执行结果：" + execute1);
                                        }
                                    }*/
                    }
                }
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine().Append("CREATE TABLE  if not exists ").Append(cache.TableName).AppendLine(" (");
                for (int i = 0; i < cache.Columns.Count; i++)
                {
                    var item = cache.Columns[i];
                    if (!item.IsTemp)
                    {
                        builder.Append("    `").Append(item.ColumnName).Append("` ").Append(item.DBType);
                        if (i < cache.Columns.Count - 1) { builder.AppendLine(","); }
                        else { builder.AppendLine(""); }
                    }
                }
                builder.AppendLine(");");
                string createsql = builder.ToString();
                try
                {
                    if (ShowSql)
                    {
                        if (log.IsErrorEnabled()) log.Error(createsql);
                    }
                    this.ExecuteQuery(con, createsql);
                    if (log.IsInfoEnabled()) log.Info("创建表：" + cache.TableName + " 完成");
                }
                catch (Exception e)
                {
                    if (log.IsErrorEnabled()) log.Error("创建表：" + cache.TableName + " 错误：" + createsql, e);
                    throw e;
                }
            }
        }

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Boolean ExistsTable(String tableName)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return ExistsTable(con, tableName, null);
            }
        }

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public Boolean ExistsTable<T>()
        {
            return ExistsTable<T>(false);
        }

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="isCloumn"></param>
        /// <returns></returns>
        public Boolean ExistsTable<T>(Boolean isCloumn)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return ExistsTable<T>(con, isCloumn);
            }
        }

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="con"></param>
        /// <param name="clazz"></param>
        /// <param name="isCloumn"></param>
        /// <returns></returns>
        public Boolean ExistsTable<T>(SQLiteConnection con, Boolean isCloumn)
        {
            DBCache dbCache = this.GetProperty(typeof(T));
            return ExistsTable(con, dbCache.TableName, dbCache);
        }

        private static String ifexitstable = "select sum(1) `TABLE_NAME` from sqlite_master where type ='table' and `name`= ? ;";

        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <param name="con"></param>
        /// <param name="tableName"></param>
        /// <param name="dbCache"></param>
        /// <returns></returns>
        protected Boolean ExistsTable(SQLiteConnection con, string tableName, DBCache dbCache)
        {
            if (this.GetResult<Int32>(con, ifexitstable, "TABLE_NAME", tableName) > 0)
            {
                if (ShowSql)
                {
                    if (log.IsErrorEnabled()) log.Error("表：" + tableName + " 检查结果：已存在");
                }
                if (dbCache != null && dbCache.Columns.Count > 0)
                {
                    foreach (var item in dbCache.Columns)
                    {
                        ExistsColumn(con, tableName, item.ColumnName);
                    }
                }
                return true;
            }
            if (log.IsErrorEnabled()) log.Error("表：" + tableName + " 检查结果：无此表");
            return false;
        }

        /// <summary>
        /// 检查表字段是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        protected Boolean ExistsColumn(String tableName, String columnName)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return ExistsColumn(con, tableName, columnName);
            }
        }

        /// <summary>
        /// 检查表字段是否存在
        /// </summary>
        /// <param name="con"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        protected Boolean ExistsColumn(SQLiteConnection con, String tableName, String columnName)
        {
            String ifexitscolumn = "SELECT sum(1) usm FROM sqlite_master WHERE name='" + tableName + "' AND sql like '%`" + columnName + "`%'";
            if (this.GetResult<int>(con, ifexitscolumn, "usm") > 0)
            {
                if (ShowSql)
                {
                    if (log.IsErrorEnabled()) log.Error("数据库：" + dbName + " 表：" + tableName + " 映射数据库字段：" + columnName + " 检查结果：已存在，将不会修改");
                }
                return true;
            }
            else
            {
                if (ShowSql)
                {
                    if (log.IsErrorEnabled()) log.Error("数据库：" + dbName + " 表：" + tableName + " 映射数据库字段：" + columnName + " 检查结果：无此字段 ");
                }
                return false;
            }
        }

        /**
         * 插入对象 默认以100的形式批量插入
         *
         * @param os
         * @return
         * @throws java.lang.Exception
         */
        public int insertList(List<Object> os)
        {
            return Insert(100, os.ToArray());
        }

        /**
         * 插入对象
         *
         * @param os
         * @param constCount 批量插入的量
         * @return
         * @throws java.lang.Exception
         */
        public int insertList(int constCount, List<Object> os)
        {
            return Insert(constCount, os.ToArray());
        }

        /**
         * 插入对象到数据库，默认以100的形式批量插入
         *
         * @param os os 必须是对同一个对象
         * @return
         * @throws java.lang.Exception
         */
        public int Insert(params Object[] os)
        {
            return Insert(100, os);
        }

        /// <summary>
        /// 插入对象到数据库
        /// </summary>
        /// <param name="constCount">批量插入的量</param>
        /// <param name="os">必须是对同一个对象</param>
        /// <returns></returns>
        public int Insert(int constCount, params Object[] os)
        {
            int insert = 0;
            SQLiteConnection con = null;
            DbTransaction transaction = null;
            try
            {
                con = GetConnection();
                transaction = con.BeginTransaction();
                insert = Insert(con, constCount, os);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return insert;
        }

        /// <summary>
        /// 写入数据,默认以100的形式批量插入
        /// </summary>
        /// <param name="con"></param>
        /// <param name="os"></param>
        /// <returns></returns>
        public int Insert(SQLiteConnection con, params Object[] os)
        {
            return Insert(con, 100, os);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="con"></param>
        /// <param name="constCount">批量插入的量</param>
        /// <param name="os"></param>
        /// <returns></returns>
        public int Insert(SQLiteConnection con, int constCount, params Object[] os)
        {

            int execute = 0;

            if (os == null || os.Length == 0)
            {
                return execute;
            }

            Dictionary<String, List<Object>> objMap = new Dictionary<String, List<Object>>();

            foreach (Object o in os)
            {
                //得到对象的类
                Type clazz = o.GetType();
                //获取表名
                String tableName = this.GetTableName(clazz);
                List<Object> Get = null;
                if (objMap.ContainsKey(tableName))
                {
                    Get = objMap[tableName];
                }

                if (Get == null)
                {
                    Get = new List<Object>();
                    objMap[tableName] = Get;
                }
                Get.Add(o);
            }
            foreach (var item in objMap)
            {
                List<Object> values = item.Value;
                Object objfirst = values[0];
                //得到对象的类
                Type clazz = objfirst.GetType();
                //获取表
                DBCache dbCache = this.GetProperty(clazz);
                String tableName = dbCache.TableName;
                //拿到表的所有要创建的字段名
                List<ColumnAttribute> columns = dbCache.Columns;

                if (!ExistsTable(con, tableName, null))
                {
                    createTable(con, dbCache);
                }

                String[] inserts = null;
                if (insertSqlMap.ContainsKey(tableName))
                {
                    inserts = insertSqlMap[tableName];
                }

                if (inserts == null)
                {

                    inserts = new String[2];

                    StringBuilder builder = new StringBuilder();

                    builder.Append("insert into `").Append(tableName).Append("` (");
                    int columnCount = 0;
                    //将所有的字段拼接成对应的SQL语句
                    foreach (ColumnAttribute column in dbCache.Columns)
                    {
                        if (column.IsAuto)
                        {
                            continue;
                        }
                        if (columnCount > 0)
                        {
                            builder.Append(", ");
                        }
                        builder.Append("`").Append(column.ColumnName).Append("`");
                        columnCount++;
                    }

                    builder.Append(") values \n");
                    inserts[0] = builder.ToString();
                    builder = new StringBuilder();
                    builder.Append("(");
                    for (int j = 0; j < columnCount; j++)
                    {
                        builder.Append("?");
                        if (j < columnCount - 1)
                        {
                            builder.Append(",");
                        }
                        builder.Append(" ");
                    }
                    builder.Append(")");
                    inserts[1] = builder.ToString();
                }

                int forcount = values.Count / constCount + (values.Count % constCount > 0 ? 1 : 0);

                for (int k = 0; k < forcount; k++)
                {
                    int count1 = 0;
                    int forcount1 = 0;
                    count1 = k * constCount;
                    forcount1 = constCount;
                    if (k == 0)
                    {
                        if (constCount > values.Count && count1 < values.Count)
                        {
                            forcount1 = values.Count;
                        }
                    }
                    else if (count1 + constCount >= values.Count)
                    {
                        forcount1 = values.Count - count1;
                    }

                    StringBuilder builder = new StringBuilder();
                    builder.Append(inserts[0]);
                    for (int i = 0; i < forcount1; i++)
                    {
                        if (i > 0)
                        {
                            builder.Append(",\n");
                        }
                        builder.Append(inserts[1]);
                    }

                    builder.Append(";");

                    String sqlString = builder.ToString();
                    if (ShowSql)
                    {
                        if (log.IsErrorEnabled()) log.Error("执行 " + sqlString + " 添加数据 表：" + tableName);
                    }
                    //指定DataAdapter的Insert语句  
                    try
                    {
                        using (SQLiteCommand cmd = con.CreateCommand())
                        {

                            cmd.CommandText = sqlString;
                            cmd.CommandType = CommandType.Text;

                            for (int i = 0; i < forcount1; i++)
                            {
                                int tmp = i * columns.Count;
                                int j = 1;
                                Object obj = values[count1 + i];
                                foreach (ColumnAttribute column in columns)
                                {
                                    if (column.IsAuto)
                                    {
                                        continue;
                                    }
                                    Object invoke = column.PropertyType.GetValue(obj, null);
                                    SetStmtParam(cmd.Parameters, column, tmp + j, invoke);
                                    j++;
                                }
                            }

                            execute += cmd.ExecuteNonQuery();

                            if (ShowSql)
                            {
                                if (log.IsErrorEnabled()) log.Error("执行 " + cmd.ToString() + " 添加数据 表：" + tableName + " 结果 影响行数：" + execute);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (log.IsErrorEnabled()) log.Error("执行sql语句错误：" + sqlString);
                        throw ex;
                    }
                }
            }
            return execute;
        }

        /// <summary>
        /// 获取所有集合对象
        /// </summary>
        public List<T> GetList<T>()
        {
            return GetListByWhere<T>(null);
        }

        /// <summary>
        /// 获取所有集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="con"></param>
        /// <returns></returns>
        public List<T> GetList<T>(SQLiteConnection con)
        {
            return GetListByWhere<T>(con, null);
        }

        /// <summary>
        /// 获取所有集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereSqlString">请加入where 例如：where a=? and b=? 或者 a=? or a=? 这样才能防止sql注入攻击</param>
        /// <param name="strs"></param>
        /// <returns></returns>

        public List<T> GetListByWhere<T>(String whereSqlString, params Object[] strs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return GetListByWhere<T>(con, whereSqlString, strs);
            }
        }

        /// <summary>
        /// 获取所有集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="con"></param>
        /// <param name="whereSqlString">请加入where 例如：where a=? and b=? 或者 a=? or a=? 这样才能防止sql注入攻击</param>
        /// <param name="strs"></param>
        /// <returns></returns>
        public List<T> GetListByWhere<T>(SQLiteConnection con, String whereSqlString, params Object[] strs)
        {
            List<T> ts = new List<T>();

            DBCache dbCache = this.GetProperty(typeof(T));
            //获取表
            String tableName = dbCache.TableName;
            //拿到表的所有要创建的字段名
            List<ColumnAttribute> columns = dbCache.Columns;

            DataTable rs = GetResultSet(con, tableName, columns, whereSqlString, strs);
            if (rs.Rows != null && rs.Rows.Count >= 0)
            {
                //得到对象的所有的方法
                foreach (DataRow r in rs.Rows)
                {
                    T t = GetObject<T>(r, tableName, columns);
                    ts.Add(t);
                }
            }
            return ts;
        }

        /// <summary>
        /// 根据传入的sql语句获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <param name="strs"></param>
        /// <returns></returns>
        public List<T> GetListBySql<T>(String sqlString, params Object[] strs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return GetListBySql<T>(con, sqlString, strs);
            }
        }

        /// <summary>
        /// 获取对象，根据传入的sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="con"></param>
        /// <param name="sqlString"></param>
        /// <param name="strs"></param>
        /// <returns></returns>
        public List<T> GetListBySql<T>(SQLiteConnection con, String sqlString, params Object[] strs)
        {
            List<T> ts = new List<T>();
            DBCache dbCache = this.GetProperty(typeof(T));
            //获取表名
            String tableName = dbCache.TableName;
            //拿到表的所有要创建的字段名
            List<ColumnAttribute> columns = dbCache.Columns;

            DataTable rs = GetResultSet(con, sqlString, strs);
            if (rs.Rows != null && rs.Rows.Count >= 0)
            {
                //得到对象的所有的方法
                //得到对象的所有的方法
                foreach (DataRow r in rs.Rows)
                {
                    T t = GetObject<T>(r, tableName, columns);
                    ts.Add(t);
                }
            }
            return ts;
        }

        /**
         * 返回查询结果集
         *
         * @param con
         * @param clazz
         * @param whereSqlString 例如： a=? and b=? 或者 a=? or a=? 这样才能防止sql注入攻击
         * @param strs
         * @return
         * @throws java.lang.Exception
         */
        public DataTable GetResultSet<T>(SQLiteConnection con, String whereSqlString, params Object[] strs)
        {
            DBCache dbCache = this.GetProperty(typeof(T));
            //获取表名
            String tableName = dbCache.TableName;
            //拿到表的所有要创建的字段名
            List<ColumnAttribute> columns = dbCache.Columns;

            DataTable resultSet = GetResultSet(con, tableName, columns, whereSqlString, strs);

            return resultSet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="sqlWhere">请加入where 范例 a=? and b=? 或者 a=? or a=?</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        protected DataTable GetResultSet(SQLiteConnection con, String tableName, List<ColumnAttribute> columns, String sqlWhere, params Object[] objs)
        {
            //这里如果不存在字段名就不需要创建了
            if (columns == null || columns.Count == 0)
            {
                throw new Exception("实体类没有任何字段，");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ");
            int i = 0;
            foreach (ColumnAttribute value in columns)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }
                builder.Append("`").Append(value.ColumnName).Append("`");
                i++;
            }
            builder.Append(" FROM `").Append(tableName).Append("` ");
            if (sqlWhere != null && sqlWhere.Length > 0)
            {
                builder.Append(" ").Append(sqlWhere);
            }
            String sqlString = builder.ToString();
            return GetResultSet(con, sqlString, objs);
        }

        /**
         * 返回查询结果集
         *
         * @param con
         * @param sqlString 范例 a=? and b=? 或者 a=? or a=?
         * @param objs
         * @return
         * @throws java.lang.Exception
         */
        public DataTable GetResultSet(String sqlString, params Object[] objs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return GetResultSet(con, sqlString, objs);
            }
        }

        /**
         * 返回查询结果集
         *
         * @param con
         * @param sqlString 范例 a=? and b=? 或者 a=? or a=?
         * @param objs
         * @return
         * @throws java.lang.Exception
         */
        public DataTable GetResultSet(SQLiteConnection con, String sqlString, params Object[] objs)
        {
            DataTable res = new DataTable();
            try
            {
                using (SQLiteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sqlString;
                    cmd.CommandType = CommandType.Text;
                    SetStmtParams(cmd.Parameters, objs);
                    if (ShowSql)
                    {
                        if (log.IsErrorEnabled()) log.Error("执行sql语句：" + cmd.CommandText);
                    }
                    using (System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(cmd))
                    {
                        da.Fill(res);
                    }
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled()) log.Error("执行sql语句错误：" + sqlString);
                throw ex;
            }
            return res;
        }

        /**
         * 返回查询结果集
         *
         * @param <T>
         * @param sqlString 完整的sql语句
         * @param valueName 需要获取的字段的名字
         * @param clazz 获取后的类型
         * @param objs
         * @return
         * @throws java.lang.Exception
         */
        public T GetResult<T>(String sqlString, String valueName, params Object[] objs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return GetResult<T>(con, sqlString, valueName, objs);
            }
        }

        /// <summary>
        /// 返回查询结果集
        /// </summary>
        /// <typeparam name="T">获取后的类型</typeparam>
        /// <param name="con"></param>
        /// <param name="sqlString">完整的sql语句</param>
        /// <param name="valueName">需要获取的字段的名字</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public T GetResult<T>(SQLiteConnection con, String sqlString, String valueName, params Object[] objs)
        {
            DataTable table = this.GetResultSet(con, sqlString, objs);
            if (table.Rows != null && table.Rows.Count > 0)
            {
                object obj = this.GetResultValue(table.Rows[0], valueName, typeof(T));
                if (obj != null)
                {
                    return (T)Convert.ChangeType(obj, typeof(T));
                }
            }
            return default(T);
        }

        /// <summary>
        /// 返回查询结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">范例 a=? and b=? 或者 a=? or a=?</param>
        /// <param name="valueName">需要获取的字段的名字</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public List<T> GetResults<T>(String sqlString, String valueName, params Object[] objs)
        {
            List<T> objects = new List<T>();
            using (SQLiteConnection con = GetConnection())
            {
                DataTable table = this.GetResultSet(con, sqlString, objs);
                if (table.Rows != null && table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        objects.Add((T)this.GetResultValue(row, valueName, typeof(T)));
                    }
                }
            }
            if (ShowSql)
            {
                if (log.IsErrorEnabled()) log.Error("执行：" + sqlString + " 影响行数：" + objects.Count);
            }
            return objects;
        }

        /// <summary>
        /// 获取查询出来的第一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetObject<T>()
        {
            return GetObjectByWhere<T>(null);
        }

        /// <summary>
        /// 返回结果对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlWhere"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public T GetObjectByWhere<T>(String sqlWhere, params Object[] objs)
        {
            T t = default(T);
            DBCache dbcache = this.GetProperty(typeof(T));
            //获取表名
            String tableName = dbcache.TableName;
            //拿到表的所有要创建的字段名
            List<ColumnAttribute> columns = dbcache.Columns;

            DataTable resultSet = null;

            using (SQLiteConnection con = GetConnection())
            {
                resultSet = GetResultSet(con, tableName, columns, sqlWhere, objs);
                if (resultSet.Rows != null && resultSet.Rows.Count > 0)
                {
                    DataRow row = resultSet.Rows[0];
                    t = GetObject<T>(row, tableName, columns);
                }
            }
            return t;
        }

        /// <summary>
        /// 如果结果是多条，只返回第一条结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString"></param>
        /// <param name="objs">sql语句的参数 为了防止sql注入攻击</param>
        /// <returns></returns>
        public T GetObjectBySql<T>(String sqlString, params Object[] objs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return GetObjectBySql<T>(con, sqlString, objs);
            }
        }

        /**
         * 如果结果是多条，只返回第一条结果
         *
         * @param <T>
         * @param con
         * @param clazz
         * @param sqlString
         * @param objs sql语句的参数 为了防止sql注入攻击
         * @return
         * @throws Exception
         */
        public T GetObjectBySql<T>(SQLiteConnection con, String sqlString, params Object[] objs)
        {
            T t = default(T);
            DBCache dbcache = this.GetProperty(typeof(T));
            String tableName = dbcache.TableName;
            //拿到表的所有要创建的字段名
            List<ColumnAttribute> columns = dbcache.Columns;

            DataTable resultSet = null;

            resultSet = GetResultSet(con, sqlString, objs);

            if (resultSet.Rows != null && resultSet.Rows.Count > 0)
            {
                DataRow row = resultSet.Rows[0];
                t = GetObject<T>(row, tableName, columns);
            }

            return t;
        }

        /// <summary>
        /// 返回结果对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="con"></param>
        /// <param name="sqlWhere"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public T GetObjectByWhere<T>(SQLiteConnection con, String sqlWhere, params Object[] objs)
        {
            T t = default(T);
            DBCache dbcache = this.GetProperty(typeof(T));
            String tableName = dbcache.TableName;
            //拿到表的所有要创建的字段名
            List<ColumnAttribute> columns = dbcache.Columns;

            DataTable resultSet = GetResultSet(con, tableName, columns, sqlWhere, objs);

            if (resultSet.Rows != null && resultSet.Rows.Count > 0)
            {
                DataRow row = resultSet.Rows[0];
                t = GetObject<T>(row, tableName, columns);
            }
            return t;
        }

        /// <summary>
        /// 返回结果对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        public T GetObject<T>(params Object[] objs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return GetObject<T>(con, objs);
            }
        }

        /// <summary>
        /// 返回结果对象
        /// </summary>
        public T GetObject<T>(SQLiteConnection con, params Object[] objs)
        {
            T t = default(T);
            DBCache dbcache = this.GetProperty(typeof(T));
            String tableName = dbcache.TableName;
            //拿到表的所有要创建的字段名
            List<ColumnAttribute> columns = dbcache.Columns;

            String sqlString = null;
            if (objs != null && objs.Length > 0)
            {
                StringBuilder builder = new StringBuilder();
                int i = 0;
                builder.Append(" where ");
                foreach (ColumnAttribute value in columns)
                {
                    if (value.IsP)
                    {
                        if (i > 0)
                        {
                            builder.Append(", ");
                        }
                        builder.Append("`").Append(value.ColumnName).Append("`= ?");
                        i++;
                    }
                }
                sqlString = builder.ToString();
            }
            DataTable resultSet = GetResultSet(con, tableName, columns, sqlString, objs);
            if (resultSet.Rows != null && resultSet.Rows.Count > 0)
            {
                DataRow row = resultSet.Rows[0];
                t = GetObject<T>(row, tableName, columns);
            }
            return t;
        }

        /// <summary>
        /// 返回结果对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rs"></param>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected T GetObject<T>(DataRow rs, String tableName, List<ColumnAttribute> columns)
        {
            /* 生成一个实例 */
            T t = (T)(typeof(T).GetConstructor(System.Type.EmptyTypes).Invoke(null)); //(T)Assembly.GetExecutingAssembly().CreateInstance(typeof(T).FullName);
            foreach (ColumnAttribute column in columns)
            {
                Object objValue = GetResultValue(rs, column.ColumnName, column.ValueType);
                if (objValue != null)
                {
                    //赋值
                    column.PropertyType.SetValue(t, objValue, null);
                }
            }
            return t;
        }

        /// <summary>
        /// 获取一个已经返回的结果集的值
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="columnName"></param>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public Object GetResultValue(DataRow rs, String columnName, Type calzz)
        {
            try
            {
                object obj = rs[columnName];
                return Convert.ChangeType(obj, calzz);
            }
            catch (Exception) { }
            return null;
        }

        /// <summary>
        /// 获取一个已经返回的结果集的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public Object GetResultValue(Object obj, String tableName, String columnName, Type columnType)
        {
            if (obj == null)
            {
                return obj;
            }
            String toLowerCase = columnType.Name.ToLower();
            try
            {
                switch (toLowerCase)
                {
                    case "int32":
                        if (string.IsNullOrWhiteSpace(obj.ToString()))
                            obj = 0;
                        else
                            obj = (Int32)(obj);
                        break;
                    case "string":
                        obj = obj.ToString();
                        break;
                    case "double":
                        if (string.IsNullOrWhiteSpace(obj.ToString()))
                            obj = 0;
                        else
                            obj = (double)(obj);
                        break;
                    case "float":
                        if (string.IsNullOrWhiteSpace(obj.ToString()))
                            obj = 0;
                        else
                            obj = (float)(obj);
                        break;
                    case "long":
                    case "int64":
                        if (string.IsNullOrWhiteSpace(obj.ToString()))
                            obj = 0;
                        else
                            obj = (long)(obj);
                        break;
                    case "decimal":
                    case "bigdecimal":
                        if (string.IsNullOrWhiteSpace(obj.ToString()))
                            obj = new Decimal(0);
                        else
                            obj = (decimal)(obj);
                        break;
                    case "byte":
                        if (string.IsNullOrWhiteSpace(obj.ToString()))
                            obj = 0;
                        else
                            obj = (byte)(obj);
                        break;
                    case "short":
                    case "int16":
                        if (string.IsNullOrWhiteSpace(obj.ToString()))
                            obj = 0;
                        else
                            obj = (Int16)(obj);
                        break;
                    case "bool":
                    case "boolean":
                        if (string.IsNullOrWhiteSpace(obj.ToString()))
                            obj = false;
                        else
                            obj = (bool)(obj);
                        break;
                    case "datetime":
                        obj = (DateTime)obj;
                        break;
                    default:
                        {
                            byte[] bytes = (byte[])obj;
                            //obj = ZipUtil.unZipObject(bytes);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled()) log.Error("加载表：" + tableName + " 字段：" + columnName + " 字段类型：" + toLowerCase + " 数据库配置值：" + obj, e);
            }
            return obj;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="os"></param>
        /// <returns></returns>
        public int UpdateList(List<Object> os)
        {
            return Update(os.ToArray());
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int Update(params Object[] objs)
        {
            int update = 0;
            SQLiteConnection con = null;
            SQLiteTransaction tran = null;
            try
            {
                con = GetConnection();
                tran = con.BeginTransaction();
                update = Update(con, objs);
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (tran != null)
                    {
                        tran.Dispose();
                    }
                    con.Close();
                    con.Dispose();
                }
            }
            return update;
        }

        /**
         * 更新数据
         *
         * @param con
         * @param objs 数据结构体不一定需要一样的
         * @return
         * @throws java.lang.Exception
         */
        public int Update(SQLiteConnection con, params Object[] objs)
        {
            if (objs == null || objs.Length == 0)
            {
                throw new Exception("objs is null");
            }
            int executeUpdate = 0;
            foreach (Object obj in objs)
            {
                if (obj == null)
                {
                    throw new Exception("obj is null");
                }
                List<ColumnAttribute> values = null;
                DBCache dbcache = this.GetProperty(obj.GetType());
                String tableName = dbcache.TableName;
                List<ColumnAttribute> columns = dbcache.Columns;
                int i = 0;
                String updateSql = null;
                if (updateSqlMap.ContainsKey(tableName))
                {
                    updateSql = updateSqlMap[tableName];
                }
                if (updateColumnMap.ContainsKey(tableName))
                {
                    values = updateColumnMap[tableName];
                }

                if (updateSql == null)
                {
                    values = new List<ColumnAttribute>();
                    StringBuilder builder = new StringBuilder();
                    builder.Append("update `").Append(tableName).Append("` set");
                    foreach (ColumnAttribute column in columns)
                    {
                        if (column.IsP || column.IsAuto)
                        {
                            continue;
                        }
                        if (i > 0)
                        {
                            builder.Append(",");
                        }
                        /* 不是主键 */
                        builder.Append(" `").Append(column.ColumnName).Append("`= ?");
                        values.Add(column);
                        i++;
                    }
                    i = 0;
                    foreach (ColumnAttribute column in columns)
                    {
                        if (column.IsP)
                        {
                            if (i == 0)
                            {
                                builder.Append(" where ");
                            }
                            else
                            {
                                builder.Append(" and ");
                            }
                            Object invoke = column.PropertyType.GetValue(obj, null);
                            KeyValuePair<Object, ColumnAttribute> value = new KeyValuePair<Object, ColumnAttribute>(invoke, column);
                            values.Add(column);

                            /* 不是主键 */
                            builder.Append(" `").Append(column.ColumnName).Append("`= ? ");
                            i++;
                        }
                    }
                    builder.Append(";\n");
                    updateSql = builder.ToString();
                    updateSqlMap[tableName] = updateSql;
                    updateColumnMap[tableName] = values;
                }
                if (ShowSql)
                {
                    if (log.IsErrorEnabled()) log.Error(updateSql);
                }
                try
                {
                    i = 1;
                    using (SQLiteCommand cmd = con.CreateCommand())
                    {

                        cmd.CommandText = updateSql;
                        cmd.CommandType = CommandType.Text;

                        foreach (ColumnAttribute ColumnAttribute in values)
                        {
                            Object invoke = ColumnAttribute.PropertyType.GetValue(obj, null);
                            SetStmtParam(cmd.Parameters, ColumnAttribute, i, invoke);
                            i++;
                        }

                        DataTable res = new DataTable();

                        using (System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(cmd))
                        {
                            da.Fill(res);
                        }
                        int exec = cmd.ExecuteNonQuery();

                        if (ShowSql)
                        {
                            if (log.IsErrorEnabled()) log.Error(cmd.ToString() + " 执行结果：" + executeUpdate);
                        }

                        executeUpdate += exec;

                    }
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled()) log.Error("执行sql语句错误：" + updateSql, ex);
                    throw ex;
                }
            }
            return executeUpdate;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int ExecuteQuery(String sql, params Object[] objs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return ExecuteQuery(con, sql, objs);
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="con"></param>
        /// <param name="sql"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int ExecuteQuery(SQLiteConnection con, String sql, params Object[] objs)
        {
            try
            {
                using (SQLiteCommand cmd = con.CreateCommand())
                {

                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    SetStmtParams(cmd.Parameters, objs);

                    int executeUpdate = cmd.ExecuteNonQuery();

                    if (ShowSql)
                    {
                        if (log.IsErrorEnabled()) log.Error(cmd.ToString() + " 执行结果：" + executeUpdate);
                    }
                    return executeUpdate;
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled()) log.Error("执行sql语句错误：" + sql, ex);
                throw ex;
            }
        }

        /**
         * 删除数据
         *
         * @param clazz
         * @return
         * @throws java.lang.Exception
         */
        public int delete<T>()
        {
            return deleteByWhere<T>(null);
        }


        /**
         * 删除行
         *
         * @param clazz
         * @param sqlWhere 请加入where
         * @param objs
         * @return
         * @throws java.lang.Exception
         */
        public int deleteByWhere<T>(String sqlWhere, params Object[] objs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return deleteByWhere<T>(con, sqlWhere, objs);
            }
        }

        /**
         * 删除行
         *
         * @param con
         * @param clazz
         * @param sqlWhere 请加入where
         * @param objs
         * @return
         * @throws java.lang.Exception
         */
        public int deleteByWhere<T>(SQLiteConnection con, String sqlWhere, params Object[] objs)
        {
            Type clazz = typeof(T);
            StringBuilder builder = new StringBuilder();
            String tableName = GetTableName(clazz);
            builder.Append("DELETE FROM `").Append(tableName).Append("`");
            if (!string.IsNullOrWhiteSpace(sqlWhere))
            {
                builder.Append(sqlWhere);
            }
            return ExecuteQuery(con, builder.ToString(), objs);
        }

        /**
         * 删除行
         *
         * @param clazz 需要删除的表结构
         * @param objs 主键字段的值
         * @return
         * @throws java.lang.Exception
         */
        public int delete<T>(params Object[] objs)
        {
            using (SQLiteConnection con = GetConnection())
            {
                return delete<T>(con, objs);
            }
        }

        /**
         * 删除 多条执行总影响行数
         *
         * @param objs
         * @return
         * @throws java.lang.Exception
         */
        public int deleteList(List<Object> objs)
        {
            return delete(objs.ToArray());
        }

        /**
         * 删除 多条执行总影响行数
         *
         * @param objs
         * @return
         * @throws java.lang.Exception
         */
        public int delete(params Object[] objs)
        {
            int del;
            SQLiteConnection con = null;
            SQLiteTransaction tran = null;
            try
            {
                con = GetConnection();
                tran = con.BeginTransaction();
                del = delete(con, objs);
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (tran != null)
                    {
                        tran.Dispose();
                    }
                    con.Close();
                    con.Dispose();
                }
            }
            return del;
        }

        /// <summary>
        /// 删除 多条执行总影响行数
        /// </summary>
        /// <param name="con"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int delete(SQLiteConnection con, params Object[] objs)
        {
            int count = 0;
            foreach (Object obj in objs)
            {
                StringBuilder builder = new StringBuilder();
                DBCache dbcache = this.GetProperty(obj.GetType());
                List<Object> values = new List<Object>();
                builder.Append("DELETE FROM `").Append(dbcache.TableName).Append("`").Append(" WHERE ");
                int i = 0;
                foreach (ColumnAttribute column in dbcache.Columns)
                {
                    if (column.IsP)
                    {
                        if (i > 0)
                        {
                            builder.Append(" and ");
                        }
                        builder.Append(column.ColumnName).Append("= ?");
                        Object invoke = column.PropertyType.GetValue(obj, null);
                        values.Add(invoke);
                        i++;
                    }
                }
                builder.Append(";");
                count += ExecuteQuery(con, builder.ToString(), values.ToArray());
            }
            return count;
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="con"></param>
        /// <param name="clazz"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int delete<T>(SQLiteConnection con, params Object[] objs)
        {
            StringBuilder builder = new StringBuilder();
            DBCache dbcache = this.GetProperty(typeof(T));
            String tableName = dbcache.TableName;
            List<ColumnAttribute> columns = dbcache.Columns;
            builder.Append("delete from `").Append(tableName).Append("`");
            int i = 0;
            if (objs != null && objs.Length > 0)
            {
                builder.Append(" WHERE ");
                foreach (ColumnAttribute column in columns)
                {
                    if (column.IsP)
                    {
                        if (i > 0)
                        {
                            builder.Append(" and ");
                        }
                        builder.Append("`").Append(column.ColumnName).Append("`").Append("= ? ");
                        i++;
                    }
                }
                builder.Append(";");
            }
            return ExecuteQuery(con, builder.ToString(), objs);
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int dropTable(Object obj)
        {
            return dropTable(obj.GetType());
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="clazz"></param>
        /// <returns></returns>
        public int dropTable(Type clazz)
        {
            StringBuilder builder = new StringBuilder();
            String tableName = GetTableName(clazz);
            builder.Append("DROP TABLE IF EXISTS `").Append(tableName).Append("`;");
            return ExecuteQuery(builder.ToString());
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public int DropDatabase(String database)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("DROP DATABASE IF EXISTS `").Append(database).Append("`;");
            return ExecuteQuery(builder.ToString());
        }

        /**
         * 创建数据库 , 吃方法创建数据库后会自动使用 use 语句
         *
         * @param database
         * @return
         * @throws java.lang.Exception
         */
        public int CreateDatabase(String database)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("CREATE DATABASE IF NOT EXISTS `").Append(database).Append("` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;");
            int exec = ExecuteQuery(builder.ToString());
            builder = new StringBuilder();
            builder.Append("use `").Append(database).Append("`;");
            ExecuteQuery(builder.ToString());
            return exec;
        }

    }
}
