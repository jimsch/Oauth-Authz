using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PeterO.Cbor;
using SortOrder = System.Windows.Forms.SortOrder;

namespace AuthServerUI
{
    public partial class Form1 : Form
    {
        private void tabControl1_ChangeResource(int entityId)
        {
            resourceViewHosts_NewEntity(entityId);
        }


        // M00TODO - should write some type of comparer to sort by X, Audience, Resource name based on selected sorting from the user.
#region List control of resources - resourceListView

        private void resourceListView_Load()
        {
            resourceListView.Clear();

            resourceListView.View = View.Details;
            resourceListView.AllowColumnReorder = true;
            resourceListView.FullRowSelect = true;
            resourceListView.GridLines = true;
            resourceListView.Sorting = SortOrder.Ascending;

            resourceListView.Columns.Add("Audience", -1, HorizontalAlignment.Left);
            resourceListView.Columns.Add("Resource Name", -1, HorizontalAlignment.Left);
            resourceListView.Columns.Add("Rules", -1, HorizontalAlignment.Left);
            resourceListView.Columns.Add("Comments", -1, HorizontalAlignment.Left);


            try {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand("SELECT * FROM ResourceTable ORDER BY Audience, Name;", _Database.Connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows) {
                    reader.Close();
                    return;
                }


                while (reader.Read()) {
                    ListViewItem item1 = new ListViewItem(reader.GetString(1)) { // Audience
                        Tag = reader.GetInt32(0)
                    }; // Audience

                    // Resource ID

                    item1.SubItems.Add(reader.GetString(4)); // Resource Name
                    string rules = CBORObject.DecodeFromBytes(reader.GetSqlBinary(2).Value).ToString();
                    item1.SubItems.Add(rules);
                    item1.SubItems.Add(reader.IsDBNull(3) ? "" : reader.GetString(3)); // Comment

                    resourceListView.Items.Add(item1);
                }

                reader.Close();

                resourceListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                resourceListView.SelectedItems.Clear();
                if (resourceListView.Items.Count > 0) {
                    resourceListView.Items[0].Selected = true;
                    tabControl1_ChangeResource((int)resourceListView.Items[0].Tag);
                }
            }
            catch (SqlException e) {
                MessageBox.Show("Exception occured trying to fill Entity Table\n" + e.Message);
            }

        }

        private void resourceListView_Selected(bool selected)
        {
            if (selected) {
                resourceTab.Show();
            }
            else {
                listViewKeys.Hide();
            }

            if (selected) {
                lowerTabControl.SelectedIndex = 1;
            }
        }


        #endregion

        #region List of hosts that support a resource

        private void resourceViewHosts_NewEntity(int i)
        {

        }
#endregion

    }
}
