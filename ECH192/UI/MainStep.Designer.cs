namespace ECH192.UI
{
    partial class MainStep
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainStep));
            this.lbMessage = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnRealTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbMessage
            // 
            this.lbMessage.AutoSize = true;
            this.lbMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbMessage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(169)))), ((int)(((byte)(206)))));
            this.lbMessage.Location = new System.Drawing.Point(0, 0);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(32, 17);
            this.lbMessage.TabIndex = 0;
            this.lbMessage.Text = "标题";
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(169)))), ((int)(((byte)(206)))));
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 35);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(45, 5);
            this.progressBar.TabIndex = 1;
            // 
            // btnRealTest
            // 
            this.btnRealTest.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRealTest.BackgroundImage")));
            this.btnRealTest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRealTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRealTest.Enabled = false;
            this.btnRealTest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnRealTest.FlatAppearance.BorderSize = 0;
            this.btnRealTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRealTest.ForeColor = System.Drawing.SystemColors.Control;
            this.btnRealTest.Location = new System.Drawing.Point(0, 17);
            this.btnRealTest.Name = "btnRealTest";
            this.btnRealTest.Size = new System.Drawing.Size(45, 18);
            this.btnRealTest.TabIndex = 5;
            this.btnRealTest.UseVisualStyleBackColor = true;
            this.btnRealTest.Click += new System.EventHandler(this.btnRealTest_Click);
            // 
            // MainStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRealTest);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lbMessage);
            this.Name = "MainStep";
            this.Size = new System.Drawing.Size(45, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbMessage;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnRealTest;
    }
}
