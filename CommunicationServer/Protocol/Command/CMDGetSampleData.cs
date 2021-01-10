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
    /// 获取采集数据
    /// </summary>
    public class CMDGetSampleData : ICommand<CommandSession, ECHRequestInfo>
    {
        public static ILog logger = LogManager.GetLogger(typeof(CMDCRCError));

        public string Name
        {
            get
            {
                return "GetSampleData";
            }
        }

        public void ExecuteCommand(CommandSession session, ECHRequestInfo requestInfo)
        {
            //解析接收数据
            FrameBuffer fd = CommandUtil.ParseRequestInfo(requestInfo);

            //正确获取到数据
            if (fd.Return == SysParam.ReturnType.Normal)
            {
                switch (fd.data.type)
                {
                    // IT法成功获取数据
                    case SysParam.TestType.IT:
                        int iteachlength = 192 * 4;
                        for (int j = 0; j < fd.data.nitem; j++)
                        {
                            int time = CommunicationManager.ITValues.Count == 0 ? 0 : CommunicationManager.ITValues[0].Time.Count;
                            for (int i = 0; i < fd.data.chn; i++)
                            {
                                double ch1value = BitConverter.ToInt32(requestInfo.Data.ToArray(), j * iteachlength + i * 4 + requestInfo.Offset);
                                double ch2value = BitConverter.ToInt32(requestInfo.Data.ToArray(), j * iteachlength + (i % 2 == 0 ? (i + 1) : (i - 1)) * 4 + requestInfo.Offset);
                                FetchValue fetchValue = new FetchValue(i, ch1value, (ch1value + ch2value) / 2);
                                fetchValue.Time = time;

                                //当IT值超出设定的范围时更新状态
                                if (ch1value > double.Parse(JudgeParam.ITMaxCurrent) || ch2value > double.Parse(JudgeParam.ITMaxCurrent)
                                    || ch1value < double.Parse(JudgeParam.ITMinCurrent) || ch2value < double.Parse(JudgeParam.ITMinCurrent))
                                {
                                    CommunicationManager.UpdateNodeStatus(i / 2, 1);
                                }

                                //当2个通道的IT值偏差大约设置的范围时更新状态
                                if (Math.Abs(ch1value - ch2value) / Math.Abs(ch1value) * 100 > double.Parse(JudgeParam.ITCurrentDiff))
                                {
                                    CommunicationManager.UpdateNodeStatus(i / 2, 2);
                                }

                                CommunicationManager.UpdateITValues(fetchValue);
                            }
                        }
                        break;
                    //CV法成功获取数据
                    case SysParam.TestType.CV:
                        int cveachlength = 192 * 4 + 4;
                        for (int j = 0; j < fd.data.nitem; j++)
                        {
                            double vvalue = BitConverter.ToInt32(requestInfo.Data.ToArray(), j * cveachlength + requestInfo.Offset);
                            for (int i = 0; i < fd.data.chn; i++)
                            {
                                double ch1value = BitConverter.ToInt32(requestInfo.Data.ToArray(), j * cveachlength + 4 + i * 4 + requestInfo.Offset);
                                double ch2value = BitConverter.ToInt32(requestInfo.Data.ToArray(), j * cveachlength + (i % 2 == 0 ? (i + 1) : (i - 1)) * 4 + 4 + requestInfo.Offset);
                                FetchValue fetchValue = new FetchValue(i, ch1value, (ch1value + ch2value) / 2, vvalue, vvalue);

                                //当CV值超出设定的范围时更新状态
                                if (ch1value > double.Parse(JudgeParam.CVMaxPositiveCurrent) || ch2value > double.Parse(JudgeParam.CVMaxPositiveCurrent)
                                    || ch1value < double.Parse(JudgeParam.CVMinPositiveCurrent) || ch2value < double.Parse(JudgeParam.CVMinPositiveCurrent))
                                {
                                    CommunicationManager.UpdateNodeStatus(i / 2, 1);
                                }

                                //当2个通道的CV值偏差大约设置的范围时更新状态
                                if (Math.Abs(ch1value - ch2value) / Math.Abs(ch1value) * 100 > double.Parse(JudgeParam.CVPositiveDiffCurrent))
                                {
                                    CommunicationManager.UpdateNodeStatus(i / 2, 2);
                                }

                                CommunicationManager.UpdateCVValues(fetchValue);
                            }
                        }
                        break;
                }
            }

            //获取数据未结束，继续获取下一条数据
            if (fd.data.finish != 100)
            {
                //继续获取下一条数据
                Dictionary<string, dynamic> datas = new Dictionary<string, dynamic>();
                datas.Add("cmd", 9);
                datas.Add("scmd", 0);
                CommandDriver.SendFrame(session, datas);
            }

            CommunicationMessage communicationmessage = new CommunicationMessage();
            Dictionary<string, dynamic> message = new Dictionary<string, dynamic>();
            message.Add("info", fd.data.type + "数据采集中--" + fd.data.finish + "%");
            message.Add("type", fd.data.type);
            //数据采集完成之后实时更新界面中的数据展示
            if (fd.data.type == SysParam.TestType.IT)
                message.Add("UpdateDataSource", 1);
            else
                message.Add("UpdateDataSource", 2);
            message.Add("finish", fd.data.finish);
            communicationmessage.Message = message;
            session.OnMessageSend(communicationmessage);
        }
    }
}
