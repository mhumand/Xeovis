namespace DatabaseReader
{
    partial class FrmMain
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
            this.btnDataManager = new System.Windows.Forms.Button();
            this.btnReadMctc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDataManager
            // 
            this.btnDataManager.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDataManager.Location = new System.Drawing.Point(30, 46);
            this.btnDataManager.Name = "btnDataManager";
            this.btnDataManager.Size = new System.Drawing.Size(283, 84);
            this.btnDataManager.TabIndex = 0;
            this.btnDataManager.Text = "Database Reader";
            this.btnDataManager.UseVisualStyleBackColor = true;
            this.btnDataManager.Click += new System.EventHandler(this.btnDataManager_Click);
            // 
            // btnReadMctc
            // 
            this.btnReadMctc.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadMctc.Location = new System.Drawing.Point(30, 145);
            this.btnReadMctc.Name = "btnReadMctc";
            this.btnReadMctc.Size = new System.Drawing.Size(283, 84);
            this.btnReadMctc.TabIndex = 0;
            this.btnReadMctc.Text = "Read Mctc";
            this.btnReadMctc.UseVisualStyleBackColor = true;
            this.btnReadMctc.Click += new System.EventHandler(this.btnReadMctc_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 405);
            this.Controls.Add(this.btnReadMctc);
            this.Controls.Add(this.btnDataManager);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xEOS Toolbox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDataManager;
        private System.Windows.Forms.Button btnReadMctc;
    }
}