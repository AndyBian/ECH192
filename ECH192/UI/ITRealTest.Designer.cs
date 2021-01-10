namespace ECH192.UI
{
    partial class ITRealTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ITRealTest));
            this.btnzoom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnzoom
            // 
            this.btnzoom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnzoom.BackgroundImage")));
            this.btnzoom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnzoom.FlatAppearance.BorderSize = 0;
            this.btnzoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnzoom.Location = new System.Drawing.Point(380, 4);
            this.btnzoom.Name = "btnzoom";
            this.btnzoom.Size = new System.Drawing.Size(12, 12);
            this.btnzoom.TabIndex = 0;
            this.btnzoom.UseVisualStyleBackColor = true;
            this.btnzoom.Click += new System.EventHandler(this.btnzoom_Click);
            // 
            // ITRealTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnzoom);
            this.Name = "ITRealTest";
            this.Size = new System.Drawing.Size(399, 276);
            this.MouseEnter += new System.EventHandler(this.MouseEnterEvent);
            this.MouseLeave += new System.EventHandler(this.MouseLeaveEvent);
            this.Resize += new System.EventHandler(this.ITRealTest_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnzoom;
    }
}
