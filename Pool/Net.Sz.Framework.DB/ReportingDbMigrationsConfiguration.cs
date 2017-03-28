using System.Data.Entity;
using System.Data.Entity.Migrations;


/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.DB
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    public sealed class ReportingDbMigrationsConfiguration<T> : DbMigrationsConfiguration<T> where T : DbContext
    {

        public static void Initializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<T, ReportingDbMigrationsConfiguration<T>>());
        }

        public ReportingDbMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;//任何Model Class的修改將會直接更新DB
            AutomaticMigrationDataLossAllowed = true;
        }

    }

}
