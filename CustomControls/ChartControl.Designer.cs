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
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomReset = new System.Windows.Forms.Button();
            this.imglistToolbar = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
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
            this.zgChart.Size = new System.Drawing.Size(502, 460);
            this.zgChart.TabIndex = 0;
            this.zgChart.UseExtendedPrintDialog = true;
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.Image = global::DatabaseReader.Properties.Resources.ZoomIn16x16;
            this.btnZoomIn.Location = new System.Drawing.Point(4, 0);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(34, 34);
            this.btnZoomIn.TabIndex = 2;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomOut.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomOut.Image = global::DatabaseReader.Properties.Resources.ZoomOut16x16;
            this.btnZoomOut.Location = new System.Drawing.Point(4, 40);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(34, 34);
            this.btnZoomOut.TabIndex = 2;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            // 
            // btnZoomReset
            // 
            this.btnZoomReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomReset.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomReset.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomReset.Image = global::DatabaseReader.Properties.Resources.ZoomReset16x16;
            this.btnZoomReset.Location = new System.Drawing.Point(4, 80);
            this.btnZoomReset.Name = "btnZoomReset";
            this.btnZoomReset.Size = new System.Drawing.Size(34, 34);
            this.btnZoomReset.TabIndex = 2;
            this.btnZoomReset.UseVisualStyleBackColor = false;
            // 
            // imglistToolbar
            // 
            this.imglistToolbar.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imglistToolbar.ImageSize = new System.Drawing.Size(16, 16);
            this.imglistToolbar.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnZoomIn);
            this.panel1.Controls.Add(this.btnZoomReset);
            this.panel1.Controls.Add(this.btnZoomOut);
            this.panel1.Location = new System.Drawing.Point(504, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(41, 460);
            this.panel1.TabIndex = 3;
            // 
            // ChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.zgChart);
            this.Name = "ChartControl";
            this.Size = new System.Drawing.Size(545, 460);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zgChart;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomReset;
        private System.Windows.Forms.ImageList imglistToolbar;
        private System.Windows.Forms.Panel panel1;
    }
}
