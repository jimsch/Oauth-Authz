namespace AuthServerUI
{
    partial class AddKey
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
            if (disposing && (components != null)) {
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbKeyName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbAsConnection = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbKeyValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDefaultAudience = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbDefaultScope = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Key Name";
            // 
            // tbKeyName
            // 
            this.tbKeyName.Location = new System.Drawing.Point(136, 43);
            this.tbKeyName.Name = "tbKeyName";
            this.tbKeyName.Size = new System.Drawing.Size(195, 20);
            this.tbKeyName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "AS Connection:";
            // 
            // cbAsConnection
            // 
            this.cbAsConnection.FormattingEnabled = true;
            this.cbAsConnection.Items.AddRange(new object[] {
            "DTLS",
            "OSCORE",
            "CWT"});
            this.cbAsConnection.Location = new System.Drawing.Point(136, 120);
            this.cbAsConnection.Name = "cbAsConnection";
            this.cbAsConnection.Size = new System.Drawing.Size(195, 21);
            this.cbAsConnection.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 302);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Comment:";
            // 
            // tbComment
            // 
            this.tbComment.AcceptsReturn = true;
            this.tbComment.Location = new System.Drawing.Point(136, 302);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbComment.Size = new System.Drawing.Size(554, 117);
            this.tbComment.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Key Value:";
            // 
            // tbKeyValue
            // 
            this.tbKeyValue.AcceptsReturn = true;
            this.tbKeyValue.Location = new System.Drawing.Point(136, 166);
            this.tbKeyValue.Multiline = true;
            this.tbKeyValue.Name = "tbKeyValue";
            this.tbKeyValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbKeyValue.Size = new System.Drawing.Size(554, 119);
            this.tbKeyValue.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(421, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Default Audience:";
            // 
            // tbDefaultAudience
            // 
            this.tbDefaultAudience.Location = new System.Drawing.Point(533, 73);
            this.tbDefaultAudience.Name = "tbDefaultAudience";
            this.tbDefaultAudience.Size = new System.Drawing.Size(157, 20);
            this.tbDefaultAudience.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(421, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Default Scope:";
            // 
            // tbDefaultScope
            // 
            this.tbDefaultScope.Location = new System.Drawing.Point(533, 121);
            this.tbDefaultScope.Name = "tbDefaultScope";
            this.tbDefaultScope.Size = new System.Drawing.Size(157, 20);
            this.tbDefaultScope.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(615, 432);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(711, 432);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(713, 166);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 16;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // AddKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 467);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbDefaultScope);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbDefaultAudience);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbKeyValue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbAsConnection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbKeyName);
            this.Controls.Add(this.label1);
            this.Name = "AddKey";
            this.Text = "AddKey";
            this.Load += new System.EventHandler(this.addKey_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbKeyName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbAsConnection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbKeyValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbDefaultAudience;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbDefaultScope;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnGenerate;
    }
}