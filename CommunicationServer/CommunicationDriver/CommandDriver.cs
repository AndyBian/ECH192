using CommunicationServer.CommonEntity;
using CommunicationServer.Protocol;
using CommunicationServer.Protocol.Entity;
using CommunicationServer.TCPServer;
using CommunicationServer.Util;
using log4net;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static CommunicationServer.Util.SysParam;

namespace CommunicationServer.CommunicationDriver
{
    public class CommandDriver
    {
        //日志类型
        public static readonly ILog logger = LogManager.GetLogger(typeof(CommandDriver));

        /// <summary>
        /// 设备指令控制主服务
        /// </summary>
        public CommandServer commandServer { get; set; }

        public string ServiceName
        {
            get
            {
                return commandServer.Name;
            }
        }

        public event EventHandler MessageEvent;

        private void Server_MessageEvent(object sender, EventArgs e)
        {
            OnDriverMessageSend((CommunicationMessage)sender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void OnDriverMessageSend(CommunicationMessage message)
        {
            MessageEvent?.Invoke(message, new EventArgs());
        }

        /// <summary>
        /// 设置服务启动参数并启动
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="TCPPort"></param>
        /// <returns></returns>
        public bool InitServerParam(string ip, int TCPPort)
        {
            string IPaddr = "Any";
            if (!string.IsNullOrEmpty(ip))
            {
                IPAddress ipAddress1 = new IPAddress(new byte[] { 0, 0, 0, 0 });
                if (IPAddress.TryParse(ip, out ipAddress1))
                    IPaddr = ip;
            }
            commandServer = new CommandServer();
            commandServer.MessageEvent += Server_MessageEvent;
            var serverConfig = new ServerConfig
            {
                Name = "commandServer",            //服务器实例的名称
                //ServerType = "AgileServer.Socket.TelnetServer, AgileServer.Socket",
                Ip = IPaddr,                   //Any - 所有的IPv4地址 IPv6Any - 所有的IPv6地址
                Mode = SocketMode.Tcp,          //服务器运行的模式, Tcp (默认) 或者 Udp
                Port = TCPPort,                //服务器监听的端口
                SendingQueueSize = 10,          //发送队列最大长度, 默认值为5
                MaxConnectionNumber = 200,      //可允许连接的最大连接数
                LogCommand = false,            //是否记录命令执行的记录
                LogBasicSessionActivity = false, //是否记录session的基本活动，如连接和断开
                LogAllSocketException = false,  //是否记录所有Socket异常和错误
                //Security = "tls",//Empty, Tls, Ssl3. Socket服务器所采用的传输层加密协议
                ReceiveBufferSize = 102400,
                MaxRequestLength = 102400,
                SendBufferSize = 10240,
                TextEncoding = "UTF-8",        //文本的默认编码，默认值是 ASCII
                KeepAliveTime = 6000,          //网络连接正常情况下的keep alive数据的发送间隔, 默认值为 600, 单位为秒
                KeepAliveInterval = 600,       //Keep alive失败之后, keep alive探测包的发送间隔，默认值为 60, 单位为秒
                ClearIdleSession = true,       //是否定时清空空闲会话，默认值是 false;
                ClearIdleSessionInterval = 120  //清空空闲会话的时间间隔, 默认值是120, 单位为秒;
            };
            var rootConfig = new RootConfig()
            {
                MaxWorkingThreads = 100,               //线程池最大工作线程数量
                MinWorkingThreads = 10,                 //线程池最小工作线程数量;
                MaxCompletionPortThreads = 100,         //线程池最大完成端口线程数量;
                MinCompletionPortThreads = 10,           //线程池最小完成端口线程数量;
                DisablePerformanceDataCollector = true,   //是否禁用性能数据采集;
                //PerformanceDataCollectInterval = 60,//性能数据采集频率 (单位为秒, 默认值: 60);
                //LogFactory = "ConsoleLogFactory",//默认logFactory的名字
                //Isolation = SuperSocket.SocketBase.IsolationMode.AppDomain// 服务器实例隔离级别                
            };
            commandServer.Setup(rootConfig, serverConfig);
            return commandServer.Start();
        }

        /// <summary>
        /// 向指令客户端发送指令数据
        /// </summary>
        /// <param name="sessionid">客户端sessionid</param>
        /// <param name="FrameData">相关参数</param>
        /// <returns></returns>
        public static bool SendFrame(CommandSession session, Dictionary<string, dynamic> Datas)
        {
            bool result = false;

            dynamic obj = null;
            //获取命令字
            int cmd;
            if (Datas.TryGetValue("cmd", out obj))
            {
                cmd = obj;
            }
            else
            {
                logger.Error("no cmd:" + obj);
                return false;
            }
            //获取子命令字
            int scmd;
            if (Datas.TryGetValue("scmd", out obj))
            {
                scmd = obj;
            }
            else
            {
                logger.Error("no scmd:" + obj);
                return false;
            }

            FrameBuffer framebuffer;
            FrameData framedata;
            FrameGroup framegroup;
            ECHRequestInfo requestinfo = new ECHRequestInfo();

            switch (cmd)
            {
                case 1:
                    switch (scmd)
                    {
                        //获取版本信息
                        case 0:
                            framebuffer = new FrameBuffer(1, 0);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //获取IP等网络参数信息
                        case 1:
                            framebuffer = new FrameBuffer(1, 1);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //获取系统时间
                        case 2:
                            framebuffer = new FrameBuffer(1, 2);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                    }
                    break;
                case 3:
                    switch (scmd)
                    {
                        //重启系统
                        case 0:
                            framebuffer = new FrameBuffer(3, 0);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //心跳包
                        case 2:
                            framebuffer = new FrameBuffer(3, 2);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                    }
                    break;
                case 4:
                    switch (scmd)
                    {
                        //停止采集
                        case 0:
                            framebuffer = new FrameBuffer(4, 0);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //开始采集
                        case 1:
                            framebuffer = new FrameBuffer(4, 1);
                            framedata = new FrameData();
                            if (Datas.TryGetValue("type", out obj))
                            {
                                framedata.type = (TestType)obj;
                            }
                            if (Datas.TryGetValue("selfcheck", out obj))
                            {
                                framedata.selftest = (int)obj;
                            }
                            framebuffer.data = framedata;
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //辅助控制
                        case 2:
                            framebuffer = new FrameBuffer(4, 2);
                            int act = 0;
                            if (Datas.TryGetValue("act", out obj))
                            {
                                act = obj;
                            }
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                    }
                    break;
                case 5:
                    switch (scmd)
                    {
                        //IT法参数获取
                        case 0:
                            framebuffer = new FrameBuffer(5, 0);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //CV法参数获取
                        case 1:
                            framebuffer = new FrameBuffer(5, 1);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                    }
                    break;
                case 6:
                    switch (scmd)
                    {
                        //IT法参数配置
                        case 0:
                            framebuffer = new FrameBuffer(6, 0);
                            framedata = new FrameData();
                            if (Datas.TryGetValue("sample_interval", out obj))
                            {
                                framedata.sample_interval = float.Parse(obj);
                            }
                            if (Datas.TryGetValue("quittime", out obj))
                            {
                                framedata.quittime = int.Parse(obj);
                            }
                            if (Datas.TryGetValue("vtg", out obj))
                            {
                                framedata.vtg = int.Parse(obj);
                            }
                            if (Datas.TryGetValue("np", out obj))
                            {
                                framedata.np = int.Parse(obj);
                            }
                            framebuffer.data = framedata;
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //CV法参数配置
                        case 1:
                            framebuffer = new FrameBuffer(6, 1);
                            framedata = new FrameData();
                            if (Datas.TryGetValue("vtg_init", out obj))
                            {
                                framedata.vtg_init = int.Parse(obj);
                            }
                            if (Datas.TryGetValue("quittime", out obj))
                            {
                                framedata.quittime = int.Parse(obj);
                            }
                            if (Datas.TryGetValue("vtg_max", out obj))
                            {
                                framedata.vtg_max = int.Parse(obj);
                            }
                            if (Datas.TryGetValue("vtg_min", out obj))
                            {
                                framedata.vtg_min = int.Parse(obj);
                            }
                            if (Datas.TryGetValue("dir", out obj))
                            {
                                framedata.dir = int.Parse(obj);
                            }
                            if (Datas.TryGetValue("vtg_rate", out obj))
                            {
                                framedata.vtg_rate = int.Parse(obj);
                            }
                            if (Datas.TryGetValue("vtg_sample", out obj))
                            {
                                framedata.vtg_sample = float.Parse(obj);
                            }
                            if (Datas.TryGetValue("nsegs", out obj))
                            {
                                framedata.nsegs = int.Parse(obj);
                            }
                            framebuffer.data = framedata;
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                    }
                    break;
                case 7:
                    switch (scmd)
                    {
                        //获取当前使能的通道
                        case 0:
                            framebuffer = new FrameBuffer(7, 0);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //获取通道电流灵敏度设置
                        case 1:
                            framebuffer = new FrameBuffer(7, 1);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                    }
                    break;
                case 8:
                    switch (scmd)
                    {
                        //使能通道
                        case 0:
                            framebuffer = new FrameBuffer(8, 0);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                        //设置通道灵敏度
                        case 1:
                            framebuffer = new FrameBuffer(8, 1);
                            framedata = new FrameData();
                            framegroup = new FrameGroup();
                            if (Datas.TryGetValue("current", out obj))
                            {
                                for (int i = 0; i < SysParam.HoleNum; i++)
                                {
                                    framegroup.index = i;
                                    framegroup.current = int.Parse(obj);
                                    framedata.group.Add(framegroup);
                                }
                            }
                            framebuffer.data = framedata;
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                    }
                    break;
                case 9:
                    switch (scmd)
                    {
                        //获取采集数据
                        case 0:
                            framebuffer = new FrameBuffer(9, 0);
                            requestinfo = GenerateRequestInfo(framebuffer);
                            break;
                    }
                    break;
            }


            byte[] byts = Encode.FrameEncode.EncodeFrame(requestinfo);
            if (session != null)
            {
                result = session.TrySend(byts, 0, byts.Length);

                if (result == false)
                    result = session.TrySend(byts, 0, byts.Length);
                if (result == false)
                    result = session.TrySend(byts, 0, byts.Length);
                if (result == false)
                    result = session.TrySend(byts, 0, byts.Length);
            }

            return result;
        }

        /// <summary>
        /// 向指令客户端发送指令数据
        /// 未选择设备信息，则从CommunicationManager中获取所有CheckState为true的设备
        /// </summary>
        /// <param name="Datas"></param>
        /// <returns></returns>
        private bool SendFrame(Dictionary<string, dynamic> Datas)
        {
            List<Device> devices = CommunicationManager.GetCheckDevices();
            bool result = true;

            foreach (Device dev in devices)
            {
                CommandSession cmdsession = commandServer.GetSessionByID(dev.CommandSessionId);
                bool re = SendFrame(cmdsession, Datas);
                if (!re)
                    result = false;

            }
            return result;
        }

        /// <summary>
        /// 重启所有选择设备
        /// </summary>
        /// <param name="Datas"></param>
        /// <returns></returns>
        public bool SendRestartFrame()
        {
            Dictionary<string, dynamic> datas = new Dictionary<string, dynamic>();
            datas.Add("cmd", 3);
            datas.Add("scmd", 0);
            //设备层级指令，下发时自动选择选中的设备进行重启控制
            return SendFrame(datas);
        }

        /// <summary>
        /// 发送指令信息
        /// </summary>
        /// <param name="devid"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public bool SendFrame(string devid, InstructionEnum instruction, Dictionary<string, dynamic> param = null)
        {
            Dictionary<string, dynamic> datas = new Dictionary<string, dynamic>();

            datas.Add("cmd", 9);
            datas.Add("scmd", (int)instruction);

            if (param != null)
            {
                foreach (KeyValuePair<string, dynamic> pair in param)
                {
                    datas.Add(pair.Key, pair.Value);
                }
            }

            return SendFrame(devid, datas);
        }

        /// <summary>
        /// 发送指令信息
        /// </summary>
        /// <param name="Datas"></param>
        /// <returns></returns>
        private bool SendFrame(String devid, Dictionary<string, dynamic> datas)
        {
            CommandSession session = commandServer.GetSessionByID(CommunicationManager.Devices.Where(p => p.DevID == devid).Select(p => p.CommandSessionId).SingleOrDefault());
            return SendFrame(session, datas);
        }

        /// <summary>
        /// 根据framebuffer组包CalibrationRequestInfo
        /// </summary>
        /// <param name="buffer"></param>
        private static ECHRequestInfo GenerateRequestInfo(FrameBuffer buffer)
        {
            string jsonstring;
            FrameContent framecontent;
            jsonstring = JsonParse.JsonToString(buffer);
            framecontent = new FrameContent(jsonstring);
            return new ECHRequestInfo(ECHRequestInfo.frameversion, ECHRequestInfo.GetNextFrameIndex(),
                SysParam.CommuType.request, SysParam.Encryption.Unencrypted, SysParam.Compress.Nocompress, 0,
                (UInt32)framecontent.DataBytes.Length, framecontent.DataBytes.ToList());
        }
    }
}
