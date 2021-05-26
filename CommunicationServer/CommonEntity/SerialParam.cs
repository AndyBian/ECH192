using CommunicationServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// 串口相关参数
    /// </summary>
    public class SerialParam
    {
        //串口名称
        public static string Name = IniWrapper.Get("SerialParam", "Name", "COM1", SysParam.IniFileFullName).ToString();
        //串口波特率
        public static string BaudRate = IniWrapper.Get("SerialParam", "BaudRate", "115200", SysParam.IniFileFullName).ToString();
        //串口数据位
        public static string DataBits = IniWrapper.Get("SerialParam", "DataBits", "8", SysParam.IniFileFullName).ToString();
        //串口停止位
        public static string StopBits = IniWrapper.Get("SerialParam", "StopBits", "1", SysParam.IniFileFullName).ToString();
    }
}
