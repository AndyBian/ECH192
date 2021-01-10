using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.ToolControl
{
    public enum InstructionTypeEnum
    {
         PRESS = 0x35,         //压机下压指令 
         UP = 0x36,            //压机上升指令
         CHANNEL1 = 0x40,      //切换到通道1指令 
         CHANNEL2 = 0x41,      //切换到通道2指令 
         CHANNEL3 = 0x42,      //切换到通道3指令 
         CHANNEL4 = 0x43,      //切换到通道4指令 
         CHANNEL5 = 0x44,      //切换到通道5指令 
         CHANNEL6 = 0x45,      //切换到通道6指令 
         RESET = 0x50,         //复位准备重新开始  压机上升 通道复位 
         STATUS = 0x51,        //查询当前工作状态 
         ADDSPEED = 0x52,      //泵速度加快一级  速度255级  到最大 不在加速 维持在当前最大速度
         MINUSSPEED = 0x53,    //泵速度变慢一级  速度255级  到最小 不在降速 维持在当前最小速度
 
         SAVEPARAM = 0x60,     //保存工作参数数据
         SETPULSE = 0x61,      //开始设置当前通道的工作脉冲计数 直到手动按下确认按键
         FLUID = 0x62,         //设置当前通道为液体工作介质
         AIR = 0x63,           //设置当前通道为气体工作介质
         NONE = 0x64           //设置当前通道为空置不用
    }
}
