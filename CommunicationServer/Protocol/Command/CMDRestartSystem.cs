using CommunicationServer.Protocol.Entity;
using CommunicationServer.TCPServer;
using CommunicationServer.Util;
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
    /// 重启系统
    /// </summary>
    public class CMDRestartSystem : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDRestartSystem));

        public string Name
        {
            get
            {
                return "RestartSystem";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            //解析接收数据
            FrameBuffer fd = CommandUtil.ParseRequestInfo(requestInfo);

            //正确重启设备
            if (fd.Return == SysParam.ReturnType.Normal)
            {
                logger.Debug("重启设备成功!");

                CommunicationMessage communicationmessage = new CommunicationMessage();
                Dictionary<string, dynamic> message = new Dictionary<string, dynamic>();
                message.Add("info", "重启设备成功");
                communicationmessage.Message = message;

                session.OnMessageSend(communicationmessage);
            }
        }
    }
}
