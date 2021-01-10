using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.ToolControl
{
    public class CRC
    {
        /// <summary>
        /// 异或和校验
        /// </summary>
        /// <param name="data">校验数据</param>
        /// <param name="temp">输出校验结果</param>
        public static byte BCC(byte[] data,int len)
        {
            int temp = 0;

            if (len > data.Length)
                len = data.Length;

            for (int index = 0; index < len; index++)
            {
                temp = temp ^ data[index];
            }

            return (byte)temp;
        }
    }
}
