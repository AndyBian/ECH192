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
    /// 帧头出错
    /// </summary>
    public class CMDHeadError : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDHeadError));

        public string Name
        {
            get
            {
                return "HeadError";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            logger.Error("Head Error!");
            //帧头出错时关闭连接
            session.Close();
        }
    }
}
