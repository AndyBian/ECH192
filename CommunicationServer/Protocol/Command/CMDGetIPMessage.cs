using CommunicationServer.CommonEntity;
using CommunicationServer.CommunicationDriver;
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
    /// 获取IP等网络参数信息
    /// </summary>
    public class CMDGetIPMessage : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDGetIPMessage));

        public string Name
        {
            get
            {
                return "GetIPMessage";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            //解析接收数据
            FrameBuffer fd = CommandUtil.ParseRequestInfo(requestInfo);

            //正确获取到连接设备的IP地址信息
            if (fd.Return == SysParam.ReturnType.Normal)
            {
                Device device = new Device();
                device.CommandSessionId = session.SessionID;
                device.IP = fd.data.localip;
                CommunicationManager.AddDeviceByIPAddress(device);

                // 设备连接成功，更新设备列表
                CommunicationMessage communicationmessage = new CommunicationMessage();
                Dictionary<string, dynamic> message = new Dictionary<string, dynamic>();
                message.Add("UpdateDataSource", 0);
                message.Add("info", "设备连接成功");
                communicationmessage.Message = message;

                session.OnMessageSend(communicationmessage);
            }
        }
    }
}
