using CommunicationServer.CommonEntity;
using CommunicationServer.CommunicationDriver;
using CommunicationServer.Protocol;
using CommunicationServer.Protocol.Entity;
using log4net;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServer.TCPServer
{
    /// <summary>
    /// 命令服务器
    /// </summary>
    public class CommandServer : AppServer<CommandSession, ECHRequestInfo>
    {
        //数据访问锁
        public object lockobj = new object();
        //日志模块
        private static readonly ILog logger = LogManager.GetLogger(typeof(CommandServer));

        //指令服务的相关处理事件
        public event EventHandler MessageEvent;

        /// <summary>
        /// 构造
        /// </summary>
        public CommandServer()
        : base(new DefaultReceiveFilterFactory<DataFixedHeaderReceiveFilter, ECHRequestInfo>())
        {
            this.NewSessionConnected += CommandServer_NewSessionConnected;
            this.SessionClosed += CommandServer_SessionClosed;
        }

        private void CommandServer_NewSessionConnected(CommandSession session)
        {
            logger.Info("command server session connect:" + session.RemoteEndPoint.Address);
            session.MessageEvent += Session_MessageEvent;

            //设备连接之后定期发送心跳包维持设备连接
            Dictionary<string, dynamic> datas = new Dictionary<string, dynamic>();
            datas.Add("cmd", 3);
            datas.Add("scmd", 2);
            CommandDriver.SendFrame(session, datas);
            Thread.Sleep(1000);

            //设备连接之后发送版本信息
            datas = new Dictionary<string, dynamic>();
            datas.Add("cmd", 1);
            datas.Add("scmd", 0);
            CommandDriver.SendFrame(session, datas);
            Thread.Sleep(1000);
        }

        private void CommandServer_SessionClosed(CommandSession session, CloseReason value)
        {
            logger.Info("command server session close:" + session.RemoteEndPoint.Address);

            lock (lockobj)
            {
                Device dev = CommunicationManager.GetDeviceBySessionId(session.SessionID, 0);

                if (dev != null)
                {
                    CommunicationManager.RemoveDevice(dev);
                }
            }

            // 设备失去连接
            CommunicationMessage communicationmessage = new CommunicationMessage();
            Dictionary<string, dynamic> message = new Dictionary<string, dynamic>();
            message.Add("UpdateDataSource", 0);
            message.Add("info", "设备失去连接");
            //失去连接后需要复位
            message.Add("lostconnection", "1");
            communicationmessage.Message = message;

            session.OnMessageSend(communicationmessage);
        }

        private void Session_MessageEvent(object sender, EventArgs e)
        {
            OnServerMessageSend((CommunicationMessage)sender);
        }

        public void OnServerMessageSend(CommunicationMessage message)
        {
            message.ServiceName = this.Name;
            MessageEvent?.Invoke(message, new EventArgs());
        }
    }
}
