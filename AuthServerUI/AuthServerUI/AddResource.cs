using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Windows.Forms;
using PeterO.Cbor;

namespace AuthServerUI
{
    public partial class AddResource : Form
    {
        private readonly MyDatabase _dataBase;
        private readonly int _resourceId;
        private List<int> oldHosts;

        public AddResource(MyDatabase db, int resourceNo = -1)
        {
            _dataBase = db;
            InitializeComponent();
            _resourceId = resourceNo;
        }

        private void AddResource_Load(object sender, EventArgs e)
        {
            SqlDataReader reader = null;
            try
            {
                string txt = "SELECT EntityNo, Name FROM EntityTable WHERE Type='RS';";
                SqlCommand cmd = new SqlCommand(txt, _dataBase.Connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows) {
                    reader.Close();
                    return;
                }

                while (reader.Read()) {
                    ResourceItem r = new ResourceItem(reader.GetInt32(0), reader.GetString(1));
                    cbHostedOn.Items.Add(r);
                }
                reader.Close();

                if (_resourceId != -1) {
                    cmd = new SqlCommand($"SELECT * FROM ResourceTable WHERE ResourceId = {_resourceId};", _dataBase.Connection);
                    reader = cmd.ExecuteReader();

                    if (!reader.HasRows) {
                        reader.Close();
                        MessageBox.Show("Internal error - no such resource number", "AuthUI", MessageBoxButtons.OK);
                        return;
                    }

                    reader.Read();

                    tbName.Text = reader.GetString(reader.GetOrdinal("Name"));
                    tbAudience.Text = reader.GetString(reader.GetOrdinal("Audience"));
                    int ord = reader.GetOrdinal("Comment");
                    if (!reader.IsDBNull(ord)) {
                        tbComment.Text = reader.GetString(ord);
                    }

                    tbRules.Text = CBORObject.DecodeFromBytes(reader.GetSqlBinary(reader.GetOrdinal("Rules")).Value).ToString();

                    reader.Close();

                    cmd.CommandText = $"SELECT * FROM ResourceMap WHERE ResourceId = {_resourceId};";
                    reader = cmd.ExecuteReader();
                    oldHosts = new List<int>();
                    if (reader.HasRows) {
                        ord = reader.GetOrdinal("EntityId");
                        while (reader.NextResult()) {
                            int eId = reader.GetInt32(ord);
                            for (int i= 0; i<cbHostedOn.Items.Count; i++) {
                                if (((ResourceItem) cbHostedOn.Items[i]).EntityIndex == eId) {
                                    cbHostedOn.SetItemChecked(i, true);
                                    oldHosts.Add(eId);
                                    break;
                                }
                            }
                        }
                    }
                }

                reader.Close();
            }
            catch {
                reader?.Close();
            }

        }

        public void InsertInDatabase(MyDatabase database)
        {
            SqlTransaction transaction = database.Connection.BeginTransaction("Creation");
            try
            {
                string txt = $"INSERT INTO ResourceTable (Name, Rules, Audience, Comment) VALUES (@Name, @Rules, @Audience, @Comment)";
                SqlCommand command = new SqlCommand(txt, database.Connection);
                command.Transaction = transaction;

                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = tbName.Text;
                command.Parameters.Add("@Rules", SqlDbType.Binary).Value =  Encoding.UTF8.GetBytes(tbRules.Text);
                command.Parameters.Add("@Audience", SqlDbType.Text).Value = tbAudience.Text;
                command.Parameters.Add("@Comment", SqlDbType.Text).Value = tbComment.Text;
                int entityNo = (int) command.ExecuteNonQuery();

                txt = $"INSERT INTO ResourceMap (EntityId, ResourceId) VALUES (@EntityID, @ResourceID);";
                command = new SqlCommand(txt, database.Connection) {
                    Transaction = transaction
                };
                command.Parameters.Add("@ResourceID", SqlDbType.Int).Value = entityNo;
                command.Parameters.Add("@EntityID", SqlDbType.Int);

                for (int i = 0; i < cbHostedOn.CheckedItems.Count; i++) {
                    command.Parameters["@EntityID"].Value = ((ResourceItem) cbHostedOn.CheckedItems[i]).EntityIndex;
                    entityNo = (int)command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (SqlException e) {
                MessageBox.Show("Exception occured trying to fill Entity Table\n" + e.Message);
                transaction.Rollback();
            }
            catch {
                transaction.Rollback();
            }

        }

        public void UpdateDatabase()
        {

            SqlTransaction transaction = _dataBase.Connection.BeginTransaction();

            try {
                StringBuilder build = new StringBuilder("UPDATE ResourceTable SET ");
                SqlCommand cmd = new SqlCommand("", _dataBase.Connection, transaction);
                string addComma = "";

                if (tbName.Modified) {
                    build.Append("Name=@Name");
                    cmd.Parameters.Add("Name", SqlDbType.Text).Value = tbName.Text;
                    addComma = ", ";
                }

                if (tbAudience.Modified) {
                    build.Append(addComma);
                    build.Append("Audience=@Audience");
                    cmd.Parameters.Add("Audience", SqlDbType.Text).Value = tbAudience.Text;
                    addComma = ", ";
                }

                if (tbComment.Modified) {
                    build.Append(addComma);
                    build.Append("Comment=@Comment");
                    cmd.Parameters.Add("Comment", SqlDbType.Text).Value = tbComment.Text;
                    addComma = ", ";
                }

                if (tbRules.Modified) {
                    build.Append(addComma);
                    build.Append("Rules=@Rules");
                    cmd.Parameters.Add("Rules", SqlDbType.Binary).Value = tbRules.Text;
                    addComma = ", ";
                }

                build.Append($" WHERE ResourceId = {_resourceId};");
                cmd.CommandText = build.ToString();

                if (addComma != "") {
                    cmd.ExecuteNonQuery();
                }

                cmd = new SqlCommand {
                    Connection = _dataBase.Connection,
                    CommandText = "INSERT INTO ResourceMap (EntityId, ResourceId) VALUES (@EntityID, @ResourceID);",
                    Transaction = transaction
                };
                cmd.Parameters.Add("ResourceID", SqlDbType.Int).Value = _resourceId;
                cmd.Parameters.Add("EntityID", SqlDbType.Int);


                foreach (ResourceItem item in cbHostedOn.CheckedItems) {
                    if (oldHosts.Contains(item.EntityIndex)) {
                        oldHosts.Remove(item.EntityIndex);
                    }
                    else {
                        cmd.Parameters["EntityID"].Value = item.EntityIndex;
                        cmd.ExecuteNonQuery();
                    }
                }

                cmd.CommandText = "DELETE FROM resourceMap WHERE EntityId = @EntityID and ResourceID = @ResourceID;";
                foreach (int entityId in oldHosts) {
                    cmd.Parameters["EntityID"].Value = entityId;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

            }
            catch (SqlException e) {
                MessageBox.Show($"Exception occured trying to update Entity Table\n{e.Message}");
                transaction.Rollback();
            }
            catch {
                transaction.Rollback();
            }
        }

        private class ResourceItem
        {
            public int EntityIndex { get; }
            private readonly string entityName;
            public ResourceItem(int index, string name)
            {
                EntityIndex = index;
                entityName = name;
            }
            /// <inheritdoc />
            public override string ToString()
            {
                return entityName;
            }
        }
    }
}
