using System;
namespace Net.Sz.Framework.ORMDB
{
    interface ISqlImpl
    {
        void Close();
        int CreateDatabase(string database);
        void createTable(System.Collections.Generic.List<Type> clazzs);
        void createTable(System.Data.SQLite.SQLiteConnection con, Type clazz);
        void createTable(Type clazz);
        int delete(params object[] objs);
        int delete(System.Data.SQLite.SQLiteConnection con, params object[] objs);
        int delete<T>();
        int delete<T>(params object[] objs);
        int delete<T>(System.Data.SQLite.SQLiteConnection con, params object[] objs);
        int deleteByWhere<T>(System.Data.SQLite.SQLiteConnection con, string sqlWhere, params object[] objs);
        int deleteByWhere<T>(string sqlWhere, params object[] objs);
        int deleteList(System.Collections.Generic.List<object> objs);
        int DropDatabase(string database);
        int dropTable(object obj);
        int dropTable(Type clazz);
        int ExecuteQuery(System.Data.SQLite.SQLiteConnection con, string sql, params object[] objs);
        int ExecuteQuery(string sql, params object[] objs);
        bool ExistsTable(string tableName);
        bool ExistsTable<T>();
        bool ExistsTable<T>(bool isCloumn);
        bool ExistsTable<T>(System.Data.SQLite.SQLiteConnection con, bool isCloumn);
        System.Data.SQLite.SQLiteConnection GetConnection();
        System.Collections.Generic.List<T> GetList<T>();
        System.Collections.Generic.List<T> GetList<T>(System.Data.SQLite.SQLiteConnection con);
        System.Collections.Generic.List<T> GetListBySql<T>(System.Data.SQLite.SQLiteConnection con, string sqlString, params object[] strs);
        System.Collections.Generic.List<T> GetListBySql<T>(string sqlString, params object[] strs);
        System.Collections.Generic.List<T> GetListByWhere<T>(System.Data.SQLite.SQLiteConnection con, string whereSqlString, params object[] strs);
        System.Collections.Generic.List<T> GetListByWhere<T>(string whereSqlString, params object[] strs);
        T GetObject<T>();
        T GetObject<T>(params object[] objs);
        T GetObject<T>(System.Data.SQLite.SQLiteConnection con, params object[] objs);
        T GetObjectBySql<T>(System.Data.SQLite.SQLiteConnection con, string sqlString, params object[] objs);
        T GetObjectBySql<T>(string sqlString, params object[] objs);
        T GetObjectByWhere<T>(System.Data.SQLite.SQLiteConnection con, string sqlWhere, params object[] objs);
        T GetObjectByWhere<T>(string sqlWhere, params object[] objs);
        DBCache GetProperty(Type oClass);
        T GetResult<T>(System.Data.SQLite.SQLiteConnection con, string sqlString, string valueName, params object[] objs);
        T GetResult<T>(string sqlString, string valueName, params object[] objs);
        System.Collections.Generic.List<T> GetResults<T>(string sqlString, string valueName, params object[] objs);
        System.Data.DataTable GetResultSet(System.Data.SQLite.SQLiteConnection con, string sqlString, params object[] objs);
        System.Data.DataTable GetResultSet(string sqlString, params object[] objs);
        System.Data.DataTable GetResultSet<T>(System.Data.SQLite.SQLiteConnection con, string whereSqlString, params object[] strs);
        object GetResultValue(System.Data.DataRow rs, string columnName, Type calzz);
        object GetResultValue(object obj, string tableName, string columnName, Type columnType);
        string GetTableName(Type oClass);
        int Insert(params object[] os);
        int Insert(System.Data.SQLite.SQLiteConnection con, params object[] os);
        int Insert(System.Data.SQLite.SQLiteConnection con, int constCount, params object[] os);
        int Insert(int constCount, params object[] os);
        int insertList(System.Collections.Generic.List<object> os);
        int insertList(int constCount, System.Collections.Generic.List<object> os);
        int Update(params object[] objs);
        int Update(System.Data.SQLite.SQLiteConnection con, params object[] objs);
        int UpdateList(System.Collections.Generic.List<object> os);
    }
}
