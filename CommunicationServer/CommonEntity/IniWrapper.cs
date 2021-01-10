using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// 读写配置信息文件类
    /// </summary>
    public class IniWrapper
    {
        // The maximal size of string used in initial file
        private static int MaxSize = 512;

        /// <summary>
        /// Write to special file
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="keyValue"></param>
        /// <param name="filePath">ini file path</param>
        public static bool Write(string section, string key, object keyValue, string filePath)
        {
            bool suc = WritePrivateProfileString(section, key, keyValue.ToString(), filePath);
            return suc;
        }

        /// <summary>
        /// Get object from special file
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static object Get(string section, string key, object defaultValue, string filePath)
        {
            return GetObject(section, key, defaultValue, filePath);
        }

        /// <summary>
        /// 从配置文件中读出值并转换为默认值提供的数据类型
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static object GetObject(string section, string key, object defaultValue, string filePath)
        {
            StringBuilder returnSB = new StringBuilder();

            returnSB.Length = MaxSize;

            long a = GetPrivateProfileString(section, key, defaultValue.ToString(), returnSB, MaxSize, filePath);


            string returnStr = returnSB.ToString();

            bool valid = true; //从ini文件中得到的值是否有效

            object objval = defaultValue; //默认值。当从ini文件中得到的值无效的时候，使用默认值

            if (defaultValue is string)
            {
                objval = returnStr;
            }
            else if (defaultValue is byte)
            {
                byte temp;

                if (byte.TryParse(returnStr, out temp))
                {
                    objval = temp;
                }
                else
                {
                    valid = false;
                }
            }
            else if (defaultValue is int)
            {
                int temp;

                if (int.TryParse(returnStr, out temp))
                {
                    objval = temp;
                }
                else
                {
                    valid = false;
                }
            }
            else if (defaultValue is long)
            {
                long temp;

                if (long.TryParse(returnStr, out temp))
                {
                    objval = temp;
                }
                else
                {
                    valid = false;
                }
            }
            else if (defaultValue is float)
            {
                float temp;

                if (float.TryParse(returnStr, out temp))
                {
                    objval = temp;
                }
                else
                {
                    valid = false;
                }
            }
            else if (defaultValue is double)
            {
                double temp;

                if (double.TryParse(returnStr, out temp))
                {
                    objval = temp;
                }
                else
                {
                    valid = false;
                }
            }
            else if (defaultValue is bool)
            {
                if (returnStr.ToLower() == "true")
                {
                    objval = true;
                }
                else if (returnStr.ToLower() == "false")
                {
                    objval = false;
                }
                else
                {
                    valid = false;
                }
            }
            else if (defaultValue is DateTime)
            {
                DateTime temp;

                if (DateTime.TryParse(returnStr, out temp))
                {
                    objval = temp;
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                objval = returnStr;
            }

            if (!valid)  //从ini文件读出的值无效，则把默认值写入ini文件
            {
                Write(section, key, defaultValue.ToString(), filePath);
            }

            return objval;
        }

        /// <summary>   
        /// 读取INI文件中指定INI文件中的所有节点名称(Section)   
        /// </summary>   
        /// <param name="iniFile">Ini文件</param>   
        /// <returns>所有节点,没有内容返回string[0]</returns>   
        public static string[] INIGetAllSectionNames(string iniFile)
        {
            uint MAX_BUFFER = 32767;    //默认为32767   

            string[] sections = new string[0];      //返回值   

            //申请内存   
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, iniFile);
            if (bytesReturned != 0)
            {
                //读取指定内存的内容   
                string local = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned).ToString();

                //每个节点之间用\0分隔,末尾有一个\0   
                sections = local.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }

            //释放内存   
            Marshal.FreeCoTaskMem(pReturnedString);

            return sections;
        }


        #region //读写INI文件的API函数

        //Write a string to initial file
        [DllImport("kernel32.dll")]
        public static extern bool WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName);

        //Get a string from initial file 
        [DllImport("kernel32.dll")]
        private static extern long GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);

        #endregion

    }
}
