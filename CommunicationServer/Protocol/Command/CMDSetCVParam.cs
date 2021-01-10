using CommunicationServer.CommonEntity;
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
    /// 设置CV采集参数
    /// </summary>
    public class CMDSetCVParam : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDCRCError));

        public string Name
        {
            get
            {
                return "SetCVParam";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            logger.Error("Set CV Param Successfully!");

            CommunicationMessage communicationmessage = new CommunicationMessage();
            Dictionary<string, dynamic> message = new Dictionary<string, dynamic>();
            message.Add("info", "设置CV法参数成功");
            communicationmessage.Message = message;
            session.OnMessageSend(communicationmessage);

            //设置CV法参数成功之后开始获取采集数据
            Dictionary<string, dynamic> ds = new Dictionary<string, dynamic>();
            ds.Add("cmd", 4);
            ds.Add("scmd", 1);
            //1表示CV法
            ds.Add("type", 1);
            if (JudgeParam.SelfCheck)
            {
                ds.Add("selfcheck", 1);
            }
            
            CommandDriver.SendFrame(session, ds);
        }
    }
}
