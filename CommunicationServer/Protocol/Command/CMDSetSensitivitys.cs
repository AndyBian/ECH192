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
    /// 设置灵敏度
    /// </summary>
    public class CMDSetSensitivitys : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDCRCError));

        public string Name
        {
            get
            {
                return "SetSensitivitys";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            logger.Error("Set Sensitivitys Successfully!");

            CommunicationMessage communicationmessage = new CommunicationMessage();
            Dictionary<string, dynamic> message = new Dictionary<string, dynamic>();
            message.Add("info", "设置灵敏度参数成功");
            communicationmessage.Message = message;
            session.OnMessageSend(communicationmessage);


        }
    }
}
