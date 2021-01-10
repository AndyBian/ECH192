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
    /// 未知指令
    /// </summary>
    public class CMDUnknow : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDUnknow));

        public string Name
        {
            get
            {
                return "UnKnow";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            logger.Error("Unknow Command!");
            //未知指令时关闭连接
            session.Close();
        }
    }
}
