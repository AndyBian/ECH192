using CommunicationServer.Protocol.Entity;
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
    /// 控制设备停止采集
    /// </summary>
    public class CMDStopSample : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDCRCError));

        public string Name
        {
            get
            {
                return "StopSample";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            logger.Error("Stop Sample Successfully!");

            CommunicationMessage communicationmessage = new CommunicationMessage();
            Dictionary<string, dynamic> message = new Dictionary<string, dynamic>();
            message.Add("info", "停止采集数据");
            communicationmessage.Message = message;
            session.OnMessageSend(communicationmessage);
        }
    }
}
