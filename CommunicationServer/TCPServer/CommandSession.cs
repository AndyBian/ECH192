using CommunicationServer.Protocol;
using CommunicationServer.Protocol.Entity;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.TCPServer
{
    public class CommandSession : AppSession<CommandSession, ECHRequestInfo>
    {
        /// <summary>
        /// 数据接收后触发事件
        /// </summary>
        public event EventHandler MessageEvent;

        public CommandSession()
        {
        }

        /// <summary>
        /// 接收到请求的之后的触发函数
        /// </summary>
        /// <param name="message"></param>
        public void OnMessageSend(CommunicationMessage message)
        {
            MessageEvent?.Invoke(message, new EventArgs());
        }
    }
}
