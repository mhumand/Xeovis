namespace DatabaseReader
{
    partial class FrmVariableDefinitions
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
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseAndRead = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbxPhase = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cklbxOxides = new System.Windows.Forms.CheckedListBox();
            this.lblPhaseFullName = new System.Windows.Forms.Label();
            this.cklbxCations = new System.Windows.Forms.CheckedListBox();
            this.tcEndmembers = new DatabaseReader.CustomControls.TableControl();
            this.lbl5 = new System.Windows.Forms.Label();
            this.cmbxModelVariable = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tcEnergyParameters = new DatabaseReader.CustomControls.TableControl();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath.Location = new System.Drawing.Point(91, 12);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(562, 22);
            this.txtFilePath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ax-File";
            // 
            // btnBrowseAndRead
            // 
            this.btnBrowseAndRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseAndRead.Location = new System.Drawing.Point(659, 12);
            this.btnBrowseAndRead.Name = "btnBrowseAndRead";
            this.btnBrowseAndRead.Size = new System.Drawing.Size(106, 22);
            this.btnBrowseAndRead.TabIndex = 2;
            this.btnBrowseAndRead.Text = "Browse and Read";
            this.btnBrowseAndRead.UseVisualStyleBackColor = true;
            this.btnBrowseAndRead.Click += new System.EventHandler(this.btnBrowseAndRead_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(609, 545);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(690, 545);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbxPhase
            // 
            this.cmbxPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxPhase.FormattingEnabled = true;
            this.cmbxPhase.Location = new System.Drawing.Point(91, 50);
            this.cmbxPhase.Name = "cmbxPhase";
            this.cmbxPhase.Size = new System.Drawing.Size(123, 21);
            this.cmbxPhase.TabIndex = 4;
            this.cmbxPhase.SelectedIndexChanged += new System.EventHandler(this.cmbxPhase_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Phase";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Composition";
            // 
            // cklbxOxides
            // 
            this.cklbxOxides.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cklbxOxides.ColumnWidth = 60;
            this.cklbxOxides.FormattingEnabled = true;
            this.cklbxOxides.Location = new System.Drawing.Point(91, 77);
            this.cklbxOxides.MultiColumn = true;
            this.cklbxOxides.Name = "cklbxOxides";
            this.cklbxOxides.Size = new System.Drawing.Size(673, 21);
            this.cklbxOxides.TabIndex = 5;
            // 
            // lblPhaseFullName
            // 
            this.lblPhaseFullName.AutoSize = true;
            this.lblPhaseFullName.Location = new System.Drawing.Point(220, 53);
            this.lblPhaseFullName.Name = "lblPhaseFullName";
            this.lblPhaseFullName.Size = new System.Drawing.Size(16, 13);
            this.lblPhaseFullName.TabIndex = 1;
            this.lblPhaseFullName.Text = "...";
            // 
            // cklbxCations
            // 
            this.cklbxCations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cklbxCations.ColumnWidth = 50;
            this.cklbxCations.FormattingEnabled = true;
            this.cklbxCations.Location = new System.Drawing.Point(221, 104);
            this.cklbxCations.MultiColumn = true;
            this.cklbxCations.Name = "cklbxCations";
            this.cklbxCations.Size = new System.Drawing.Size(543, 21);
            this.cklbxCations.TabIndex = 5;
            // 
            // tcEndmembers
            // 
            this.tcEndmembers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcEndmembers.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcEndmembers.Location = new System.Drawing.Point(91, 136);
            this.tcEndmembers.MultiSelect = true;
            this.tcEndmembers.Name = "tcEndmembers";
            this.tcEndmembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcEndmembers.Size = new System.Drawing.Size(242, 403);
            this.tcEndmembers.TabIndex = 6;
            // 
            // lbl5
            // 
            this.lbl5.AutoSize = true;
            this.lbl5.Location = new System.Drawing.Point(32, 107);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(53, 13);
            this.lbl5.TabIndex = 1;
            this.lbl5.Text = "Variables";
            // 
            // cmbxModelVariable
            // 
            this.cmbxModelVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxModelVariable.FormattingEnabled = true;
            this.cmbxModelVariable.Location = new System.Drawing.Point(92, 104);
            this.cmbxModelVariable.Name = "cmbxModelVariable";
            this.cmbxModelVariable.Size = new System.Drawing.Size(123, 21);
            this.cmbxModelVariable.TabIndex = 7;
            this.cmbxModelVariable.SelectedIndexChanged += new System.EventHandler(this.cmbxModelVariable_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Parameters";
            // 
            // tcEnergyParameters
            // 
            this.tcEnergyParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tcEnergyParameters.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcEnergyParameters.Location = new System.Drawing.Point(339, 136);
            this.tcEnergyParameters.MultiSelect = true;
            this.tcEnergyParameters.Name = "tcEnergyParameters";
            this.tcEnergyParameters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
            this.tcEnergyParameters.Size = new System.Drawing.Size(426, 403);
            this.tcEnergyParameters.TabIndex = 6;
            // 
            // FrmVariableDefinitions
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(777, 580);
            this.Controls.Add(this.cmbxModelVariable);
            this.Controls.Add(this.tcEnergyParameters);
            this.Controls.Add(this.tcEndmembers);
            this.Controls.Add(this.cklbxCations);
            this.Controls.Add(this.cklbxOxides);
            this.Controls.Add(this.cmbxPhase);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowseAndRead);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPhaseFullName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFilePath);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmVariableDefinitions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Variable Definitions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseAndRead;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbxPhase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox cklbxOxides;
        private System.Windows.Forms.Label lblPhaseFullName;
        private System.Windows.Forms.CheckedListBox cklbxCations;
        private CustomControls.TableControl tcEndmembers;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.ComboBox cmbxModelVariable;
        private System.Windows.Forms.Label label4;
        private CustomControls.TableControl tcEnergyParameters;
    }
}