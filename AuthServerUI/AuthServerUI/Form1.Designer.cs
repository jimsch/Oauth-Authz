namespace AuthServerUI
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entityNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keysMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.addKeyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableKeyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteKeyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.resourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addResourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editResourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableResourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteResourceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.resourceTab = new System.Windows.Forms.TabPage();
            this.resourceListView = new System.Windows.Forms.ListView();
            this.tabPageEntities = new System.Windows.Forms.TabPage();
            this.listViewEntities = new System.Windows.Forms.ListView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.topTabControl = new System.Windows.Forms.TabControl();
            this.lowerTabControl = new System.Windows.Forms.TabControl();
            this.tabPageKeys = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.listViewKeys = new System.Windows.Forms.ListView();
            this.KeyName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.resourceHostsTab = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.resourceTab.SuspendLayout();
            this.tabPageEntities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.topTabControl.SuspendLayout();
            this.lowerTabControl.SuspendLayout();
            this.tabPageKeys.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.entityToolStripMenuItem,
            this.keysMenu,
            this.resourcesToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // entityToolStripMenuItem
            // 
            this.entityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.entityNewMenuItem,
            this.disableToolStripMenuItem});
            this.entityToolStripMenuItem.Name = "entityToolStripMenuItem";
            this.entityToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.entityToolStripMenuItem.Text = "Entity";
            // 
            // entityNewMenuItem
            // 
            this.entityNewMenuItem.Name = "entityNewMenuItem";
            this.entityNewMenuItem.Size = new System.Drawing.Size(112, 22);
            this.entityNewMenuItem.Text = "New";
            this.entityNewMenuItem.Click += new System.EventHandler(this.entityNew_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.disableToolStripMenuItem.Text = "Disable";
            // 
            // keysMenu
            // 
            this.keysMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addKeyMenu,
            this.editKeyMenuItem,
            this.disableKeyMenu,
            this.deleteKeyMenu});
            this.keysMenu.Name = "keysMenu";
            this.keysMenu.Size = new System.Drawing.Size(43, 20);
            this.keysMenu.Text = "Keys";
            this.keysMenu.Click += new System.EventHandler(this.keysMenu_Load);
            // 
            // addKeyMenu
            // 
            this.addKeyMenu.Name = "addKeyMenu";
            this.addKeyMenu.Size = new System.Drawing.Size(180, 22);
            this.addKeyMenu.Text = "Add";
            this.addKeyMenu.Click += new System.EventHandler(this.addKey_click);
            // 
            // editKeyMenuItem
            // 
            this.editKeyMenuItem.Name = "editKeyMenuItem";
            this.editKeyMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editKeyMenuItem.Text = "Edit";
            this.editKeyMenuItem.Click += new System.EventHandler(this.editKeysMenu_click);
            // 
            // disableKeyMenu
            // 
            this.disableKeyMenu.Name = "disableKeyMenu";
            this.disableKeyMenu.Size = new System.Drawing.Size(180, 22);
            this.disableKeyMenu.Text = "Disable";
            // 
            // deleteKeyMenu
            // 
            this.deleteKeyMenu.Name = "deleteKeyMenu";
            this.deleteKeyMenu.Size = new System.Drawing.Size(180, 22);
            this.deleteKeyMenu.Text = "Delete";
            // 
            // resourcesToolStripMenuItem
            // 
            this.resourcesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addResourceMenuItem,
            this.editResourceMenuItem,
            this.disableResourceMenuItem,
            this.deleteResourceMenuItem});
            this.resourcesToolStripMenuItem.Name = "resourcesToolStripMenuItem";
            this.resourcesToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.resourcesToolStripMenuItem.Text = "Resources";
            this.resourcesToolStripMenuItem.Click += new System.EventHandler(this.resourcesMenu_click);
            // 
            // addResourceMenuItem
            // 
            this.addResourceMenuItem.Name = "addResourceMenuItem";
            this.addResourceMenuItem.Size = new System.Drawing.Size(112, 22);
            this.addResourceMenuItem.Text = "Add";
            this.addResourceMenuItem.Click += new System.EventHandler(this.addResource_click);
            // 
            // editResourceMenuItem
            // 
            this.editResourceMenuItem.Name = "editResourceMenuItem";
            this.editResourceMenuItem.Size = new System.Drawing.Size(112, 22);
            this.editResourceMenuItem.Text = "Edit";
            this.editResourceMenuItem.Click += new System.EventHandler(this.editResource_click);
            // 
            // disableResourceMenuItem
            // 
            this.disableResourceMenuItem.Name = "disableResourceMenuItem";
            this.disableResourceMenuItem.Size = new System.Drawing.Size(112, 22);
            this.disableResourceMenuItem.Text = "Disable";
            // 
            // deleteResourceMenuItem
            // 
            this.deleteResourceMenuItem.Name = "deleteResourceMenuItem";
            this.deleteResourceMenuItem.Size = new System.Drawing.Size(112, 22);
            this.deleteResourceMenuItem.Text = "Delete";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(699, 402);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(588, 402);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // resourceTab
            // 
            this.resourceTab.Controls.Add(this.resourceListView);
            this.resourceTab.Location = new System.Drawing.Point(4, 22);
            this.resourceTab.Name = "resourceTab";
            this.resourceTab.Padding = new System.Windows.Forms.Padding(3);
            this.resourceTab.Size = new System.Drawing.Size(792, 158);
            this.resourceTab.TabIndex = 1;
            this.resourceTab.Text = "Resources";
            this.resourceTab.UseVisualStyleBackColor = true;
            // 
            // resourceListView
            // 
            this.resourceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resourceListView.Location = new System.Drawing.Point(3, 3);
            this.resourceListView.Name = "resourceListView";
            this.resourceListView.Size = new System.Drawing.Size(786, 152);
            this.resourceListView.TabIndex = 0;
            this.resourceListView.UseCompatibleStateImageBehavior = false;
            // 
            // tabPageEntities
            // 
            this.tabPageEntities.Controls.Add(this.listViewEntities);
            this.tabPageEntities.Location = new System.Drawing.Point(4, 22);
            this.tabPageEntities.Name = "tabPageEntities";
            this.tabPageEntities.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEntities.Size = new System.Drawing.Size(792, 158);
            this.tabPageEntities.TabIndex = 0;
            this.tabPageEntities.Text = "Entities";
            this.tabPageEntities.UseVisualStyleBackColor = true;
            // 
            // listViewEntities
            // 
            this.listViewEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewEntities.Location = new System.Drawing.Point(3, 3);
            this.listViewEntities.MultiSelect = false;
            this.listViewEntities.Name = "listViewEntities";
            this.listViewEntities.Size = new System.Drawing.Size(786, 152);
            this.listViewEntities.TabIndex = 2;
            this.listViewEntities.UseCompatibleStateImageBehavior = false;
            this.listViewEntities.SelectedIndexChanged += new System.EventHandler(this.listViewEntities_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.topTabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lowerTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(800, 369);
            this.splitContainer1.SplitterDistance = 184;
            this.splitContainer1.TabIndex = 8;
            // 
            // topTabControl
            // 
            this.topTabControl.Controls.Add(this.tabPageEntities);
            this.topTabControl.Controls.Add(this.resourceTab);
            this.topTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topTabControl.Location = new System.Drawing.Point(0, 0);
            this.topTabControl.Name = "topTabControl";
            this.topTabControl.SelectedIndex = 0;
            this.topTabControl.Size = new System.Drawing.Size(800, 184);
            this.topTabControl.TabIndex = 5;
            this.topTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.topTabControl_Selected);
            // 
            // lowerTabControl
            // 
            this.lowerTabControl.Controls.Add(this.tabPageKeys);
            this.lowerTabControl.Controls.Add(this.resourceHostsTab);
            this.lowerTabControl.Controls.Add(this.tabPage3);
            this.lowerTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lowerTabControl.Location = new System.Drawing.Point(0, 0);
            this.lowerTabControl.Name = "lowerTabControl";
            this.lowerTabControl.SelectedIndex = 0;
            this.lowerTabControl.Size = new System.Drawing.Size(800, 181);
            this.lowerTabControl.TabIndex = 3;
            // 
            // tabPageKeys
            // 
            this.tabPageKeys.Controls.Add(this.button1);
            this.tabPageKeys.Controls.Add(this.listViewKeys);
            this.tabPageKeys.Location = new System.Drawing.Point(4, 22);
            this.tabPageKeys.Name = "tabPageKeys";
            this.tabPageKeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKeys.Size = new System.Drawing.Size(792, 155);
            this.tabPageKeys.TabIndex = 0;
            this.tabPageKeys.Text = "Keys";
            this.tabPageKeys.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(594, 184);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // listViewKeys
            // 
            this.listViewKeys.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.KeyName,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listViewKeys.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewKeys.Location = new System.Drawing.Point(3, 3);
            this.listViewKeys.Name = "listViewKeys";
            this.listViewKeys.Size = new System.Drawing.Size(786, 164);
            this.listViewKeys.TabIndex = 0;
            this.listViewKeys.UseCompatibleStateImageBehavior = false;
            // 
            // KeyName
            // 
            this.KeyName.Text = "Key Name";
            // 
            // resourceHostsTab
            // 
            this.resourceHostsTab.Location = new System.Drawing.Point(4, 22);
            this.resourceHostsTab.Name = "resourceHostsTab";
            this.resourceHostsTab.Padding = new System.Windows.Forms.Padding(3);
            this.resourceHostsTab.Size = new System.Drawing.Size(792, 155);
            this.resourceHostsTab.TabIndex = 1;
            this.resourceHostsTab.Text = "Hosts";
            this.resourceHostsTab.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 155);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Form1_Layout);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.resourceTab.ResumeLayout(false);
            this.tabPageEntities.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.topTabControl.ResumeLayout(false);
            this.lowerTabControl.ResumeLayout(false);
            this.tabPageKeys.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TabPage resourceTab;
        private System.Windows.Forms.TabPage tabPageEntities;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl lowerTabControl;
        private System.Windows.Forms.TabPage tabPageKeys;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listViewKeys;
        private System.Windows.Forms.ColumnHeader KeyName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TabPage resourceHostsTab;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listViewEntities;
        private System.Windows.Forms.TabControl topTabControl;
        private System.Windows.Forms.ListView resourceListView;
        private System.Windows.Forms.ToolStripMenuItem entityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entityNewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keysMenu;
        private System.Windows.Forms.ToolStripMenuItem addKeyMenu;
        private System.Windows.Forms.ToolStripMenuItem disableKeyMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteKeyMenu;
        private System.Windows.Forms.ToolStripMenuItem resourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addResourceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableResourceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteResourceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editResourceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editKeyMenuItem;
    }
}

