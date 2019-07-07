using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PeterO.Cbor;
using SortOrder = System.Windows.Forms.SortOrder;

namespace AuthServerUI
{
    public partial class Form1 : Form
    {

#region functions for list of entities control - listViewEntities
        private void listViewEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEntities.SelectedItems.Count > 0) {
                int entityId = (int)listViewEntities.SelectedItems[0].Tag;
                tabControl1_ChangeEntity(entityId);
            }
        }

        private void listViewEntities_Load()
        {
            listViewEntities.Clear();

            listViewEntities.View = View.Details;
            listViewEntities.AllowColumnReorder = true;
            listViewEntities.FullRowSelect = true;
            listViewEntities.GridLines = true;
            listViewEntities.Sorting = SortOrder.Ascending;

            listViewEntities.Columns.Add("Entity Name", -1, HorizontalAlignment.Left);
            listViewEntities.Columns.Add("Entity Type", -1, HorizontalAlignment.Left);
            listViewEntities.Columns.Add("Profiles", -1, HorizontalAlignment.Left);
            listViewEntities.Columns.Add("Attributes", -1, HorizontalAlignment.Left);
            listViewEntities.Columns.Add("Comment", -2, HorizontalAlignment.Left);


            try {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand("SELECT * FROM EntityTable;", _Database.Connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows) {
                    reader.Close();
                    return;
                }


                while (reader.Read()) {
                    ListViewItem item1 = new ListViewItem(reader.GetString(1));

                    item1.Tag = reader.GetInt32(0);

                    // item1.SubItems.Add(); // EName
                    item1.SubItems.Add(reader.GetString(3)); // Type
                    item1.SubItems.Add(reader.IsDBNull(7) ? "" : reader.GetString(7));
                    item1.SubItems.Add(reader.IsDBNull(9) ? "" : reader.GetString(9));
                    item1.SubItems.Add(reader.IsDBNull(2) ? "" : reader.GetString(2));

                    listViewEntities.Items.Add(item1);
                }

                reader.Close();

                listViewEntities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listViewEntities.SelectedItems.Clear();
                if (listViewEntities.Items.Count > 0) {
                    listViewEntities.Items[0].Selected = true;
                    tabControl1_ChangeEntity((int)listViewEntities.Items[0].Tag);
                }
            }
            catch (SqlException e) {
                MessageBox.Show("Exception occured trying to fill Entity Table\n" + e.Message);
            }

        }


        private void listViewEntities_Selected(bool selected)
        {
            if (selected) {
                listViewKeys.Show();
            }
            else {
                listViewKeys.Hide();
            }

            if (selected) {
                lowerTabControl.SelectedIndex = 0;
            }
        }


        #endregion

        private void tabControl1_ChangeEntity(int entityId)
        {
            listViewKeys_NewEntity(entityId);
        }

#region Entity Key List - listeViewKeys
        private void listViewKeys_NewEntity(int entityId)
        {
            listViewKeys.Clear();

            listViewKeys.View = View.Details;
            listViewKeys.AllowColumnReorder = true;
            listViewKeys.FullRowSelect = true;
            listViewKeys.GridLines = true;
            listViewKeys.Sorting = SortOrder.Ascending;

            listViewKeys.Columns.Add("Key Name", -1, HorizontalAlignment.Left);
            listViewKeys.Columns.Add("Key", -1, HorizontalAlignment.Left);
            listViewKeys.Columns.Add("Connection", -1, HorizontalAlignment.Left);
            listViewKeys.Columns.Add("Audience", -1, HorizontalAlignment.Left);
            listViewKeys.Columns.Add("Scope", -2, HorizontalAlignment.Left);
            listViewKeys.Columns.Add("Comment", -2, HorizontalAlignment.Left);

            try {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand($"SELECT * FROM KeyTable WHERE EntityID = {entityId};",
                                                _Database.Connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows) {
                    reader.Close();
                    return;
                }

                while (reader.Read()) {
                    ListViewItem item1 = new ListViewItem(reader.GetString(1)); // Key Name

                    item1.Tag = reader.GetInt32(0); // Key ID
                    item1.SubItems.Add(CBORObject.DecodeFromBytes(reader.GetSqlBinary(5).Value).ToString());  // Key Value
                    item1.SubItems.Add(reader.GetString(3)); // AS Connection
                    item1.SubItems.Add(reader.IsDBNull(9) ? "" : reader.GetString(9));
                    item1.SubItems.Add(reader.IsDBNull(10) ? "" : CBORObject.DecodeFromBytes(reader.GetSqlBinary(10).Value).ToString());
                    item1.SubItems.Add(reader.IsDBNull(4) ? "" : reader.GetString(4));

                    listViewKeys.Items.Add(item1);
                }

                reader.Close();

                listViewKeys.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            catch (SqlException e) {
                MessageBox.Show("Exception occured trying to fill Entity Table\n" + e.Message);
            }
        }
        #endregion

#region KeyFunctions


        private void addKey_click(object sender, EventArgs e)
        {
            if (listViewEntities.SelectedItems.Count == 0) {
                return;
            }

            object o = listViewEntities.SelectedItems[0].Tag;

            AddKey addKey = new AddKey(_Database, (int) o);
            if (addKey.ShowDialog() == DialogResult.OK) {
                addKey.AddToDatabase(_Database, (int) o);
            }
        }
#endregion

    }
}