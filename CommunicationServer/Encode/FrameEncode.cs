using CommunicationServer.Protocol;
using CommunicationServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Encode
{
    /// <summary>
    /// 组包进行数据上送
    /// </summary>
    public class FrameEncode
    {
        /// <summary>
        /// 将指令内容按照协议进行组包上送
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static byte[] EncodeFrame(ECHRequestInfo Data)
        {
            List<byte> frame = new List<byte>();

            byte[] head = new byte[4] { 0xBA, 0x5A, 0x5A, 0xAB };
            frame.AddRange(head);

            frame.Add(Data.FrameVersion);
            frame.AddRange(BitConverter.GetBytes(Data.FrameIndex));

            int Sign = (byte)Data.CommuTypeIns | (byte)((int)Data.EncryptionIns << 1) | (byte)((int)Data.CompressIns << 2);
            frame.AddRange(BitConverter.GetBytes(Sign));

            frame.AddRange(Data.Obligate);

            frame.AddRange(BitConverter.GetBytes(Data.FrameLen));
            frame.AddRange(BitConverter.GetBytes(Data.FrameOffset));
            frame.AddRange(BitConverter.GetBytes(Data.DataLen));
            frame.AddRange(Data.Data);

            //CRC32校验
            frame.AddRange(BitConverter.GetBytes(CRC32Tools.GetCRC32(frame.ToArray(), (int)Data.FrameLen - 5)));
            //帧尾
            frame.Add(0x0A);

            return frame.ToArray();
        }
    }
}
