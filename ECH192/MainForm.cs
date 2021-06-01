using CommunicationServer.CommonEntity;
using CommunicationServer.CommunicationDriver;
using CommunicationServer.Protocol.Entity;
using ECH192.Entity;
using ECH192.Entity.Enum;
using ECH192.SysControl;
using ECH192.ToolControl;
using ECH192.UI;
using LiteDB;
using log4net;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ECH192
{
    public partial class MainForm : MetroForm
    {
        #region 参数

        // 界面中的所有测点
        List<NodeChn> chns = new List<NodeChn>();

        // 普通测点的图标
        private Image ImageCommon;
        // 通道间偏离值测点的图标
        private Image ImageRed;
        // 平均值偏离指标测点的图标
        private Image ImageYellow;

        // 正常运行图标
        private Image ImagePlay;
        // 运行停止图标
        private Image ImageStop;
        // 运行灰色图标
        private Image ImageGrayPlay;
        // 各个步骤未运行图标
        private Image ImageUnPlay;
        // 各个步骤停止运行图标
        private Image ImageMainStop;
        // 各个步骤运行图标
        private Image ImageMainPlay;

        // IT测试结果统计界面
        private ITTest ItTest;

        // CV测试结果统计界面
        private CVTest CvTest1;
        private CVTest CvTest2;
        private CVTest CvTest3;
        private CVTest CvTest4;

        // IT实时测试结果
        private ITRealTest ItRealTest1;
        private ITRealTest ItRealTest2;
        private ITRealTest ItRealTest3;
        private ITRealTest ItRealTest4;
        private ITRealTest ItRealTest5;
        private ITRealTest ItRealTest6;

        // IT实时测试放大窗口
        private ZoomITRealForm ZoomItRealForm;
        private int ITGroupIndex = 0;

        // CV实时测试结果
        private CVRealTest CvRealTest1;
        private CVRealTest CvRealTest2;
        private CVRealTest CvRealTest3;
        private CVRealTest CvRealTest4;
        private CVRealTest CvRealTest5;
        private CVRealTest CvRealTest6;

        // CV实时测试放大窗口
        private ZoomCVRealForm ZoomCvRealForm;
        private int CVGroupIndex = 0;

        // 添加的步骤
        List<StepInterface> steps = new List<StepInterface>();
        // 主界面中的步骤，和添加的步骤一致
        List<MainStep> mainsteps = new List<MainStep>();

        //设备连接命令操作服务器
        private CommandDriver CMDDriver = new CommandDriver();

        //动态更新设备信息列表、通道信息列表，type表示更新的类型,0表示设备信息更新，1表示通道信息更新
        public delegate void UpdateDataSourceDelegate(int type);

        //数据列表界面，实时动态更新
        DataSourceForm dataSourceForm;

        public static ILog logger = LogManager.GetLogger(typeof(MainForm));

        /// <summary>
        /// 串口实例
        /// </summary>
        SerialPort sp = new SerialPort();

        #region 测试步骤的控制参数

        // 当前的测试步骤
        public int stepindex = 0;

        // 当前是否正在测试
        public bool TestingStatus = false;

        // 当前是否正在调试
        public bool IsDebug = false;

        // 当前是自动测试还是手动测试
        public bool AutoTesting = false;

        #endregion

        #endregion

        #region 界面相关事件

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //  获取步骤信息
            GetStepInfo();

            // 初始化设备端列表信息
            InitDevMessage();
            // 主界面信息初始化
            InitNodes();
            // IT测试结果统计界面初始化
            InitITResult();
            // CV测试结果统计界面初始化
            InitCVResult();
            // IT测试实时结果展示
            InitITRealResult();
            // CV测试实时结果展示
            InitCVRealResult();
            // 初始化测试参数
            InitTestParam();
            // 初始化结果判定参数
            InitJudgeParam();
            // 启动监听线程
            StartThread();

            for (int i = 0; i < steps.Count; i++)
            {
                Steppanel.Controls.Add(steps[i]);
                // 从尾部添加
                Steppanel.Controls.SetChildIndex(steps[i], 0);
                steps[i].ShowTimes();
            }
            AddStepInMain();

            //初始化测点状态
            CommunicationManager.InitNodeStatus();
            UpdateNodeStatus(CommunicationManager.NodeStatus);
        }

        /// <summary>
        /// 动态修改界面中各个控件的位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            UpdateSize();

            //AddStepInMain();
        }

        /// <summary>
        /// 动态更新主界面中各个panel的大小
        /// </summary>
        private void UpdateSize()
        {
            // 定位实时状态位置信息
            lbMessage.Location = new Point(10, this.Height - lbCompany.Height - 10);

            // 定位公司版权信息位置
            lbCompany.Location = new Point(this.Width - lbCompany.Width - 10, this.Height - lbCompany.Height - 10);

            // IT法实时页面保持6个页面大小一致
            ITRealpanelTop.Height = ITRealTimePage.Height / 2;
            ITRealpanel1.Width = ITRealpanel2.Width = ITRealpanel3.Width = ITRealpanelTop.Width / 3;
            ITRealpanel4.Width = ITRealpanel5.Width = ITRealpanel6.Width = ITRealpanelTop.Width / 3;

            // CV法实时页面保持6个页面大小一致
            CVRealpanelTop.Height = CVRealTimePage.Height / 2;
            CVRealpanel1.Width = CVRealpanel2.Width = CVRealpanel3.Width = CVRealpanelTop.Width / 3;
            CVRealpanel4.Width = CVRealpanel5.Width = CVRealpanel6.Width = CVRealpanelTop.Width / 3;

            // CV法结果页面保持4个页面大小一致
            CVStatisticPanelTop.Height = CVPage.Height / 2;
            CVStatisticPanelOne.Width = CVStatisticPanelTwo.Width = CVStatisticPanelTop.Width / 2;
            CVStatisticPanelThree.Width = CVStatisticPanelFour.Width = CVStatisticPanelbottom.Width / 2;

            // 结果判定界面2个panel宽度大小一致
            gbITQualifyParam.Width = ResultSetPage.Width / 2;

            //// 流程配置界面3个panel宽度大小一致
            gbShowStep.Width = gbITParam.Width = ProcessSetPage.Width / 3;

            // 主页流程的换行控制
            MainStepPanelOne.Height = gbProcess.Height * 2 / 5;

            // 数据列表按钮位置
            btnDataSource.Location = new Point(panelStatusTips.Location.X + panelStatusTips.Width - 200, 3);
        }

        private void MaintabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MsgForm mf = new MsgForm("提示", "确认要关闭系统么?");
            mf.TopMost = true;
            mf.ShowDialog();
            if (!mf.IsConfirm)
                e.Cancel = true;
        }


        /// <summary>
        /// 解决自动添加控件时闪烁的问题
        /// </summary>
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}


        #endregion

        #region 主页面操作

        /// <summary>
        /// 绘制主界面中各个测点图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testpointpanel_Paint(object sender, PaintEventArgs e)
        {
            foreach (NodeChn nc in chns)
            {
                e.Graphics.DrawImage(nc.img, nc.IconRect, NodeChn.IconImageSize, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// 初始化测点的信息
        /// </summary>
        private void InitNodes()
        {
            System.Resources.ResourceManager res =
                   new System.Resources.ResourceManager("ECH192.Resource.IconResource",
                   System.Reflection.Assembly.GetExecutingAssembly());

            ImageCommon = (Bitmap)res.GetObject("green");
            ImageRed = (Bitmap)res.GetObject("red");
            ImageYellow = (Bitmap)res.GetObject("yellow");

            ImagePlay = (Bitmap)res.GetObject("play");
            ImageGrayPlay = (Bitmap)res.GetObject("grayplay");
            ImageStop = (Bitmap)res.GetObject("stop");
            ImageMainPlay = (Bitmap)res.GetObject("mainplay");
            ImageMainStop = (Bitmap)res.GetObject("mainstop");
            ImageUnPlay = (Bitmap)res.GetObject("unplay");

            chns.Clear();
            int xstart = 100;
            int ystart = 70;

            lbGroup1.Location = new Point(xstart - 80, ystart - 3);
            lbGroup2.Location = new Point(xstart - 80 + 20 * 30, ystart - 3);
            lbGroup3.Location = new Point(xstart - 80, ystart - 3 + 50);
            lbGroup4.Location = new Point(xstart - 80 + 20 * 30, ystart - 3 + 50);
            lbGroup5.Location = new Point(xstart - 80, ystart - 3 + 100);
            lbGroup6.Location = new Point(xstart - 80 + 20 * 30, ystart - 3 + 100);

            for (int i = 0; i < SystemParam.NodeNum; i++)
            {
                int group = i / 16;

                int starty = ystart + group / 2 * 50;
                int startx = xstart + i % 16 * 30 + group % 2 * 20 * 30;
                NodeChn node = new NodeChn(new Point(startx, starty));
                node.img = ImageCommon;
                chns.Add(node);
            }

            for (int i = 0; i < chns.Count; i++)
            {
                PointF pt = new PointF(chns[i].Location.X, chns[i].Location.Y);

                NodeChn node = new NodeChn(pt);
                Graphics g = this.CreateGraphics();
                SizeF si = g.MeasureString("CH", NodeChn.NodeFontForCfgChnForm);
                Image img = new Bitmap((int)(si.Width), (int)si.Height);
                node.StrSize = new SizeF(img.Width, img.Height);
            }

            // 设置绘制测点的panel无效后，系统会重新调用gbStatus_Paint方法进行测点的重新绘制
            testpointpanel.Invalidate();
        }

        /// <summary>
        /// 更新各个测点的状态图标
        /// </summary>
        /// <param name="status"></param>
        private void UpdateNodeStatus(List<int> status)
        {
            if (status.Count != SystemParam.NodeNum)
                return;

            chns.Clear();
            int xstart = 100;
            int ystart = 70;

            lbGroup1.Location = new Point(xstart - 80, ystart - 3);
            lbGroup2.Location = new Point(xstart - 80 + 20 * 30, ystart - 3);
            lbGroup3.Location = new Point(xstart - 80, ystart - 3 + 50);
            lbGroup4.Location = new Point(xstart - 80 + 20 * 30, ystart - 3 + 50);
            lbGroup5.Location = new Point(xstart - 80, ystart - 3 + 100);
            lbGroup6.Location = new Point(xstart - 80 + 20 * 30, ystart - 3 + 100);
            for (int i = 0; i < SystemParam.NodeNum; i++)
            {
                int group = i / 16;

                int starty = ystart + group / 2 * 50;
                int startx = xstart + i % 16 * 30 + group % 2 * 20 * 30;
                NodeChn node = new NodeChn(new Point(startx, starty));
                switch (status[i])
                {
                    case 0:
                        node.img = ImageCommon;
                        break;
                    case 1:
                        node.img = ImageYellow;
                        break;
                    case 2:
                        node.img = ImageRed;
                        break;
                }
               
                chns.Add(node);
            }

            for (int i = 0; i < chns.Count; i++)
            {
                PointF pt = new PointF(chns[i].Location.X, chns[i].Location.Y);

                NodeChn node = new NodeChn(pt);
                Graphics g = this.CreateGraphics();
                SizeF si = g.MeasureString("CH", NodeChn.NodeFontForCfgChnForm);
                Image img = new Bitmap((int)(si.Width), (int)si.Height);
                node.StrSize = new SizeF(img.Width, img.Height);
            }

            // 设置绘制测点的panel无效后，系统会重新调用gbStatus_Paint方法进行测点的重新绘制
            testpointpanel.Invalidate();
        }

        /// <summary>
        /// 增加主页中的步骤信息
        /// </summary>
        public void AddStepInMain()
        {
            MainStepPanelOne.Controls.Clear();
            MainStepPanelTwo.Controls.Clear();

            // 1个panel容纳的步骤个数
            int capicity = MainPanelStep.Width / 130;
            if (capicity < steps.Count)
            {
                MainStepPanelTwo.Visible = true;
            }
            else
            {
                MainStepPanelTwo.Visible = false;
            }

            mainsteps.Clear();
            for (int i = 0; i < steps.Count; i++)
            {
                MainStep mainStep = new MainStep();
                mainStep.message = steps[i].stepNum + "." + steps[i].stepname;
                mainStep.starttest += new MainStep.StartTest(MainStepStartTest);
                mainStep.UpdateLabelMessage();
                if (i == 0)
                {
                    mainStep.UpdatePlayBackImage(ImageMainPlay, true, 0);
                }

                mainStep.Dock = DockStyle.Left;

                if (i <= capicity)
                {
                    MainStepPanelOne.Controls.Add(mainStep);
                    // 从尾部添加
                    MainStepPanelOne.Controls.SetChildIndex(mainStep, 0);
                }
                else
                {
                    MainStepPanelTwo.Controls.Add(mainStep);
                    // 从尾部添加
                    MainStepPanelTwo.Controls.SetChildIndex(mainStep, 0);
                }

                mainsteps.Add(mainStep);

                if (i != steps.Count - 1)
                {
                    Arrow arrow = new Arrow();
                    arrow.Dock = DockStyle.Left;

                    if (i <= capicity)
                    {
                        MainStepPanelOne.Controls.Add(arrow);
                        // 从尾部添加
                        MainStepPanelOne.Controls.SetChildIndex(arrow, 0);
                    }
                    else
                    {
                        MainStepPanelTwo.Controls.Add(arrow);
                        // 从尾部添加
                        MainStepPanelTwo.Controls.SetChildIndex(arrow, 0);
                    }
                }
            }
        }

        /// <summary>
        /// 判断当前节点区域
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        private int JudgeImageRegion(Point pt)
        {
            for (int i = 0; i < chns.Count; i++)  //判断该点是否在某个节点区域
            {
                if (chns[i].IconRect.Contains(pt))//if (CableNodes[i].Rect.Contains(imagept))
                {
                    return i;                           //该点属于第i个节点区域  
                }
            }

            return -1; //该点不属于任何节点区域
        }

        /// <summary>
        /// 鼠标移动绘制tips显示各个测点的采集数据信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testpointpanel_MouseMove(object sender, MouseEventArgs e)
        {
            int SelectedIndex = JudgeImageRegion(e.Location);

            if (SelectedIndex != -1)
            {
                List<DataSource> datasources = CommunicationManager.GetDataSource();
                if (datasources.Count > 0)
                {
                    string Hole1ITValue = "Hole1 IT Value:" + datasources[SelectedIndex * 2].ITValue + "\r\n";
                    string Hole2ITValue = "Hole2 IT Value:" + datasources[SelectedIndex * 2 + 1].ITValue + "\r\n";
                    string Hole1CVValue = "Hole1 CV Value:" + datasources[SelectedIndex * 2].CVX1Value + "   " + datasources[SelectedIndex * 2].CVY1Value + "\r\n";
                    string Hole2CVValue = "Hole2 CV Value:" + datasources[SelectedIndex * 2 + 1].CVX1Value + "   " + datasources[SelectedIndex * 2 + 1].CVY1Value + "\r\n";
                    ChannelInfoTip.Show(Hole1ITValue + Hole1CVValue + Hole2ITValue + Hole2CVValue, this, e.Location.X + 40, e.Location.Y + 310);

                }
            }
            else
                ChannelInfoTip.Hide(this);
        }

        /// <summary>
        /// 主页面中点击步骤按钮
        /// </summary>
        /// <param name="message"></param>
        private void MainStepStartTest(string message)
        {
            if (AutoTesting)
            {
                AutoTesting = false;
                TestingStatus = false;
                btnStart.BackgroundImage = ImagePlay;
                mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true, 0);
                return;
            }

            if (TestingStatus)
            {
                MsgForm msgForm = new MsgForm("提示", "正在测试中!");
                msgForm.ShowDialog();
                return;
            }

            //获取当前测试的索引号
            int index = int.Parse(message.Split('.')[0]);

            //第一个测点时，初始化测点状态
            if (index == 1)
            {
                if (CommunicationManager.GetDefaultDevice() == null)
                {
                    MsgForm msgForm = new MsgForm("提示", "未有设备连接!");
                    msgForm.ShowDialog();
                    return;
                }
                //初始化测点状态
                CommunicationManager.InitNodeStatus();
                UpdateNodeStatus(CommunicationManager.NodeStatus);
            }

            if (TestingStatus == true)
            {
                TestingStatus = false;
                btnStart.BackgroundImage = ImagePlay;
                mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true, 0);
                return;
            }

            lbMessage.Text = message;
            stepindex = index - 1;
            TestingStatus = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(teststep), stepindex);
        }

        /// <summary>
        /// 展示数据列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataSource_Click(object sender, EventArgs e)
        {
            dataSourceForm = new DataSourceForm();

            dataSourceForm.ShowDialog();
        }

        #endregion

        #region IT法实时曲线


        /// <summary>
        /// 初始化IT实时数据列表
        /// </summary>
        private void InitITRealResult()
        {
            ITRealpanel1.Controls.Clear();
            ItRealTest1 = new ITRealTest(1);
            ItRealTest1.zoomshow += new ITRealTest.ZoomShowDelegate(ZoomITShow);
            ItRealTest1.Dock = DockStyle.Fill;
            ItRealTest1.Init();
            ITRealpanel1.Controls.Add(ItRealTest1);

            ITRealpanel2.Controls.Clear();
            ItRealTest2 = new ITRealTest(2);
            ItRealTest2.zoomshow += new ITRealTest.ZoomShowDelegate(ZoomITShow);
            ItRealTest2.Dock = DockStyle.Fill;
            ItRealTest2.Init();
            ITRealpanel2.Controls.Add(ItRealTest2);

            ITRealpanel3.Controls.Clear();
            ItRealTest3 = new ITRealTest(3);
            ItRealTest3.zoomshow += new ITRealTest.ZoomShowDelegate(ZoomITShow);
            ItRealTest3.Dock = DockStyle.Fill;
            ItRealTest3.Init();
            ITRealpanel3.Controls.Add(ItRealTest3);

            ITRealpanel4.Controls.Clear();
            ItRealTest4 = new ITRealTest(4);
            ItRealTest4.zoomshow += new ITRealTest.ZoomShowDelegate(ZoomITShow);
            ItRealTest4.Dock = DockStyle.Fill;
            ItRealTest4.Init();
            ITRealpanel4.Controls.Add(ItRealTest4);

            ITRealpanel5.Controls.Clear();
            ItRealTest5 = new ITRealTest(5);
            ItRealTest5.zoomshow += new ITRealTest.ZoomShowDelegate(ZoomITShow);
            ItRealTest5.Dock = DockStyle.Fill;
            ItRealTest5.Init();
            ITRealpanel5.Controls.Add(ItRealTest5);

            ITRealpanel6.Controls.Clear();
            ItRealTest6 = new ITRealTest(6);
            ItRealTest6.zoomshow += new ITRealTest.ZoomShowDelegate(ZoomITShow);
            ItRealTest6.Dock = DockStyle.Fill;
            ItRealTest6.Init();
            ITRealpanel6.Controls.Add(ItRealTest6);
        }

        /// <summary>
        /// 显示IT采集法的放大处理
        /// </summary>
        /// <param name="index"></param>
        private void ZoomITShow(int index)
        {
            ZoomItRealForm = new ZoomITRealForm(index);
            ITGroupIndex = index;
            ZoomItRealForm.closezoomshow += new ZoomITRealForm.CloseZoomShowDelegate(CloseITZoomShow);

            List<ITValue> itvalues = CommunicationManager.ITValues.Where(p => p.Index >= (index - 1) * 32 && p.Index < index * 32).ToList();
            ZoomItRealForm.SetData(itvalues);

            ZoomItRealForm.ShowDialog();
        }

        /// <summary>
        /// 关闭IT展示界面的放大
        /// </summary>
        private void CloseITZoomShow()
        {
            ZoomItRealForm = null;
        }

        #endregion

        #region IT法结果曲线

        /// <summary>
        /// 初始化IT测试结果界面
        /// </summary>
        private void InitITResult()
        {
            ItTest = new ITTest();
            ItTest.Dock = DockStyle.Fill;
            ItTest.Init();
            ITResultpanel.Controls.Add(ItTest);
        }

        #endregion

        #region CV法实时曲线

        /// <summary>
        /// 初始化CV实时数据列表
        /// </summary>
        private void InitCVRealResult()
        {
            CvRealTest1= new CVRealTest(1);
            CvRealTest1.zoomshow += new CVRealTest.ZoomShowDelegate(ZoomCVShow);
            CvRealTest1.Dock = DockStyle.Fill;
            CvRealTest1.Init();
            CVRealpanel1.Controls.Add(CvRealTest1);

            CvRealTest2 = new CVRealTest(2);
            CvRealTest2.zoomshow += new CVRealTest.ZoomShowDelegate(ZoomCVShow);
            CvRealTest2.Dock = DockStyle.Fill;
            CvRealTest2.Init();
            CVRealpanel2.Controls.Add(CvRealTest2);

            CvRealTest3 = new CVRealTest(3);
            CvRealTest3.zoomshow += new CVRealTest.ZoomShowDelegate(ZoomCVShow);
            CvRealTest3.Dock = DockStyle.Fill;
            CvRealTest3.Init();
            CVRealpanel3.Controls.Add(CvRealTest3);

            CvRealTest4 = new CVRealTest(4);
            CvRealTest4.zoomshow += new CVRealTest.ZoomShowDelegate(ZoomCVShow);
            CvRealTest4.Dock = DockStyle.Fill;
            CvRealTest4.Init();
            CVRealpanel4.Controls.Add(CvRealTest4);

            CvRealTest5 = new CVRealTest(5);
            CvRealTest5.zoomshow += new CVRealTest.ZoomShowDelegate(ZoomCVShow);
            CvRealTest5.Dock = DockStyle.Fill;
            CvRealTest5.Init();
            CVRealpanel5.Controls.Add(CvRealTest5);

            CvRealTest6 = new CVRealTest(6);
            CvRealTest6.zoomshow += new CVRealTest.ZoomShowDelegate(ZoomCVShow);
            CvRealTest6.Dock = DockStyle.Fill;
            CvRealTest6.Init();
            CVRealpanel6.Controls.Add(CvRealTest6);
        }

        /// <summary>
        /// 显示IT采集法的放大处理
        /// </summary>
        /// <param name="index"></param>
        private void ZoomCVShow(int index)
        {
            ZoomCvRealForm = new ZoomCVRealForm(index);
            CVGroupIndex = index;
            ZoomCvRealForm.closezoomshow += new ZoomCVRealForm.CloseZoomShowDelegate(CloseCVZoomShow);

            List<CVValue> cvvalues = CommunicationManager.CVValues.Where(p => p.Index >= (index - 1) * 32 && p.Index < index * 32).ToList();
            ZoomCvRealForm.SetData(cvvalues);

            ZoomCvRealForm.ShowDialog();
        }

        /// <summary>
        /// 关闭IT展示界面的放大
        /// </summary>
        private void CloseCVZoomShow()
        {
            ZoomCvRealForm = null;
        }

        #endregion

        #region CV法结果曲线


        /// <summary>
        /// 初始化IT测试结果界面
        /// </summary>
        private void InitCVResult()
        {
            CvTest1 = new CVTest("电流(nA)");
            CvTest1.Dock = DockStyle.Fill;
            CvTest1.InitChart("CV法结果-正向电流");
            CVStatisticPanelOne.Controls.Add(CvTest1);

            CvTest2 = new CVTest("电流(nA)");
            CvTest2.Dock = DockStyle.Fill;
            CvTest2.InitChart("CV法结果-反向电流");
            CVStatisticPanelTwo.Controls.Add(CvTest2);

            CvTest3 = new CVTest("电压(mV)");
            CvTest3.Dock = DockStyle.Fill;
            CvTest3.InitChart("CV法结果-正向电压");
            CVStatisticPanelThree.Controls.Add(CvTest3);

            CvTest4 = new CVTest("电压(mV)");
            CvTest4.Dock = DockStyle.Fill;
            CvTest4.InitChart("CV法结果-反向电压");
            CVStatisticPanelFour.Controls.Add(CvTest4);
        }

        #endregion

        #region 流程配置

        /// <summary>
        /// 初始化测试相关参数
        /// </summary>
        private void InitTestParam()
        {
            txtITInitVolt.Text = TestParam.ITInitVolt;
            txtITTestTime.Text = TestParam.ITTestTime;
            cbITSensitivity.SelectedIndex = int.Parse(TestParam.ITSensitivity) == 0 ? 0 : int.Parse(TestParam.ITSensitivity) - 2;
            txtITInterval.Text = TestParam.ITInterval;
            txtITStopTime.Text = TestParam.ITStopTime;

            txtCVInitVolt.Text = TestParam.CVInitVolt;
            txtCVMaxVolt.Text = TestParam.CVMaxVolt;
            txtCVMinVolt.Text = TestParam.CVMinVolt;
            cbCVScanDirection.SelectedIndex = int.Parse(TestParam.CVScanDirection);
            txtCVScanSpeed.Text = TestParam.CVScanSpeed;
            txtCVScanCount.Text = TestParam.CVScanCount;
            txtCVScanInterval.Text = TestParam.CVInterval;
            cbCVSensitivity.SelectedIndex = int.Parse(TestParam.CVSensitivity) == 0 ? 0 : int.Parse(TestParam.CVSensitivity) - 2;
            txtCVStopTime.Text = TestParam.CVStopTime;
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (TestingStatus)
            {
                MsgForm msgForm = new MsgForm("提示", "测试中，请稍后!");
                msgForm.ShowDialog();
                return;
            }
            if (steps.Count >= 18)
            {
                MsgForm msg = new MsgForm("提示", "步骤个数最多为18个!");
                msg.ShowDialog();
                return;
            }
            StepInterface step = new StepInterface();
            step.updatestepinfo += new StepInterface.UpdateStepInfo(StepInterfaceUpdate);
            step.teststep += new StepInterface.TestStep(TestStep);
            step.stepNum = steps.Count + 1;
            step.Dock = DockStyle.Top;
            Steppanel.Controls.Add(step);
            // 从尾部添加
            Steppanel.Controls.SetChildIndex(step, 0);

            Steppanel.VerticalScroll.Value = Steppanel.VerticalScroll.Maximum;

            steps.Add(step);
            AddStepInMain();
            SaveStepInfo();
        }

        /// <summary>
        /// 将步骤上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnup_Click(object sender, EventArgs e)
        {
            if (TestingStatus)
            {
                MsgForm msgForm = new MsgForm("提示", "测试中，请稍后!");
                msgForm.ShowDialog();
                return;
            }

            StepInterface upstep = steps.Where(p => p.CheckState == true).FirstOrDefault();

            if (upstep != null && upstep.stepNum != 1 && steps.Count > 1)
            {
                steps.Where(p => p.stepNum == upstep.stepNum - 1).FirstOrDefault().stepNum++;
                upstep.stepNum--;
            }
            RefreshStep();
            SaveStepInfo();
        }

        /// <summary>
        /// 将步骤下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btndown_Click(object sender, EventArgs e)
        {
            if (TestingStatus)
            {
                MsgForm msgForm = new MsgForm("提示", "测试中，请稍后!");
                msgForm.ShowDialog();
                return;
            }

            StepInterface upstep = steps.Where(p => p.CheckState == true).FirstOrDefault();

            if (upstep != null && upstep.stepNum != steps.Count && steps.Count > 1)
            {
                steps.Where(p => p.stepNum == upstep.stepNum + 1).FirstOrDefault().stepNum--;
                upstep.stepNum++;
            }
            RefreshStep();
            SaveStepInfo();
        }

        /// <summary>
        /// 删除步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btndelete_Click(object sender, EventArgs e)
        {
            List<StepInterface> upsteps = steps.Where(p => p.CheckState == true).ToList();
            foreach (StepInterface step in upsteps)
                steps.Remove(step);
            RefreshStep();
            SaveStepInfo();
        }


        /// <summary>
        /// 重新绘制步骤
        /// </summary>
        private void RefreshStep()
        {
            Steppanel.Controls.Clear();

            //排序后更新序号
            steps = steps.OrderBy(p => p.stepNum).ToList();
            for (int i = 0; i < steps.Count; i++)
            {
                steps[i].stepNum = i + 1;
                steps[i].SetLbStep();
            }

            //更新后重新在界面中绘制
            for (int i = 0; i < steps.Count; i++)
            {
                steps[i].Dock = DockStyle.Top;
                steps[i].SetLbStep();
                Steppanel.Controls.Add(steps[i]);
                // 从尾部添加
                Steppanel.Controls.SetChildIndex(steps[i], 0);
            }
            AddStepInMain();
        }

        /// <summary>
        /// 将步骤信息保存
        /// </summary>
        private void SaveStepInfo()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\File\\";
            string filename = path + "step.db";

            //当配置路径不存在时，创建路径
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            if (System.IO.File.Exists(filename))
                System.IO.File.Delete(filename);

            using (var db = new LiteDatabase(filename))
            {
                // Get customer collection
                var cl = db.GetCollection<StepInfo>("stepinfo");

                // 批量写入步骤
                List<StepInfo> datas = new List<StepInfo>();
                for(int i = 0; i < steps.Count; i++)
                {
                    StepInfo s = new StepInfo(steps[i]);
                    datas.Add(s);
                }

                foreach(StepInfo sinfo in datas)
                {
                    cl.Insert(sinfo);
                    if (datas.Count < 6)
                    {
                        logger.Debug(datas.Count + " " + sinfo.stepname);
                    }
                    logger.Debug(datas.Count + " " + sinfo.stepname);
                }
                db.Dispose();
            }

        }

        /// <summary>
        /// 更改步骤时，触发操作
        /// </summary>
        private void StepInterfaceUpdate()
        {
            SaveStepInfo();
            AddStepInMain();
        }

        /// <summary>
        /// 系统启动时获取步骤信息
        /// </summary>
        private void GetStepInfo()
        {
            string filename = System.IO.Directory.GetCurrentDirectory() + "\\File\\" + "step.db";

            if (System.IO.File.Exists(filename))
            {
                List<StepInfo> stepinfos = new List<StepInfo>();
                using (var db = new LiteDatabase(filename))
                {
                    var cl = db.GetCollection<StepInfo>("stepinfo");

                    IEnumerator<StepInfo> importdata = cl.FindAll().GetEnumerator();
                    while (importdata.MoveNext())
                        stepinfos.Add(importdata.Current);

                    db.Dispose();
                }
                for (int i = 0; i < stepinfos.Count; i++)
                {
                    StepInterface s = stepinfos[i].GenerateStepInterface();
                    s.updatestepinfo += new StepInterface.UpdateStepInfo(StepInterfaceUpdate);
                    s.teststep += new StepInterface.TestStep(TestStep);
                    s.Dock = DockStyle.Top;
                    //s.update();
                    steps.Add(s);
                }

                for (int i = 0; i < steps.Count; i++)
                    steps[i].update();
            }
        }

        /// <summary>
        /// 实时测试各个步骤
        /// </summary>
        /// <param name="stepEnum"></param>
        private void TestStep(StepEnum stepEnum,int stepNum)
        {
            lbMessage.Text = stepEnum.ToString();

            if (TestingStatus)
            {
                MsgForm msgForm = new MsgForm("提示", "正在测试中!");
                msgForm.ShowDialog();
                return;
            }
            if (CommunicationManager.GetDefaultDevice() == null)
            {
                MsgForm msgForm = new MsgForm("提示", "未有设备连接!");
                msgForm.ShowDialog();
                return;
            }

            TestingStatus = true;
            IsDebug = true;
            // 进行模拟测试
            ThreadPool.QueueUserWorkItem(new WaitCallback(teststep), stepNum);
        }

        #region 修改流程配置中参数

        private void txtITInitVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtITInitVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtITInitVolt.Text = "-2500";
                msg.ShowDialog();
                return;
            }
            int value = int.Parse(txtITInitVolt.Text);
            if (value < -2500 || value > 2500)
            {
                MsgForm msg = new MsgForm("提示", "数值范围为-2500~2500");
                txtITInitVolt.Text = "-2500";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("ITParam", "InitVolt", txtITInitVolt.Text, SystemParam.IniFileFullName);
            TestParam.ITInitVolt = txtITInitVolt.Text;
        }

        private void txtITTestTime_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtITTestTime.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtITTestTime.Text = "10";
                msg.ShowDialog();
                return;
            }
            int value = int.Parse(txtITTestTime.Text);
            if (value <0 || value > 1000)
            {
                MsgForm msg = new MsgForm("提示", "数值范围为0~1000");
                txtITTestTime.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("ITParam", "TestTime", txtITTestTime.Text, SystemParam.IniFileFullName);
            TestParam.ITTestTime = txtITTestTime.Text;
        }

        private void txtITSensitivity_TextChanged(object sender, EventArgs e)
        {
            //if (!UtilHelp.CheckInput(txtITSensitivity.Text))
            //{
            //    MsgForm msg = new MsgForm("提示", "数值输入错误");
            //    txtITSensitivity.Text = "10";
            //    msg.ShowDialog();
            //    return;
            //}
            //IniWrapper.Write("ITParam", "Sensitivity", txtITSensitivity.Text, SystemParam.IniFileFullName);
            //TestParam.ITSensitivity = txtITSensitivity.Text;
        }

        private void txtITInterval_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtITInterval.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtITInterval.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("ITParam", "Interval", txtITInterval.Text, SystemParam.IniFileFullName);
            TestParam.ITInterval = txtITInterval.Text;
        }

        private void txtITStopTime_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtITStopTime.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtITStopTime.Text = "10";
                msg.ShowDialog();
                return;
            }
            int value = int.Parse(txtITStopTime.Text);
            if (value < 0 || value > 10000)
            {
                MsgForm msg = new MsgForm("提示", "数值范围为0~10000");
                txtITStopTime.Text = "1000";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("ITParam", "StopTime", txtITStopTime.Text, SystemParam.IniFileFullName);
            TestParam.ITStopTime = txtITStopTime.Text;
        }

        private void txtCVInitVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVInitVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVInitVolt.Text = "-2500";
                msg.ShowDialog();
                return;
            }

            int value = int.Parse(txtCVInitVolt.Text);

            if (value<-2500 || value > 2500)
            {
                MsgForm msg = new MsgForm("提示", "数值范围为-2500~2500");
                txtCVInitVolt.Text = "-2500";
                msg.ShowDialog();
                return;
            }

            IniWrapper.Write("CVParam", "InitVolt", txtCVInitVolt.Text, SystemParam.IniFileFullName);
            TestParam.CVInitVolt = txtCVInitVolt.Text;
        }

        private void txtCVMaxVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMaxVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMaxVolt.Text = "-2500";
                msg.ShowDialog();
                return;
            }
            int value = int.Parse(txtCVMaxVolt.Text);
            if (value < -2500 || value > 2500)
            {
                MsgForm msg = new MsgForm("提示", "数值范围为-2500~2500");
                txtCVMaxVolt.Text = "-2500";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVParam", "MaxVolt", txtCVMaxVolt.Text, SystemParam.IniFileFullName);
            TestParam.CVMaxVolt = txtCVMaxVolt.Text;
        }

        private void txtCVMinVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMinVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMinVolt.Text = "-2500";
                msg.ShowDialog();
                return;
            }
            int value = int.Parse(txtCVMinVolt.Text);
            if (value < -2500 || value > 2500)
            {
                MsgForm msg = new MsgForm("提示", "数值范围为-2500~2500");
                txtCVMinVolt.Text = "-2500";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVParam", "MinVolt", txtCVMinVolt.Text, SystemParam.IniFileFullName);
            TestParam.CVMinVolt = txtCVMinVolt.Text;
        }

        private void cbCVScanDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniWrapper.Write("CVParam", "ScanDirection", cbCVScanDirection.SelectedIndex.ToString(), SystemParam.IniFileFullName);
            TestParam.CVScanDirection = cbCVScanDirection.SelectedIndex.ToString();
        }

        private void txtCVScanSpeed_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVScanSpeed.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVScanSpeed.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVParam", "ScanSpeed", txtCVScanSpeed.Text, SystemParam.IniFileFullName);
            TestParam.CVScanSpeed = txtCVScanSpeed.Text;
        }

        private void txtCVScanCount_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVScanCount.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVScanCount.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVParam", "ScanCount", txtCVScanCount.Text, SystemParam.IniFileFullName);
            TestParam.CVScanCount = txtCVScanCount.Text;
        }

        private void txtCVScanInterval_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVScanInterval.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVScanInterval.Text = "500";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVParam", "Interval", txtCVScanInterval.Text, SystemParam.IniFileFullName);
            TestParam.CVInterval = txtCVScanInterval.Text;
        }

        private void txtCVSensitivity_TextChanged(object sender, EventArgs e)
        {
            //if (!UtilHelp.CheckInput(txtCVSensitivity.Text))
            //{
            //    MsgForm msg = new MsgForm("提示", "数值输入错误");
            //    txtCVSensitivity.Text = "10";
            //    msg.ShowDialog();
            //    return;
            //}
            //IniWrapper.Write("CVParam", "Sensitivity", txtCVSensitivity.Text, SystemParam.IniFileFullName);
            //TestParam.CVSensitivity = txtCVSensitivity.Text;
        }

        private void txtCVStopTime_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVStopTime.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVStopTime.Text = "10";
                msg.ShowDialog();
                return;
            }
            int value = int.Parse(txtCVStopTime.Text);
            if (value < 0 || value > 10000)
            {
                MsgForm msg = new MsgForm("提示", "数值范围为0~10000");
                txtCVStopTime.Text = "1000";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVParam", "StopTime", txtCVStopTime.Text, SystemParam.IniFileFullName);
            TestParam.CVStopTime = txtCVStopTime.Text;
        }

        #endregion

        #endregion

        #region 结果判定配置

        private void InitJudgeParam()
        {
            if (JudgeParam.SelfCheck)
                cbSelfCheck.Checked = true;
            else
                cbSelfCheck.Checked = false;

            if (JudgeParam.AutoSave)
                cbAutoSave.Checked = true;
            else
                cbAutoSave.Checked = false;

            // IT法结果判定参数
            txtITMaxCurrent.Text = JudgeParam.ITMaxCurrent;
            txtITMinCurrent.Text = JudgeParam.ITMinCurrent;
            txtITCurrentDiff.Text = JudgeParam.ITCurrentDiff;

            // CV法正向电流结果判定参数
            txtCVMaxPositiveCurrent.Text = JudgeParam.CVMaxPositiveCurrent;
            txtCVMinPositiveCurrent.Text = JudgeParam.CVMinPositiveCurrent;
            txtCVPositiveDiffCurrent.Text = JudgeParam.CVPositiveDiffCurrent;

            // CV法反向电流结果判定参数
            txtCVMaxNegativeCurrent.Text = JudgeParam.CVMaxNegativeCurrent;
            txtCVMinNegativeCurrent.Text = JudgeParam.CVMinNegativeCurrent;
            txtCVNegativeDiffCurrent.Text = JudgeParam.CVNegativeDiffCurrent;

            // CV法正向电压结果判定参数
            txtCVMaxPositiveVolt.Text = JudgeParam.CVMaxPositiveVolt;
            txtCVMinPositiveVolt.Text = JudgeParam.CVMinPositiveVolt;
            txtCVPositiveDiffVolt.Text = JudgeParam.CVPositiveDiffVolt;

            // CV法反向电压结果判定参数
            txtCVMaxNegativeVolt.Text = JudgeParam.CVMaxNegativeVolt;
            txtCVMinNegativeVolt.Text = JudgeParam.CVMinNegativeVolt;
            txtCVNegativeDiffVolt.Text = JudgeParam.CVNegativeDiffVolt;
        }

        #region 修改结果判定配置中参数

        private void txtITMaxCurrent_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtITMaxCurrent.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtITMaxCurrent.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("ITJudgeParam", "MaxCurrent", txtITMaxCurrent.Text, SystemParam.IniFileFullName);
            JudgeParam.ITMaxCurrent = txtITMaxCurrent.Text;
        }

        private void txtITMinCurrent_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtITMinCurrent.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtITMinCurrent.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("ITJudgeParam", "MinCurrent", txtITMinCurrent.Text, SystemParam.IniFileFullName);
            JudgeParam.ITMinCurrent = txtITMinCurrent.Text;
        }

        private void txtITCurrentDiff_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtITCurrentDiff.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtITCurrentDiff.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("ITJudgeParam", "CurrentDiff", txtITCurrentDiff.Text, SystemParam.IniFileFullName);
            JudgeParam.ITCurrentDiff = txtITCurrentDiff.Text;
        }

        private void txtCVMaxPositiveCurrent_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMaxPositiveCurrent.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMaxPositiveCurrent.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "MaxPositiveCurrent", txtCVMaxPositiveCurrent.Text, SystemParam.IniFileFullName);
            JudgeParam.CVMaxPositiveCurrent = txtCVMaxPositiveCurrent.Text;
        }

        private void txtCVMinPositiveCurrent_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMinPositiveCurrent.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMinPositiveCurrent.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "MinPositiveCurrent", txtCVMinPositiveCurrent.Text, SystemParam.IniFileFullName);
            JudgeParam.CVMinPositiveCurrent = txtCVMinPositiveCurrent.Text;
        }

        private void txtCVPositiveDiffCurrent_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVPositiveDiffCurrent.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVPositiveDiffCurrent.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "PositiveDiffCurrent", txtCVPositiveDiffCurrent.Text, SystemParam.IniFileFullName);
            JudgeParam.CVPositiveDiffCurrent = txtCVPositiveDiffCurrent.Text;
        }

        private void txtCVMaxNegativeCurrent_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMaxNegativeCurrent.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMaxNegativeCurrent.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "MaxNegativeCurrent", txtCVMaxNegativeCurrent.Text, SystemParam.IniFileFullName);
            JudgeParam.CVMaxNegativeCurrent = txtCVMaxNegativeCurrent.Text;
        }

        private void txtCVMinNegativeCurrent_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMinNegativeCurrent.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMinNegativeCurrent.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "MinNegativeCurrent", txtCVMinNegativeCurrent.Text, SystemParam.IniFileFullName);
            JudgeParam.CVMinNegativeCurrent = txtCVMinNegativeCurrent.Text;
        }

        private void txtCVNegativeDiffCurrent_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVNegativeDiffCurrent.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVNegativeDiffCurrent.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "NegativeDiffCurrent", txtCVNegativeDiffCurrent.Text, SystemParam.IniFileFullName);
            JudgeParam.CVNegativeDiffCurrent = txtCVNegativeDiffCurrent.Text;
        }

        private void txtCVMaxPositiveVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMaxPositiveVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMaxPositiveVolt.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "MaxPositiveVolt", txtCVMaxPositiveVolt.Text, SystemParam.IniFileFullName);
            JudgeParam.CVMaxPositiveVolt = txtCVMaxPositiveVolt.Text;
        }

        private void txtCVMinPositiveVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMinPositiveVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMinPositiveVolt.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "MinPositiveVolt", txtCVMinPositiveVolt.Text, SystemParam.IniFileFullName);
            JudgeParam.CVMinPositiveVolt = txtCVMinPositiveVolt.Text;
        }

        private void txtCVPositiveDiffVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVPositiveDiffVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVPositiveDiffVolt.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "PositiveDiffVolt", txtCVPositiveDiffVolt.Text, SystemParam.IniFileFullName);
            JudgeParam.CVPositiveDiffVolt = txtCVPositiveDiffVolt.Text;
        }

        private void txtCVMaxNegativeVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMaxNegativeVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMaxNegativeVolt.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "MaxNegativeVolt", txtCVMaxNegativeVolt.Text, SystemParam.IniFileFullName);
            JudgeParam.CVMaxNegativeVolt = txtCVMaxNegativeVolt.Text;
        }

        private void txtCVMinNegativeVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVMinNegativeVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVMinNegativeVolt.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "MinNegativeVolt", txtCVMinNegativeVolt.Text, SystemParam.IniFileFullName);
            JudgeParam.CVMinNegativeVolt = txtCVMinNegativeVolt.Text;
        }

        private void txtCVNegativeDiffVolt_TextChanged(object sender, EventArgs e)
        {
            if (!UtilHelp.CheckInput(txtCVNegativeDiffVolt.Text))
            {
                MsgForm msg = new MsgForm("提示", "数值输入错误");
                txtCVNegativeDiffVolt.Text = "10";
                msg.ShowDialog();
                return;
            }
            IniWrapper.Write("CVJudgeParam", "NegativeDiffVolt", txtCVNegativeDiffVolt.Text, SystemParam.IniFileFullName);
            JudgeParam.CVNegativeDiffVolt = txtCVNegativeDiffVolt.Text;
        }

        #endregion

        #endregion

        #region 设备信息

        /// <summary>
        /// 动态添加设备信息栏中字段数据
        /// </summary>
        private void InitDevMessage()
        {
            //绑定数据源时不自动添加列
            this.gridviewDev.AutoGenerateColumns = false;

            //序号
            DataGridViewColumn dgc = UtilHelp.CreateColumn("SerialNumDev", "SerialNum", "序号", true, true, 50, 1);
            this.gridviewDev.Columns.Add(dgc);

            //设备ID
            dgc = UtilHelp.CreateColumn("DevIDDev", "DevID", "设备ID", true, true, 100, 2);
            this.gridviewDev.Columns.Add(dgc);

            //设备类型
            dgc = UtilHelp.CreateColumn("DevTypeDev", "DeviceCode", "设备类型", true, true, 80, 3);
            this.gridviewDev.Columns.Add(dgc);

            //设备IP
            dgc = UtilHelp.CreateColumn("DevIPDev", "IP", "设备IP", true, true, 100, 4);
            this.gridviewDev.Columns.Add(dgc);

            //软件版本号
            dgc = UtilHelp.CreateColumn("SoftWareVersionDev", "SWVers", "设备版本号", true, true, 100, 5);
            this.gridviewDev.Columns.Add(dgc);
        }

        /// <summary>
        /// 动态添加复选框功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCheckBox(object sender, DataGridViewBindingCompleteEventArgs e, DataGridView dgv, string columnname)
        {
            DataGridviewCheckboxHeaderCell cell = new DataGridviewCheckboxHeaderCell();
            cell.OnCheckBoxClicked += new DataGridviewCheckboxHeaderCellEventHander(cell_OnCheckBoxClieck);

            DataGridViewCheckBoxColumn cc = dgv.Columns[columnname] as DataGridViewCheckBoxColumn;
            if (cc != null)
            {
                cc.HeaderCell = cell;
                cc.HeaderCell.Value = "";
            }
        }

        /// <summary>
        /// 在DataGridView表头上添加一列复选框功能实现全选功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="dgv">添加复选框功能的datagridview控件</param>
        private void cell_OnCheckBoxClieck(object sender, DataGridviewCheckboxHeaderEventHander e, DataGridView dgv)
        {
            try
            {
                foreach (DataGridViewRow dgvrow in dgv.Rows)
                {
                    if (e.CheckedState)
                    {
                        if (dgv.Columns.Contains("CheckAllDev"))
                            dgvrow.Cells["CheckAllDev"].Value = true;
                    }
                    else
                    {
                        if (dgv.Columns.Contains("CheckAllDev"))
                            dgvrow.Cells["CheckAllDev"].Value = false;
                    }
                }
                //当前列表为设备信息列表时，包含的序号名为SerialNum
                if (dgv.Rows.Count > 0 && dgv.Columns.Contains("SerialNumDev"))
                    dgv.CurrentCell = dgv.Rows[0].Cells["SerialNumDev"];

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 设备信息列表中数据更新完成之后，动态添加复选框功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewDev_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AddCheckBox(sender, e, this.gridviewDev, "CheckAllDev");
        }

        /// <summary>
        /// 重启设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (GetChooseDevNum())
            {
                lbMessage.Text = "开始重启设备";
                CMDDriver.SendRestartFrame();
            }
        }

        /// <summary>
        /// 获取当前是否有选择的设备进行操作
        /// </summary>
        /// <returns></returns>
        private bool GetChooseDevNum()
        {
            //选择设备个数
            int selecteddevcount = 0;
            for (int i = 0; i < CommunicationManager.Devices.Count; i++)
            {
                if (CommunicationManager.Devices[i].CheckState)
                    selecteddevcount++;
            }

            if (selecteddevcount == 0)
            {
                MsgForm mf = new MsgForm("提示", "请选择设备!");
                mf.ShowDialog();
                return false;
            }
            return true;
        }

        #endregion

        #region 数据通信相关操作

        /// <summary>
        /// 添加委托事件
        /// </summary>
        private void AddAllDelegateEvent()
        {
            CMDDriver.MessageEvent += new EventHandler(ShowInfoEvent);
        }

        /// <summary>
        /// 更新设备列表和采集数据信息
        /// </summary>
        /// <param name="type">更新列表</param>
        private void UpdateDataSource(int type)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataSourceDelegate(UpdateDataSource), new object[] { type });
            }
            else
            {
                int index = -1;
                switch (type)
                {
                    //0表示更新设备列表中的信息
                    case 0:
                        //datagridview更新后保持选中项在原来选中的位置
                        index = this.gridviewDev.FirstDisplayedScrollingRowIndex;
                        List<Device> devs = new List<Device>();
                        this.gridviewDev.DataSource = devs;
                        CommunicationManager.UpdateDeviceSerialNum();
                        this.gridviewDev.DataSource = CommunicationManager.Devices;
                        if (index > 0 && index <= this.gridviewDev.Rows.Count)
                            this.gridviewDev.FirstDisplayedScrollingRowIndex = index;
                        break;
                    //1表示更新IT法采集数据的信息
                    case 1:
                        for(int i = 1; i <= 6; i++)
                        {
                            List<ITValue> itvalues = CommunicationManager.ITValues.Where(p => p.Index >= (i - 1) * 32 && p.Index < i * 32).ToList();

                            List<ITValue> filteritvalues = UtilHelp.FilterITData(itvalues, JudgeParam.ShowCount,int.Parse(TestParam.ITInterval));

                            switch (i)
                            {
                                case 1:
                                    ItRealTest1.SetData(filteritvalues);
                                    break;
                                case 2:
                                    ItRealTest2.SetData(filteritvalues);
                                    break;
                                case 3:
                                    ItRealTest3.SetData(filteritvalues);
                                    break;
                                case 4:
                                    ItRealTest4.SetData(filteritvalues);
                                    break;
                                case 5:
                                    ItRealTest5.SetData(filteritvalues);
                                    break;
                                case 6:
                                    ItRealTest6.SetData(filteritvalues);
                                    break;
                            }                            
                        }

                        //更新实时采集数据
                        if (dataSourceForm != null)
                            dataSourceForm.SetData(CommunicationManager.GetDataSource());

                        //实时获取IT采集法的统计结果并更新界面展示
                        ITStatisticResult itStatisticResult = CommunicationManager.GetITStatisticResult();
                        ItTest.SetData(itStatisticResult);

                        // 实时更新放大的IT采集数据
                        if (ZoomItRealForm != null)
                        {
                            List<ITValue> itvalues = CommunicationManager.ITValues.Where(p => p.Index >= (ITGroupIndex - 1) * 32 && p.Index < ITGroupIndex * 32).ToList();
                            List<ITValue> filteritvalues = UtilHelp.FilterITData(itvalues, JudgeParam.ShowCount * 2, int.Parse(TestParam.ITInterval));
                            ZoomItRealForm.SetData(filteritvalues);
                        }

                        break;
                    //2表示更新CV法采集数据的信息
                    case 2:
                        
                        for (int i = 1; i <= 6; i++)
                        {
                            List<CVValue> cvvalues = CommunicationManager.CVValues.Where(p => p.Index >= (i - 1) * 32 && p.Index < i * 32).ToList();

                            List<CVValue> filtercvvalues = UtilHelp.FilterCVData(cvvalues, JudgeParam.ShowCount);
                            switch (i)
                            {
                                case 1:
                                    CvRealTest1.SetData(filtercvvalues);
                                    break;
                                case 2:
                                    CvRealTest2.SetData(filtercvvalues);
                                    break;
                                case 3:
                                    CvRealTest3.SetData(filtercvvalues);
                                    break;
                                case 4:
                                    CvRealTest4.SetData(filtercvvalues);
                                    break;
                                case 5:
                                    CvRealTest5.SetData(filtercvvalues);
                                    break;
                                case 6:
                                    CvRealTest6.SetData(filtercvvalues);
                                    break;
                            }

                        }

                        //更新实时采集数据
                        if (dataSourceForm != null)
                            dataSourceForm.SetData(CommunicationManager.GetDataSource());

                        //实时获取CV采集法的统计结果并更新界面展示
                        CVStatisticResult cvStatisticResult = CommunicationManager.GetCVStatisticResult(CVResultTypeEnum.PositiveCurrent);
                        CvTest1.SetData(cvStatisticResult);

                        cvStatisticResult = CommunicationManager.GetCVStatisticResult(CVResultTypeEnum.NegativeCurrent);
                        CvTest2.SetData(cvStatisticResult);

                        cvStatisticResult = CommunicationManager.GetCVStatisticResult(CVResultTypeEnum.PositiveVolt);
                        CvTest3.SetData(cvStatisticResult);

                        cvStatisticResult = CommunicationManager.GetCVStatisticResult(CVResultTypeEnum.NegativeVolt);
                        CvTest4.SetData(cvStatisticResult);

                        // 实时更新放大的CV采集数据
                        if (ZoomCvRealForm != null)
                        {
                            List<CVValue> cvvalues = CommunicationManager.CVValues.Where(p => p.Index >= (CVGroupIndex - 1) * 32 && p.Index < CVGroupIndex * 32).ToList();
                            List<CVValue> filtercvvalues = UtilHelp.FilterCVData(cvvalues, JudgeParam.ShowCount * 2);
                            ZoomCvRealForm.SetData(filtercvvalues);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 指令服务和数据上送服务接收数据之后的触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowInfoEvent(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(ShowInfoEvent), new object[] { sender, e });
            }
            else
            {
                CommunicationMessage message = (CommunicationMessage)sender;

                dynamic info = "";

                #region 界面显示日志信息

                //获取到日志信息，则界面中显示
                if (message.Message.TryGetValue("info", out info))
                {
                    if (info != "")
                    {
                        logger.Info(info);
                        lbMessage.Text = info;
                    }
                }

                #endregion

                #region 界面更新数据列表

                //获取到更新数据后更新界面中的展示功能
                if (message.Message.TryGetValue("UpdateDataSource", out info))
                {
                    UpdateDataSource(int.Parse(info.ToString()));
                }

                #endregion

                #region 停止采集后退出

                //控制测试结束时，发送采集停止指令
                if (TestingStatus == false)
                {
                    //控制设备停止采集
                    Dictionary<string, dynamic> datas = new Dictionary<string, dynamic>();
                    datas.Add("cmd", 4);
                    datas.Add("scmd", 0);

                    try
                    {
                        CommandDriver.SendFrame(CMDDriver.commandServer.GetSessionByID(CommunicationManager.GetDefaultDevice().CommandSessionId), datas);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("send stop sample error:" + ex.ToString());
                    }
                    return;
                }

                #endregion

                #region 更新IT、CV法数据采集结果

                if (message.Message.TryGetValue("finish", out info))
                {
                    //数据采集完成
                    if (int.Parse(info.ToString()) == 100)
                    {
                        if (AutoTesting)
                        {
                            stepindex += 1;
                            ThreadPool.QueueUserWorkItem(new WaitCallback(teststep), stepindex);
                        }
                        else
                        {
                            if (IsDebug)
                            {
                                IsDebug = false;
                            }
                            else
                            {
                                mainsteps[stepindex].UpdatePlayBackImage(ImageUnPlay, false, 100);
                                if ((stepindex + 1) >= mainsteps.Count)
                                {
                                    stepindex = 0;
                                    mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true);
                                }
                                else
                                {
                                    mainsteps[stepindex + 1].UpdatePlayBackImage(ImageMainPlay, true, 0);
                                    stepindex += 1;
                                }
                            }
                            TestingStatus = false;
                        }
                    }
                    else
                    {
                        mainsteps[stepindex].UpdatePlayBackImage(ImageMainStop, true, int.Parse(info.ToString()));
                    }

                    //实时更新测点的状态
                    UpdateNodeStatus(CommunicationManager.NodeStatus);
                }

                #endregion

                #region 失去连接后复位

                //获取到日志信息，则界面中显示
                if (message.Message.TryGetValue("lostconnection", out info))
                {
                    stepindex = 0;
                    TestingStatus = false;
                    AutoTesting = false;
                    btnStart.Invoke(new Action(() => btnStart.BackgroundImage = ImagePlay));
                    //将第一个设置为可运行
                    mainsteps[0].UpdatePlayBackImage(ImageMainPlay, true, 0);
                    //将其他步骤设置为不可运行
                    for (int i = 1; i < mainsteps.Count; i++)
                        mainsteps[i].UpdatePlayBackImage(ImageUnPlay, false, 0);
                }

                #endregion
            }
        }

        /// <summary>
        /// 根据当前选择的链路和设备类型启动相关监听服务线程
        /// </summary>
        private void StartThread()
        {
            //监听服务器IP
            string ipaddress = IniWrapper.Get("EthernetParam", "IPAddress", SystemParam.DefaultIPAddress, SystemParam.IniFileFullName).ToString();
            //监听服务器端口号
            int commandport = SystemParam.DefaultCommandPort;
            int.TryParse(IniWrapper.Get("EthernetParam", "CommandPort", SystemParam.DefaultCommandPort, SystemParam.IniFileFullName).ToString(), out commandport);
            //当端口号转换失败时，使用默认端口号
            if (commandport == 0)
                commandport = SystemParam.DefaultCommandPort;

            AddAllDelegateEvent();
            //启动命令服务器和数据上送服务器
            CMDDriver.InitServerParam(ipaddress, commandport);
        }

        #endregion

        #region 串口的相关操作

        private bool OpenSerial()
        {
            try
            {
                if (sp.IsOpen)
                    return true;
                else
                {
                    sp.PortName = SerialParam.Name;
                    sp.BaudRate = int.Parse(SerialParam.BaudRate);
                    sp.DataBits = int.Parse(SerialParam.DataBits);
                    sp.StopBits = (StopBits)int.Parse(SerialParam.StopBits);
                    sp.Open();
                }
                if (sp.IsOpen)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (CommunicationManager.GetDefaultDevice() == null)
            {
                MsgForm msgForm = new MsgForm("提示", "未有设备连接!");
                msgForm.ShowDialog();
                return;
            }

            if (!OpenSerial())
            {
                MsgForm msgForm = new MsgForm("提示", "串口制具连接失败!");
                msgForm.ShowDialog();
                return;
            }

            if (TestingStatus)
            {
                MsgForm msgForm = new MsgForm("提示", "正在测试中!");
                msgForm.ShowDialog();
                return;
            }

            //初始化测点状态
            CommunicationManager.InitNodeStatus();
            UpdateNodeStatus(CommunicationManager.NodeStatus);

            // 未自动运行时，则开始自动运行
            if (!AutoTesting)
            {
                if (stepindex != 0)
                {
                    MsgForm msgForm = new MsgForm("提示", "是否重新开始测试");
                    msgForm.ShowDialog();
                    if (msgForm.IsConfirm)
                    {
                        stepindex = 0;
                        InitMainStepImage();
                    }
                }
                else
                    InitMainStepImage();

                TestingStatus = true;
                AutoTesting = true;
                btnStart.BackgroundImage = ImageStop;
                ThreadPool.QueueUserWorkItem(new WaitCallback(teststep), stepindex);
            }
            // 运行中时，则停止自动运行功能
            else
            {
                AutoTesting = false;
                btnStart.BackgroundImage = ImagePlay;
            }
        }

        /// <summary>
        /// 测试各个步骤
        /// </summary>
        /// <param name="step"></param>
        private void teststep(object index)
        {
            int step = int.Parse(index.ToString());

            if (step >= steps.Count)
            {
                // 如果设置了自动存储，则在自动测试完成整个测试之后进行数据存储
                if (JudgeParam.AutoSave && AutoTesting)
                {
                    SaveTestResult();
                }

                stepindex = 0;
                TestingStatus = false;
                AutoTesting = false;
                btnStart.Invoke(new Action(() => btnStart.BackgroundImage = ImagePlay));
                lbMessage.Invoke(new Action(() => lbMessage.Text = "测试完成"));  // 跨线程访问UI控件

                // 测试结束后将第一个置为可用，最后一个置为不可用
                mainsteps[step - 1].UpdatePlayBackImage(ImageUnPlay, false, 100);
                mainsteps[0].UpdatePlayBackImage(ImageMainPlay, true, 100);

                return;
            }

            // 获取当前测试的测试类型
            StepEnum steptype = (StepEnum)Enum.Parse(typeof(StepEnum), steps[step].stepname);

            if (!IsDebug)
            {
                if (step > 0)
                {
                    mainsteps[step - 1].UpdatePlayBackImage(ImageUnPlay, false, 100);
                    mainsteps[step].UpdatePlayBackImage(ImageMainStop, true, 0);
                }
                else
                    mainsteps[step].UpdatePlayBackImage(ImageMainStop, true, 0);
            }

            Dictionary<string, dynamic> datas;
            switch (steptype)
            {
                case StepEnum.针板下压:                   
                    if (OpenSerial())
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "开始" + steps[step].stepname));  // 跨线程访问UI控件
                        byte[] byts = ToolControl.Instruction.generateInstruction(ToolControl.InstructionTypeEnum.PRESS);
                        ToolControl.SerialUtil.WriteSerial(sp, byts, byts.Length);
                    }
                    else
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "串口打开失败：" + steps[step].stepname));  // 跨线程访问UI控件
                        stepindex = 0;
                        TestingStatus = false;
                        AutoTesting = false;
                        btnStart.Invoke(new Action(() => btnStart.BackgroundImage = ImagePlay));
                        //将第一个设置为可运行
                        mainsteps[0].UpdatePlayBackImage(ImageMainPlay, true, 0);
                        //将其他步骤设置为不可运行
                        for (int i = 1; i < mainsteps.Count; i++)
                            mainsteps[i].UpdatePlayBackImage(ImageUnPlay, false, 0);
                        return;
                    }

                    Thread.Sleep(steps[step].steptime * 1000);

                    if (IsDebug)
                    {
                        TestingStatus = false;
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "针板下压完成"));  // 跨线程访问UI控件
                        return;
                    }

                    if (AutoTesting)
                    {
                        stepindex += 1;
                        teststep(stepindex);
                    }
                    else
                    {
                        mainsteps[stepindex].UpdatePlayBackImage(ImageUnPlay, false, 100);
                        if ((stepindex + 1) >= mainsteps.Count)
                        {
                            stepindex = 0;
                            mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true);
                        }
                        else
                        {
                            mainsteps[stepindex + 1].UpdatePlayBackImage(ImageMainPlay, true, 0);
                            stepindex += 1;
                            
                        }
                        TestingStatus = false;
                    }
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "针板下压完成"));  // 跨线程访问UI控件
                    break;
                case StepEnum.针板上升:
                    if (OpenSerial())
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "开始" + steps[step].stepname));  // 跨线程访问UI控件
                        byte[] byts = ToolControl.Instruction.generateInstruction(ToolControl.InstructionTypeEnum.UP);
                        ToolControl.SerialUtil.WriteSerial(sp, byts, byts.Length);
                    }
                    else
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "串口打开失败：" + steps[step].stepname));  // 跨线程访问UI控件
                        stepindex = 0;
                        TestingStatus = false;
                        AutoTesting = false;
                        btnStart.Invoke(new Action(() => btnStart.BackgroundImage = ImagePlay));
                        //将第一个设置为可运行
                        mainsteps[0].UpdatePlayBackImage(ImageMainPlay, true, 0);
                        //将其他步骤设置为不可运行
                        for (int i = 1; i < mainsteps.Count; i++)
                            mainsteps[i].UpdatePlayBackImage(ImageUnPlay, false, 0);
                        return;
                    }

                    Thread.Sleep(steps[step].steptime * 1000);

                    if (IsDebug)
                    {
                        TestingStatus = false;
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "针板上升完成"));  // 跨线程访问UI控件
                        return;
                    }

                    if (AutoTesting)
                    {
                        stepindex += 1;
                        teststep(stepindex);
                    }
                    else
                    {
                        mainsteps[stepindex].UpdatePlayBackImage(ImageUnPlay, false, 100);
                        if ((stepindex + 1) >= mainsteps.Count)
                        {
                            stepindex = 0;
                            mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true);
                        }
                        else
                        {
                            mainsteps[stepindex + 1].UpdatePlayBackImage(ImageMainPlay, true, 0);
                            stepindex += 1;
                            
                        }
                        TestingStatus = false;
                    }
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "针板上升完成"));  // 跨线程访问UI控件
                    break;
                case StepEnum.通纯水:
                    if (OpenSerial())
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "开始" + steps[step].stepname));  // 跨线程访问UI控件
                        byte[] byts = ToolControl.Instruction.generateInstruction(ToolControl.InstructionTypeEnum.CHANNEL1);
                        ToolControl.SerialUtil.WriteSerial(sp, byts, byts.Length);
                    }
                    else
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "串口打开失败：" + steps[step].stepname));  // 跨线程访问UI控件
                        stepindex = 0;
                        TestingStatus = false;
                        AutoTesting = false;
                        btnStart.Invoke(new Action(() => btnStart.BackgroundImage = ImagePlay));
                        //将第一个设置为可运行
                        mainsteps[0].UpdatePlayBackImage(ImageMainPlay, true, 0);
                        //将其他步骤设置为不可运行
                        for (int i = 1; i < mainsteps.Count; i++)
                            mainsteps[i].UpdatePlayBackImage(ImageUnPlay, false, 0);
                        return;
                    }

                    Thread.Sleep(steps[step].steptime * 1000);

                    if (IsDebug)
                    {
                        TestingStatus = false;
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "通纯水完成"));  // 跨线程访问UI控件
                        return;
                    }

                    if (AutoTesting)
                    {
                        stepindex += 1;
                        teststep(stepindex);
                    }
                    else
                    {
                        mainsteps[stepindex].UpdatePlayBackImage(ImageUnPlay, false, 100);
                        if ((stepindex + 1) >= mainsteps.Count)
                        {
                            stepindex = 0;
                            mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true);
                        }
                        else
                        {
                            mainsteps[stepindex + 1].UpdatePlayBackImage(ImageMainPlay, true, 0);
                            stepindex += 1;                            
                        }
                        TestingStatus = false;
                    }
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "通纯水完成"));  // 跨线程访问UI控件
                    break;
                case StepEnum.通空气:
                    if (OpenSerial())
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "开始" + steps[step].stepname));  // 跨线程访问UI控件
                        byte[] byts = ToolControl.Instruction.generateInstruction(ToolControl.InstructionTypeEnum.CHANNEL2);
                        ToolControl.SerialUtil.WriteSerial(sp, byts, byts.Length);
                    }
                    else
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "串口打开失败：" + steps[step].stepname));  // 跨线程访问UI控件
                        stepindex = 0;
                        TestingStatus = false;
                        AutoTesting = false;
                        btnStart.Invoke(new Action(() => btnStart.BackgroundImage = ImagePlay));
                        //将第一个设置为可运行
                        mainsteps[0].UpdatePlayBackImage(ImageMainPlay, true, 0);
                        //将其他步骤设置为不可运行
                        for (int i = 1; i < mainsteps.Count; i++)
                            mainsteps[i].UpdatePlayBackImage(ImageUnPlay, false, 0);
                        return;
                    }

                    Thread.Sleep(steps[step].steptime * 1000);

                    if (IsDebug)
                    {
                        TestingStatus = false;
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "通空气完成"));  // 跨线程访问UI控件
                        return;
                    }

                    if (AutoTesting)
                    {
                        stepindex += 1;
                        teststep(stepindex);
                    }
                    else
                    {
                        mainsteps[stepindex].UpdatePlayBackImage(ImageUnPlay, false, 100);
                        if ((stepindex + 1) >= mainsteps.Count)
                        {
                            stepindex = 0;
                            mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true);
                        }
                        else
                        {
                            mainsteps[stepindex + 1].UpdatePlayBackImage(ImageMainPlay, true, 0);
                            stepindex += 1;
                        }
                        TestingStatus = false;
                    }
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "通空气完成"));  // 跨线程访问UI控件
                    break;
                case StepEnum.通溶液1:
                    if (OpenSerial())
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "开始" + steps[step].stepname));  // 跨线程访问UI控件
                        byte[] byts = ToolControl.Instruction.generateInstruction(ToolControl.InstructionTypeEnum.CHANNEL3);
                        ToolControl.SerialUtil.WriteSerial(sp, byts, byts.Length);
                    }
                    else
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "串口打开失败：" + steps[step].stepname));  // 跨线程访问UI控件
                        stepindex = 0;
                        TestingStatus = false;
                        AutoTesting = false;
                        btnStart.Invoke(new Action(() => btnStart.BackgroundImage = ImagePlay));
                        //将第一个设置为可运行
                        mainsteps[0].UpdatePlayBackImage(ImageMainPlay, true, 0);
                        //将其他步骤设置为不可运行
                        for (int i = 1; i < mainsteps.Count; i++)
                            mainsteps[i].UpdatePlayBackImage(ImageUnPlay, false, 0);
                        return;
                    }

                    Thread.Sleep(steps[step].steptime * 1000);

                    if (IsDebug)
                    {
                        TestingStatus = false;
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "通溶液1完成"));  // 跨线程访问UI控件
                        return;
                    }

                    if (AutoTesting)
                    {
                        stepindex += 1;
                        teststep(stepindex);
                    }
                    else
                    {
                        mainsteps[stepindex].UpdatePlayBackImage(ImageUnPlay, false, 100);
                        if ((stepindex + 1) >= mainsteps.Count)
                        {
                            stepindex = 0;
                            mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true);
                        }
                        else
                        {
                            mainsteps[stepindex + 1].UpdatePlayBackImage(ImageMainPlay, true, 0);
                            stepindex += 1;
                            
                        }
                        TestingStatus = false;
                    }
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "通溶液1完成"));  // 跨线程访问UI控件
                    break;
                case StepEnum.通溶液2:
                    if (OpenSerial())
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "开始" + steps[step].stepname));  // 跨线程访问UI控件
                        byte[] byts = ToolControl.Instruction.generateInstruction(ToolControl.InstructionTypeEnum.CHANNEL4);
                        ToolControl.SerialUtil.WriteSerial(sp, byts, byts.Length);
                    }
                    else
                    {
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "串口打开失败：" + steps[step].stepname));  // 跨线程访问UI控件
                        stepindex = 0;
                        TestingStatus = false;
                        AutoTesting = false;
                        btnStart.Invoke(new Action(() => btnStart.BackgroundImage = ImagePlay));
                        //将第一个设置为可运行
                        mainsteps[0].UpdatePlayBackImage(ImageMainPlay, true, 0);
                        //将其他步骤设置为不可运行
                        for (int i = 1; i < mainsteps.Count; i++)
                            mainsteps[i].UpdatePlayBackImage(ImageUnPlay, false, 0);
                        return;
                    }

                    Thread.Sleep(steps[step].steptime * 1000);

                    if (IsDebug)
                    {
                        TestingStatus = false;
                        lbMessage.Invoke(new Action(() => lbMessage.Text = "通溶液2完成"));  // 跨线程访问UI控件
                        return;
                    }

                    if (AutoTesting)
                    {
                        stepindex += 1;
                        teststep(stepindex);
                    }
                    else
                    {
                        mainsteps[stepindex].UpdatePlayBackImage(ImageUnPlay, false, 100);
                        if ((stepindex + 1) >= mainsteps.Count)
                        {
                            stepindex = 0;
                            mainsteps[stepindex].UpdatePlayBackImage(ImageMainPlay, true);
                        }
                        else
                        {
                            mainsteps[stepindex + 1].UpdatePlayBackImage(ImageMainPlay, true, 0);
                            stepindex += 1;
                            
                        }
                        TestingStatus = false;
                    }
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "通溶液2完成"));  // 跨线程访问UI控件
                    break;
                case StepEnum.IT法电沉积:
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "开始" + steps[step].stepname));  // 跨线程访问UI控件

                    //重新进行IT法电沉积测试
                    CommunicationManager.ITValues.Clear();
                    ItRealTest1.Invoke(new Action(() => ItRealTest1.InitData()));
                    ItRealTest2.Invoke(new Action(() => ItRealTest2.InitData()));
                    ItRealTest3.Invoke(new Action(() => ItRealTest3.InitData()));
                    ItRealTest4.Invoke(new Action(() => ItRealTest4.InitData()));
                    ItRealTest5.Invoke(new Action(() => ItRealTest5.InitData()));
                    ItRealTest6.Invoke(new Action(() => ItRealTest6.InitData()));

                    ItTest.Invoke(new Action(() => ItTest.InitData()));

                    //先配置IT法参数
                    datas = new Dictionary<string, dynamic>();
                    //设置IT灵敏度
                    datas.Add("cmd", 8);
                    datas.Add("scmd", 1);
                    datas.Add("current", TestParam.ITSensitivity);
                    CommandDriver.SendFrame(CMDDriver.commandServer.GetSessionByID(CommunicationManager.GetDefaultDevice().CommandSessionId), datas);

                    datas.Clear();
                    datas.Add("cmd", 6);
                    datas.Add("scmd", 0);
                    //设备参数信息
                    datas.Add("sample_interval", TestParam.ITInterval);
                    datas.Add("quittime", TestParam.ITStopTime);
                    datas.Add("vtg", TestParam.ITInitVolt);
                    datas.Add("np", ((int)(float.Parse(TestParam.ITTestTime) * 1000 / float.Parse(TestParam.ITInterval))).ToString());
                    CommandDriver.SendFrame(CMDDriver.commandServer.GetSessionByID(CommunicationManager.GetDefaultDevice().CommandSessionId), datas);
                    break;
                case StepEnum.CV法扫描:
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "开始" + steps[step].stepname));  // 跨线程访问UI控件

                    //重新进行CV法扫描测试
                    CommunicationManager.CVValues.Clear();
                    CvRealTest1.Invoke(new Action(() => CvRealTest1.InitData()));
                    CvRealTest2.Invoke(new Action(() => CvRealTest2.InitData()));
                    CvRealTest3.Invoke(new Action(() => CvRealTest3.InitData()));
                    CvRealTest4.Invoke(new Action(() => CvRealTest4.InitData()));
                    CvRealTest5.Invoke(new Action(() => CvRealTest5.InitData()));
                    CvRealTest6.Invoke(new Action(() => CvRealTest6.InitData()));

                    CvTest1.Invoke(new Action(() => CvTest1.InitData()));
                    CvTest2.Invoke(new Action(() => CvTest2.InitData()));
                    CvTest3.Invoke(new Action(() => CvTest3.InitData()));
                    CvTest4.Invoke(new Action(() => CvTest4.InitData()));

                    //先配置CV法参数
                    datas = new Dictionary<string, dynamic>();
                    //设置CV灵敏度
                    datas.Add("cmd", 8);
                    datas.Add("scmd", 1);
                    datas.Add("current", TestParam.CVSensitivity);
                    CommandDriver.SendFrame(CMDDriver.commandServer.GetSessionByID(CommunicationManager.GetDefaultDevice().CommandSessionId), datas);

                    datas.Clear();
                    datas.Add("cmd", 6);
                    datas.Add("scmd", 1);
                    //设备参数信息
                    datas.Add("vtg_init", TestParam.CVInitVolt);
                    datas.Add("quittime", TestParam.CVStopTime);
                    datas.Add("vtg_max", TestParam.CVMaxVolt);
                    datas.Add("vtg_min", TestParam.CVMinVolt);
                    datas.Add("dir", TestParam.CVScanDirection);
                    datas.Add("vtg_rate", TestParam.CVScanSpeed);
                    datas.Add("vtg_sample", TestParam.CVInterval);
                    datas.Add("nsegs", TestParam.CVScanCount);
                    CommandDriver.SendFrame(CMDDriver.commandServer.GetSessionByID(CommunicationManager.GetDefaultDevice().CommandSessionId), datas);
                    break;
            }
        }

        /// <summary>
        /// 初始化主界面中各个步骤的图标
        /// </summary>
        private void InitMainStepImage()
        {
            for (int i = 0; i < mainsteps.Count; i++)
            {
                mainsteps[i].UpdatePlayBackImage(ImageUnPlay, false, 0);
            }

            if (mainsteps.Count > 0)
                mainsteps[0].UpdatePlayBackImage(ImageMainStop, true, 0);
        }

        private void cbITSensitivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value = 0;
            if (cbITSensitivity.SelectedIndex != 0)
            {
                value = cbITSensitivity.SelectedIndex + 2;
            }

            IniWrapper.Write("ITParam", "Sensitivity", value, SystemParam.IniFileFullName);
            TestParam.ITSensitivity = value.ToString();
        }

        private void cbCVSensitivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value = 0;
            if (cbCVSensitivity.SelectedIndex != 0)
            {
                value = cbCVSensitivity.SelectedIndex + 2;
            }

            IniWrapper.Write("CVParam", "Sensitivity", value, SystemParam.IniFileFullName);
            TestParam.CVSensitivity = value.ToString();
        }

        private void cbSelfCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSelfCheck.Checked)
            {
                IniWrapper.Write("SystemParam", "SelfCheck", true, SystemParam.IniFileFullName);
                JudgeParam.SelfCheck = true;
            }
            else
            {
                IniWrapper.Write("SystemParam", "SelfCheck", false, SystemParam.IniFileFullName);
                JudgeParam.SelfCheck = false;
            }
        }

        private void cbAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoSave.Checked)
            {
                IniWrapper.Write("SystemParam", "AutoSave", true, SystemParam.IniFileFullName);
                JudgeParam.AutoSave = true;
            }
            else
            {
                IniWrapper.Write("SystemParam", "AutoSave", false, SystemParam.IniFileFullName);
                JudgeParam.AutoSave = false;
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        private void SaveTestResult(String SavePath = null)
        {
            try
            {
                // 测试完成后自动进行数据文件存储
                TestResultSerialize testResultSerialize = new TestResultSerialize(CommunicationManager.ITValues, CommunicationManager.CVValues, CommunicationManager.NodeStatus);
                string path = System.AppDomain.CurrentDomain.BaseDirectory + @"ResultFile";
                
                if (SavePath != null)
                    path = SavePath;
                else
                {
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    path += "\\Result_" + DateTime.Now.ToString("yyyyMMddHHmmss")+".bin";
                }

                bool result = SerializeUtil.SaveObject(path, testResultSerialize);
                if (result)
                {
                    logger.Info("测试结果保存成功");
                    lbMessage.Invoke(new Action(() => lbMessage.Text = "测试结果保存成功!"));  // 跨线程访问UI控件
                }
                else
                    logger.Info("测试结果保存失败");
            }
            catch (Exception)
            {
                logger.Info("测试结果保存失败");
            }
        }

        /// <summary>
        /// 手动保存测试结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveResult_Click(object sender, EventArgs e)
        {
            //string localFilePath, fileNameExt, newFileName, FilePath; 
            SaveFileDialog sfd = new SaveFileDialog();

            //设置文件类型 
            sfd.Filter = "ECH192文件（*.bin）|*.bin";

            //设置默认文件类型显示顺序 
            sfd.FilterIndex = 1;

            //保存对话框是否记忆上次打开的目录 
            sfd.RestoreDirectory = true;

            //点了保存按钮进入 
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string localFilePath = sfd.FileName.ToString(); //获得文件路径 
                SaveTestResult(localFilePath);
            }
        }

        /// <summary>
        /// 手动加载测试结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadResult_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";
            dialog.Filter = "ECH192文件(*.bin)|*.bin";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = dialog.FileName;

                TestResultSerialize result = new TestResultSerialize();
                try
                {
                    result = SerializeUtil.ReadObject<TestResultSerialize>(file);
                }
                catch (Exception)
                {
                    MsgForm msgForm = new MsgForm("提示", "文件解析失败!");
                    msgForm.ShowDialog();
                    return;
                }

                CommunicationManager.CVValues = result.CVValues;
                CommunicationManager.ITValues = result.ITValues;
                CommunicationManager.NodeStatus = result.NodeStatus;

                UpdateDataSource(1);
                UpdateDataSource(2);
                UpdateNodeStatus(CommunicationManager.NodeStatus);
                lbMessage.Invoke(new Action(() => lbMessage.Text = "测试结果文件打开成功!"));  // 跨线程访问UI控件
            }
        }
    }
}
