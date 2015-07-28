namespace LiveSplit.Quake2
{
    partial class Settings
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstUsedEvents = new System.Windows.Forms.ListBox();
            this.lstAvailEvents = new System.Windows.Forms.ListBox();
            this.lblUsedEvents = new System.Windows.Forms.Label();
            this.btnAllEvents = new System.Windows.Forms.Button();
            this.btnNoEvents = new System.Windows.Forms.Button();
            this.btnAddEvent = new System.Windows.Forms.Button();
            this.btnRemoveEvent = new System.Windows.Forms.Button();
            this.lblAvailEvents = new System.Windows.Forms.Label();
            this.chkPauseGameTime = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lstUsedEvents
            // 
            this.lstUsedEvents.FormattingEnabled = true;
            this.lstUsedEvents.Location = new System.Drawing.Point(13, 23);
            this.lstUsedEvents.Name = "lstUsedEvents";
            this.lstUsedEvents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstUsedEvents.Size = new System.Drawing.Size(196, 160);
            this.lstUsedEvents.TabIndex = 3;
            // 
            // lstAvailEvents
            // 
            this.lstAvailEvents.FormattingEnabled = true;
            this.lstAvailEvents.Location = new System.Drawing.Point(254, 23);
            this.lstAvailEvents.Name = "lstAvailEvents";
            this.lstAvailEvents.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAvailEvents.Size = new System.Drawing.Size(195, 160);
            this.lstAvailEvents.TabIndex = 4;
            // 
            // lblUsedEvents
            // 
            this.lblUsedEvents.AutoSize = true;
            this.lblUsedEvents.Location = new System.Drawing.Point(10, 7);
            this.lblUsedEvents.Name = "lblUsedEvents";
            this.lblUsedEvents.Size = new System.Drawing.Size(172, 13);
            this.lblUsedEvents.TabIndex = 5;
            this.lblUsedEvents.Text = "Split on these events (in this order):";
            // 
            // btnAllEvents
            // 
            this.btnAllEvents.Location = new System.Drawing.Point(215, 131);
            this.btnAllEvents.Name = "btnAllEvents";
            this.btnAllEvents.Size = new System.Drawing.Size(33, 23);
            this.btnAllEvents.TabIndex = 9;
            this.btnAllEvents.Text = "<<";
            this.btnAllEvents.UseVisualStyleBackColor = true;
            this.btnAllEvents.Click += new System.EventHandler(this.btnAllEvents_Click);
            // 
            // btnNoEvents
            // 
            this.btnNoEvents.Location = new System.Drawing.Point(215, 160);
            this.btnNoEvents.Name = "btnNoEvents";
            this.btnNoEvents.Size = new System.Drawing.Size(33, 23);
            this.btnNoEvents.TabIndex = 10;
            this.btnNoEvents.Text = ">>";
            this.btnNoEvents.UseVisualStyleBackColor = true;
            this.btnNoEvents.Click += new System.EventHandler(this.btnNoEvents_Click);
            // 
            // btnAddEvent
            // 
            this.btnAddEvent.Location = new System.Drawing.Point(215, 23);
            this.btnAddEvent.Name = "btnAddEvent";
            this.btnAddEvent.Size = new System.Drawing.Size(33, 23);
            this.btnAddEvent.TabIndex = 11;
            this.btnAddEvent.Text = "<";
            this.btnAddEvent.UseVisualStyleBackColor = true;
            this.btnAddEvent.Click += new System.EventHandler(this.btnAddEvent_Click);
            // 
            // btnRemoveEvent
            // 
            this.btnRemoveEvent.Location = new System.Drawing.Point(215, 52);
            this.btnRemoveEvent.Name = "btnRemoveEvent";
            this.btnRemoveEvent.Size = new System.Drawing.Size(33, 23);
            this.btnRemoveEvent.TabIndex = 12;
            this.btnRemoveEvent.Text = ">";
            this.btnRemoveEvent.UseVisualStyleBackColor = true;
            this.btnRemoveEvent.Click += new System.EventHandler(this.btnRemoveEvent_Click);
            // 
            // lblAvailEvents
            // 
            this.lblAvailEvents.AutoSize = true;
            this.lblAvailEvents.Location = new System.Drawing.Point(251, 7);
            this.lblAvailEvents.Name = "lblAvailEvents";
            this.lblAvailEvents.Size = new System.Drawing.Size(88, 13);
            this.lblAvailEvents.TabIndex = 13;
            this.lblAvailEvents.Text = "Available events:";
            // 
            // chkPauseGameTime
            // 
            this.chkPauseGameTime.AutoSize = true;
            this.chkPauseGameTime.Location = new System.Drawing.Point(13, 198);
            this.chkPauseGameTime.Name = "chkPauseGameTime";
            this.chkPauseGameTime.Size = new System.Drawing.Size(173, 17);
            this.chkPauseGameTime.TabIndex = 14;
            this.chkPauseGameTime.Text = "Pause game time when loading";
            this.chkPauseGameTime.UseVisualStyleBackColor = true;
            this.chkPauseGameTime.CheckedChanged += new System.EventHandler(this.chkPauseGameTime_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkPauseGameTime);
            this.Controls.Add(this.lblAvailEvents);
            this.Controls.Add(this.btnRemoveEvent);
            this.Controls.Add(this.btnAddEvent);
            this.Controls.Add(this.btnNoEvents);
            this.Controls.Add(this.btnAllEvents);
            this.Controls.Add(this.lblUsedEvents);
            this.Controls.Add(this.lstAvailEvents);
            this.Controls.Add(this.lstUsedEvents);
            this.Name = "Settings";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Size = new System.Drawing.Size(459, 241);
            this.HandleDestroyed += new System.EventHandler(this.settings_HandleDestroyed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstUsedEvents;
        private System.Windows.Forms.ListBox lstAvailEvents;
        private System.Windows.Forms.Label lblUsedEvents;
        private System.Windows.Forms.Button btnAllEvents;
        private System.Windows.Forms.Button btnNoEvents;
        private System.Windows.Forms.Button btnAddEvent;
        private System.Windows.Forms.Button btnRemoveEvent;
        private System.Windows.Forms.Label lblAvailEvents;
        private System.Windows.Forms.CheckBox chkPauseGameTime;
    }
}
