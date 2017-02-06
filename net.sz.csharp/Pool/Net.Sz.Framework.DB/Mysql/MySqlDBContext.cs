using Net.Sz.Framework.Log;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.DB.Mysql
{
    /// <summary>
    /// 
    /// </summary>
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MysqlDbContext : DbContext
    {

        //[System.ComponentModel.DataAnnotations.Key, System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public MysqlDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

           
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); //表名为类名，不是上面带s的名字  //移除复数表名的契约
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();     //不创建EdmMetadata表  //防止黑幕交易 要不然每次都要访问 EdmMetadata这个表

        }

        public void InitializeDatabase()
        {
            try
            {
                Logger.Info("开始创建数据库");
                Logger.Info(this.Database.CreateIfNotExists() + "");
                Logger.Info("创建数据库完成");
            }
            catch (Exception ex)
            {
                Logger.Error("创建数据库失败" + ex);
            }
        }
    }
}
