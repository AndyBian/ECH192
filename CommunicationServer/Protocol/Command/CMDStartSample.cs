using CommunicationServer.CommunicationDriver;
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
    /// 控制设备开始采集
    /// </summary>
    public class CMDStartSample : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDCRCError));

        public string Name
        {
            get
            {
                return "StartSample";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            logger.Error("Start Sample Successfully!");

            CommunicationMessage communicationmessage = new CommunicationMessage();
            Dictionary<string, dynamic> message = new Dictionary<string, dynamic>();
            message.Add("info", "开始采集数据");
            communicationmessage.Message = message;
            session.OnMessageSend(communicationmessage);

            //开始采集之后获取采集数据
            Dictionary<string, dynamic> ds = new Dictionary<string, dynamic>();
            ds.Add("cmd", 9);
            ds.Add("scmd", 0);
            CommandDriver.SendFrame(session, ds);
        }
    }
}
