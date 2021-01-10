using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommunicationServer.Util.SysParam;

namespace CommunicationServer.Protocol.Entity
{
    /// <summary>
    /// 命令格式
    /// </summary>
    public class FrameBuffer
    {
        /// <summary>
        /// 命令字
        /// </summary>
        public int cmd { get; set; }

        /// <summary>
        /// 子命令字
        /// </summary>
        public int scmd { get; set; }

        /// <summary>
        /// 发送或接收到的命令参数
        /// </summary>
        public FrameData data { get; set; }

        /// <summary>
        /// 返回的错误码
        /// </summary>
        public ReturnType Return { get; set; }

        public FrameBuffer(int cmd, int scmd)
        {
            this.cmd = cmd;
            this.scmd = scmd;
        }
    }
}
