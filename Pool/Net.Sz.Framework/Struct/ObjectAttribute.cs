using System;
using System.Collections.Generic;
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
namespace Net.Sz
{
    /// <summary>
    ///
    /// <para>PS:</para>
    /// <para>@author 失足程序员</para>
    /// <para>@Blog http://www.cnblogs.com/ty408/</para>
    /// <para>@mail 492794628@qq.com</para>
    /// <para>@phone 13882122019</para>
    /// </summary>
    [Serializable]
    public sealed class ObjectAttribute : System.Collections.Concurrent.ConcurrentDictionary<String, Object>
    {
        /// <summary>
        /// 如果未找到也返回 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object GetValue(String key)
        {
            Object ret;
            this.TryGetValue(key, out ret);
            return ret;
        }


        public T GetTValue<T>(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (T)ret;
            }
            return default(T);
        }

        /// <summary>
        /// 如果未找到 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String GetStringValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return ret.ToString();
            }
            return null;
        }

        /// <summary>
        /// 如果未找到也返回 0
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetintValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (int)ret;
            }
            return 0;
        }

        /// <summary>
        /// 如果未找到 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int? GetIntValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (int)ret;
            }
            return null;
        }

        /// <summary>
        /// 如果未找到也返回 0
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetlongValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (long)ret;
            }
            return 0;
        }

        /// <summary>
        /// 如果未找到 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long? GetLongValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (long)ret;
            }
            return null;
        }


        /// <summary>
        /// 如果未找到也返回 0
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetfloatValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (float)ret;
            }
            return 0;
        }

        /// <summary>
        /// 如果未找到  null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float? GetFloatValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (float)ret;
            }
            return null;
        }

        /// <summary>
        /// 如果未找到也返回 false
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetbooleanValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (bool)ret;
            }
            return false;
        }

        /// <summary>
        /// 如果未找到 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool? GetBooleanValue(String key)
        {
            Object ret = GetValue(key);
            if (ret != null)
            {
                return (bool)ret;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append("{");
            int forcount = 0;
            foreach (var item in this)
            {
                sbuilder.Append("\"").Append(item.Key).Append("\"").Append(":").Append("\"").Append(item.Value).Append("\"");
                forcount++;
                if (forcount < this.Count - 1)
                {
                    sbuilder.Append(",");
                }
            }
            sbuilder.Append("}");
            return sbuilder.ToString();
        }
    }
}
