using CommunicationServer.CommonEntity;
using ECH192.SysControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.Entity
{
    /// <summary>
    /// IT法、CV法测试参数
    /// </summary>
    public class TestParam
    {
        //IT初始电位
        public static string ITInitVolt = IniWrapper.Get("ITParam", "InitVolt", "0.2", SystemParam.IniFileFullName).ToString();
        //IT测试运行时间
        public static string ITTestTime = IniWrapper.Get("ITParam", "TestTime", "0.2", SystemParam.IniFileFullName).ToString();
        //IT电流灵敏度
        public static string ITSensitivity = IniWrapper.Get("ITParam", "Sensitivity", "0.2", SystemParam.IniFileFullName).ToString();
        //IT采样间隔
        public static string ITInterval = IniWrapper.Get("ITParam", "Interval", "0.2", SystemParam.IniFileFullName).ToString();
        //IT静止时间
        public static string ITStopTime = IniWrapper.Get("ITParam", "StopTime", "0.2", SystemParam.IniFileFullName).ToString();

        //CV初始化电位
        public static string CVInitVolt = IniWrapper.Get("CVParam", "InitVolt", "0.2", SystemParam.IniFileFullName).ToString();
        //CV最大电位
        public static string CVMaxVolt = IniWrapper.Get("CVParam", "MaxVolt", "0.2", SystemParam.IniFileFullName).ToString();
        //CV最小电位
        public static string CVMinVolt = IniWrapper.Get("CVParam", "MinVolt", "0.2", SystemParam.IniFileFullName).ToString();
        //CV扫描方向
        public static string CVScanDirection = IniWrapper.Get("CVParam", "ScanDirection", "1", SystemParam.IniFileFullName).ToString();
        //CV扫描速率
        public static string CVScanSpeed = IniWrapper.Get("CVParam", "ScanSpeed", "0.2", SystemParam.IniFileFullName).ToString();
        //CV扫描段数
        public static string CVScanCount = IniWrapper.Get("CVParam", "ScanCount", "0.2", SystemParam.IniFileFullName).ToString();
        //CV采样间隔
        public static string CVInterval = IniWrapper.Get("CVParam", "Interval", "0.2", SystemParam.IniFileFullName).ToString();
        //CV电流灵敏度
        public static string CVSensitivity = IniWrapper.Get("CVParam", "Sensitivity", "0.2", SystemParam.IniFileFullName).ToString();
        //CV静止时间
        public static string CVStopTime = IniWrapper.Get("CVParam", "StopTime", "0.2", SystemParam.IniFileFullName).ToString();
    }
}
