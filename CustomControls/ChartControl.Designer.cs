namespace DatabaseReader.CustomControls
{
    partial class ChartControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.zgChart = new ZedGraph.ZedGraphControl();
            this.imglistToolbar = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // zgChart
            // 
            this.zgChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zgChart.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zgChart.Location = new System.Drawing.Point(0, 0);
            this.zgChart.Name = "zgChart";
            this.zgChart.ScrollGrace = 0D;
            this.zgChart.ScrollMaxX = 0D;
            this.zgChart.ScrollMaxY = 0D;
            this.zgChart.ScrollMaxY2 = 0D;
            this.zgChart.ScrollMinX = 0D;
            this.zgChart.ScrollMinY = 0D;
            this.zgChart.ScrollMinY2 = 0D;
            this.zgChart.Size = new System.Drawing.Size(500, 500);
            this.zgChart.TabIndex = 0;
            this.zgChart.UseExtendedPrintDialog = true;
            this.zgChart.ZoomEvent += new ZedGraph.ZedGraphControl.ZoomEventHandler(this.zgChart_ZoomEvent);
            this.zgChart.MouseClick += zgChart_MouseClick;
            this.zgChart.KeyDown += zgChart_KeyDown;
            // 
            // imglistToolbar
            // 
            this.imglistToolbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imglistToolbar.ImageSize = new System.Drawing.Size(16, 16);
            this.imglistToolbar.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zgChart);
            this.Name = "ChartControl";
            this.Size = new System.Drawing.Size(500, 500);
            this.Resize += new System.EventHandler(this.ChartControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zgChart;
        private System.Windows.Forms.ImageList imglistToolbar;
    }
}
