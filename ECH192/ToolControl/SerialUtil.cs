using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.ToolControl
{
    public class SerialUtil
    {
        /// <summary>
        /// 串口接收指定长度数据
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="receivebytes"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool ReadSerial(SerialPort sp, byte[] receivebytes, int length)
        {
            try
            {
                int cnt = length;                    //接收数据长度
                int pos = 0;                         //每次接收数据的起始位置
                int num = 0;                         //每次实际接收到的数据长度

                int zeroTimes = 0;                   //接收到数据长度为0的次数

                while (cnt > 0)
                {
                    num = sp.Read(receivebytes, pos, cnt);

                    pos += num;
                    cnt -= num;
                    if (num == 0)
                    { zeroTimes++; }
                    if (zeroTimes > 100)
                    { return false; }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 向端口发送指定的数据
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="sendbytes"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool WriteSerial(SerialPort sp, byte[] sendbytes, int length)
        {
            try
            {
                if (sp.IsOpen)
                {
                    sp.DiscardInBuffer();
                    sp.Write(sendbytes, 0, length);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
