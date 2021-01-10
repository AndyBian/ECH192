using ECH192.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.Entity
{
    /// <summary>
    /// 步骤的相关信息
    /// </summary>
    [Serializable]
    public class StepInfo
    {
        public long _id
        {
            get
            {
                return stepNum;
            }
            set
            {
                value = stepNum;
            }
        }

        //表示当前是第几步
        public int stepNum { get; set; }

        //步骤名称
        public string stepname { get; set; }

        //步骤耗时
        public int steptime { get; set; }

        public StepInfo()
        {
        }

        public StepInfo(StepInterface s)
        {
            this.stepNum = s.stepNum;
            this.stepname = s.stepname;
            this.steptime = s.steptime;
        }

        public StepInterface GenerateStepInterface()
        {
            StepInterface s = new StepInterface();
            s.stepNum = this.stepNum;
            s.stepname = this.stepname;
            s.steptime = this.steptime;
            return s;
        }
    }
}
