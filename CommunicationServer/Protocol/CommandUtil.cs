using CommunicationServer.Protocol.Entity;
using CommunicationServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Protocol
{
    public class CommandUtil
    {
        /// <summary>
        /// 解析接收到的数据
        /// </summary>
        /// <param name="requestInfo"></param>
        /// <returns></returns>
        public static FrameBuffer ParseRequestInfo(ECHRequestInfo requestInfo)
        {
            //报文格式中报文长度：headlength+数据内容长度
            int headlength = SysParam.ExteriorDataLength;

            //解析数据内容，分析出指令类型,13为偏移4+数据总长度4+CRC 4+帧尾1
            byte[] datas = new byte[requestInfo.Data.Count - 13];
            Buffer.BlockCopy(requestInfo.Data.ToArray(), 8, datas, 0, datas.Length);

            FrameContent frameWork = new FrameContent(datas);

            FrameBuffer fb = JsonParse.JsonParseFromString(frameWork.JsonStr);
            //fb.data = frameWork.BinaryBytes;

            return fb;
        }
    }
}
