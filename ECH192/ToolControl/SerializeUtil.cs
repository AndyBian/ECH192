using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.ToolControl
{
    // 序列化工具类
    public class SerializeUtil
    {
        // 将类对象保存在本地
        public static bool SaveObject(string path, object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream(@"" + path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, obj);
                stream.Close();
                return true;
                //将对象写入到本地
            }
            catch (Exception ex)
            {
                return false ;
            }

        }

        /// <summary>
        /// 从指定位置读取存储的历史文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T ReadObject<T>(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(@"" + path, FileMode.Open, FileAccess.Read, FileShare.None);
                T myObj = (T)formatter.Deserialize(stream);
                stream.Close();
                return myObj;
            }
            catch (Exception)
            {
                if (File.Exists(path))//如果文件不存在，创建文件
                {
                }
                else
                {
                    File.Create(path).Dispose();
                }
            }

            T t = default(T);
            return t;
        }
    }
}
