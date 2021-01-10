using CommunicationServer.Protocol;
using CommunicationServer.Protocol.Entity;
using CommunicationServer.Util;
using log4net;
using SuperSocket.Facility.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommunicationServer.Util.SysParam;

namespace CommunicationServer.TCPServer
{
    public class DataFixedHeaderReceiveFilter : FixedHeaderReceiveFilter<ECHRequestInfo>
    {
        //日志文件
        private static readonly ILog logger = LogManager.GetLogger(typeof(DataFixedHeaderReceiveFilter));

        //初始化协议长度
        public DataFixedHeaderReceiveFilter()
        : base(31)
        { }

        /// <summary>
        /// 获取出去头部的其他数据长度
        /// </summary>
        /// <param name="header"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            //获取数据包的长度
            int bodylen = BitConverter.ToInt32(header, offset + 27);

            return bodylen - 31;
        }

        /// <summary>
        /// 按照协议解析出ECHRequestInfo
        /// </summary>
        /// <param name="header"></param>
        /// <param name="bodyBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected override ECHRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            ECHRequestInfo request = new ECHRequestInfo();
            List<byte> frame = new List<byte>();
            frame.AddRange(header);
            byte[] buftp = new byte[length];
            Buffer.BlockCopy(bodyBuffer, offset, buftp, 0, length);
            frame.AddRange(buftp);

            //判断帧头是否正确0xAB5A5ABA
            if (frame[0] != 0xBA || frame[1] != 0x5A || frame[2] != 0x5A || frame[3] != 0xAB)
            {
                request.Key = "HeadError";
                return request;
            }

            //判断帧尾是否正确0x0A
            if (frame[frame.Count - 1] != 0x0A)
            {
                request.Key = "TailError";
                return request;
            }

            //最后5个字节中4个字节为校验字节，1个字节为帧尾0x03
            UInt32 crc = BitConverter.ToUInt32(frame.ToArray(), frame.Count - 5);
            UInt32 calcrc = CRC32Tools.GetCRC32(frame.ToArray(), frame.Count - 5);
            //if (crc != calcrc)
            //{
            //    //判断CRC校验是否正确
            //    request.Key = "CRCError";
            //    logger.Error("CalculateCrc:" + calcrc + "-crc:" + crc);
            //}
            //else
            //{}
            //版本号
            request.FrameVersion = frame.ToArray()[4];
            //序号
            request.FrameIndex = BitConverter.ToUInt16(frame.ToArray(), 5);
            //通信链路
            request.CommuTypeIns = (CommuType)(BitConverter.ToUInt32(frame.ToArray(), 7) & 0x01);
            //加密方式
            request.EncryptionIns = (Encryption)((BitConverter.ToUInt32(frame.ToArray(), 7) & 0x02) >> 1);
            //压缩方式
            request.CompressIns = (Compress)((BitConverter.ToUInt32(frame.ToArray(), 7) & 0x0C) >> 2);
            //备用
            Buffer.BlockCopy(frame.ToArray(), 11, request.Obligate, 0, 16);
            //报文长度
            request.FrameLen = BitConverter.ToUInt32(frame.ToArray(), 27);
            //偏移
            request.FrameOffset = BitConverter.ToUInt32(frame.ToArray(), 31);
            //数据总长度
            request.DataLen = BitConverter.ToUInt32(frame.ToArray(), 35);

            request.Data.AddRange(buftp);

            //解析数据内容，分析出指令类型,13为偏移4+数据总长度4+CRC 4+帧尾1
            byte[] datas = new byte[buftp.Length - 13];
            Buffer.BlockCopy(buftp, 8, datas, 0, datas.Length);
            FrameContent frameContent = new FrameContent(datas);

            //二进制文件中采集数据的起始位置
            request.Offset = frameContent.JsonLength + 8 + 13 - 5;

            FrameBuffer fb = JsonParse.JsonParseFromString(frameContent.JsonStr);

            switch (fb.cmd)
            {
                case 1:
                    switch (fb.scmd)
                    {
                        case 0:
                            //获取版本信息
                            request.Key = "GetVersion";
                            break;
                        case 1:
                            //获取版本信息
                            request.Key = "GetIPMessage";
                            break;
                        case 2:
                            //获取系统时间
                            request.Key = "GetSystemTime";
                            break;
                    }
                    break;
                case 3:
                    switch (fb.scmd)
                    {
                        case 0:
                            //重启系统
                            request.Key = "RestartSystem";
                            break;
                        case 2:
                            //心跳包
                            request.Key = "HeartBeat";
                            break;
                    }
                    break;
                case 4:
                    switch (fb.scmd)
                    {
                        case 0:
                            //停止采集
                            request.Key = "StopSample";
                            break;
                        case 1:
                            //启动采集
                            request.Key = "StartSample";
                            break;
                        case 2:
                            //辅助控制
                            request.Key = "AssistControl";
                            break;
                    }
                    break;
                case 5:
                    switch (fb.scmd)
                    {
                        case 0:
                            //IT法参数获取
                            request.Key = "GetITParam";
                            break;
                        case 1:
                            //CV法参数获取
                            request.Key = "GetCVParam";
                            break;
                    }
                    break;
                case 6:
                    switch (fb.scmd)
                    {
                        case 0:
                            //IT法参数设置
                            request.Key = "SetITParam";
                            break;
                        case 1:
                            //CV法参数设置
                            request.Key = "SetCVParam";
                            break;
                    }
                    break;
                case 7:
                    switch (fb.scmd)
                    {
                        case 0:
                            //获取当前使能的通道
                            request.Key = "GetEnableChannels";
                            break;
                        case 1:
                            //获取通道电流灵敏度参数
                            request.Key = "GetSensitivitys";
                            break;
                    }
                    break;
                case 8:
                    switch (fb.scmd)
                    {
                        case 0:
                            //设置当前使能的通道
                            request.Key = "EnableChannels";
                            break;
                        case 1:
                            //设置通道电流灵敏度参数
                            request.Key = "SetSensitivitys";
                            break;
                    }
                    break;
                case 9:
                    switch (fb.scmd)
                    {
                        case 0:
                            //获取采集数据
                            request.Key = "GetSampleData";
                            break;
                    }
                    break;
                default:
                    //未知指令
                    request.Key = "Unknow";
                    break;
            }
            return request;
        }
    }
}
