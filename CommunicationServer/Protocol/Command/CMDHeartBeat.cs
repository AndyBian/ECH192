using CommunicationServer.CommunicationDriver;
using CommunicationServer.TCPServer;
using log4net;
using SuperSocket.SocketBase.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServer.Protocol.Command
{
    /// <summary>
    /// 心跳包
    /// </summary>
    public class CMDHeartBeat : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDHeartBeat));

        public string Name
        {
            get
            {
                return "HeartBeat";
            }
        }

        public async void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            //每10秒钟发送一次心跳包
            //Thread.Sleep(1000);
            await Task.Delay(10000);

            Dictionary<string, dynamic> datas = new Dictionary<string, dynamic>();
            datas.Add("cmd", 3);
            datas.Add("scmd", 2);
            CommandDriver.SendFrame(session, datas);
        }
    }
}
