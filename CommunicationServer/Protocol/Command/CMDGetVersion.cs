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
    /// 获取版本等信息
    /// </summary>
    public class CMDGetVersion : ICommand<CommandSession, ECHRequestInfo>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CMDGetVersion));

        public string Name
        {
            get
            {
                return "GetVersion";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            //解析接收数据
            FrameBuffer fd = CommandUtil.ParseRequestInfo(requestInfo);

            //正确获取到连接设备信息
            if (fd.Return == SysParam.ReturnType.Normal)
            {
                Device device = new Device(fd.data.name, fd.data.devicecode, fd.data.version, fd.data.sn);
                device.CommandSessionId = session.SessionID;
                //正确获取到版本信息之后更新设备信息
                CommunicationManager.AddDeviceByVersion(device);

                //正确获取到版本信息之后开始获取设备的IP地址等信息
                Dictionary<string, dynamic> ds = new Dictionary<string, dynamic>();
                ds.Add("cmd", 1);
                ds.Add("scmd", 1);
                CommandDriver.SendFrame(session, ds);
            }
        }
    }
}
