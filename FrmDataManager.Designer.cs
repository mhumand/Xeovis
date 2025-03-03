namespace DatabaseReader
{
    partial class FrmDataManager
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
            this.btnReadExperiments = new System.Windows.Forms.Button();
            this.cklbxExperiments = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnScanDirectory = new System.Windows.Forms.Button();
            this.cmbxExperiments = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbxManualSelection = new System.Windows.Forms.ComboBox();
            this.ckbxManualSelection = new System.Windows.Forms.CheckBox();
            this.cmbxPhases = new System.Windows.Forms.ComboBox();
            this.ckbxSubDirectory = new System.Windows.Forms.CheckBox();
            this.txtSubDirectory = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblAllPhases = new System.Windows.Forms.Label();
            this.txtAllPhases = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageDataManager = new System.Windows.Forms.TabPage();
            this.tabControlDataManager = new System.Windows.Forms.TabControl();
            this.tabPageDataManager_General = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnSubselectRuns = new System.Windows.Forms.Button();
            this.tabPageDataManager_Advanced = new System.Windows.Forms.TabPage();
            this.lblPhaseDetail = new System.Windows.Forms.Label();
            this.cklbxDMAdvancedOtherPhases = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbxDMAdvancedPhaseList = new System.Windows.Forms.ComboBox();
            this.tabPageDataManager_StudiesOverview = new System.Windows.Forms.TabPage();
            this.ckbxShortNameForStudies = new System.Windows.Forms.CheckBox();
            this.tabPageSubselectRuns = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbxRunFilterType = new System.Windows.Forms.ComboBox();
            this.btnGenerateRuns = new System.Windows.Forms.Button();
            this.flpFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddRunFilterType = new System.Windows.Forms.Button();
            this.tcExperiment = new DatabaseReader.CustomControls.TableControl();
            this.tcPhases = new DatabaseReader.CustomControls.TableControl();
            this.tcADPhaseCompSummary = new DatabaseReader.CustomControls.TableControl();
            this.tcAdvancedDataManager = new DatabaseReader.CustomControls.TableControl();
            this.tcStudiesOverview = new DatabaseReader.CustomControls.TableControl();
            this.tcFilteredRuns = new DatabaseReader.CustomControls.TableControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageDataManager.SuspendLayout();
            this.tabControlDataManager.SuspendLayout();
            this.tabPageDataManager_General.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPageDataManager_Advanced.SuspendLayout();
            this.tabPageDataManager_StudiesOverview.SuspendLayout();
            this.tabPageSubselectRuns.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReadExperiments
            // 
            this.btnReadExperiments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadExperiments.Location = new System.Drawing.Point(6, 74);
            this.btnReadExperiments.Name = "btnReadExperiments";
            this.btnReadExperiments.Size = new System.Drawing.Size(181, 23);
            this.btnReadExperiments.TabIndex = 2;
            this.btnReadExperiments.Text = "Read Selected Experiments";
            this.btnReadExperiments.UseVisualStyleBackColor = true;
            this.btnReadExperiments.Click += new System.EventHandler(this.btnReadExperiments_Click);
            // 
            // cklbxExperiments
            // 
            this.cklbxExperiments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cklbxExperiments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cklbxExperiments.CheckOnClick = true;
            this.cklbxExperiments.FormattingEnabled = true;
            this.cklbxExperiments.HorizontalScrollbar = true;
            this.cklbxExperiments.Location = new System.Drawing.Point(6, 107);
            this.cklbxExperiments.Name = "cklbxExperiments";
            this.cklbxExperiments.Size = new System.Drawing.Size(181, 459);
            this.cklbxExperiments.TabIndex = 3;
            this.cklbxExperiments.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cklbxExperiments_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Directory";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(87, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(629, 22);
            this.txtPath.TabIndex = 1;
            // 
            // btnScanDirectory
            // 
            this.btnScanDirectory.Location = new System.Drawing.Point(722, 12);
            this.btnScanDirectory.Name = "btnScanDirectory";
            this.btnScanDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnScanDirectory.TabIndex = 2;
            this.btnScanDirectory.Text = "Scan";
            this.btnScanDirectory.UseVisualStyleBackColor = true;
            this.btnScanDirectory.Click += new System.EventHandler(this.btnScanDirectory_Click);
            // 
            // cmbxExperiments
            // 
            this.cmbxExperiments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxExperiments.FormattingEnabled = true;
            this.cmbxExperiments.Location = new System.Drawing.Point(6, 22);
            this.cmbxExperiments.Name = "cmbxExperiments";
            this.cmbxExperiments.Size = new System.Drawing.Size(201, 21);
            this.cmbxExperiments.TabIndex = 4;
            this.cmbxExperiments.SelectedIndexChanged += new System.EventHandler(this.cmbxExperiments_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.cmbxManualSelection);
            this.groupBox1.Controls.Add(this.ckbxManualSelection);
            this.groupBox1.Controls.Add(this.btnReadExperiments);
            this.groupBox1.Controls.Add(this.cklbxExperiments);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 594);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Studies with data";
            // 
            // cmbxManualSelection
            // 
            this.cmbxManualSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbxManualSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxManualSelection.FormattingEnabled = true;
            this.cmbxManualSelection.Location = new System.Drawing.Point(6, 47);
            this.cmbxManualSelection.Name = "cmbxManualSelection";
            this.cmbxManualSelection.Size = new System.Drawing.Size(181, 21);
            this.cmbxManualSelection.TabIndex = 7;
            this.cmbxManualSelection.SelectedIndexChanged += new System.EventHandler(this.cmbxManualSelection_SelectedIndexChanged);
            // 
            // ckbxManualSelection
            // 
            this.ckbxManualSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxManualSelection.AutoSize = true;
            this.ckbxManualSelection.Location = new System.Drawing.Point(8, 24);
            this.ckbxManualSelection.Name = "ckbxManualSelection";
            this.ckbxManualSelection.Size = new System.Drawing.Size(115, 17);
            this.ckbxManualSelection.TabIndex = 7;
            this.ckbxManualSelection.Text = "Manual Selection";
            this.ckbxManualSelection.UseVisualStyleBackColor = true;
            this.ckbxManualSelection.CheckedChanged += new System.EventHandler(this.ckbxManualSelection_CheckedChanged);
            // 
            // cmbxPhases
            // 
            this.cmbxPhases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxPhases.FormattingEnabled = true;
            this.cmbxPhases.Location = new System.Drawing.Point(6, 21);
            this.cmbxPhases.Name = "cmbxPhases";
            this.cmbxPhases.Size = new System.Drawing.Size(201, 21);
            this.cmbxPhases.TabIndex = 4;
            this.cmbxPhases.SelectedIndexChanged += new System.EventHandler(this.cmbxPhases_SelectedIndexChanged);
            // 
            // ckbxSubDirectory
            // 
            this.ckbxSubDirectory.AutoSize = true;
            this.ckbxSubDirectory.Checked = true;
            this.ckbxSubDirectory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxSubDirectory.Location = new System.Drawing.Point(803, 16);
            this.ckbxSubDirectory.Name = "ckbxSubDirectory";
            this.ckbxSubDirectory.Size = new System.Drawing.Size(132, 17);
            this.ckbxSubDirectory.TabIndex = 8;
            this.ckbxSubDirectory.Text = "Include Subdirectory";
            this.ckbxSubDirectory.UseVisualStyleBackColor = true;
            this.ckbxSubDirectory.CheckedChanged += new System.EventHandler(this.ckbxSubDirectory_CheckedChanged);
            // 
            // txtSubDirectory
            // 
            this.txtSubDirectory.Location = new System.Drawing.Point(932, 13);
            this.txtSubDirectory.Name = "txtSubDirectory";
            this.txtSubDirectory.Size = new System.Drawing.Size(100, 22);
            this.txtSubDirectory.TabIndex = 9;
            this.txtSubDirectory.Text = "MH";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblAllPhases);
            this.groupBox2.Controls.Add(this.txtAllPhases);
            this.groupBox2.Controls.Add(this.cmbxExperiments);
            this.groupBox2.Controls.Add(this.tcExperiment);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1019, 262);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Overview";
            // 
            // lblAllPhases
            // 
            this.lblAllPhases.AutoSize = true;
            this.lblAllPhases.Location = new System.Drawing.Point(237, 25);
            this.lblAllPhases.Name = "lblAllPhases";
            this.lblAllPhases.Size = new System.Drawing.Size(62, 13);
            this.lblAllPhases.TabIndex = 6;
            this.lblAllPhases.Text = "All phases:";
            // 
            // txtAllPhases
            // 
            this.txtAllPhases.Location = new System.Drawing.Point(305, 21);
            this.txtAllPhases.Name = "txtAllPhases";
            this.txtAllPhases.ReadOnly = true;
            this.txtAllPhases.Size = new System.Drawing.Size(370, 22);
            this.txtAllPhases.TabIndex = 9;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tcPhases);
            this.groupBox3.Controls.Add(this.cmbxPhases);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1019, 263);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Phases";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageDataManager);
            this.tabControlMain.Controls.Add(this.tabPageSubselectRuns);
            this.tabControlMain.Location = new System.Drawing.Point(16, 41);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1257, 629);
            this.tabControlMain.TabIndex = 10;
            this.tabControlMain.Visible = false;
            // 
            // tabPageDataManager
            // 
            this.tabPageDataManager.Controls.Add(this.tabControlDataManager);
            this.tabPageDataManager.Controls.Add(this.groupBox1);
            this.tabPageDataManager.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataManager.Name = "tabPageDataManager";
            this.tabPageDataManager.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataManager.Size = new System.Drawing.Size(1249, 603);
            this.tabPageDataManager.TabIndex = 0;
            this.tabPageDataManager.Text = "Data Manager";
            this.tabPageDataManager.UseVisualStyleBackColor = true;
            // 
            // tabControlDataManager
            // 
            this.tabControlDataManager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlDataManager.Controls.Add(this.tabPageDataManager_General);
            this.tabControlDataManager.Controls.Add(this.tabPageDataManager_Advanced);
            this.tabControlDataManager.Controls.Add(this.tabPageDataManager_StudiesOverview);
            this.tabControlDataManager.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControlDataManager.Location = new System.Drawing.Point(204, 6);
            this.tabControlDataManager.Name = "tabControlDataManager";
            this.tabControlDataManager.SelectedIndex = 0;
            this.tabControlDataManager.Size = new System.Drawing.Size(1039, 591);
            this.tabControlDataManager.TabIndex = 13;
            // 
            // tabPageDataManager_General
            // 
            this.tabPageDataManager_General.Controls.Add(this.splitContainer1);
            this.tabPageDataManager_General.Controls.Add(this.btnSubselectRuns);
            this.tabPageDataManager_General.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataManager_General.Name = "tabPageDataManager_General";
            this.tabPageDataManager_General.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataManager_General.Size = new System.Drawing.Size(1031, 565);
            this.tabPageDataManager_General.TabIndex = 0;
            this.tabPageDataManager_General.Text = "General";
            this.tabPageDataManager_General.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(6, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(1019, 529);
            this.splitContainer1.SplitterDistance = 262;
            this.splitContainer1.TabIndex = 12;
            // 
            // btnSubselectRuns
            // 
            this.btnSubselectRuns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubselectRuns.Location = new System.Drawing.Point(889, 538);
            this.btnSubselectRuns.Name = "btnSubselectRuns";
            this.btnSubselectRuns.Size = new System.Drawing.Size(136, 23);
            this.btnSubselectRuns.TabIndex = 11;
            this.btnSubselectRuns.Text = "Subselect Runs >>";
            this.btnSubselectRuns.UseVisualStyleBackColor = true;
            this.btnSubselectRuns.Click += new System.EventHandler(this.btnSubselectRuns_Click);
            // 
            // tabPageDataManager_Advanced
            // 
            this.tabPageDataManager_Advanced.Controls.Add(this.tcADPhaseCompSummary);
            this.tabPageDataManager_Advanced.Controls.Add(this.lblPhaseDetail);
            this.tabPageDataManager_Advanced.Controls.Add(this.cklbxDMAdvancedOtherPhases);
            this.tabPageDataManager_Advanced.Controls.Add(this.label1);
            this.tabPageDataManager_Advanced.Controls.Add(this.cmbxDMAdvancedPhaseList);
            this.tabPageDataManager_Advanced.Controls.Add(this.tcAdvancedDataManager);
            this.tabPageDataManager_Advanced.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataManager_Advanced.Name = "tabPageDataManager_Advanced";
            this.tabPageDataManager_Advanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataManager_Advanced.Size = new System.Drawing.Size(1031, 565);
            this.tabPageDataManager_Advanced.TabIndex = 1;
            this.tabPageDataManager_Advanced.Text = "Advanced";
            this.tabPageDataManager_Advanced.UseVisualStyleBackColor = true;
            // 
            // lblPhaseDetail
            // 
            this.lblPhaseDetail.AutoSize = true;
            this.lblPhaseDetail.Location = new System.Drawing.Point(6, 31);
            this.lblPhaseDetail.Name = "lblPhaseDetail";
            this.lblPhaseDetail.Size = new System.Drawing.Size(16, 13);
            this.lblPhaseDetail.TabIndex = 4;
            this.lblPhaseDetail.Text = "...";
            // 
            // cklbxDMAdvancedOtherPhases
            // 
            this.cklbxDMAdvancedOtherPhases.CheckOnClick = true;
            this.cklbxDMAdvancedOtherPhases.FormattingEnabled = true;
            this.cklbxDMAdvancedOtherPhases.Location = new System.Drawing.Point(200, 6);
            this.cklbxDMAdvancedOtherPhases.MultiColumn = true;
            this.cklbxDMAdvancedOtherPhases.Name = "cklbxDMAdvancedOtherPhases";
            this.cklbxDMAdvancedOtherPhases.Size = new System.Drawing.Size(825, 21);
            this.cklbxDMAdvancedOtherPhases.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Phase";
            // 
            // cmbxDMAdvancedPhaseList
            // 
            this.cmbxDMAdvancedPhaseList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxDMAdvancedPhaseList.FormattingEnabled = true;
            this.cmbxDMAdvancedPhaseList.Location = new System.Drawing.Point(49, 6);
            this.cmbxDMAdvancedPhaseList.Name = "cmbxDMAdvancedPhaseList";
            this.cmbxDMAdvancedPhaseList.Size = new System.Drawing.Size(145, 21);
            this.cmbxDMAdvancedPhaseList.TabIndex = 0;
            // 
            // tabPageDataManager_StudiesOverview
            // 
            this.tabPageDataManager_StudiesOverview.Controls.Add(this.ckbxShortNameForStudies);
            this.tabPageDataManager_StudiesOverview.Controls.Add(this.tcStudiesOverview);
            this.tabPageDataManager_StudiesOverview.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataManager_StudiesOverview.Name = "tabPageDataManager_StudiesOverview";
            this.tabPageDataManager_StudiesOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDataManager_StudiesOverview.Size = new System.Drawing.Size(1031, 565);
            this.tabPageDataManager_StudiesOverview.TabIndex = 2;
            this.tabPageDataManager_StudiesOverview.Text = "Overview";
            this.tabPageDataManager_StudiesOverview.UseVisualStyleBackColor = true;
            // 
            // ckbxShortNameForStudies
            // 
            this.ckbxShortNameForStudies.AutoSize = true;
            this.ckbxShortNameForStudies.Location = new System.Drawing.Point(6, 6);
            this.ckbxShortNameForStudies.Name = "ckbxShortNameForStudies";
            this.ckbxShortNameForStudies.Size = new System.Drawing.Size(164, 17);
            this.ckbxShortNameForStudies.TabIndex = 1;
            this.ckbxShortNameForStudies.Text = "Use short name for studies";
            this.ckbxShortNameForStudies.UseVisualStyleBackColor = true;
            this.ckbxShortNameForStudies.CheckedChanged += new System.EventHandler(this.ckbxShortNameForStudies_CheckedChanged);
            // 
            // tabPageSubselectRuns
            // 
            this.tabPageSubselectRuns.Controls.Add(this.groupBox4);
            this.tabPageSubselectRuns.Controls.Add(this.tcFilteredRuns);
            this.tabPageSubselectRuns.Location = new System.Drawing.Point(4, 22);
            this.tabPageSubselectRuns.Name = "tabPageSubselectRuns";
            this.tabPageSubselectRuns.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSubselectRuns.Size = new System.Drawing.Size(1249, 603);
            this.tabPageSubselectRuns.TabIndex = 1;
            this.tabPageSubselectRuns.Text = "Subselect Runs";
            this.tabPageSubselectRuns.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.cmbxRunFilterType);
            this.groupBox4.Controls.Add(this.btnGenerateRuns);
            this.groupBox4.Controls.Add(this.flpFilters);
            this.groupBox4.Controls.Add(this.btnAddRunFilterType);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(213, 591);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Add Filters";
            // 
            // cmbxRunFilterType
            // 
            this.cmbxRunFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxRunFilterType.FormattingEnabled = true;
            this.cmbxRunFilterType.Location = new System.Drawing.Point(6, 21);
            this.cmbxRunFilterType.Name = "cmbxRunFilterType";
            this.cmbxRunFilterType.Size = new System.Drawing.Size(153, 21);
            this.cmbxRunFilterType.TabIndex = 3;
            // 
            // btnGenerateRuns
            // 
            this.btnGenerateRuns.Location = new System.Drawing.Point(6, 565);
            this.btnGenerateRuns.Name = "btnGenerateRuns";
            this.btnGenerateRuns.Size = new System.Drawing.Size(200, 23);
            this.btnGenerateRuns.TabIndex = 5;
            this.btnGenerateRuns.Text = "Apply and Generate";
            this.btnGenerateRuns.UseVisualStyleBackColor = true;
            this.btnGenerateRuns.Click += new System.EventHandler(this.btnGenerateRuns_Click);
            // 
            // flpFilters
            // 
            this.flpFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpFilters.Location = new System.Drawing.Point(6, 47);
            this.flpFilters.Name = "flpFilters";
            this.flpFilters.Size = new System.Drawing.Size(200, 512);
            this.flpFilters.TabIndex = 0;
            // 
            // btnAddRunFilterType
            // 
            this.btnAddRunFilterType.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRunFilterType.Location = new System.Drawing.Point(162, 20);
            this.btnAddRunFilterType.Name = "btnAddRunFilterType";
            this.btnAddRunFilterType.Size = new System.Drawing.Size(44, 23);
            this.btnAddRunFilterType.TabIndex = 4;
            this.btnAddRunFilterType.Text = "Add";
            this.btnAddRunFilterType.UseVisualStyleBackColor = true;
            this.btnAddRunFilterType.Click += new System.EventHandler(this.btnAddRunFilterType_Click);
            // 
            // tcExperiment
            // 
            this.tcExperiment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcExperiment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcExperiment.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcExperiment.Location = new System.Drawing.Point(6, 49);
            this.tcExperiment.MultiSelect = true;
            this.tcExperiment.Name = "tcExperiment";
            this.tcExperiment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcExperiment.Size = new System.Drawing.Size(1007, 207);
            this.tcExperiment.TabIndex = 5;
            // 
            // tcPhases
            // 
            this.tcPhases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcPhases.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcPhases.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcPhases.Location = new System.Drawing.Point(6, 48);
            this.tcPhases.MultiSelect = true;
            this.tcPhases.Name = "tcPhases";
            this.tcPhases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcPhases.Size = new System.Drawing.Size(1007, 209);
            this.tcPhases.TabIndex = 5;
            // 
            // tcADPhaseCompSummary
            // 
            this.tcADPhaseCompSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcADPhaseCompSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcADPhaseCompSummary.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcADPhaseCompSummary.Location = new System.Drawing.Point(6, 458);
            this.tcADPhaseCompSummary.MultiSelect = true;
            this.tcADPhaseCompSummary.Name = "tcADPhaseCompSummary";
            this.tcADPhaseCompSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcADPhaseCompSummary.Size = new System.Drawing.Size(1019, 107);
            this.tcADPhaseCompSummary.TabIndex = 5;
            // 
            // tcAdvancedDataManager
            // 
            this.tcAdvancedDataManager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcAdvancedDataManager.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcAdvancedDataManager.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcAdvancedDataManager.Location = new System.Drawing.Point(6, 50);
            this.tcAdvancedDataManager.MultiSelect = true;
            this.tcAdvancedDataManager.Name = "tcAdvancedDataManager";
            this.tcAdvancedDataManager.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcAdvancedDataManager.Size = new System.Drawing.Size(1019, 402);
            this.tcAdvancedDataManager.TabIndex = 2;
            // 
            // tcStudiesOverview
            // 
            this.tcStudiesOverview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcStudiesOverview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcStudiesOverview.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcStudiesOverview.Location = new System.Drawing.Point(6, 29);
            this.tcStudiesOverview.MultiSelect = true;
            this.tcStudiesOverview.Name = "tcStudiesOverview";
            this.tcStudiesOverview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcStudiesOverview.Size = new System.Drawing.Size(1019, 533);
            this.tcStudiesOverview.TabIndex = 0;
            // 
            // tcFilteredRuns
            // 
            this.tcFilteredRuns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcFilteredRuns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcFilteredRuns.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcFilteredRuns.Location = new System.Drawing.Point(225, 26);
            this.tcFilteredRuns.MultiSelect = true;
            this.tcFilteredRuns.Name = "tcFilteredRuns";
            this.tcFilteredRuns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcFilteredRuns.Size = new System.Drawing.Size(1018, 536);
            this.tcFilteredRuns.TabIndex = 1;
            // 
            // FrmDataManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 696);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.txtSubDirectory);
            this.Controls.Add(this.ckbxSubDirectory);
            this.Controls.Add(this.btnScanDirectory);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmDataManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Manager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageDataManager.ResumeLayout(false);
            this.tabControlDataManager.ResumeLayout(false);
            this.tabPageDataManager_General.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPageDataManager_Advanced.ResumeLayout(false);
            this.tabPageDataManager_Advanced.PerformLayout();
            this.tabPageDataManager_StudiesOverview.ResumeLayout(false);
            this.tabPageDataManager_StudiesOverview.PerformLayout();
            this.tabPageSubselectRuns.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnReadExperiments;
        private System.Windows.Forms.CheckedListBox cklbxExperiments;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnScanDirectory;
        private System.Windows.Forms.ComboBox cmbxExperiments;
        private CustomControls.TableControl tcExperiment;
        private System.Windows.Forms.CheckBox ckbxSubDirectory;
        private System.Windows.Forms.TextBox txtSubDirectory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbxPhases;
        private CustomControls.TableControl tcPhases;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDataManager;
        private System.Windows.Forms.ComboBox cmbxManualSelection;
        private System.Windows.Forms.CheckBox ckbxManualSelection;
        private System.Windows.Forms.TabPage tabPageSubselectRuns;
        private System.Windows.Forms.Button btnSubselectRuns;
        private System.Windows.Forms.Button btnAddRunFilterType;
        private System.Windows.Forms.ComboBox cmbxRunFilterType;
        private CustomControls.TableControl tcFilteredRuns;
        private System.Windows.Forms.FlowLayoutPanel flpFilters;
        private System.Windows.Forms.Button btnGenerateRuns;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControlDataManager;
        private System.Windows.Forms.TabPage tabPageDataManager_General;
        private System.Windows.Forms.TabPage tabPageDataManager_Advanced;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbxDMAdvancedPhaseList;
        private CustomControls.TableControl tcAdvancedDataManager;
        private System.Windows.Forms.CheckedListBox cklbxDMAdvancedOtherPhases;
        private System.Windows.Forms.Label lblPhaseDetail;
        private CustomControls.TableControl tcADPhaseCompSummary;
        private System.Windows.Forms.Label lblAllPhases;
        private System.Windows.Forms.TextBox txtAllPhases;
        private System.Windows.Forms.TabPage tabPageDataManager_StudiesOverview;
        private CustomControls.TableControl tcStudiesOverview;
        private System.Windows.Forms.CheckBox ckbxShortNameForStudies;
    }
}

