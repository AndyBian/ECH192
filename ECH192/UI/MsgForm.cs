using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECH192.UI
{
    public partial class MsgForm : MetroForm
    {
        public bool IsConfirm { get; set; }

        public MsgForm(string title, string message)
        {
            InitializeComponent();
            this.Text = title;
            InfoLable.Text = message;
        }
        public MsgForm(string title, string message, string btnconfirm, string btncancel)
        {
            InitializeComponent();
            this.Text = title;
            InfoLable.Text = message;
            confirmbtn.Text = btnconfirm;
            cancelbtn.Text = btncancel;
        }
        #region 标题栏拖动
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.Location.X - this.oldX;
                //新的鼠标位置                
                this.Top += e.Location.Y - this.oldY;
            }
        }
        private int oldX = 0;

        private int oldY = 0;

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.oldX = e.Location.X;
                //鼠标原来位置                
                this.oldY = e.Location.Y;
            }
        }
        #endregion


        #region 标题外边框
        private SolidBrush SegBrush; //   功控填充颜色所用brush 
        /// <summary>
        /// //绘制边框
        /// </summary>
        /// <param name="g"></param>
        /// <param name="color">lable背景颜色</param>
        /// <param name="color">边框颜色</param>
        /// <param name="x">label宽度</param>
        /// <param name="y">label高度</param>
        private void DrawBorder(System.Drawing.Graphics g, Color color, Color bordercolor, int x, int y)
        {
            SegBrush = new SolidBrush(color);
            Pen pen = new Pen(SegBrush, 1);
            pen.Color = Color.White;
            Rectangle myRectangle = new Rectangle(0, 0, x, y);
            ControlPaint.DrawBorder(g, myRectangle, bordercolor, ButtonBorderStyle.Solid);//边框                                                                                          
        }

        #endregion

        private void confirmbtn_Click(object sender, EventArgs e)
        {
            IsConfirm = true;
            this.Close();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            IsConfirm = false;
            this.Close();
        }

    }
}
