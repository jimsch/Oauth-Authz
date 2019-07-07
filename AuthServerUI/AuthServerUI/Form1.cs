using System;
using System.Drawing;
using System.Windows.Forms;

namespace AuthServerUI
{
    public partial class Form1 : Form
    {
        public MyDatabase _Database;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Database = new MyDatabase();
            if (!_Database.OpenMyDatabase()) {
                MessageBox.Show("Error trying to open the database");
                return;
            }

            listViewEntities_Load();
            resourceListView_Load();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MinimumSize = new Size(100, listViewEntities.Height+100);
        }

        private void Form1_Layout(object sender, LayoutEventArgs e)
        {
            Size formSize = this.ClientSize;
            lowerTabControl.Size = new Size(formSize.Width, formSize.Height - listViewEntities.Height);
        }

        private void topTabControl_Selected(object sender, TabControlEventArgs e)
        {
            listViewEntities_Selected(e.TabPageIndex == 0);
            resourceListView_Selected(e.TabPageIndex == 1);
        }

        private void entityNew_Click(object sender, EventArgs e)
        {
            AddEntity form = new AddEntity();
            if (form.ShowDialog() == DialogResult.OK) {
                //  Add data to database
                form.InsertInDatabase(_Database);
                listViewEntities_Load();
            }
        }

        private void addResource_click(object sender, EventArgs e)
        {
            AddResource res = new AddResource(_Database);
            if (res.ShowDialog() == DialogResult.OK) {
                res.InsertInDatabase(_Database);

            }

        }

        private void editResource_click(object sender, EventArgs e)
        {
            AddResource res = new AddResource(_Database, (int) resourceListView.SelectedItems[0].Tag);
            if (res.ShowDialog() == DialogResult.OK) {
                res.UpdateDatabase();
            }
        }

        private void resourcesMenu_click(object sender, EventArgs e)
        {
            bool f = topTabControl.SelectedIndex == 1; // Is the resource tab selected?
            f &= (resourceListView.Items.Count > 0); // And is there something that is selected?
            editResourceMenuItem.Enabled = f;
            disableResourceMenuItem.Enabled = f;
            deleteResourceMenuItem.Enabled = f;
        }

        private void keysMenu_Load(object sender, EventArgs e)
        {
            bool enable = topTabControl.SelectedIndex == 0;
            enable &= (listViewKeys.Items.Count > 0);
            editKeyMenuItem.Enabled = enable;
            disableKeyMenu.Enabled = enable;
            deleteKeyMenu.Enabled = enable;
        }

        private void editKeysMenu_click(object sender, EventArgs e)
        {
            AddKey dialog = new AddKey(_Database, -1, (int) listViewKeys.SelectedItems[0].Tag);

            if (dialog.ShowDialog() == DialogResult.OK) {
                dialog.UpdateDatabase();
            }
        }
    }
}
