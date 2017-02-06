using Net.Sz.Framework.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


/**
 * 
 * @author 失足程序员
 * @Blog http://www.cnblogs.com/ty408/
 * @mail 492794628@qq.com
 * @phone 13882122019
 * 
 */
namespace Net.Sz.Framework.Netty.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpClient
    {
        /// <summary>
        /// 向url 发送http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="method">post or get</param>
        /// <param name="postdatas">post or get data</param>
        /// <returns>url的响应</returns>
        public static string SendUrl(string url, string method, params string[] postdatas)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            request.KeepAlive = false;
            request.Accept = "*/*";
            request.ContentType = "application/x-www-form-urlencoded";
            if ("post".Equals(method.ToLower()) && !(postdatas == null || postdatas.Length == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in postdatas)
                {
                    if (i > 0) { buffer.Append("&"); }
                    else { i++; }
                    buffer.Append(key);
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                request.ContentLength = data.Length;
                //Logger.Debug("xxxxxxxxxxxxxxxxxxxxxxxx");
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

