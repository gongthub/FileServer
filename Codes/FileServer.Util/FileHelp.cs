using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileServer.Util
{
    public class FileHelp
    {
        private static object OBJLOCK = new object();

        #region 根据文件相对路径获取绝对路径 +static string GetFullPath(string path)
        /// <summary>
        /// 根据文件相对路径获取绝对路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns></returns>
        public string GetFullPath(string path)
        {
            if (HttpContext.Current != null)
            {
                if (!FileExists(path))
                {
                    path = HttpContext.Current.Server.MapPath(path);
                }
                return path;
            }
            else
            {
                return System.IO.Path.GetFullPath(path);
            }
        }
        #endregion

        #region 返回文件是否存在
        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            bool bState = false;
            bState = System.IO.File.Exists(filename);
            return bState;
        }
        #endregion

        #region 以只读方式读取文本文件
        /// <summary>
        /// 以只读方式读取文本文件
        /// </summary>
        /// <param name="filePath">文件路径及文件名</param>
        /// <returns></returns>
        public string ReadTxtFile(string filePath)
        {
            lock (OBJLOCK)
            {
                filePath = GetFullPath(filePath);
                string content = "";//返回的字符串
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                    {
                        string text = string.Empty;
                        while (!reader.EndOfStream)
                        {
                            text += reader.ReadLine();
                            content = text;
                        }
                    }
                }
                return content;
            }
        }
        #endregion

    }
}
