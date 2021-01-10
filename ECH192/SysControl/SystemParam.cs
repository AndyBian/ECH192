using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.SysControl
{
    /// <summary>
    /// 一些常用的系统参数
    /// </summary>
    public class SystemParam
    {
        /// <summary>
        /// 系统配置文件完整路径
        /// </summary>AttenuationValue
        public static string IniFileFullName = System.Windows.Forms.Application.StartupPath + "\\ParamConfig.ini";
        public static string IniFileName = "ParamConfig.ini";

        // 测点的个数，固定为96
        public const int NodeNum = 96;

        #region 以太网参数

        //默认通信服务器IP地址
        public static string DefaultIPAddress = "192.168.0.84";

        //默认端口号（指令端口）
        public static int DefaultCommandPort = 40001;

        #endregion
    }
}
