
/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Script
{
    /// <summary>
    /// 需要执行初始化脚步
    /// </summary>
    public interface IInitBaseScript : IBaseScript
    {
        /// <summary>
        /// 卸载脚本
        /// </summary>
        void InitScript();
    }
}
