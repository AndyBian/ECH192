using CommunicationServer.TCPServer;
using log4net;
using SuperSocket.SocketBase.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Protocol.Command
{
    /// <summary>
    /// 帧尾异常
    /// </summary>
    public class CMDTailError : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDTailError));

        public string Name
        {
            get
            {
                return "TailError";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            logger.Error("Tail Error!");
            //帧尾出错时关闭连接
            session.Close();
        }
    }
}
