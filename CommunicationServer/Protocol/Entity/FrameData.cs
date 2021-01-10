using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommunicationServer.Util.SysParam;

namespace CommunicationServer.Protocol.Entity
{
    /// <summary>
    /// 指令中数据值
    /// </summary>
    public class FrameData
    {
        /// <summary>
        /// 软件版本集合
        /// </summary>
        public string[] version { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public DeviceCode devicecode { get; set; }

        /// <summary>
        /// 设备SN码
        /// </summary>
        public string sn { get; set; }

        /// <summary>
        /// 设备物理通道数
        /// </summary>
        public int chnum { get; set; }

        /// <summary>
        /// 设备IP地址
        /// </summary>
        public string localip { get; set; }

        /// <summary>
        /// 子网掩码
        /// </summary>
        public string netmask { get; set; }

        /// <summary>
        /// 网关
        /// </summary>
        public string getway { get; set; }

        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string serverip { get; set; }

        /// <summary>
        /// 系统时间
        /// 时间字符串，格式YYYYMMDDHHMMSSmmm
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 启动采集的类型，0表示IT法，1表示CV法
        /// </summary>
        public TestType type { get; set; }

        /// <summary>
        /// 是否包含自检信号
        /// </summary>
        public int selftest { get; set; }

        /// <summary>
        /// IT法采样间隔，单位ms
        /// </summary>
        public float sample_interval { get; set; }

        /// <summary>
        /// IT法设置的电压，单位uV
        /// </summary>
        public int vtg { get; set; }

        /// <summary>
        /// IT法采集点数，单位点
        /// </summary>
        public int np { get; set; }

        /// <summary>
        /// CV法设置的电压，单位uV
        /// </summary>
        public int vtg_init { get; set; }

        /// <summary>
        /// CV法静止时间，单位ms
        /// </summary>
        public int quittime { get; set; }

        /// <summary>
        /// CV法三角波最大电压，单位uV
        /// </summary>
        public int vtg_max { get; set; }

        /// <summary>
        /// CV法三角波最小电压，单位uV
        /// </summary>
        public int vtg_min { get; set; }

        /// <summary>
        /// CV法初始方向，0：正向，1：负向
        /// </summary>
        public int dir { get; set; }

        /// <summary>
        /// CV法电压扫描速率，单位mV/s
        /// </summary>
        public int vtg_rate { get; set; }

        /// <summary>
        /// CV法采样间隔
        /// </summary>
        public float vtg_sample { get; set; }

        /// <summary>
        /// CV法三角电压段数
        /// </summary>
        public int nsegs { get; set; }

        /// <summary>
        /// 跟灵敏度相关的参数配置
        /// </summary>
        public List<FrameGroup> group = new List<FrameGroup>();

        /// <summary>
        /// 使能的通道索引号
        /// </summary>
        public List<int> enabled = new List<int>();

        /// <summary>
        /// 采集序号索引
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// 二进制数据中包含的结构数据个数
        /// </summary>
        public int nitem { get; set; }

        /// <summary>
        /// 二进制数据中包含的通道数
        /// </summary>
        public int chn { get; set; }

        /// <summary>
        /// 二进制数据字节数据
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 获取数据的进度，100表示获取完成
        /// </summary>
        public int finish { get; set; }

    }
}
