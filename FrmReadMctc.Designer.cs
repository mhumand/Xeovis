namespace DatabaseReader
{
    partial class FrmReadMctc
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnReadMctc = new System.Windows.Forms.Button();
            this.tvCaseSelection = new System.Windows.Forms.TreeView();
            this.btnPlotAndTabulate = new System.Windows.Forms.Button();
            this.ckbxIncludeBatches = new System.Windows.Forms.CheckBox();
            this.cmbxModelVariable = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudTransparency = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ccMctcPlot = new DatabaseReader.CustomControls.ChartControl();
            this.tcPlotInfo = new DatabaseReader.CustomControls.TableControl();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReadMctc
            // 
            this.btnReadMctc.Location = new System.Drawing.Point(22, 41);
            this.btnReadMctc.Name = "btnReadMctc";
            this.btnReadMctc.Size = new System.Drawing.Size(173, 23);
            this.btnReadMctc.TabIndex = 0;
            this.btnReadMctc.Text = "Read Mctc Results";
            this.btnReadMctc.UseVisualStyleBackColor = true;
            this.btnReadMctc.Click += new System.EventHandler(this.btnReadMctc_Click);
            // 
            // tvCaseSelection
            // 
            this.tvCaseSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvCaseSelection.CheckBoxes = true;
            this.tvCaseSelection.FullRowSelect = true;
            this.tvCaseSelection.Location = new System.Drawing.Point(22, 93);
            this.tvCaseSelection.Name = "tvCaseSelection";
            this.tvCaseSelection.Size = new System.Drawing.Size(173, 607);
            this.tvCaseSelection.TabIndex = 1;
            this.tvCaseSelection.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvCaseSelection_AfterCheck);
            this.tvCaseSelection.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvCaseSelection_MouseClick);
            // 
            // btnPlotAndTabulate
            // 
            this.btnPlotAndTabulate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlotAndTabulate.Location = new System.Drawing.Point(22, 731);
            this.btnPlotAndTabulate.Name = "btnPlotAndTabulate";
            this.btnPlotAndTabulate.Size = new System.Drawing.Size(173, 23);
            this.btnPlotAndTabulate.TabIndex = 2;
            this.btnPlotAndTabulate.Text = "Plot and Tabulate";
            this.btnPlotAndTabulate.UseVisualStyleBackColor = true;
            this.btnPlotAndTabulate.Click += new System.EventHandler(this.btnPlotAndTabulate_Click);
            // 
            // ckbxIncludeBatches
            // 
            this.ckbxIncludeBatches.AutoSize = true;
            this.ckbxIncludeBatches.Location = new System.Drawing.Point(28, 70);
            this.ckbxIncludeBatches.Name = "ckbxIncludeBatches";
            this.ckbxIncludeBatches.Size = new System.Drawing.Size(107, 17);
            this.ckbxIncludeBatches.TabIndex = 3;
            this.ckbxIncludeBatches.Text = "Include batches";
            this.ckbxIncludeBatches.UseVisualStyleBackColor = true;
            // 
            // cmbxModelVariable
            // 
            this.cmbxModelVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxModelVariable.FormattingEnabled = true;
            this.cmbxModelVariable.Location = new System.Drawing.Point(95, 3);
            this.cmbxModelVariable.Name = "cmbxModelVariable";
            this.cmbxModelVariable.Size = new System.Drawing.Size(121, 21);
            this.cmbxModelVariable.TabIndex = 5;
            this.cmbxModelVariable.SelectedIndexChanged += new System.EventHandler(this.cmbxModelVariable_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Model Variable";
            // 
            // nudTransparency
            // 
            this.nudTransparency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudTransparency.Location = new System.Drawing.Point(135, 706);
            this.nudTransparency.Name = "nudTransparency";
            this.nudTransparency.Size = new System.Drawing.Size(60, 22);
            this.nudTransparency.TabIndex = 7;
            this.nudTransparency.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 708);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Color transparency";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(201, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.cmbxModelVariable);
            this.splitContainer1.Panel1.Controls.Add(this.ccMctcPlot);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tcPlotInfo);
            this.splitContainer1.Size = new System.Drawing.Size(1150, 742);
            this.splitContainer1.SplitterDistance = 630;
            this.splitContainer1.TabIndex = 9;
            // 
            // ccMctcPlot
            // 
            this.ccMctcPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ccMctcPlot.Location = new System.Drawing.Point(3, 30);
            this.ccMctcPlot.Name = "ccMctcPlot";
            this.ccMctcPlot.Size = new System.Drawing.Size(624, 625);
            this.ccMctcPlot.TabIndex = 4;
            this.ccMctcPlot.UseLabelFiltering = false;
            // 
            // tcPlotInfo
            // 
            this.tcPlotInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcPlotInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcPlotInfo.Location = new System.Drawing.Point(0, 0);
            this.tcPlotInfo.MultiSelect = true;
            this.tcPlotInfo.Name = "tcPlotInfo";
            this.tcPlotInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcPlotInfo.Size = new System.Drawing.Size(516, 742);
            this.tcPlotInfo.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(113, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(22, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(82, 23);
            this.btnOpen.TabIndex = 10;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // FrmReadMctc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1363, 766);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.nudTransparency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ckbxIncludeBatches);
            this.Controls.Add(this.btnPlotAndTabulate);
            this.Controls.Add(this.tvCaseSelection);
            this.Controls.Add(this.btnReadMctc);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmReadMctc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Read Mctc";
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparency)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadMctc;
        private System.Windows.Forms.TreeView tvCaseSelection;
        private System.Windows.Forms.Button btnPlotAndTabulate;
        private System.Windows.Forms.CheckBox ckbxIncludeBatches;
        private CustomControls.ChartControl ccMctcPlot;
        private System.Windows.Forms.ComboBox cmbxModelVariable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudTransparency;
        private System.Windows.Forms.Label label2;
        private CustomControls.TableControl tcPlotInfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
    }
}