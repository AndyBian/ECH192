using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Util
{
    public class SysParam
    {
        /// <summary>
        /// 系统配置文件完整路径
        /// </summary>AttenuationValue
        public static string IniFileFullName = System.IO.Directory.GetCurrentDirectory() + "\\ParamConfig.ini";
        public static string IniFileName = "ParamConfig.ini";

        // 通道个数，固定为192
        public const int HoleNum = 192;

        // 测试结果划分等分的个数
        public const int SplitCount = 10;

        //通信协议中除去数据之外的固定长度
        public static int ExteriorDataLength = 44;

        //通信链路
        public enum CommuType
        {
            request,
            response
        }

        //是否加密
        public enum Encryption
        {
            Unencrypted,
            Encrypted
        }

        //是否压缩
        public enum Compress
        {
            Nocompress,
            Gzip,
            Bzip
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public enum DataType
        {
            BOOL,
            STRING,
            INTEGER,
            FLOAT,
            DOUBLE,
            ARRAY
        }

        /// <summary>
        /// 返回类型
        /// </summary>
        public enum ReturnType
        {
            Normal,             //正常
            Error,              //一般性错误
            HardwareError,      //硬件错误
            ParamError          //参数错误
        }

        /// <summary>
        /// 设备类型码
        /// </summary>
        public enum DeviceCode
        {
            //ECH192设备
            ECH192 = 1
        }

        /// <summary>
        /// 设备类型码
        /// </summary>
        public enum TestType
        {
            //IT法测试
            IT,
            //CV法测试
            CV
        }
    }
}
