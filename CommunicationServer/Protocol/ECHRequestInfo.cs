using CommunicationServer.Util;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommunicationServer.Util.SysParam;

namespace CommunicationServer.Protocol
{
    public class ECHRequestInfo : IRequestInfo
    {
        /// <summary>
        /// 帧key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 帧版本号
        /// </summary>
        public byte FrameVersion { get; set; }
        public static byte frameversion = 1;
        /// <summary>
        /// 帧序号
        /// </summary>
        public UInt16 FrameIndex { get; set; }
        public static UInt16 frameindex = 0;
        /// <summary>
        /// 包标志通信链路
        /// </summary>
        public CommuType CommuTypeIns { get; set; }
        /// <summary>
        /// 包标志加密方式
        /// </summary>
        public Encryption EncryptionIns { get; set; }
        /// <summary>
        /// 包标志压缩方式
        /// </summary>
        public Compress CompressIns { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        public byte[] Obligate { get; set; } = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        /// <summary>
        /// 帧报文总长度
        /// </summary>
        public UInt32 FrameLen { get; set; }
        /// <summary>
        /// 帧偏移量
        /// </summary>
        public UInt32 FrameOffset { get; set; }
        /// <summary>
        /// 数据总长度
        /// </summary>
        public UInt32 DataLen { get; set; }
        /// <summary>
        /// 数据byte[]
        /// </summary>
        public List<byte> Data { get; set; }
        /// <summary>
        /// 数据采集二进制的起始偏移量
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        public ECHRequestInfo()
        {
            Data = new List<byte>();
        }

        public ECHRequestInfo(byte frameversion, UInt16 frameindex, CommuType ct, Encryption ey, Compress cm, UInt32 offset, UInt32 datalen, List<byte> datas)
        {
            this.FrameVersion = frameversion;
            this.FrameIndex = frameindex;
            this.CommuTypeIns = ct;
            this.EncryptionIns = ey;
            this.CompressIns = cm;
            this.FrameLen = (UInt32)(datas.Count + SysParam.ExteriorDataLength);
            this.FrameOffset = offset;
            this.DataLen = datalen;
            this.Data = datas;
        }

        /// <summary>
        /// 获取帧序号
        /// </summary>
        /// <returns></returns>
        public static UInt16 GetNextFrameIndex()
        {
            frameindex++;
            if (frameindex > 65535)
                frameindex = 1;
            return frameindex;
        }
    }
}
