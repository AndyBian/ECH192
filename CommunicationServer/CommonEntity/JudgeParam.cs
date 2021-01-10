using CommunicationServer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.CommonEntity
{
    /// <summary>
    /// 对结果的判断参数
    /// </summary>
    public class JudgeParam
    {
        //数据采集时是否需要开启自检
        public static bool SelfCheck = bool.Parse(IniWrapper.Get("SystemParam", "SelfCheck", "true", SysParam.IniFileFullName).ToString());

        //IT法最大合格电流
        public static string ITMaxCurrent = IniWrapper.Get("ITJudgeParam", "MaxCurrent", "10", SysParam.IniFileFullName).ToString();
        //IT法最小合格电流
        public static string ITMinCurrent = IniWrapper.Get("ITJudgeParam", "MinCurrent", "5", SysParam.IniFileFullName).ToString();
        //IT法通道间差异
        public static string ITCurrentDiff = IniWrapper.Get("ITJudgeParam", "CurrentDiff", "10", SysParam.IniFileFullName).ToString();

        //CV法正向电流最大合格电流
        public static string CVMaxPositiveCurrent = IniWrapper.Get("CVJudgeParam", "MaxPositiveCurrent", "10", SysParam.IniFileFullName).ToString();
        //CV法正向电流最小合格电流
        public static string CVMinPositiveCurrent = IniWrapper.Get("CVJudgeParam", "MinPositiveCurrent", "5", SysParam.IniFileFullName).ToString();
        //CV法正向电流通道间差异
        public static string CVPositiveDiffCurrent = IniWrapper.Get("CVJudgeParam", "PositiveDiffCurrent", "10", SysParam.IniFileFullName).ToString();

        //CV法反向电流最大合格电流
        public static string CVMaxNegativeCurrent = IniWrapper.Get("CVJudgeParam", "MaxNegativeCurrent", "10", SysParam.IniFileFullName).ToString();
        //CV法反向电流最小合格电流
        public static string CVMinNegativeCurrent = IniWrapper.Get("CVJudgeParam", "MinNegativeCurrent", "5", SysParam.IniFileFullName).ToString();
        //CV法反向电流通道间差异
        public static string CVNegativeDiffCurrent = IniWrapper.Get("CVJudgeParam", "NegativeDiffCurrent", "10", SysParam.IniFileFullName).ToString();

        //CV法正向电压最大合格电压
        public static string CVMaxPositiveVolt = IniWrapper.Get("CVJudgeParam", "MaxPositiveVolt", "10", SysParam.IniFileFullName).ToString();
        //CV法正向电压最小合格电压
        public static string CVMinPositiveVolt = IniWrapper.Get("CVJudgeParam", "MinPositiveVolt", "5", SysParam.IniFileFullName).ToString();
        //CV法正向电压通道间差异
        public static string CVPositiveDiffVolt = IniWrapper.Get("CVJudgeParam", "PositiveDiffVolt", "10", SysParam.IniFileFullName).ToString();

        //CV法反向电压最大合格电压
        public static string CVMaxNegativeVolt = IniWrapper.Get("CVJudgeParam", "MaxNegativeVolt", "10", SysParam.IniFileFullName).ToString();
        //CV法反向电压最小合格电压
        public static string CVMinNegativeVolt = IniWrapper.Get("CVJudgeParam", "MinNegativeVolt", "5", SysParam.IniFileFullName).ToString();
        //CV法反向电压通道间差异
        public static string CVNegativeDiffVolt = IniWrapper.Get("CVJudgeParam", "NegativeDiffVolt", "10", SysParam.IniFileFullName).ToString();

    }
}
