using CommunicationServer.CommonEntity;
using CommunicationServer.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommunicationDriver
{
    public class CommunicationManager
    {
        //日志接口
        public static readonly ILog logger = LogManager.GetLogger(typeof(CommunicationManager));

        //数据锁
        public static object lockdataobj = new object();

        /// <summary>
        /// 控制服务所有前端信息
        /// </summary>
        public static List<Device> Devices = new List<Device>();

        /// <summary>
        /// 存储测试过程中所有采集的IT结果值
        /// </summary>
        public static List<ITValue> ITValues = new List<ITValue>();

        /// <summary>
        /// 存储测试过程中所有采集的CV结果值
        /// </summary>
        public static List<CVValue> CVValues = new List<CVValue>();

        /// <summary>
        /// 测点的状态
        /// </summary>
        public static List<int> NodeStatus = new List<int>();

        /// <summary>
        /// 初始化测点的状态，均为正常
        /// </summary>
        public static void InitNodeStatus()
        {
            NodeStatus.Clear();
            for (int i = 0; i < SysParam.HoleNum / 2; i++)
            {
                NodeStatus.Add(0);
            }
        }

        /// <summary>
        /// 按照设备ID进行排序
        /// 更新设备列表信息中的序号
        /// </summary>
        public static void UpdateDeviceSerialNum()
        {
            lock (Devices)
            {
                Devices = Devices.OrderBy(p => p.DevID).ToList();
                for (int i = 0; i < Devices.Count; i++)
                {
                    Devices[i].SerialNum = i + 1;
                }
            }
        }

        /// <summary>
        /// 添加或者更新设备信息
        /// 主要用于设备的两个服务连接时更新队形的客户端信息
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static void AddDeviceByVersion(Device dev)
        {
            //寻找连接设备的信息是否已经存在
            if (Devices.Where(p => p.DevID == dev.DevID)?.Count() > 0)
            {
                Devices.Where(p => p.DevID == dev.DevID).FirstOrDefault().CommandSessionId = dev.CommandSessionId;
            }
            else
                Devices.Add(dev);
        }

        /// <summary>
        /// 获取到IP地址后更新设备信息
        /// IP地址由指令服务器获取
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static void AddDeviceByIPAddress(Device dev)
        {
            //寻找连接设备的信息是否已经存在
            if (Devices.Select(p => p.CommandSessionId == dev.CommandSessionId)?.Count() > 0)
            {
                Devices.Where(p => p.CommandSessionId == dev.CommandSessionId).FirstOrDefault().IP = dev.IP;
            }
            else
                Devices.Add(dev);
        }

        /// <summary>
        /// 获取所有被用户选择的设备(CheckState为true)
        /// </summary>
        /// <returns></returns>
        public static List<Device> GetCheckDevices()
        {
            List<Device> devices = new List<Device>();
            devices = CommunicationManager.Devices.Where(p => p.CheckState == true).ToList();
            return devices;
        }

        /// <summary>
        /// 根据SessionId获取对应的设备信息
        /// </summary>
        /// <param name="sessionid">sessionid</param>
        /// <param name="type">sessionid类型，0表示命令session,1表示数据上送session</param>
        /// <returns></returns>
        public static Device GetDeviceBySessionId(string sessionid, int type)
        {
            Device device = new Device();
            device = CommunicationManager.Devices.Where(p => p.CommandSessionId == sessionid).FirstOrDefault();
            return device;
        }

        /// <summary>
        /// 获取默认设备信息（因此系统只支持1个设备）
        /// </summary>
        /// <param name="sessionid">sessionid</param>
        /// <param name="type">sessionid类型，0表示命令session,1表示数据上送session</param>
        /// <returns></returns>
        public static Device GetDefaultDevice()
        {
            Device device = new Device();
            device = CommunicationManager.Devices.FirstOrDefault();
            return device;
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool RemoveDevice(Device device)
        {
            if (Devices.Contains(device))
            {
                return Devices.Remove(device);
            }
            else
                return false;
        }

        /// <summary>
        /// 初始化采集数据
        /// </summary>
        public static void InitResult()
        {
            ITValues = new List<ITValue>();
            CVValues = new List<CVValue>();
        }

        /// <summary>
        /// 添加采集IT数据
        /// </summary>
        public static void UpdateITValues(FetchValue fetchvalue)
        {
            // 继续采集数据
            if (ITValues.Where(p => p.Index == fetchvalue.Index).Count() > 0)
            {
                ITValues.Where(p => p.Index == fetchvalue.Index).FirstOrDefault().CurrentValue.Add(fetchvalue.CurrentValue);
                ITValues.Where(p => p.Index == fetchvalue.Index).FirstOrDefault().AvgCurrentValue.Add(fetchvalue.AvgCurrentValue);
                ITValues.Where(p => p.Index == fetchvalue.Index).FirstOrDefault().Time.Add(fetchvalue.Time);
            }
            // 第一次采集数据
            else
            {
                List<double> current = new List<double>();
                current.Add(fetchvalue.CurrentValue);
                List<double> avgcurrent = new List<double>();
                avgcurrent.Add(fetchvalue.AvgCurrentValue);
                ITValue itvalue = new ITValue(fetchvalue.Index, current, avgcurrent);
                List<float> times = new List<float>();
                times.Add(fetchvalue.Time);
                itvalue.Time = times;
                ITValues.Add(itvalue);
            }
        }

        /// <summary>
        /// 添加采集CV数据
        /// </summary>
        public static void UpdateCVValues(FetchValue fetchvalue)
        {
            if (CVValues.Where(p => p.Index == fetchvalue.Index).Count() > 0)
            {
                CVValues.Where(p => p.Index == fetchvalue.Index).FirstOrDefault().CurrentValue.Add(fetchvalue.CurrentValue);
                CVValues.Where(p => p.Index == fetchvalue.Index).FirstOrDefault().AvgCurrentValue.Add(fetchvalue.AvgCurrentValue);
                CVValues.Where(p => p.Index == fetchvalue.Index).FirstOrDefault().VoltageValue.Add(fetchvalue.VoltageValue);
                CVValues.Where(p => p.Index == fetchvalue.Index).FirstOrDefault().AvgVoltageValue.Add(fetchvalue.AvgVoltageValue);
            }
            else
            {
                List<double> current = new List<double>();
                current.Add(fetchvalue.CurrentValue);
                List<double> avgcurrent = new List<double>();
                avgcurrent.Add(fetchvalue.AvgCurrentValue);
                List<double> voltage = new List<double>();
                voltage.Add(fetchvalue.VoltageValue);
                List<double> avgvoltage = new List<double>();
                avgvoltage.Add(fetchvalue.AvgVoltageValue);
                CVValue cvvalue = new CVValue(fetchvalue.Index, current, avgcurrent, voltage, avgvoltage);
                CVValues.Add(cvvalue);
            }
        }

        /// <summary>
        /// 更新测点的状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="status"></param>
        public static void UpdateNodeStatus(int index, int status)
        {
            if (NodeStatus[index] < status)
                NodeStatus[index] = status;
        }

        /// <summary>
        /// 获取最新的数据结果
        /// </summary>
        /// <returns></returns>
        public static List<DataSource> GetDataSource()
        {
            List<DataSource> dataSources = new List<DataSource>();
            int serialnum = 1;
            for(int i = 0; i < SysParam.HoleNum; i++)
            {
                DataSource data = new DataSource();
                data.SerialNum = serialnum++;
                data.ChannelName = "Hole" + (i / 2 + 1) + "-" + (i % 2 + 1);

                if (ITValues != null && ITValues.Count > 0)
                {
                    data.ITValue = ITValues[i].CurrentValue[ITValues[i].AvgCurrentValue.Count - 1].ToString();
                }

                if (CVValues != null && CVValues.Count > 0)
                {
                    data.CVX1Value = CVValues[i].AvgCurrentValue[CVValues[i].AvgCurrentValue.Count - 1].ToString();
                    data.CVY1Value = CVValues[i].AvgVoltageValue[CVValues[i].AvgVoltageValue.Count - 1].ToString();

                    data.CVX2Value = CVValues[i].CurrentValue[CVValues[i].CurrentValue.Count-1].ToString();
                    data.CVY2Value = CVValues[i].VoltageValue[CVValues[i].VoltageValue.Count-1].ToString();
                }
                dataSources.Add(data);
            }

            return dataSources;
        }

        /// <summary>
        /// 根据IT采集结果实时获取统计结果
        /// </summary>
        /// <returns></returns>
        public static ITStatisticResult GetITStatisticResult()
        {
            ITStatisticResult itstatisticresult = new ITStatisticResult();

            if (ITValues != null && ITValues.Count > 0)
            {
                List<int> ITValueInterval;
                List<int> counts;
                StatisticITValues(out ITValueInterval, out counts);
                itstatisticresult.ITValues = ITValueInterval;
                itstatisticresult.Counts = counts;
            }

            return itstatisticresult;
        }

        /// <summary>
        /// 根据采集的IT值统计最终结果
        /// </summary>
        /// <param name="ITValueInterval"></param>
        /// <param name="counts"></param>
        private static void StatisticITValues(out List<int> ITValueInterval, out List<int> counts)
        {
            ITValueInterval = new List<int>();
            counts = new List<int>();

            List<double> maxvalues = new List<double>();
            List<double> minvalues = new List<double>();
            for (int i = 0; i < ITValues.Count; i++)
            {
                maxvalues.Add(ITValues[i].AvgCurrentValue.Max());
                minvalues.Add(ITValues[i].AvgCurrentValue.Min());
            }

            //获取到最大值和最小值
            double maxvalue = maxvalues.Max();
            double minvalue = minvalues.Min();

            //获取每个间隔的值
            double interval = (maxvalue - minvalue) / SysParam.SplitCount;

            // 防止intelval为0的情况出现
            interval = interval == 0 ? 1L : interval;

            for (int i = 0; i < SysParam.SplitCount; i++)
            {
                ITValueInterval.Add((int)(minvalue + interval * i));
                counts.Add(0);
            }

            for (int i = 0; i < ITValues.Count; i = i + 2)
            {
                //获取每个数据采集的值属于哪一个间隔
                int index = (int)((ITValues[i].AvgCurrentValue[ITValues[i].AvgCurrentValue.Count - 1] - minvalue) / interval);
                // 将对应间隔的数据个数增加1
                if (index == SysParam.SplitCount)
                    index--;
                counts[index]++;
            }
        }

        /// <summary>
        /// 根据CV采集结果以及类型实时获取统计结果
        /// </summary>
        /// <returns></returns>
        public static CVStatisticResult GetCVStatisticResult(CVResultTypeEnum type)
        {
            CVStatisticResult itstatisticresult = new CVStatisticResult();
            List<double> maxvolt;
            List<double> maxcurrent;
            List<double> minvolt;
            List<double> mincurrent;
            // 获取正向电流、反向电流、正向电压、反向电压
            GetMaxValueAndMinValue(out maxvolt, out maxcurrent, out minvolt, out mincurrent);
            switch (type)
            {
                case CVResultTypeEnum.PositiveCurrent:
                    itstatisticresult = StatisticCVValues(maxcurrent);
                    break;
                case CVResultTypeEnum.PositiveVolt:
                    itstatisticresult = StatisticCVValues(maxvolt);
                    break;
                case CVResultTypeEnum.NegativeCurrent:
                    itstatisticresult = StatisticCVValues(mincurrent);
                    break;
                case CVResultTypeEnum.NegativeVolt:
                    itstatisticresult = StatisticCVValues(minvolt);
                    break;
            }

            return itstatisticresult;
        }

        /// <summary>
        /// 获取CV法采集结果中最大值和最小值
        /// </summary>
        /// <param name="maxvolt"></param>
        /// <param name="maxcurrent"></param>
        /// <param name="minvolt"></param>
        /// <param name="mincurrent"></param>
        private static void GetMaxValueAndMinValue(out List<double> maxvolt,out List<double> maxcurrent,out List<double> minvolt,out List<double> mincurrent)
        {
            maxvolt = new List<double>();
            maxcurrent = new List<double>();
            minvolt = new List<double>();
            mincurrent = new List<double>();

            

            for (int i = 0; i < CVValues.Count; i += 2)
            {
                // 将CV值按照平均电流值排序后取最大值和最小值
                CVValue cv = DeepCopyByBin(CVValues[i]);
                cv.AvgCurrentValue.Sort();
                mincurrent.Add(cv.AvgCurrentValue.FirstOrDefault());
                maxcurrent.Add(cv.AvgCurrentValue.LastOrDefault());

                minvolt.Add(cv.AvgVoltageValue.FirstOrDefault());
                maxvolt.Add(cv.AvgVoltageValue.LastOrDefault());
            }
        }

        /// <summary>
        /// 通过序列化和反序列化进行深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByBin<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        /// <summary>
        /// 根据采集的CV值统计最终结果
        /// </summary>
        /// <param name="CVValueInterval"></param>
        /// <param name="counts"></param>
        private static CVStatisticResult StatisticCVValues(List<double> values)
        {
            List<int> CVValueInterval = new List<int>();
            List<int> counts = new List<int>();

            double maxvalue = values.Max();
            double minvalue = values.Min();

            //获取每个间隔的值
            double interval = (maxvalue - minvalue) / SysParam.SplitCount;

            // 防止intelval为0的情况出现
            interval = interval == 0 ? 1L : interval;

            for (int i = 0; i < SysParam.SplitCount; i++)
            {
                CVValueInterval.Add((int)(minvalue + interval * i));
                counts.Add(0);
            }

            for (int i = 0; i < values.Count; i++)
            {
                //获取每个数据采集的值属于哪一个间隔
                int index = (int)((maxvalue - minvalue) / interval);
                // 将对应间隔的数据个数增加1
                if (index == SysParam.SplitCount)
                    index--;
                counts[index]++;
            }

            CVStatisticResult cvstatisticresult = new CVStatisticResult(CVValueInterval, counts);
            return cvstatisticresult;
        }
    }
}
