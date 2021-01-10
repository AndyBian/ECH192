using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECH192.Entity
{
    /// <summary>
    /// 主界面中节点绘制参数
    /// </summary>
    public class NodeChn
    {

        //日志类型
        public static readonly ILog logger = LogManager.GetLogger(typeof(NodeChn));

        #region 节点绘制规格定义

        /// <summary>
        /// 节点的图片
        /// </summary>
        public Image img = null;

        /// <summary>
        /// 节点框的大小(以界面实际大小为参照)
        /// </summary>
        public SizeF StrSize = new SizeF(18, 18);

        /// <summary>
        /// 指示灯的大小（以界面实际大小为参考）
        /// </summary>
        public SizeF IconSize = new SizeF(18, 18);

        /// <summary>
        /// 指示灯图标的大小
        /// </summary>
        public static RectangleF IconImageSize = new RectangleF(0, 0, 148, 148);

        /// <summary>
        /// 字体原始大小
        /// </summary>
        public const float femSize = 9f;

        /// <summary>
        /// 节点框字体
        /// </summary>
        public static Font NodeFont = new Font("微软雅黑", 12);

        public static Font NodeFontForCfgChnForm = new Font("微软雅黑", 12);


        /// <summary>
        /// 节点框笔刷
        /// </summary>
        public static SolidBrush NodeBrush = new SolidBrush(SystemColors.ControlDarkDark);

        /// <summary>
        /// 报警时节点框的笔刷
        /// </summary>
        public static SolidBrush NodeAlarmBrush = new SolidBrush(Color.Red);

        /// <summary>
        /// 节点框背景的笔刷
        /// </summary>
        public static SolidBrush NodeBackgroundBrush = new SolidBrush(Color.White);

        /// <summary>
        /// 节点框的字的格式 最多显示两位小数
        /// </summary>
        public static string NodeFormat = "{0:F2}";

        /// <summary>
        ///界面中绘制区域宽度
        /// </summary>
        public static int intWidth = 0;

        /// <summary>
        /// 界面中绘制区域高度
        /// </summary>
        public static int intHeight = 0;



        #endregion

        /// <summary>
        /// 节点在界面中的左上点坐标  
        /// </summary>
        public PointF Location;

        /// <summary>
        /// 节点在界面中中的区域 
        /// </summary>
        public RectangleF Rect
        {
            get
            {
                return new RectangleF(this.Location.X, this.Location.Y, StrSize.Width, StrSize.Height);
            }
        }

        /// <summary>
        /// 节点string在界面中的区域 
        /// </summary>
        public RectangleF StrRect
        {
            get
            {
                return new RectangleF(this.Location.X, this.Location.Y + IconSize.Height, StrSize.Width, StrSize.Height);
            }
        }

        /// <summary>
        /// 节点指示灯在界面中的区域 
        /// </summary>
        public RectangleF IconRect
        {
            get
            {
                return new RectangleF(this.Location.X, this.Location.Y, IconSize.Width, IconSize.Height);
            }
        }

        public NodeChn(PointF location)
        {
            this.Location = location;
        }

    }
}
