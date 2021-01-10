using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.ToolControl
{
    /// <summary>
    /// 制具控制的相关指令类
    /// </summary>
    public class Instruction
    {
        //数据包的起头字
        private static byte STX = 0xfe;

        //数据包的结束字
        private static byte ETX = 0xef;

        //数据包的描述
        private static byte SCK = 0x23;

        //指令的长度
        private static int INSTRUCTIONLEN = 5;

        public static byte[] generateInstruction(InstructionTypeEnum type)
        {
            byte[] instruction = new byte[INSTRUCTIONLEN];

            instruction[0] = STX;
            instruction[1] = SCK;
            instruction[2] = (byte)type;
            instruction[3] = CRC.BCC(instruction, 3);
            instruction[4] = ETX;

            return instruction;
        }
    }
}
