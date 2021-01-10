using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommunicationServer.Util.SysParam;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class Device
    {
        /// <summary>
        /// 设备序号(从1开始，逐个递增)
        /// </summary>
        public int SerialNum { get; set; }

        /// <summary>
        /// 当前设备是否被选中
        /// </summary>
        public bool CheckState { get; set; }

        /// <summary>
        /// 连接的命令SessionID
        /// </summary>
        public string CommandSessionId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public DeviceCode DeviceCode { get; set; }

        /// <summary>
        /// 软件版本
        /// </summary>
        public string[] SWVer { get; set; }
        public string SWVers
        {
            get
            {
                string result = "";
                for (int i = 0; i < SWVer.Length; i++)
                    result += SWVer[i] + ",";
                return result.Trim(',').Trim();
            }
        }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string DevID { get; set; }

        /// <summary>
        /// 设备IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 通道个数
        /// </summary>
        public int ChnNum { get; set; }

        public Device()
        {

        }

        public Device(string devname, DeviceCode devcode, string[] swver, string devid)
        {
            this.DeviceName = devname;
            this.DeviceCode = devcode;
            this.SWVer = swver;
            this.DevID = devid;
        }
    }
}
