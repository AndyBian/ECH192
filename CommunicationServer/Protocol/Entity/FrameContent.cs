using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Protocol.Entity
{
    /// <summary>
    /// 数据上送每帧的数据内容
    /// json字符长度/二进制数据长度/json字符串/二进制数据
    /// </summary>
    public class FrameContent
    {
        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] DataBytes;

        /// <summary>
        /// json字符长度
        /// </summary>
        public int JsonLength { get; set; }

        /// <summary>
        /// 二进制数据长度
        /// </summary>
        public int BinaryLength { get; set; }

        /// <summary>
        /// json字符串
        /// </summary>
        public string JsonStr { get; set; }

        /// <summary>
        /// 二进制数据
        /// </summary>
        public byte[] BinaryBytes { get; set; }

        /// <summary>
        /// 返回只有json字符串
        /// </summary>
        /// <param name="JsonStr"></param>
        public FrameContent(string JsonStr)
        {
            byte[] js = Encoding.Default.GetBytes(JsonStr);

            this.DataBytes = new byte[js.Length + 8];

            BitConverter.GetBytes(js.Length).CopyTo(this.DataBytes, 0);

            BitConverter.GetBytes(0).CopyTo(this.DataBytes, 4);

            Buffer.BlockCopy(js, 0, this.DataBytes, 8, js.Length);
        }
        /// <summary>
        /// 解析
        /// </summary>
        public FrameContent(byte[] datas)
        {
            this.DataBytes = datas;

            if (datas.Length >= 8)
            {
                this.JsonLength = (int)BitConverter.ToUInt32(DataBytes, 0);

                this.BinaryLength = (int)BitConverter.ToUInt32(DataBytes, 4);

                this.JsonStr = Encoding.Default.GetString(DataBytes, 8, this.JsonLength);

                this.BinaryBytes = new byte[this.BinaryLength];

                Buffer.BlockCopy(this.DataBytes, 8 + JsonLength, this.BinaryBytes, 0, this.BinaryLength);
            }
        }

        /// <summary>
        /// 将实际数值转换为字节数组
        /// </summary>
        public void DataToBytes()
        {
            this.DataBytes = new byte[8 + this.JsonLength + this.BinaryLength];

            BitConverter.GetBytes(this.JsonLength).CopyTo(this.DataBytes, 0);

            BitConverter.GetBytes(this.BinaryLength).CopyTo(this.DataBytes, 4);

            Encoding.Default.GetBytes(this.JsonStr).CopyTo(this.DataBytes, 8);

            Buffer.BlockCopy(this.BinaryBytes, 0, this.DataBytes, 8 + JsonLength, this.BinaryLength);
        }
    }
}
